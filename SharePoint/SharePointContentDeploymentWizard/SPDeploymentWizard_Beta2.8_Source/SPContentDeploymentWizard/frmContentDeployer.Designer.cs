namespace COB.SharePoint.Utilities.DeploymentWizard.UI
{
    partial class frmContentDeployer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmContentDeployer));
            this.f_dlgSelectExportFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.f_dlgSelectImportSingleFile = new System.Windows.Forms.OpenFileDialog();
            this.f_dlgSelectImportNoCompression = new System.Windows.Forms.FolderBrowserDialog();
            this.ctxtMenuExportItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeExportItemMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.f_dlgSaveSettingsFile = new System.Windows.Forms.SaveFileDialog();
            this.f_dlgLoadSettingsFile = new System.Windows.Forms.OpenFileDialog();
            this.wizardControl1 = new WizardBase.WizardControl();
            this.startStep1 = new WizardBase.StartStep();
            this.btnAbout = new System.Windows.Forms.Button();
            this.intermediateStep1 = new WizardBase.IntermediateStep();
            this.btnLoadSettings = new System.Windows.Forms.Button();
            this.gpActionSelect = new System.Windows.Forms.GroupBox();
            this.lblWhichAction = new System.Windows.Forms.Label();
            this.rdoImport = new System.Windows.Forms.RadioButton();
            this.rdoExport = new System.Windows.Forms.RadioButton();
            this.gpSiteAuthDetails = new System.Windows.Forms.GroupBox();
            this.lblWebUrl = new System.Windows.Forms.Label();
            this.txtWebUrl = new System.Windows.Forms.TextBox();
            this.lblSiteUrl = new System.Windows.Forms.Label();
            this.txtSiteUrl = new System.Windows.Forms.TextBox();
            this.intermediateStep2 = new WizardBase.IntermediateStep();
            this.gpImportSettings = new System.Windows.Forms.GroupBox();
            this.lblIncludeSecurityImport = new System.Windows.Forms.Label();
            this.cboIncludeSecurityImport = new System.Windows.Forms.ComboBox();
            this.lblImportLogfilePathValue = new System.Windows.Forms.Label();
            this.lblImportLogfilePath = new System.Windows.Forms.Label();
            this.cboVersionOptions = new System.Windows.Forms.ComboBox();
            this.lblVersionOptions = new System.Windows.Forms.Label();
            this.chkRetainIDs = new System.Windows.Forms.CheckBox();
            this.lblRetainID = new System.Windows.Forms.Label();
            this.gpSingleFile = new System.Windows.Forms.GroupBox();
            this.pnlImportNote = new System.Windows.Forms.Panel();
            this.lblImportNoteDetail = new System.Windows.Forms.Label();
            this.lblImportNote = new System.Windows.Forms.Label();
            this.lblImportPathValue = new System.Windows.Forms.Label();
            this.btnImportPathBrowse = new System.Windows.Forms.Button();
            this.lblImportPathConfig = new System.Windows.Forms.Label();
            this.lblImportType = new System.Windows.Forms.Label();
            this.cboUserInfo = new System.Windows.Forms.ComboBox();
            this.lblUpdateTimeDate = new System.Windows.Forms.Label();
            this.intermediateStep3 = new WizardBase.IntermediateStep();
            this.gpExportContents = new System.Windows.Forms.GroupBox();
            this.lblRetrievingData = new System.Windows.Forms.Label();
            this.trvContent = new System.Windows.Forms.TreeView();
            this.gpSelectedItems = new System.Windows.Forms.GroupBox();
            this.lstExportItems = new System.Windows.Forms.ListView();
            this.clmItem = new System.Windows.Forms.ColumnHeader();
            this.clmType = new System.Windows.Forms.ColumnHeader();
            this.clmIncDescs = new System.Windows.Forms.ColumnHeader();
            this.intermediateStep4 = new WizardBase.IntermediateStep();
            this.gbExportSettings = new System.Windows.Forms.GroupBox();
            this.lblCompressionNote = new System.Windows.Forms.Label();
            this.chkDisableCompression = new System.Windows.Forms.CheckBox();
            this.lblDisableCompression = new System.Windows.Forms.Label();
            this.chkExcludeDependencies = new System.Windows.Forms.CheckBox();
            this.lblExcludeDependencies = new System.Windows.Forms.Label();
            this.lblExportBaseFilenameValue = new System.Windows.Forms.Label();
            this.lblExportFile = new System.Windows.Forms.Label();
            this.txtExportBaseFilename = new System.Windows.Forms.TextBox();
            this.lblExportBaseFilename = new System.Windows.Forms.Label();
            this.lblExportLogPathValue = new System.Windows.Forms.Label();
            this.lblExportLogPath = new System.Windows.Forms.Label();
            this.cboIncludeSecurity = new System.Windows.Forms.ComboBox();
            this.cboIncludeVersions = new System.Windows.Forms.ComboBox();
            this.lblIncludeSecurityConfig = new System.Windows.Forms.Label();
            this.lblIncludeVersionsConfig = new System.Windows.Forms.Label();
            this.cboExportMethod = new System.Windows.Forms.ComboBox();
            this.lblExportMethodConfig = new System.Windows.Forms.Label();
            this.btnFilePathBrowse = new System.Windows.Forms.Button();
            this.lblExportFolderValue = new System.Windows.Forms.Label();
            this.lblExportFolder = new System.Windows.Forms.Label();
            this.finishStep1 = new WizardBase.FinishStep();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnReturnToStart = new System.Windows.Forms.Button();
            this.lblItemAction = new System.Windows.Forms.Label();
            this.prgImport = new System.Windows.Forms.ProgressBar();
            this.prgExport = new System.Windows.Forms.ProgressBar();
            this.pnlSettingsSummary = new System.Windows.Forms.Panel();
            this.pnlExportSettings = new System.Windows.Forms.Panel();
            this.lblExportLogFilePathValue = new System.Windows.Forms.LinkLabel();
            this.lblExportPathValue = new System.Windows.Forms.LinkLabel();
            this.lblExportMethodValue = new System.Windows.Forms.Label();
            this.lblIncludeVersionsExportValue = new System.Windows.Forms.Label();
            this.lblIncludeSecurityExportValue = new System.Windows.Forms.Label();
            this.lblExportObjectsValue = new System.Windows.Forms.Label();
            this.lblExportSiteValue = new System.Windows.Forms.Label();
            this.lblExportLogFilePath = new System.Windows.Forms.Label();
            this.lblExportPath = new System.Windows.Forms.Label();
            this.lblExportMethod = new System.Windows.Forms.Label();
            this.lblIncludeVersionsExport = new System.Windows.Forms.Label();
            this.lblIncludeSecurityExport = new System.Windows.Forms.Label();
            this.lblExportObjects = new System.Windows.Forms.Label();
            this.lblExportSite = new System.Windows.Forms.Label();
            this.pnlImportSettings = new System.Windows.Forms.Panel();
            this.lblImportLogFilePathValueFinish = new System.Windows.Forms.LinkLabel();
            this.lblImportPathValueFinish = new System.Windows.Forms.LinkLabel();
            this.lblUpdateVersionsValue = new System.Windows.Forms.Label();
            this.lblRetainObjectIdentityValue = new System.Windows.Forms.Label();
            this.lblIncludeSecurityImportValue = new System.Windows.Forms.Label();
            this.lblImportObjectsValue = new System.Windows.Forms.Label();
            this.lblImportSiteValue = new System.Windows.Forms.Label();
            this.lblImportLogFilePathFinish = new System.Windows.Forms.Label();
            this.lblImportPathFinish = new System.Windows.Forms.Label();
            this.lblUpdateVersions = new System.Windows.Forms.Label();
            this.lblRetainObjectIdentity = new System.Windows.Forms.Label();
            this.lblIncludeSecurity = new System.Windows.Forms.Label();
            this.lblImportObjects = new System.Windows.Forms.Label();
            this.lblImportSite = new System.Windows.Forms.Label();
            this.lblFinishScreenSubTitle = new System.Windows.Forms.Label();
            this.lblFinishScreenTitle = new System.Windows.Forms.Label();
            this.ctxtMenuExportItem.SuspendLayout();
            this.startStep1.SuspendLayout();
            this.intermediateStep1.SuspendLayout();
            this.gpActionSelect.SuspendLayout();
            this.gpSiteAuthDetails.SuspendLayout();
            this.intermediateStep2.SuspendLayout();
            this.gpImportSettings.SuspendLayout();
            this.gpSingleFile.SuspendLayout();
            this.pnlImportNote.SuspendLayout();
            this.intermediateStep3.SuspendLayout();
            this.gpExportContents.SuspendLayout();
            this.gpSelectedItems.SuspendLayout();
            this.intermediateStep4.SuspendLayout();
            this.gbExportSettings.SuspendLayout();
            this.finishStep1.SuspendLayout();
            this.pnlProgress.SuspendLayout();
            this.pnlSettingsSummary.SuspendLayout();
            this.pnlExportSettings.SuspendLayout();
            this.pnlImportSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // f_dlgSelectImportSingleFile
            // 
            this.f_dlgSelectImportSingleFile.FileName = "openFileDialog1";
            this.f_dlgSelectImportSingleFile.Filter = "Content Migration Package|*.cmp";
            // 
            // ctxtMenuExportItem
            // 
            this.ctxtMenuExportItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeExportItemMenuItem});
            this.ctxtMenuExportItem.Name = "ctxtMenuExportItem";
            this.ctxtMenuExportItem.Size = new System.Drawing.Size(114, 26);
            // 
            // removeExportItemMenuItem
            // 
            this.removeExportItemMenuItem.Name = "removeExportItemMenuItem";
            this.removeExportItemMenuItem.Size = new System.Drawing.Size(113, 22);
            this.removeExportItemMenuItem.Text = "Remove";
            // 
            // f_dlgSaveSettingsFile
            // 
            this.f_dlgSaveSettingsFile.Filter = "XML files|*.xml";
            this.f_dlgSaveSettingsFile.RestoreDirectory = true;
            // 
            // f_dlgLoadSettingsFile
            // 
            this.f_dlgLoadSettingsFile.Filter = "XML files|*.xml";
            // 
            // wizardControl1
            // 
            this.wizardControl1.BackButtonEnabled = false;
            this.wizardControl1.BackButtonVisible = true;
            this.wizardControl1.CancelButtonEnabled = true;
            this.wizardControl1.CancelButtonText = "Exit";
            this.wizardControl1.CancelButtonVisible = true;
            this.wizardControl1.HelpButtonEnabled = true;
            this.wizardControl1.HelpButtonVisible = false;
            this.wizardControl1.Location = new System.Drawing.Point(1, -1);
            this.wizardControl1.Name = "wizardControl1";
            this.wizardControl1.NextButtonEnabled = true;
            this.wizardControl1.NextButtonVisible = true;
            this.wizardControl1.Size = new System.Drawing.Size(789, 569);
            this.wizardControl1.WizardSteps.Add(this.startStep1);
            this.wizardControl1.WizardSteps.Add(this.intermediateStep1);
            this.wizardControl1.WizardSteps.Add(this.intermediateStep2);
            this.wizardControl1.WizardSteps.Add(this.intermediateStep3);
            this.wizardControl1.WizardSteps.Add(this.intermediateStep4);
            this.wizardControl1.WizardSteps.Add(this.finishStep1);
            // 
            // startStep1
            // 
            this.startStep1.BindingImage = ((System.Drawing.Image)(resources.GetObject("startStep1.BindingImage")));
            this.startStep1.Controls.Add(this.btnAbout);
            this.startStep1.Icon = ((System.Drawing.Image)(resources.GetObject("startStep1.Icon")));
            this.startStep1.Name = "startStep1";
            this.startStep1.Subtitle = "This tool uses the SharePoint content migration API to move content between Share" +
                "Point sites and environments.";
            this.startStep1.SubtitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.startStep1.Title = "Welcome to the SharePoint Content Deployment Wizard";
            this.startStep1.TitleFont = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(671, 472);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(93, 24);
            this.btnAbout.TabIndex = 17;
            this.btnAbout.Text = "About..";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // intermediateStep1
            // 
            this.intermediateStep1.BindingImage = ((System.Drawing.Image)(resources.GetObject("intermediateStep1.BindingImage")));
            this.intermediateStep1.Controls.Add(this.btnLoadSettings);
            this.intermediateStep1.Controls.Add(this.gpActionSelect);
            this.intermediateStep1.Controls.Add(this.gpSiteAuthDetails);
            this.intermediateStep1.Name = "intermediateStep1";
            this.intermediateStep1.Subtitle = "Supply details for the site you wish to use";
            this.intermediateStep1.SubtitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.intermediateStep1.Title = "Bind to site";
            this.intermediateStep1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            // 
            // btnLoadSettings
            // 
            this.btnLoadSettings.Location = new System.Drawing.Point(45, 490);
            this.btnLoadSettings.Name = "btnLoadSettings";
            this.btnLoadSettings.Size = new System.Drawing.Size(135, 24);
            this.btnLoadSettings.TabIndex = 17;
            this.btnLoadSettings.Text = "Load saved settings...";
            this.btnLoadSettings.UseVisualStyleBackColor = true;
            this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
            // 
            // gpActionSelect
            // 
            this.gpActionSelect.Controls.Add(this.lblWhichAction);
            this.gpActionSelect.Controls.Add(this.rdoImport);
            this.gpActionSelect.Controls.Add(this.rdoExport);
            this.gpActionSelect.Location = new System.Drawing.Point(41, 88);
            this.gpActionSelect.Name = "gpActionSelect";
            this.gpActionSelect.Size = new System.Drawing.Size(384, 127);
            this.gpActionSelect.TabIndex = 15;
            this.gpActionSelect.TabStop = false;
            this.gpActionSelect.Text = "Action";
            // 
            // lblWhichAction
            // 
            this.lblWhichAction.Location = new System.Drawing.Point(12, 22);
            this.lblWhichAction.Name = "lblWhichAction";
            this.lblWhichAction.Size = new System.Drawing.Size(268, 26);
            this.lblWhichAction.TabIndex = 15;
            this.lblWhichAction.Text = "Which action do you wish to perform?";
            // 
            // rdoImport
            // 
            this.rdoImport.AutoSize = true;
            this.rdoImport.Location = new System.Drawing.Point(18, 55);
            this.rdoImport.Name = "rdoImport";
            this.rdoImport.Size = new System.Drawing.Size(54, 17);
            this.rdoImport.TabIndex = 0;
            this.rdoImport.TabStop = true;
            this.rdoImport.Text = "Import";
            this.rdoImport.UseVisualStyleBackColor = true;
            this.rdoImport.CheckedChanged += new System.EventHandler(this.rdoImport_CheckedChanged);
            // 
            // rdoExport
            // 
            this.rdoExport.AutoSize = true;
            this.rdoExport.Location = new System.Drawing.Point(18, 81);
            this.rdoExport.Name = "rdoExport";
            this.rdoExport.Size = new System.Drawing.Size(55, 17);
            this.rdoExport.TabIndex = 1;
            this.rdoExport.TabStop = true;
            this.rdoExport.Text = "Export";
            this.rdoExport.UseVisualStyleBackColor = true;
            // 
            // gpSiteAuthDetails
            // 
            this.gpSiteAuthDetails.Controls.Add(this.lblWebUrl);
            this.gpSiteAuthDetails.Controls.Add(this.txtWebUrl);
            this.gpSiteAuthDetails.Controls.Add(this.lblSiteUrl);
            this.gpSiteAuthDetails.Controls.Add(this.txtSiteUrl);
            this.gpSiteAuthDetails.Location = new System.Drawing.Point(40, 242);
            this.gpSiteAuthDetails.Name = "gpSiteAuthDetails";
            this.gpSiteAuthDetails.Size = new System.Drawing.Size(385, 122);
            this.gpSiteAuthDetails.TabIndex = 12;
            this.gpSiteAuthDetails.TabStop = false;
            this.gpSiteAuthDetails.Text = "Sharepoint site";
            // 
            // lblWebUrl
            // 
            this.lblWebUrl.Enabled = false;
            this.lblWebUrl.Location = new System.Drawing.Point(12, 68);
            this.lblWebUrl.Name = "lblWebUrl";
            this.lblWebUrl.Size = new System.Drawing.Size(88, 24);
            this.lblWebUrl.TabIndex = 3;
            this.lblWebUrl.Text = "Import web URL:";
            // 
            // txtWebUrl
            // 
            this.txtWebUrl.Enabled = false;
            this.txtWebUrl.Location = new System.Drawing.Point(112, 66);
            this.txtWebUrl.Name = "txtWebUrl";
            this.txtWebUrl.Size = new System.Drawing.Size(257, 20);
            this.txtWebUrl.TabIndex = 3;
            // 
            // lblSiteUrl
            // 
            this.lblSiteUrl.AutoSize = true;
            this.lblSiteUrl.Location = new System.Drawing.Point(12, 33);
            this.lblSiteUrl.Name = "lblSiteUrl";
            this.lblSiteUrl.Size = new System.Drawing.Size(53, 13);
            this.lblSiteUrl.TabIndex = 1;
            this.lblSiteUrl.Text = "Site URL:";
            // 
            // txtSiteUrl
            // 
            this.txtSiteUrl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtSiteUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtSiteUrl.Location = new System.Drawing.Point(112, 30);
            this.txtSiteUrl.Name = "txtSiteUrl";
            this.txtSiteUrl.Size = new System.Drawing.Size(257, 20);
            this.txtSiteUrl.TabIndex = 2;
            // 
            // intermediateStep2
            // 
            this.intermediateStep2.BindingImage = ((System.Drawing.Image)(resources.GetObject("intermediateStep2.BindingImage")));
            this.intermediateStep2.Controls.Add(this.gpImportSettings);
            this.intermediateStep2.Name = "intermediateStep2";
            this.intermediateStep2.Subtitle = "Select options for this import.";
            this.intermediateStep2.SubtitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.intermediateStep2.Title = "Import settings";
            this.intermediateStep2.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            // 
            // gpImportSettings
            // 
            this.gpImportSettings.Controls.Add(this.lblIncludeSecurityImport);
            this.gpImportSettings.Controls.Add(this.cboIncludeSecurityImport);
            this.gpImportSettings.Controls.Add(this.lblImportLogfilePathValue);
            this.gpImportSettings.Controls.Add(this.lblImportLogfilePath);
            this.gpImportSettings.Controls.Add(this.cboVersionOptions);
            this.gpImportSettings.Controls.Add(this.lblVersionOptions);
            this.gpImportSettings.Controls.Add(this.chkRetainIDs);
            this.gpImportSettings.Controls.Add(this.lblRetainID);
            this.gpImportSettings.Controls.Add(this.gpSingleFile);
            this.gpImportSettings.Controls.Add(this.lblImportType);
            this.gpImportSettings.Controls.Add(this.cboUserInfo);
            this.gpImportSettings.Controls.Add(this.lblUpdateTimeDate);
            this.gpImportSettings.Location = new System.Drawing.Point(52, 67);
            this.gpImportSettings.Name = "gpImportSettings";
            this.gpImportSettings.Size = new System.Drawing.Size(571, 449);
            this.gpImportSettings.TabIndex = 7;
            this.gpImportSettings.TabStop = false;
            this.gpImportSettings.Text = "Import settings";
            // 
            // lblIncludeSecurityImport
            // 
            this.lblIncludeSecurityImport.AutoSize = true;
            this.lblIncludeSecurityImport.Location = new System.Drawing.Point(17, 313);
            this.lblIncludeSecurityImport.Name = "lblIncludeSecurityImport";
            this.lblIncludeSecurityImport.Size = new System.Drawing.Size(84, 13);
            this.lblIncludeSecurityImport.TabIndex = 25;
            this.lblIncludeSecurityImport.Text = "Include security:";
            // 
            // cboIncludeSecurityImport
            // 
            this.cboIncludeSecurityImport.FormattingEnabled = true;
            this.cboIncludeSecurityImport.Location = new System.Drawing.Point(141, 310);
            this.cboIncludeSecurityImport.Name = "cboIncludeSecurityImport";
            this.cboIncludeSecurityImport.Size = new System.Drawing.Size(121, 21);
            this.cboIncludeSecurityImport.TabIndex = 24;
            // 
            // lblImportLogfilePathValue
            // 
            this.lblImportLogfilePathValue.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblImportLogfilePathValue.Location = new System.Drawing.Point(138, 219);
            this.lblImportLogfilePathValue.Name = "lblImportLogfilePathValue";
            this.lblImportLogfilePathValue.Size = new System.Drawing.Size(416, 27);
            this.lblImportLogfilePathValue.TabIndex = 23;
            // 
            // lblImportLogfilePath
            // 
            this.lblImportLogfilePath.AutoSize = true;
            this.lblImportLogfilePath.Location = new System.Drawing.Point(15, 218);
            this.lblImportLogfilePath.Name = "lblImportLogfilePath";
            this.lblImportLogfilePath.Size = new System.Drawing.Size(44, 13);
            this.lblImportLogfilePath.TabIndex = 22;
            this.lblImportLogfilePath.Text = "Log file:";
            // 
            // cboVersionOptions
            // 
            this.cboVersionOptions.FormattingEnabled = true;
            this.cboVersionOptions.Location = new System.Drawing.Point(141, 359);
            this.cboVersionOptions.Name = "cboVersionOptions";
            this.cboVersionOptions.Size = new System.Drawing.Size(121, 21);
            this.cboVersionOptions.TabIndex = 21;
            // 
            // lblVersionOptions
            // 
            this.lblVersionOptions.AutoSize = true;
            this.lblVersionOptions.Location = new System.Drawing.Point(15, 362);
            this.lblVersionOptions.Name = "lblVersionOptions";
            this.lblVersionOptions.Size = new System.Drawing.Size(82, 13);
            this.lblVersionOptions.TabIndex = 20;
            this.lblVersionOptions.Text = "Version options:";
            // 
            // chkRetainIDs
            // 
            this.chkRetainIDs.AutoSize = true;
            this.chkRetainIDs.Location = new System.Drawing.Point(141, 264);
            this.chkRetainIDs.Name = "chkRetainIDs";
            this.chkRetainIDs.Size = new System.Drawing.Size(15, 14);
            this.chkRetainIDs.TabIndex = 19;
            this.chkRetainIDs.UseVisualStyleBackColor = true;
            // 
            // lblRetainID
            // 
            this.lblRetainID.Location = new System.Drawing.Point(15, 257);
            this.lblRetainID.Name = "lblRetainID";
            this.lblRetainID.Size = new System.Drawing.Size(92, 35);
            this.lblRetainID.TabIndex = 18;
            this.lblRetainID.Text = "Retain object IDs and locations:";
            // 
            // gpSingleFile
            // 
            this.gpSingleFile.Controls.Add(this.pnlImportNote);
            this.gpSingleFile.Controls.Add(this.lblImportPathValue);
            this.gpSingleFile.Controls.Add(this.btnImportPathBrowse);
            this.gpSingleFile.Controls.Add(this.lblImportPathConfig);
            this.gpSingleFile.Location = new System.Drawing.Point(137, 23);
            this.gpSingleFile.Name = "gpSingleFile";
            this.gpSingleFile.Size = new System.Drawing.Size(416, 178);
            this.gpSingleFile.TabIndex = 16;
            this.gpSingleFile.TabStop = false;
            this.gpSingleFile.Text = "Single file/multiple files";
            // 
            // pnlImportNote
            // 
            this.pnlImportNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlImportNote.Controls.Add(this.lblImportNoteDetail);
            this.pnlImportNote.Controls.Add(this.lblImportNote);
            this.pnlImportNote.Location = new System.Drawing.Point(23, 29);
            this.pnlImportNote.Name = "pnlImportNote";
            this.pnlImportNote.Size = new System.Drawing.Size(373, 47);
            this.pnlImportNote.TabIndex = 27;
            // 
            // lblImportNoteDetail
            // 
            this.lblImportNoteDetail.Location = new System.Drawing.Point(95, 9);
            this.lblImportNoteDetail.Name = "lblImportNoteDetail";
            this.lblImportNoteDetail.Size = new System.Drawing.Size(231, 34);
            this.lblImportNoteDetail.TabIndex = 14;
            this.lblImportNoteDetail.Text = "For multiple files, please browse to the \'base\' .cmp file for the set of files";
            // 
            // lblImportNote
            // 
            this.lblImportNote.AutoSize = true;
            this.lblImportNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImportNote.Location = new System.Drawing.Point(45, 10);
            this.lblImportNote.Name = "lblImportNote";
            this.lblImportNote.Size = new System.Drawing.Size(38, 13);
            this.lblImportNote.TabIndex = 0;
            this.lblImportNote.Text = "Note:";
            // 
            // lblImportPathValue
            // 
            this.lblImportPathValue.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblImportPathValue.Location = new System.Drawing.Point(89, 107);
            this.lblImportPathValue.Name = "lblImportPathValue";
            this.lblImportPathValue.Size = new System.Drawing.Size(220, 41);
            this.lblImportPathValue.TabIndex = 13;
            // 
            // btnImportPathBrowse
            // 
            this.btnImportPathBrowse.Location = new System.Drawing.Point(321, 101);
            this.btnImportPathBrowse.Name = "btnImportPathBrowse";
            this.btnImportPathBrowse.Size = new System.Drawing.Size(75, 25);
            this.btnImportPathBrowse.TabIndex = 5;
            this.btnImportPathBrowse.Text = "Browse...";
            this.btnImportPathBrowse.UseVisualStyleBackColor = true;
            this.btnImportPathBrowse.Click += new System.EventHandler(this.btnImportPathBrowse_Click);
            // 
            // lblImportPathConfig
            // 
            this.lblImportPathConfig.AutoSize = true;
            this.lblImportPathConfig.Location = new System.Drawing.Point(20, 105);
            this.lblImportPathConfig.Name = "lblImportPathConfig";
            this.lblImportPathConfig.Size = new System.Drawing.Size(63, 13);
            this.lblImportPathConfig.TabIndex = 2;
            this.lblImportPathConfig.Text = "Import path:";
            // 
            // lblImportType
            // 
            this.lblImportType.AutoSize = true;
            this.lblImportType.Location = new System.Drawing.Point(15, 92);
            this.lblImportType.Name = "lblImportType";
            this.lblImportType.Size = new System.Drawing.Size(66, 13);
            this.lblImportType.TabIndex = 15;
            this.lblImportType.Text = "Import file(s):";
            // 
            // cboUserInfo
            // 
            this.cboUserInfo.FormattingEnabled = true;
            this.cboUserInfo.Location = new System.Drawing.Point(141, 412);
            this.cboUserInfo.Name = "cboUserInfo";
            this.cboUserInfo.Size = new System.Drawing.Size(121, 21);
            this.cboUserInfo.TabIndex = 6;
            // 
            // lblUpdateTimeDate
            // 
            this.lblUpdateTimeDate.AutoSize = true;
            this.lblUpdateTimeDate.Location = new System.Drawing.Point(15, 413);
            this.lblUpdateTimeDate.Name = "lblUpdateTimeDate";
            this.lblUpdateTimeDate.Size = new System.Drawing.Size(88, 13);
            this.lblUpdateTimeDate.TabIndex = 5;
            this.lblUpdateTimeDate.Text = "User info update:";
            // 
            // intermediateStep3
            // 
            this.intermediateStep3.BindingImage = ((System.Drawing.Image)(resources.GetObject("intermediateStep3.BindingImage")));
            this.intermediateStep3.Controls.Add(this.gpExportContents);
            this.intermediateStep3.Controls.Add(this.gpSelectedItems);
            this.intermediateStep3.Name = "intermediateStep3";
            this.intermediateStep3.Subtitle = "Right-click items to add to export list.";
            this.intermediateStep3.SubtitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.intermediateStep3.Title = "Select content for export";
            this.intermediateStep3.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            // 
            // gpExportContents
            // 
            this.gpExportContents.Controls.Add(this.lblRetrievingData);
            this.gpExportContents.Controls.Add(this.trvContent);
            this.gpExportContents.Location = new System.Drawing.Point(25, 73);
            this.gpExportContents.Name = "gpExportContents";
            this.gpExportContents.Size = new System.Drawing.Size(711, 218);
            this.gpExportContents.TabIndex = 10;
            this.gpExportContents.TabStop = false;
            this.gpExportContents.Text = "Export content";
            // 
            // lblRetrievingData
            // 
            this.lblRetrievingData.AutoSize = true;
            this.lblRetrievingData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRetrievingData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblRetrievingData.Location = new System.Drawing.Point(272, 13);
            this.lblRetrievingData.Name = "lblRetrievingData";
            this.lblRetrievingData.Size = new System.Drawing.Size(174, 13);
            this.lblRetrievingData.TabIndex = 9;
            this.lblRetrievingData.Text = "Retrieving data, please wait..";
            this.lblRetrievingData.Visible = false;
            // 
            // trvContent
            // 
            this.trvContent.BackColor = System.Drawing.SystemColors.Control;
            this.trvContent.Location = new System.Drawing.Point(16, 34);
            this.trvContent.Name = "trvContent";
            this.trvContent.Size = new System.Drawing.Size(678, 168);
            this.trvContent.TabIndex = 8;
            // 
            // gpSelectedItems
            // 
            this.gpSelectedItems.Controls.Add(this.lstExportItems);
            this.gpSelectedItems.Location = new System.Drawing.Point(25, 309);
            this.gpSelectedItems.Name = "gpSelectedItems";
            this.gpSelectedItems.Size = new System.Drawing.Size(711, 205);
            this.gpSelectedItems.TabIndex = 11;
            this.gpSelectedItems.TabStop = false;
            this.gpSelectedItems.Text = "Selected items";
            // 
            // lstExportItems
            // 
            this.lstExportItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmItem,
            this.clmType,
            this.clmIncDescs});
            this.lstExportItems.FullRowSelect = true;
            this.lstExportItems.Location = new System.Drawing.Point(16, 23);
            this.lstExportItems.Name = "lstExportItems";
            this.lstExportItems.Size = new System.Drawing.Size(678, 166);
            this.lstExportItems.TabIndex = 13;
            this.lstExportItems.UseCompatibleStateImageBehavior = false;
            this.lstExportItems.View = System.Windows.Forms.View.Details;
            // 
            // clmItem
            // 
            this.clmItem.Text = "Item";
            this.clmItem.Width = 491;
            // 
            // clmType
            // 
            this.clmType.Text = "Type";
            // 
            // clmIncDescs
            // 
            this.clmIncDescs.Text = "Include descendents";
            this.clmIncDescs.Width = 295;
            // 
            // intermediateStep4
            // 
            this.intermediateStep4.BindingImage = ((System.Drawing.Image)(resources.GetObject("intermediateStep4.BindingImage")));
            this.intermediateStep4.Controls.Add(this.gbExportSettings);
            this.intermediateStep4.Name = "intermediateStep4";
            this.intermediateStep4.Subtitle = "Select options for this export.";
            this.intermediateStep4.SubtitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.intermediateStep4.Title = "Export settings";
            this.intermediateStep4.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            // 
            // gbExportSettings
            // 
            this.gbExportSettings.Controls.Add(this.lblCompressionNote);
            this.gbExportSettings.Controls.Add(this.chkDisableCompression);
            this.gbExportSettings.Controls.Add(this.lblDisableCompression);
            this.gbExportSettings.Controls.Add(this.chkExcludeDependencies);
            this.gbExportSettings.Controls.Add(this.lblExcludeDependencies);
            this.gbExportSettings.Controls.Add(this.lblExportBaseFilenameValue);
            this.gbExportSettings.Controls.Add(this.lblExportFile);
            this.gbExportSettings.Controls.Add(this.txtExportBaseFilename);
            this.gbExportSettings.Controls.Add(this.lblExportBaseFilename);
            this.gbExportSettings.Controls.Add(this.lblExportLogPathValue);
            this.gbExportSettings.Controls.Add(this.lblExportLogPath);
            this.gbExportSettings.Controls.Add(this.cboIncludeSecurity);
            this.gbExportSettings.Controls.Add(this.cboIncludeVersions);
            this.gbExportSettings.Controls.Add(this.lblIncludeSecurityConfig);
            this.gbExportSettings.Controls.Add(this.lblIncludeVersionsConfig);
            this.gbExportSettings.Controls.Add(this.cboExportMethod);
            this.gbExportSettings.Controls.Add(this.lblExportMethodConfig);
            this.gbExportSettings.Controls.Add(this.btnFilePathBrowse);
            this.gbExportSettings.Controls.Add(this.lblExportFolderValue);
            this.gbExportSettings.Controls.Add(this.lblExportFolder);
            this.gbExportSettings.Location = new System.Drawing.Point(44, 90);
            this.gbExportSettings.Name = "gbExportSettings";
            this.gbExportSettings.Size = new System.Drawing.Size(678, 405);
            this.gbExportSettings.TabIndex = 13;
            this.gbExportSettings.TabStop = false;
            this.gbExportSettings.Text = "Export settings";
            // 
            // lblCompressionNote
            // 
            this.lblCompressionNote.AutoSize = true;
            this.lblCompressionNote.Location = new System.Drawing.Point(190, 283);
            this.lblCompressionNote.Name = "lblCompressionNote";
            this.lblCompressionNote.Size = new System.Drawing.Size(288, 13);
            this.lblCompressionNote.TabIndex = 21;
            this.lblCompressionNote.Text = "(N.B. Import of uncompressed files is via command-line only)";
            // 
            // chkDisableCompression
            // 
            this.chkDisableCompression.AutoSize = true;
            this.chkDisableCompression.Location = new System.Drawing.Point(152, 283);
            this.chkDisableCompression.Name = "chkDisableCompression";
            this.chkDisableCompression.Size = new System.Drawing.Size(15, 14);
            this.chkDisableCompression.TabIndex = 20;
            this.chkDisableCompression.UseVisualStyleBackColor = true;
            // 
            // lblDisableCompression
            // 
            this.lblDisableCompression.AutoSize = true;
            this.lblDisableCompression.Location = new System.Drawing.Point(16, 280);
            this.lblDisableCompression.Name = "lblDisableCompression";
            this.lblDisableCompression.Size = new System.Drawing.Size(107, 13);
            this.lblDisableCompression.TabIndex = 19;
            this.lblDisableCompression.Text = "Disable compression:";
            // 
            // chkExcludeDependencies
            // 
            this.chkExcludeDependencies.AutoSize = true;
            this.chkExcludeDependencies.Location = new System.Drawing.Point(152, 30);
            this.chkExcludeDependencies.Name = "chkExcludeDependencies";
            this.chkExcludeDependencies.Size = new System.Drawing.Size(15, 14);
            this.chkExcludeDependencies.TabIndex = 18;
            this.chkExcludeDependencies.UseVisualStyleBackColor = true;
            // 
            // lblExcludeDependencies
            // 
            this.lblExcludeDependencies.Location = new System.Drawing.Point(16, 25);
            this.lblExcludeDependencies.Name = "lblExcludeDependencies";
            this.lblExcludeDependencies.Size = new System.Drawing.Size(121, 31);
            this.lblExcludeDependencies.TabIndex = 17;
            this.lblExcludeDependencies.Text = "Exclude dependencies of selected objects:";
            // 
            // lblExportBaseFilenameValue
            // 
            this.lblExportBaseFilenameValue.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblExportBaseFilenameValue.Location = new System.Drawing.Point(149, 314);
            this.lblExportBaseFilenameValue.Name = "lblExportBaseFilenameValue";
            this.lblExportBaseFilenameValue.Size = new System.Drawing.Size(477, 27);
            this.lblExportBaseFilenameValue.TabIndex = 16;
            // 
            // lblExportFile
            // 
            this.lblExportFile.AutoSize = true;
            this.lblExportFile.Location = new System.Drawing.Point(16, 314);
            this.lblExportFile.Name = "lblExportFile";
            this.lblExportFile.Size = new System.Drawing.Size(56, 13);
            this.lblExportFile.TabIndex = 15;
            this.lblExportFile.Text = "Export file:";
            // 
            // txtExportBaseFilename
            // 
            this.txtExportBaseFilename.Location = new System.Drawing.Point(152, 237);
            this.txtExportBaseFilename.Name = "txtExportBaseFilename";
            this.txtExportBaseFilename.Size = new System.Drawing.Size(206, 20);
            this.txtExportBaseFilename.TabIndex = 14;
            this.txtExportBaseFilename.TextChanged += new System.EventHandler(this.txtExportBaseFilename_TextChanged);
            // 
            // lblExportBaseFilename
            // 
            this.lblExportBaseFilename.AutoSize = true;
            this.lblExportBaseFilename.Location = new System.Drawing.Point(15, 240);
            this.lblExportBaseFilename.Name = "lblExportBaseFilename";
            this.lblExportBaseFilename.Size = new System.Drawing.Size(76, 13);
            this.lblExportBaseFilename.TabIndex = 13;
            this.lblExportBaseFilename.Text = "Base filename:";
            // 
            // lblExportLogPathValue
            // 
            this.lblExportLogPathValue.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblExportLogPathValue.Location = new System.Drawing.Point(149, 353);
            this.lblExportLogPathValue.Name = "lblExportLogPathValue";
            this.lblExportLogPathValue.Size = new System.Drawing.Size(453, 27);
            this.lblExportLogPathValue.TabIndex = 12;
            // 
            // lblExportLogPath
            // 
            this.lblExportLogPath.AutoSize = true;
            this.lblExportLogPath.Location = new System.Drawing.Point(16, 353);
            this.lblExportLogPath.Name = "lblExportLogPath";
            this.lblExportLogPath.Size = new System.Drawing.Size(44, 13);
            this.lblExportLogPath.TabIndex = 11;
            this.lblExportLogPath.Text = "Log file:";
            // 
            // cboIncludeSecurity
            // 
            this.cboIncludeSecurity.FormattingEnabled = true;
            this.cboIncludeSecurity.Location = new System.Drawing.Point(152, 143);
            this.cboIncludeSecurity.Name = "cboIncludeSecurity";
            this.cboIncludeSecurity.Size = new System.Drawing.Size(121, 21);
            this.cboIncludeSecurity.TabIndex = 10;
            // 
            // cboIncludeVersions
            // 
            this.cboIncludeVersions.FormattingEnabled = true;
            this.cboIncludeVersions.Location = new System.Drawing.Point(152, 101);
            this.cboIncludeVersions.Name = "cboIncludeVersions";
            this.cboIncludeVersions.Size = new System.Drawing.Size(121, 21);
            this.cboIncludeVersions.TabIndex = 9;
            // 
            // lblIncludeSecurityConfig
            // 
            this.lblIncludeSecurityConfig.AutoSize = true;
            this.lblIncludeSecurityConfig.Location = new System.Drawing.Point(15, 146);
            this.lblIncludeSecurityConfig.Name = "lblIncludeSecurityConfig";
            this.lblIncludeSecurityConfig.Size = new System.Drawing.Size(84, 13);
            this.lblIncludeSecurityConfig.TabIndex = 8;
            this.lblIncludeSecurityConfig.Text = "Include security:";
            // 
            // lblIncludeVersionsConfig
            // 
            this.lblIncludeVersionsConfig.AutoSize = true;
            this.lblIncludeVersionsConfig.Location = new System.Drawing.Point(15, 103);
            this.lblIncludeVersionsConfig.Name = "lblIncludeVersionsConfig";
            this.lblIncludeVersionsConfig.Size = new System.Drawing.Size(87, 13);
            this.lblIncludeVersionsConfig.TabIndex = 7;
            this.lblIncludeVersionsConfig.Text = "Include versions:";
            // 
            // cboExportMethod
            // 
            this.cboExportMethod.FormattingEnabled = true;
            this.cboExportMethod.Location = new System.Drawing.Point(152, 64);
            this.cboExportMethod.Name = "cboExportMethod";
            this.cboExportMethod.Size = new System.Drawing.Size(121, 21);
            this.cboExportMethod.TabIndex = 6;
            this.cboExportMethod.SelectedIndexChanged += new System.EventHandler(this.cboExportMethod_SelectedIndexChanged);
            // 
            // lblExportMethodConfig
            // 
            this.lblExportMethodConfig.AutoSize = true;
            this.lblExportMethodConfig.Location = new System.Drawing.Point(15, 67);
            this.lblExportMethodConfig.Name = "lblExportMethodConfig";
            this.lblExportMethodConfig.Size = new System.Drawing.Size(78, 13);
            this.lblExportMethodConfig.TabIndex = 5;
            this.lblExportMethodConfig.Text = "Export method:";
            // 
            // btnFilePathBrowse
            // 
            this.btnFilePathBrowse.Location = new System.Drawing.Point(152, 187);
            this.btnFilePathBrowse.Name = "btnFilePathBrowse";
            this.btnFilePathBrowse.Size = new System.Drawing.Size(75, 25);
            this.btnFilePathBrowse.TabIndex = 4;
            this.btnFilePathBrowse.Text = "Browse...";
            this.btnFilePathBrowse.UseVisualStyleBackColor = true;
            this.btnFilePathBrowse.Click += new System.EventHandler(this.btnFilePathBrowse_Click);
            // 
            // lblExportFolderValue
            // 
            this.lblExportFolderValue.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblExportFolderValue.Location = new System.Drawing.Point(245, 186);
            this.lblExportFolderValue.Name = "lblExportFolderValue";
            this.lblExportFolderValue.Size = new System.Drawing.Size(395, 35);
            this.lblExportFolderValue.TabIndex = 3;
            // 
            // lblExportFolder
            // 
            this.lblExportFolder.AutoSize = true;
            this.lblExportFolder.Location = new System.Drawing.Point(15, 191);
            this.lblExportFolder.Name = "lblExportFolder";
            this.lblExportFolder.Size = new System.Drawing.Size(69, 13);
            this.lblExportFolder.TabIndex = 2;
            this.lblExportFolder.Text = "Export folder:";
            // 
            // finishStep1
            // 
            this.finishStep1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.finishStep1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("finishStep1.BackgroundImage")));
            this.finishStep1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.finishStep1.Controls.Add(this.pnlProgress);
            this.finishStep1.Controls.Add(this.pnlSettingsSummary);
            this.finishStep1.Controls.Add(this.lblFinishScreenSubTitle);
            this.finishStep1.Controls.Add(this.lblFinishScreenTitle);
            this.finishStep1.Name = "finishStep1";
            // 
            // pnlProgress
            // 
            this.pnlProgress.BackColor = System.Drawing.Color.Transparent;
            this.pnlProgress.Controls.Add(this.btnSaveSettings);
            this.pnlProgress.Controls.Add(this.btnReturnToStart);
            this.pnlProgress.Controls.Add(this.lblItemAction);
            this.pnlProgress.Controls.Add(this.prgImport);
            this.pnlProgress.Controls.Add(this.prgExport);
            this.pnlProgress.Location = new System.Drawing.Point(34, 369);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Size = new System.Drawing.Size(713, 143);
            this.pnlProgress.TabIndex = 4;
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnSaveSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSaveSettings.Location = new System.Drawing.Point(268, 112);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(151, 24);
            this.btnSaveSettings.TabIndex = 17;
            this.btnSaveSettings.Text = "Save settings...";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnReturnToStart
            // 
            this.btnReturnToStart.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnReturnToStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnReturnToStart.Location = new System.Drawing.Point(434, 112);
            this.btnReturnToStart.Name = "btnReturnToStart";
            this.btnReturnToStart.Size = new System.Drawing.Size(151, 24);
            this.btnReturnToStart.TabIndex = 16;
            this.btnReturnToStart.Text = "Return to start";
            this.btnReturnToStart.UseVisualStyleBackColor = true;
            this.btnReturnToStart.Visible = false;
            this.btnReturnToStart.Click += new System.EventHandler(this.btnReturnToStart_Click);
            // 
            // lblItemAction
            // 
            this.lblItemAction.AutoSize = true;
            this.lblItemAction.BackColor = System.Drawing.Color.Transparent;
            this.lblItemAction.Location = new System.Drawing.Point(124, 30);
            this.lblItemAction.Name = "lblItemAction";
            this.lblItemAction.Size = new System.Drawing.Size(0, 13);
            this.lblItemAction.TabIndex = 15;
            // 
            // prgImport
            // 
            this.prgImport.Location = new System.Drawing.Point(127, 61);
            this.prgImport.Name = "prgImport";
            this.prgImport.Size = new System.Drawing.Size(459, 24);
            this.prgImport.TabIndex = 14;
            this.prgImport.Visible = false;
            // 
            // prgExport
            // 
            this.prgExport.Location = new System.Drawing.Point(127, 62);
            this.prgExport.Name = "prgExport";
            this.prgExport.Size = new System.Drawing.Size(459, 23);
            this.prgExport.TabIndex = 13;
            // 
            // pnlSettingsSummary
            // 
            this.pnlSettingsSummary.AutoSize = true;
            this.pnlSettingsSummary.BackColor = System.Drawing.Color.Transparent;
            this.pnlSettingsSummary.Controls.Add(this.pnlExportSettings);
            this.pnlSettingsSummary.Controls.Add(this.pnlImportSettings);
            this.pnlSettingsSummary.Location = new System.Drawing.Point(34, 124);
            this.pnlSettingsSummary.Name = "pnlSettingsSummary";
            this.pnlSettingsSummary.Size = new System.Drawing.Size(713, 421);
            this.pnlSettingsSummary.TabIndex = 3;
            // 
            // pnlExportSettings
            // 
            this.pnlExportSettings.BackColor = System.Drawing.Color.Transparent;
            this.pnlExportSettings.Controls.Add(this.lblExportLogFilePathValue);
            this.pnlExportSettings.Controls.Add(this.lblExportPathValue);
            this.pnlExportSettings.Controls.Add(this.lblExportMethodValue);
            this.pnlExportSettings.Controls.Add(this.lblIncludeVersionsExportValue);
            this.pnlExportSettings.Controls.Add(this.lblIncludeSecurityExportValue);
            this.pnlExportSettings.Controls.Add(this.lblExportObjectsValue);
            this.pnlExportSettings.Controls.Add(this.lblExportSiteValue);
            this.pnlExportSettings.Controls.Add(this.lblExportLogFilePath);
            this.pnlExportSettings.Controls.Add(this.lblExportPath);
            this.pnlExportSettings.Controls.Add(this.lblExportMethod);
            this.pnlExportSettings.Controls.Add(this.lblIncludeVersionsExport);
            this.pnlExportSettings.Controls.Add(this.lblIncludeSecurityExport);
            this.pnlExportSettings.Controls.Add(this.lblExportObjects);
            this.pnlExportSettings.Controls.Add(this.lblExportSite);
            this.pnlExportSettings.Location = new System.Drawing.Point(55, 33);
            this.pnlExportSettings.Name = "pnlExportSettings";
            this.pnlExportSettings.Size = new System.Drawing.Size(636, 220);
            this.pnlExportSettings.TabIndex = 0;
            // 
            // lblExportLogFilePathValue
            // 
            this.lblExportLogFilePathValue.AutoSize = true;
            this.lblExportLogFilePathValue.BackColor = System.Drawing.Color.Transparent;
            this.lblExportLogFilePathValue.Enabled = false;
            this.lblExportLogFilePathValue.Location = new System.Drawing.Point(234, 139);
            this.lblExportLogFilePathValue.Name = "lblExportLogFilePathValue";
            this.lblExportLogFilePathValue.Size = new System.Drawing.Size(92, 13);
            this.lblExportLogFilePathValue.TabIndex = 14;
            this.lblExportLogFilePathValue.TabStop = true;
            this.lblExportLogFilePathValue.Text = "xxxxxxxxxxxxxxxxx";
            this.lblExportLogFilePathValue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblExportLogFilePathValue_LinkClicked);
            // 
            // lblExportPathValue
            // 
            this.lblExportPathValue.AutoSize = true;
            this.lblExportPathValue.BackColor = System.Drawing.Color.Transparent;
            this.lblExportPathValue.Enabled = false;
            this.lblExportPathValue.Location = new System.Drawing.Point(234, 117);
            this.lblExportPathValue.Name = "lblExportPathValue";
            this.lblExportPathValue.Size = new System.Drawing.Size(92, 13);
            this.lblExportPathValue.TabIndex = 13;
            this.lblExportPathValue.TabStop = true;
            this.lblExportPathValue.Text = "xxxxxxxxxxxxxxxxx";
            this.lblExportPathValue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblExportPathValue_LinkClicked);
            // 
            // lblExportMethodValue
            // 
            this.lblExportMethodValue.AutoSize = true;
            this.lblExportMethodValue.BackColor = System.Drawing.Color.Transparent;
            this.lblExportMethodValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportMethodValue.Location = new System.Drawing.Point(234, 92);
            this.lblExportMethodValue.Name = "lblExportMethodValue";
            this.lblExportMethodValue.Size = new System.Drawing.Size(122, 13);
            this.lblExportMethodValue.TabIndex = 11;
            this.lblExportMethodValue.Text = "xxxxxxxxxxxxxxxxxxxxxxx";
            // 
            // lblIncludeVersionsExportValue
            // 
            this.lblIncludeVersionsExportValue.AutoSize = true;
            this.lblIncludeVersionsExportValue.BackColor = System.Drawing.Color.Transparent;
            this.lblIncludeVersionsExportValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncludeVersionsExportValue.Location = new System.Drawing.Point(234, 70);
            this.lblIncludeVersionsExportValue.Name = "lblIncludeVersionsExportValue";
            this.lblIncludeVersionsExportValue.Size = new System.Drawing.Size(122, 13);
            this.lblIncludeVersionsExportValue.TabIndex = 10;
            this.lblIncludeVersionsExportValue.Text = "xxxxxxxxxxxxxxxxxxxxxxx";
            // 
            // lblIncludeSecurityExportValue
            // 
            this.lblIncludeSecurityExportValue.AutoSize = true;
            this.lblIncludeSecurityExportValue.BackColor = System.Drawing.Color.Transparent;
            this.lblIncludeSecurityExportValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncludeSecurityExportValue.Location = new System.Drawing.Point(234, 48);
            this.lblIncludeSecurityExportValue.Name = "lblIncludeSecurityExportValue";
            this.lblIncludeSecurityExportValue.Size = new System.Drawing.Size(122, 13);
            this.lblIncludeSecurityExportValue.TabIndex = 9;
            this.lblIncludeSecurityExportValue.Text = "xxxxxxxxxxxxxxxxxxxxxxx";
            // 
            // lblExportObjectsValue
            // 
            this.lblExportObjectsValue.AutoSize = true;
            this.lblExportObjectsValue.BackColor = System.Drawing.Color.Transparent;
            this.lblExportObjectsValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportObjectsValue.Location = new System.Drawing.Point(234, 27);
            this.lblExportObjectsValue.Name = "lblExportObjectsValue";
            this.lblExportObjectsValue.Size = new System.Drawing.Size(122, 13);
            this.lblExportObjectsValue.TabIndex = 8;
            this.lblExportObjectsValue.Text = "xxxxxxxxxxxxxxxxxxxxxxx";
            // 
            // lblExportSiteValue
            // 
            this.lblExportSiteValue.AutoSize = true;
            this.lblExportSiteValue.BackColor = System.Drawing.Color.Transparent;
            this.lblExportSiteValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportSiteValue.Location = new System.Drawing.Point(234, 5);
            this.lblExportSiteValue.Name = "lblExportSiteValue";
            this.lblExportSiteValue.Size = new System.Drawing.Size(122, 13);
            this.lblExportSiteValue.TabIndex = 7;
            this.lblExportSiteValue.Text = "xxxxxxxxxxxxxxxxxxxxxxx";
            // 
            // lblExportLogFilePath
            // 
            this.lblExportLogFilePath.AutoSize = true;
            this.lblExportLogFilePath.BackColor = System.Drawing.Color.Transparent;
            this.lblExportLogFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportLogFilePath.Location = new System.Drawing.Point(29, 141);
            this.lblExportLogFilePath.Name = "lblExportLogFilePath";
            this.lblExportLogFilePath.Size = new System.Drawing.Size(53, 13);
            this.lblExportLogFilePath.TabIndex = 6;
            this.lblExportLogFilePath.Text = "Log file:";
            // 
            // lblExportPath
            // 
            this.lblExportPath.AutoSize = true;
            this.lblExportPath.BackColor = System.Drawing.Color.Transparent;
            this.lblExportPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportPath.Location = new System.Drawing.Point(29, 117);
            this.lblExportPath.Name = "lblExportPath";
            this.lblExportPath.Size = new System.Drawing.Size(76, 13);
            this.lblExportPath.TabIndex = 5;
            this.lblExportPath.Text = "Export path:";
            // 
            // lblExportMethod
            // 
            this.lblExportMethod.AutoSize = true;
            this.lblExportMethod.BackColor = System.Drawing.Color.Transparent;
            this.lblExportMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportMethod.Location = new System.Drawing.Point(29, 92);
            this.lblExportMethod.Name = "lblExportMethod";
            this.lblExportMethod.Size = new System.Drawing.Size(92, 13);
            this.lblExportMethod.TabIndex = 4;
            this.lblExportMethod.Text = "Export method:";
            // 
            // lblIncludeVersionsExport
            // 
            this.lblIncludeVersionsExport.AutoSize = true;
            this.lblIncludeVersionsExport.BackColor = System.Drawing.Color.Transparent;
            this.lblIncludeVersionsExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncludeVersionsExport.Location = new System.Drawing.Point(29, 70);
            this.lblIncludeVersionsExport.Name = "lblIncludeVersionsExport";
            this.lblIncludeVersionsExport.Size = new System.Drawing.Size(104, 13);
            this.lblIncludeVersionsExport.TabIndex = 3;
            this.lblIncludeVersionsExport.Text = "Include versions:";
            // 
            // lblIncludeSecurityExport
            // 
            this.lblIncludeSecurityExport.AutoSize = true;
            this.lblIncludeSecurityExport.BackColor = System.Drawing.Color.Transparent;
            this.lblIncludeSecurityExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncludeSecurityExport.Location = new System.Drawing.Point(29, 48);
            this.lblIncludeSecurityExport.Name = "lblIncludeSecurityExport";
            this.lblIncludeSecurityExport.Size = new System.Drawing.Size(101, 13);
            this.lblIncludeSecurityExport.TabIndex = 2;
            this.lblIncludeSecurityExport.Text = "Include security:";
            // 
            // lblExportObjects
            // 
            this.lblExportObjects.AutoSize = true;
            this.lblExportObjects.BackColor = System.Drawing.Color.Transparent;
            this.lblExportObjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportObjects.Location = new System.Drawing.Point(29, 27);
            this.lblExportObjects.Name = "lblExportObjects";
            this.lblExportObjects.Size = new System.Drawing.Size(92, 13);
            this.lblExportObjects.TabIndex = 1;
            this.lblExportObjects.Text = "Export objects:";
            // 
            // lblExportSite
            // 
            this.lblExportSite.AutoSize = true;
            this.lblExportSite.BackColor = System.Drawing.Color.Transparent;
            this.lblExportSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportSite.Location = new System.Drawing.Point(29, 5);
            this.lblExportSite.Name = "lblExportSite";
            this.lblExportSite.Size = new System.Drawing.Size(33, 13);
            this.lblExportSite.TabIndex = 0;
            this.lblExportSite.Text = "Site:";
            // 
            // pnlImportSettings
            // 
            this.pnlImportSettings.BackColor = System.Drawing.Color.Transparent;
            this.pnlImportSettings.Controls.Add(this.lblImportLogFilePathValueFinish);
            this.pnlImportSettings.Controls.Add(this.lblImportPathValueFinish);
            this.pnlImportSettings.Controls.Add(this.lblUpdateVersionsValue);
            this.pnlImportSettings.Controls.Add(this.lblRetainObjectIdentityValue);
            this.pnlImportSettings.Controls.Add(this.lblIncludeSecurityImportValue);
            this.pnlImportSettings.Controls.Add(this.lblImportObjectsValue);
            this.pnlImportSettings.Controls.Add(this.lblImportSiteValue);
            this.pnlImportSettings.Controls.Add(this.lblImportLogFilePathFinish);
            this.pnlImportSettings.Controls.Add(this.lblImportPathFinish);
            this.pnlImportSettings.Controls.Add(this.lblUpdateVersions);
            this.pnlImportSettings.Controls.Add(this.lblRetainObjectIdentity);
            this.pnlImportSettings.Controls.Add(this.lblIncludeSecurity);
            this.pnlImportSettings.Controls.Add(this.lblImportObjects);
            this.pnlImportSettings.Controls.Add(this.lblImportSite);
            this.pnlImportSettings.Location = new System.Drawing.Point(62, 31);
            this.pnlImportSettings.Name = "pnlImportSettings";
            this.pnlImportSettings.Size = new System.Drawing.Size(636, 212);
            this.pnlImportSettings.TabIndex = 1;
            // 
            // lblImportLogFilePathValueFinish
            // 
            this.lblImportLogFilePathValueFinish.AutoSize = true;
            this.lblImportLogFilePathValueFinish.BackColor = System.Drawing.Color.Transparent;
            this.lblImportLogFilePathValueFinish.Enabled = false;
            this.lblImportLogFilePathValueFinish.Location = new System.Drawing.Point(234, 139);
            this.lblImportLogFilePathValueFinish.Name = "lblImportLogFilePathValueFinish";
            this.lblImportLogFilePathValueFinish.Size = new System.Drawing.Size(92, 13);
            this.lblImportLogFilePathValueFinish.TabIndex = 14;
            this.lblImportLogFilePathValueFinish.TabStop = true;
            this.lblImportLogFilePathValueFinish.Text = "xxxxxxxxxxxxxxxxx";
            this.lblImportLogFilePathValueFinish.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblImportLogFilePathValueFinish_LinkClicked);
            // 
            // lblImportPathValueFinish
            // 
            this.lblImportPathValueFinish.AutoSize = true;
            this.lblImportPathValueFinish.BackColor = System.Drawing.Color.Transparent;
            this.lblImportPathValueFinish.Enabled = false;
            this.lblImportPathValueFinish.Location = new System.Drawing.Point(234, 117);
            this.lblImportPathValueFinish.Name = "lblImportPathValueFinish";
            this.lblImportPathValueFinish.Size = new System.Drawing.Size(92, 13);
            this.lblImportPathValueFinish.TabIndex = 13;
            this.lblImportPathValueFinish.TabStop = true;
            this.lblImportPathValueFinish.Text = "xxxxxxxxxxxxxxxxx";
            this.lblImportPathValueFinish.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblImportPathValueFinish_LinkClicked);
            // 
            // lblUpdateVersionsValue
            // 
            this.lblUpdateVersionsValue.AutoSize = true;
            this.lblUpdateVersionsValue.BackColor = System.Drawing.Color.Transparent;
            this.lblUpdateVersionsValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdateVersionsValue.Location = new System.Drawing.Point(234, 92);
            this.lblUpdateVersionsValue.Name = "lblUpdateVersionsValue";
            this.lblUpdateVersionsValue.Size = new System.Drawing.Size(122, 13);
            this.lblUpdateVersionsValue.TabIndex = 11;
            this.lblUpdateVersionsValue.Text = "xxxxxxxxxxxxxxxxxxxxxxx";
            // 
            // lblRetainObjectIdentityValue
            // 
            this.lblRetainObjectIdentityValue.AutoSize = true;
            this.lblRetainObjectIdentityValue.BackColor = System.Drawing.Color.Transparent;
            this.lblRetainObjectIdentityValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRetainObjectIdentityValue.Location = new System.Drawing.Point(234, 70);
            this.lblRetainObjectIdentityValue.Name = "lblRetainObjectIdentityValue";
            this.lblRetainObjectIdentityValue.Size = new System.Drawing.Size(122, 13);
            this.lblRetainObjectIdentityValue.TabIndex = 10;
            this.lblRetainObjectIdentityValue.Text = "xxxxxxxxxxxxxxxxxxxxxxx";
            // 
            // lblIncludeSecurityImportValue
            // 
            this.lblIncludeSecurityImportValue.AutoSize = true;
            this.lblIncludeSecurityImportValue.BackColor = System.Drawing.Color.Transparent;
            this.lblIncludeSecurityImportValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncludeSecurityImportValue.Location = new System.Drawing.Point(234, 48);
            this.lblIncludeSecurityImportValue.Name = "lblIncludeSecurityImportValue";
            this.lblIncludeSecurityImportValue.Size = new System.Drawing.Size(122, 13);
            this.lblIncludeSecurityImportValue.TabIndex = 9;
            this.lblIncludeSecurityImportValue.Text = "xxxxxxxxxxxxxxxxxxxxxxx";
            // 
            // lblImportObjectsValue
            // 
            this.lblImportObjectsValue.AutoSize = true;
            this.lblImportObjectsValue.BackColor = System.Drawing.Color.Transparent;
            this.lblImportObjectsValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImportObjectsValue.Location = new System.Drawing.Point(234, 27);
            this.lblImportObjectsValue.Name = "lblImportObjectsValue";
            this.lblImportObjectsValue.Size = new System.Drawing.Size(122, 13);
            this.lblImportObjectsValue.TabIndex = 8;
            this.lblImportObjectsValue.Text = "xxxxxxxxxxxxxxxxxxxxxxx";
            // 
            // lblImportSiteValue
            // 
            this.lblImportSiteValue.AutoSize = true;
            this.lblImportSiteValue.BackColor = System.Drawing.Color.Transparent;
            this.lblImportSiteValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImportSiteValue.Location = new System.Drawing.Point(234, 5);
            this.lblImportSiteValue.Name = "lblImportSiteValue";
            this.lblImportSiteValue.Size = new System.Drawing.Size(122, 13);
            this.lblImportSiteValue.TabIndex = 7;
            this.lblImportSiteValue.Text = "xxxxxxxxxxxxxxxxxxxxxxx";
            // 
            // lblImportLogFilePathFinish
            // 
            this.lblImportLogFilePathFinish.AutoSize = true;
            this.lblImportLogFilePathFinish.BackColor = System.Drawing.Color.Transparent;
            this.lblImportLogFilePathFinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImportLogFilePathFinish.Location = new System.Drawing.Point(29, 141);
            this.lblImportLogFilePathFinish.Name = "lblImportLogFilePathFinish";
            this.lblImportLogFilePathFinish.Size = new System.Drawing.Size(53, 13);
            this.lblImportLogFilePathFinish.TabIndex = 6;
            this.lblImportLogFilePathFinish.Text = "Log file:";
            // 
            // lblImportPathFinish
            // 
            this.lblImportPathFinish.AutoSize = true;
            this.lblImportPathFinish.BackColor = System.Drawing.Color.Transparent;
            this.lblImportPathFinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImportPathFinish.Location = new System.Drawing.Point(29, 117);
            this.lblImportPathFinish.Name = "lblImportPathFinish";
            this.lblImportPathFinish.Size = new System.Drawing.Size(75, 13);
            this.lblImportPathFinish.TabIndex = 5;
            this.lblImportPathFinish.Text = "Import path:";
            // 
            // lblUpdateVersions
            // 
            this.lblUpdateVersions.AutoSize = true;
            this.lblUpdateVersions.BackColor = System.Drawing.Color.Transparent;
            this.lblUpdateVersions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdateVersions.Location = new System.Drawing.Point(29, 92);
            this.lblUpdateVersions.Name = "lblUpdateVersions";
            this.lblUpdateVersions.Size = new System.Drawing.Size(103, 13);
            this.lblUpdateVersions.TabIndex = 4;
            this.lblUpdateVersions.Text = "Update versions:";
            // 
            // lblRetainObjectIdentity
            // 
            this.lblRetainObjectIdentity.AutoSize = true;
            this.lblRetainObjectIdentity.BackColor = System.Drawing.Color.Transparent;
            this.lblRetainObjectIdentity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRetainObjectIdentity.Location = new System.Drawing.Point(29, 70);
            this.lblRetainObjectIdentity.Name = "lblRetainObjectIdentity";
            this.lblRetainObjectIdentity.Size = new System.Drawing.Size(132, 13);
            this.lblRetainObjectIdentity.TabIndex = 3;
            this.lblRetainObjectIdentity.Text = "Retain object identity:";
            // 
            // lblIncludeSecurity
            // 
            this.lblIncludeSecurity.AutoSize = true;
            this.lblIncludeSecurity.BackColor = System.Drawing.Color.Transparent;
            this.lblIncludeSecurity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncludeSecurity.Location = new System.Drawing.Point(29, 48);
            this.lblIncludeSecurity.Name = "lblIncludeSecurity";
            this.lblIncludeSecurity.Size = new System.Drawing.Size(101, 13);
            this.lblIncludeSecurity.TabIndex = 2;
            this.lblIncludeSecurity.Text = "Include security:";
            // 
            // lblImportObjects
            // 
            this.lblImportObjects.AutoSize = true;
            this.lblImportObjects.BackColor = System.Drawing.Color.Transparent;
            this.lblImportObjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImportObjects.Location = new System.Drawing.Point(29, 27);
            this.lblImportObjects.Name = "lblImportObjects";
            this.lblImportObjects.Size = new System.Drawing.Size(91, 13);
            this.lblImportObjects.TabIndex = 1;
            this.lblImportObjects.Text = "Import objects:";
            // 
            // lblImportSite
            // 
            this.lblImportSite.AutoSize = true;
            this.lblImportSite.BackColor = System.Drawing.Color.Transparent;
            this.lblImportSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImportSite.Location = new System.Drawing.Point(29, 5);
            this.lblImportSite.Name = "lblImportSite";
            this.lblImportSite.Size = new System.Drawing.Size(33, 13);
            this.lblImportSite.TabIndex = 0;
            this.lblImportSite.Text = "Site:";
            // 
            // lblFinishScreenSubTitle
            // 
            this.lblFinishScreenSubTitle.AutoSize = true;
            this.lblFinishScreenSubTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblFinishScreenSubTitle.Location = new System.Drawing.Point(40, 35);
            this.lblFinishScreenSubTitle.Name = "lblFinishScreenSubTitle";
            this.lblFinishScreenSubTitle.Size = new System.Drawing.Size(242, 13);
            this.lblFinishScreenSubTitle.TabIndex = 2;
            this.lblFinishScreenSubTitle.Text = "Confirm details then click finish to begin operation.";
            // 
            // lblFinishScreenTitle
            // 
            this.lblFinishScreenTitle.AutoSize = true;
            this.lblFinishScreenTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblFinishScreenTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinishScreenTitle.Location = new System.Drawing.Point(26, 15);
            this.lblFinishScreenTitle.Name = "lblFinishScreenTitle";
            this.lblFinishScreenTitle.Size = new System.Drawing.Size(90, 13);
            this.lblFinishScreenTitle.TabIndex = 1;
            this.lblFinishScreenTitle.Text = "Confirm details";
            // 
            // frmContentDeployer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 573);
            this.Controls.Add(this.wizardControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmContentDeployer";
            this.Text = "SharePoint Content Deployment Wizard (2.8 beta)";
            this.ctxtMenuExportItem.ResumeLayout(false);
            this.startStep1.ResumeLayout(false);
            this.intermediateStep1.ResumeLayout(false);
            this.gpActionSelect.ResumeLayout(false);
            this.gpActionSelect.PerformLayout();
            this.gpSiteAuthDetails.ResumeLayout(false);
            this.gpSiteAuthDetails.PerformLayout();
            this.intermediateStep2.ResumeLayout(false);
            this.gpImportSettings.ResumeLayout(false);
            this.gpImportSettings.PerformLayout();
            this.gpSingleFile.ResumeLayout(false);
            this.gpSingleFile.PerformLayout();
            this.pnlImportNote.ResumeLayout(false);
            this.pnlImportNote.PerformLayout();
            this.intermediateStep3.ResumeLayout(false);
            this.gpExportContents.ResumeLayout(false);
            this.gpExportContents.PerformLayout();
            this.gpSelectedItems.ResumeLayout(false);
            this.intermediateStep4.ResumeLayout(false);
            this.gbExportSettings.ResumeLayout(false);
            this.gbExportSettings.PerformLayout();
            this.finishStep1.ResumeLayout(false);
            this.finishStep1.PerformLayout();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.pnlSettingsSummary.ResumeLayout(false);
            this.pnlExportSettings.ResumeLayout(false);
            this.pnlExportSettings.PerformLayout();
            this.pnlImportSettings.ResumeLayout(false);
            this.pnlImportSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog f_dlgSelectExportFolder;
        private System.Windows.Forms.OpenFileDialog f_dlgSelectImportSingleFile;
        private System.Windows.Forms.FolderBrowserDialog f_dlgSelectImportNoCompression;
        private System.Windows.Forms.GroupBox gpExportContents;
        private System.Windows.Forms.ListView lstExportItems;
        private System.Windows.Forms.TreeView trvContent;
        private System.Windows.Forms.ColumnHeader clmItem;
        private System.Windows.Forms.ColumnHeader clmIncDescs;
        private System.Windows.Forms.ColumnHeader clmType;
        private System.Windows.Forms.GroupBox gpSelectedItems;
        private System.Windows.Forms.GroupBox gbExportSettings;
        private System.Windows.Forms.Label lblExportBaseFilenameValue;
        private System.Windows.Forms.Label lblExportFile;
        private System.Windows.Forms.TextBox txtExportBaseFilename;
        private System.Windows.Forms.Label lblExportBaseFilename;
        private System.Windows.Forms.Label lblExportLogPathValue;
        private System.Windows.Forms.Label lblExportLogPath;
        private System.Windows.Forms.ComboBox cboIncludeSecurity;
        private System.Windows.Forms.ComboBox cboIncludeVersions;
        private System.Windows.Forms.Label lblIncludeSecurityConfig;
        private System.Windows.Forms.Label lblIncludeVersionsConfig;
        private System.Windows.Forms.ComboBox cboExportMethod;
        private System.Windows.Forms.Label lblExportMethodConfig;
        private System.Windows.Forms.Button btnFilePathBrowse;
        private System.Windows.Forms.Label lblExportFolderValue;
        private System.Windows.Forms.Label lblExportFolder;
        private WizardBase.WizardControl wizardControl1;
        private WizardBase.StartStep startStep1;
        private WizardBase.IntermediateStep intermediateStep1;
        private WizardBase.FinishStep finishStep1;
        private System.Windows.Forms.RadioButton rdoExport;
        private System.Windows.Forms.RadioButton rdoImport;
        private WizardBase.IntermediateStep intermediateStep2;
        private WizardBase.IntermediateStep intermediateStep3;
        private System.Windows.Forms.GroupBox gpActionSelect;
        private System.Windows.Forms.GroupBox gpImportSettings;
        private System.Windows.Forms.Label lblImportLogfilePathValue;
        private System.Windows.Forms.Label lblImportLogfilePath;
        private System.Windows.Forms.ComboBox cboVersionOptions;
        private System.Windows.Forms.Label lblVersionOptions;
        private System.Windows.Forms.CheckBox chkRetainIDs;
        private System.Windows.Forms.Label lblRetainID;
        private System.Windows.Forms.GroupBox gpSingleFile;
        private System.Windows.Forms.Label lblImportPathValue;
        private System.Windows.Forms.Button btnImportPathBrowse;
        private System.Windows.Forms.Label lblImportPathConfig;
        private System.Windows.Forms.Label lblImportType;
        private System.Windows.Forms.ComboBox cboUserInfo;
        private System.Windows.Forms.Label lblUpdateTimeDate;
        private WizardBase.IntermediateStep intermediateStep4;
        private System.Windows.Forms.Label lblWhichAction;
        private System.Windows.Forms.Label lblFinishScreenSubTitle;
        private System.Windows.Forms.Label lblFinishScreenTitle;
        private System.Windows.Forms.Panel pnlSettingsSummary;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.ProgressBar prgExport;
        private System.Windows.Forms.ProgressBar prgImport;
        private System.Windows.Forms.Label lblIncludeSecurityImport;
        private System.Windows.Forms.ComboBox cboIncludeSecurityImport;
        private System.Windows.Forms.GroupBox gpSiteAuthDetails;
        private System.Windows.Forms.Label lblSiteUrl;
        private System.Windows.Forms.TextBox txtSiteUrl;
        private System.Windows.Forms.CheckBox chkExcludeDependencies;
        private System.Windows.Forms.Label lblExcludeDependencies;
        private System.Windows.Forms.Label lblItemAction;
        private System.Windows.Forms.ContextMenuStrip ctxtMenuExportItem;
        private System.Windows.Forms.ToolStripMenuItem removeExportItemMenuItem;
        private System.Windows.Forms.Button btnReturnToStart;
        private System.Windows.Forms.Label lblWebUrl;
        private System.Windows.Forms.TextBox txtWebUrl;
        private System.Windows.Forms.Label lblRetrievingData;
        private System.Windows.Forms.Panel pnlExportSettings;
        private System.Windows.Forms.Label lblExportSite;
        private System.Windows.Forms.Label lblExportLogFilePath;
        private System.Windows.Forms.Label lblExportPath;
        private System.Windows.Forms.Label lblExportMethod;
        private System.Windows.Forms.Label lblIncludeVersionsExport;
        private System.Windows.Forms.Label lblIncludeSecurityExport;
        private System.Windows.Forms.Label lblExportObjects;
        private System.Windows.Forms.Label lblIncludeSecurityExportValue;
        private System.Windows.Forms.Label lblExportObjectsValue;
        private System.Windows.Forms.Label lblExportSiteValue;
        private System.Windows.Forms.Label lblIncludeVersionsExportValue;
        private System.Windows.Forms.Label lblExportMethodValue;
        private System.Windows.Forms.LinkLabel lblExportPathValue;
        private System.Windows.Forms.LinkLabel lblExportLogFilePathValue;
        private System.Windows.Forms.Panel pnlImportSettings;
        private System.Windows.Forms.LinkLabel lblImportLogFilePathValueFinish;
        private System.Windows.Forms.LinkLabel lblImportPathValueFinish;
        private System.Windows.Forms.Label lblUpdateVersionsValue;
        private System.Windows.Forms.Label lblRetainObjectIdentityValue;
        private System.Windows.Forms.Label lblIncludeSecurityImportValue;
        private System.Windows.Forms.Label lblImportObjectsValue;
        private System.Windows.Forms.Label lblImportSiteValue;
        private System.Windows.Forms.Label lblImportLogFilePathFinish;
        private System.Windows.Forms.Label lblImportPathFinish;
        private System.Windows.Forms.Label lblUpdateVersions;
        private System.Windows.Forms.Label lblRetainObjectIdentity;
        private System.Windows.Forms.Label lblIncludeSecurity;
        private System.Windows.Forms.Label lblImportObjects;
        private System.Windows.Forms.Label lblImportSite;
        private System.Windows.Forms.Button btnLoadSettings;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.SaveFileDialog f_dlgSaveSettingsFile;
        private System.Windows.Forms.OpenFileDialog f_dlgLoadSettingsFile;
        private System.Windows.Forms.Panel pnlImportNote;
        private System.Windows.Forms.Label lblImportNote;
        private System.Windows.Forms.Label lblImportNoteDetail;
        private System.Windows.Forms.Label lblDisableCompression;
        private System.Windows.Forms.CheckBox chkDisableCompression;
        private System.Windows.Forms.Label lblCompressionNote;
    }
}

