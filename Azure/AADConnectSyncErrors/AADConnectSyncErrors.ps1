## Azure AD Connect: PowerShell Script to Report on Connector Sync Errors for the AAD Connect Client ##

<#
Description:
This script generates a list of users who are failing to export to Azure AD.

This script makes use of the FimSyncPowerShellModule - https://fimpowershellmodule.codeplex.com/ 

Download and copy the module to "C:\Windows\System32\WindowsPowerShell\v1.0\Modules", or Import it from it's file location

October 17 2013
Planet Technologies
www.go-planet.com
#>

$ConnectorName = "YourTenant.onmicrosoft.com - AAD" #Change your connector name to match your tenant

#Import the FimSyncPowerShellModule Module if copied to the default PowerShell Modules location
#Import-Module FimSyncPowerShellModule

#Import the FimSyncPowerShellModule Module directly
Import-Module "T:\BoxBuild\GitHub\Tools\Azure\AADConnectSyncErrors\FIMSyncPowerShellModule\FimSyncPowerShellModule.psm1" #Change this path to match your environment

#Get the last export run
$LastExportRun = (Get-MIIS_RunHistory -MaName $ConnectorName -RunProfile 'Export')[0]

#Get error objects from last export run (user errors only)
$UserErrorObjects = $LastExportRun | Get-RunHistoryDetailErrors | ? {$_.dn -ne $null}

$UsersWithErrors = @()
$ErrorFile = @()

#Build the custom Output Object
$UserErrorObjects | % {
    $TmpCSObject = Get-MIIS_CSObject  -ManagementAgent $ConnectorName -DN $_.DN
    [xml]$UserXML = $TmpCSObject.UnappliedExportHologram
    $MyObject = New-Object PSObject -Property @{
        EmailAddress = (Select-Xml -Xml $UserXML -XPath "/entry/attr" | select -expand node | ? {$_.name -eq 'mail'}).value
        UPN =          (Select-Xml -Xml $UserXML -XPath "/entry/attr" | select -expand node | ? {$_.name -eq 'userPrincipalName'}).value
        ErrorType = $_.ErrorType
        DN = $_.DN
        }
    $ErrorFile += $MyObject
    }

$FileName = "$env:TMP\AADSyncErrorsList-{0:dd.MM.yy}" -f (Get-Date) + ".CSV" #Change the file name and properties to match your requirements
$ErrorFile | select UPN, EmailAddress, ErrorType, DN | epcsv $FileName -NoType

#Output to the screen
$ErrorFile | select UPN, EmailAddress, ErrorType, DN

Write-Host
Write-Host $ErrorFile.count "users with errors. See here for a list:" -F Yellow
Write-Host $FileName -F Yellow
Write-Host