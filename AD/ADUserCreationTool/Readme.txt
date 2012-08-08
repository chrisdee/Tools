New in 1.1: 
•Added ability to select custom format for sAMAccountName, UPN, and Display Name
 •Minor bug fixes 

New in 1.0:
 •Updated to be compatible with PowerShell v3 Beta 
•Enter custom sAMAccountName, Display Name, and userPrincipalName in single-user and CSV modes
 •Turn off auto-generation of sAMAccountName, Display Name, and userPrincipalName in single-user mode
 •Better error-handling during user creation 
•Set new accounts as enabled or disabled 
•Set 'Password must change at logon' to enabled or disabled 

 
 
One task that every systems administrator has to go through at some point is the creation of new user accounts.  Powershell makes this process simple and adds additional functionality.  The included XML file allows you to set default entries for fields and pre-populate dropdown lists.  The GUI takes the information that you type in for First Name and Last Name to auto-generate the Display Name, sAMAccountName and userPrincipalName (or you can change them manually).  And because the GUI uses the Microsoft-provided ActiveDirectory module, there is no extra software to download or syntax to learn.
 
You also have the ability of bulk-adding users via CSV.  To create users from a CSV, click on File > CSV Mode.  You can then import the CSV and browse through the users in the CSV.  Once the CSV is imported, you can create one user at a time or all at once.  If you want a CSV template created for you, click on File > Create CSV Template.
 
 
 
*Please submit any feature requests via the Q/A tab.
 
ANUC Requirements:
 - Powershell v2 (Minimum)
 - ActiveDirectory module
 
Usage: Download the ANUC.zip file from the TechNet ScriptCenter and extract it into any directory.  Right-click on ANUC.ps1 and select 'Run with PowerShell'.  To modify the available options for drop-down lists and default entries, edit the ANUC.Options.XML file. To create users from a CSV, click on File > CSV Mode.  You can then import the CSV and browse through the users in the CSV.  Once the CSV is imported, you can create one user at a time or all at once.
 
Features:
 •Allows user creation with oft-used Active Directory attributes 
•Bulk creation of users from CSV 
•Optional Auto-generation of account attributes based on other attributes (Customizable in Options.XML) •Display Name 
•samAccountName 
•userPrincipalName 

•Default entries •Domain 
•OU 
•Phone Number (can use full number or company prefix '212-555-') 
•Department 
•Company 
•Description 
•Password (Accounts are set to change at first logon) 
•Site (HQ, Branch Office 1, etc) 
•Street Address 
•City 
•State 
•Postal Code 

•Pre-populated fields for easy selection •Address information 
•Domains 
•OUs 
•Descriptions 
•Departments 

