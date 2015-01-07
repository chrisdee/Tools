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

namespace Sharepoint.DBExporter.Export.Formatters
{
	/// <summary>
	/// Export metadata in a single XML file.
	/// </summary>
	public class XmlMetaFormatter : MetaFormatterBase
	{
		private string _FilePath;
		private System.Xml.XmlTextWriter _StreamWriter = null; 

		public XmlMetaFormatter(string xmlFilePath)
		{
			_FilePath = xmlFilePath;
		}

		public override void Initialize()
		{
			base.Initialize ();

			System.IO.StreamWriter oStream = new System.IO.StreamWriter(_FilePath, false, System.Text.Encoding.UTF8);
			_StreamWriter = new System.Xml.XmlTextWriter(oStream);
			_StreamWriter.Formatting = System.Xml.Formatting.Indented;

			_StreamWriter.WriteStartDocument();
			_StreamWriter.WriteStartElement("ListItems");
		}

		public override void BeginExportRow(object rowIdentifier)
		{
			_StreamWriter.WriteStartElement("ListItem");
		}

		public override void ExportField(string fieldName, object fieldValue)
		{
			_StreamWriter.WriteAttributeString(fieldName, fieldValue.ToString());
		}

		public override void EndExportRow()
		{
			_StreamWriter.WriteEndElement();
		}

		public override void Terminate()
		{
			base.Terminate ();

			if (_StreamWriter != null)
			{
				_StreamWriter.WriteEndElement();
				_StreamWriter.WriteEndDocument();

				_StreamWriter.Close();
				_StreamWriter = null;
			}
		}
	}
}
