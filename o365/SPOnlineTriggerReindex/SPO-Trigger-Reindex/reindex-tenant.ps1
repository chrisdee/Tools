param([Parameter(Mandatory=$true,ValueFromPipeline=$true)]$url, [Parameter(ValueFromPipeline=$true)][string]$username, [Parameter(ValueFromPipeline=$true)][string]$password, [ValidateSet('skip','on','off')][System.String]$enableAllManagedProperties="skip"  )
# Re-index SPO tenant script, and enable ManagedProperties managed property
# Author: Mikael Svenson - @mikaelsvenson
# Blog: http://techmikael.blogspot.com

# Modified by Eric Skaggs on 10/21/2014 - had trouble running this script as it was; functionality has not been changed

function Reset-Webs( $siteUrl ) 
{
	$clientContext = New-Object Microsoft.SharePoint.Client.ClientContext($siteUrl)
	$clientContext.Credentials = $credentials 
	 
	if (!$clientContext.ServerObjectIsNull.Value) 
	{ 
		Write-Host "Connected to SharePoint Online site: '$siteUrl'" -ForegroundColor Green 
	} 

	$rootWeb = $clientContext.Web
	processWeb($rootWeb)	
}

function processWeb($web)
{
	$subWebs = $web.Webs
	$clientContext.Load($web)		
	$clientContext.Load($web.AllProperties)
	$clientContext.Load($subWebs)
	$clientContext.ExecuteQuery()
	
	Write-Host "Web URL:" $web.Url -ForegroundColor White
	if( $enableAllManagedProperties -ne "skip" ) {
		Set-AllManagedProperties -web $web -clientContext $clientContext -enableAllManagedProps $enableAllManagedProperties
	}
	
	[int]$version = 0
	$allProperties = $web.AllProperties
	if( $allProperties.FieldValues.ContainsKey("vti_searchversion") -eq $true ) {
		$version = $allProperties["vti_searchversion"]
	}
	$version++
	$allProperties["vti_searchversion"] = $version
	Write-Host "-- Updated search version: " $version -ForegroundColor Green
	$web.Update()
	$clientContext.ExecuteQuery()
	
	# No need to process subwebs if we only mark site collection for indexing
	if($enableAllManagedProperties -ne "skip") {
		foreach ($subWeb in $subWebs)
		{
			processWeb($subWeb)
		}
	}
}

function Set-AllManagedProperties( $web, $clientContext, $enableAllManagedProps )
{
	$lists = $web.Lists
	$clientContext.Load($lists)
    $clientContext.ExecuteQuery()
 
    foreach ($list in $lists)
    {
        Write-Host "--" $list.Title
		
		if( $list.NoCrawl ) {
			Write-Host "--  Skipping list due to not being crawled" -ForegroundColor Yellow
			continue
		}
 
		$skip = $false;
		$eventReceivers = $list.EventReceivers
		$clientContext.Load($eventReceivers)
		$clientContext.ExecuteQuery()
		
		foreach( $eventReceiver in $eventReceivers )
		{
			if( $eventReceiver.ReceiverClass -eq "Microsoft.SharePoint.Publishing.CatalogEventReceiver" ) 
			{
				$skip = $true
				Write-Host "--  Skipping list as it's published as a catalog" -ForegroundColor Yellow
				break
			}
		}
		if( $skip ) {continue}
 
		$folder = $list.RootFolder
		$props = $folder.Properties
		$clientContext.Load($folder)		
		$clientContext.Load($props)
		$clientContext.ExecuteQuery()
		
		if( $enableAllManagedProps -eq "on" ) {
			Write-Host "--  Enabling all managed properties" -ForegroundColor Green
			$props["vti_indexedpropertykeys"] = "UAB1AGIAbABpAHMAaABpAG4AZwBDAGEAdABhAGwAbwBnAFMAZQB0AHQAaQBuAGcAcwA=|SQBzAFAAdQBiAGwAaQBzAGgAaQBuAGcAQwBhAHQAYQBsAG8AZwA=|"
			$props["IsPublishingCatalog"] = "True"
		}
		if( $enableAllManagedProps -eq "off" ) {
			Write-Host "--  Disabling all managed properties" -ForegroundColor Green
			$props["vti_indexedpropertykeys"] = $null
			$props["IsPublishingCatalog"] = $null
		}
		$folder.Update()
		$clientContext.ExecuteQuery()		
    }
}

# promo
Write-Host "Check your browser to get the store app" -ForegroundColor Yellow
Start-Process -FilePath https://store.office.com/en-us/sharepoint-online-search-toolbox-by-puzzlepart-WA104380514.aspx

Import-Module Microsoft.Online.SharePoint.PowerShell
# change to the path of your CSOM dlls and add their types
$csomPath = "c:\Program Files\Common Files\microsoft shared\Web Server Extensions\15\ISAPI"
Add-Type -Path "$csomPath\Microsoft.SharePoint.Client.dll" 
Add-Type -Path "$csomPath\Microsoft.SharePoint.Client.Runtime.dll" 

if([String]::IsNullOrWhiteSpace($username)) {
	$username = Read-host "What's your username?"
}

if([String]::IsNullOrWhiteSpace($password)) {
	$securePassword = Read-host "What's your password?" -AsSecureString 
} else {
	$securePassword = ConvertTo-SecureString $password -AsPlainText -Force 
}

$credentials = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($username, $securePassword) 
$spoCredentials = New-Object System.Management.Automation.PSCredential($username, $securePassword)
Connect-SPOService -Url $url -Credential $spoCredentials
Get-SPOSite | foreach {Reset-Webs -siteUrl $_.Url }
