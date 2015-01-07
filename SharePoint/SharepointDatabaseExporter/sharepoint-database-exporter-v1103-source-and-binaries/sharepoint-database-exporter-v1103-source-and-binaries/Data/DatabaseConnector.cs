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
using System.Data.SqlClient;

namespace Sharepoint.DBExporter.Data
{
	public class DatabaseConnector
	{
		#region TestResult

		public class TestResult
		{
			public bool Success = true;
			public string Message = string.Empty;

			public TestResult(bool success, string message)
			{
				this.Success = success;
				this.Message = message;
			}
		}

		#endregion

		private ConnectionDetails _ConnectionDetails = null;
		private SqlConnection _Connection = null;

		public DatabaseConnector(ConnectionDetails connectionDetails)
		{
			_ConnectionDetails = connectionDetails;
			_Connection = new SqlConnection(this.BuildConnectionString());
			_Connection.Open();
		}

		#region static members

		public static TestResult Test(ConnectionDetails connectionDetails)
		{
			try
			{
				using (SqlConnection oConnection = new SqlConnection(DatabaseConnector.BuildConnectionString(connectionDetails)))
				{
					oConnection.Open();
				}

				return (new TestResult(true, string.Empty));
			}
			catch (Exception ex)
			{
				return (new TestResult(false, ex.Message));
			}
		}

		private static string BuildConnectionString(ConnectionDetails connectionDetails)
		{
			string sConnectString;

			if (connectionDetails.ConnectionType == DatabaseConnectionType.WindowsIntegrated)
				sConnectString = string.Format("Server={0};Database={1};Integrated Security=SSPI;", connectionDetails.ServerName, connectionDetails.DBName);
			else
				sConnectString = string.Format("Server={0};Database={1};User={2};Password={3}", connectionDetails.ServerName, connectionDetails.DBName, connectionDetails.SQLUsername, connectionDetails.SQLPassword);

			return (sConnectString);
		}

		#endregion

		#region ExecuteDataSet

		public DataSet ExecuteDataSet(string statement, string tableName)
		{
			DataSet oResult = new DataSet();

			using (SqlCommand oCommand = this.CreateCommand(statement))
			{
				SqlDataAdapter oAdapter = new SqlDataAdapter(oCommand);
				oAdapter.Fill(oResult, tableName);

				return (oResult);
			}
		}

		public DataSet ExecuteDataSet(string statement)
		{
			return (this.ExecuteDataSet(statement, null));
		}

		#endregion

		#region ExecuteScalar

		public object ExecuteScalar(string statement)
		{
			using (SqlCommand oCommand = this.CreateCommand(statement))
			{
				return (oCommand.ExecuteScalar());
			}
		}

		#endregion

		#region ExecuteReader

		public SqlDataReader ExecuteReader(string statement)
		{
			return (this.ExecuteReader(statement, false));
		}

		public SqlDataReader ExecuteReader(string statement, bool autoCloseConnection)
		{
			using (SqlCommand oCommand = this.CreateCommand(statement))
			{
				if (autoCloseConnection)
					return (oCommand.ExecuteReader(CommandBehavior.CloseConnection));
				else
					return (oCommand.ExecuteReader());
			}
		}

		#endregion

		private SqlCommand CreateCommand(string statement)
		{
			SqlCommand oCommand = new SqlCommand(statement, _Connection);
			oCommand.CommandType = CommandType.Text;
			oCommand.CommandTimeout = _ConnectionDetails.DefaultCommandTimeout;

			return (oCommand);
		}

		private string BuildConnectionString()
		{
			return (DatabaseConnector.BuildConnectionString(_ConnectionDetails));
		}

		public DatabaseConnector Clone()
		{
			DatabaseConnector oClone = new DatabaseConnector(_ConnectionDetails);

			return (oClone);
		}
	}
}
