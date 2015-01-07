//Copyright (c) 2006-2010, Pascal Gill (http://blog.dreamdevil.com)
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
	/// Summary description for LinkedAttachmentFormatter.
	/// </summary>
	public class LinkedAttachmentFormatter : IAttachmentFormatter
	{
		private string _AttachmentFolder = null;
		private IMetaFormatter _ParentFormatter = null;

		public LinkedAttachmentFormatter(IMetaFormatter parentFormatter, string attachmentFolder)
		{
			_AttachmentFolder = attachmentFolder;
			_ParentFormatter = parentFormatter;
		}

		#region IAttachmentFormatter Members

		public void Initialize()
		{
			if (!System.IO.Directory.Exists(_AttachmentFolder))
			{
				System.IO.Directory.CreateDirectory(_AttachmentFolder);
			}
		}

		public void ExportAttachment(int attachmentNumber, string sourceFolder, string sourceFilename, byte[] content)
		{
			//following changes made by Amrit to create folder structure
			string sPath = System.IO.Path.Combine(_AttachmentFolder, sourceFolder.Replace("/", "\\"));

			if (!System.IO.Directory.Exists(sPath))
			{
				System.IO.Directory.CreateDirectory(sPath);
			}

			sPath = System.IO.Path.Combine(sPath, sourceFilename);
			if (System.IO.File.Exists(sPath))
			{
				int nCollisionCounter = 2;
				string sExtension = System.IO.Path.GetExtension(sourceFilename);
				string sFilename = System.IO.Path.GetFileNameWithoutExtension(sourceFilename);

				do
				{
					sPath = System.IO.Path.Combine(_AttachmentFolder, string.Format("{0} ({1}){2}", sFilename, nCollisionCounter.ToString(), sExtension));
					nCollisionCounter++;
				}
				while (System.IO.File.Exists(sPath));
			}

			using (System.IO.FileStream oCurrentRowStream = System.IO.File.Create(sPath))
			{
				oCurrentRowStream.Write(content, 0, content.Length);
			}

			if (_ParentFormatter != null)
			{
				string sAttachmentNumber = attachmentNumber.ToString();
				_ParentFormatter.ExportField(string.Format("Attachment{0}Folder", sAttachmentNumber), sourceFolder);
				_ParentFormatter.ExportField(string.Format("Attachment{0}Filename", sAttachmentNumber), sourceFilename);
				_ParentFormatter.ExportField(string.Format("Attachment{0}ExportLocation", sAttachmentNumber), sPath);
			}
		}

		public void Terminate()
		{
			// nothing to do
		}

		#endregion
	}
}
