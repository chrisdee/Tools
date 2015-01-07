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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sharepoint.DBExporter.UI
{
	/// <summary>
	/// Summary description for ExplorerForm.
	/// </summary>
	public class ExplorerForm : System.Windows.Forms.Form
	{

		#region AttachmentListItem

		public class AttachmentListItem : TreeNode
		{
			public const int LASTEST_COMMITED_VERSION = 0;
			public const int CHECKED_OUT_VERSION = -1;

			public int ItemID = 0;
			public SP.SPListDefinition ParentList;
			public string AttachmentID;
			public int VersionNumber = LASTEST_COMMITED_VERSION;
			public string Filename;

			public AttachmentListItem(string text, string filename, int itemID, SP.SPListDefinition parentList, string attachmentID)
				: this(text, filename, itemID, parentList, attachmentID, LASTEST_COMMITED_VERSION)
			{
			}

			public AttachmentListItem(string text, string filename, int itemID, SP.SPListDefinition parentList, string attachmentID, int versionNumber)
				: base(text)
			{
				ItemID = itemID;
				ParentList = parentList;
				AttachmentID = attachmentID;
				VersionNumber = versionNumber;
				Filename = filename;
			}
		}

		#endregion

		#region PreviewListItem

		public class PreviewListItem : ListViewItem
		{
			public int ItemID = 0;
			public SP.SPListDefinition ParentList = null;

			public PreviewListItem(string text, int itemID, SP.SPListDefinition parentList)
				:base(text)
			{
				ItemID = itemID;
				ParentList = parentList;
			}
		}

		#endregion

		#region SiteNodes

		private class SiteNodeBase : TreeNode
		{
			public string Title;
			public string Id;

			public SiteNodeBase(string title, string id)
			{
				Title = title;
				Id = id;

				this.Text = title;
				//this.Text = string.Format("{0} ({1})", Title, id);
			}
		}

		private class WebsiteNode : SiteNodeBase
		{
			public WebsiteNode(string title, string id, string description)
				: base(title, id)
			{
				this.ImageIndex = 0;
				this.SelectedImageIndex = 0;
			}
		}

		private class ListNode : SiteNodeBase
		{
			public SP.SPListDefinition SharepointList = null;

			public ListNode(SP.SPListDefinition list)
				: base(list.Title, list.ID)
			{
				SharepointList = list;
				int nImageIndex = 1;

				if (SharepointList.ListType == SP.SharepointListType.DocumenLibrary)
					nImageIndex = 2;

				this.ImageIndex = nImageIndex;
				this.SelectedImageIndex = nImageIndex;
			}

			public WebsiteNode ParentWebNode
			{
				get { return ((WebsiteNode) this.Parent); }
			}
		}

		#endregion

		private AttachmentListItem _SelectedPreviewFile = null;
		private ExecutionContext _ExecContext = null;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.TreeView _SitesTreeView;
		private System.Windows.Forms.ImageList _SitesTreeImages;
		private System.Windows.Forms.Panel _LeftPanel;
		private System.Windows.Forms.Panel _RightPanel;
		private System.Windows.Forms.TabControl _TabControl;
		private System.Windows.Forms.TabPage _PreviewTabPage;
		private System.Windows.Forms.TabPage _ExportTabPage;
		private System.Windows.Forms.ListView _PreviewList;
		private System.Windows.Forms.TreeView _PreviewAttachmentList;
		private System.Windows.Forms.Label _AttachementsLabel;
		private System.Windows.Forms.Panel _PreviewAttachmentListContainerPanel;
		private System.Windows.Forms.Splitter _PreviewListsSplitter;
		private System.Windows.Forms.ContextMenu _PreviewAttachmentListContextMenu;
		private System.Windows.Forms.ImageList _PreviewImageList;
		private System.Windows.Forms.MenuItem _PreviewAttachmentListSaveAsMenuItem;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TabControl _ExportFormatTabControl;
		private System.Windows.Forms.TabPage _XMLExportFormatTabPage;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TabControl _ExportAttachmentOption;
		private System.Windows.Forms.TabPage _EmbeddedExportAttachmentTabPage;
		private System.Windows.Forms.TabPage _LinkedFileExportAttachmentTabPage;
		private System.Windows.Forms.TabPage _NotExportedExportAttachmentTabPage;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button _ExportButton;
		private System.Windows.Forms.ToolTip _Tooltips;
		private System.Windows.Forms.TabPage _NotExportedExportFormat;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox _XmlExportToFilenameTextBox;
		private System.Windows.Forms.Button _XmlExportToFilenameSelectButton;
		private System.Windows.Forms.Button _LinkedAttachmentFolderSelectButton;
		private System.Windows.Forms.TextBox _LinkedAttachmentFolderTextBox;
		private System.Windows.Forms.Label _InfoLabel;
		private System.Windows.Forms.StatusBar _StatusBar;
		private System.ComponentModel.IContainer components;

		public ExplorerForm()
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ExplorerForm));
			this._SitesTreeView = new System.Windows.Forms.TreeView();
			this._SitesTreeImages = new System.Windows.Forms.ImageList(this.components);
			this._LeftPanel = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this._TabControl = new System.Windows.Forms.TabControl();
			this._PreviewTabPage = new System.Windows.Forms.TabPage();
			this._PreviewListsSplitter = new System.Windows.Forms.Splitter();
			this._PreviewList = new System.Windows.Forms.ListView();
			this._PreviewAttachmentListContainerPanel = new System.Windows.Forms.Panel();
			this._PreviewAttachmentList = new System.Windows.Forms.TreeView();
			this._PreviewImageList = new System.Windows.Forms.ImageList(this.components);
			this._AttachementsLabel = new System.Windows.Forms.Label();
			this._ExportTabPage = new System.Windows.Forms.TabPage();
			this._InfoLabel = new System.Windows.Forms.Label();
			this._ExportButton = new System.Windows.Forms.Button();
			this._ExportAttachmentOption = new System.Windows.Forms.TabControl();
			this._NotExportedExportAttachmentTabPage = new System.Windows.Forms.TabPage();
			this._EmbeddedExportAttachmentTabPage = new System.Windows.Forms.TabPage();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this._LinkedFileExportAttachmentTabPage = new System.Windows.Forms.TabPage();
			this._LinkedAttachmentFolderSelectButton = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this._LinkedAttachmentFolderTextBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this._ExportFormatTabControl = new System.Windows.Forms.TabControl();
			this._NotExportedExportFormat = new System.Windows.Forms.TabPage();
			this._XMLExportFormatTabPage = new System.Windows.Forms.TabPage();
			this._XmlExportToFilenameSelectButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this._XmlExportToFilenameTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this._PreviewAttachmentListContextMenu = new System.Windows.Forms.ContextMenu();
			this._PreviewAttachmentListSaveAsMenuItem = new System.Windows.Forms.MenuItem();
			this._RightPanel = new System.Windows.Forms.Panel();
			this._Tooltips = new System.Windows.Forms.ToolTip(this.components);
			this._StatusBar = new System.Windows.Forms.StatusBar();
			this._LeftPanel.SuspendLayout();
			this._TabControl.SuspendLayout();
			this._PreviewTabPage.SuspendLayout();
			this._PreviewAttachmentListContainerPanel.SuspendLayout();
			this._ExportTabPage.SuspendLayout();
			this._ExportAttachmentOption.SuspendLayout();
			this._EmbeddedExportAttachmentTabPage.SuspendLayout();
			this._LinkedFileExportAttachmentTabPage.SuspendLayout();
			this._ExportFormatTabControl.SuspendLayout();
			this._XMLExportFormatTabPage.SuspendLayout();
			this._RightPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _SitesTreeView
			// 
			this._SitesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._SitesTreeView.HideSelection = false;
			this._SitesTreeView.ImageList = this._SitesTreeImages;
			this._SitesTreeView.Location = new System.Drawing.Point(2, 2);
			this._SitesTreeView.Name = "_SitesTreeView";
			this._SitesTreeView.Size = new System.Drawing.Size(260, 452);
			this._SitesTreeView.TabIndex = 0;
			this._SitesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._SitesTreeView_AfterSelect);
			this._SitesTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this._SitesTreeView_BeforeExpand);
			// 
			// _SitesTreeImages
			// 
			this._SitesTreeImages.ImageSize = new System.Drawing.Size(16, 16);
			this._SitesTreeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_SitesTreeImages.ImageStream")));
			this._SitesTreeImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _LeftPanel
			// 
			this._LeftPanel.Controls.Add(this._SitesTreeView);
			this._LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this._LeftPanel.DockPadding.Bottom = 2;
			this._LeftPanel.DockPadding.Left = 2;
			this._LeftPanel.DockPadding.Right = 2;
			this._LeftPanel.DockPadding.Top = 2;
			this._LeftPanel.Location = new System.Drawing.Point(0, 0);
			this._LeftPanel.Name = "_LeftPanel";
			this._LeftPanel.Size = new System.Drawing.Size(264, 456);
			this._LeftPanel.TabIndex = 1;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(264, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 456);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// _TabControl
			// 
			this._TabControl.Controls.Add(this._PreviewTabPage);
			this._TabControl.Controls.Add(this._ExportTabPage);
			this._TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._TabControl.Location = new System.Drawing.Point(2, 2);
			this._TabControl.Name = "_TabControl";
			this._TabControl.SelectedIndex = 0;
			this._TabControl.Size = new System.Drawing.Size(444, 452);
			this._TabControl.TabIndex = 9;
			// 
			// _PreviewTabPage
			// 
			this._PreviewTabPage.Controls.Add(this._PreviewListsSplitter);
			this._PreviewTabPage.Controls.Add(this._PreviewList);
			this._PreviewTabPage.Controls.Add(this._PreviewAttachmentListContainerPanel);
			this._PreviewTabPage.DockPadding.All = 1;
			this._PreviewTabPage.Location = new System.Drawing.Point(4, 22);
			this._PreviewTabPage.Name = "_PreviewTabPage";
			this._PreviewTabPage.Size = new System.Drawing.Size(436, 426);
			this._PreviewTabPage.TabIndex = 0;
			this._PreviewTabPage.Text = "Preview";
			// 
			// _PreviewListsSplitter
			// 
			this._PreviewListsSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._PreviewListsSplitter.Location = new System.Drawing.Point(1, 322);
			this._PreviewListsSplitter.Name = "_PreviewListsSplitter";
			this._PreviewListsSplitter.Size = new System.Drawing.Size(434, 3);
			this._PreviewListsSplitter.TabIndex = 2;
			this._PreviewListsSplitter.TabStop = false;
			// 
			// _PreviewList
			// 
			this._PreviewList.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PreviewList.FullRowSelect = true;
			this._PreviewList.GridLines = true;
			this._PreviewList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this._PreviewList.HideSelection = false;
			this._PreviewList.Location = new System.Drawing.Point(1, 1);
			this._PreviewList.Name = "_PreviewList";
			this._PreviewList.Size = new System.Drawing.Size(434, 324);
			this._PreviewList.TabIndex = 0;
			this._PreviewList.View = System.Windows.Forms.View.Details;
			this._PreviewList.SelectedIndexChanged += new System.EventHandler(this._PreviewList_SelectedIndexChanged);
			// 
			// _PreviewAttachmentListContainerPanel
			// 
			this._PreviewAttachmentListContainerPanel.Controls.Add(this._PreviewAttachmentList);
			this._PreviewAttachmentListContainerPanel.Controls.Add(this._AttachementsLabel);
			this._PreviewAttachmentListContainerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._PreviewAttachmentListContainerPanel.Location = new System.Drawing.Point(1, 325);
			this._PreviewAttachmentListContainerPanel.Name = "_PreviewAttachmentListContainerPanel";
			this._PreviewAttachmentListContainerPanel.Size = new System.Drawing.Size(434, 100);
			this._PreviewAttachmentListContainerPanel.TabIndex = 1;
			// 
			// _PreviewAttachmentList
			// 
			this._PreviewAttachmentList.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PreviewAttachmentList.ImageList = this._PreviewImageList;
			this._PreviewAttachmentList.Location = new System.Drawing.Point(0, 16);
			this._PreviewAttachmentList.Name = "_PreviewAttachmentList";
			this._PreviewAttachmentList.Size = new System.Drawing.Size(434, 84);
			this._PreviewAttachmentList.TabIndex = 0;
			this._PreviewAttachmentList.MouseUp += new System.Windows.Forms.MouseEventHandler(this._PreviewAttachmentList_MouseUp);
			// 
			// _PreviewImageList
			// 
			this._PreviewImageList.ImageSize = new System.Drawing.Size(16, 16);
			this._PreviewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_PreviewImageList.ImageStream")));
			this._PreviewImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _AttachementsLabel
			// 
			this._AttachementsLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this._AttachementsLabel.Location = new System.Drawing.Point(0, 0);
			this._AttachementsLabel.Name = "_AttachementsLabel";
			this._AttachementsLabel.Size = new System.Drawing.Size(434, 16);
			this._AttachementsLabel.TabIndex = 1;
			this._AttachementsLabel.Text = "Attachments:";
			// 
			// _ExportTabPage
			// 
			this._ExportTabPage.Controls.Add(this._InfoLabel);
			this._ExportTabPage.Controls.Add(this._ExportButton);
			this._ExportTabPage.Controls.Add(this._ExportAttachmentOption);
			this._ExportTabPage.Controls.Add(this.label4);
			this._ExportTabPage.Controls.Add(this._ExportFormatTabControl);
			this._ExportTabPage.Controls.Add(this.label2);
			this._ExportTabPage.Location = new System.Drawing.Point(4, 22);
			this._ExportTabPage.Name = "_ExportTabPage";
			this._ExportTabPage.Size = new System.Drawing.Size(436, 448);
			this._ExportTabPage.TabIndex = 1;
			this._ExportTabPage.Text = "Export";
			// 
			// _InfoLabel
			// 
			this._InfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._InfoLabel.Location = new System.Drawing.Point(48, 280);
			this._InfoLabel.Name = "_InfoLabel";
			this._InfoLabel.Size = new System.Drawing.Size(272, 72);
			this._InfoLabel.TabIndex = 14;
			// 
			// _ExportButton
			// 
			this._ExportButton.Location = new System.Drawing.Point(336, 216);
			this._ExportButton.Name = "_ExportButton";
			this._ExportButton.Size = new System.Drawing.Size(88, 24);
			this._ExportButton.TabIndex = 13;
			this._ExportButton.Text = "Export";
			this._ExportButton.Click += new System.EventHandler(this._ExportButton_Click);
			// 
			// _ExportAttachmentOption
			// 
			this._ExportAttachmentOption.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this._ExportAttachmentOption.Controls.Add(this._NotExportedExportAttachmentTabPage);
			this._ExportAttachmentOption.Controls.Add(this._EmbeddedExportAttachmentTabPage);
			this._ExportAttachmentOption.Controls.Add(this._LinkedFileExportAttachmentTabPage);
			this._ExportAttachmentOption.Location = new System.Drawing.Point(8, 136);
			this._ExportAttachmentOption.Name = "_ExportAttachmentOption";
			this._ExportAttachmentOption.SelectedIndex = 0;
			this._ExportAttachmentOption.Size = new System.Drawing.Size(424, 72);
			this._ExportAttachmentOption.TabIndex = 12;
			// 
			// _NotExportedExportAttachmentTabPage
			// 
			this._NotExportedExportAttachmentTabPage.Location = new System.Drawing.Point(4, 25);
			this._NotExportedExportAttachmentTabPage.Name = "_NotExportedExportAttachmentTabPage";
			this._NotExportedExportAttachmentTabPage.Size = new System.Drawing.Size(416, 43);
			this._NotExportedExportAttachmentTabPage.TabIndex = 2;
			this._NotExportedExportAttachmentTabPage.Text = "Not exported";
			// 
			// _EmbeddedExportAttachmentTabPage
			// 
			this._EmbeddedExportAttachmentTabPage.Controls.Add(this.comboBox2);
			this._EmbeddedExportAttachmentTabPage.Controls.Add(this.label5);
			this._EmbeddedExportAttachmentTabPage.Location = new System.Drawing.Point(4, 25);
			this._EmbeddedExportAttachmentTabPage.Name = "_EmbeddedExportAttachmentTabPage";
			this._EmbeddedExportAttachmentTabPage.Size = new System.Drawing.Size(416, 43);
			this._EmbeddedExportAttachmentTabPage.TabIndex = 0;
			this._EmbeddedExportAttachmentTabPage.Text = "Embedded";
			// 
			// comboBox2
			// 
			this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox2.Location = new System.Drawing.Point(92, 8);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(220, 21);
			this.comboBox2.TabIndex = 13;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 12);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 16);
			this.label5.TabIndex = 12;
			this.label5.Text = "Format";
			// 
			// _LinkedFileExportAttachmentTabPage
			// 
			this._LinkedFileExportAttachmentTabPage.Controls.Add(this._LinkedAttachmentFolderSelectButton);
			this._LinkedFileExportAttachmentTabPage.Controls.Add(this.label3);
			this._LinkedFileExportAttachmentTabPage.Controls.Add(this._LinkedAttachmentFolderTextBox);
			this._LinkedFileExportAttachmentTabPage.Location = new System.Drawing.Point(4, 25);
			this._LinkedFileExportAttachmentTabPage.Name = "_LinkedFileExportAttachmentTabPage";
			this._LinkedFileExportAttachmentTabPage.Size = new System.Drawing.Size(416, 43);
			this._LinkedFileExportAttachmentTabPage.TabIndex = 1;
			this._LinkedFileExportAttachmentTabPage.Text = "Linked file";
			// 
			// _LinkedAttachmentFolderSelectButton
			// 
			this._LinkedAttachmentFolderSelectButton.Location = new System.Drawing.Point(384, 8);
			this._LinkedAttachmentFolderSelectButton.Name = "_LinkedAttachmentFolderSelectButton";
			this._LinkedAttachmentFolderSelectButton.Size = new System.Drawing.Size(24, 20);
			this._LinkedAttachmentFolderSelectButton.TabIndex = 5;
			this._LinkedAttachmentFolderSelectButton.Text = "...";
			this._LinkedAttachmentFolderSelectButton.Click += new System.EventHandler(this._LinkedAttachmentFolderSelectButton_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 20);
			this.label3.TabIndex = 4;
			this.label3.Text = "Export to:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _LinkedAttachmentFolderTextBox
			// 
			this._LinkedAttachmentFolderTextBox.Location = new System.Drawing.Point(68, 8);
			this._LinkedAttachmentFolderTextBox.Name = "_LinkedAttachmentFolderTextBox";
			this._LinkedAttachmentFolderTextBox.Size = new System.Drawing.Size(316, 20);
			this._LinkedAttachmentFolderTextBox.TabIndex = 3;
			this._LinkedAttachmentFolderTextBox.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(152, 16);
			this.label4.TabIndex = 11;
			this.label4.Text = "Attachments export format:";
			// 
			// _ExportFormatTabControl
			// 
			this._ExportFormatTabControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this._ExportFormatTabControl.Controls.Add(this._NotExportedExportFormat);
			this._ExportFormatTabControl.Controls.Add(this._XMLExportFormatTabPage);
			this._ExportFormatTabControl.Location = new System.Drawing.Point(8, 40);
			this._ExportFormatTabControl.Name = "_ExportFormatTabControl";
			this._ExportFormatTabControl.SelectedIndex = 0;
			this._ExportFormatTabControl.Size = new System.Drawing.Size(424, 64);
			this._ExportFormatTabControl.TabIndex = 10;
			// 
			// _NotExportedExportFormat
			// 
			this._NotExportedExportFormat.Location = new System.Drawing.Point(4, 25);
			this._NotExportedExportFormat.Name = "_NotExportedExportFormat";
			this._NotExportedExportFormat.Size = new System.Drawing.Size(416, 35);
			this._NotExportedExportFormat.TabIndex = 2;
			this._NotExportedExportFormat.Text = "Not exported";
			// 
			// _XMLExportFormatTabPage
			// 
			this._XMLExportFormatTabPage.Controls.Add(this._XmlExportToFilenameSelectButton);
			this._XMLExportFormatTabPage.Controls.Add(this.label1);
			this._XMLExportFormatTabPage.Controls.Add(this._XmlExportToFilenameTextBox);
			this._XMLExportFormatTabPage.Location = new System.Drawing.Point(4, 25);
			this._XMLExportFormatTabPage.Name = "_XMLExportFormatTabPage";
			this._XMLExportFormatTabPage.Size = new System.Drawing.Size(416, 35);
			this._XMLExportFormatTabPage.TabIndex = 1;
			this._XMLExportFormatTabPage.Text = "XML";
			// 
			// _XmlExportToFilenameSelectButton
			// 
			this._XmlExportToFilenameSelectButton.Location = new System.Drawing.Point(384, 8);
			this._XmlExportToFilenameSelectButton.Name = "_XmlExportToFilenameSelectButton";
			this._XmlExportToFilenameSelectButton.Size = new System.Drawing.Size(24, 20);
			this._XmlExportToFilenameSelectButton.TabIndex = 2;
			this._XmlExportToFilenameSelectButton.Text = "...";
			this._XmlExportToFilenameSelectButton.Click += new System.EventHandler(this._XmlExportToFilenameSelectButton_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "Export to:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _XmlExportToFilenameTextBox
			// 
			this._XmlExportToFilenameTextBox.Location = new System.Drawing.Point(68, 8);
			this._XmlExportToFilenameTextBox.Name = "_XmlExportToFilenameTextBox";
			this._XmlExportToFilenameTextBox.Size = new System.Drawing.Size(316, 20);
			this._XmlExportToFilenameTextBox.TabIndex = 0;
			this._XmlExportToFilenameTextBox.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 16);
			this.label2.TabIndex = 9;
			this.label2.Text = "Metadata export format:";
			// 
			// _PreviewAttachmentListContextMenu
			// 
			this._PreviewAttachmentListContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																											  this._PreviewAttachmentListSaveAsMenuItem});
			// 
			// _PreviewAttachmentListSaveAsMenuItem
			// 
			this._PreviewAttachmentListSaveAsMenuItem.Index = 0;
			this._PreviewAttachmentListSaveAsMenuItem.Text = "&Save As...";
			this._PreviewAttachmentListSaveAsMenuItem.Click += new System.EventHandler(this._PreviewAttachmentListSaveAsMenuItem_Click);
			// 
			// _RightPanel
			// 
			this._RightPanel.Controls.Add(this._TabControl);
			this._RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._RightPanel.DockPadding.All = 2;
			this._RightPanel.Location = new System.Drawing.Point(264, 0);
			this._RightPanel.Name = "_RightPanel";
			this._RightPanel.Size = new System.Drawing.Size(448, 456);
			this._RightPanel.TabIndex = 10;
			// 
			// _StatusBar
			// 
			this._StatusBar.Location = new System.Drawing.Point(0, 456);
			this._StatusBar.Name = "_StatusBar";
			this._StatusBar.Size = new System.Drawing.Size(712, 22);
			this._StatusBar.TabIndex = 11;
			// 
			// ExplorerForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(712, 478);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this._RightPanel);
			this.Controls.Add(this._LeftPanel);
			this.Controls.Add(this._StatusBar);
			this.Name = "ExplorerForm";
			this.Text = "Sharepoint Explorer";
			this._LeftPanel.ResumeLayout(false);
			this._TabControl.ResumeLayout(false);
			this._PreviewTabPage.ResumeLayout(false);
			this._PreviewAttachmentListContainerPanel.ResumeLayout(false);
			this._ExportTabPage.ResumeLayout(false);
			this._ExportAttachmentOption.ResumeLayout(false);
			this._EmbeddedExportAttachmentTabPage.ResumeLayout(false);
			this._LinkedFileExportAttachmentTabPage.ResumeLayout(false);
			this._ExportFormatTabControl.ResumeLayout(false);
			this._XMLExportFormatTabPage.ResumeLayout(false);
			this._RightPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public ExecutionContext ExecutionContext
		{
			set { _ExecContext = value; }
		}

		protected override void OnLoad(EventArgs e)
		{
			_InfoLabel.Text = "To simply export files of a document library:\r\n- choose to not export metadata\r\n- and choose to export attachments as Linked file.";
			_StatusBar.Text = _ExecContext.SPDatabase.GetDatabaseInfos();

			this.LoadUserPreferences();
			this.LoadTree_Websites(string.Empty, _SitesTreeView.Nodes);

			base.OnLoad (e);
		}

		private void LoadUserPreferences()
		{
			if (_ExecContext != null)
			{
				_XmlExportToFilenameTextBox.Text = _ExecContext.Preferences.DefaultXmlMetaExportPath;
				_LinkedAttachmentFolderTextBox.Text = _ExecContext.Preferences.DefaultLinkedAttachmentExportFolder;
			}
		}

		private void LoadTree_Websites(string parentId, TreeNodeCollection parentNodeCollection)
		{
			// returns a reader with 4 fields: Title, Description, Id, HasChildren, HasChildLists
			using (System.Data.IDataReader oReader = _ExecContext.SPDatabase.GetWebsites(parentId))
			{
				while (oReader.Read())
				{
					string sTitle = oReader["Title"].ToString();
					if (sTitle == null || sTitle.TrimEnd().Length == 0)
						sTitle = "(no title)";

					string sUrl = oReader["FullUrl"].ToString();
					string sId = oReader["Id"].ToString();
					string sDesc = oReader["Description"].ToString();

					if (parentId != null && parentId.Length > 0)
						sUrl = sUrl.Substring(sUrl.LastIndexOf("/")+1);

					WebsiteNode oNode = new WebsiteNode("/" + sUrl, sId, sTitle + "\r\n" + sDesc);
					parentNodeCollection.Add(oNode);

					if (Convert.ToBoolean(oReader["HasChildren"]) || Convert.ToBoolean(oReader["HasChildLists"]))
						oNode.Nodes.Add(new TreeNode("..."));
				}
			}
		}

		private void LoadTree_Lists(string parentWebId, TreeNodeCollection parentWebNodeCollection)
		{
			SP.SPListDefinitionCollection colLists = _ExecContext.SPDatabase.GetLists(parentWebId);

			foreach (SP.SPListDefinition oList in colLists)
			{
				parentWebNodeCollection.Add(new ListNode(oList));
			}
		}

		private void LoadPreviewList(ListNode node)
		{
			SP.SPListDefinition oSPList = node.SharepointList;

			_PreviewList.Items.Clear();
			_PreviewList.Columns.Clear();

			this.LoadPreviewAttachmentList(null, 0);

			foreach (SP.SPFieldDefinition oField in oSPList.Fields)
			{
				ColumnHeader oColumn = new ColumnHeader();
				oColumn.Text = oField.DisplayName;
				oColumn.Width = 75;

				_PreviewList.Columns.Add(oColumn);
			}

			System.Data.IDataReader oReader = _ExecContext.SPDatabase.GetListItemsAsReader(oSPList, false, false);
			while (oReader.Read())
			{
				bool bFirst = true;
				ListViewItem oItem = new PreviewListItem(string.Empty, Convert.ToInt32(oReader["ID"]), oSPList);

				foreach (SP.SPFieldDefinition oField in oSPList.Fields)
				{
					string sText = _ExecContext.SPDatabase.GetFieldText(oField, oReader);
					if (bFirst)
					{
						oItem.Text = sText;
						bFirst = false;
					}
					else
					{
						oItem.SubItems.Add(sText);
					}
				}

				_PreviewList.Items.Add(oItem);
			}
		}

		/// <summary>
		/// This method loads a listitem attachments.  
		/// 
		/// In case of a document library, the nature of the list (contains files by itself), 
		/// this list will always contain a single attachment file.
		/// </summary>
		/// <param name="spList"></param>
		/// <param name="spItemID"></param>
		private void LoadPreviewAttachmentList(SP.SPListDefinition spList, int spItemID)
		{
			_PreviewAttachmentList.Nodes.Clear();

			if (spList == null || spItemID == 0)
				return;

			// extract the attachments linked to the currently selected list item.
			DataTable oFilesTable = _ExecContext.SPDatabase.GetListItemAttachmentsList(spList, spItemID, false);
			if (oFilesTable != null)
			{
				// add an item in the treeview for each attachment.
				foreach (DataRow oFileRow in oFilesTable.Rows)
				{
					string sFilename = oFileRow["Filename"].ToString();
					string sText = sFilename;
					string sAttachmentID = oFileRow["DocID"].ToString();

					if (!oFileRow.IsNull("CheckoutDate"))
						sText = string.Format("{0}   ===> a pending version is available (expand to see)", sFilename);

					AttachmentListItem oAttachmentNode = new AttachmentListItem(sText, sFilename, spItemID, spList, sAttachmentID);
					_PreviewAttachmentList.Nodes.Add(oAttachmentNode);

					//if the file is currently checked out, we display the information
					//next to the filename.
					if (!oFileRow.IsNull("CheckoutDate"))
					{
						sText = string.Format("pending version checked-out on: {0:yyyy/MM/dd hh:mm}", oFileRow["CheckoutDate"]);

						AttachmentListItem oHistoryNode = new AttachmentListItem(sText, sFilename, spItemID, spList, sAttachmentID, AttachmentListItem.CHECKED_OUT_VERSION);
						oAttachmentNode.Nodes.Add(oHistoryNode);
					}

					// extract the previous versions of the attachment (if any).
					DataTable oHistory = _ExecContext.SPDatabase.GetAttachmentHistoryList(spList, sAttachmentID);
					foreach (DataRow oHistoryRow in oHistory.Rows)
					{
						sText = string.Format("v.{0} ({1:yyyy-MM-dd HH:mm:ss})", oHistoryRow["VersionNumber"], oHistoryRow["TimeCreated"]);
						string sHistoryFilename = string.Format("{0}-v{1}{2}", System.IO.Path.GetFileNameWithoutExtension(sFilename), oHistoryRow["VersionNumber"], System.IO.Path.GetExtension(sFilename));

						AttachmentListItem oHistoryNode = new AttachmentListItem(sText, sHistoryFilename, spItemID, spList, sAttachmentID, Convert.ToInt32(oHistoryRow["VersionNumber"]));
						oAttachmentNode.Nodes.Add(oHistoryNode);
					}
				}
			}
		}

		private string SelectExportFile(string selectionFilters, string initialDirectory, string defaultFilename)
		{
			using (SaveFileDialog oDlg = new SaveFileDialog())
			{
				oDlg.CheckPathExists = true;
				oDlg.Filter = selectionFilters;
				oDlg.OverwritePrompt = true;
				oDlg.Title = "Save as...";

				if (initialDirectory != null && initialDirectory.Length > 0)
					oDlg.InitialDirectory = initialDirectory;

				if (defaultFilename != null && defaultFilename.Length > 0)
					oDlg.FileName = defaultFilename;

				if (oDlg.ShowDialog(this) == DialogResult.OK)
					return (oDlg.FileName);
				else
					return (null);
			}
		}

		private void _SitesTreeView_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			try
			{
				if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "...")
				{
					e.Node.Nodes.RemoveAt(0);

					if (e.Node is SiteNodeBase)
					{
						SiteNodeBase oNode = (SiteNodeBase) e.Node;

						this.LoadTree_Websites(oNode.Id, oNode.Nodes);
						this.LoadTree_Lists(oNode.Id, oNode.Nodes);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void _SitesTreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			try
			{
				if (e.Node is ListNode)
				{
					_TabControl.Visible = true;
					this.LoadPreviewList((ListNode) e.Node);
				}
				else 
				{
					_TabControl.Visible = false;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void _PreviewList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (_PreviewList.SelectedItems.Count == 1)
				{
					PreviewListItem oListItem = (PreviewListItem) _PreviewList.SelectedItems[0];
					this.LoadPreviewAttachmentList(oListItem.ParentList, oListItem.ItemID);
				}
				else
				{
					this.LoadPreviewAttachmentList(null, 0);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// This method is executed when the "Save As" popup menu item is clicked in preview attachment list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _PreviewAttachmentListSaveAsMenuItem_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (_SelectedPreviewFile == null)
					return;

				string sFilename = this.SelectExportFile("*.*|*.*", _ExecContext.Preferences.DefaultExportFolder, _SelectedPreviewFile.Filename);
				if (sFilename != null)
				{
					_ExecContext.Preferences.DefaultExportFolder = System.IO.Path.GetDirectoryName(sFilename);

					byte [] arrContent = null;

					// if version number is greater than -1, that means we're saving an older version
					// of the file.  When version number is -1, we're saving the latest version of a file.
					if (_SelectedPreviewFile.VersionNumber == AttachmentListItem.LASTEST_COMMITED_VERSION)
					{	// latest checked-in version
						arrContent = _ExecContext.SPDatabase.GetFileContent(_SelectedPreviewFile.AttachmentID, false);
					}
					else if (_SelectedPreviewFile.VersionNumber == AttachmentListItem.CHECKED_OUT_VERSION)
					{	// pending checked-out version
						arrContent = _ExecContext.SPDatabase.GetFileContent(_SelectedPreviewFile.AttachmentID, true);
					}
					else
					{	// previous version (history)
						arrContent = _ExecContext.SPDatabase.GetFileContent(_SelectedPreviewFile.AttachmentID, _SelectedPreviewFile.VersionNumber);
					}

					if (arrContent != null)
					{
						using(System.IO.FileStream oStreamWriter = System.IO.File.Create(sFilename))
						{
							oStreamWriter.Write(arrContent, 0, arrContent.Length);
							oStreamWriter.Close();
						}
					}
					else
						MessageBox.Show("Attachment data not found.  The file may be empty or this type of list is not well handled (in short= maybe a bug in the program)");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void _PreviewAttachmentList_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					TreeNode oNode = _PreviewAttachmentList.GetNodeAt(e.X, e.Y);

					if (oNode != null)
					{
						_SelectedPreviewFile = (AttachmentListItem) oNode;
						_PreviewAttachmentListContextMenu.Show(_PreviewAttachmentList, new Point(e.X, e.Y));
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void _ExportButton_Click(object sender, System.EventArgs e)
		{
			UI.ProgressForm oProgress = null;

			try
			{
				if (!(_SitesTreeView.SelectedNode is ListNode))
					return;

				//TODO: Perform validations

				SP.SPListDefinition oSPList = ((ListNode) _SitesTreeView.SelectedNode).SharepointList;

				Export.Formatters.IMetaFormatter oMetaFormatter = null;
				Export.Formatters.IAttachmentFormatter oAttachmentFormatter = null;

				// *** MetaData Export Format

				if (_ExportFormatTabControl.SelectedTab == _XMLExportFormatTabPage)
				{
					oMetaFormatter = new Export.Formatters.XmlMetaFormatter(_XmlExportToFilenameTextBox.Text);
				}
				else if (_ExportFormatTabControl.SelectedTab == _NotExportedExportFormat)
				{
					oMetaFormatter = new Export.Formatters.NoMetaFormatter();
				}

				// *** Attachment Export Format

				if (_ExportAttachmentOption.SelectedTab == _LinkedFileExportAttachmentTabPage)
				{
					oAttachmentFormatter = new Export.Formatters.LinkedAttachmentFormatter(oMetaFormatter, _LinkedAttachmentFolderTextBox.Text);
				}
				else if (_ExportAttachmentOption.SelectedTab == _EmbeddedExportAttachmentTabPage)
				{
					oAttachmentFormatter = new Export.Formatters.EmbeddedAttachmentFormatter(oMetaFormatter);
				}

				if (oAttachmentFormatter != null)
					oMetaFormatter.AttachmentFormatter = oAttachmentFormatter;

				oProgress = new ProgressForm();
				oProgress.Owner = this;
				oProgress.Show();

				Export.ExportManager oManager = new Export.ExportManager(oMetaFormatter);
				oManager.Start(_ExecContext.SPDatabase, oSPList, oProgress);
			}
			catch (Exception ex)
			{
				if (oProgress != null)
				{
					((UI.IProgressNotifier) oProgress).SetComplete("Export has been interrupted because of an error!");
				}

				MessageBox.Show(ex.ToString());
			}
		}

		private void _XmlExportToFilenameSelectButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				string sFilePath = this.SelectExportFile("*.xml|*.xml", _ExecContext.Preferences.DefaultExportFolder, _ExecContext.Preferences.DefaultXmlMetaExportPath);
				if (sFilePath != null)
				{
					_ExecContext.Preferences.DefaultXmlMetaExportPath = sFilePath;
					_XmlExportToFilenameTextBox.Text = sFilePath;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void _LinkedAttachmentFolderSelectButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				using (FolderBrowserDialog oDlg = new FolderBrowserDialog())
				{
					oDlg.Description = "Attachment export folder";

					if (_LinkedAttachmentFolderTextBox.Text.Length > 0)
						oDlg.SelectedPath = _LinkedAttachmentFolderTextBox.Text;

					oDlg.ShowNewFolderButton = true;
					
					if (oDlg.ShowDialog(this) == DialogResult.OK)
					{
						_LinkedAttachmentFolderTextBox.Text = oDlg.SelectedPath;
						_ExecContext.Preferences.DefaultLinkedAttachmentExportFolder = _LinkedAttachmentFolderTextBox.Text;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}


	}
}
