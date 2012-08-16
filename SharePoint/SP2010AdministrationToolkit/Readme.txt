SharePoint 2010 Administration Toolkit (SharePoint Server 2010)

Published: July 15, 2010

This article provides information for farm administrators about the April 2011 release of the SharePoint Administration Toolkit version 2.0, which includes the following features:

The Load Testing Kit, which generates a Visual Studio Team System 2008 (VSTS) load test based on Microsoft Office SharePoint Server 2007 IIS logs. The VSTS load test can be used to generate synthetic load against Microsoft SharePoint Server 2010 as part of a capacity planning exercise or a pre-upgrade stress test.

The Security Configuration Wizard (SCW) manifest, which add roles for SharePoint 2010 Products to Windows Server 2008 with Service Pack 2 or to Windows Server 2008 R2.

The User Profile Replication Engine, which provides a shared services administrator the ability to replicate user profiles and social data between shared services providers (SSP) in Office SharePoint Server 2007 and User Profile service applications in SharePoint Server 2010.

Note:
This tool is not supported for SharePoint Foundation 2010.
The Content Management Interoperability Services (CMIS) connector for Microsoft SharePoint Server 2010, which enables SharePoint users to interact with content stored in any repository that has implemented the CMIS standard, as well as making SharePoint 2010 content available to any application that has implemented the CMIS standard.

Note:
This is not supported for SharePoint Foundation 2010.
New in SharePoint Administration Toolkit 2.0, the SharePoint Diagnostic Studio 2010 (SPDiag 3.0) provides SharePoint administrators with a unified interface that can be used to gather relevant information from a farm, display the results in a meaningful way, identify performance issues, and share or export the collected data and reports for analysis by Microsoft support personnel.

When you plan to install the entire toolkit, the SharePoint Administration Toolkit installer supports the use of a quiet installation using the /quiet switch. However, to script partial installations, the following command must be used to extract the Spat.msi from the SharePoint2010AdministrationToolkit.exe file:

SharePoint2010AdministrationToolkit.exe /extract:<path>

where path is the location of where the extracted files will reside.

The installation package is customizable and selectable, meaning that a user can control which tools are installed or uninstalled and can specify a custom installation folder for any tool.

To download the toolkit, see Microsoft SharePoint 2010 Administration Toolkit (http://go.microsoft.com/fwlink/p/?LinkId=196866).