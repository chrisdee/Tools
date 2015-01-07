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

namespace Sharepoint.DBExporter.Export
{
	/// <summary>
	/// Summary description for ExportManager.
	/// </summary>
	public class ExportManager
	{
		private Formatters.IMetaFormatter _Formatter = null;

		public ExportManager(Formatters.IMetaFormatter formatter)
		{
			_Formatter = formatter;
		}

		public void Start(SP.ISPDatabase sharepointDatabase, SP.SPListDefinition list, UI.IProgressNotifier notifier)
		{
			notifier.Reset("Initializing...");
			_Formatter.Initialize();

			SP.SPFieldDefinitionCollection oFieldList = list.Fields;

			notifier.SetProgress("Retrieving records from Sharepoint database...", 0);

			System.Data.IDataReader oReader = sharepointDatabase.GetListItemsAsReader(list, true, false);

			oReader.Read();
			int nTotalRecords = oReader.GetInt32(0);
			int nProcessedRecords = 0;

			notifier.SetProgress(string.Format("{0} records retrieved.  Now exporting...", nTotalRecords.ToString()), 10);

			oReader.NextResult();
			while (oReader.Read())
			{
				int nListItemID = Convert.ToInt32(oReader["ID"]);

				_Formatter.BeginExportRow(nListItemID.ToString());

				foreach (SP.SPFieldDefinition oField in oFieldList)
				{
					_Formatter.ExportField(oField.InternalName, sharepointDatabase.GetFieldText(oField, oReader));
				}

				if (_Formatter.SupportAttachments && oReader.GetBoolean(oReader.GetOrdinal("HasAttachments")))
				{
					DataTable oAttachmentList = sharepointDatabase.GetListItemAttachmentsList(list, nListItemID, true);
					int nCounter = 1;

					foreach (DataRow oAttachment in oAttachmentList.Rows)
					{
						_Formatter.ExportAttachment(nCounter, oAttachment["Folder"].ToString(), oAttachment["Filename"].ToString(), (byte[]) oAttachment["Content"]);
						nCounter++;
					}
				}

				_Formatter.EndExportRow();
				nProcessedRecords++;

				if (nProcessedRecords % 100 == 0)
					notifier.SetProgress(null, (short) (10 + (nProcessedRecords * 90 / nTotalRecords)));
			}

			_Formatter.Terminate();
			notifier.SetComplete(null);
		}
	}
}
