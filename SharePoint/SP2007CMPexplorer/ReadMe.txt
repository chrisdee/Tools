Background

Content migration EXPORT produces either a single CMP file or a folder with multiple XML files.

To specify the desired output type - use the FILECOMPRESSION EXPORT settings.

To extract files from a CMP file - copy it and rename its extension to CAB.

How To?

Once you have the files in a folder, you are ready to use the CMP Explorer.

Simply type the folder name and click to inspect some of the content of the XML files.

More information

Content migration output XML files are described in details in the WSS SDK, and their schemas are at C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\TEMPLATE\XML\Deployment*.xsd .

Here are excerpts from the WSS SDK:

Manifest.xml is the primary file used by content migration.
Requirements.xml contains information that is used as a preliminary check before any import takes place.
ExportSettings.xml is used to verify the logic of the export and ensure that what is expected in the export is included in the package.
RootObjectMap.xml defines the top-level object to import.
SystemData.xml contains all the default objects that are installed on a server that is extended with Windows SharePoint Services. This information is used primarily when the export/import operation is retaining GUIDs between the source and destination locations. This file contains the schema version. If this does not match the version at the destination, the import fails.
UserGroup.xml contains all the user and group information from the source Web site.
LookupListMap.xml maintains a simple lookup list that records SharePoint list item (list item to list item) references.
ViewFormsList.xml maintains a list of Web Parts and tracks whether each is a view or form.