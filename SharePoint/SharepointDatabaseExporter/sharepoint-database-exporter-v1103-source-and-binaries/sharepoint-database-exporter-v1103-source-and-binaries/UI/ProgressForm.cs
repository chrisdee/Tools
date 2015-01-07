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
	/// Summary description for ProgressForm.
	/// </summary>
	public class ProgressForm : System.Windows.Forms.Form, IProgressNotifier	
	{
		private System.Windows.Forms.Label _MessageLabel;
		private System.Windows.Forms.ProgressBar _ProgressBar;
		private System.Windows.Forms.ListBox _LogListBox;
		private System.Windows.Forms.Button _CloseButton;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProgressForm()
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
			this._MessageLabel = new System.Windows.Forms.Label();
			this._ProgressBar = new System.Windows.Forms.ProgressBar();
			this._LogListBox = new System.Windows.Forms.ListBox();
			this._CloseButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _MessageLabel
			// 
			this._MessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._MessageLabel.Location = new System.Drawing.Point(8, 16);
			this._MessageLabel.Name = "_MessageLabel";
			this._MessageLabel.Size = new System.Drawing.Size(376, 23);
			this._MessageLabel.TabIndex = 0;
			this._MessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _ProgressBar
			// 
			this._ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._ProgressBar.Location = new System.Drawing.Point(8, 48);
			this._ProgressBar.Name = "_ProgressBar";
			this._ProgressBar.Size = new System.Drawing.Size(376, 23);
			this._ProgressBar.TabIndex = 1;
			// 
			// _LogListBox
			// 
			this._LogListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._LogListBox.IntegralHeight = false;
			this._LogListBox.Location = new System.Drawing.Point(8, 80);
			this._LogListBox.Name = "_LogListBox";
			this._LogListBox.Size = new System.Drawing.Size(376, 108);
			this._LogListBox.TabIndex = 2;
			// 
			// _CloseButton
			// 
			this._CloseButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this._CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._CloseButton.Enabled = false;
			this._CloseButton.Location = new System.Drawing.Point(159, 192);
			this._CloseButton.Name = "_CloseButton";
			this._CloseButton.TabIndex = 3;
			this._CloseButton.Text = "&Close";
			this._CloseButton.Click += new System.EventHandler(this._CloseButton_Click);
			// 
			// ProgressForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this._CloseButton;
			this.ClientSize = new System.Drawing.Size(392, 218);
			this.ControlBox = false;
			this.Controls.Add(this._CloseButton);
			this.Controls.Add(this._LogListBox);
			this.Controls.Add(this._ProgressBar);
			this.Controls.Add(this._MessageLabel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProgressForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Progress";
			this.ResumeLayout(false);

		}
		#endregion

		#region IProgressNotifier Members

		void IProgressNotifier.Reset(string comment)
		{
			_MessageLabel.Text = string.Empty;
			_LogListBox.Items.Clear();
			_ProgressBar.Value = _ProgressBar.Minimum;
			_CloseButton.Enabled = false;
		}

		void IProgressNotifier.SetProgress(string comment, short percentCompleted)
		{
			if (comment != null)
			{
				_MessageLabel.Text = comment;
				this.AppendToLog(comment);
			}

			_ProgressBar.Value = percentCompleted;
		}

		void IProgressNotifier.Verbose(string comment)
		{
			this.AppendToLog(comment);
		}

		void IProgressNotifier.SetComplete(string comment)
		{
			if (comment != null && comment.Length > 0)
				((UI.IProgressNotifier) this).SetProgress(comment, 100);
			else
				((UI.IProgressNotifier) this).SetProgress("Completed!", 100);

			this.ActivateCloseButton();
		}

		#endregion

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);
			_CloseButton.Left = (this.Width - _CloseButton.Width) / 2;
		}

		private void AppendToLog(string message)
		{
			_LogListBox.Items.Add(message);
			_LogListBox.SelectedIndex = _LogListBox.Items.Count -1;
		}

		private void ActivateCloseButton()
		{
			_CloseButton.Enabled = true;
		}

		private void _CloseButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
