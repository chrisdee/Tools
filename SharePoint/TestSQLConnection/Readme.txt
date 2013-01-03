This tool is used to test the connection between any SharePoint box and SQL Server. You can also use this tool to test the connection to any sql server not necessary related to SharePoint. This tool runs only on x64 bit OS with .NET framework 3.5 installed.

Run the tool with no parameters to auto retrieve the SharePoint connection string and test the connection to SQL Server.

Example:

C:\> TestSQLConnection.exe 

You also can test the connection to any SQL Server by supplying the SQL Server and the Database Name parameters as follows:

TestSQLConnection DATABASE_NAME\\<SQL_INSTANCE> DATABASE_NAME 

Example:

C:\> TestSQLConnection.exe CONTOSOdbServer\\SPInstance SharePoint_Config 

If the test is taking too long to return you can specify a timeout limit in seconds as follows: (Keep trying for 15 seconds) 

C:\> TestSQLConnection.exe CONTOSOdbServer\\SPInstance SharePoint_Config 15 

Please note: if you are test the SharePoint connection to SQL make sure you are running the tool with a farm admin. This tool support SharePoint version 2007 and above.