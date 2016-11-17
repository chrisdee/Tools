#################################################################################
# 
# The sample scripts are not supported under any Microsoft standard support 
# program or service. The sample scripts are provided AS IS without warranty 
# of any kind. Microsoft further disclaims all implied warranties including, without 
# limitation, any implied warranties of merchantability or of fitness for a particular 
# purpose. The entire risk arising out of the use or performance of the sample scripts 
# and documentation remains with you. In no event shall Microsoft, its authors, or 
# anyone else involved in the creation, production, or delivery of the scripts be liable 
# for any damages whatsoever (including, without limitation, damages for loss of business 
# profits, business interruption, loss of business information, or other pecuniary loss) 
# arising out of the use of or inability to use the sample scripts or documentation, 
# even if Microsoft has been advised of the possibility of such damages
#
###############################################################################

=======================================
Log Parser Studio Quick Start 
=======================================

Need to skip all the details and run queries now? Start here or visit the blog:

http://blogs.technet.com/b/karywa/archive/2013/04/21/getting-started-with-log-parser-studio.aspx

The very first thing Log Parser Studio needs to know is where the log files are, and the default location that you would 
like any queries that export their results as CSV files to be saved.   
   
1.  Setup your default CSV output path: 
   
	a.  Options > Preferences > Default Output Path. 
	b.  Browse to and select the folder you would like to use for exported results. 
	c.  Click Apply. 
	d.  Any queries that export CSV files will now be saved in this folder. 
 
NOTE: If you forget to set this path before you start the CSV files will be saved in %AppData%\Microsoft\Log Parser Studio by 
default but it is recommended that you move this to another location. 
 
2.  Tell LPS where the log files are by opening the Log File Manager. If you try to run a query before 
completing this step Log Parser studio will prompt and ask you to set the log path. 

Upon clicking OK you are presented with the Log File Manager. Click Add Folder to add a folder or Add File to 
add a single or multiple files. When when adding a folder you still must select at least one file so LPS will know which type of log we are working with. When doing so, LPS will automatically turn this into a wildcard (*.xxx) 
Indicating that all matching logs in the folder will be searched.  
 
You can easily tell which folder or files are currently being searched by examining the status bar at the bottom-
right of Log Parser Studio. To see the full path, roll your mouse over the status bar:  
 
NOTE: LPS and LogParser handle multiple types of logs and queryable objects. It is important to remember that 
the type of log you are querying must match the query you are performing. In other words, when running a 
query that expects IIS logs, only IIS logs should be selected in the File Manager. Failure to do this (it’s easy to 
forget) will result errors or unexpected behavior will be returned when running the query.   

3.  Choose a query from the library and run it: 
 
	a.  Click the library tab if it isn’t already selected. 
	b.  Choose a query in the list and double-click it. This will open the query in its own tab. 
	c.  Click the Run Single Query button to execute the query: 
 

The query will begin doing its work in the background. Once the query has completed there are two possible outputs 
targets; the result grid in the top half of the query tab or a CSV file. Some queries return to the grid while other more 
memory intensive queries are saved to CSV.  
 
As a general rule queries that may return very large result sets are probably best served going to a CSV file for further 
processing in Excel.  Once you have the results there are many features for working with those results.  See the rest of 
the manual for details. [Manual is now online, see blog link above]
 
NOTE: Log Parser 2.2 is a 32Bit COM interface. This means that even on 64bit machines there is a 32 bit memory limit. Due to this limitation very large result sets can easily exceed the total memory LPS has at its disposal (slightly less than 2GB).  It is much better to send very large results to CSV as this process bypasses having to load thousands and thousands of records into memory and goes directly to a file instead. For results that are less than 50,000 records the grid should be fine. 

=======================================
Latest Changes [Short list]
=======================================

Build: 2.0.0.100
Build Date: 6/62010 10:29:51 AM

==> Export and/or Chart are not becoming enabled until all queries are complete (batch) 
==> In some cases switching tabs fixes the export button 
==> Add ability to auto add [OUTFILEPATH] and [LOGFILEPATH]  F7 & F8
==> Added F9 which inserts LIKE '%%'
==> Added preliminary support for all LP 2.2 log formats
==> Added cancellation code which allows basic query abort functionality.
==> Fixed bug that now shows the current grids clear grid button without needing to switch tabs
==> Added AutoTrimWorking Set to fire when pressing CTRL+W. There are two ways to trim the WS: CTRL+W or close all grid tabs 
==> Added LogType to error logging.
==> Fixed various issues with duplicating queries.
==> Modified queries now show an asterisk by the query name.
==> Added log file label (bottom right) as clickable link that opens log file manager.
==> Added delete key to library queries
==> Adding CSV back-end support for exchange protocols. AKA EEL and EELX.
==> Fixed some issues when duplicating tabs for populating lpcontrol data.
==> Fixed various issues that determine which buttons are enabled when switching tabs.
==> Fixed bug where tab rename window could drop behind the main form. 
==> Added keypreview to save query dialog. Enter = Save & Close 
==> Added delete key to delete selected queries in libary.
==> Added > Copy to result grid context menu.
==> Added functionality to copy cells or cells and headers in result grid (CTRL+SHIFT+C = cells only)
==> LPS now stores the library in the install directory
==> Fix bug in MAS\Find User has * after FROM 
==> Improved searching of queries.
==> Added Right-Click > Import to query editor window. Allows direct import of SQL or XML queries.
==> Add -silent switch so automated queries auto-close the app during automation: LPS.exe folders.fld batch.xml -silent
==> Add feature to open/copy log file path in log file manager 
==> Fixed issue where a new library, creating saving new queries.
==> Fix rename of tab to keep original name.
==> Added LP function reference and keyboard shortcuts (SHIFT+F3, CTRL+K)
==> Expanded keyboard shortcuts.
==> F11 comments out selected text in query window.
==> Added new query button.
==> Added date modified field to library schema.
==> Added PowerShell export functionality.
==> Fixed Possible regression on aborted/non-aborted queries.
==> Fixed Executing an already running query changes busy status to Idle
==> Fixed Abort button behaviour inconsistent and timers are not resetting properly when aborting.
==> Fixed Query logs default empty query if the query is a new query
==> Improved error handling and various chart bugs.
==> Fixed Opening a query makes it dirty
==> Fixed When saving a duplicated query, GUID does not change unless saved then reopened.
==> Fixed Ctrl+K is not pulling up keyboard shorcuts.
==> Fixed Log file browser needs to account for log types including non-file input formats
==> Added log file name, date modified and preview to import library dialog.
==> Fixed various threading/cancellation token issues when aborting multiple queries.
==> Added advanced XML query import allowing importing disparate queries from disparate files.
==> SHIFT+F12 auto-inserts a custom ExtractPrefix/Suffix line for parsing logs with embedded variables.
==> Added drill-down search capabilities for query tabs.
==> Categories are not deprecated, enable in preferences.
==> Added query logging. Logs queries executed to file so that no executed query is ever lost.
==> Added multiple new queries 
==> Fixed various existing queries.
==> Fixed bugs posted in Q&A section on LPS download page.
==> Multiple additional improvements, fixes and features.
==================================

Build: 1.0.1.75
Build Date: 3/8/2010 12:39:44 PM

++ Fixed rare issue where updating the main timer may throw an exception.
++ Added build Date/Time to about dialog since we are not yet incrementing build numbers.

