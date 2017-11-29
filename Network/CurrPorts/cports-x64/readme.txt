


CurrPorts v2.36
Copyright (c) 2004 - 2017 Nir Sofer
Web site: http://www.nirsoft.net



Description
===========

CurrPorts displays the list of all currently opened TCP/IP and UDP ports
on your local computer. For each port in the list, information about the
process that opened the port is also displayed, including the process
name, full path of the process, version information of the process
(product name, file description, and so on), the time that the process
was created, and the user that created it.
In addition, CurrPorts allows you to close unwanted TCP connections, kill
the process that opened the ports, and save the TCP/UDP ports information
to HTML file , XML file, or to tab-delimited text file.
CurrPorts also automatically mark with pink color suspicious TCP/UDP
ports owned by unidentified applications (Applications without version
information and icons)



Versions History
================


* Version 2.36:
  o Added 'Auto Size Columns+Headers' option.

* Version 2.35:
  o The 'Resolve IP Addresses' option now also works with IPv6
    addresses.

* Version 2.32:
  o Added option to choose another font (name and size) to display in
    the main window (Under the View menu).

* Version 2.31:
  o You can now specify process ID in the /close command-line option,
    for example:
    cports.exe /close * * * * 2154

* Version 2.30:
  o Added separated display filter for every TCP state, under Options
    -> State Display Filter ('Display Syn-Sent', 'Display Time Wait', and
    more...)

* Version 2.25:
  o Added 'Hide Items With Loopback Address' option (Hide items that
    their Local Address or Remote Address is 127.0.0.1 or ::1 )

* Version 2.22:
  o Fixed bug: The 'Run As Administrator' option failed to work on
    some systems.

* Version 2.21:
  o Changed the way that the 'Use DNS Cache For Host Names' option
    works, in order to solve a memory leak problem.

* Version 2.20:
  o CurrPorts now displays the process names (Some of them without
    the full path) for most processes when you run it without elevation
    ('Run As Administrator'). Also, the 32-bit version of CurrPorts now
    detects 64-bit processes properly.

* Version 2.15:
  o Added 'Run As Administrator' option (Ctrl+F11), which allows you
    to easily run CurrPorts as Administrator on Windows Vista/7/8/2008.
    (When you run CurrPorts as admin, information about all prcesses is
    displayed)
  o Fixed bug: CurrPorts failed to remember the last size/position of
    the main window if it was not located in the primary monitor.

* Version 2.12:
  o You can now disable/enable all filters from the toolbar.

* Version 2.11:
  o Fixed memory leak problem.

* Version 2.10:
  o When saving the opened ports from command-line, CurrPorts now
    uses the same columns order saved in the .cfg file.

* Version 2.09:
  o Fixed bug from version 2.08: Some filters stopped working...

* Version 2.08:
  o Added support for filtering by process ID (In Advanced Filters
    window), for example:
    include:process:327

* Version 2.07:
  o Fixed the flickering on automatic refresh.

* Version 2.06:
  o Fixed issue: The properties dialog-box and other windows opened
    in the wrong monitor, on multi-monitors system.

* Version 2.05:
  o Added support for GeoLite City database. You can now download the
    GeoLite City database (GeoLiteCity.dat.gz), put it in the same folder
    of cports.exe, and CurrPorts will automatically use it to get the
    country/city information for every remote IP address.

* Version 2.02:
  o CurrPorts now displays a simple error message if it fails to
    close one or more TCP connections.

* Version 2.01:
  o The 'Remote Address' and 'Local Address' columns are now sorted
    by the IP address numerically. (In previous versions they were sorted
    alphabetically)

* Version 2.00:
  o Added optional fifth parameter to the /close command-line option,
    which allows you to specify a process name (e.g: firefox.exe)

* Version 1.97:
  o The 'Use DNS Cache For Host Names' option is now turned off by
    default, because it seems that reading the DNS cache causes a memory
    leak on some Windows 7/x64 systems.

* Version 1.96:
  o Fixed bug: CurrPorts randomly failed to display remote port
    numbers of IPv6 connections.

* Version 1.95:
  o Added 'Use DNS Cache For Host Names' option. When it's turned on,
    CurrPorts uses the DNS cache of Windows to resolve remote IP
    addresses.

* Version 1.94:
  o Added 'Custom' AutoRefresh option under Options -> Auto Refresh.
    The number of seconds for the Custom AutoRefresh can be set in the
    Advanced Options window (Ctrl+O)
  o Fixed the problem with sending the data to stdout (when the
    filename is empty string).

* Version 1.93:
  o Updated the internal country names (added more 14 countries) that
    are used for displaying the country name in the 'Remote IP Country'
    column.

* Version 1.92:
  o When choosing 'Clear Log File' option, CurrPorts now asks you
    whether you want to clear the log, in order to avoid from clearing
    the log file by mistake.

* Version 1.91:
  o Added 'Beep On New Ports' option.

* Version 1.90:
  o Added 'Tray Balloon On New Ports' option. When both this option
    and 'Put Icon On Tray' option are turned on, every new port detected
    by CurrPorts will be displayed in a tray balloon. (If the TCP/UDP
    port is filtered by the other CurrPorts options and it's not
    displayed in the main window, it won't be displayed in the tray
    balloon.)

* Version 1.87:
  o Improved the 'User Name' column. If you run CurrPorts as
    administrator, this column will display the user name for all
    processes. (In previous versions, CurrPorts failed to detect
    processes created by other users, even when you run it as
    Administrator)

* Version 1.86:
  o Added 'Mark Odd/Even Rows' option, under the View menu. When it's
    turned on, the odd and even rows are displayed in different color, to
    make it easier to read a single line.

* Version 1.85:
  o Added command-line options to control the settings under the
    Options and View menus: /MarkPorts, /DisplayUdpPorts,
    /DisplayTcpPorts, /DisplayClosedPorts, and more...

* Version 1.83:
  o Added 'Add Header Line To CSV/Tab-Delimited File' option. When
    this option is turned on, the column names are added as the first
    line when you export to csv or tab-delimited file.

* Version 1.82:
  o Added 'Resize Columns On Every Refresh' option, which allows you
    to automatically resize the columns according to the text length on
    every refresh.

* Version 1.81:
  o Added more include/exclude filter options in the context menu of
    CurrPorts.

* Version 1.80:
  o Added custom log line option (In 'Advanced Options' window),
    which allows you to set the format of the log line and put in it any
    column value you like.

* Version 1.76:
  o Added 'One-Click Tray Icon' option. When it's checked and you use
    the tray icon option, one click on the tray icon with the left mouse
    button will open CurrPorts. (Without this option, double-click is
    required)

* Version 1.75:
  o Added 'Exclude Selected Processes In Filters' option in the
    context menu.
  o Added accelerator key for 'Include Selected Processes In Filters'
    option.
  o Fixed bug 'Include Selected Processes In Filters' option: failed
    to work on system process.
  o Added 'Disable All Filters' option to easily toggle between
    active filter state and no filter state, as an alternative for 'Clear
    All Filters', which doesn't allow you to return back the filters.

* Version 1.70:
  o Added /sort command-line option for sorting the connections list
    saved from command-line.

* Version 1.66:
  o Fixed issue: When CurrPorts window is hidden and there is an icon
    in the taskbar, running CurrPorts again will open the existing
    instance of CurrPorts, instead of creating another one.

* Version 1.65:
  o Added drag And drop icon in the toolbar that allows to to easily
    filter by the desired application. Simply drag the target icon into
    the window of the application, and CurrPorts will display only the
    opened ports of this application.

* Version 1.60:
  o Added new column: Window Title (The window title of the process)
  o Added 'Clear All Filters' option.
  o Added 'Include Selected Processes In Filters' option. Allows you
    to easily filter by selected processes.

* Version 1.56:
  o Added new option: Ask before any action. (If you uncheck this
    option, CurrPorts won't ask you any question before closing
    ports/applications)

* Version 1.55:
  o Added number of remote connections to the status bar.
  o Added ports information in the tray icon tooltip.

* Version 1.51:
  o Fixed bug: In rare cases, exception window may appear when
    starting CurrPorts.

* Version 1.50:
  o Added 'Display Port In Address' option. When this option is
    checked, the addresses will be displayed in 'address:port' format.

* Version 1.48:
  o Fixed the Alt+1 accelerator key.

* Version 1.47:
  o Added AutoRefresh every 1 second.

* Version 1.46:
  o Automatically launch IPNetInfo when it's in the same folder of
    CurrPorts.

* Version 1.45:
  o Added 'Remote IP Country' column that displays the country name
    of the remote IP address (requires to download an external file from
    here)

* Version 1.41:
  o Fixed bug: CurrPorts failed to display the current Auto Refresh
    status in Menu.

* Version 1.40:
  o Added support for IPv6.

* Version 1.37:
  o Fixed bug: CurrPorts failed to display process information when
    running under Windows Vista with non-admin user.
  o Added Module Filename column (works only on XP/SP2)

* Version 1.36:
  o Fixed bug: The main window lost the focus when the user switched
    to another application and then returned back to CurrPorts.

* Version 1.35:
  o Fixed bug in saving as comma-delimited file when field values
    contained comma character.

* Version 1.34:
  o New Option: Remember Last Filter (The filter is saved in
    cports_filter.txt)

* Version 1.33:
  o Added support for saving comma-delimited (.csv) files.
  o Added new command-line option: /scomma

* Version 1.32:
  o New Option: Start CurrPorts As Hidden (Only when 'Put Icon On
    Tray' is turned on)
  o New Option: Copy Remote IP Address (F2).

* Version 1.31:
  o Fixed bug: TCP and UDP ports with the same number and in the same
    process merged into one item.

* Version 1.30:
  o New column: Added On - Displays the date that the specified
    connection was added.
  o New Option: Put Icon On Tray.
  o New Option: Log File.

* Version 1.20:
  o Added support for filters.
  o The settings of CurrPorts utility is now saved to cfg file
    instead of using the Registry.
  o New command-line options.
  o You can now send the information to stdout by specifying an empty
    filename ("") in the command-line.
  o Added support for x64.

* Version 1.11:
  o Added support for process information in Vista.

* Version 1.10:
  o A tooltip is displayed when a string in a column is longer than
    the column length.

* Version 1.09:
  o /close command-line parameter - Close a connection from
    command-line

* Version 1.08:
  o Fixed columns order bug.

* Version 1.07:
  o New option: Resolve the remote IP addresses.

* Version 1.06:
  o New column: Process Attributes - Display the file attributes of
    the process (H for hidden, R for read-only, and so on)
  o Added support for working with IPNetInfo utility

* Version 1.05:
  o Fixed bug: identify process path starting with '\??\'

* Version 1.04:
  o Added more accelerator keys.
  o Added support for Windows XP visual styles.

* Version 1.03:
  o New Option: Display Listening
  o New Option: Display Established
  o New Option: Display Items With Unknown State
  o New Option: Display Items Without Remote Address

* Version 1.02:
  o Fixed bug: "Exception C0000005" message when running CurrPorts on
    Windows NT/2000 without administrator rights.
  o New column: "Process Services" - Displays the list of services of
    a process.

* Version 1.01:
  o The 'Copy Selected Items' option now copies the ports data in
    tab-delimited format, so you can instantly paste the data into your
    Excel worksheet.
  o Improvment in ports to process binding under Windows 2000.
    Process information is now also displayed under Windows NT.

* Version 1.00: First release.



System Requirements
===================

This utility works perfectly under Windows NT, Windows 2000, Windows XP,
Windows Server 2003, Windows Server 2008, Windows Vista, Windows 7,
Windows 8, and Windows 10. There is also a separated download of
CurrPorts for x64 versions of Windows. If you want to use this utility on
Windows NT, you should install psapi.dll in your system32 directory.
You can also use this utility on older versions of Windows (Windows
98/ME), but in these versions of Windows, the process information for
each port won't be displayed.



Using CurrPorts
===============

CurrPorts utility is a standalone executable, and it doesn't require any
installation process or additional DLLs. In order to start using it, just
copy the executable file (cports.exe) to any folder you like, and run it.

The main window of CurrPorts displays the list of all currently opened
TCP and UDP ports. You can select one or more items, and then close the
selected connections, copy the ports information to the clipboard, or
save it to HTML/XML/Text file. If you don't want to view all available
columns, or you want to change the order of the columns on the screen and
in the files you save, select 'Choose Column' from the View menu, and
select the desired columns and their order. In order to sort the list by
specific column, click on the header of the desired column.



The Options Menu
================

The following options are available under the Options menu:
* Display Listening: If this option is enabled, all listening ports are
  displayed.
* Display Established: If this option is enabled, all established
  connections are displayed.
* Display Closed: If this option is enabled, closed ports (with 'Time
  Wait', 'Close Wait', or 'Closed' state) are displayed.
* Display Items With Unknown State: If this option is enabled, items
  with unknown state (the state column is empty) are displayed.
* Display Items Without Remote Address: If this option is enabled,
  disconnected ports with no remote address are displayed.
* Display TCP Ports: If this option is disabled, TCP ports won't be
  displayed in the ports list.
* Display UDP Ports: If this option is disabled, UDP ports won't be
  displayed in the ports list.
* Mark Ports Of Unidentified Applications: If this option is enabled,
  all TCP/UDP ports that opened by applications with no version
  information and with no icons, are automatically marked with pink
  color. If you have on your system one or more ports marked with pink
  color, you should deeply check the processes that created these ports.
  It could be only an innocent application that simply doesn't contain
  any icons and version information (For example: the executables of
  MySQL and Oracle servers don't contain any icons or version info, so if
  you have MySQL/Oracle servers on your system, the ports they open will
  be marked.) , but it can also be a trojan or other unwanted application
  that infiltrated into your system.
* Mark New/Modified Ports: If this option is enabled, each time the
  ports list is refreshed, all newly added ports and existing ports with
  changes are marked with green color.
* Auto Refresh: Allows you to automatically refresh the opened ports
  list each 2, 4, 6, 8, or 10 seconds.
* Sort On Auto Refresh If this option is enabled, the entire ports list
  is sorted each time that the list is refreshed automatically.
  Otherwise, new/modified ports are added to the bottom of the list.



The 'Remote IP Country' column
==============================

In order to watch the countries of the remote IP addresses, you have to
download the latest IP To Country file from here. You have the put the
'IpToCountry.csv' file in the same folder of cports.exe

You can also use the GeoLite City database. Simply download the GeoLite
City in Binary / gzip (GeoLiteCity.dat.gz) and put it in the same folder
of cports.exe
If you want to get faster loading process, extract the GeoLiteCity.dat
from the GeoLiteCity.dat.gz and put it in the same folder of cports.exe



Using Filters
=============

Starting from version 1.20, you can monitor only the opened ports that
you need, by using the "Advanced Filters" option (Options -> Advanced
Filters).

In the filters dialog-box, you can add one or more filter strings
(separated by spaces, semicolon, or CRLF) in the following syntax:
[include | exclude] : [local | remote | both | process] : [tcp | udp |
tcpudp] : [IP Range | Ports Range]

Here's some examples that demonstrate how to create a filter string:
* Display only packets with remote tcp port 80 (Web sites):
  include:remote:tcp:80
* Display only packets with remote tcp port 80 (Web sites) and udp port
  53 (DNS):
  include:remote:tcp:80
  include:remote:udp:53
* Display only packets originated from the following IP address range:
  192.168.0.1 192.168.0.100:
  include:remote:tcpudp:192.168.0.1-192.168.0.100
* Display only TCP and UDP packets that use the following port range:
  53 - 139:
  include:both:tcpudp:53-139
* Filter most BitTorrent packets (port 6881):
  exclude:both:tcpupd:6881
* Display only the opened ports of FireFox browser:
  include:process:firefox.exe



Integration with IPNetInfo utility
==================================

If you want to get more information about the remote IP address displayed
in CurrPorts utility, you can utilize the Integration with IPNetInfo
utility in order to easily view the IP address information from WHOIS
servers:
1. Download and run the latest version of IPNetInfo utility. (If you
   have IPNetInfo with version prior to v1.06, you must download the
   newer version.)
2. Select the desired connections, and then choose "IPNetInfo" from
   the File menu (or simply click Ctrl+I).
3. IPNetInfo will retrieve the information about remote IP addresses
   of the selected connections.



Log File
========

Starting from version 1.30, CurrPorts allows you to save all changes
(added and removed connections) into a log file. In order to start
writing to the log file, check the 'Log Changes' option under the File
menu. By default, the log file is saved as 'cports.log' in the same
folder that cports.exe is located. You can change the default log
filename by setting the 'LogFilename' entry in cports.cfg file.

Be aware that the log file is updated only when you refresh the ports
list manually, or when the 'Auto Refresh' option is turned on.



Custom Log Line
===============

Starting from version 1.80, you can set the format of the lines in the
log file according to your needs. In order to use this feature, go to
'Advanced Options' window (Ctrl+O), check the custom log line option,
type the desired format string.

In the format string, you can use the following variables:
%Process_Name%
%Protocol%
%Local_Port%
%Local_Address%
%Remote_Port%
%Remote_Address%
%Process_Path%
%Process_ID%
%State%
%Product_Name%
%File_Description%
%File_Version%
%Company%
%Process_Created_On%
%Local_Port_Name%
%Remote_Port_Name%
%User_Name%
%Process_Services%
%Process_Attributes%
%Remote_Host_Name%
%Added_On%
%Module_Filename%
%Remote_IP Country%
%Window_Title%

You can also set the minimum number of characters for the column value,
for example:
%Process_Name.25% (Fill with spaces - up to 25 characters)

Notice: %Remote_Host_Name% variable is not displayed on newly added
connections, because the IP address resolving is asynchronous operation,
and the host name is still not available when the log line is added.



Command-Line Options
====================



/stext <Filename>
Save the list of all opened TCP/UDP ports into a regular text file.

/stab <Filename>
Save the list of all opened TCP/UDP ports into a tab-delimited text file.

/scomma <Filename>
Save the list of all opened TCP/UDP ports into a comma-delimited text
file.

/stabular <Filename>
Save the list of all opened TCP/UDP ports into a tabular text file.

/shtml <Filename>
Save the list of all opened TCP/UDP ports into HTML file (Horizontal).

/sverhtml <Filename>
Save the list of all opened TCP/UDP ports into HTML file (Vertical).

/sxml <Filename>
Save the list of all opened TCP/UDP ports to XML file.

/sort <column>
This command-line option can be used with other save options for sorting
by the desired column. If you don't specify this option, the list is
sorted according to the last sort that you made from the user interface.
The <column> parameter can specify the column index (0 for the first
column, 1 for the second column, and so on) or the name of the column,
like "Remote Port" and "Remote Address". You can specify the '~' prefix
character (e.g: "~Remote Address") if you want to sort in descending
order. You can put multiple /sort in the command-line if you want to sort
by multiple columns.

Examples:
cports.exe /shtml "f:\temp\1.html" /sort 2 /sort ~1
cports.exe /shtml "f:\temp\1.html" /sort "Protocol" /sort "~Remote
Address"

/nosort
When you specify this command-line option, the list will be saved without
any sorting.

/filter <filter string>
Start CurrPorts with the specified filters. If you want to specify more
than one filter, use the ';' character as a delimiter.

/cfg <cfg filename>
Start CurrPorts with the specified config file.


/MarkPorts
/DisplayUdpPorts
/DisplayTcpPorts
/DisplayClosedPorts
/MarkNewModifiedPorts
/SortOnAutoRefresh
/AlwaysOnTop
/AskBefore
/DisplayIPv6Ports
/DisplayListening
/DisplayEstablished
/DisplayNoState
/DisplayNoRemoteIP
/ResolveAddresses
/RememberLastFilter
/DisplayPortInAddress
/AutoRefresh,
/ShowInfoTip
/TrayIcon
/TrayIconOneClick
/StartAsHidden
/LogChanges
/LogFilename
/DisabledFilters
/AddExportHeaderLine
You can use all these parameters to control the options that are
available under the Options and View menus.
For example, if you want to start CurrPorts with 'Display UDP Ports'
turned off and 'Display Closed' turned on:
cports.exe /DisplayUdpPorts 0 /DisplayClosedPorts 1

You can also use these parameters in conjunction with all save
parameters. For example: If you want to save into tab-delimited file only
the UDP ports:
cports.exe /DisplayUdpPorts 1 /DisplayTcpPorts 0 /stab "c:\temp\udp.txt"



Here's some examples:
* Save all opened TCP/IP ports created by Internet Explorer browser to
  HTML file:
  cports.exe /filter "include:process:iexplore" /shtml
  "c:\temp\ports.html"
* Add all opened ports information to ports.txt (as tab-delimited text
  file). This example only works when running it from a command-prompt
  window.
  cports.exe /stab "" >> c:\temp\cports1.txt
* Start CurrPorts with filter that will only display the opened ports
  of Internet Explorer and FireFox:
  cports.exe /filter "include:process:firefox;include:process:iexplore"



Closing a Connection From Command-Line
======================================

Starting from version 1.09, you can close one or more connections from
command-line, by using /close parameter.
The syntax of /close command:
/close <Local Address> <Local Port> <Remote Address> <Remote Port>
{Process Name/ID}

For each parameter, you can specify "*" in order to include all ports or
addresses. The process name is an optional parameter. If you specify a
process, only the ports of the specified process will be closed.
Examples:
* Close all connections with remote port 80 and remote address
  192.168.1.10:
  /close * * 192.168.1.10 80
* Close all connections with remote port 80 (for all remote addresses):
  /close * * * 80
* Close all connections to remote address 192.168.20.30:
  /close * * 192.168.20.30 *
* Close all connections with local port 80:
  /close * 80 * *
* Close all connections of Firefox with remote port 80:
  /close * * * 80 firefox.exe
* Close all connections of the process that its ID is 3276:
  /close * * * * 3276



Translating CurrPorts To Another Language
=========================================

CurrPorts allows you to easily translate all menus, dialog-boxes, and
other strings to other languages.
In order to do that, follow the instructions below:
1. Run CurrPorts with /savelangfile parameter:
   cports.exe /savelangfile
   A file named cports_lng.ini will be created in the folder of CurrPorts
   utility.
2. Open the created language file in Notepad or in any other text
   editor.
3. Translate all menus, dialog-boxes, and string entries to the
   desired language.
4. After you finish the translation, Run CurrPorts, and all translated
   strings will be loaded from the language file.
   If you want to run CurrPorts without the translation, simply rename
   the language file, or move it to another folder.



License
=======

This utility is released as freeware. You are allowed to freely
distribute this utility via floppy disk, CD-ROM, Internet, or in any
other way, as long as you don't charge anything for this. If you
distribute this utility, you must include all files in the distribution
package, without any modification !



Disclaimer
==========

The software is provided "AS IS" without any warranty, either expressed
or implied, including, but not limited to, the implied warranties of
merchantability and fitness for a particular purpose. The author will not
be liable for any special, incidental, consequential or indirect damages
due to loss of data or any other reason.



Feedback
========

If you have any problem, suggestion, comment, or you found a bug in my
utility, you can send a message to nirsofer@yahoo.com
