rem To get started with scripted deployments, use the following steps:
rem 1. Generate 2 settings files for the export and import of the content you wish to deploy. This is done using 
rem		the GUI version of the Wizard - click 'Save settings' to save your selections to an XML file. 
rem 2. Edit the file paths in the commands below to point to your settings files.
rem 3. Edit the XCOPY command so that the .cmp filename is the same as specified in your settings files.
rem 4. Test! 

stsadm -o RunWizardExport -settingsFile "C:\Development\COB\Wizard\WizardExports\Extracted\1Web1List1File_2.xml" -quiet
xcopy "C:\Exports\1Web1List1File_2.cmp" "C:\Exports\Automated\1Web1List1File_2.cmp"
stsadm -o RunWizardImport -settingsFile "C:\Exports\Automated\Import_1Web1List1File.xml" -quiet

pause..