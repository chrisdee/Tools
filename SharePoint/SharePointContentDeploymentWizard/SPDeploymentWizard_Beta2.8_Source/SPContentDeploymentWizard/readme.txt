** Updated for Release 2.8 (beta) 22 Mar 2010 - scroll down for notes on this release **
------------------------------------------------------------------------------------------

Version History
---------------------------------

Beta 1 - 05 December 2007
Beta 2 - 28 February 2008
Release 1.0 - 22 June 2008
Release 1.1 - 22 September 2008
Release 2.0 (beta) - 25 March 2009
Release 2.5 (beta) - 16 August 2009
Release 2.6 (beta) - 29 August 2009
Release 2.7 (beta) - 29 August 2009
Release 2.8 (beta) - 22 Mar 2010

---------------------------------

The Content Deployment Wizard can be used to to deploy the following SharePoint content:

- site collections
- webs
- lists
- list items (including files)


USAGE NOTES
------------

Site URL - enter using format http://source.test.dev
Import web URL - enter using format http://target.com/targetweb


IMPORT/EXPORT CHARACTERISTICS
------------

Since the Content Migration API is used, imports/exports have the following characteristics:

- dependencies of selected content (e.g. referenced CSS files, master pages) are automatically included in the 
	export - check 'Exclude dependencies of selected objects' to disable this
- all required content types, columns etc. are automatically included in the export
- in contrast to STSADM export, it is possible to retain GUIDs during deployment (where objects are not 
	being reparented) - check 'Retain object IDs and locations' to enable this

- no filesystem files (assemblies, SharePoint Solutions/Features etc.) are deployed - these must already be 
	present on the target for the import to succeed)
- the following content does not get captured by the Content Migration API - alerts, audit trail, change log 
	history, recycle-bin items, workflow tasks/state


REPARENTING
------------

Reparenting of objects such as webs and lists is possible with the following usage on the import:

- enter the URL for the target web in the 'Import web URL' textbox
- ensure 'Retain object IDs and locations' is not checked


PERMISSIONS
------------

The Content Migration API (PRIME) is used to package the content as a .cmp file, which can be copied to another 
server for import. The application MUST be run from the SharePoint server(s), and MUST be run under the context 
of an account which has appropriate SharePoint permissions - currently the way to specify an alternative user 
to the currently-logged on user is by using the 'Run as..' feature in Windows 2003 (right-click on the .exe and 
select 'Run as..').

RELEASE NOTES - BETA 2
-------------

Several of the performance issues of the beta 1 release have now been resolved. This release also adds:

- better handling of large sites
- support for WSS-only
- no separate WizardBase.dll
- automatic opening of log files at end of operation
- auto-discovery of sites in 'Site URL' textbox
- logging via System.Diagnostics framework - add a trace switch for 'COB.SharePoint.Utilities.ContentDeploymentWizard'
- icons in treeview

RELEASE NOTES - RELEASE 1.0
-------------

This release adds only minor changes to beta 2, since this codebase has now been proven to be stable. Additions:

- ability to modify which lists exist on your site but are not shown in the treeview. Generally only 'system'-type lists 
	are hidden, but on occasion you may want to 'unhide' some lists e.g. 'Variation Labels' and 'Relationships List' lists if you are using 
	variations. To do this add an appSettings key in the application config file named 'ListsNotForExport' and specify a comma-separated 
	list of the lists you wish to hide. The default value (if no override found in config) to use as your starting point is: 	
	
	Cache Profiles, Content and Structure Reports, Converted Forms, Long Running Operation Status, Notification List, Quick Deploy Items, 
	Relationships List, Reporting Metadata, Reporting Templates, User Information List, Variation Labels,Workflows, Workflow Tasks, 
	Workflow History, fpdatasources
    
- minor bug fixes e.g. .cmp file now correctly saved with extension if a period (.) is present in your chosen filename, validation to 
	prevent child objects being added from treeview when parent site collection already added

RELEASE NOTES - RELEASE 1.1 
-------------

This release is primarily for the public release of the source code. Code has been tidied and one bug fixed:

- running the tool with insufficient SharePoint permissions is now handled gracefully - the user will be shown a message which 
	suggests they use 'Run as', where it would previously fail with an unhandled exception

RELEASE NOTES - RELEASE 2.0 (beta) - 
-------------

This release provides command-line support, the ability to load/save settings and an installer. 

	[ALL INSTALLED FILES CAN BE FOUND UNDER <install directory>\Resources IF REQUIRED - 
	BY DEFAULT THIS IS Program Files\Chris O'Brien\SharePoint Content Deployment Wizard]

Known issues with this release:

- installer expects default installation location for SharePoint i.e. C:\Program Files\Common Files\microsoft shared\Web Server Extensions\12\.
 This is used to drop a file into the 12\Config directory for the custom STSADM command. Unfortunately I've not been able to 
 work out how to detect and drop path into correct location, so you'll need to move the file manually post-install if using a different path.
- lack of testing - sorry, this is where I need help from the community!

RELEASE NOTES - RELEASE 2.5 (beta)
-------------

This release provides:

- support for incremental deployments (select 'ExportChanges' from the dropdown)
- support for "no compression" deployments via the 'Disable compression' checkbox
- support for allowing the root web to be deployed on it's own - extra options in context menus to allow selection of 'site' or 'root web' 

	[ALL INSTALLED FILES CAN BE FOUND UNDER <install directory>\Resources IF REQUIRED - 
	BY DEFAULT THIS IS Program Files\Chris O'Brien\SharePoint Content Deployment Wizard]

Known issues with this release:

- installer expects default installation location for SharePoint i.e. C:\Program Files\Common Files\microsoft shared\Web Server Extensions\12\.
 This is used to drop a file into the 12\Config directory for the custom STSADM command. Unfortunately I've not been able to 
 work out how to detect and drop path into correct location, so you'll need to move the file manually post-install if using a different path.
- as above, lack of testing - sorry, this is where I need help from the community!

RELEASE NOTES - RELEASE 2.6 (beta)
-------------

This release provides:

- Fix to issue where lists cannot be exported with 'All descendents' option because menu item is missing
- Fix to issue on 64-bit VMs where STSADMCOMMANDS.COB.SPDEPLOYMENTWIZARD.XML file does not get installed to 12\config directory 

RELEASE NOTES - RELEASE 2.7 (beta)
-------------

This release provides:

- Ability to export/import list items between different lists, so long as list fields are the same (e.g. same content type). This can 
be thought of as "re-parenting" list items. Previously it was only possible to import to the same named list e.g. 
our.site.dev/Lists/Announcents/ - > our.site.test/Lists/Announcents/. Thanks to Alex Angas for submitting this patch.
- Fix to issue where "no compression" checkbox did not work properly (and added info note on export page) 

RELEASE NOTES - RELEASE 2.8 (beta) - (this release)
-------------

This release provides:

- Fix to threading issue introduced in previous release - caused problems typically on importing. 


KNOWN ISSUES (GENERAL)
------------

- export/import issues are quite often due to a problem with the underlying SharePoint API rather than code in the Content Deployment Wizard. See 
	my post 'Recipe for successful use of Content Deployment Wizard' for guidance at http://www.sharepointnutsandbolts.com/2008/04/recipe-for-successful-use-of-content.html 

As always, take a backup before importing over valuable data if the operation cannot easily be 
reversed (e.g. updates of many existing pages/items).

Use www.codeplex.com/SPDeploymentWizard for updates/bug reports/feature requests etc.

Many thanks,

Chris O'Brien, 22nd March 2010
www.sharepointnutsandbolts.com





