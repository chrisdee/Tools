param([Parameter(Mandatory=$true,ValueFromPipeline=$true)]$url, [Parameter(ValueFromPipeline=$true)][string]$username, [Parameter(ValueFromPipeline=$true)][string]$password, [ValidateSet('SPS-Birthday','Department')][System.String]$changeProperty="Department" )
# Re-index SPO user profiles script
# Author: Mikael Svenson - @mikaelsvenson
# Blog: http://techmikael.blogspot.com


function Reset-UserProfiles( $siteUrl )
{
	$clientContext = New-Object Microsoft.SharePoint.Client.ClientContext($siteUrl)
	$clientContext.Credentials = $credentials 
	 
	if (!$clientContext.ServerObjectIsNull.Value) 
	{ 
		Write-Host "Connected to SharePoint Online site: '$siteUrl'" -ForegroundColor Green 
	} 
	$start = 0
	$rowLimit = 100
	do
	{
		$query = new-object Microsoft.SharePoint.Client.Search.Query.KeywordQuery($clientContext)
		$query.QueryText="*"
		$query.SourceId= [Guid]"b09a7990-05ea-4af9-81ef-edfab16c4e31"
		$query.StartRow=$start
		$query.RowLimit=$rowLimit 
		$query.SelectProperties.Add("accountname")
		$query.SelectProperties.Add("write")
		$query.SelectProperties.Add("crawltime")
		$query.SelectProperties.Add("PreferredName")
		$query.TrimDuplicates = $false
		$executor = new-object Microsoft.SharePoint.Client.Search.Query.SearchExecutor($clientContext)
		$result = $executor.ExecuteQuery($query)
		$clientContext.ExecuteQuery()

		$currentCount = 0
		if ($result.Value -ne $null)
		{			
			$currentCount = $result.Value.ResultRows.Length
			Write-Host "Iterating $currentCount profiles" -ForegroundColor Green
			$start = ($start + $rowLimit)
			foreach ($dictionary in $result.Value.ResultRows)
			{
				Write-Host $dictionary["accountname"] "Saved:" $dictionary["write"] "Indexed:" $dictionary["crawltime"] -ForegroundColor Cyan
				$pm = New-Object Microsoft.SharePoint.Client.UserProfiles.PeopleManager($clientContext)
				$props = $pm.GetPropertiesFor($dictionary["accountname"])
				$clientContext.Load($props)
				$clientContext.ExecuteQuery()
				
				if( $changeProperty -eq "SPS-Birthday" ) {	
					$birthday = $props.UserProfileProperties["SPS-Birthday"]
					if( $birthday -eq $null) {
						Write-Host "`tSkipping as user doesn't have the SPS-Birthday field" -ForegroundColor Yellow
						continue
					}

					# Force save by setting a random birthday value
					$pm.SetSingleValueProfileProperty($props.AccountName, "SPS-Birthday",  [DateTime]::Now.ToString("yyyyMMddHHmmss.0Z"))
					$clientContext.ExecuteQuery()

					if( $birthday -eq "" ) {
						Write-Host "`tKeeping birthday as not defined" -ForegroundColor Green
						$pm.SetSingleValueProfileProperty($props.AccountName, "SPS-Birthday",  [String]::Empty)
					} else {
						$oldDate = [DateTime]::Parse($birthday)
						Write-Host "`tRe-setting birthday to" $oldDate -ForegroundColor Green	
						$pm.SetSingleValueProfileProperty($props.AccountName, "SPS-Birthday",  $oldDate)
					}
				}
				if( $changeProperty -eq "Department" ) {
					$oldDepartment = $props.UserProfileProperties["Department"]
					if( $oldDepartment -eq $null) {
						Write-Host "`tSkipping as user doesn't have the Department field" -ForegroundColor Yellow
						continue
					}
					$pm.SetSingleValueProfileProperty($props.AccountName, "Department",  "mAdcOW reindex placeholder")
					$clientContext.ExecuteQuery()
					Write-Host "`tRe-setting Department to" $oldDepartment -ForegroundColor Green
					$pm.SetSingleValueProfileProperty($props.AccountName, "Department",  $oldDepartment)
				}
				$clientContext.ExecuteQuery()
			}
		}
	}
	while ($currentCount -eq $rowLimit)	
}

# promo
Write-Host "Check your browser to get the store app" -ForegroundColor Yellow
Start-Process -FilePath https://store.office.com/en-us/sharepoint-online-search-toolbox-by-puzzlepart-WA104380514.aspx

# change to the path of your CSOM dlls and add their types
$csomPath = "c:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\ISAPI"
Add-Type -Path "$csomPath\Microsoft.SharePoint.Client.dll" 
Add-Type -Path "$csomPath\Microsoft.SharePoint.Client.Runtime.dll" 
Add-Type -Path "$csomPath\Microsoft.SharePoint.Client.Search.dll" 
Add-Type -Path "$csomPath\Microsoft.SharePoint.Client.UserProfiles.dll" 

if([String]::IsNullOrWhiteSpace($username)) {
	$username = Read-host "What's your username?"
}

if([String]::IsNullOrWhiteSpace($password)) {
	$securePassword = Read-host "What's your password?" -AsSecureString 
} else {
	$securePassword = ConvertTo-SecureString $password -AsPlainText -Force 
}


$credentials = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($username, $securePassword) 
if( $url.tolower() -notlike '*-admin*') {
	Write-Host "This script has to be executed against the admin site of SPO. Eg. https://tenant-admin.sharepoint.com" -ForegroundColor Yellow
	return;
} else {
	Reset-UserProfiles -siteUrl $url
}
