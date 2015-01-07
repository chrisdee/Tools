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
	/// Summary description for EmbeddedAttachmentFormatter.
	/// </summary>
	public class EmbeddedAttachmentFormatter : IAttachmentFormatter
	{
		private IMetaFormatter _ParentFormatter = null;

		public EmbeddedAttachmentFormatter(IMetaFormatter parentFormatter)
		{
			_ParentFormatter = parentFormatter;
		}

		#region IAttachmentFormatter Members

		public void Initialize()
		{
			// nothing to do
		}

		public void ExportAttachment(int attachmentNumber, string sourceFolder, string sourceFilename, byte[] content)
		{
			if (_ParentFormatter != null)
			{
				string sAttachmentNumber = attachmentNumber.ToString();

				string sContent = Convert.ToBase64String(content);

				_ParentFormatter.ExportField(string.Format("Attachment{0}Folder", sAttachmentNumber), sourceFolder);
				_ParentFormatter.ExportField(string.Format("Attachment{0}Filename", sAttachmentNumber), sourceFilename);
				_ParentFormatter.ExportField(string.Format("Attachment{0}Content", sAttachmentNumber), sContent);
			}
		}

		public void Terminate()
		{
			// nothing to do
		}

		#endregion
	}
}
