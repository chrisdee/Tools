
 --- Snap2HTML -----------------------------------------------------------
 
  Freeware by RL Vision (c) 2011-2014
  Homepage: http://www.rlvision.com
  
  Portable:
    - Just unzip and run
    - Settings are saved in the application folder


 --- About ---------------------------------------------------------------
 
  This application takes a snapshot of the folder structure on your
  harddrive and saves it as an HTML file. What's unique about Snap2HTML is
  that the HTML file uses modern techniques to make it feel like a "real"
  application, displaying a "treeview with folders that you can click to 
  view the files contained within. There is also a built in file search.
  Still, everything is contained in a single HTML file that you can easily
  store or distribute.
  
  Exported file listings can be used in many ways. One is as a complement
  to your backups (note however that this program does not backup your
  files! It only creates a list of the files and directories). You can
  also keep a file list on your "other" computer in case you need to look
  something up, or save for historic reasons and documentation. When
  helping your friends with their computer problems you can ask them to
  send a snapshop of their folders so you can better understand their
  problem. It's really up to you to decide what it can be used for!

  Security Notice: The finished HTML file contains embedded javascript.
  Web browsers (especially Internet Explorer) may limit execution of scripts 
  as a security measure. If the page is stuck on "loading..." (with no cpu 
  activity - large files may take a while to load) this is probably what the
  problem is.


 --- Linking Files -------------------------------------------------------

  Linking allows you open the listed files directly in your web browser. 
  This is designed to be flexible, which also sometimes makes it tricky
  to get right. Here are some examples that shows how to use it:

	-> Link to fully qualified local path
		Root folder:	"c:\my_root\"
		Link to:		"c:\my_root\"
		Save snapshot:	[anywhere locally]

	-> Link to relative local path
		Root folder:	"c:\my_root\"
		Link to:		"my_root\"
		Save snapshot:	"c:\snapshot.html"

	-> Link to same folder as snapshot is saved in
		Root folder:	"c:\my_root\"
		Link to:		[leave textbox empty]
		Save snapshot:	"c:\my_root\snapshot.html"

	-> Link to a web server with mirror of local folder
		Root folder:	"c:\my_www_root\"
		Link to:		"http://www.mywebserver.com/"
		Save snapshot:	[anywhere]

	-> Link to a relative path on a web server with mirror of local folder
		Root folder:	"c:\my_www_root\subfolder"
		Link to:		"subfolder/"
		Save snapshot:	"http://www.mywebserver.com/snapshot.html"


  Notes:
    
	Only files can be linked. Folders are automatically linked to open the
	path in the page.

    Different browsers handle local links in different ways, probably for 
    security reasons. For example, Internet Explorer will not let you open
    links to files on your local machine at all. (You can however copy the
    link and paste into the location field.)


 --- Command Line ---------------------------------------------------------

  You can automate Snap2HTML by starting it with command line options:

  Simple:   Snap2HTMl.exe "c:\path\to\root\folder"
    
              Starts the program with the given root path already set


  Full:     Snap2HTMl.exe [-path:"root folder path"] [-outfile:"filename"] [-link:"link to path"] [-title:"page title"] [-hidden] [-system]

              -path:"root folder path"   - The root path to load.
                                           Example: -path:"c:\temp"
                                         
              -outfile:"filename"        - The filename to save the snapshot as.
                                           Don't forget the html extension!
                                           Example: -outfile:"c:\temp\out.html"

              -link:"link to path"       - The path to link files to.
                                           Example: -link:"c:\temp"
                                         
              -title:"page title"        - Set the page title
			  
			  -hidden                    - Include hidden items
            
              -system                    - Include system items


  Notes:    Using -path and -outfile will cause the program to automatically
            start generating the snapshot, and quit when done!

            Always surround paths and filenames with quotes ("")!


 --- Template Design -----------------------------------------------------

  If you know html and javascript you may want to have a look at the file
  "template.html" in the application folder. This is the base for the
  output, and you can modify it with your own enhancements and design changes. 
  If you make something nice you are welcome, to send it to me and I might 
  distribute it with future versions of the program!


  --- Known Problems -----------------------------------------------------

  One user reported needing to start Snap2HTML with "Run as Administrator"
  on Win7 Basic, otherwise it would hang when clicking on the browse for 
  folders button.

  Internet Explorer may fail to load very large files. The problems seems 
  to be a hard limit in some versions of IE. I have seem this problem in 
  IE11 myself. Being a hard limit there is no easy solution right now.

  If you get the error message "The application failed to initialize 
  properly (0xc0000135)" this means you don't have .Net 2.0 installed.
  This should be pre-installed on Windows XP (SP3), Vista & 7, but 
  Windows 8+ users might need to enable it first:
    1. Go to Control Panel –> Programs –> Get Programs 
    2. Click Turn Windows features on or off 
    3. Enable '.NET Framework 3.5 (includes .NET 2.0 and 3.0)'
    4. Click OK. 

 
  --- Version History -----------------------------------------------------

  v1.0 (2011-07-25)
	Initial release

  v1.1 (2011-08-11)
	Added tooltips when hovering folders
	Bugfixes

  v1.2 (2011-08-18)
	Fixed some folder sorting problems
	Better error handling when permissions do not allow reading

  v1.5 (2012-06-18)
	Added command line support
	Files can now be linked to a target of your choice
	Option to automatically open snapshots when generated
	Several bugfixes and tweaks

  v1.51 (2012-07-11)
	Improved error handling

  v1.9 (2013-07-24)
    Major overhaul of HTML template
    MUCH faster HTML loading
	Reduced HTML size by about 1/3
	Folders are now also displayed in the HTML file list
	Added option to set page title
	Application now saves it settings (in application folder)
	GUI enhancements: Drag & Drop a source folder, tooltips
	Many smaller fixes to both application and HTML

  v1.91 (2013-12-29)
    Smaller change to hide root folder when linking files

  v1.92 (2014-06-12)
    Fixed various bugs reported by users lately
	Slight changes to the internals of the template file


 --- End Use License Agreement -------------------------------------------
  
  1. License
  
  By receiving and/or using RL Vision software, you accept the following
  User Agreement. This agreement is a binding legal agreement between RL
  Vision and the users of RL Vision's software and products. IF YOU DO NOT
  INTEND TO HONOR THIS AGREEMENT, DELETE THIS SOFTWARE NOW.
  
  2. Distribution
  
  This freeware software may be freely distributed, provided that:
  
    1. Such distribution includes only the original archive supplied by RL
    Vision. You may not alter, delete or add any files in the distribution
    archive.
  
    2. No money is charged to the person receiving the software, beyond
    reasonable cost of packaging and other overhead.
  
  3. User Agreement
  
  3.1. Usage and distribution restrictions
  
  The user may not use or distribute RL Vision's software for any unlawful
  purpose.
  
  The user is not allowed to attempt to reverse engineer, disassemble or
  decompile RL Vision Software and products.
  
  3.2 Copyright restriction
  
  All parts of RL Vision software and products are copyright protected
  unless otherwise stated. No program, code, part, image, video clip,
  audio sample, text or computer generated sequence of images may be
  copied or used in any way by the user except as intended within the
  bounds of the single user program.
  
  3.3. Limitation of responsibility
  
  The user of RL Vision software will indemnify, hold harmless, and defend
  RL Vision against lawsuits, claims, costs associated with defense or
  accusations that result from the use of RL Vision software.
  
  RL Vision is not responsible for any damages whatsoever, including loss
  of information, interruption of business, personal injury and/or any
  damage or consequential damage without limitation, incurred before,
  during or after the use of our products.
