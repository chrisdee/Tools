SharePoint Manager 2007

Created by
Carsten Keutmann
Copyright 2007

The SharePoint Manager 2007 (SPM2007)  
This version enables you to explorer and edit the data in the object model.
The application is not meant to be a windows form application version of the SharePoint 3.0 Central Administration, but to provide extra functionality that if only found in stsadm.exe or nowhere else. 

Note:
The SPM2007 application only uses the SharePoint Windows Services 3.0 object model. 
The application do not access the Sql Server directly or using other kind of hacks.

Warning!
This program enables you to update the data in SharePoint object model directly.
Therefore be very careful when changing the data.

How to install:
1.	Download the ZIP file and unpack on a SharePoint 2007 Server.*
2.	Run the SharePoint Manager 2007.exe program.

(*) This program has to run on a server where Microsoft SharePoint Services 3.0 or Microsoft Office SharePoint Server 2007 is installed.

NB! Be sure that you are the server and SharePoint administrator.


Prerequisites:
------------------
Microsoft Server 2003
Microsoft  .NET Framework 2.0
Microsoft SharePoint Services 3.0 


License:
------------------
Please read the License.txt file.

Ver: 0.9.9.0405
------------------
- PropertiesXmlForUncustomizedViews tab added for Views
- Minor bugfixes. Provided by Mads Nissen


Ver: 0.9.9.0316
------------------
- "BaseView Xml" Tab added on Views node.
- "WebPart Xml" Tab added on WebPart node.



Ver: 0.9.8.1002
------------------
- Feature collection now uses try catch on invalid features.
- Title used to show names of JobDefinitions.
- SPEventReceiverDefinition implemented. EventReceivers are now browsable.

Ver: 0.32.7.1212
------------------
- Folder node for every site added.
- Default view changed to Advanced.

Release - Features 
Ver: 0.32.7.0418
------------------
- Spanish localization added. (Thanks to Gustavo Velez)
- Minor bug fixes.


Ver: 0.31.7.0130
------------------
- QuotaTemplate nodes added.
- Language bug fixed.

Ver: 0.31.7.0127
------------------
- Workflow nodes added.
- View nodes added.
- CAML View of views added.
- Bug fix of SPWebApplication
- Minor bug fixes.


Ver: 0.30.6.1229
------------------
- View menu added. Minimal, Medium and Full view of the object model.
- Major bug fix on features handling. Now works properly.
- Minor bug fixes.

Ver: 0.29.6.1213
------------------
- Copy/paste of Site columns between Site Collections.
- New nodes types added.
- Bug fixed: Program not caching icons.


Ver: 0.28.6.1118
------------------
- Connect to SharePoint Configuration database.
- Versions of List items added.
- Download function of files in document librarys.
- RecycleBin nodes added.
- RecycleBin Restore function added.
- Refresh of data fixed.



Ver: 0.22.6.1103
------------------
- Edit data directly in manager.
- Cancel data modifications.
- Refresh of data.
- Restucture of treeview to match object model.
- Added properties collection.


November 1. 2006
------------------
- Explorer view
- Properties grid view
- Feature install/uninstall - Activate/deactivate


