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
	public class SPListDefinition
	{
		private System.Data.DataRow _ListData = null;
		bool _IsDecoded = false;
		private SPFieldDefinitionCollection _Fields = null;
		private SharepointListType _Type = SharepointListType.Other;

		public SPListDefinition(System.Data.DataRow list)
		{
			_ListData = list;
			_Type = (Convert.ToInt32(list["BaseType"]) == 1) ? SharepointListType.DocumenLibrary : SharepointListType.Other;
		}

		public string Title
		{
			get { return ((string) _ListData["Title"]); }
		}

		public string ID
		{
			get { return (_ListData["ID"].ToString()); }
		}

		public SharepointListType ListType
		{
			get { return _Type; }
		}

		public int ServerTemplate
		{
			get { return Convert.ToInt32(_ListData["ServerTemplate"]);  }
		}

		public SPFieldDefinitionCollection Fields
		{
			get
			{
				if (!_IsDecoded)
				{
					this.DecodeFieldList();
					_IsDecoded = true;
				}

				return (_Fields);
			}
		}

		private void DecodeFieldList()
		{
			SPFieldDefinitionCollection oTemplateFields = SPFieldListTemplates.GetTemplateFieldList(this.ServerTemplate);

			_Fields = new SPFieldDefinitionCollection();
			_Fields.AddRange(SPFieldListTemplates.GetBasicFieldList(this.ListType));  // add the default definitions available no matter the list template.

			if (_ListData.IsNull("Fields"))
			{
				// the field definition is NULL, that means no customization has been applied to the server template for this list.
				// 
				// We get the default field definitions for the list template.
				_Fields.AddRange(oTemplateFields);
			}
			else
			{
				// the field definitions are specified, that means the default server template has been override with custom metadata.

				// there 2 main node types for field definitions.
				// FieldRef and Field.  I understood that FieldRef maps to a server template default definition and 
				// Field is a custom definition.

				// For each fieldref, we try to get its definition from the template field definitions.
				// for each field, we are going to create a new instance of field definition based on the element attributes.

				System.Xml.XmlDocument oDoc = null;
				string sXML = "<Fields>" + _ListData["Fields"].ToString() + "</Fields>";

				//note: for an unknown reason, oDoc.LoadXml fails in debug (within the VS IDE)
				//      I get rid of the error by freeing memory (closing other opened application).
				//		That piss me off and I discovered that when using an XmlReader it does not generate this error...
				oDoc = new System.Xml.XmlDocument();
				System.IO.StringReader oXmlReader = new System.IO.StringReader(sXML);
				oDoc.Load(oXmlReader);

				System.Xml.XmlNodeList oNodes = oDoc.SelectNodes("Fields/*");

				if (oNodes != null)
				{
					foreach (System.Xml.XmlElement oFieldXmlNode in oNodes)
					{
						SPFieldDefinition oDefinition = null;
						string sAttributeValue = null;
					
						switch (oFieldXmlNode.Name)
						{
							case "Field":
								if (oFieldXmlNode.GetAttribute("Type") == "URL")
									oDefinition = new SPUrlFieldDefinition(
										oFieldXmlNode.GetAttribute("DisplayName"),
										oFieldXmlNode.GetAttribute("Name"),
										new string[] { oFieldXmlNode.GetAttribute("ColName"), oFieldXmlNode.GetAttribute("ColName2") });
								else if (oFieldXmlNode.GetAttribute("ColName").Length != 0)
									oDefinition = new SPFieldDefinition(
										oFieldXmlNode.GetAttribute("DisplayName"),
										oFieldXmlNode.GetAttribute("Name"),
										oFieldXmlNode.GetAttribute("ColName"));
								break;

							case "FieldRef":
								sAttributeValue = oFieldXmlNode.GetAttribute("ColName");
								string sName = oFieldXmlNode.GetAttribute("Name");

								if (sAttributeValue != null && sAttributeValue.Length > 0)
								{
									// we have a ColName value, we use it.
									string sDisplayName = sName.Replace("_x0020_", " ");
									oDefinition = new SPFieldDefinition(sDisplayName, sName, sAttributeValue);
								}
								else
								{
									// we do not have a ColName value.
									// we search the server template definition to get the default column mapping
									oDefinition = oTemplateFields.FindByInternalName(sName);
								}
								break;
						}

						if (oDefinition != null && _Fields.FindByInternalName(oDefinition.InternalName) == null)
							_Fields.Add(oDefinition);
					}
				}

			}
		}
	}
}
