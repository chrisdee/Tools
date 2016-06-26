


ShadowCopyView v1.01
Copyright (c) 2016 Nir Sofer
Web site: http://www.nirsoft.net



Description
===========

ShadowCopyView is simple tool for Windows 10/8/7/Vista that lists the
snapshots of your hard drive created by the 'Volume Shadow Copy' service
of Windows. Every snapshot contains an older versions of your files and
folders from the date that the snapshot was created, you can browse the
older version of your files and folders, and optionally copy them into a
folder on your disk.




ShadowCopyView vs 'Previous Versions' feature of Windows
========================================================

Windows operating system allows you to access the 'Shadow Copies' by
right-clicking on the file/folder/drive, choosing 'Properties' and then
'Previous Versions'. However, this feature might omit some of the shadow
copies. ShadowCopyView displays all of them.
Also, the 'Previous Versions' window doesn't allow you to access copy
system files unless UAC (User Account Control) is turned off.
ShadowCopyView is executed with full admin rights (elevated process), so
you can access and extract any file you need.



System Requirements
===================

This utility works on any version of Windows, starting from Windows
Vista, and up to Windows 10. Windows XP is not supported. In order to use
ShadowCopyView on 64-bit systems, you must download and use the 64-bit
version of this tool.



Versions History
================


* Version 1.01:
  o Fixed issue: On some systems, ShadowCopyView displayed 'error
    0x00000001' message if there is no any shadow copy on the system.

* Version 1.00 - First release.



Start Using ShadowCopyView
==========================

ShadowCopyView doesn't require any installation process or additional DLL
files. In order to start using it, simply run the executable file -
ShadowCopyView.exe
After running ShadowCopyView, the upper pane of the main window displays
the list of all snapshots created by the Volume Shadow Copy service of
Windows. When selecting a snapshot in the upper pane, the lower pane
displays the files and folder of the selected snapshot. You can navigate
to a subfolder by double-clicking a folder or pressing the enter key. You
can return back to the previous folders by using the 'Directory Level Up'
(Alt+Up Arrow) option or 'Go To Root Directory' option (Ctrl+R).
You can also navigate to the desired folder by typing the path (e.g:
\Windows\System32 ) in the text-box above the files/folders list and
pressing enter.

Optionally, you can also open the selected snapshot in Windows Explorer
by pressing F2 (Open In Windows Explorer), but this option will work only
if the 'Previous Versions' feature of Windows supports the selected
snapshot. Otherwise, you'll get an error message.



Extracting Shadow Copy Files
============================

You can extract/copy files by from a snapshot by selecting files/folders
in the lower pane and then pressing F8 ('Copy Selected Files To' option).
In the 'Copy Files' dialog-box you should choose the destination folder
and then press 'Do it!'. Be aware that if a file or folder is already
exist in the destination folder, then ShadowCopyView will not overwrite
your files, but it'll copy the files using another name.



Accessing shadow copies with other software
===========================================

The path displayed under the 'Snapshot Name' column (e.g:
\\?\GLOBALROOT\Device\HarddiskVolumeShadowCopy6 ) can be used to access
the shadow copies on some other programs. For example, if you specify
this path in NirSoft SearchMyFiles utility as the base folder to search,
SearchMyFiles will search files inside the specified shadow copy.
You can also use this path to access shadow copies on 7-zip File Manager,
However... I found out that on some versions of this tool it fails to
access the root folder but it still works properly when specifying a
subfolder (e.g: \\?\GLOBALROOT\Device\HarddiskVolumeShadowCopy1\Windows )



Translating ShadowCopyView to other languages
=============================================

In order to translate ShadowCopyView to other language, follow the
instructions below:
1. Run ShadowCopyView with /savelangfile parameter:
   ShadowCopyView.exe /savelangfile
   A file named ShadowCopyView_lng.ini will be created in the folder of
   ShadowCopyView utility.
2. Open the created language file in Notepad or in any other text
   editor.
3. Translate all string entries to the desired language. Optionally,
   you can also add your name and/or a link to your Web site.
   (TranslatorName and TranslatorURL values) If you add this information,
   it'll be used in the 'About' window.
4. After you finish the translation, Run ShadowCopyView, and all
   translated strings will be loaded from the language file.
   If you want to run ShadowCopyView without the translation, simply
   rename the language file, or move it to another folder.



License
=======

This utility is released as freeware. You are allowed to freely
distribute this utility via floppy disk, CD-ROM, Internet, or in any
other way, as long as you don't charge anything for this and you don't
sell it or distribute it as a part of commercial product. If you
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
