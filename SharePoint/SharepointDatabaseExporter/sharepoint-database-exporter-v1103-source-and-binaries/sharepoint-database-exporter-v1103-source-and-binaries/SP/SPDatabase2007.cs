//Copyright (c) 2006-2008, Pascal Gill (http://blog.dreamdevil.com)
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, 
//are permitted provided that the following conditions are met:
//
//	*	Redistributions of source code must retain the above copyright notice, 
//		this list of conditions and the following disclaimer.
//	*	Redistributions in binary form must reproduce the above copyright notice, 
//		this list of conditions and the following disclaimer in the documentation 
//		and/or other materials provided with the distribution.
//	*	Neither the name of the author nor the names of its contributors may be 
//		used to endorse or promote products derived from this software without 
//		specific prior written permission.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS 
//OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
//AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER 
//OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
//CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
//LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
//LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
//ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
using System;
using System.Data;
using System.Text;

namespace Sharepoint.DBExporter.SP
{
	/// <summary>
	/// This class is a wrapper over the Sharepoint Database.
	/// </summary>
	/// <History author="Pascal Gill" date="2005/05/01">Bug fix while exporting document libraries containing sub-folders (thanks Merijn Boom for reporting the bug)</History>
	public class SPDatabase2007 : ISPDatabase
	{
		private Data.DatabaseConnector _DB = null;

		public SPDatabase2007(Data.DatabaseConnector db)
		{
			_DB = db;
		}

		public string GetDatabaseInfos()
		{
			return ("Sharepoint 2007 Database Format");
		}

		/// <summary>
		/// Gets the root website.
		/// </summary>
		public IDataReader GetWebsites()
		{
			return (this.GetWebsites(string.Empty));
		}

		/// <summary>
		/// Gets the children websites of the specified parent id.
		/// </summary>
		/// <param name="parentId"></param>
		public IDataReader GetWebsites(string parentId)
		{
			System.Text.StringBuilder sbSQL = new StringBuilder();

			sbSQL.Append("SELECT Title, Description, Id, FullUrl, ");
			sbSQL.Append("CAST(ISNULL((SELECT TOP 1 1 FROM Webs w2 WHERE w2.ParentWebId = w1.Id), 0) AS BIT) AS HasChildren, ");
			sbSQL.Append("CAST(ISNULL((SELECT TOP 1 1 FROM Lists l2 WHERE l2.tp_WebId = w1.Id), 0) AS BIT) as HasChildLists ");
			sbSQL.Append("FROM Webs w1 ");

			if (parentId == string.Empty)
			{
				// Portals Never have a ParentWebID and it's Full URL is always blank (also, the WebTemplate for Portals is 20)
				sbSQL.Append("WHERE ParentWebId IS NULL ");
			}
			else
			{
				sbSQL.AppendFormat("WHERE ParentWebId = '{0}' ", parentId);
			}

			return (_DB.ExecuteReader(sbSQL.ToString()));
		}

		/// <summary>
		/// Gets all lists inside a website (excluding Document Libraries and Picture Libraries)
		/// </summary>
		/// <param name="parentId"></param>
		/// <returns></returns>
		public SPListDefinitionCollection GetLists(string parentId)
		{
			System.Text.StringBuilder sbSQL = new StringBuilder();
			SPListDefinitionCollection oLists = new SPListDefinitionCollection();

			sbSQL.Append("SELECT l.tp_Title AS Title, l.tp_Id AS Id, l.tp_BaseType AS BaseType, l.tp_ServerTemplate AS ServerTemplate, l.tp_Fields AS Fields ");
			sbSQL.Append("FROM Lists l ");
			sbSQL.Append("INNER JOIN Webs w ON (w.Id = l.tp_WebId) ");
			sbSQL.AppendFormat("WHERE l.tp_WebId = '{0}' ", parentId);

			DataSet dsLists = _DB.ExecuteDataSet(sbSQL.ToString(), "Lists");
			foreach (DataRow drList in dsLists.Tables["Lists"].Rows)
			{
				oLists.Add(new SPListDefinition(drList));
			}

			return (oLists);
		}

		public DataTable GetListItemsAsDataTable(SPListDefinition list)
		{
			return (_DB.ExecuteDataSet(this.GetListItemsQuery(list, false, false), "ListItems").Tables["ListItems"]);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="list"></param>
		/// <param name="evaluateRecordCount"></param>
		/// <param name="includeDocumentContent">
		///		For document libraries, when this parameter is true, returns an additionnal field containing the file content. 
		///		Ignored if the list is not a document library.</param>
		/// <returns></returns>
		public IDataReader GetListItemsAsReader(SPListDefinition list, bool evaluateRecordCount, bool includeDocumentContent)
		{
			// when returning a datareader, we do it on a separate connection so we do not
			// exclusively lock the main connection.  (Data Readers monopolize the connection until they are closed).
			//
			// In order to prevent the connection pool to grow too much, we set the autoCloseConnection option to true
			// on the ExecuteReader method.
			Data.DatabaseConnector oDB = _DB.Clone();

			if (evaluateRecordCount)
				return (oDB.ExecuteReader(this.GetListItemsQuery(list, includeDocumentContent, true) + ";" + this.GetListItemsQuery(list, includeDocumentContent, false), true));
			else
				return (oDB.ExecuteReader(this.GetListItemsQuery(list, includeDocumentContent, false), true));
		}

		public DataTable GetAttachmentHistoryList(SP.SPListDefinition list, string documentID)
		{
			StringBuilder sbSQL = new StringBuilder();

			//2008-02-21:	Fix to better handle a file when checked-out.
			//				DocStreams.Level usually equals 1 for the last checked-in version.
			//				DocStreams.Level equals 255 for the "pending" checked-out version (saved but not checked-in).
			//
			//				When a document is pending (checked-out) there is 2 records in the Docs table
			//				with the same Id.  We can differentiate them by the version number (Version field) or by the
			//				IsCurrentVersion flag.
			sbSQL.Append("SELECT d.LeafName, v.Version AS VersionNumber, v.TimeCreated ");
			sbSQL.Append("FROM DocVersions v ");
			sbSQL.Append("INNER JOIN Docs d ON (d.id = v.id AND d.IsCurrentVersion = 1) ");
			sbSQL.AppendFormat("WHERE d.id = '{0}' ", documentID);
			sbSQL.Append("ORDER BY v.Version DESC ");

			return (_DB.ExecuteDataSet(sbSQL.ToString(), "History").Tables["History"]);
		}

		public DataTable GetListItemAttachmentsList(SP.SPListDefinition list, int itemID, bool includeContent)
		{
			StringBuilder sbSQL = new StringBuilder();

			if (list.ListType == SharepointListType.DocumenLibrary)
			{
				//2008-02-21:	Fix to better handle a file when checked-out.
				//				DocStreams.Level usually equals 1 for the last checked-in version.
				//				DocStreams.Level equals 255 for the "pending" checked-out version (saved but not checked-in).
				//
				//				When a document is pending (checked-out) there is 2 records in the Docs table
				//				with the same Id.  We can differentiate them by the version number (Version field) or by the
				//				IsCurrentVersion flag.
				sbSQL.Append("SELECT docs.Id AS DocID, docs.DirName AS Folder, docs.LeafName AS [Filename], CheckoutDate %CONTENT_FIELD%");
				sbSQL.Append("FROM Docs docs ");
				sbSQL.Append("INNER JOIN DocStreams content ON (content.Id = docs.Id AND content.Level < 255) ");
				sbSQL.AppendFormat("WHERE docs.ListId = '{0}' ", list.ID);
				sbSQL.AppendFormat("AND docs.DocLibRowId = '{0}' ", itemID.ToString());
				sbSQL.AppendFormat("AND docs.HasStream = CONVERT(bit, 1) ");
				sbSQL.AppendFormat("AND docs.IsCurrentVersion = CONVERT(bit, 1) ");
			}
			else
			{
				sbSQL.Append("SELECT docs.Id AS DocID, docs.DirName AS Folder, docs.LeafName AS [Filename], CAST(NULL AS DATETIME) AS CheckoutDate %CONTENT_FIELD%");
				sbSQL.Append("FROM Docs parentItem ");
				sbSQL.Append("INNER JOIN DocStreams content ON (content.ParentId = parentItem.Id) ");
				sbSQL.Append("INNER JOIN Docs docs ON (docs.Id = content.Id) ");
				sbSQL.AppendFormat("WHERE parentItem.ListId = '{0}' ", list.ID);
				sbSQL.AppendFormat("AND parentItem.LeafName = '{0}' ", itemID.ToString());
			}

			if (includeContent)
				sbSQL.Replace("%CONTENT_FIELD%", ", content.Content ");
			else
				sbSQL.Replace("%CONTENT_FIELD%", string.Empty);

			return (_DB.ExecuteDataSet(sbSQL.ToString(), "Files").Tables["Files"]);
		}

		/// <summary>
		/// Extract the content of a previous version of a file.
		/// </summary>
		/// <param name="documentId"></param>
		/// <param name="versionNumber"></param>
		/// <returns></returns>
		public byte[] GetFileContent(string documentId, int versionNumber)
		{
			string sSQL = string.Format("SELECT Content FROM DocVersions WHERE Id = '{0}' AND Version = {1} ", documentId, versionNumber.ToString());
			object oContent = _DB.ExecuteScalar(sSQL);

			if (oContent is byte [])
				return ((byte []) oContent);
			else
				return (null);
		}

		/// <summary>
		/// Extract the content of the latest version of a file.
		/// </summary>
		/// <param name="documentId"></param>
		/// <returns></returns>
		public byte[] GetFileContent(string documentId, bool checkedOutVersion)
		{
			//2008-02-21: Fix to better handle a file when checked-out.
			//            DocStreams.Level usually equals 1 for the last checked-in version.
			//			  DocStreams.Level equals 255 for the "pending" checked-out version (saved but not checked-in).
			string sLevelCondition = checkedOutVersion ? "Level = 255" : "Level < 255";
			string sSQL = string.Format("SELECT Content FROM DocStreams WHERE Id = '{0}' AND {1}", documentId, sLevelCondition);
			object oContent = _DB.ExecuteScalar(sSQL);

			if (oContent is byte [])
				return ((byte []) oContent);
			else
				return (null);
		}

		public object GetFieldValue(SPFieldDefinition field, IDataReader reader)
		{
			return (reader[field.DisplayName]);
		}

		public string GetFieldText(SPFieldDefinition field, IDataReader reader)
		{
			return (reader[field.DisplayName].ToString());
		}

		public string GetFieldText(SPFieldDefinition field, DataRow data)
		{
			return (data[field.DisplayName].ToString());
		}

		private string GetListItemsQuery(SPListDefinition list, bool includeDocumentContent, bool countQuery)
		{
			StringBuilder sbSQL = new StringBuilder();
			bool bFirst = true;

			sbSQL.Append("SELECT ");

			if (countQuery)
			{
				sbSQL.Append("COUNT(*)");
			}
			else
			{
				SPFieldDefinitionCollection oFields = list.Fields;

				//Note: Beware, the includeDocumentContent works ONLY on document libraries.
				//	    for custom lists we must use GetListItemAttachmentsList method.
				if (includeDocumentContent)
				{
					oFields = new SPFieldDefinitionCollection();
					oFields.AddRange(list.Fields);
					oFields.Add(new SPFieldDefinition("DocContent", "DocContent", "Docs.Content"));
				}

				foreach (SPFieldDefinition oField in oFields)
				{
					if (bFirst)
						bFirst = false;
					else
						sbSQL.Append(", ");

					sbSQL.AppendFormat("{0} AS [{1}]", oField.GetCompletePhysicalName(), oField.DisplayName);
				}
			}

			// Join between UserData and Docs occurs only for Document Libraries
			// Docs.Type = 0 = File
			// Docs.Type = 1 = Folder

			sbSQL.Append(" FROM UserData ");

			if (list.ListType == SharepointListType.DocumenLibrary)
			{
				// 2007/05/01: Bug Fix - Thanks to Merijn Boom
				//  This query was returning some invalid records (one per folder found in document libraries)
				//  that caused the exporter to crash.
				//
				//  The fix is to perform an INNER JOIN instead of a LEFT JOIN to keep only records of type "File" 
				//  (exluding entries of type "Folder") when processing a document library.
				//
				// 2009/01/29:	Bug Fix - Thanks to Finn Olesen for discovering the issue.
				//				I have removed a condition in the INNER JOIN (on Doc.Version = UserData.tp_version)
				//				that have nothing to do here.  The unique value of a document in a document library
				//				can be obtain with only the 2 following fields: ListId, DocLibRowId
				sbSQL.Append("INNER JOIN Docs ON (Docs.ListId = UserData.tp_ListId AND Docs.DocLibRowId = UserData.tp_ID AND Docs.Type = 0) ");
			}

			sbSQL.Append("LEFT JOIN UserInfo Author ON (Author.tp_ID = UserData.tp_Author AND Author.tp_SiteID = UserData.tp_SiteID) ");
			sbSQL.Append("LEFT JOIN UserInfo Editor ON (Editor.tp_ID = UserData.tp_Editor AND Editor.tp_SiteID = UserData.tp_SiteID) ");
			sbSQL.AppendFormat("WHERE UserData.tp_ListId = '{0}' ", list.ID);
			sbSQL.AppendFormat("AND UserData.tp_IsCurrent = CONVERT(bit, 1) ");

			return (sbSQL.ToString());
		}
	}
}
