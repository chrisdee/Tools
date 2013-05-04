The “SharePoint Multiple File Upload Script” is a PowerShell script which uploads multiple files from a local folder or network share into SharePoint document libraries. It also provides the option to apply metadata to the files during upload using an XML manifest file. 

It includes the following features:
Upload files from a single folder or an entire folder hierarchy 
Retain the folder hierarchy from the source path into the destination document library 
Choose the root or a specific destination folder in the document library to copy the files into 
Support for adding values to the following column types in the manifest file: Single line of text 
Multiple lines of text 
Choice – Single value (drop-down/radio button) 
Choice – Multiple value (checkboxes) 
Currency 
Number 
Yes/No (checkbox) 
Person or Group – Single value 
Person or Group – Multiple value 
Hyperlink or Picture 
Managed Metadata – Single value 
Managed Metadata – Multiple value 

Automatic file check in after upload, if required 
Automatic file approval after upload, if required 
Option to overwrite existing files in the destination document library 
Option to flatten the structure when copying from a multi-level folder hierarchy to a single folder in a document library (i.e., merging sub-folders to a single location) 
