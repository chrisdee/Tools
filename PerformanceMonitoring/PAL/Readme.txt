Project Description
Ever have a performance problem, but don't know what performance counters to collect or how to analyze them? The PAL (Performance Analysis of Logs) tool is a powerful tool that reads in a performance monitor counter log and analyzes it using known thresholds.
Features

    Thresholds files for most of the major Microsoft products such as IIS, MOSS, SQL Server, BizTalk, Exchange, and Active Directory.
    An easy to use GUI interface which makes creating batch files for the PAL.ps1 script.
    A GUI editor for creating or editing your own threshold files.
    Creates an HTML based report for ease of copy/pasting into other applications.
    Analyzes performance counter logs for thresholds using thresholds that change their criteria based on the computer's role or hardware specs.

To use PAL
The PAL tool is primarily a PowerShell script that requires arguments/parameters passed to it in order to properly analyze performance monitor logs.
Requirements
Operating Systems
The tool is tested only tested on Microsoft Windows 7 64-bit. If you encounter problems with the tool on other operating systems, then consider reusing the tool on Windows 7 64-bit.

Required Products (free and public):
- PowerShell v2.0 or greater.
- Microsoft .NET Framework 3.5 Service Pack 1
- Microsoft Chart Controls for Microsoft .NET Framework 3.5

Known Issues
PAL must be ran under an English-US locale until globalization can be added. We are looking for assistance from users like you to help contribute to this cause.

Download locations:

Microsoft .NET Framework 3.5 Service Pack 1 (Partial package - internet access required)
http://www.microsoft.com/downloads/details.aspx?familyid=AB99342F-5D1A-413D-8319-81DA479AB0D7&displaylang=en

Microsoft .NET Framework 3.5 Service Pack 1 (full package - no internet access required)
http://download.microsoft.com/download/2/0/e/20e90413-712f-438c-988e-fdaa79a8ac3d/dotnetfx35.exe

Microsoft Chart Controls for Microsoft .NET Framework 3.5
http://www.microsoft.com/downloads/details.aspx?FamilyID=130f7986-bf49-4fe5-9ca8-910ae6ea442c&DisplayLang=en

PowerShell v2.0 (Windows Management Framework (Windows PowerShell 2.0, WinRM 2.0, and BITS 4.0))
http://support.microsoft.com/kb/968929

Training
Download it (20071005IntrotoPALwmv.zip) from:
https://www.codeplex.com/Release/ProjectReleases.aspx?ProjectName=PAL&ReleaseId=6759

Related Blogs and Reviews
Clint Huffman's Windows Performance Analysis Blog
http://blogs.technet.com/clint_huffman

Mike Lagase's Exchange Performance Analysis Blog
http://blogs.technet.com/mikelag/archive/2008/08/20/performance-troubleshooting-using-the-pal-tool.aspx

Get a Handle on Windows Performance Analysis (Windows IT Pro Magazine)
http://windowsitpro.com/Windows/Articles/ArticleID/101162/pg/2/2.html

Two Exchange Server Tools You Should Know About
http://windowsitpro.com/article/articleid/100132/two-exchange-server-tools-you-should-know-about.html