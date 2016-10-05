<# * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
   *                                                                                   *
   *  [DESCRIPTION]                                                                    *
   *                                                                                   *
   *  This Microsoft® Windows® PowerShell script offer functionality to manage         *
   *  Microsoft® Office 365 services.                                                  *
   *                                                                                   *
   *  Its main purpose is to disable or enable (show/hide) the Microsoft® Sway tile    *
   *  on the app launcher of the Microsoft® Office 365 portal, but can also be used    *
   *  to manage any other Microsoft® Office 365 service.                               *
   *                                                                                   *
   *  [VERSION]                                                                        *
   *                                                                                   *
   *  1.06                                                                             *
   *                                                                                   *
   *  [CREATE DATE]                                                                    *
   *                                                                                   *
   *  06/01/2016                                                                       *
   *                                                                                   *
   *  [AUTHOR]                                                                         *
   *                                                                                   *
   *  Claus Schiroky                                                                   *
   *  Customer Service & Support EMEA                                                  *
   *  Microsoft® Deutschland GmbH                                                      *
   *                                                                                   *   
   *  [SOURCE]                                                                         *
   *                                                                                   *
   *  https://technet.microsoft.com/en-us/library/mt671130.aspx                        *
   *                                                                                   *
   *  [SPECIAL THANKS TO]                                                              *
   *                                                                                   *
   *  Zeyad Rajabi, Chris Davis and Nuno Alexandre                                     *
   *                                                                                   *
   *  [COPYRIGHT]                                                                      *
   *                                                                                   *
   *  Copyright ©2016 by Microsoft®. All rights reserved.                              *
   *                                                                                   *
   * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * #>

<# Defining global variables #>
$Global:MyScriptVersion = "1.06"
$Global:MyCreateDate = "06/02/2016"
$Global:MyOffice365GlobalAdmin = $Null
$Global:MyCredentials = $Null
$Global:MyAccountSkuId = $Null
$Global:MyService = $Null
$Global:MyMenu2Extended = $False
$Global:MyWindowsVersion = (Get-WmiObject -Class Win32_OperatingSystem).Caption <# Evaluating Windows Editon #>
$Global:MyExecutionPolicy = "Restricted" <# Setting default to "Restriced" for security reasons #>

<# Start transcript logging #>
Start-Transcript -Path $Private:PSScriptRoot"\Transcript.log" -Append -Force

<# Check for existing log file and act accordingly #>
If ($(Test-Path $Private:PSScriptRoot"\ManageSway.log") -Match $False) {
    
    Out-File -FilePath $Private:PSScriptRoot"\ManageSway.log" -Encoding UTF8 -Append -Force <# Create log file #>
    Add-Content -Path $Private:PSScriptRoot"\ManageSway.log" -Value "DATE, TIME, FUNCTION, DESCRIPTION, VALUE" <# Adding headers to log file #>
}

Write-Host "Logging started, output file is $Private:PSScriptRoot\ManageSway.log"

Function fncCreateLogEntry ($LogFunction, $LogDescription, $LogValue) {

    <# Adding log entry to log file #>
    Add-Content -Path $Private:PSScriptRoot"\ManageSway.log" -Value ($(Get-Date -UFormat "%m/%d/%Y") + ", " + $(Get-Date -Format T) + ", $LogFunction, $LogDescription, $LogValue")

}

Function fncCreateDefaultLogEntries {

    <# Logging script version #>
    fncCreateLogEntry -LogFunction "root" -LogDescription "Script version" -LogValue $Global:MyScriptVersion 

    <# Logging Azure AD module version #>
    If (Test-Path "$Env:WinDir\System32\WindowsPowerShell\v1.0\Modules\MSOnline\Microsoft.Online.Administration.Automation.PSModule.dll") {

        fncCreateLogEntry -LogFunction "root" -LogDescription "Azure AD module version" -LogValue ($(Get-Item "C:\Windows\System32\WindowsPowerShell\v1.0\Modules\MSOnline\Microsoft.Online.Administration.Automation.PSModule.dll").VersionInfo.FileVersion)
    }
    Else {

        fncCreateLogEntry -LogFunction "root" -LogDescription "Azure AD module version" -LogValue $False 
    }

    <# Logging .Net Framework 3.5 #>
    If (@(dir $Env:windir\Microsoft.NET\Framework\v* -Name) -Contains "v3.5" -Match $True) {

        fncCreateLogEntry -LogFunction "root" -LogDescription ".NET Framework 3.5 installed" -LogValue $True 
        fncCreateLogEntry -LogFunction "root" -LogDescription ".NET Framework 3.5 version" -LogValue (Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5\1033" -Name Version).Version
    }
    Else {
        fncCreateLogEntry -LogFunction "root" -LogDescription ".NET Framework 3.5 installed" -LogValue $False
    }

    <# Logging Windows version and edition#>
    fncCreateLogEntry -LogFunction "fncCheckWindowsVersion" -LogDescription "Windows edition" -LogValue $Global:MyWindowsVersion <# Windows edition#>
    fncCreateLogEntry -LogFunction "fncCheckWindowsVersion" -LogDescription "Windows version" -LogValue (Get-WmiObject -Class Win32_OperatingSystem).Version <# Windows version#>   

}

Function fncCheckWindowsVersion {

    If ($Global:MyWindowsVersion -Match "Windows 7" -Or $Global:MyWindowsVersion -Match "Windows 8"-Or $Global:MyWindowsVersion -Match "Windows 10") {
        $Global:MyExecutionPolicy = "Restricted"
    }
    ElseIf ($Global:MyWindowsVersion -Match "2012") {
        $Global:MyExecutionPolicy = "RemoteSigned"
    }
    Else {
        
        Set-ExecutionPolicy $Global:MyExecutionPolicy <# Setting back execution policy to its default value #>

        <# Clearing used variables before exiting #>
        $Global:MyWindowsVersion = $Null
        $Global:MyExecutionPolicy = $Null
        
        Clear-Host

        Write-Host ""
        Write-Host "  ╔══════════════════════════════════════════════════════════════════════════════════╗ " -ForegroundColor Yellow
        Write-Host "  ║                                                                                  ║ " -ForegroundColor Yellow
        Write-Host "  ║  [INFORMATION]                                                                   ║ " -ForegroundColor Yellow
        Write-Host "  ║                                                                                  ║ " -ForegroundColor Yellow
        Write-Host "  ║  This script does not support the operating system you're using.                 ║ " -ForegroundColor Yellow
        Write-Host "  ║                                                                                  ║ " -ForegroundColor Yellow
        Write-Host "  ║  Please ensure to use a supported operating system. The script supports the      ║ " -ForegroundColor Yellow
        Write-Host "  ║  following operating systems: Microsoft® Windows® 7, Windows® 8, Windows® 8.1,   ║ " -ForegroundColor Yellow
        Write-Host "  ║  Windows® 10, Windows® Server 2012 and Windows® Server 2012 R2.                  ║ " -ForegroundColor Yellow        
        Write-Host "  ║                                                                                  ║ " -ForegroundColor Yellow
        Write-Host "  ║  Note:                                                                           ║ " -ForegroundColor Yellow
        Write-Host "  ║  The execution policy has been set back to its default value.                    ║ " -ForegroundColor Yellow
        Write-Host "  ║                                                                                  ║ " -ForegroundColor Yellow
        Write-Host "  ╚══════════════════════════════════════════════════════════════════════════════════╝ " -ForegroundColor Yellow
        Write-Host ""

        Exit
    }

}

Function fncClearCache {

    <# Clearing the cache by releasing some global variables #>
    $Global:MyOffice365GlobalAdmin = $Null
    $Global:MyCredentials = $Null
    $Global:MyAccountSkuId = $Null

    <# Logging cache cleared #>
    fncCreateLogEntry -LogFunction "fncClearCache" -LogDescription "Cache cleared" -LogValue $True

}

Function fncPause ($Private:Message) {
 
    <# Pausing the script with a message #>
    Write-Host "$Private:Message"
    $Private:Value = $Private:Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

    <# Logging pause#>
    fncCreateLogEntry -LogFunction "fncPause" -LogDescription "Pause" -LogValue $True

} 

Function fncCheckISE {

    <# Checking if running in PowerShell ISE and accting accordingly #>
    If ($Global:psISE) {
        Start-Sleep 10 <# 10 secondd break when running script in ISE #>
        fncCreateLogEntry -LogFunction "fncCheckISE" -LogDescription "Sleep" -LogValue "10" <# Logging break#>
    }
    Else {
        fncPause "Press any key to continue" <# Action when running script in console window #>
    }
    
}

Function fncLogon {
  
    <# Feeding the "Windows® PowerShell credential request"-dialog with admin email address #>
    $Global:MyCredentials = Get-Credential $Global:MyOffice365GlobalAdmin

    <# Connecting to Office 365 with admin credentials #>
    Connect-MsolService -Credential $Global:MyCredentials

    <# Logging #>
    fncCreateLogEntry -LogFunction "fncLogon" -LogDescription "Admin" -LogValue $Global:MyOffice365GlobalAdmin <# Admin mail #>
    fncCreateLogEntry -LogFunction "fncLogon" -LogDescription "Credentials dialog" -LogValue $True <# Credentials dialog call #>

}

Function fncCheckForGlobalAdmin {

    <# Checking if a previously used email address exist #>
    If ($Global:MyOffice365GlobalAdmin) {Write-Host "  Found previously used Microsoft® Office 365 global admin email address:"} 
    Else {$Global:MyOffice365GlobalAdmin = Read-Host "  Please type your Microsoft® Office 365 global admin email address and press enter"} 

    <# Logging  used admin #>
    fncCreateLogEntry -LogFunction "fncCheckForGlobalAdmin" -LogDescription "Admin" -LogValue $Global:MyOffice365GlobalAdmin 
}

Function fncResetLogging {

    Stop-Transcript <# Stop transcript logging #>
    If ($(Test-Path $Private:PSScriptRoot"\Transcript.log") -Match $True) {
        Remove-Item $Private:PSScriptRoot"\Transcript.log"
        Start-Transcript -Path $Private:PSScriptRoot"\Transcript.log" -Append -Force <# Create new Transcript #>
    }
    If ($(Test-Path $Private:PSScriptRoot"\Transcript.log") -Match $True) {
        Remove-Item $Private:PSScriptRoot"\ManageSway.log"
        Out-File -FilePath $Private:PSScriptRoot"\ManageSway.log" -Encoding UTF8 -Append -Force <# Create new log file #>
        Add-Content -Path $Private:PSScriptRoot"\ManageSway.log" -Value "DATE, TIME, FUNCTION, DESCRIPTION, VALUE" <# Adding headers to log file #>
        fncCreateDefaultLogEntries <# Creating basic log entries #>
        Write-Host "Logging started, output file is $Private:PSScriptRoot\ManageSway.log"
    }
    Write-Host 
    Clear-Host

}

Function fncCheckForAccountSku {

    <# Checking if a previously used account SKU ID exist #>
    If ($Global:MyAccountSkuId) {Write-Host "  Found previously used AccountSkuId:"}
    Else {$Global:MyAccountSkuId = Read-Host "  Please type your AccountSkuId and press enter"}

    <# Logging previously used AccountSkuId #>
    fncCreateLogEntry -LogFunction "fncCheckForAccountSku" -LogDescription "AccountSkuId" -LogValue $Global:MyAccountSkuId

}

 Function fncCheckForMyService {

    <# Checking for service plan #>
    $Global:MyService = Read-Host "  Please type one ServicePlan name and press enter"

    <# Logging service plan #>
    fncCreateLogEntry -LogFunction "fncCheckForMyService" -LogDescription "Service plan" -LogValue $Global:MyService

}

Function fncMain($ActionInstruction) {

    Write-Host "  You selected:"
  
    If ($ActionInstruction -Eq "ReviewAccountSkuId") {

        Write-Host "  [1] REVIEW account SKU IDs" -ForegroundColor Green; Write-Host ""

        fncCheckForGlobalAdmin <# Calling function to check if a previously used address exist #>
        Write-Host "  Using global admin email address:" $Global:MyOffice365GlobalAdmin -ForegroundColor Green; Write-Host ""

        fncLogon <# Function to logon to Microsoft® Office 365 #>

        <# Informational output #>
        Write-Host "  Note:" -ForegroundColor Yellow
        Write-Host "  You can now select and copy your related AccountSkuId." -ForegroundColor Yellow
  
        $Private:MyAcocuntSku = Get-MsolAccountSku
        Out-Host -InputObject $Private:MyAcocuntSku <# Showing your Account SKU IDs" #>
        
        <# Logging Account SKU ID check#>
        fncCreateLogEntry -LogFunction "fncMain (ReviewAccountSkuId)" -LogDescription "Account SKU ID reviewed" -LogValue $True 

    }

    If ($ActionInstruction -Eq "DisableAServiceForAllUsers") {

        Write-Host "  [21] DISABLE a service for all users" -ForegroundColor Green; Write-Host ""

        fncCheckForAccountSku <# Checking if a previously used account SKU ID exist #>
        Write-Host "  Using AccountSkuId:" $Global:MyAccountSkuId -ForegroundColor Green; Write-Host ""

        fncCheckForGlobalAdmin <# Calling function to check if a previously used address exist #>
        Write-Host "  Using global admin email address:" $Global:MyOffice365GlobalAdmin -ForegroundColor Green; Write-Host ""

        fncLogon <# Function to logon to Microsoft® Office 365 #>

        fncCheckForMyService <# Checking which service plan name to use #>
        Write-Host "  Using ServicePlan name:" $Global:MyService -ForegroundColor Green; Write-Host ""        

        <# Disabling service for all licensed users #>
        Get-MsolUser -All | Where-Object {$_.IsLicensed -Eq $True} | % {
            <# Output user being disabled #>
            $Private:UPN = $_.UserPrincipalName
            <# Select the AccountSkuId license of the user #>
            $Private:UserLicenses = $_.Licenses | Where-Object {$_.AccountSkuId -Eq $Global:MyAccountSkuId}
            <# Build the list of service plans that are disabled #>
            $Private:DisabledPlans = @()
            $Private:DisabledPlans += $Private:UserLicenses.ServiceStatus | Where-Object {$_.ProvisioningStatus -Eq "Disabled"} | % {$_.ServicePlan.ServiceName}
            <# Add the new service plan to set as disabled #>
            $Private:DisabledPlans += $Global:MyService
            <# Logging disabled users#>
            fncCreateLogEntry -LogFunction "fncMain (DisableAServiceForAllUsers)" -LogDescription "$Global:MyService disabled" -LogValue $Private:UPN
            <# Create a new license option #>
            $Private:LicenseOptions = New-MsolLicenseOptions -AccountSkuId $Global:MyAccountSkuId -DisabledPlans $Private:DisabledPlans
            <# Set user license #>
            Set-MsolUserLicense -UserPrincipalName $Private:UPN -LicenseOptions $Private:LicenseOptions
        }
         
        Write-Host "  The ServicePlan $Global:MyService should be shown as `"Disabled`":"  -ForegroundColor Green
        Out-Host -InputObject (Get-MsolUser -UserPrincipalName $Global:MyOffice365GlobalAdmin).Licenses.ServiceStatus <# Displaying service status #>

    }

    If ($ActionInstruction -Eq "DisableAServiceForOneUser") {

        Write-Host "  [22] DISABLE a service for one user" -ForegroundColor Green; Write-Host ""

        fncCheckForAccountSku <# Checking if a previously used account SKU ID exist #>
        Write-Host "  Using AccountSkuId:" $Global:MyAccountSkuId -ForegroundColor Green; Write-Host ""

        fncCheckForGlobalAdmin <# Calling function to check if a previously used address exist #>
        Write-Host "  Using global admin email address:" $Global:MyOffice365GlobalAdmin -ForegroundColor Green; Write-Host ""
        
        fncLogon <# Function to logon to Microsoft® Office 365 #>

        fncCheckForMyService <# Checking which service plan name to use #>
        Write-Host "  Using ServicePlan name:" $Global:MyService -ForegroundColor Green; Write-Host "" 

        $Private:MyOffice365User = Read-Host "  Please type a Microsoft® Office 365 user email address and press enter" <# Asking for related user #>
        Write-Host "  Using user email address:" $Private:MyOffice365User -ForegroundColor Green; Write-Host ""
      
        <# Disabling the choosen service for the choosen user #>
        $Private:User = Get-MsolUser -UserPrincipalName $Private:MyOffice365User <# Get the user #>
        <# Select the AccountSkuId license of the user #>
        $Private:UserLicenses = $Private:User.Licenses | Where-Object {$_.AccountSkuId -Eq $Global:MyAccountSkuId}
        <# Build the list of service plans that are disabled #>
        $Private:DisabledPlans = @()
        $Private:DisabledPlans += $Private:UserLicenses.ServiceStatus | Where-Object {$_.ProvisioningStatus -Eq "Disabled"} | % {$_.ServicePlan.ServiceName}
        <# Add the new service plan to set as disabled #>
        $Private:DisabledPlans += $Global:MyService
        <# Logging disabled user#>
        fncCreateLogEntry -LogFunction "fncMain (DisableAServiceForOneUser)" -LogDescription "$Global:MyService disabled" -LogValue $Private:MyOffice365User
        <# Create a new license option #>
        $Private:LicenseOptions = New-MsolLicenseOptions -AccountSkuId $Global:MyAccountSkuId -DisabledPlans $Private:DisabledPlans
        <# Set user license #>
        Set-MsolUserLicense -UserPrincipalName $Private:MyOffice365User -LicenseOptions $Private:LicenseOptions

        Write-Host "  The ServicePlan name $Global:MyService should be shown as `"Disabled`":" -ForegroundColor Green
        Out-Host -InputObject (Get-MsolUser -UserPrincipalName $Private:MyOffice365User).Licenses.ServiceStatus <# Displaying service status #>
    }

    If ($ActionInstruction -Eq "EnableAllServicesForAllUsers") {

        Write-Host "  [3] ENABLE all services for all users" -ForegroundColor Green; Write-Host ""

        fncCheckForAccountSku <# Checking if a previously used account SKU ID exist #>
        Write-Host "  Using AccountSkuId:" $Global:MyAccountSkuId -ForegroundColor Green; Write-Host ""
      
        fncCheckForGlobalAdmin <# Calling function to check if a previously used address exist #>
        Write-Host "  Using global admin email address:" $Global:MyOffice365GlobalAdmin -ForegroundColor Green; Write-Host ""

        fncLogon <# Function to logon to Microsoft® Office 365 #>

        <# Enabling the choosen service for all users #>
        Get-MsolUser -All | Set-MsolUserLicense -LicenseOptions (New-MsolLicenseOptions -AccountSkuId $Global:MyAccountSkuId -DisabledPlans $Null)
  
        Write-Host "  No ServicePlan should be shown as `"Disabled`":" -ForegroundColor Green
        Out-Host -InputObject (Get-MsolUser -UserPrincipalName $Global:MyOffice365GlobalAdmin).Licenses.ServiceStatus <# Displaying service status #>

        <# Logging disabled user#>
        fncCreateLogEntry -LogFunction "fncMain (EnableAllServicesForAllUsers)" -LogDescription "All services enabled" -LogValue $True

    }

    If ($ActionInstruction -Eq "ReviewServicePlanStatusForOneUser") {

        Write-Host "  [4] REVIEW service plan status for one user" -ForegroundColor Green; Write-Host ""

        fncCheckForAccountSku <# Checking if a previously used account SKU ID exist #>
        Write-Host "  Using AccountSkuId:" $Global:MyAccountSkuId -ForegroundColor Green; Write-Host ""
 
        fncCheckForGlobalAdmin <# Calling function to check if a previously used address exist #>
        Write-Host "  Using global admin email address:" $Global:MyOffice365GlobalAdmin -ForegroundColor Green; Write-Host ""
        
        fncLogon <# Function to logon to Microsoft® Office 365 #>

        $MyOffice365User = Read-Host "  Please type the Microsoft® Office 365 user email address and press enter" <# Asking for related user #>
        Write-Host "  Using user email address:" $MyOffice365User -ForegroundColor Green; Write-Host ""
    
        Write-Host "  Status:" -ForegroundColor Green
        Out-Host -InputObject (Get-MsolUser -UserPrincipalName $MyOffice365User).Licenses.ServiceStatus <# Displaying service status #>

        <# Logging service plan review #>
        fncCreateLogEntry -LogFunction "fncMain (ReviewServicePlanStatusForOneUser)" -LogDescription "Services plan reviewed" -LogValue $MyOffice365User

    }
    
    fncCheckISE <# Checking if running in PowerShell ISE and accting accordingly #>
}

Function fncShowWarning {

    Write-Host ""
    Write-Host "  ╔══════════════════════════════════════════════════════════════════════════════════╗ " -ForegroundColor Red
    Write-Host "  ║                                                                                  ║ " -ForegroundColor Red
    Write-Host "  ║  [WARNING!]                                                                      ║ " -ForegroundColor Red
    Write-Host "  ║                                                                                  ║ " -ForegroundColor Red
    Write-Host "  ║  You are using this Microsoft® Windows® PowerShell script at our own risk.       ║ " -ForegroundColor Red
    Write-Host "  ║  Microsoft® provides this script code 'as is' without warranty of any kind,      ║ " -ForegroundColor Red
    Write-Host "  ║  either express or implied, including but not limited to the implied warranties  ║ " -ForegroundColor Red
    Write-Host "  ║  of merchantability and/or fitness for a particular purpose. This script is      ║ " -ForegroundColor Red
    Write-Host "  ║  only meant to be a sample which should demonstrate a specific technique and is  ║ " -ForegroundColor Red
    Write-Host "  ║  intended to be a 'point to start from'. It is not intended to be a 'ready to    ║ " -ForegroundColor Red
    Write-Host "  ║  use' application. Please do understand that there is no guarantee that this     ║ " -ForegroundColor Red
    Write-Host "  ║  script will run with any given environment or configuration.                    ║ " -ForegroundColor Red
    Write-Host "  ║                                                                                  ║ " -ForegroundColor Red
    Write-Host "  ║  There is no support for this script.                                            ║ " -ForegroundColor Red
    Write-Host "  ║                                                                                  ║ " -ForegroundColor Red
    Write-Host "  ║  It is highly recommended to test this script with a free trial version of       ║ " -ForegroundColor Red
    Write-Host "  ║  Microsoft® Office 365 before proceeding the script in a live environment.       ║ " -ForegroundColor Red
    Write-Host "  ║                                                                                  ║ " -ForegroundColor Red
    Write-Host "  ╚══════════════════════════════════════════════════════════════════════════════════╝ " -ForegroundColor Red
    Write-Host ""

    <# Logging warning review #>
    fncCreateLogEntry -LogFunction "fncShowWarning" -LogDescription "Warning reviewed" -LogValue $True
  
    fncCheckISE <# Checking if running in PowerShell ISE and accting accordingly #>
  
    Clear-Host
}

Function fncShowInformation {

    Write-Host "  You selected:"
    Write-Host "  [6] INFORMATION" -ForegroundColor Green; Write-Host ""

    Write-Host "  ┌──────────────────────────────────────────────────────────────────────────────────┐" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  [DESCRIPTION]                                                                   │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  This Microsoft® Windows® PowerShell script offer functionality to manage        │" -ForegroundColor Yellow
    Write-Host "  │  Microsoft® Office 365 services.                                                 │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  Its main purpose is to disable or enable (show/hide) the Microsoft® Sway tile   │" -ForegroundColor Yellow
    Write-Host "  │  on the app launcher of the Microsoft® Office 365 portal, but can also be used   │" -ForegroundColor Yellow
    Write-Host "  │  to manage any other Microsoft® Office 365 service.                              │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  [VERSION]                                                                       │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  $Global:MyScriptVersion                                                                            │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  [CREATE DATE]                                                                   │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  $Global:MyCreateDate                                                                      │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  [AUTHOR]                                                                        │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  Claus Schiroky                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  Customer Service & Support EMEA                                                 │" -ForegroundColor Yellow
    Write-Host "  │  Microsoft® Deutschland GmbH                                                     │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  [SOURCE]                                                                        │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  https://technet.microsoft.com/en-us/library/mt671130.aspx                       │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow    
    Write-Host "  │  [SPECIAL THANKS TO]                                                             │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  Zeyad Rajabi, Chris Davis and Nuno Alexandre                                    │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  [COPYRIGHT]                                                                     │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  │  Copyright ©2016 by Microsoft®. All rights reserved.                             │" -ForegroundColor Yellow
    Write-Host "  │                                                                                  │" -ForegroundColor Yellow
    Write-Host "  └──────────────────────────────────────────────────────────────────────────────────┘" -ForegroundColor Yellow
    Write-Host ""

    <# Logging information review #>
    fncCreateLogEntry -LogFunction "fncShowInformation" -LogDescription "Information reviewed" -LogValue $True

    fncCheckISE <# Checking if running in PowerShell ISE and accting accordingly #>

}

Function fncShowHelp {

    Write-Host "  You selected:"
    Write-Host "  [7] HELP" -ForegroundColor Green; Write-Host ""
    
    Invoke-Item $Private:PSScriptRoot"\ManageSwayHelp.htm"

    <# Logging help file call #>
    fncCreateLogEntry -LogFunction "fncShowHelp" -LogDescription "Help reviewed" -LogValue $True

    fncCheckISE <# Checking if running in PowerShell ISE and accting accordingly #>

}

Function fncExit {
  
    Clear-Host

    Write-Host "  You selected:"
    Write-Host "  [8] EXIT" -ForegroundColor Green; Write-Host ""
    Write-Host "  Are you sure you want to exit?"; $Private:ConfirmExit = Read-Host "  [Y] Yes  [N] No"

    <# Actions if Yes has been selected #>
    If ($Private:ConfirmExit -Eq "Y") {
  
        Write-Host "" <# Insert a linebreak #>

        <# Releasing global variables #>
        fncClearCache
        $Global:MyWindowsVersion = $Null

        Set-ExecutionPolicy $Global:MyExecutionPolicy <# Setting back execution policy to its default value #>

        <# Logging execution policy #>
        fncCreateLogEntry -LogFunction "fncExit" -LogDescription "Execution policy default set" -LogValue $True
        
        <# Informational output #>
        Write-Host "  Note:" -ForegroundColor Yellow
        Write-Host "  The execution policy has been set back to its default value ($Global:MyExecutionPolicy)." -ForegroundColor Yellow
        Write-Host ""
    
        $Global:MyExecutionPolicy = $Null

        <# Logging exit #>
        fncCreateLogEntry -LogFunction "fncExit" -LogDescription "Exit script" -LogValue $True
        Add-Content -Path $Private:PSScriptRoot"\ManageSway.log" -Value "*********" <# Adding seperator to log file #>
        
        Stop-Transcript <# Stop transcript logging #>
		Write-Host "Logging stopped, output file is $Private:PSScriptRoot\ManageSway.log"
        Exit <# Exit PowerShell script #>
        
    }
   
    <# Actions if No has been selected #>
    If ($Private:ConfirmExit -Eq "N") {Clear-Host; fncMenu}
    Else {fncExit} <# For any other action jump back to menu #>

}

Function fncMenu {  

    fncCreateLogEntry -LogFunction "fncMenu" -LogDescription "Main menu" -LogValue $True <# Logging exit #>

    Write-Host "  [1] REVIEW account SKU IDs" -ForegroundColor Green
    Write-Host "  [2] DISABLE a service" -ForegroundColor Green
    If (@($Global:MyMenu2Extended) -Match $True) {
        Write-Host "   ├──[21] DISABLE a service for all users" -ForegroundColor Green
        Write-Host "   └──[22] DISABLE a service for one user" -ForegroundColor Green
        Write-Host "  [3] ENABLE all services for all users" -ForegroundColor Green
        Write-Host "  [4] REVIEW service plan status for one user" -ForegroundColor Green
        Write-Host "  [5] WARNING" -ForegroundColor Red
        Write-Host "  [6] INFORMATION" -ForegroundColor Yellow
        Write-Host "  [7] HELP" -ForegroundColor Yellow
        Write-Host "  [8] EXIT" -ForegroundColor Yellow; Write-Host ""
    }
    Else {
        Write-Host "  [3] ENABLE all services for all users" -ForegroundColor Green
        Write-Host "  [4] REVIEW service plan status for one user" -ForegroundColor Green
        Write-Host "  [5] WARNING" -ForegroundColor Red
        Write-Host "  [6] INFORMATION" -ForegroundColor Yellow
        Write-Host "  [7] HELP" -ForegroundColor Yellow
        Write-Host "  [8] EXIT" -ForegroundColor Yellow; Write-Host ""
    }
 
    $Private:ReturnValue = Read-Host "Please select an option and press enter"
    
    If ($Private:ReturnValue -Eq 1) {fncCreateLogEntry -LogFunction "fncMenu" -LogDescription "[1]" -LogValue "Selected"; Clear-Host; fncMain -ActionInstruction "ReviewAccountSkuId"}
    If ($Private:ReturnValue -Eq 2) {
        If (@($Global:MyMenu2Extended) -Match $True) {$Global:MyMenu2Extended = $False}
        Else {$Global:MyMenu2Extended = $True}
        fncCreateLogEntry -LogFunction "fncMenu" -LogDescription "[2]" -LogValue "Selected"
        Clear-Host
        fncMenu
    }
    If ($Private:ReturnValue -Eq 21) {fncCreateLogEntry -LogFunction "fncDisableAService" -LogDescription "[21]" -LogValue "Selected"; Clear-Host; fncMain -ActionInstruction "DisableAServiceForAllUsers"}
    If ($Private:ReturnValue -Eq 22) {fncCreateLogEntry -LogFunction "fncDisableAService" -LogDescription "[22]" -LogValue "Selected"; Clear-Host; fncMain -ActionInstruction "DisableAServiceForOneUser"}
    If ($Private:ReturnValue -Eq 3) {fncCreateLogEntry -LogFunction "fncMenu" -LogDescription "[3]" -LogValue "Selected"; Clear-Host; fncMain -ActionInstruction "EnableAllServicesForAllUsers"}
    If ($Private:ReturnValue -Eq 4) {fncCreateLogEntry -LogFunction "fncMenu" -LogDescription "[4]" -LogValue "Selected"; Clear-Host; fncMain -ActionInstruction "ReviewServicePlanStatusForOneUser"}    
    If ($Private:ReturnValue -Eq 5) {fncCreateLogEntry -LogFunction "fncMenu" -LogDescription "[5]" -LogValue "Selected"; Clear-Host; Write-Host "  You selected:"; Write-Host "  [5] WARNING" -ForegroundColor Green; fncShowWarning}
    If ($Private:ReturnValue -Eq 6) {fncCreateLogEntry -LogFunction "fncMenu" -LogDescription "[6]" -LogValue "Selected"; Clear-Host; fncShowInformation}
    If ($Private:ReturnValue -Eq 7) {fncCreateLogEntry -LogFunction "fncMenu" -LogDescription "[7]" -LogValue "Selected"; Clear-Host; fncShowHelp}
    If ($Private:ReturnValue -Eq 8) {fncCreateLogEntry -LogFunction "fncMenu" -LogDescription "[8]" -LogValue "Selected"; fncExit}
    If ($Private:ReturnValue -Eq "C") {fncCreateLogEntry -LogFunction "fncMenu" -LogDescription "[C]" -LogValue "Selected"; Clear-Host; fncClearCache; fncMenu} <# Clearing cache #>
    If ($Private:ReturnValue -Eq "R") {Clear-Host; fncResetLogging; fncMenu} <# Reset logging #>
    Else {Clear-Host;fncMenu}

 } 

Clear-Host

fncCreateDefaultLogEntries <# Creating default log entries #>
fncCheckWindowsVersion <# Checking Windows® version and exit if running on unsupported version #>
fncShowWarning <# Showing warning message #>
fncMenu <# Calling main menu #>