Description
SPSWakeUp is a PowerShell script tool to warm up all site collection in your SharePoint environment.
It's compatible with all supported versions for SharePoint.
This current version supports only NTLM Authentication.

Features
1.5 Email notifications, HttpWebRequest, Disable Internet Explorer ESC,
1.4 Can exclude Site Collection Url(s) with xml input file
1.3 Can exclude Central Administration with xml input file
1.2 Add User Account in User Policy for Each Web Application
1.1 Clear Cache Internet Explorer, Browsing Custom Urls, Input XML File
Supports SharePoint 2007, SharePoint 2010 and 2013
Warms Up Central Administration and All site collection by Web Application
Add Web Application Urls in Internet Options Security
Use Internet Explorer to download JS, CSS and Pictures files
Log script results in rtf file

Step by Step Guide

1. Download SPSWakeUP.zip

2. Unzip and copy the files on each Web Front-End

3. Modify input xml file for Custom Urls, Exclude Urls and Configuration

    <!--- This CustomUrls section add urls to be warming up. -->
    <CustomUrls>
      <CustomUrl>http://intranet/SitePages/utilities.aspx</CustomUrl>
      <CustomUrl>http://intranet/SitePages/pagenotfound.aspx</CustomUrl>
      <CustomUrl>[ADD URL HERE]</CustomUrl>
    </CustomUrls>

  <!--- This ExcludeUrls section remove site collection urls to be warming up.-->
  <ExcludeUrls>
    <ExcludeUrl>http://intranet/search</ExcludeUrl>
    <ExcludeUrl>http://intranet/sites/Admin</ExcludeUrl>
    <ExcludeUrl>[ADD URL HERE]</ExcludeUrl>
  </ExcludeUrls>
4. Create Task Scheduler item with program PowerShell.exe :

<Command>C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe</Command>
<Arguments>-Command Start-Process "$PSHOME\powershell.exe" -Verb RunAs -ArgumentList "'-ExecutionPolicy Bypass E:\SCRIPT\SPSWakeUP.ps1 E:\SCRIPT\SPSWakeUP.xml'"</Arguments>