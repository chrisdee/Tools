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

namespace Sharepoint.DBExporter.SP
{
	internal class SPFieldListTemplates
	{
		//TODO: implement template defaults for
		//	- Picture Library
		//	- Events
		//	- Discussion Board
		//	- Surveys
		//	- Issues

		private const int CUSTOM_LIST_TEMPLATE = 100;
		private const int LINK_LIST_TEMPLATE = 103;
		private const int ANNOUNCEMENT_LIST_TEMPLATE = 104;
		private const int CONTACT_LIST_TEMPLATE = 105;
		private const int TASK_LIST_TEMPLATE = 107;

		private static SPFieldDefinitionCollection __ListsDefinitions = null;
		private static SPFieldDefinitionCollection __DocLibsDefinitions = null;
		private static SPFieldDefinitionCollection __LinkLists = null;
		private static SPFieldDefinitionCollection __AnnoucementLists = null;
		private static SPFieldDefinitionCollection __ContactLists = null;
		private static SPFieldDefinitionCollection __TaskLists = null;
		private static SPFieldDefinitionCollection __CustomLists = null;

		#region static constructor

		static SPFieldListTemplates()
		{
			__ListsDefinitions = new SPFieldDefinitionCollection();
			__ListsDefinitions.Add(new SPFieldDefinition("ID", "ID", "UserData.tp_ID"));
			__ListsDefinitions.Add(new SPFieldDefinition("Created", "Created", "UserData.tp_Created"));
			__ListsDefinitions.Add(new SPFieldDefinition("Created By", "Created_x0020_By", "Author.tp_Login"));
			__ListsDefinitions.Add(new SPFieldDefinition("Modified", "Modified", "UserData.tp_Modified"));
			__ListsDefinitions.Add(new SPFieldDefinition("Modified By", "Modified_x0020_By", "Editor.tp_Login"));
			__ListsDefinitions.Add(new SPFieldDefinition("ModerationStatus", "_ModerationStatus", "UserData.tp_ModerationStatus"));
			__ListsDefinitions.Add(new SPFieldDefinition("HasAttachments", "Attachments", "UserData.tp_HasAttachment"));

			__DocLibsDefinitions = new SPFieldDefinitionCollection();
			__DocLibsDefinitions.Add(new SPFieldDefinition("ID", "ID", "UserData.tp_ID"));
			__DocLibsDefinitions.Add(new SPFieldDefinition("Created", "Created", "UserData.tp_Created"));
			__DocLibsDefinitions.Add(new SPFieldDefinition("Created By", "Created_x0020_By", "Author.tp_Login"));
			__DocLibsDefinitions.Add(new SPFieldDefinition("Modified", "Modified", "UserData.tp_Modified"));
			__DocLibsDefinitions.Add(new SPFieldDefinition("Modified By", "Modified_x0020_By", "Editor.tp_Login"));
			__DocLibsDefinitions.Add(new SPFieldDefinition("ModerationStatus", "_ModerationStatus", "UserData.tp_ModerationStatus"));
			__DocLibsDefinitions.Add(new SPFieldDefinition("HasAttachments", "Attachments", "CAST(1 AS BIT)"));
			__DocLibsDefinitions.Add(new SPFieldDefinition("Folder", "Folder", "Docs.DirName"));
			__DocLibsDefinitions.Add(new SPFieldDefinition("Filename", "Filename", "Docs.LeafName"));
			__DocLibsDefinitions.Add(new SPFieldDefinition("ContentID", "ContentID", "Docs.Id"));
			__DocLibsDefinitions.Add(new SPFieldDefinition("Title", "Title", "nvarchar7"));

			__CustomLists = new SPFieldDefinitionCollection();
			__CustomLists.Add(new SPFieldDefinition("Title", "Title", "nvarchar1"));

			__ContactLists = new SPFieldDefinitionCollection();
			__ContactLists.Add(new SPFieldDefinition("Last Name", "Title", "nvarchar1"));
			__ContactLists.Add(new SPFieldDefinition("First Name", "FirstName", "nvarchar3"));
			__ContactLists.Add(new SPFieldDefinition("Full Name", "FullName", "nvarchar5"));
			__ContactLists.Add(new SPFieldDefinition("Email", "Email", "nvarchar6"));
			__ContactLists.Add(new SPFieldDefinition("Company", "Company", "nvarchar8"));
			__ContactLists.Add(new SPFieldDefinition("JobTitle", "JobTitle", "nvarchar9"));
			__ContactLists.Add(new SPFieldDefinition("WorkPhone", "WorkPhone", "nvarchar10"));
			__ContactLists.Add(new SPFieldDefinition("HomePhone", "HomePhone", "nvarchar11"));
			__ContactLists.Add(new SPFieldDefinition("CellPhone", "CellPhone", "nvarchar12"));
			__ContactLists.Add(new SPFieldDefinition("WorkFax", "WorkFax", "nvarchar13"));
			__ContactLists.Add(new SPFieldDefinition("WorkAddress", "WorkAddress", "ntext2"));
			__ContactLists.Add(new SPFieldDefinition("WorkCity", "WorkCity", "nvarchar14"));
			__ContactLists.Add(new SPFieldDefinition("WorkState", "WorkState", "nvarchar15"));
			__ContactLists.Add(new SPFieldDefinition("WorkZip", "WorkZip", "nvarchar16"));
			__ContactLists.Add(new SPFieldDefinition("WorkCountry", "WorkCountry", "nvarchar17"));
			__ContactLists.Add(new SPUrlFieldDefinition("WebPage", "WebPage", new string [] {"nvarchar18", "nvarchar19"}));
			__ContactLists.Add(new SPFieldDefinition("Comments", "Comments", "ntext3"));

			__TaskLists = new SPFieldDefinitionCollection();
			__TaskLists.Add(new SPFieldDefinition("Title", "Title", "nvarchar1"));
			__TaskLists.Add(new SPFieldDefinition("Priority", "Priority", "nvarchar2"));
			__TaskLists.Add(new SPFieldDefinition("PercentComplete", "PercentComplete", "float1"));
			__TaskLists.Add(new SPFieldDefinition("AssignedTo", "AssignedTo", "int1"));
			__TaskLists.Add(new SPFieldDefinition("Body", "Body", "ntext2"));
			__TaskLists.Add(new SPFieldDefinition("StartDate", "StartDate", "datetime1"));
			__TaskLists.Add(new SPFieldDefinition("DueDate", "DueDate", "datetime2"));

			__LinkLists = new SPFieldDefinitionCollection();
			__LinkLists.Add(new SPUrlFieldDefinition("URL", "URL", new string [] {"nvarchar2", "nvarchar3"}));
			__LinkLists.Add(new SPFieldDefinition("Comments", "Comments", "ntext2"));

			__AnnoucementLists = new SPFieldDefinitionCollection();
			__AnnoucementLists.Add(new SPFieldDefinition("Title", "Title", "nvarchar1"));
			__AnnoucementLists.Add(new SPFieldDefinition("Body", "Body", "ntext2"));
			__AnnoucementLists.Add(new SPFieldDefinition("Expires", "Expires", "datetime1"));
		}

		#endregion

		public static SPFieldDefinitionCollection GetTemplateFieldList(int serverTemplate)
		{
			switch (serverTemplate)
			{
				case CUSTOM_LIST_TEMPLATE: return (__CustomLists);
				case LINK_LIST_TEMPLATE: return (__LinkLists);
				case ANNOUNCEMENT_LIST_TEMPLATE: return (__AnnoucementLists);
				case CONTACT_LIST_TEMPLATE: return (__ContactLists);
				case TASK_LIST_TEMPLATE: return (__TaskLists);
			}

			// if server template didn't match. we return an empty collection for friendly operation with
			// client calls like "Collection.AddRange(GetTemplateFieldDefinitions(...))".
			return (new SPFieldDefinitionCollection());
		}

		public static SPFieldDefinitionCollection GetBasicFieldList(SharepointListType listType)
		{
			if (listType == SharepointListType.DocumenLibrary)
				return (__DocLibsDefinitions);
			else
				return (__ListsDefinitions);
		}

	}
}