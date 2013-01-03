This tool is used to detect the existing location of the SharePoint configuration cache files then remove them to trigger the timer service to rebuild a fresh new cache. This tool runs on any SharePoint box version 2003 and above supporting x64 bit & x32 bit OS assuming .NET framework 3.5 is installed.

You must run the tool with elevated privileges if running on Win 2008 server to ensure that the tool has enough rights to restart the timer service. The tool auto-detects whether its running in an elevated privilege mode or not.


Example:

C:\> ResetSPConfigCache.exe