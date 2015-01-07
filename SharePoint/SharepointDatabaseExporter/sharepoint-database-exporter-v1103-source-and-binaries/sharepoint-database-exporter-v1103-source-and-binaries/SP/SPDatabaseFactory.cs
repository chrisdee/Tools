using System;

namespace Sharepoint.DBExporter.SP
{
	/// <summary>
	/// Summary description for SPDatabaseFactory.
	/// </summary>
	public class SPDatabaseFactory
	{
		private SPDatabaseFactory()
		{}

		public static ISPDatabase CreateSPDatabase(Data.DatabaseConnector db)
		{
			string sSQL = "SELECT table_type FROM information_schema.tables WHERE table_name = 'Docs'";
			object oTableType = db.ExecuteScalar(sSQL);
			ISPDatabase oDB = null;

			if (oTableType == null || oTableType == DBNull.Value)
			{
				throw new ApplicationException("This database structure is not recognized as a Sharepoint 2003 or Sharepoint 2007 database.");
			}

			string sTableType = (string) oTableType;
			
			// in Sharepoint 2003, the Docs table is a BASE TABLE
			// in Sharepoint 2007, the Docs table is a VIEW
			if (sTableType == "BASE TABLE")
				oDB = new SPDatabase2003(db);
			else if (sTableType == "VIEW")
				oDB = new SPDatabase2007(db);
			else
				throw new ApplicationException("This database structure is not recognized as a Sharepoint 2003 or Sharepoint 2007 database.");

			return (oDB);
		}
	}
}
