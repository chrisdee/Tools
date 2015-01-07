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
using System.Windows.Forms;

namespace Sharepoint.DBExporter
{
	/// <summary>
	/// Summary description for Program.
	/// </summary>
	public class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string [] args) 
		{
			UserPreferences oPreferences = Program.ReadUserPreferences();

			// License agreement.
			if (Convert.ToBoolean(oPreferences.ShowLicense) )
			{
				using (System.IO.Stream oStream = typeof(Program).Assembly.GetManifestResourceStream("Sharepoint.DBExporter._License.txt"))
				{
					System.IO.StreamReader oReader = new System.IO.StreamReader(oStream);

					string sLicense = oReader.ReadToEnd();

					System.Windows.Forms.DialogResult nResult = System.Windows.Forms.MessageBox.Show(sLicense + "\r\n\r\nAccept?", "License", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information);
					// if do not agree with license terms, exit.
					if (nResult == System.Windows.Forms.DialogResult.No)
						return;
				}

				//if flag was not defined, we show it once then we don't bother the user anymore.
				if (oPreferences.ShowLicense == -1)
				{
					oPreferences.ShowLicense = 0;
					Program.WriteUserPreferences(oPreferences);
				}
			}

			ExecutionContext oContext = Program.GetContextFromCommandLine(args);
			if (oContext == null)
				oContext = Program.GetContextFromUser();
			
			if (oContext != null && oContext != ExecutionContext.None)
			{
				oContext.Preferences = oPreferences;

				using (UI.ExplorerForm oMainForm = new UI.ExplorerForm())
				{
					oMainForm.ExecutionContext = oContext;

					Application.Run(oMainForm);

					Program.WriteUserPreferences(oContext.Preferences);
				}
			}
		}

		private static UserPreferences ReadUserPreferences()
		{
			string sPath = System.IO.Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().Location, "pref.xml");
			UserPreferences oReturn = null;

			if (System.IO.File.Exists(sPath))
			{
				System.Xml.Serialization.XmlSerializer oSerializer = new System.Xml.Serialization.XmlSerializer(typeof(UserPreferences));
				System.Xml.XmlTextReader oReader = new System.Xml.XmlTextReader(sPath);

				try
				{
					oReturn = (UserPreferences) oSerializer.Deserialize(oReader);
				}
				catch (Exception)
				{
					oReturn = new UserPreferences();
				}
				finally
				{
					oReader.Close();
				}
			}

			if (oReturn == null)
				oReturn = new UserPreferences();

			return (oReturn);
		}

		private static void WriteUserPreferences(UserPreferences preferences)
		{
			System.Xml.Serialization.XmlSerializer oSerializer = new System.Xml.Serialization.XmlSerializer(typeof(UserPreferences));
			string sPath = System.IO.Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().Location, "pref.xml");

			System.Xml.XmlTextWriter oWriter = new System.Xml.XmlTextWriter(sPath, System.Text.Encoding.UTF8);
			oWriter.Formatting = System.Xml.Formatting.Indented;
			try
			{
				oSerializer.Serialize(oWriter, preferences);
			}
			finally
			{
				oWriter.Close();
			}
		}

		private static ExecutionContext GetContextFromCommandLine(string [] args)
		{
			if (args == null || args.Length == 0)
				return (null);

			ExecutionContext oContext = null;
			string sServer = null;
			string sDB = null;
			Data.DatabaseConnectionType nConnectionType = Data.DatabaseConnectionType.WindowsIntegrated;
			string sPwd = null;
			string sUser = null;

			foreach (string sArg in args)
			{
				if (System.Text.RegularExpressions.Regex.IsMatch(sArg, @"^[-/]([Hh?]|help|Help|HELP)$"))
				{
					MessageBox.Show("Sharepoint2003.DBExporter.exe -s:<server> -d:<database> -a:W|S [-u:<username> -p:<password>]\n\n" +
						"s:<server>	= server name\n" + 
						"d:<database>	= database name\n" +
						"a:W|S		= authentication mode ([W]indows integrated or [S]QL server)\n" +
						"u:<username>	= username for sql server authentication\n" +
						"p:<password>	= password for sql server authentication");

					return (ExecutionContext.None);
				}

				System.Text.RegularExpressions.Match oMatch = null;
				
				oMatch = System.Text.RegularExpressions.Regex.Match(sArg, @"^[-/]s:(?<val>[^ ]+)$");
				if (oMatch.Success)
					sServer = oMatch.Groups["val"].Value;

				oMatch = System.Text.RegularExpressions.Regex.Match(sArg, @"^[-/]d:(?<val>[^ ]+)$");
				if (oMatch.Success)
					sDB = oMatch.Groups["val"].Value;

				oMatch = System.Text.RegularExpressions.Regex.Match(sArg, @"^[-/]a:(?<val>[WwSs])$");
				if (oMatch.Success)
					nConnectionType = (oMatch.Groups["val"].Value.ToLower() == "w") ? Data.DatabaseConnectionType.WindowsIntegrated : Data.DatabaseConnectionType.SQLAuthentication;

				oMatch = System.Text.RegularExpressions.Regex.Match(sArg, @"^[-/]p:(?<val>[^ ]+)$");
				if (oMatch.Success)
					sPwd = oMatch.Groups["val"].Value;

				oMatch = System.Text.RegularExpressions.Regex.Match(sArg, @"^[-/]u:(?<val>[^ ]+)$");
				if (oMatch.Success)
					sUser = oMatch.Groups["val"].Value;
			}

			if (sServer != null && sServer.Length > 0)
			{
				Data.ConnectionDetails oDetails = new Data.ConnectionDetails(sServer, sDB, nConnectionType, sUser, sPwd, 0);
				Data.DatabaseConnector.TestResult oResult = Data.DatabaseConnector.Test(oDetails);

				if (oResult.Success)
					oContext = ExecutionContext.Create(oDetails);
				else
					MessageBox.Show(oResult.Message);
			}
				
			return (oContext);
		}

		private static ExecutionContext GetContextFromUser()
		{
			bool bTryAgain = true;
			ExecutionContext oContext = null;

			while (bTryAgain && oContext == null)
			{
				using (UI.DBSelectionForm oDBSelect = new UI.DBSelectionForm())
				{
					bTryAgain = (oDBSelect.ShowDialog() == DialogResult.OK);
					if (bTryAgain)
					{
						try
						{
							oContext = ExecutionContext.Create(oDBSelect.SelectedConnectionDetails);
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.ToString());
						}
					}
				}
			}

			return (oContext);
		}
	}
}
