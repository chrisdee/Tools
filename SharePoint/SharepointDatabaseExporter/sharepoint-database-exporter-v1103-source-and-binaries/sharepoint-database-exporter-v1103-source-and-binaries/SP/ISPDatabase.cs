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

namespace Sharepoint.DBExporter.SP
{
	/// <summary>
	/// Summary description for ISPDatabase.
	/// </summary>
	public interface ISPDatabase
	{
		string GetDatabaseInfos();

		/// <summary>
		/// Gets the root website.
		/// </summary>
		IDataReader GetWebsites();

		/// <summary>
		/// Gets the children websites of the specified parent id.
		/// </summary>
		/// <param name="parentId"></param>
		IDataReader GetWebsites(string parentId);

		/// <summary>
		/// Gets all lists inside a website (excluding Document Libraries and Picture Libraries)
		/// </summary>
		/// <param name="parentId"></param>
		/// <returns></returns>
		SPListDefinitionCollection GetLists(string parentId);

		DataTable GetListItemsAsDataTable(SPListDefinition list);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="list"></param>
		/// <param name="evaluateRecordCount"></param>
		/// <param name="includeDocumentContent">
		///		For document libraries, when this parameter is true, returns an additionnal field containing the file content. 
		///		Ignored if the list is not a document library.</param>
		/// <returns></returns>
		IDataReader GetListItemsAsReader(SPListDefinition list, bool evaluateRecordCount, bool includeDocumentContent);
		DataTable GetAttachmentHistoryList(SP.SPListDefinition list, string documentID);
		DataTable GetListItemAttachmentsList(SP.SPListDefinition list, int itemID, bool includeContent);

		/// <summary>
		/// Extract the content of a previous version of a file.
		/// </summary>
		/// <param name="documentId"></param>
		/// <param name="versionNumber"></param>
		/// <returns></returns>
		byte[] GetFileContent(string documentId, int versionNumber);

		/// <summary>
		/// Extract the content of the latest version of a file.
		/// </summary>
		/// <param name="documentId"></param>
		/// <returns></returns>
		byte[] GetFileContent(string documentId, bool checkedOutVersion);
		object GetFieldValue(SPFieldDefinition field, IDataReader reader);
		string GetFieldText(SPFieldDefinition field, IDataReader reader);
		string GetFieldText(SPFieldDefinition field, DataRow data);
	}
}
