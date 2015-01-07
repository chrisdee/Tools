== Sharepoint 2003 and 2007 Database Exporter ==

Some months ago I was experimenting with Windows Sharepoint Services and found out that my machine was not powerfull enough to run Windows 2003, SQL Server and Sharepoint at a decent speed. I had data in Sharepoint I wanted to keep so I did an SQL Server backup of the database. I tought that I would re-setup everything easily after a machine upgrade… I was wrong! I never succeeded to restore the database backup in a running Sharepoint environment (I learned the hard way that I should have used WSS backup utilities instead of relying only on a database backup).

From that moment I decided to explore how Microsoft stores its data within Sharepoint. I found out about Docs and UserData tables and I thought I could develop a little tool to extract my data. In the mean time I discovered the Sharepoint Database Explorer from James Edelen. I was happy that I would’nt have to code it myself :)… The tool is nice, but it didn’t allow me to export custom lists and metadata…

As a learning experience, I decided to build an exporter program. It happens that I find it usefull in a couple of situations… So I decided to share in case anybody would have the same needs than I had! :)

***** Updated (2010-02-01) *****

Features (v1.1.0.3):

Export attachments with the same directory structure than original document library. Thanks to Amrit for providing the code.
***** Updated (2009-01-28) *****

Features (v1.1.0.2):

Bug fix regarding the extraction of document librarie items.  Some files where not “seen” and not exported (thanks to Finn Olesen for discovering the problem)
***** Updated (2008-02-21) *****

Features (v1.1.0.1):

It is possible to preview the pending version (checked-out and saved, but not yet checked-in) of a file.
(on both: WSS 2.0 and WSS 3.0) (see this screenshot)

Bug fix regarding the extraction of document librarie items when files are checked-out.
Features (v1.1.0.0):

Connects to either Sharepoint 2003 or Sharepoint 2007 database structure (automatic detection).
It is possible to preview the version history of a file and to export individually a previous version of a file.
Features (v1.0.0.1):

Connects to Sharepoint 2003 databases directly
Does not rely on a working Sharepoint environment… it only connects to the database
You can preview the list contents (see screenshots)
It is possible to export individual files from the preview window.
You can export custom list attachments and document library files
You can export metadata
Metadata is exported in an xml file. For attachments, you have the choice to export them in a folder or to embed them in the xml.

Known issues:

When a list is not customized (kept as-is — no additional fields added), Microsoft does not store the column mapping (Sharepoint field name mapping to SQL Server column name). They probably store them in a resource file so its faster to retrieve than querying the database. For us, that means we have to hardcode the mapping for each list type (Custom List, Contact List, Discussion List, Calendar, etc.). For this reason, some list types may not export well.