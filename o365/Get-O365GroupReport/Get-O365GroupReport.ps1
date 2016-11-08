<#
.SYNOPSIS
Get-O365GroupReport.ps1 - Reports on Office 365 Groups that have been created or modified since the script last ran.

.DESCRIPTION 
This script provides an email report of Office 365 Group status. Groups that have been created, modified, or unchanged are shown in the report.

.OUTPUTS
Email to defined recipient(s).

.PARAMETER UseCredential
Credentials to pass to Connect-EXOnline. Requires that the Connect-EXOnline
function (https://github.com/cunninghamp/Office-365-Scripts/tree/master/Connect-EXOnline) and
the Get-StoredCredential function (http://practical365.com/blog/saving-credentials-for-office-365-powershell-scripts-and-scheduled-tasks/)
be available. If those functions are not available, the script will fall back
on a basic Exchange Online connection function that will prompt for
admin credentials.

.EXAMPLE
.\Get-O365GroupReport.ps1

.EXAMPLE
.\Get-O365GroupReport.ps1 -UseCredential admin@tenantname.onmicrosoft.com

.EXAMPLE
.\Get-O365GroupReport.ps1 -Verbose

.LINK
https://github.com/cunninghamp/Office365GroupsReport

.NOTES
Written by Paul Cunningham and Chris Brown

For more Office 365 tips, tutorials and news
check out Practical365.com.


Version history:
V1.00, 20/10/2016 - Initial version


License:

The MIT License (MIT)

Copyright (c) 2015 Paul Cunningham

Permission is hereby granted, free of charge, to any person obtaining a copy 
of this software and associated documentation files (the "Software"), to deal 
in the Software without restriction, including without limitation the rights 
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
copies of the Software, and to permit persons to whom the Software is 
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all 
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.

#>

[CmdletBinding()]
param (
        [Parameter(Mandatory=$false)]
        [string]$UseCredential
)


#...................................
# Variables
#...................................

$now = Get-Date

$myDir = Split-Path -Parent $MyInvocation.MyCommand.Path

$XMLFileName = "$($myDir)\UnifiedGroups.xml"

$NewGroups = @()
$ModifiedGroups = @()
$DeletedGroups = @()
$UnmodifiedGroups = @()


#...................................
# Settings
#...................................

# Import settings from configuration file
$ScriptName = $([System.IO.Path]::GetFileNameWithoutExtension($MyInvocation.MyCommand.Name))
$ConfigFile = Join-Path -Path $MyDir -ChildPath "$ScriptName.xml"
if (-not (Test-Path $ConfigFile)) {
    throw "Could not find configuration file! Be sure to rename the '$ScriptName.xml.sample' file to '$ScriptName.xml'."
}

$settings = ([xml](Get-Content $ConfigFile)).Settings

# If the $smtpSettings.SmtpServer value is either "" or $null, the script
# will attempt to automatically derive the SMTP server from the recipient
# domain's MX records
$smtpsettings = @{
	To =  $settings.EmailSettings.To
	From = $settings.EmailSettings.From
	Subject = "$($settings.EmailSettings.Subject) - $now"
	SmtpServer = $settings.EmailSettings.SmtpServer
	}


#...................................
# Functions
#...................................

Function ConnectToEXO() {
    Write-Verbose "Admin credentials are required for connecting to Exchange Online."
    $Credential = Get-Credential -Message "Enter your Exchange Online administrative credentials."
    if ($Credential -ne $null) {
        $URL = "https://outlook.office365.com/powershell-liveid/"
        $EXOSession = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri $URL -Credential $Credential -Authentication Basic -AllowRedirection -Name "Exchange Online"
        Import-PSSession $EXOSession
    }
    else {
        throw "A credential was not provided."
    }
}


#...................................
# Script
#...................................

#Check for previous results
if (Test-Path $XMLFileName) {
    
    #XML file found, ingest as last results
    $LastResults = Import-Clixml -Path $XMLFileName
}
else {
    Write-Verbose "No previous results found."
}

#Check whether an EXO remote session is already established and requires cmdlet is available
try {
    Get-Command Get-UnifiedGroup -ErrorAction STOP | Out-Null
}
catch {
    Write-Verbose "Get-UnifiedGroup cmdlet not available. Need to connect to Exchange Online."

    if ($UseCredential) {
        
        #Try connecting with Connect-EXOnline and stored credential
        try {
            Connect-EXOnline -UseCredential $UseCredential -ErrorAction STOP | Out-Null
        }
        catch {
            Write-Warning $_.Exception.Message
            Write-Warning "I recommend adding Connect-EXOnline to your PowerShell profile (Refer to: https://github.com/cunninghamp/Office-365-Scripts/tree/master/Connect-EXOnline)"
            Write-Warning "And the PSStoredCredentials functions (Refer to: http://practical365.com/blog/saving-credentials-for-office-365-powershell-scripts-and-scheduled-tasks/"
        
            Write-Warning "Using basic EXO connect function instead"
            ConnectToEXO
        }
    }
    else {
        ConnectToEXO
    }

}


#...................................
# Retrieve current list of Groups
#...................................

$UnifiedGroups = @(Get-UnifiedGroup | Select Guid,DisplayName,AccessType,Notes,ManagedBy,WhenCreated)

$LastResultsGuids = $LastResults.Guid

#Loop through Guids and check whether they exist in previous results
foreach ($Guid in $UnifiedGroups.Guid) {
    Write-Verbose "Checking if $Guid exists in last results"

    if ($LastResultsGuids -icontains $Guid) {
        
        #Build a custom object to store the group's details for the report and any changed properties
        $objectProperties = [ordered]@{
				"DisplayName" = $null
				"AccessType" = $null
				"Notes" = $null
				"ManagedBy" = $null
				"WhenCreated" = $null
				}
        
        $GroupObject = New-Object -TypeName PSObject -Property $objectProperties

        $HasChanged = $false
        $Changes = @()

        $CurrentObject = $UnifiedGroups | Where {$_.Guid -eq $Guid}
        $PreviousObject = $LastResults | Where {$_.Guid -eq $Guid}

        $GroupProperties = @($CurrentObject | Get-Member | Where {$_.MemberType -eq "NoteProperty"}).Name

        Write-Verbose "Group ""$($CurrentObject.DisplayName)"" exists in previous results"

        #Compare each property to determine if any have changed
        foreach ($Property in $GroupProperties) {
            
            #Compare all properties except Guid
            if (-not($Property -eq "Guid")) {
                if ($CurrentObject.$Property -ieq $PreviousObject.$Property) {
            
                    Write-Verbose "No change detected for $Property"
                    $GroupObject.$Property = $CurrentObject.$Property
                }
                else {
                    
                    #If a value is null, replace with string "blank"

                    if ($($PreviousObject.$Property) -eq $null -or $($PreviousObject.$Property) -eq "") {
                        $previous = "blank"
                    }
                    else {
                        $previous = $PreviousObject.$Property
                    }

                    if ($($CurrentObject.$Property) -eq $null -or $($CurrentObject.$Property) -eq "") {
                        $current = "Blank"
                    }
                    else {
                        $current = $CurrentObject.$Property
                    }

                    if ($current -ceq "Blank") {
                        Write-Verbose "$Property is different (was $previous, and is now $($current.ToLower())"
                    }
                    else {
                        Write-Verbose "$Property is different (was $previous, and is now $current)"
                    }

                    $HasChanged = $true
                    $GroupObject.$Property = "$current (was $($previous))"
                }
            }
        }

        #Add the group to either the modified or unmodified list
        if ($HasChanged) {
            
            $ModifiedGroups += $GroupObject

        }
        else {
        
            $UnmodifiedGroups += $GroupObject
        
        }
            
    }
    else {

        Write-Verbose "Group does not exist in previous results, therefore is a new group"

        $NewObject = $UnifiedGroups | Where {$_.Guid -eq $Guid} | Select DisplayName,AccessType,Notes,ManagedBy,WhenCreated

        $NewGroups += $NewObject

    }
}

#Loop through Last Results to check for deleted groups

$CurrentGuids = $UnifiedGroups.Guid

foreach ($Guid in $LastResultsGuids) {
    Write-Verbose "Checking if $Guid has been deleted since last report"

    if (-not ($CurrentGuids -icontains $Guid)) {
    
        Write-Verbose "Group $Guid has been deleted since last report"

        $DeletedObject = $LastResults | Where {$_.Guid -eq $Guid} | Select DisplayName,AccessType,Notes,ManagedBy,WhenCreated

        $DeletedGroups += $DeletedObject

    }

}

#Output a summary

Write-Verbose "============ Summary ============"

Write-Verbose "New groups: $($NewGroups.Count)"
Write-Verbose "Modified groups: $($ModifiedGroups.Count)"
Write-Verbose "Unmodified groups: $($UnmodifiedGroups.Count)"

#Output current Groups info to XML for next run
#TODO - preserve last X copies of XML file as a backup for troubleshooting
try {
    Write-Verbose "Writing current groups info to XML for comparison on next run."
    $UnifiedGroups | Export-Clixml -Path $XMLFileName -ErrorAction STOP
}
catch {
    Write-Warning $_.Exception.Message
}

#...................................
# Validate SMTP Settings
#...................................

# If there's no SMTP Server specified, attempt to derive one from MX records
if ([string]::IsNullOrWhiteSpace($settings.EmailSettings.SmtpServer)) {
    Write-Verbose "No SMTP server was specified - deriving one from DNS"
    try {
        $recipientSmtpDomain = $settings.EmailSettings.To.Split("@")[1]
        $MX = Resolve-DnsName -Name $recipientSmtpDomain -Type MX | 
            Where-Object {$_.Type -eq "MX"} | 
            Sort-Object Preference | 
            Select-Object -First 1 -ExpandProperty NameExchange
        Write-Verbose "Found MX record: '$MX'"
        $smtpsettings.SmtpServer = $MX
    } catch {
        throw "Unable to resolve SMTP Server and none was specified.`n$($_.Exception.Message)"
    }
}

#...................................
# Build the report
#...................................

#HTML HEAD with styles
$htmlhead="<html>
			<style>
			BODY{font-family: Arial; font-size: 10pt;}
			H1{font-size: 22px;}
			H2{font-size: 18px; padding-top: 10px;}
			H3{font-size: 16px; padding-top: 8px;}
			TABLE{border: 1px solid black; border-collapse: collapse; font-size: 8pt; table-layout: fixed;}
            TABLE.testresults{width: 850px;}
            TABLE.summary{text-align: center; width: auto;}
			TH{border: 1px solid black; background: #dddddd; padding: 5px; color: #000000;}
            TH.summary{width: 80px;}
            TH.test{width: 120px;}
            TH.description{width: 150px;}
            TH.outcome{width: 50px}
            TH.comments{width: 120px;}
            TH.details{width: 270px;}
            TH.reference{width: 60px;}
			TD{border: 1px solid black; padding: 5px; vertical-align: top; }
			td.pass{background: #7FFF00;}
			td.warn{background: #FFE600;}
			td.fail{background: #FF0000; color: #ffffff;}
			td.info{background: #85D4FF;}
            ul{list-style: inside; padding-left: 0px;}
			</style>
			<body>"

#HTML intro
$IntroHtml = "<h1>Office 365 Groups Report</h1>
			<p><strong>Generated:</strong> $now</p>"

#HTML report body

#New Groups
$NewGroupsIntro = "<h2>New Office 365 Groups</h2>"
if ($($NewGroups.Count) -eq 0) {
    $NewGroupsIntro += "<p>No new Groups were found since the last report.</p>"
}
else {
    $NewGroupsIntro += "<p>New Groups found:</p>"
    $NewGroupsTable = $NewGroups | ConvertTo-Html -Fragment
}

#Modified Groups
$ModifiedGroupsIntro = "<h2>Modified Office 365 Groups</h2>"
if ($($ModifiedGroups.Count) -eq 0) {
    $ModifiedGroupsIntro += "<p>No modified Groups were found since the last report.</p>"
}
else {
    $ModifiedGroupsIntro += "<p>Modified Groups found:</p>"
    $ModifiedGroupsTable = $ModifiedGroups | ConvertTo-Html -Fragment
}

#Deleted Groups
$DeletedGroupsIntro = "<h2>Deleted Office 365 Groups</h2>"
if ($($DeletedGroups.Count) -eq 0) {
    $DeletedGroupsIntro += "<p>No deleted Groups were found since the last report.</p>"
}
else {
    $DeletedGroupsIntro += "<p>Deleted Groups found:</p>"
    $DeletedGroupsTable = $DeletedGroups | ConvertTo-Html -Fragment
}

#Unmodified Groups
$UnmodifiedGroupsIntro = "<h2>Unmodified Office 365 Groups</h2>"
if ($($UnmodifiedGroups.Count) -eq 0) {
    $UnmodifiedGroupsIntro += "<p>No unmodified Groups were found since the last report.</p>"
}
else {
    $UnmodifiedGroupsIntro += "<p>Unmodified Groups found:</p>"
    $UnmodifiedGroupsTable = $UnmodifiedGroups | ConvertTo-Html -Fragment
}

$ReportBodyHtml = $NewGroupsIntro + $NewGroupsTable + $ModifiedGroupsIntro + $ModifiedGroupsTable + $DeletedGroupsIntro + $DeletedGroupsTable + $UnmodifiedGroupsIntro + $UnmodifiedGroupsTable

#HTML TAIL
$htmltail = "<p>Report created by <a href=""http://practical365.com"">Practical365</a>.</p>
            </body>
			</html>"

$htmlreport = $htmlhead + $IntroHtml + $ReportBodyHtml + $htmltail

#TODO - Add option to output to HTML file

#TODO - Make this a parameter/switch
try {
    Send-MailMessage @smtpsettings -Body $htmlreport -BodyAsHtml -ErrorAction STOP
    Write-Verbose "Email report sent."
}
catch {
    Write-Warning $_.Exception.Message
    Write-Verbose "Email report not sent."
}

#...................................
# Finished
#...................................