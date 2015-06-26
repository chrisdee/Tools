# Script to Setup a new password encrypted string to put it into a script file
#Copyright Microsoft @ 2012

#DISCLAIMER

#The sample scripts are not supported under any Microsoft standard support program or service. 

#The sample scripts are provided AS IS without warranty of any kind. 

#Microsoft further disclaims all implied warranties including, without limitation, 

#any implied warranties of merchantability or of fitness for a particular purpose. 

#The entire risk arising out of the use or performance of the sample scripts and documentation remains with you. 

#In no event shall Microsoft, its authors, or anyone else involved in the creation, production, 

#or delivery of the scripts be liable for any damages whatsoever (including, without limitation, 

#damages for loss of business profits, business interruption, loss of business information, 

#or other pecuniary loss) arising out of the use of or inability to use the sample scripts or documentation, 

#even if Microsoft has been advised of the possibility of such damages.

Clear

$AssignmentScriptTemplate = "AssignLicense.tmp"

$AssignmentScriptName="AssignLicense.ps1"

$ReportScriptTemplate = "Get-MSOLUserLicensingReport.tmp"

$ReportScriptName="Get-MSOLUserLicensingReport.ps1"

$ADScriptTemplate="Get-LicensingInputFromAD.tmp"

$ADScriptName="Get-LicensingInputFromAD.ps1"

$script:userName=""

$script:pass=""

$host.ui.RawUI.ForegroundColor = "DarkYellow"

$host.ui.RawUI.BackgroundColor = "Black"

$FilterDefault="*"

$existingSKUInfo=$null

$userLicenseInfo="employeeType"

###added 20140515-timbos

$DefaultUsageLocation = "GR"

###

function getCredentials{

write-host 'Please enter your Office365 tenant admin username.'

$script:userName=Read-Host -Prompt "UserName:"

write-host 'Please enter your Office365 tenant admin password to secure it for use in the license assignment script.'

$sec=Read-Host -Prompt "Enter the Password:" -AsSecureString

$script:pass=ConvertFrom-SecureString $sec

# create a credential object and try to connect to Office 365

$p=ConvertTo-SecureString $script:pass

$cred = New-Object System.Management.Automation.PSCredential $script:userName,$p

verifyCredentials $cred

}

function verifyCredentials($cred){

Write-Host "Verifying Credentials. Please wait."

$bModuleLoaded=$false

$bConnectedToService=$false

Get-Module|%{if($_.Name -eq "MsOnline"){$bModuleLoaded = $true}}

if($bModuleLoaded -eq $true)

{

#Module is loaded proceed checking if we are logged in.

Write-Host -ForeGroundColor yellow "MsOnline Module is loaded."

}

else

{

# Module is not loaded. Load the module and connect.

try

{

Import-Module MsOnline -ErrorAction Stop

}

catch

{

Write-Host -ForegroundColor Red "Could not load MsOnline Module. Ensure it is installed."

exit

}

}

try

{

Connect-MsolService -Credential $cred -ErrorAction Stop

Write-Host -ForegroundColor Green "Your Credentials have been successfully Verified."

Write-Host "Retrieving SKU Information from Tenant"

$existingSKUInfo=Get-MsolAccountSku

$existingSKUInfo

}

catch

{

Write-Host -ForegroundColor Red "Could not connect to the service. Ensure the credentials are correct and then try again."

exit

}

}

function SetADAttributeName{

If((Test-Path $ADScriptName) -eq $true)

{

Write-Host -ForegroundColor Yellow "The script $ADScriptName has already been configured. Do you want to reset the configuration (Y/N)?"

$res = Read-Host

if($res.ToLower() -eq "y")

{

Remove-Item $ADScriptName -Force

}

else

{

Write-Host -ForegroundColor Red "Backup the " $ADScriptName " File and run the SetupScript again."

exit

}

}

                             

###

Write-Host -ForegroundColor Green "Please enter the ldapDisplayName of the Attribute you will be using to search for users in AD."

$ldapName=Read-Host -Prompt "ldapdisplayName: "

Write-Host -ForegroundColor Green "Please enter the Filter Value for the attribute you entered above."

Write-Host "The default value is * so all objects will be returned having the attribute set regardless of the value in the attribute."

Write-Host "to limit the objects returned you can use any valid LDAP Filter syntax."

$FilterDefault=Read-Host -Prompt "Attribute Filter"

if($ldapName.ToLower() -eq "memberof")

{

Write-Host -ForegroundColor Yellow "You have chosen to filter by group membership. This requires some additional information."

Write-Host "Please specify the ldapDisplayName of the attribute of the group object $FilterDefault which will contain the license information."

$grpLicenseInfoAttrib = Read-Host -Prompt "ldapDisplayName"

}

elseif($ldapName.ToLower() -ne "employeetype")

{

Write-Host -ForegroundColor Green "You have selected an attribute different then employeeType"

Write-Host "Please specify the ldapDisplayName of the attribute of the user object containing the license information."

$userLicenseInfo=Read-Host -Prompt "ldapDisplayName"

}

if ($FilterDefault -eq $null)

{

$FilterDefault="*"

}

$ldapName=$ldapName.ToLower()

if(($ldapName -eq $null) -or ($ldapName -eq "employeetype"))

{

Write-Host -ForegroundColor Green "Default Value accepted and set."

$ldapName="employeeType"

}

$rootDSE=[ADSI]"LDAP://RootDSE"

$Ldap="LDAP://"+$rootDSE.schemaNamingContext

$Ldap=$Ldap.Replace("LDAP://","")

$filter="(ldapdisplayName=$ldapName)"

$searcher=[adsisearcher]$Filter

$searcher.SearchRoot="LDAP://$Ldap"

$searcher.propertiesToLoad.Add("ldapDisplayName")

$results=$searcher.FindAll()

if($results.count -ne $null)

{

$input = Get-Content $ADScriptTemplate

foreach($l in $input)

{

$l=$l.replace("AttribNotSet",$ldapName)

$l=$l.replace("FilterNotSet",$FilterDefault)

$l=$l.replace("GroupInfoNotSet",$grpLicenseInfoAttrib)

$l=$l.replace("userLicenseAttribute",$userLicenseInfo)

Out-File -FilePath $ADScriptName -InputObject $l -Append

}

}

else

{

Write-Host -ForegroundColor Red "The specified Attribute $ldapName was not found. Please verify the Atribute Name and try again."

Write-Host "The configuration will be aborted!"

exit

}

}

function configureScript{

If((Test-Path $AssignmentScriptName) -eq $true)

{

Write-Host -ForegroundColor Yellow "The script $AssignmentScriptName has already been configured. Do you want to reset the configuration (Y/N)?"

$res = Read-Host

if($res.ToLower() -eq "y")

{

Remove-Item $AssignmentScriptName -Force

}

else

{

Write-Host -ForegroundColor Red "Backup the " $AssignmentScriptName " File and run the SetupScript again."

exit

}

}

###added 20140515-timbos

Write-Host "Configuring Scripts"

Write-Host -ForegroundColor Green "Please enter the default UsageLocation for your users if the Country attribute 'c' is not set for the user in your AD"

Write-Host -ForegroundColor Green "Use ISO-3166-1 alpha-2 notation - for more information see http://en.wikipedia.org/wiki/ISO_two-letter_country_codes if the code is unknwon to you"

Write-host -ForegroundColor Green "If you just press enter - then it is all greek to the script :)"

$script:defaultusagelocation=Read-Host -Prompt "DefaultUsageLocation: "

if (!$script:defaultusagelocation) {$script:defaultusagelocation = $DefaultUsageLocation}

       

Write-Host "Configuring Scripts"

$Content=Get-Content $AssignmentScriptTemplate

Foreach($line in $Content)

{

$line = $line.Replace("usernotset",$script:userName)

$line=$line.Replace("passnotset",$script:pass)

###added 20140515-timbos

$line=$line.Replace("usagelocationnotset",$script:defaultusagelocation)

###

Out-File -FilePath $AssignmentScriptName -InputObject $line -Append

}

$Content=Get-Content $ReportScriptTemplate 

Foreach($line in $Content)

{

$line = $line.Replace("usernotset",$script:userName)

$line=$line.Replace("passnotset",$script:pass)

Out-File -FilePath $ReportScriptName -InputObject $line -Append

}

}

function configureFolders{

Write-Host "Configuring Folders"

if((Test-Path ".\Logs") -eq $false){md ".\Logs"}

if((Test-Path ".\queuedLicense") -eq $false){md ".\queuedLicense"}

if((Test-Path ".\completedImportFiles") -eq $false){md ".\completedImportFiles"}

}

# Get the input from the user

getCredentials

# configure the Script with the credentials

configureScript

# Set the LDAP Property

SetADAttributeName

# configure the required folders

configureFolders

# show the valid license packages

$licMsg ="To configure the correct License Information in your on premises Active Directory use the following information"

Write-Host $licMsg

Out-File -FilePath licenseInformation.txt -InputObject $licMsg

if($existingSKUInfo -eq $null){$existingSKUInfo = Get-MsolAccountSku}

foreach($sku in $existingSKUInfo)

{

if($sku.ServiceStatus.Count -gt 1)

{

foreach($o in ($sku.ServiceStatus))

{

if($options -eq "")

{

$options= $o.ServicePlan.ServiceName

}

else

{

$options = $options +", " + $o.ServicePlan.ServiceName

}

}

}

else

{

 $options = "No options available."

}

$skuID=$sku.AccountSkuID.Split(":")[1]

Write-Host "$skuID has the following options: $options"

$outLine="$skuID;$options"

Out-File -Append -FilePath .\licenseinformation.txt -InputObject $outLine

$options =""

$skuID=""

$outLine=""

}

Write-Host "License Information has been stored in file licenseinformation.txt for your reference."

Write-Host "Setup Complete."  

 