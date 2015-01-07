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

namespace Sharepoint.DBExporter.Data
{
	/// <summary>
	/// Summary description for ConnectionDetails.
	/// </summary>
	public class ConnectionDetails
	{
		private string _ServerName = string.Empty;
		private string _DBName = string.Empty;
		private DatabaseConnectionType _ConnectionType = DatabaseConnectionType.WindowsIntegrated;
		private string _SQLUsername = string.Empty;
		private string _SQLPassword = string.Empty;
		private int _CommandTimeout = 15;

		public ConnectionDetails(string serverName, string dbName)
			: this(serverName, dbName, DatabaseConnectionType.WindowsIntegrated, string.Empty, string.Empty, 15)
		{
		}

		public ConnectionDetails(string serverName, string dbName, DatabaseConnectionType connectionType, string sqlUsername, string sqlPassword, int commandTimeout)
		{
			_ServerName = serverName;
			_DBName = dbName;
			_ConnectionType = connectionType;
			_SQLUsername = sqlUsername;
			_SQLPassword = sqlPassword;
			_CommandTimeout = commandTimeout;
		}

		public string ServerName
		{
			get { return (_ServerName); }
		}

		public string DBName
		{
			get { return (_DBName); }
		}

		public DatabaseConnectionType ConnectionType
		{
			get { return (_ConnectionType); }
		}

		public string SQLUsername
		{
			get { return (_SQLUsername); }
		}

		public string SQLPassword
		{
			get { return (_SQLPassword); }
			set { _SQLPassword = value;  }
		}

		public int DefaultCommandTimeout
		{
			get { return (_CommandTimeout); }
		}

	}
}
