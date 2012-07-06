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
 |---> readme.txt


SPSFarmReport.exe
=================
This is a 32-bit executable that relies on .NET Framework 3.0. This assembly can run on both x86 and x64 Windows operating systems where Windows SharePoint Services 3.0, Office SharePoint Server 2007, and/or Project Server 2007 is installed and configured using psconfig. It references the 12.0.0.0 microsoft.sharepoint.dll and at during runtime, loads the 12.0.0.0 microsoft.sharepoint.search.dll if osearch is installed and configured. 

2010SPSFR.exe
=============
This is a 64-bit executable that relies on .NET Framework 3.5. This assembly can only run on x64 Windows operating systems where SharePoint Foundation 2010, SharePoint Server 2010, and/or Project Server 2010 is installed and configured using psconfig. It references the 14.0.0.0 microsoft.sharepoint.dll and the powershell interface through system.management.automation.dll. It is recommended that you use IE to view the generated output and then optionally enable scripts.

-jvijayw