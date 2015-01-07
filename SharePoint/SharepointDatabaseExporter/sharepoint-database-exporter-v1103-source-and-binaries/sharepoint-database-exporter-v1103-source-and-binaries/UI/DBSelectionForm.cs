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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sharepoint.DBExporter.UI
{
	/// <summary>
	/// Summary description for DBSelectionForm.
	/// </summary>
	public class DBSelectionForm : System.Windows.Forms.Form
	{
		private Data.ConnectionDetails _ConnectionDetails = null;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button _AcceptButton;
		private System.Windows.Forms.Button _CancelButton;
		private System.Windows.Forms.RadioButton _UseIntegratedAuthentication;
		private System.Windows.Forms.TextBox _ServerNameTextBox;
		private System.Windows.Forms.TextBox _DBNameTextBox;
		private System.Windows.Forms.RadioButton _UseSQLAuthenticationOption;
		private System.Windows.Forms.TextBox _SQLUsernameTextBox;
		private System.Windows.Forms.TextBox _SQLPasswordTextBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown _CommandTimeoutUpDown;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DBSelectionForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._UseIntegratedAuthentication = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this._ServerNameTextBox = new System.Windows.Forms.TextBox();
			this._DBNameTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this._UseSQLAuthenticationOption = new System.Windows.Forms.RadioButton();
			this._SQLUsernameTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this._SQLPasswordTextBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this._AcceptButton = new System.Windows.Forms.Button();
			this._CancelButton = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this._CommandTimeoutUpDown = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this._CommandTimeoutUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// _UseIntegratedAuthentication
			// 
			this._UseIntegratedAuthentication.Checked = true;
			this._UseIntegratedAuthentication.Location = new System.Drawing.Point(8, 104);
			this._UseIntegratedAuthentication.Name = "_UseIntegratedAuthentication";
			this._UseIntegratedAuthentication.Size = new System.Drawing.Size(192, 24);
			this._UseIntegratedAuthentication.TabIndex = 4;
			this._UseIntegratedAuthentication.TabStop = true;
			this._UseIntegratedAuthentication.Text = "Use integrated authentication";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "SQL Server name";
			// 
			// _ServerNameTextBox
			// 
			this._ServerNameTextBox.Location = new System.Drawing.Point(112, 8);
			this._ServerNameTextBox.Name = "_ServerNameTextBox";
			this._ServerNameTextBox.Size = new System.Drawing.Size(208, 20);
			this._ServerNameTextBox.TabIndex = 1;
			this._ServerNameTextBox.Text = "";
			// 
			// _DBNameTextBox
			// 
			this._DBNameTextBox.Location = new System.Drawing.Point(112, 32);
			this._DBNameTextBox.Name = "_DBNameTextBox";
			this._DBNameTextBox.Size = new System.Drawing.Size(208, 20);
			this._DBNameTextBox.TabIndex = 3;
			this._DBNameTextBox.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "Database name";
			// 
			// _UseSQLAuthenticationOption
			// 
			this._UseSQLAuthenticationOption.Location = new System.Drawing.Point(8, 136);
			this._UseSQLAuthenticationOption.Name = "_UseSQLAuthenticationOption";
			this._UseSQLAuthenticationOption.Size = new System.Drawing.Size(192, 24);
			this._UseSQLAuthenticationOption.TabIndex = 5;
			this._UseSQLAuthenticationOption.Text = "Use SQL Server authentication";
			// 
			// _SQLUsernameTextBox
			// 
			this._SQLUsernameTextBox.Location = new System.Drawing.Point(112, 168);
			this._SQLUsernameTextBox.Name = "_SQLUsernameTextBox";
			this._SQLUsernameTextBox.Size = new System.Drawing.Size(208, 20);
			this._SQLUsernameTextBox.TabIndex = 7;
			this._SQLUsernameTextBox.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(32, 168);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 20);
			this.label3.TabIndex = 6;
			this.label3.Text = "Username";
			// 
			// _SQLPasswordTextBox
			// 
			this._SQLPasswordTextBox.Location = new System.Drawing.Point(112, 192);
			this._SQLPasswordTextBox.Name = "_SQLPasswordTextBox";
			this._SQLPasswordTextBox.PasswordChar = '*';
			this._SQLPasswordTextBox.Size = new System.Drawing.Size(208, 20);
			this._SQLPasswordTextBox.TabIndex = 9;
			this._SQLPasswordTextBox.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(32, 192);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 20);
			this.label4.TabIndex = 8;
			this.label4.Text = "Password";
			// 
			// _AcceptButton
			// 
			this._AcceptButton.Location = new System.Drawing.Point(168, 224);
			this._AcceptButton.Name = "_AcceptButton";
			this._AcceptButton.TabIndex = 10;
			this._AcceptButton.Text = "&Ok";
			this._AcceptButton.Click += new System.EventHandler(this._AcceptButton_Click);
			// 
			// _CancelButton
			// 
			this._CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._CancelButton.Location = new System.Drawing.Point(248, 224);
			this._CancelButton.Name = "_CancelButton";
			this._CancelButton.TabIndex = 11;
			this._CancelButton.Text = "&Cancel";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 64);
			this.label5.Name = "label5";
			this.label5.TabIndex = 12;
			this.label5.Text = "Command timeout";
			// 
			// _CommandTimeoutUpDown
			// 
			this._CommandTimeoutUpDown.Increment = new System.Decimal(new int[] {
																					10,
																					0,
																					0,
																					0});
			this._CommandTimeoutUpDown.Location = new System.Drawing.Point(112, 64);
			this._CommandTimeoutUpDown.Maximum = new System.Decimal(new int[] {
																				  32000,
																				  0,
																				  0,
																				  0});
			this._CommandTimeoutUpDown.Name = "_CommandTimeoutUpDown";
			this._CommandTimeoutUpDown.Size = new System.Drawing.Size(80, 20);
			this._CommandTimeoutUpDown.TabIndex = 13;
			// 
			// DBSelectionForm
			// 
			this.AcceptButton = this._AcceptButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this._CancelButton;
			this.ClientSize = new System.Drawing.Size(330, 252);
			this.Controls.Add(this._CommandTimeoutUpDown);
			this.Controls.Add(this.label5);
			this.Controls.Add(this._CancelButton);
			this.Controls.Add(this._AcceptButton);
			this.Controls.Add(this._SQLPasswordTextBox);
			this.Controls.Add(this._SQLUsernameTextBox);
			this.Controls.Add(this._DBNameTextBox);
			this.Controls.Add(this._ServerNameTextBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this._UseSQLAuthenticationOption);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this._UseIntegratedAuthentication);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DBSelectionForm";
			this.Text = "Database Selection";
			((System.ComponentModel.ISupportInitialize)(this._CommandTimeoutUpDown)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		public Data.ConnectionDetails SelectedConnectionDetails
		{
			get { return (_ConnectionDetails); }
		}

		private void _AcceptButton_Click(object sender, System.EventArgs e)
		{
			Data.ConnectionDetails oDetails = new Data.ConnectionDetails(
				_ServerNameTextBox.Text,
				_DBNameTextBox.Text,
				_UseIntegratedAuthentication.Checked ? Data.DatabaseConnectionType.WindowsIntegrated : Data.DatabaseConnectionType.SQLAuthentication,
				_SQLUsernameTextBox.Text,
				_SQLPasswordTextBox.Text, 
				Convert.ToInt32(_CommandTimeoutUpDown.Value));
				
			_ConnectionDetails = oDetails;

			this.DialogResult = DialogResult.OK;

			this.Close();
		}
	}
}
