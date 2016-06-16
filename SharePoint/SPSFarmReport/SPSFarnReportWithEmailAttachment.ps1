## SharePoint Server: PowerShell Script To Automate Sending SPSFarm Reports Attached To An Email ##
## Overview: Useful script to send the SPSFarm Report Tools generated HTML documents as an email attachment

#Set your SPSFarmReport Application Locations

$spsFarmReport = "D:\Scripts\SPSFarmReport\o12" #Change this to suit your environment for '12' hive farms
#$spsFarmReport = "D:\Scripts\SPSFarmReport\o14" #Change this to suit your environment for '14' hive farms
#$spsFarmReport = "D:\Scripts\SPSFarmReport\o15" #Change this to suit your environment for '15' hive farms

cd $spsFarmReport

./SPSFarmReport.exe #For '12' hive farms
#./2010SPSFR.exe #For '14' hive farms
#./2013SPSFarmReport.ps1 #For '15' hive farms

$hostName = hostname
$file = "D:\Scripts\SPSFarmReport\o12\SPSFarmReport.html" #Change your SPSFarm report file location details here
$scheduledTask = "SPSFarmReport" #Change your scheduled task name details here
$smtpServer = "email.yourcompanyname.com" #Put your SMTP Server details here

$msg = new-object Net.Mail.MailMessage
$att = new-object Net.Mail.Attachment($file)
$smtp = new-object Net.Mail.SmtpClient($smtpServer)

$msg.From = "SharePointFarmReports@yourcompanyname.com" #Change the from address here
$msg.To.Add("your.name@yourcompanyname.com") #Change the to address here. Add additional recipients with a ',' after each other
$msg.Subject = "$hostName: $scheduledTask Results" #Change the subject here
$msg.Body = "The $scheduledTask scheduled task has run on $hostName, please view the attached report file for the results of this." #Change the body here
$msg.Attachments.Add($att)

$smtp.Send($msg)

$att.Dispose()
