#==================================================================================================================
#  http://comb.codeplex.com/
#==================================================================================================================
#  Filename:        EventComb.ps1
#  Author:          Jeff Jones
#  Version:         1.3
#  Last Modified:   01-14-2015
#  Description:     Gather eventlogs from servers into a daily summary email with attached CSV detail.
#                   Helps administrators be proactive with issue resolution by better understanding internal 
#                   server health.  Open with Microsoft Excel for PivotTable, PivotCharts, and further analysis.
#
#                   NEW - Now includes SharePoint farm level detail (Build, AD, Search, WSP, etc.)
#
#                   NOTE - Please adjust lines 25-35 for your environment.
#
#                   Comments and suggestions always welcome!  spjeff@spjeff.com or @spjeff
#
#==================================================================================================================

param (
	[switch]$install
)

# Configuration
$global:configHours = -24											# time threshold (previous day)
$global:configMaxEvents = 4999										# maximum number of events from any 1 server
$global:configIsSharePoint = $true									# include SharePoint farm detail
$global:configTargetMachines = @("spautodetect")					# target servers. use = @("spautodetect") for SharePoint farm auto detection
$global:configSendMailTo = @("admin1@demo.com","admin2@demo.com")	# to address
$global:configSendMailFrom = "no-reply@domain.com"					# from address
$global:configSendMailHost = "mailrelay"							# outbound SMTP mail server
$global:configWarnDisk = 0.20										# threshold for warning (15%)
$global:configErrorDisk = 0.10										# threshold for warning (10%)
$global:configExcludeMaintenanceHours = @(21,22,23,0,1,2,3)			# exclude 11PM-7AM nightly maintenance window
$global:configExcludeEventSources = @("Schannel~36888","Schannel~36874","McAfee PortalShield~2053") # exclude known event sources

Function Installer() {
	# Add to Task Scheduler
	Write-Host "  Installing to Task Scheduler..." -ForegroundColor Green
	$user = $ENV:USERDOMAIN+"\"+$ENV:USERNAME
	Write-Host "  Current User: $user"
	
	# Attempt to detect password from IIS Pool (if current user is local admin & farm account)
	$appPools = gwmi -namespace "root\MicrosoftIISV2" -class "IIsApplicationPoolSetting" | select WAMUserName, WAMUserPass
	foreach ($pool in $appPools) {			
		if ($pool.WAMUserName -like $user) {
			$pass = $pool.WAMUserPass
			if ($pass) {
				break
			}
		}
	}
	
	# Manual input if auto detect failed
	if (!$pass) {
		$response = Read-host "Enter password for $user " -AsSecureString 
		$pass = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($response))
	}
	
	# Create Task
	schtasks /create /tn "EventComb" /ru $user /rp $pass /rl highest /sc daily /st 03:00 /tr "PowerShell.exe -ExecutionPolicy Bypass $global:path"
	Write-Host "  [OK]" -ForegroundColor Green
	Write-Host
}

Function BuildDescription($build) {
	switch ($build) {
		# Build numbers from http://toddklindt.com/sp2013builds
		"15.0.4128.1014" {return "Beta"; break;}
		"15.0.4420.1017" {return "RTM"; break;}
		"15.0.4433.1506" {return "December 2012 Hotfix"; break;}
		"15.0.4481.1005" {return "March 2013 Public Update"; break;}
		"15.0.4505.1002" {return "April 2013 CU"; break;}
		"15.0.4505.1005" {return "​April 2013 CU"; break;}
		"15.0.4517.1003" {return "June 2013 CU"; break;}
		"15.0.4535.1000" {return "August 2013 CU"; break;}
		"15.0.4551.1001" {return "October 2013 CU"; break;}
		"15.0.4551.1005" {return "October 2013 CU"; break;}
		"15.0.4551.1508" {return "December 2013 CU"; break;}
		"15.0.4551.1511" {return "December 2013 CU"; break;}
		"15.0.4569.1000" {return "Service Pack 1"; break;}
		"​​15.0.4605.1000" {return "April 2014 CU"; break;}
		"15.0.4615.1001" {return "MS14-022"; break;}
		"15.0.4617.1000" {return "June 2014 CU"; break;}
		"15.0.4631.1001" {return "July 2014 CU"; break;}
		"15.0.4641.1001" {return "August 2014 CU"; break;}
		"15.0.4649.1001" {return "September 2014 CU"; break;}
		"15.0.4659.1001" {return "October 2014 CU"; break;}
		"15.0.4667.1000" {return "November 2014 CU"; break;}
		"15.0.4667.1000" {return "November 2014 CU"; break;}
		"15.0.4675.1000" {return "December 2014 CU"; break;}
		default {return "Unknown"}
	}
}

Function EventComb() {
	# Auto detect on SharePoint farms
	if ($global:configTargetMachines -eq @("spautodetect")) {
		Add-PSSnapin Microsoft.SharePoint.PowerShell -ErrorAction SilentlyContinue
		$global:configTargetMachines = @()
		foreach ($s in ((Get-SPFarm).Servers |? {$_.Role -ne "Invalid"} )) {
			$global:configTargetMachines += $s.Address
		}
	}
	
	# Initialize
	$xml = New-Object -TypeName PSObject
	$start = Get-Date
	$logAfter = (Get-Date).AddHours($global:configHours)
	Write-Host ("{0} machine(s) targeted" -f $global:configTargetMachines.Count)
	$csv = @()
	$xml | Add-Member -MemberType NoteProperty -Name "configTargetMachines" -Value $global:configTargetMachines

	# Loop for all machines
	foreach ($machine in $global:configTargetMachines) {
		foreach ($log in @("Application", "System")) {
			 Write-Host ("Gathering log {0} for {1} ... " -f $log, $machine) -NoNewline
			 # Gather event log detail
			 foreach ($type in @("Error","Warning")) {
				 $events = Get-EventLog -ComputerName $machine -Logname $log -After $logAfter -EntryType $type -Newest $global:configMaxEvents
				 if ($events) {
					foreach ($e in $events) {
						$keep = $true
						# Exclude based on ID and Source
						foreach ($skip in $global:configExcludeEventSources) {
							if ($e.Source -eq  $skip.Split("~")[0] -and $e.EventID -eq $skip.Split("~")[1]) {
								$keep = $false
							}
						}
						# Exclude based on maintenance hours
						foreach ($hour in $global:configExcludeMaintenanceHours) {
							if ($e.TimeWritten.Hour -eq $hour) {
								$keep = $false
							}
						}
						# Append to CSV
						if ($keep) {
							$csv += $e
						}
					 }
				 }
			 }
			 Write-Host "[OK]" -ForegroundColor Green
		}
	}
	
	# Write CSV file
	Write-Host "Writing CSV file ..." -NoNewline
	$today = (Get-Date).ToString("yyyy-MM-dd")
	$farm = "$env:spfarm-$today"
	$csv | Export-Csv -Path "EventComb-$farm.csv" -NoTypeInformation -Force
	Write-Host "[OK]" -ForegroundColor Green
	
	# Format HTML summary
	$totalErr = 0
	$totalWarn = 0
	$html = ("The below table summaries the eventlog entries of the last {0} hours on these machines:<br><br><table>" -f $global:configHours)
	$html += "<tr><td>&nbsp;</td><td width='20px'>&nbsp;</td><td><b>Error</b></td><td></td><td><b>Warn</b></td></tr>"
	$coll = @()
	foreach ($machine in $global:configTargetMachines) {
		# Summary total for Errors
		$countErr = 0
		$logErr = ($csv |? {$_.EntryType -eq "Error" -and $_.MachineName -like "$machine*"})
		if ($logErr) {
			$countErr = $logErr.Count
			$totalErr += $countErr
		}
		# Summary total for Warnings
		$countWarn = 0
		$logWarn = ($csv |? {$_.EntryType -eq "Warning" -and $_.MachineName -like "$machine*"})
		if ($logWarn) {
			$countWarn = $logWarn.Count
			$totalWarn += $countWarn
		}
		$coll += @($machine,$countErr,$countWarn)
		$html += ("<tr><td>{0}</td><td>&nbsp;</td><td style='background-color: #FF9D9D;'>{1}</td><td style='color: #FFFFFF'>-</td><td style='background-color: #FFFF6C;'>{2}</td></tr>" -f $machine, $countErr, $countWarn)
	}
	$html += ("<tr><td>&nbsp;</td><td width='20px'>&nbsp;</td><td>{0}</td><td style='color: #FFFFFF'>-</td><td>{1}</td></tr>" -f $totalErr, $totalWarn)
	$html += "</table>"

	$xml | Add-Member -MemberType NoteProperty -Name "machineErrWarn" -Value $coll
	$xml | Add-Member -MemberType NoteProperty -Name "totalErr" -Value $totalErr
	$xml | Add-Member -MemberType NoteProperty -Name "totalWarn" -Value $totalWarn
	
	# Format HTML pivot tables
	Write-Host "Pivot tables ... " -NoNewline
	$html += "<br><br><table><tr><td colspan=3><b>Source Pivots</b></td></tr>"
	$groups = $csv | group Source -NoElement | sort Count -Descending
	foreach ($g in $groups) {
		$html += "<tr><td> " + $g.Name + " </td><td style='color: #FFFFFF'>-</td><td style='background-color: #99FF99;'> " + $g.Count + " </td></tr>";
	}
	$html += "</table>"
	$html += "<br><br><table><tr><td colspan=3><b>EventID Pivots</b></td></tr>"
	$groups = $csv | group EventID -NoElement | sort Count -Descending
	foreach ($g in $groups) {
		$html += "<tr><td> " + $g.Name + " </td><td style='color: #FFFFFF'>-</td><td style='background-color: #99CCFF;'> " + $g.Count + " </td></tr>";
	}
	$html += "</table>"
	Write-Host "[OK]" -ForegroundColor Green

	# Free disk space
	$coll=@()
	$html += "<br><br><table><tr><td colspan=3><b>Free Disk</b></td></tr>"
	foreach ($machine in $global:configTargetMachines) {
		$html += "<tr><td valign='top'>$machine</td></tr>"
		$wql = "SELECT Size, FreeSpace, Name, FileSystem FROM Win32_LogicalDisk WHERE DriveType = 3"
		$wmi = Get-WmiObject -ComputerName $machine -Query $wql
		foreach ($w in $wmi) {
			$color = ""
			$note = ""
			$letter = $w.Name
			$freeSpace = ($w.FreeSpace / 1GB)
			$prctFree = ($w.FreeSpace / $w.Size)
			if ($prctFree -lt $global:configWarnDisk) {
				$color = "yellow"
			}
			if ($prctFree -lt $global:configErrorDisk) {
				$color = "lightred"
				$note = "*"
			}
			$html += ("<tr><td></td><td>$letter</td><td style='background-color: $color;'>&nbsp;{0:N1} GB ({1:P0}) $note&nbsp;</td></tr>" -f $freeSpace, $prctFree)
			$coll += @($machine,$letter,($w.Size/1GB),$freeSpace,$prctFree)
		}
	}
	$xml | Add-Member -MemberType NoteProperty -Name "machineFreeDisk" -Value $coll
	$html += "</table>"
	
	if ($global:configIsSharePoint) {
		# SharePoint farm build
		Write-Host "SharePoint farm ... " -NoNewline
		$f=Get-SPFarm;$p=Get-SPProduct;$p.PatchableUnitDisplayNames |% {$n=$_;$v=($p.GetPatchableUnitInfoByDisplayName($n).patches | sort version -desc)[0].version;if (!$maxv) {$maxv=$v};if ($v -gt $maxv) {$max=$v}};$obj=New-Object -TypeName PSObject -Prop (@{'FarmName'=$f.Name;'FarmBuild'=$f.BuildVersion;'Product'=$max;});
		
		#Display
		$max = $obj.FarmBuild
		if ($obj.Product -gt $obj.FarmBuild) {$max = $obj.Product}
		$html += "<p><b>Farm Patch Level</b></p><table>"
		$html += "<tr>Build</td><td>$($obj.FarmBuild)</tr>"
		$html += "<tr>Product</td><td>$($obj.Product)</tr>"
		$html += "<tr><td>CU</td><td style='background-color:yellow'>$(BuildDescription($max))</td></tr></table>"
		$xml | Add-Member -MemberType NoteProperty -Name "FarmBuild" -Value $obj.FarmBuild
		$xml | Add-Member -MemberType NoteProperty -Name "FarmProduct" -Value $obj.Product
		$xml | Add-Member -MemberType NoteProperty -Name "FarmBuildDescription" -Value $(BuildDescription($max))
		
		# Health Rules
		$coll=@(); $wa=Get-SPWebApplication -IncludeCentralAdministration |? {$_.IsAdministrationWebApplication -eq $true}; $ca=get-spweb $wa.Url; $c=0; $ca.Lists["Review problems and solutions"].Items |% {if ($_["Severity"] -ne "4 - Success") {$coll += $_}}; 
		
		#Display
		$html += "<p><b>Health Rules ({0})</b></p><ul>" -f $coll.Count
		$coll |% {$html += "<li>$($_.Title)</li>"}
		$html += "</ul>"
		#REM $xml | Add-Member -MemberType NoteProperty -Name "HealthRules" -Value $coll
		
		# User Profile Services - "Started"
		$upsfarm = Get-SPServiceInstance |? {($_.TypeName -eq "User Profile Synchronization Service" -or $_.TypeName -eq "User Profile Service") -and $_.Status -eq "Online"} | Select TypeName, @{n="Server";e={$_.Server.Address}}, Status
		
		if ($upsfarm) {
			# UPS Count
			$wa = (Get-SPWebApplication)[0]
			$site = Get-SPSite $wa.Url
			$context = Get-SPServiceContext $site
			$profileManager = New-Object Microsoft.Office.Server.UserProfiles.UserProfileManager($context)
			$upscount = $profileManager.Count
			
			# Display
			$html += "<p><b>User Profile</b></p><table>"
			$html += "<tr><td>Service</td><td>Machine</td><td>Status</td><td>Profile Count</td></tr>"
			$upsfarm |% {$color="yellow";if ($_.Status -eq "Online") {$color="lightgreen"};$html += ("<tr><td>{0}</td><td>{1}</td><td style='background-color:$color'>{2}</td><td>{3:n0}</td></tr>" -f $_.TypeName, $_.Server, $_.Status, $upscount)}
			$html += "</table>"
			$xml | Add-Member -MemberType NoteProperty -Name "UserProfileService" -Value $upsfarm
		} else {
			# Not found
			$html += "<p><b>User Profile</b></p><table>"
			$html += "<tr><td style='background-color:yellow'>Not Found</td></tr></table>"
		}

		# Distributed Cache - UP/DOWN and # MB and # items for all nodes
		Use-CacheCluster
		$dcstat = Get-AFCacheHostStatus | % {$ServerName = $_.HostName; $status = (Get-CacheHost |? {$_.HostName -eq $ServerName}).Status; Get-AFCacheStatistics -ComputerName $_.HostName -CachePort $_.PortNo | Add-Member -MemberType NoteProperty -Name 'ServerName' -Value $ServerName -PassThru | Add-Member -MemberType NoteProperty -Name 'Status' -Value $status -PassThru} | Sort Server
		
		#Display
		$html += "<p><b>Distributed Cache</b></p><table>"
		$html += "<tr><td>Machine</td><td>Status</td><td>Size</td><td>ItemCount</td><td>RegionCount</td><td>NamedCacheCount</td><td>RequestCount</td><td>MissCount</td></tr>"
		$dcstat |% {$color="lightgreen";if ($_.Status.ToString().ToUpper() -ne "UP"){$color="red"};$html += ("<tr><td>{0}</td><td style='background-color:$color'>[{1}]</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td></tr>" -f $_.ServerName,$_.Status.ToString().ToUpper(),$_.Size,$_.ItemCount,$_.RegionCount,$_.NamedCacheCount,$_.RequestCount,$_.MissCount)}
		$html += "</table>"
		$xml | Add-Member -MemberType NoteProperty -Name "DistributedCache" -Value $dcstat
		
		# Account expiration and lockout
		$coll = Get-SPManagedAccount |% {$login=($_.UserName.Split('\')[1]); $u=Get-ADUser $login -Properties *; $_} | Select UserName, @{n='AccountExpirationDate';e={$u.AccountExpirationDate}},@{n='LockedOut';e={$u.LockedOut}},@{n='PasswordExpired';e={$u.PasswordExpired}},@{n='PasswordNeverExpires';e={$u.PasswordNeverExpires}},@{n='PasswordLastSet';e={$u.PasswordLastSet}},@{n='LastBadPasswordAttempt';e={$u.LastBadPasswordAttempt}},@{n='LastLogonDate';e={$u.LastLogonDate}} | Sort UserName
		
		# Display
		$html += "<p><b>AD Service Accounts ({0})</b></p><table>" -f $coll.Count
		$html += "<tr><td>UserName</td><td>LockedOut</td><td>PasswordExpired</td><td>AccountExpirationDate</td><td>PasswordNeverExpires</td><td>PasswordLastSet</td><td>LastBadPasswordAttempt</td><td>LastLogonDate</td></tr>"
		$coll |% {$colorlock='';$colorexp='';if ($_.LockedOut.ToString() -eq 'True'){$colorlock='yellow'};if($_.PasswordExpired.ToString() -eq 'True'){$colorexp='yellow'};$html += ("<tr><td>{0}</td><td style='background-color:$colorlock'>{1}</td><td style='background-color:$colorexp'>{2}</td><td>{3}</td><td>{4}</td></tr>" -f $_.UserName, $_.LockedOut, $_.PasswordExpired, $_.AccountExpirationDate, $_.PasswordNeverExpires, $_.PasswordLastSet, $_.LastBadPasswordAttempt, $_.LastLogonDate)}
		$html += "</table>"
		$xml | Add-Member -MemberType NoteProperty -Name "ActiveDirectory" -Value $coll
		
		# Search Topology
		$ssa = Get-SPEnterpriseSearchServiceApplication
		$t=Get-SPEnterpriseSearchTopology -SearchApplication $ssa -Active
		$c=Get-SPEnterpriseSearchComponent -SearchTopology $t
		$coll = $c | Select ServerName,Name | Sort Name
		$html += "<p><b>Search Topology ({0})</b></p><table>" -f $coll.Count
		$html += "<tr><td>Machine</td><td>Role</td></tr>"
		$coll |% {$html += "<tr><td>{0}</td><td>{1}</td></tr>" -f $_.ServerName, $_.Name.Replace("Component","")}
		$html += "</table>"
		$xml | Add-Member -MemberType NoteProperty -Name "SearchTopology" -Value $coll

		$sta = Get-SPEnterpriseSearchStatus -SearchApplication $ssa
		$coll = $sta | Select Name,State | Sort Name
		$html += "<p><b>Search Components ({0})</b></p><table>" -f $coll.Count
		$html += "<tr><td>Component</td><td>State</td></tr>"
		$coll |% {$color=''; if($_.State -ne 'Active'){$color='yellow';};$html += "<tr><td>{0}</td><td style='background-color:$color'>{1}</td></tr>" -f $_.Name.Replace("Component",""), $_.State}
		$html += "</table>"
		$xml | Add-Member -MemberType NoteProperty -Name "SearchStatus" -Value $coll
		
		# WSP custom solution
		$coll = @()
		$wsp = Get-SPSolution
		$html += "<p><b>WSP Solutions ({0})</b></p><table>" -f $wsp.Count
		$html += "<tr><td>Name</td><td>DeploymentState</td><td>LastOperationEndTime</td><td>DeployedWebApplications</td></tr>";
		$wsp | Sort LastOperationEndTime -Descending |% {$w=$_;$name=$w.Name;$ds=$w.DeploymentState;$lo=$w.LastOperationEndTime;$color="";if (!$_.DeploymentState.ToString().Contains("Deployed")){$color="yellow"};$waurls="";$_.DeployedWebApplications |% {$waurls += ($_.Url+", ");};$coll += (New-Object -TypeName PSObject -Prop (@{"Name"=$name;"DeploymentState"=$ds;"LastOperationEndTime"=$lo;"WAUrls"=$waurls}));$html += ("<tr><td>{0}</td><td style='background-color:$color'>{1}</td><td>{2}</td><td>{3}</td></tr>" -f $_.Name,$_.DeploymentState,$_.LastOperationEndTime,$waurls)};
		$html += "</table>"
		$xml | Add-Member -MemberType NoteProperty -Name "WSPSolution" -Value $coll
		
		# User Content
		$sites = Get-SPSite -Limit All
		$gb = [Math]::Round((($sites.Usage.Storage | Measure -Sum).Sum/1GB),2)
		$sc = $sites.Count
		$dc = (Get-SPContentDatabase).Count
		$html += "<p><b>User Content</p><table>" -f $wsp.Count
		$html += "<tr><td>Sites</td><td>$sc</td></tr>"
		$html += "<tr><td>GB</td><td>$gb</td></tr>"
		$html += "<tr><td>Databases</td><td>$dc</td></tr>"
		$html += "</table>"
		$xml | Add-Member -MemberType NoteProperty -Name "ContentSites" -Value $sc
		$xml | Add-Member -MemberType NoteProperty -Name "ContentGB" -Value $gb
		$xml | Add-Member -MemberType NoteProperty -Name "ContentDB" -Value $dn
		
		# complete
		Write-Host "[OK]" -ForegroundColor Green
	}
	
	# Send email summary with CSV attachment
	$html += "<p>=== FROM " + $env:computername + " ===</p>"
	$xml | Export-Clixml "EventComb-$farm.xml"
	Copy-Item "EventComb-$farm.csv" "EventComb-$farm.txt" -Force
	$total = ($totalErr  + $totalWarn)
	Send-MailMessage -To $global:configSendMailTo -From $global:configSendMailFrom -Subject "$farm EventComb - $total" -BodyAsHtml -Body $html -Attachments @("EventComb-$farm.csv","EventComb-$farm.txt","EventComb-$farm.xml") -SmtpServer $global:configSendMailHost
	Write-Host ("Operation completed successfully in {0} seconds" -f ((Get-Date) - $start).Seconds)
	Remove-Item "EventComb-$farm.csv"
	Remove-Item "EventComb-$farm.txt"
	Remove-Item "EventComb-$farm.xml"
}

#Main
Write-Host "EventComb v1.2  (last updated 11-20-2014)`n"

#Check Permission Level
If (-NOT ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole(`
	[Security.Principal.WindowsBuiltInRole] "Administrator"))
{
	Write-Warning "You do not have Administrator rights to run this script!`nPlease re-run this script as an Administrator!"
	Break
} else {
	#EventComb
	$global:path = $MyInvocation.MyCommand.Path
	$tasks = schtasks /query /fo csv | ConvertFrom-Csv
	$spb = $tasks | Where-Object {$_.TaskName -eq "\EventComb"}
	if (!$spb -and !$install) {
		Write-Host "Tip: to install on Task Scheduler run the command ""EventComb.ps1 -install""" -ForegroundColor Yellow
	}
	if ($install) {
		Installer
	}
	EventComb
}