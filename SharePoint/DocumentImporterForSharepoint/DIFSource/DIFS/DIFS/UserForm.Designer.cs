namespace DIFS
{
    partial class UserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserForm));
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPageImport = new System.Windows.Forms.TabPage();
            this.groupBoxImportSettings = new System.Windows.Forms.GroupBox();
            this.textBoxSettingsFile = new System.Windows.Forms.TextBox();
            this.buttonSaveSettingsAs = new System.Windows.Forms.Button();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.buttonLoadSettings = new System.Windows.Forms.Button();
            this.groupBoxImportControl = new System.Windows.Forms.GroupBox();
            this.buttonStartImport = new System.Windows.Forms.Button();
            this.buttonPauseImport = new System.Windows.Forms.Button();
            this.buttonResumeImport = new System.Windows.Forms.Button();
            this.buttonCancelImport = new System.Windows.Forms.Button();
            this.groupBoxImportSummary = new System.Windows.Forms.GroupBox();
            this.textBoxItemsFailed = new System.Windows.Forms.TextBox();
            this.textBoxItemsSucceeded = new System.Windows.Forms.TextBox();
            this.labelItemsFailed = new System.Windows.Forms.Label();
            this.labelItemsSucceeded = new System.Windows.Forms.Label();
            this.labelItemsProcessed = new System.Windows.Forms.Label();
            this.textBoxItemsProcessed = new System.Windows.Forms.TextBox();
            this.labelItemsCount = new System.Windows.Forms.Label();
            this.textItemsCount = new System.Windows.Forms.TextBox();
            this.progressBarImport = new System.Windows.Forms.ProgressBar();
            this.tabSource = new System.Windows.Forms.TabPage();
            this.groupBoxSourceFile = new System.Windows.Forms.GroupBox();
            this.buttonSourceFile = new System.Windows.Forms.Button();
            this.textBoxSourceFile = new System.Windows.Forms.TextBox();
            this.labelSourceFile = new System.Windows.Forms.Label();
            this.groupBoxSourceType = new System.Windows.Forms.GroupBox();
            this.radioButtonCSVFile = new System.Windows.Forms.RadioButton();
            this.radioButtonXMLDataSet = new System.Windows.Forms.RadioButton();
            this.radioButtonFileSystemFolder = new System.Windows.Forms.RadioButton();
            this.groupBoxSourceFolder = new System.Windows.Forms.GroupBox();
            this.labelSourceFolder = new System.Windows.Forms.Label();
            this.buttonSourceFolder = new System.Windows.Forms.Button();
            this.textBoxSourceFolder = new System.Windows.Forms.TextBox();
            this.tabDestination = new System.Windows.Forms.TabPage();
            this.groupBoxDestination = new System.Windows.Forms.GroupBox();
            this.labelDestinationLibraryName = new System.Windows.Forms.Label();
            this.textBoxDestinationLibraryName = new System.Windows.Forms.TextBox();
            this.labelDestinationServerUrl = new System.Windows.Forms.Label();
            this.labelDestinationFolderUrl = new System.Windows.Forms.Label();
            this.labelDestinationWebUrl = new System.Windows.Forms.Label();
            this.textBoxDestinationFolderURL = new System.Windows.Forms.TextBox();
            this.textBoxDestinationWebURL = new System.Windows.Forms.TextBox();
            this.textBoxDestinationServerURL = new System.Windows.Forms.TextBox();
            this.groupBoxSelectDestination = new System.Windows.Forms.GroupBox();
            this.textBoxSharePointSite = new System.Windows.Forms.TextBox();
            this.labelSharePointSite = new System.Windows.Forms.Label();
            this.buttonSharePointSite = new System.Windows.Forms.Button();
            this.tvLists = new System.Windows.Forms.TreeView();
            this.labelDestinationLibrary = new System.Windows.Forms.Label();
            this.tabMetaData = new System.Windows.Forms.TabPage();
            this.groupBoxAdditionalMetaData = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelAdditionalMetaData = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRefreshSharePoint = new System.Windows.Forms.Button();
            this.groupBoxMetaData = new System.Windows.Forms.GroupBox();
            this.panelModified = new System.Windows.Forms.Panel();
            this.radioButtonModifiedFromFileSystem = new System.Windows.Forms.RadioButton();
            this.radioButtonModifiedDefault = new System.Windows.Forms.RadioButton();
            this.labelModified = new System.Windows.Forms.Label();
            this.panelCreated = new System.Windows.Forms.Panel();
            this.radioButtonCreatedFromFileSystem = new System.Windows.Forms.RadioButton();
            this.radioButtonCreatedDefault = new System.Windows.Forms.RadioButton();
            this.labelCreated = new System.Windows.Forms.Label();
            this.tabAuthentication = new System.Windows.Forms.TabPage();
            this.groupBoxAuthentication = new System.Windows.Forms.GroupBox();
            this.panelAuthenticationCredentials = new System.Windows.Forms.Panel();
            this.textBoxAuthenticationPassword = new System.Windows.Forms.TextBox();
            this.labelAuthenticationPassword = new System.Windows.Forms.Label();
            this.textBoxAuthenticationUsername = new System.Windows.Forms.TextBox();
            this.labelAuthenticationUsername = new System.Windows.Forms.Label();
            this.textBoxAuthenticationDomain = new System.Windows.Forms.TextBox();
            this.labelAuthenticationDomain = new System.Windows.Forms.Label();
            this.panelAuthenticationMode = new System.Windows.Forms.Panel();
            this.radioButtonAuthenticationModeOffice365 = new System.Windows.Forms.RadioButton();
            this.radioButtonAuthenticationModeForms = new System.Windows.Forms.RadioButton();
            this.radioButtonAuthenticationModeSpecified = new System.Windows.Forms.RadioButton();
            this.radioButtonAuthenticationModeCurrent = new System.Windows.Forms.RadioButton();
            this.labelAuthenticationMode = new System.Windows.Forms.Label();
            this.tabLogging = new System.Windows.Forms.TabPage();
            this.groupBoxLogging = new System.Windows.Forms.GroupBox();
            this.checkBoxLoggingToFile = new System.Windows.Forms.CheckBox();
            this.tabExceptions = new System.Windows.Forms.TabPage();
            this.groupBoxExceptions = new System.Windows.Forms.GroupBox();
            this.labelSaveExceptions = new System.Windows.Forms.Label();
            this.buttonSaveExceptions = new System.Windows.Forms.Button();
            this.tabHelpAbout = new System.Windows.Forms.TabPage();
            this.groupboxHelp = new System.Windows.Forms.GroupBox();
            this.linkLabelHelp = new System.Windows.Forms.LinkLabel();
            this.groupBoxAbout = new System.Windows.Forms.GroupBox();
            this.textBoxVersion = new System.Windows.Forms.TextBox();
            this.labelAbout = new System.Windows.Forms.Label();
            this.folderBrowserDialogSourceFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.bgwDestination = new System.ComponentModel.BackgroundWorker();
            this.bgwImport = new System.ComponentModel.BackgroundWorker();
            this.saveFileDialogSaveSettings = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabMain.SuspendLayout();
            this.tabPageImport.SuspendLayout();
            this.groupBoxImportSettings.SuspendLayout();
            this.groupBoxImportControl.SuspendLayout();
            this.groupBoxImportSummary.SuspendLayout();
            this.tabSource.SuspendLayout();
            this.groupBoxSourceFile.SuspendLayout();
            this.groupBoxSourceType.SuspendLayout();
            this.groupBoxSourceFolder.SuspendLayout();
            this.tabDestination.SuspendLayout();
            this.groupBoxDestination.SuspendLayout();
            this.groupBoxSelectDestination.SuspendLayout();
            this.tabMetaData.SuspendLayout();
            this.groupBoxAdditionalMetaData.SuspendLayout();
            this.groupBoxMetaData.SuspendLayout();
            this.panelModified.SuspendLayout();
            this.panelCreated.SuspendLayout();
            this.tabAuthentication.SuspendLayout();
            this.groupBoxAuthentication.SuspendLayout();
            this.panelAuthenticationCredentials.SuspendLayout();
            this.panelAuthenticationMode.SuspendLayout();
            this.tabLogging.SuspendLayout();
            this.groupBoxLogging.SuspendLayout();
            this.tabExceptions.SuspendLayout();
            this.groupBoxExceptions.SuspendLayout();
            this.tabHelpAbout.SuspendLayout();
            this.groupboxHelp.SuspendLayout();
            this.groupBoxAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabPageImport);
            this.tabMain.Controls.Add(this.tabSource);
            this.tabMain.Controls.Add(this.tabDestination);
            this.tabMain.Controls.Add(this.tabMetaData);
            this.tabMain.Controls.Add(this.tabAuthentication);
            this.tabMain.Controls.Add(this.tabLogging);
            this.tabMain.Controls.Add(this.tabExceptions);
            this.tabMain.Controls.Add(this.tabHelpAbout);
            this.tabMain.Location = new System.Drawing.Point(3, 5);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(765, 300);
            this.tabMain.TabIndex = 0;
            // 
            // tabPageImport
            // 
            this.tabPageImport.Controls.Add(this.groupBoxImportSettings);
            this.tabPageImport.Controls.Add(this.groupBoxImportControl);
            this.tabPageImport.Controls.Add(this.groupBoxImportSummary);
            this.tabPageImport.Location = new System.Drawing.Point(4, 22);
            this.tabPageImport.Name = "tabPageImport";
            this.tabPageImport.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImport.Size = new System.Drawing.Size(757, 274);
            this.tabPageImport.TabIndex = 0;
            this.tabPageImport.Text = "Import";
            this.tabPageImport.UseVisualStyleBackColor = true;
            // 
            // groupBoxImportSettings
            // 
            this.groupBoxImportSettings.Controls.Add(this.textBoxSettingsFile);
            this.groupBoxImportSettings.Controls.Add(this.buttonSaveSettingsAs);
            this.groupBoxImportSettings.Controls.Add(this.buttonSaveSettings);
            this.groupBoxImportSettings.Controls.Add(this.buttonLoadSettings);
            this.groupBoxImportSettings.Location = new System.Drawing.Point(20, 10);
            this.groupBoxImportSettings.Name = "groupBoxImportSettings";
            this.groupBoxImportSettings.Size = new System.Drawing.Size(720, 60);
            this.groupBoxImportSettings.TabIndex = 10;
            this.groupBoxImportSettings.TabStop = false;
            this.groupBoxImportSettings.Text = "Import Settings";
            // 
            // textBoxSettingsFile
            // 
            this.textBoxSettingsFile.Location = new System.Drawing.Point(530, 20);
            this.textBoxSettingsFile.Name = "textBoxSettingsFile";
            this.textBoxSettingsFile.ReadOnly = true;
            this.textBoxSettingsFile.Size = new System.Drawing.Size(150, 20);
            this.textBoxSettingsFile.TabIndex = 4;
            // 
            // buttonSaveSettingsAs
            // 
            this.buttonSaveSettingsAs.Location = new System.Drawing.Point(360, 20);
            this.buttonSaveSettingsAs.Name = "buttonSaveSettingsAs";
            this.buttonSaveSettingsAs.Size = new System.Drawing.Size(150, 25);
            this.buttonSaveSettingsAs.TabIndex = 3;
            this.buttonSaveSettingsAs.Text = "Save Settings As";
            this.buttonSaveSettingsAs.UseVisualStyleBackColor = true;
            this.buttonSaveSettingsAs.Click += new System.EventHandler(this.buttonSaveSettingsAs_Click);
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Enabled = false;
            this.buttonSaveSettings.Location = new System.Drawing.Point(190, 20);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(150, 25);
            this.buttonSaveSettings.TabIndex = 2;
            this.buttonSaveSettings.Text = "Save Settings";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // buttonLoadSettings
            // 
            this.buttonLoadSettings.Location = new System.Drawing.Point(20, 20);
            this.buttonLoadSettings.Name = "buttonLoadSettings";
            this.buttonLoadSettings.Size = new System.Drawing.Size(150, 25);
            this.buttonLoadSettings.TabIndex = 0;
            this.buttonLoadSettings.Text = "Load Settings";
            this.buttonLoadSettings.UseVisualStyleBackColor = true;
            this.buttonLoadSettings.Click += new System.EventHandler(this.buttonLoadSettings_Click);
            // 
            // groupBoxImportControl
            // 
            this.groupBoxImportControl.Controls.Add(this.buttonStartImport);
            this.groupBoxImportControl.Controls.Add(this.buttonPauseImport);
            this.groupBoxImportControl.Controls.Add(this.buttonResumeImport);
            this.groupBoxImportControl.Controls.Add(this.buttonCancelImport);
            this.groupBoxImportControl.Location = new System.Drawing.Point(20, 75);
            this.groupBoxImportControl.Name = "groupBoxImportControl";
            this.groupBoxImportControl.Size = new System.Drawing.Size(720, 60);
            this.groupBoxImportControl.TabIndex = 9;
            this.groupBoxImportControl.TabStop = false;
            this.groupBoxImportControl.Text = "Import Control";
            // 
            // buttonStartImport
            // 
            this.buttonStartImport.Location = new System.Drawing.Point(20, 20);
            this.buttonStartImport.Name = "buttonStartImport";
            this.buttonStartImport.Size = new System.Drawing.Size(150, 25);
            this.buttonStartImport.TabIndex = 0;
            this.buttonStartImport.Text = "Start Import";
            this.buttonStartImport.UseVisualStyleBackColor = true;
            this.buttonStartImport.Click += new System.EventHandler(this.buttonStartImport_Click);
            // 
            // buttonPauseImport
            // 
            this.buttonPauseImport.Enabled = false;
            this.buttonPauseImport.Location = new System.Drawing.Point(190, 20);
            this.buttonPauseImport.Name = "buttonPauseImport";
            this.buttonPauseImport.Size = new System.Drawing.Size(150, 25);
            this.buttonPauseImport.TabIndex = 2;
            this.buttonPauseImport.Text = "Pause Import";
            this.buttonPauseImport.UseVisualStyleBackColor = true;
            this.buttonPauseImport.Click += new System.EventHandler(this.buttonPauseImport_Click);
            // 
            // buttonResumeImport
            // 
            this.buttonResumeImport.Enabled = false;
            this.buttonResumeImport.Location = new System.Drawing.Point(530, 20);
            this.buttonResumeImport.Name = "buttonResumeImport";
            this.buttonResumeImport.Size = new System.Drawing.Size(150, 25);
            this.buttonResumeImport.TabIndex = 4;
            this.buttonResumeImport.Text = "Resume Import";
            this.buttonResumeImport.UseVisualStyleBackColor = true;
            this.buttonResumeImport.Click += new System.EventHandler(this.buttonResumeImport_Click);
            // 
            // buttonCancelImport
            // 
            this.buttonCancelImport.Enabled = false;
            this.buttonCancelImport.Location = new System.Drawing.Point(360, 20);
            this.buttonCancelImport.Name = "buttonCancelImport";
            this.buttonCancelImport.Size = new System.Drawing.Size(150, 25);
            this.buttonCancelImport.TabIndex = 5;
            this.buttonCancelImport.Text = "Cancel Import";
            this.buttonCancelImport.UseVisualStyleBackColor = true;
            this.buttonCancelImport.Click += new System.EventHandler(this.buttonCancelImport_Click);
            // 
            // groupBoxImportSummary
            // 
            this.groupBoxImportSummary.Controls.Add(this.textBoxItemsFailed);
            this.groupBoxImportSummary.Controls.Add(this.textBoxItemsSucceeded);
            this.groupBoxImportSummary.Controls.Add(this.labelItemsFailed);
            this.groupBoxImportSummary.Controls.Add(this.labelItemsSucceeded);
            this.groupBoxImportSummary.Controls.Add(this.labelItemsProcessed);
            this.groupBoxImportSummary.Controls.Add(this.textBoxItemsProcessed);
            this.groupBoxImportSummary.Controls.Add(this.labelItemsCount);
            this.groupBoxImportSummary.Controls.Add(this.textItemsCount);
            this.groupBoxImportSummary.Controls.Add(this.progressBarImport);
            this.groupBoxImportSummary.Location = new System.Drawing.Point(20, 140);
            this.groupBoxImportSummary.Name = "groupBoxImportSummary";
            this.groupBoxImportSummary.Size = new System.Drawing.Size(720, 120);
            this.groupBoxImportSummary.TabIndex = 8;
            this.groupBoxImportSummary.TabStop = false;
            this.groupBoxImportSummary.Text = "Import Summary";
            // 
            // textBoxItemsFailed
            // 
            this.textBoxItemsFailed.Enabled = false;
            this.textBoxItemsFailed.Location = new System.Drawing.Point(340, 90);
            this.textBoxItemsFailed.Name = "textBoxItemsFailed";
            this.textBoxItemsFailed.Size = new System.Drawing.Size(100, 20);
            this.textBoxItemsFailed.TabIndex = 11;
            // 
            // textBoxItemsSucceeded
            // 
            this.textBoxItemsSucceeded.Enabled = false;
            this.textBoxItemsSucceeded.Location = new System.Drawing.Point(340, 60);
            this.textBoxItemsSucceeded.Name = "textBoxItemsSucceeded";
            this.textBoxItemsSucceeded.Size = new System.Drawing.Size(100, 20);
            this.textBoxItemsSucceeded.TabIndex = 10;
            // 
            // labelItemsFailed
            // 
            this.labelItemsFailed.AutoSize = true;
            this.labelItemsFailed.Location = new System.Drawing.Point(242, 93);
            this.labelItemsFailed.Name = "labelItemsFailed";
            this.labelItemsFailed.Size = new System.Drawing.Size(63, 13);
            this.labelItemsFailed.TabIndex = 2;
            this.labelItemsFailed.Text = "Items Failed";
            // 
            // labelItemsSucceeded
            // 
            this.labelItemsSucceeded.AutoSize = true;
            this.labelItemsSucceeded.Location = new System.Drawing.Point(242, 63);
            this.labelItemsSucceeded.Name = "labelItemsSucceeded";
            this.labelItemsSucceeded.Size = new System.Drawing.Size(90, 13);
            this.labelItemsSucceeded.TabIndex = 3;
            this.labelItemsSucceeded.Text = "Items Succeeded";
            // 
            // labelItemsProcessed
            // 
            this.labelItemsProcessed.AutoSize = true;
            this.labelItemsProcessed.Location = new System.Drawing.Point(20, 93);
            this.labelItemsProcessed.Name = "labelItemsProcessed";
            this.labelItemsProcessed.Size = new System.Drawing.Size(85, 13);
            this.labelItemsProcessed.TabIndex = 9;
            this.labelItemsProcessed.Text = "Items Processed";
            // 
            // textBoxItemsProcessed
            // 
            this.textBoxItemsProcessed.Enabled = false;
            this.textBoxItemsProcessed.Location = new System.Drawing.Point(120, 90);
            this.textBoxItemsProcessed.Name = "textBoxItemsProcessed";
            this.textBoxItemsProcessed.Size = new System.Drawing.Size(100, 20);
            this.textBoxItemsProcessed.TabIndex = 8;
            // 
            // labelItemsCount
            // 
            this.labelItemsCount.AutoSize = true;
            this.labelItemsCount.Location = new System.Drawing.Point(20, 63);
            this.labelItemsCount.Name = "labelItemsCount";
            this.labelItemsCount.Size = new System.Drawing.Size(63, 13);
            this.labelItemsCount.TabIndex = 7;
            this.labelItemsCount.Text = "Items Count";
            // 
            // textItemsCount
            // 
            this.textItemsCount.Enabled = false;
            this.textItemsCount.Location = new System.Drawing.Point(120, 60);
            this.textItemsCount.Name = "textItemsCount";
            this.textItemsCount.Size = new System.Drawing.Size(100, 20);
            this.textItemsCount.TabIndex = 3;
            // 
            // progressBarImport
            // 
            this.progressBarImport.Location = new System.Drawing.Point(20, 20);
            this.progressBarImport.Name = "progressBarImport";
            this.progressBarImport.Size = new System.Drawing.Size(680, 25);
            this.progressBarImport.TabIndex = 6;
            // 
            // tabSource
            // 
            this.tabSource.AutoScroll = true;
            this.tabSource.Controls.Add(this.groupBoxSourceFile);
            this.tabSource.Controls.Add(this.groupBoxSourceType);
            this.tabSource.Controls.Add(this.groupBoxSourceFolder);
            this.tabSource.Location = new System.Drawing.Point(4, 22);
            this.tabSource.Name = "tabSource";
            this.tabSource.Padding = new System.Windows.Forms.Padding(3);
            this.tabSource.Size = new System.Drawing.Size(757, 274);
            this.tabSource.TabIndex = 1;
            this.tabSource.Text = "Source";
            this.tabSource.UseVisualStyleBackColor = true;
            // 
            // groupBoxSourceFile
            // 
            this.groupBoxSourceFile.Controls.Add(this.buttonSourceFile);
            this.groupBoxSourceFile.Controls.Add(this.textBoxSourceFile);
            this.groupBoxSourceFile.Controls.Add(this.labelSourceFile);
            this.groupBoxSourceFile.Location = new System.Drawing.Point(20, 150);
            this.groupBoxSourceFile.Name = "groupBoxSourceFile";
            this.groupBoxSourceFile.Size = new System.Drawing.Size(720, 100);
            this.groupBoxSourceFile.TabIndex = 4;
            this.groupBoxSourceFile.TabStop = false;
            this.groupBoxSourceFile.Text = "Source File";
            this.groupBoxSourceFile.Visible = false;
            // 
            // buttonSourceFile
            // 
            this.buttonSourceFile.Location = new System.Drawing.Point(540, 20);
            this.buttonSourceFile.Name = "buttonSourceFile";
            this.buttonSourceFile.Size = new System.Drawing.Size(150, 23);
            this.buttonSourceFile.TabIndex = 2;
            this.buttonSourceFile.Text = "Select";
            this.buttonSourceFile.UseVisualStyleBackColor = true;
            this.buttonSourceFile.Click += new System.EventHandler(this.buttonSourceXMLDataSet_Click);
            // 
            // textBoxSourceFile
            // 
            this.textBoxSourceFile.Location = new System.Drawing.Point(190, 20);
            this.textBoxSourceFile.Name = "textBoxSourceFile";
            this.textBoxSourceFile.Size = new System.Drawing.Size(250, 20);
            this.textBoxSourceFile.TabIndex = 1;
            // 
            // labelSourceFile
            // 
            this.labelSourceFile.AutoSize = true;
            this.labelSourceFile.Location = new System.Drawing.Point(10, 20);
            this.labelSourceFile.Name = "labelSourceFile";
            this.labelSourceFile.Size = new System.Drawing.Size(60, 13);
            this.labelSourceFile.TabIndex = 0;
            this.labelSourceFile.Text = "Source File";
            // 
            // groupBoxSourceType
            // 
            this.groupBoxSourceType.Controls.Add(this.radioButtonCSVFile);
            this.groupBoxSourceType.Controls.Add(this.radioButtonXMLDataSet);
            this.groupBoxSourceType.Controls.Add(this.radioButtonFileSystemFolder);
            this.groupBoxSourceType.Location = new System.Drawing.Point(20, 10);
            this.groupBoxSourceType.Name = "groupBoxSourceType";
            this.groupBoxSourceType.Size = new System.Drawing.Size(720, 60);
            this.groupBoxSourceType.TabIndex = 3;
            this.groupBoxSourceType.TabStop = false;
            this.groupBoxSourceType.Text = "Source Type";
            // 
            // radioButtonCSVFile
            // 
            this.radioButtonCSVFile.AutoSize = true;
            this.radioButtonCSVFile.Location = new System.Drawing.Point(310, 20);
            this.radioButtonCSVFile.Name = "radioButtonCSVFile";
            this.radioButtonCSVFile.Size = new System.Drawing.Size(65, 17);
            this.radioButtonCSVFile.TabIndex = 2;
            this.radioButtonCSVFile.TabStop = true;
            this.radioButtonCSVFile.Text = "CSV File";
            this.radioButtonCSVFile.UseVisualStyleBackColor = true;
            this.radioButtonCSVFile.CheckedChanged += new System.EventHandler(this.radioButtonCSVFile_CheckedChanged);
            // 
            // radioButtonXMLDataSet
            // 
            this.radioButtonXMLDataSet.AutoSize = true;
            this.radioButtonXMLDataSet.Location = new System.Drawing.Point(140, 20);
            this.radioButtonXMLDataSet.Name = "radioButtonXMLDataSet";
            this.radioButtonXMLDataSet.Size = new System.Drawing.Size(155, 17);
            this.radioButtonXMLDataSet.TabIndex = 1;
            this.radioButtonXMLDataSet.Text = "Exceptions / XML Data Set";
            this.radioButtonXMLDataSet.UseVisualStyleBackColor = true;
            this.radioButtonXMLDataSet.CheckedChanged += new System.EventHandler(this.radioButtonXMLDataSet_CheckedChanged);
            // 
            // radioButtonFileSystemFolder
            // 
            this.radioButtonFileSystemFolder.AutoSize = true;
            this.radioButtonFileSystemFolder.Checked = true;
            this.radioButtonFileSystemFolder.Location = new System.Drawing.Point(10, 20);
            this.radioButtonFileSystemFolder.Name = "radioButtonFileSystemFolder";
            this.radioButtonFileSystemFolder.Size = new System.Drawing.Size(110, 17);
            this.radioButtonFileSystemFolder.TabIndex = 0;
            this.radioButtonFileSystemFolder.TabStop = true;
            this.radioButtonFileSystemFolder.Text = "File System Folder";
            this.radioButtonFileSystemFolder.UseVisualStyleBackColor = true;
            this.radioButtonFileSystemFolder.CheckedChanged += new System.EventHandler(this.radioButtonFileSystemFolder_CheckedChanged);
            // 
            // groupBoxSourceFolder
            // 
            this.groupBoxSourceFolder.Controls.Add(this.labelSourceFolder);
            this.groupBoxSourceFolder.Controls.Add(this.buttonSourceFolder);
            this.groupBoxSourceFolder.Controls.Add(this.textBoxSourceFolder);
            this.groupBoxSourceFolder.Location = new System.Drawing.Point(20, 80);
            this.groupBoxSourceFolder.Name = "groupBoxSourceFolder";
            this.groupBoxSourceFolder.Size = new System.Drawing.Size(720, 60);
            this.groupBoxSourceFolder.TabIndex = 2;
            this.groupBoxSourceFolder.TabStop = false;
            this.groupBoxSourceFolder.Text = "Source Folder";
            // 
            // labelSourceFolder
            // 
            this.labelSourceFolder.AutoSize = true;
            this.labelSourceFolder.Location = new System.Drawing.Point(10, 20);
            this.labelSourceFolder.Name = "labelSourceFolder";
            this.labelSourceFolder.Size = new System.Drawing.Size(73, 13);
            this.labelSourceFolder.TabIndex = 0;
            this.labelSourceFolder.Text = "Source Folder";
            this.labelSourceFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonSourceFolder
            // 
            this.buttonSourceFolder.Location = new System.Drawing.Point(540, 20);
            this.buttonSourceFolder.Name = "buttonSourceFolder";
            this.buttonSourceFolder.Size = new System.Drawing.Size(150, 23);
            this.buttonSourceFolder.TabIndex = 1;
            this.buttonSourceFolder.Text = "Select";
            this.buttonSourceFolder.UseVisualStyleBackColor = true;
            this.buttonSourceFolder.Click += new System.EventHandler(this.buttonSourceFolder_Click);
            // 
            // textBoxSourceFolder
            // 
            this.textBoxSourceFolder.Location = new System.Drawing.Point(190, 20);
            this.textBoxSourceFolder.Name = "textBoxSourceFolder";
            this.textBoxSourceFolder.Size = new System.Drawing.Size(250, 20);
            this.textBoxSourceFolder.TabIndex = 1;
            // 
            // tabDestination
            // 
            this.tabDestination.AutoScroll = true;
            this.tabDestination.Controls.Add(this.groupBoxDestination);
            this.tabDestination.Controls.Add(this.groupBoxSelectDestination);
            this.tabDestination.Location = new System.Drawing.Point(4, 22);
            this.tabDestination.Name = "tabDestination";
            this.tabDestination.Padding = new System.Windows.Forms.Padding(3);
            this.tabDestination.Size = new System.Drawing.Size(757, 274);
            this.tabDestination.TabIndex = 2;
            this.tabDestination.Text = "Destination";
            this.tabDestination.UseVisualStyleBackColor = true;
            // 
            // groupBoxDestination
            // 
            this.groupBoxDestination.Controls.Add(this.labelDestinationLibraryName);
            this.groupBoxDestination.Controls.Add(this.textBoxDestinationLibraryName);
            this.groupBoxDestination.Controls.Add(this.labelDestinationServerUrl);
            this.groupBoxDestination.Controls.Add(this.labelDestinationFolderUrl);
            this.groupBoxDestination.Controls.Add(this.labelDestinationWebUrl);
            this.groupBoxDestination.Controls.Add(this.textBoxDestinationFolderURL);
            this.groupBoxDestination.Controls.Add(this.textBoxDestinationWebURL);
            this.groupBoxDestination.Controls.Add(this.textBoxDestinationServerURL);
            this.groupBoxDestination.Location = new System.Drawing.Point(20, 150);
            this.groupBoxDestination.Name = "groupBoxDestination";
            this.groupBoxDestination.Size = new System.Drawing.Size(720, 140);
            this.groupBoxDestination.TabIndex = 5;
            this.groupBoxDestination.TabStop = false;
            this.groupBoxDestination.Text = "Destination";
            // 
            // labelDestinationLibraryName
            // 
            this.labelDestinationLibraryName.AutoSize = true;
            this.labelDestinationLibraryName.Location = new System.Drawing.Point(10, 110);
            this.labelDestinationLibraryName.Name = "labelDestinationLibraryName";
            this.labelDestinationLibraryName.Size = new System.Drawing.Size(125, 13);
            this.labelDestinationLibraryName.TabIndex = 12;
            this.labelDestinationLibraryName.Text = "Destination Library Name";
            // 
            // textBoxDestinationLibraryName
            // 
            this.textBoxDestinationLibraryName.Location = new System.Drawing.Point(190, 110);
            this.textBoxDestinationLibraryName.Name = "textBoxDestinationLibraryName";
            this.textBoxDestinationLibraryName.Size = new System.Drawing.Size(500, 20);
            this.textBoxDestinationLibraryName.TabIndex = 11;
            // 
            // labelDestinationServerUrl
            // 
            this.labelDestinationServerUrl.AutoSize = true;
            this.labelDestinationServerUrl.Location = new System.Drawing.Point(10, 83);
            this.labelDestinationServerUrl.Name = "labelDestinationServerUrl";
            this.labelDestinationServerUrl.Size = new System.Drawing.Size(160, 13);
            this.labelDestinationServerUrl.TabIndex = 10;
            this.labelDestinationServerUrl.Text = "Destination Server Url (Absolute)";
            // 
            // labelDestinationFolderUrl
            // 
            this.labelDestinationFolderUrl.AutoSize = true;
            this.labelDestinationFolderUrl.Location = new System.Drawing.Point(10, 53);
            this.labelDestinationFolderUrl.Name = "labelDestinationFolderUrl";
            this.labelDestinationFolderUrl.Size = new System.Drawing.Size(156, 13);
            this.labelDestinationFolderUrl.TabIndex = 9;
            this.labelDestinationFolderUrl.Text = "Destination Folder Url (Relative)";
            // 
            // labelDestinationWebUrl
            // 
            this.labelDestinationWebUrl.AutoSize = true;
            this.labelDestinationWebUrl.Location = new System.Drawing.Point(10, 23);
            this.labelDestinationWebUrl.Name = "labelDestinationWebUrl";
            this.labelDestinationWebUrl.Size = new System.Drawing.Size(150, 13);
            this.labelDestinationWebUrl.TabIndex = 8;
            this.labelDestinationWebUrl.Text = "Destination Web Url (Relative)";
            // 
            // textBoxDestinationFolderURL
            // 
            this.textBoxDestinationFolderURL.Location = new System.Drawing.Point(190, 50);
            this.textBoxDestinationFolderURL.Name = "textBoxDestinationFolderURL";
            this.textBoxDestinationFolderURL.Size = new System.Drawing.Size(500, 20);
            this.textBoxDestinationFolderURL.TabIndex = 6;
            // 
            // textBoxDestinationWebURL
            // 
            this.textBoxDestinationWebURL.Location = new System.Drawing.Point(190, 20);
            this.textBoxDestinationWebURL.Name = "textBoxDestinationWebURL";
            this.textBoxDestinationWebURL.Size = new System.Drawing.Size(500, 20);
            this.textBoxDestinationWebURL.TabIndex = 5;
            // 
            // textBoxDestinationServerURL
            // 
            this.textBoxDestinationServerURL.Location = new System.Drawing.Point(190, 80);
            this.textBoxDestinationServerURL.Name = "textBoxDestinationServerURL";
            this.textBoxDestinationServerURL.Size = new System.Drawing.Size(500, 20);
            this.textBoxDestinationServerURL.TabIndex = 7;
            // 
            // groupBoxSelectDestination
            // 
            this.groupBoxSelectDestination.Controls.Add(this.textBoxSharePointSite);
            this.groupBoxSelectDestination.Controls.Add(this.labelSharePointSite);
            this.groupBoxSelectDestination.Controls.Add(this.buttonSharePointSite);
            this.groupBoxSelectDestination.Controls.Add(this.tvLists);
            this.groupBoxSelectDestination.Controls.Add(this.labelDestinationLibrary);
            this.groupBoxSelectDestination.Location = new System.Drawing.Point(20, 10);
            this.groupBoxSelectDestination.Name = "groupBoxSelectDestination";
            this.groupBoxSelectDestination.Size = new System.Drawing.Size(720, 140);
            this.groupBoxSelectDestination.TabIndex = 8;
            this.groupBoxSelectDestination.TabStop = false;
            this.groupBoxSelectDestination.Text = "Select Destination";
            // 
            // textBoxSharePointSite
            // 
            this.textBoxSharePointSite.Location = new System.Drawing.Point(190, 20);
            this.textBoxSharePointSite.Name = "textBoxSharePointSite";
            this.textBoxSharePointSite.Size = new System.Drawing.Size(250, 20);
            this.textBoxSharePointSite.TabIndex = 1;
            // 
            // labelSharePointSite
            // 
            this.labelSharePointSite.AutoSize = true;
            this.labelSharePointSite.Location = new System.Drawing.Point(10, 20);
            this.labelSharePointSite.Name = "labelSharePointSite";
            this.labelSharePointSite.Size = new System.Drawing.Size(80, 13);
            this.labelSharePointSite.TabIndex = 0;
            this.labelSharePointSite.Text = "SharePoint Site";
            // 
            // buttonSharePointSite
            // 
            this.buttonSharePointSite.Location = new System.Drawing.Point(540, 20);
            this.buttonSharePointSite.Name = "buttonSharePointSite";
            this.buttonSharePointSite.Size = new System.Drawing.Size(150, 23);
            this.buttonSharePointSite.TabIndex = 2;
            this.buttonSharePointSite.Text = "Load";
            this.buttonSharePointSite.UseVisualStyleBackColor = true;
            this.buttonSharePointSite.Click += new System.EventHandler(this.buttonSharePointSite_Click);
            // 
            // tvLists
            // 
            this.tvLists.Location = new System.Drawing.Point(190, 50);
            this.tvLists.Name = "tvLists";
            this.tvLists.PathSeparator = "/";
            this.tvLists.Size = new System.Drawing.Size(500, 80);
            this.tvLists.TabIndex = 3;
            this.tvLists.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvLists_AfterSelect);
            // 
            // labelDestinationLibrary
            // 
            this.labelDestinationLibrary.AutoSize = true;
            this.labelDestinationLibrary.Location = new System.Drawing.Point(10, 50);
            this.labelDestinationLibrary.Name = "labelDestinationLibrary";
            this.labelDestinationLibrary.Size = new System.Drawing.Size(94, 13);
            this.labelDestinationLibrary.TabIndex = 4;
            this.labelDestinationLibrary.Text = "Destination Library";
            // 
            // tabMetaData
            // 
            this.tabMetaData.AutoScroll = true;
            this.tabMetaData.Controls.Add(this.groupBoxAdditionalMetaData);
            this.tabMetaData.Controls.Add(this.groupBoxMetaData);
            this.tabMetaData.Location = new System.Drawing.Point(4, 22);
            this.tabMetaData.Name = "tabMetaData";
            this.tabMetaData.Padding = new System.Windows.Forms.Padding(3);
            this.tabMetaData.Size = new System.Drawing.Size(757, 274);
            this.tabMetaData.TabIndex = 5;
            this.tabMetaData.Text = "Meta Data";
            this.tabMetaData.UseVisualStyleBackColor = true;
            // 
            // groupBoxAdditionalMetaData
            // 
            this.groupBoxAdditionalMetaData.AutoSize = true;
            this.groupBoxAdditionalMetaData.Controls.Add(this.tableLayoutPanelAdditionalMetaData);
            this.groupBoxAdditionalMetaData.Controls.Add(this.buttonRefreshSharePoint);
            this.groupBoxAdditionalMetaData.Location = new System.Drawing.Point(20, 120);
            this.groupBoxAdditionalMetaData.Name = "groupBoxAdditionalMetaData";
            this.groupBoxAdditionalMetaData.Size = new System.Drawing.Size(720, 146);
            this.groupBoxAdditionalMetaData.TabIndex = 1;
            this.groupBoxAdditionalMetaData.TabStop = false;
            this.groupBoxAdditionalMetaData.Text = "Additional Document Meta Data";
            this.groupBoxAdditionalMetaData.Visible = false;
            // 
            // tableLayoutPanelAdditionalMetaData
            // 
            this.tableLayoutPanelAdditionalMetaData.AutoSize = true;
            this.tableLayoutPanelAdditionalMetaData.ColumnCount = 2;
            this.tableLayoutPanelAdditionalMetaData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAdditionalMetaData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAdditionalMetaData.Location = new System.Drawing.Point(10, 58);
            this.tableLayoutPanelAdditionalMetaData.Name = "tableLayoutPanelAdditionalMetaData";
            this.tableLayoutPanelAdditionalMetaData.RowCount = 4;
            this.tableLayoutPanelAdditionalMetaData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAdditionalMetaData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAdditionalMetaData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelAdditionalMetaData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelAdditionalMetaData.Size = new System.Drawing.Size(360, 69);
            this.tableLayoutPanelAdditionalMetaData.TabIndex = 1;
            // 
            // buttonRefreshSharePoint
            // 
            this.buttonRefreshSharePoint.Location = new System.Drawing.Point(10, 20);
            this.buttonRefreshSharePoint.Name = "buttonRefreshSharePoint";
            this.buttonRefreshSharePoint.Size = new System.Drawing.Size(159, 23);
            this.buttonRefreshSharePoint.TabIndex = 0;
            this.buttonRefreshSharePoint.Text = "Refresh SharePoint";
            this.buttonRefreshSharePoint.UseVisualStyleBackColor = true;
            this.buttonRefreshSharePoint.Click += new System.EventHandler(this.buttonRefreshSharePoint_Click);
            // 
            // groupBoxMetaData
            // 
            this.groupBoxMetaData.Controls.Add(this.panelModified);
            this.groupBoxMetaData.Controls.Add(this.panelCreated);
            this.groupBoxMetaData.Location = new System.Drawing.Point(20, 20);
            this.groupBoxMetaData.Name = "groupBoxMetaData";
            this.groupBoxMetaData.Size = new System.Drawing.Size(720, 90);
            this.groupBoxMetaData.TabIndex = 0;
            this.groupBoxMetaData.TabStop = false;
            this.groupBoxMetaData.Text = "Standard Document Meta Data";
            // 
            // panelModified
            // 
            this.panelModified.Controls.Add(this.radioButtonModifiedFromFileSystem);
            this.panelModified.Controls.Add(this.radioButtonModifiedDefault);
            this.panelModified.Controls.Add(this.labelModified);
            this.panelModified.Location = new System.Drawing.Point(10, 50);
            this.panelModified.Name = "panelModified";
            this.panelModified.Size = new System.Drawing.Size(700, 25);
            this.panelModified.TabIndex = 2;
            // 
            // radioButtonModifiedFromFileSystem
            // 
            this.radioButtonModifiedFromFileSystem.AutoSize = true;
            this.radioButtonModifiedFromFileSystem.Location = new System.Drawing.Point(240, 5);
            this.radioButtonModifiedFromFileSystem.Name = "radioButtonModifiedFromFileSystem";
            this.radioButtonModifiedFromFileSystem.Size = new System.Drawing.Size(179, 17);
            this.radioButtonModifiedFromFileSystem.TabIndex = 2;
            this.radioButtonModifiedFromFileSystem.TabStop = true;
            this.radioButtonModifiedFromFileSystem.Text = "From File System (Modified Date)";
            this.radioButtonModifiedFromFileSystem.UseVisualStyleBackColor = true;
            // 
            // radioButtonModifiedDefault
            // 
            this.radioButtonModifiedDefault.AutoSize = true;
            this.radioButtonModifiedDefault.Checked = true;
            this.radioButtonModifiedDefault.Location = new System.Drawing.Point(100, 5);
            this.radioButtonModifiedDefault.Name = "radioButtonModifiedDefault";
            this.radioButtonModifiedDefault.Size = new System.Drawing.Size(59, 17);
            this.radioButtonModifiedDefault.TabIndex = 1;
            this.radioButtonModifiedDefault.TabStop = true;
            this.radioButtonModifiedDefault.Text = "Default";
            this.radioButtonModifiedDefault.UseVisualStyleBackColor = true;
            // 
            // labelModified
            // 
            this.labelModified.AutoSize = true;
            this.labelModified.Location = new System.Drawing.Point(0, 5);
            this.labelModified.Name = "labelModified";
            this.labelModified.Size = new System.Drawing.Size(47, 13);
            this.labelModified.TabIndex = 0;
            this.labelModified.Text = "Modified";
            // 
            // panelCreated
            // 
            this.panelCreated.Controls.Add(this.radioButtonCreatedFromFileSystem);
            this.panelCreated.Controls.Add(this.radioButtonCreatedDefault);
            this.panelCreated.Controls.Add(this.labelCreated);
            this.panelCreated.Location = new System.Drawing.Point(10, 20);
            this.panelCreated.Name = "panelCreated";
            this.panelCreated.Size = new System.Drawing.Size(700, 25);
            this.panelCreated.TabIndex = 1;
            // 
            // radioButtonCreatedFromFileSystem
            // 
            this.radioButtonCreatedFromFileSystem.AutoSize = true;
            this.radioButtonCreatedFromFileSystem.Location = new System.Drawing.Point(240, 5);
            this.radioButtonCreatedFromFileSystem.Name = "radioButtonCreatedFromFileSystem";
            this.radioButtonCreatedFromFileSystem.Size = new System.Drawing.Size(176, 17);
            this.radioButtonCreatedFromFileSystem.TabIndex = 2;
            this.radioButtonCreatedFromFileSystem.TabStop = true;
            this.radioButtonCreatedFromFileSystem.Text = "From File System (Created Date)";
            this.radioButtonCreatedFromFileSystem.UseVisualStyleBackColor = true;
            // 
            // radioButtonCreatedDefault
            // 
            this.radioButtonCreatedDefault.AutoSize = true;
            this.radioButtonCreatedDefault.Checked = true;
            this.radioButtonCreatedDefault.Location = new System.Drawing.Point(100, 5);
            this.radioButtonCreatedDefault.Name = "radioButtonCreatedDefault";
            this.radioButtonCreatedDefault.Size = new System.Drawing.Size(59, 17);
            this.radioButtonCreatedDefault.TabIndex = 1;
            this.radioButtonCreatedDefault.TabStop = true;
            this.radioButtonCreatedDefault.Text = "Default";
            this.radioButtonCreatedDefault.UseVisualStyleBackColor = true;
            // 
            // labelCreated
            // 
            this.labelCreated.AutoSize = true;
            this.labelCreated.Location = new System.Drawing.Point(0, 5);
            this.labelCreated.Name = "labelCreated";
            this.labelCreated.Size = new System.Drawing.Size(44, 13);
            this.labelCreated.TabIndex = 0;
            this.labelCreated.Text = "Created";
            // 
            // tabAuthentication
            // 
            this.tabAuthentication.Controls.Add(this.groupBoxAuthentication);
            this.tabAuthentication.Location = new System.Drawing.Point(4, 22);
            this.tabAuthentication.Name = "tabAuthentication";
            this.tabAuthentication.Padding = new System.Windows.Forms.Padding(3);
            this.tabAuthentication.Size = new System.Drawing.Size(757, 274);
            this.tabAuthentication.TabIndex = 7;
            this.tabAuthentication.Text = "Authentication";
            this.tabAuthentication.ToolTipText = "Authentication Settings";
            this.tabAuthentication.UseVisualStyleBackColor = true;
            // 
            // groupBoxAuthentication
            // 
            this.groupBoxAuthentication.Controls.Add(this.panelAuthenticationCredentials);
            this.groupBoxAuthentication.Controls.Add(this.panelAuthenticationMode);
            this.groupBoxAuthentication.Location = new System.Drawing.Point(20, 10);
            this.groupBoxAuthentication.Name = "groupBoxAuthentication";
            this.groupBoxAuthentication.Size = new System.Drawing.Size(720, 200);
            this.groupBoxAuthentication.TabIndex = 0;
            this.groupBoxAuthentication.TabStop = false;
            this.groupBoxAuthentication.Text = "Authentication";
            // 
            // panelAuthenticationCredentials
            // 
            this.panelAuthenticationCredentials.Controls.Add(this.textBoxAuthenticationPassword);
            this.panelAuthenticationCredentials.Controls.Add(this.labelAuthenticationPassword);
            this.panelAuthenticationCredentials.Controls.Add(this.textBoxAuthenticationUsername);
            this.panelAuthenticationCredentials.Controls.Add(this.labelAuthenticationUsername);
            this.panelAuthenticationCredentials.Controls.Add(this.textBoxAuthenticationDomain);
            this.panelAuthenticationCredentials.Controls.Add(this.labelAuthenticationDomain);
            this.panelAuthenticationCredentials.Enabled = false;
            this.panelAuthenticationCredentials.Location = new System.Drawing.Point(1, 60);
            this.panelAuthenticationCredentials.Name = "panelAuthenticationCredentials";
            this.panelAuthenticationCredentials.Size = new System.Drawing.Size(718, 100);
            this.panelAuthenticationCredentials.TabIndex = 1;
            // 
            // textBoxAuthenticationPassword
            // 
            this.textBoxAuthenticationPassword.Location = new System.Drawing.Point(200, 55);
            this.textBoxAuthenticationPassword.Name = "textBoxAuthenticationPassword";
            this.textBoxAuthenticationPassword.PasswordChar = '*';
            this.textBoxAuthenticationPassword.Size = new System.Drawing.Size(250, 20);
            this.textBoxAuthenticationPassword.TabIndex = 5;
            // 
            // labelAuthenticationPassword
            // 
            this.labelAuthenticationPassword.AutoSize = true;
            this.labelAuthenticationPassword.Location = new System.Drawing.Point(10, 55);
            this.labelAuthenticationPassword.Name = "labelAuthenticationPassword";
            this.labelAuthenticationPassword.Size = new System.Drawing.Size(53, 13);
            this.labelAuthenticationPassword.TabIndex = 4;
            this.labelAuthenticationPassword.Text = "Password";
            // 
            // textBoxAuthenticationUsername
            // 
            this.textBoxAuthenticationUsername.Location = new System.Drawing.Point(200, 30);
            this.textBoxAuthenticationUsername.Name = "textBoxAuthenticationUsername";
            this.textBoxAuthenticationUsername.Size = new System.Drawing.Size(250, 20);
            this.textBoxAuthenticationUsername.TabIndex = 3;
            // 
            // labelAuthenticationUsername
            // 
            this.labelAuthenticationUsername.AutoSize = true;
            this.labelAuthenticationUsername.Location = new System.Drawing.Point(10, 30);
            this.labelAuthenticationUsername.Name = "labelAuthenticationUsername";
            this.labelAuthenticationUsername.Size = new System.Drawing.Size(55, 13);
            this.labelAuthenticationUsername.TabIndex = 2;
            this.labelAuthenticationUsername.Text = "Username";
            // 
            // textBoxAuthenticationDomain
            // 
            this.textBoxAuthenticationDomain.Location = new System.Drawing.Point(200, 5);
            this.textBoxAuthenticationDomain.Name = "textBoxAuthenticationDomain";
            this.textBoxAuthenticationDomain.Size = new System.Drawing.Size(250, 20);
            this.textBoxAuthenticationDomain.TabIndex = 1;
            // 
            // labelAuthenticationDomain
            // 
            this.labelAuthenticationDomain.AutoSize = true;
            this.labelAuthenticationDomain.Location = new System.Drawing.Point(10, 5);
            this.labelAuthenticationDomain.Name = "labelAuthenticationDomain";
            this.labelAuthenticationDomain.Size = new System.Drawing.Size(43, 13);
            this.labelAuthenticationDomain.TabIndex = 0;
            this.labelAuthenticationDomain.Text = "Domain";
            // 
            // panelAuthenticationMode
            // 
            this.panelAuthenticationMode.Controls.Add(this.radioButtonAuthenticationModeOffice365);
            this.panelAuthenticationMode.Controls.Add(this.radioButtonAuthenticationModeForms);
            this.panelAuthenticationMode.Controls.Add(this.radioButtonAuthenticationModeSpecified);
            this.panelAuthenticationMode.Controls.Add(this.radioButtonAuthenticationModeCurrent);
            this.panelAuthenticationMode.Controls.Add(this.labelAuthenticationMode);
            this.panelAuthenticationMode.Location = new System.Drawing.Point(1, 20);
            this.panelAuthenticationMode.Name = "panelAuthenticationMode";
            this.panelAuthenticationMode.Size = new System.Drawing.Size(718, 30);
            this.panelAuthenticationMode.TabIndex = 0;
            // 
            // radioButtonAuthenticationModeOffice365
            // 
            this.radioButtonAuthenticationModeOffice365.AutoSize = true;
            this.radioButtonAuthenticationModeOffice365.Location = new System.Drawing.Point(590, 5);
            this.radioButtonAuthenticationModeOffice365.Name = "radioButtonAuthenticationModeOffice365";
            this.radioButtonAuthenticationModeOffice365.Size = new System.Drawing.Size(74, 17);
            this.radioButtonAuthenticationModeOffice365.TabIndex = 4;
            this.radioButtonAuthenticationModeOffice365.TabStop = true;
            this.radioButtonAuthenticationModeOffice365.Text = "Office 365";
            this.radioButtonAuthenticationModeOffice365.UseVisualStyleBackColor = true;
            this.radioButtonAuthenticationModeOffice365.CheckedChanged += new System.EventHandler(this.radioButtonAuthenticationModeOffice365_CheckedChanged);
            // 
            // radioButtonAuthenticationModeForms
            // 
            this.radioButtonAuthenticationModeForms.AutoSize = true;
            this.radioButtonAuthenticationModeForms.Location = new System.Drawing.Point(460, 5);
            this.radioButtonAuthenticationModeForms.Name = "radioButtonAuthenticationModeForms";
            this.radioButtonAuthenticationModeForms.Size = new System.Drawing.Size(124, 17);
            this.radioButtonAuthenticationModeForms.TabIndex = 3;
            this.radioButtonAuthenticationModeForms.Text = "Forms Authentication";
            this.radioButtonAuthenticationModeForms.UseVisualStyleBackColor = true;
            this.radioButtonAuthenticationModeForms.CheckedChanged += new System.EventHandler(this.radioButtonAuthenticationModeForms_CheckedChanged);
            // 
            // radioButtonAuthenticationModeSpecified
            // 
            this.radioButtonAuthenticationModeSpecified.AutoSize = true;
            this.radioButtonAuthenticationModeSpecified.Location = new System.Drawing.Point(330, 5);
            this.radioButtonAuthenticationModeSpecified.Name = "radioButtonAuthenticationModeSpecified";
            this.radioButtonAuthenticationModeSpecified.Size = new System.Drawing.Size(124, 17);
            this.radioButtonAuthenticationModeSpecified.TabIndex = 2;
            this.radioButtonAuthenticationModeSpecified.Text = "Specified Credentials";
            this.radioButtonAuthenticationModeSpecified.UseVisualStyleBackColor = true;
            this.radioButtonAuthenticationModeSpecified.CheckedChanged += new System.EventHandler(this.radioButtonAuthenticationModeSpecified_CheckedChanged);
            // 
            // radioButtonAuthenticationModeCurrent
            // 
            this.radioButtonAuthenticationModeCurrent.AutoSize = true;
            this.radioButtonAuthenticationModeCurrent.Checked = true;
            this.radioButtonAuthenticationModeCurrent.Location = new System.Drawing.Point(200, 5);
            this.radioButtonAuthenticationModeCurrent.Name = "radioButtonAuthenticationModeCurrent";
            this.radioButtonAuthenticationModeCurrent.Size = new System.Drawing.Size(114, 17);
            this.radioButtonAuthenticationModeCurrent.TabIndex = 1;
            this.radioButtonAuthenticationModeCurrent.TabStop = true;
            this.radioButtonAuthenticationModeCurrent.Text = "Current Credentials";
            this.radioButtonAuthenticationModeCurrent.UseVisualStyleBackColor = true;
            this.radioButtonAuthenticationModeCurrent.CheckedChanged += new System.EventHandler(this.radioButtonAuthenticationModeCurrent_CheckedChanged);
            // 
            // labelAuthenticationMode
            // 
            this.labelAuthenticationMode.AutoSize = true;
            this.labelAuthenticationMode.Location = new System.Drawing.Point(10, 5);
            this.labelAuthenticationMode.Name = "labelAuthenticationMode";
            this.labelAuthenticationMode.Size = new System.Drawing.Size(105, 13);
            this.labelAuthenticationMode.TabIndex = 0;
            this.labelAuthenticationMode.Text = "Authentication Mode";
            // 
            // tabLogging
            // 
            this.tabLogging.Controls.Add(this.groupBoxLogging);
            this.tabLogging.Location = new System.Drawing.Point(4, 22);
            this.tabLogging.Name = "tabLogging";
            this.tabLogging.Padding = new System.Windows.Forms.Padding(3);
            this.tabLogging.Size = new System.Drawing.Size(757, 274);
            this.tabLogging.TabIndex = 3;
            this.tabLogging.Text = "Logging";
            this.tabLogging.UseVisualStyleBackColor = true;
            // 
            // groupBoxLogging
            // 
            this.groupBoxLogging.Controls.Add(this.checkBoxLoggingToFile);
            this.groupBoxLogging.Location = new System.Drawing.Point(20, 10);
            this.groupBoxLogging.Name = "groupBoxLogging";
            this.groupBoxLogging.Size = new System.Drawing.Size(720, 140);
            this.groupBoxLogging.TabIndex = 0;
            this.groupBoxLogging.TabStop = false;
            this.groupBoxLogging.Text = "Logging";
            // 
            // checkBoxLoggingToFile
            // 
            this.checkBoxLoggingToFile.AutoSize = true;
            this.checkBoxLoggingToFile.Location = new System.Drawing.Point(20, 20);
            this.checkBoxLoggingToFile.Name = "checkBoxLoggingToFile";
            this.checkBoxLoggingToFile.Size = new System.Drawing.Size(300, 17);
            this.checkBoxLoggingToFile.TabIndex = 0;
            this.checkBoxLoggingToFile.Text = "Enable Logging To File (DIFS.exe.yyyyMMddHHmmss.log)";
            this.checkBoxLoggingToFile.UseVisualStyleBackColor = true;
            // 
            // tabExceptions
            // 
            this.tabExceptions.Controls.Add(this.groupBoxExceptions);
            this.tabExceptions.Location = new System.Drawing.Point(4, 22);
            this.tabExceptions.Name = "tabExceptions";
            this.tabExceptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabExceptions.Size = new System.Drawing.Size(757, 274);
            this.tabExceptions.TabIndex = 4;
            this.tabExceptions.Text = "Exceptions";
            this.tabExceptions.UseVisualStyleBackColor = true;
            // 
            // groupBoxExceptions
            // 
            this.groupBoxExceptions.Controls.Add(this.labelSaveExceptions);
            this.groupBoxExceptions.Controls.Add(this.buttonSaveExceptions);
            this.groupBoxExceptions.Location = new System.Drawing.Point(20, 10);
            this.groupBoxExceptions.Name = "groupBoxExceptions";
            this.groupBoxExceptions.Size = new System.Drawing.Size(720, 100);
            this.groupBoxExceptions.TabIndex = 0;
            this.groupBoxExceptions.TabStop = false;
            this.groupBoxExceptions.Text = "Exceptions";
            // 
            // labelSaveExceptions
            // 
            this.labelSaveExceptions.AutoSize = true;
            this.labelSaveExceptions.Location = new System.Drawing.Point(20, 20);
            this.labelSaveExceptions.Name = "labelSaveExceptions";
            this.labelSaveExceptions.Size = new System.Drawing.Size(371, 13);
            this.labelSaveExceptions.TabIndex = 1;
            this.labelSaveExceptions.Text = "Save current exceptions to file (DIFS.exe.Exceptions.yyyyMMddHHmmss.xml)";
            // 
            // buttonSaveExceptions
            // 
            this.buttonSaveExceptions.Location = new System.Drawing.Point(540, 20);
            this.buttonSaveExceptions.Name = "buttonSaveExceptions";
            this.buttonSaveExceptions.Size = new System.Drawing.Size(150, 23);
            this.buttonSaveExceptions.TabIndex = 0;
            this.buttonSaveExceptions.Text = "Save";
            this.buttonSaveExceptions.UseVisualStyleBackColor = true;
            this.buttonSaveExceptions.Click += new System.EventHandler(this.buttonSaveExceptions_Click);
            // 
            // tabHelpAbout
            // 
            this.tabHelpAbout.Controls.Add(this.groupboxHelp);
            this.tabHelpAbout.Controls.Add(this.groupBoxAbout);
            this.tabHelpAbout.Location = new System.Drawing.Point(4, 22);
            this.tabHelpAbout.Name = "tabHelpAbout";
            this.tabHelpAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabHelpAbout.Size = new System.Drawing.Size(757, 274);
            this.tabHelpAbout.TabIndex = 6;
            this.tabHelpAbout.Text = "Help & About";
            this.tabHelpAbout.ToolTipText = "Help and about this this application.";
            this.tabHelpAbout.UseVisualStyleBackColor = true;
            // 
            // groupboxHelp
            // 
            this.groupboxHelp.Controls.Add(this.linkLabelHelp);
            this.groupboxHelp.Location = new System.Drawing.Point(20, 70);
            this.groupboxHelp.Name = "groupboxHelp";
            this.groupboxHelp.Size = new System.Drawing.Size(720, 50);
            this.groupboxHelp.TabIndex = 1;
            this.groupboxHelp.TabStop = false;
            this.groupboxHelp.Text = "Help";
            // 
            // linkLabelHelp
            // 
            this.linkLabelHelp.ActiveLinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabelHelp.AutoSize = true;
            this.linkLabelHelp.Location = new System.Drawing.Point(10, 20);
            this.linkLabelHelp.Name = "linkLabelHelp";
            this.linkLabelHelp.Size = new System.Drawing.Size(105, 13);
            this.linkLabelHelp.TabIndex = 0;
            this.linkLabelHelp.TabStop = true;
            this.linkLabelHelp.Text = "Load help in browser";
            this.linkLabelHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelHelp_LinkClicked);
            // 
            // groupBoxAbout
            // 
            this.groupBoxAbout.Controls.Add(this.textBoxVersion);
            this.groupBoxAbout.Controls.Add(this.labelAbout);
            this.groupBoxAbout.Location = new System.Drawing.Point(20, 10);
            this.groupBoxAbout.Name = "groupBoxAbout";
            this.groupBoxAbout.Size = new System.Drawing.Size(720, 50);
            this.groupBoxAbout.TabIndex = 0;
            this.groupBoxAbout.TabStop = false;
            this.groupBoxAbout.Text = "About";
            // 
            // textBoxVersion
            // 
            this.textBoxVersion.Enabled = false;
            this.textBoxVersion.Location = new System.Drawing.Point(90, 16);
            this.textBoxVersion.Name = "textBoxVersion";
            this.textBoxVersion.Size = new System.Drawing.Size(100, 20);
            this.textBoxVersion.TabIndex = 1;
            // 
            // labelAbout
            // 
            this.labelAbout.AutoSize = true;
            this.labelAbout.Location = new System.Drawing.Point(10, 20);
            this.labelAbout.Name = "labelAbout";
            this.labelAbout.Size = new System.Drawing.Size(69, 13);
            this.labelAbout.TabIndex = 0;
            this.labelAbout.Text = "DIFS Version";
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Location = new System.Drawing.Point(3, 341);
            this.textBoxStatus.MaximumSize = new System.Drawing.Size(765, 80);
            this.textBoxStatus.MinimumSize = new System.Drawing.Size(765, 80);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.Size = new System.Drawing.Size(765, 80);
            this.textBoxStatus.TabIndex = 1;
            // 
            // bgwImport
            // 
            this.bgwImport.WorkerReportsProgress = true;
            // 
            // saveFileDialogSaveSettings
            // 
            this.saveFileDialogSaveSettings.DefaultExt = "XML";
            this.saveFileDialogSaveSettings.Filter = "XML Files|*.xml";
            this.saveFileDialogSaveSettings.Title = "Save As";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 433);
            this.Controls.Add(this.textBoxStatus);
            this.Controls.Add(this.tabMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(780, 460);
            this.MinimumSize = new System.Drawing.Size(780, 460);
            this.Name = "UserForm";
            this.Text = "Document Importer For SharePoint";
            this.tabMain.ResumeLayout(false);
            this.tabPageImport.ResumeLayout(false);
            this.groupBoxImportSettings.ResumeLayout(false);
            this.groupBoxImportSettings.PerformLayout();
            this.groupBoxImportControl.ResumeLayout(false);
            this.groupBoxImportSummary.ResumeLayout(false);
            this.groupBoxImportSummary.PerformLayout();
            this.tabSource.ResumeLayout(false);
            this.groupBoxSourceFile.ResumeLayout(false);
            this.groupBoxSourceFile.PerformLayout();
            this.groupBoxSourceType.ResumeLayout(false);
            this.groupBoxSourceType.PerformLayout();
            this.groupBoxSourceFolder.ResumeLayout(false);
            this.groupBoxSourceFolder.PerformLayout();
            this.tabDestination.ResumeLayout(false);
            this.groupBoxDestination.ResumeLayout(false);
            this.groupBoxDestination.PerformLayout();
            this.groupBoxSelectDestination.ResumeLayout(false);
            this.groupBoxSelectDestination.PerformLayout();
            this.tabMetaData.ResumeLayout(false);
            this.tabMetaData.PerformLayout();
            this.groupBoxAdditionalMetaData.ResumeLayout(false);
            this.groupBoxAdditionalMetaData.PerformLayout();
            this.groupBoxMetaData.ResumeLayout(false);
            this.panelModified.ResumeLayout(false);
            this.panelModified.PerformLayout();
            this.panelCreated.ResumeLayout(false);
            this.panelCreated.PerformLayout();
            this.tabAuthentication.ResumeLayout(false);
            this.groupBoxAuthentication.ResumeLayout(false);
            this.panelAuthenticationCredentials.ResumeLayout(false);
            this.panelAuthenticationCredentials.PerformLayout();
            this.panelAuthenticationMode.ResumeLayout(false);
            this.panelAuthenticationMode.PerformLayout();
            this.tabLogging.ResumeLayout(false);
            this.groupBoxLogging.ResumeLayout(false);
            this.groupBoxLogging.PerformLayout();
            this.tabExceptions.ResumeLayout(false);
            this.groupBoxExceptions.ResumeLayout(false);
            this.groupBoxExceptions.PerformLayout();
            this.tabHelpAbout.ResumeLayout(false);
            this.groupboxHelp.ResumeLayout(false);
            this.groupboxHelp.PerformLayout();
            this.groupBoxAbout.ResumeLayout(false);
            this.groupBoxAbout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPageImport;
        private System.Windows.Forms.TabPage tabSource;
        private System.Windows.Forms.TabPage tabDestination;
        private System.Windows.Forms.Button buttonSourceFolder;
        private System.Windows.Forms.TextBox textBoxSourceFolder;
        private System.Windows.Forms.Label labelSourceFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogSourceFolder;
        private System.Windows.Forms.TextBox textBoxSharePointSite;
        private System.Windows.Forms.Label labelSharePointSite;
        private System.Windows.Forms.Label labelDestinationLibrary;
        private System.Windows.Forms.TreeView tvLists;
        private System.Windows.Forms.Button buttonSharePointSite;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Button buttonStartImport;
        private System.Windows.Forms.TextBox textBoxDestinationWebURL;
        private System.ComponentModel.BackgroundWorker bgwDestination;
        private System.ComponentModel.BackgroundWorker bgwImport;
        private System.Windows.Forms.TextBox textBoxDestinationServerURL;
        private System.Windows.Forms.TextBox textBoxDestinationFolderURL;
        private System.Windows.Forms.GroupBox groupBoxImportSettings;
        private System.Windows.Forms.TextBox textBoxSettingsFile;
        private System.Windows.Forms.Button buttonSaveSettingsAs;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.Button buttonLoadSettings;
        private System.Windows.Forms.GroupBox groupBoxImportControl;
        private System.Windows.Forms.Button buttonPauseImport;
        private System.Windows.Forms.Button buttonResumeImport;
        private System.Windows.Forms.Button buttonCancelImport;
        private System.Windows.Forms.GroupBox groupBoxImportSummary;
        private System.Windows.Forms.Label labelItemsCount;
        private System.Windows.Forms.TextBox textItemsCount;
        private System.Windows.Forms.ProgressBar progressBarImport;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSaveSettings;
        private System.Windows.Forms.TextBox textBoxItemsProcessed;
        private System.Windows.Forms.TextBox textBoxItemsFailed;
        private System.Windows.Forms.TextBox textBoxItemsSucceeded;
        private System.Windows.Forms.Label labelItemsFailed;
        private System.Windows.Forms.Label labelItemsSucceeded;
        private System.Windows.Forms.Label labelItemsProcessed;
        private System.Windows.Forms.GroupBox groupBoxDestination;
        private System.Windows.Forms.Label labelDestinationServerUrl;
        private System.Windows.Forms.Label labelDestinationFolderUrl;
        private System.Windows.Forms.Label labelDestinationWebUrl;
        private System.Windows.Forms.GroupBox groupBoxSelectDestination;
        private System.Windows.Forms.TabPage tabLogging;
        private System.Windows.Forms.GroupBox groupBoxLogging;
        private System.Windows.Forms.CheckBox checkBoxLoggingToFile;
        private System.Windows.Forms.TabPage tabExceptions;
        private System.Windows.Forms.GroupBox groupBoxExceptions;
        private System.Windows.Forms.Button buttonSaveExceptions;
        private System.Windows.Forms.Label labelSaveExceptions;
        private System.Windows.Forms.GroupBox groupBoxSourceFile;
        private System.Windows.Forms.GroupBox groupBoxSourceType;
        private System.Windows.Forms.RadioButton radioButtonXMLDataSet;
        private System.Windows.Forms.RadioButton radioButtonFileSystemFolder;
        private System.Windows.Forms.GroupBox groupBoxSourceFolder;
        private System.Windows.Forms.Button buttonSourceFile;
        private System.Windows.Forms.TextBox textBoxSourceFile;
        private System.Windows.Forms.Label labelSourceFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabPage tabMetaData;
        private System.Windows.Forms.GroupBox groupBoxMetaData;
        private System.Windows.Forms.Panel panelCreated;
        private System.Windows.Forms.Label labelCreated;
        private System.Windows.Forms.Panel panelModified;
        private System.Windows.Forms.RadioButton radioButtonCreatedFromFileSystem;
        private System.Windows.Forms.RadioButton radioButtonCreatedDefault;
        private System.Windows.Forms.RadioButton radioButtonModifiedFromFileSystem;
        private System.Windows.Forms.RadioButton radioButtonModifiedDefault;
        private System.Windows.Forms.Label labelModified;
        private System.Windows.Forms.TabPage tabHelpAbout;
        private System.Windows.Forms.GroupBox groupBoxAbout;
        private System.Windows.Forms.TextBox textBoxVersion;
        private System.Windows.Forms.Label labelAbout;
        private System.Windows.Forms.GroupBox groupboxHelp;
        private System.Windows.Forms.LinkLabel linkLabelHelp;
        private System.Windows.Forms.TabPage tabAuthentication;
        private System.Windows.Forms.GroupBox groupBoxAuthentication;
        private System.Windows.Forms.Panel panelAuthenticationCredentials;
        private System.Windows.Forms.TextBox textBoxAuthenticationDomain;
        private System.Windows.Forms.Label labelAuthenticationDomain;
        private System.Windows.Forms.Panel panelAuthenticationMode;
        private System.Windows.Forms.RadioButton radioButtonAuthenticationModeForms;
        private System.Windows.Forms.RadioButton radioButtonAuthenticationModeSpecified;
        private System.Windows.Forms.RadioButton radioButtonAuthenticationModeCurrent;
        private System.Windows.Forms.Label labelAuthenticationMode;
        private System.Windows.Forms.Label labelAuthenticationUsername;
        private System.Windows.Forms.TextBox textBoxAuthenticationPassword;
        private System.Windows.Forms.Label labelAuthenticationPassword;
        private System.Windows.Forms.TextBox textBoxAuthenticationUsername;
        private System.Windows.Forms.RadioButton radioButtonCSVFile;
        private System.Windows.Forms.GroupBox groupBoxAdditionalMetaData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAdditionalMetaData;
        private System.Windows.Forms.Button buttonRefreshSharePoint;
        private System.Windows.Forms.Label labelDestinationLibraryName;
        private System.Windows.Forms.TextBox textBoxDestinationLibraryName;
        private System.Windows.Forms.RadioButton radioButtonAuthenticationModeOffice365;
    }
}

