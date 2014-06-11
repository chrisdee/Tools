SPSFarmReport.zip Contents
===========================


[SPSFarmReport]
 |
 |
 |     ------
 |----/ o12  \
 |    \______/
 |       |
 |       |-------> SPSFarmReport.exe
 |
 |
 |     ------
 |----/ o14  \
 |    \______/
 |       |
 |       |-------> 2010SPSFR.exe
 |
 |
 |     ------
 |----/ o15  \
 |    \______/
 |       |
 |       |-------> 2013SPSFarmReport.ps1 
 |       |
 |       |
 |       |-------> SPSFarmReport.xslt
 |
 |
 |
 |---> readme.txt


SPSFarmReport.exe
=================
This is a 32-bit executable that relies on .NET Framework 3.0. This assembly can run on both x86 and x64 Windows operating systems where Windows SharePoint Services 3.0, Office SharePoint Server 2007, and/or Project Server 2007 is installed and configured using psconfig. It references the 12.0.0.0 microsoft.sharepoint.dll and at during runtime, loads the 12.0.0.0 microsoft.sharepoint.search.dll if osearch is installed and configured. 

2010SPSFR.exe
=============
This is a 64-bit executable that relies on .NET Framework 3.5. This assembly can only run on x64 Windows operating systems where SharePoint Foundation 2010, SharePoint Server 2010, and/or Project Server 2010 is installed and configured using psconfig. It references the 14.0.0.0 microsoft.sharepoint.dll and the powershell interface through system.management.automation.dll. It is recommended that you use IE to view the generated output and then optionally enable scripts.

2013SPSFarmReport.ps1
=====================
This one is a powershell'd version that works on 2013 farm environments. You will need to run "Set-ExecutionPolicy -ExecutionPolicy Unrestricted" to allow the execution of this PowerShell script. The ouput is written in the form of an XML file. Run the "[Environment]::CurrentDirectory" command to know where the output XML is written to. 

SPSFarmReport.xslt
==================
This is a stylesheet you use to view the contents of the 2013 SPSFarmReport XML file. Be sure to have this file in the same folder as the output XML.

-jvijayw