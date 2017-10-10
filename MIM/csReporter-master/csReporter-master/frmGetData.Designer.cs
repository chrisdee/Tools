namespace csReporter
{
    partial class frmGetData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGetData));
            this.gbSource = new System.Windows.Forms.GroupBox();
            this.btnShowExamples = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.gbDataSelection = new System.Windows.Forms.GroupBox();
            this.cbSystem = new System.Windows.Forms.CheckBox();
            this.rbImportError = new System.Windows.Forms.RadioButton();
            this.rbExportError = new System.Windows.Forms.RadioButton();
            this.rbExport = new System.Windows.Forms.RadioButton();
            this.rbImport = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.cbbMAs = new System.Windows.Forms.ComboBox();
            this.lblMAname = new System.Windows.Forms.Label();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.rbFile = new System.Windows.Forms.RadioButton();
            this.rbGenerate = new System.Windows.Forms.RadioButton();
            this.ofdCSfile = new System.Windows.Forms.OpenFileDialog();
            this.sfdReport = new System.Windows.Forms.SaveFileDialog();
            this.gbSource.SuspendLayout();
            this.gbDataSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSource
            // 
            this.gbSource.Controls.Add(this.btnShowExamples);
            this.gbSource.Controls.Add(this.btnGenerate);
            this.gbSource.Controls.Add(this.gbDataSelection);
            this.gbSource.Controls.Add(this.cbbMAs);
            this.gbSource.Controls.Add(this.lblMAname);
            this.gbSource.Controls.Add(this.tbFile);
            this.gbSource.Controls.Add(this.btnOpenFile);
            this.gbSource.Controls.Add(this.rbFile);
            this.gbSource.Controls.Add(this.rbGenerate);
            this.gbSource.Location = new System.Drawing.Point(12, 12);
            this.gbSource.Name = "gbSource";
            this.gbSource.Size = new System.Drawing.Size(553, 276);
            this.gbSource.TabIndex = 0;
            this.gbSource.TabStop = false;
            this.gbSource.Text = "Report Source";
            // 
            // btnShowExamples
            // 
            this.btnShowExamples.Location = new System.Drawing.Point(393, 223);
            this.btnShowExamples.Name = "btnShowExamples";
            this.btnShowExamples.Size = new System.Drawing.Size(75, 41);
            this.btnShowExamples.TabIndex = 9;
            this.btnShowExamples.Text = "csexport examples";
            this.btnShowExamples.UseVisualStyleBackColor = true;
            this.btnShowExamples.Click += new System.EventHandler(this.btnShowExamples_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Location = new System.Drawing.Point(449, 60);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(91, 23);
            this.btnGenerate.TabIndex = 8;
            this.btnGenerate.Text = "Generate File";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // gbDataSelection
            // 
            this.gbDataSelection.Controls.Add(this.cbSystem);
            this.gbDataSelection.Controls.Add(this.rbImportError);
            this.gbDataSelection.Controls.Add(this.rbExportError);
            this.gbDataSelection.Controls.Add(this.rbExport);
            this.gbDataSelection.Controls.Add(this.rbImport);
            this.gbDataSelection.Controls.Add(this.rbAll);
            this.gbDataSelection.Enabled = false;
            this.gbDataSelection.Location = new System.Drawing.Point(304, 29);
            this.gbDataSelection.Name = "gbDataSelection";
            this.gbDataSelection.Size = new System.Drawing.Size(130, 154);
            this.gbDataSelection.TabIndex = 7;
            this.gbDataSelection.TabStop = false;
            this.gbDataSelection.Text = "Data Selection";
            // 
            // cbSystem
            // 
            this.cbSystem.AutoSize = true;
            this.cbSystem.Location = new System.Drawing.Point(31, 129);
            this.cbSystem.Name = "cbSystem";
            this.cbSystem.Size = new System.Drawing.Size(86, 17);
            this.cbSystem.TabIndex = 6;
            this.cbSystem.Text = "System Data";
            this.cbSystem.UseVisualStyleBackColor = true;
            this.cbSystem.CheckedChanged += new System.EventHandler(this.cbSystem_CheckedChanged);
            // 
            // rbImportError
            // 
            this.rbImportError.AutoSize = true;
            this.rbImportError.Location = new System.Drawing.Point(30, 105);
            this.rbImportError.Name = "rbImportError";
            this.rbImportError.Size = new System.Drawing.Size(84, 17);
            this.rbImportError.TabIndex = 5;
            this.rbImportError.TabStop = true;
            this.rbImportError.Text = "Import Errors";
            this.rbImportError.UseVisualStyleBackColor = true;
            // 
            // rbExportError
            // 
            this.rbExportError.AutoSize = true;
            this.rbExportError.Location = new System.Drawing.Point(30, 82);
            this.rbExportError.Name = "rbExportError";
            this.rbExportError.Size = new System.Drawing.Size(85, 17);
            this.rbExportError.TabIndex = 4;
            this.rbExportError.TabStop = true;
            this.rbExportError.Text = "Export Errors";
            this.rbExportError.UseVisualStyleBackColor = true;
            // 
            // rbExport
            // 
            this.rbExport.AutoSize = true;
            this.rbExport.Location = new System.Drawing.Point(31, 59);
            this.rbExport.Name = "rbExport";
            this.rbExport.Size = new System.Drawing.Size(55, 17);
            this.rbExport.TabIndex = 3;
            this.rbExport.TabStop = true;
            this.rbExport.Text = "Export";
            this.rbExport.UseVisualStyleBackColor = true;
            // 
            // rbImport
            // 
            this.rbImport.AutoSize = true;
            this.rbImport.Location = new System.Drawing.Point(31, 36);
            this.rbImport.Name = "rbImport";
            this.rbImport.Size = new System.Drawing.Size(54, 17);
            this.rbImport.TabIndex = 2;
            this.rbImport.TabStop = true;
            this.rbImport.Text = "Import";
            this.rbImport.UseVisualStyleBackColor = true;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(31, 14);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(36, 17);
            this.rbAll.TabIndex = 0;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // cbbMAs
            // 
            this.cbbMAs.Enabled = false;
            this.cbbMAs.FormattingEnabled = true;
            this.cbbMAs.Location = new System.Drawing.Point(75, 62);
            this.cbbMAs.Name = "cbbMAs";
            this.cbbMAs.Size = new System.Drawing.Size(206, 21);
            this.cbbMAs.TabIndex = 6;
            // 
            // lblMAname
            // 
            this.lblMAname.AutoSize = true;
            this.lblMAname.Location = new System.Drawing.Point(41, 66);
            this.lblMAname.Name = "lblMAname";
            this.lblMAname.Size = new System.Drawing.Size(28, 13);
            this.lblMAname.TabIndex = 5;
            this.lblMAname.Text = "MAs";
            // 
            // tbFile
            // 
            this.tbFile.Enabled = false;
            this.tbFile.Location = new System.Drawing.Point(44, 234);
            this.tbFile.Name = "tbFile";
            this.tbFile.ReadOnly = true;
            this.tbFile.Size = new System.Drawing.Size(237, 20);
            this.tbFile.TabIndex = 4;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Enabled = false;
            this.btnOpenFile.Location = new System.Drawing.Point(287, 231);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 3;
            this.btnOpenFile.Text = "Select File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // rbFile
            // 
            this.rbFile.AutoSize = true;
            this.rbFile.Location = new System.Drawing.Point(24, 206);
            this.rbFile.Name = "rbFile";
            this.rbFile.Size = new System.Drawing.Size(279, 17);
            this.rbFile.TabIndex = 0;
            this.rbFile.TabStop = true;
            this.rbFile.Text = "Provide existing file  (made using one of the examples)";
            this.rbFile.UseVisualStyleBackColor = true;
            this.rbFile.CheckedChanged += new System.EventHandler(this.rbFile_CheckedChanged);
            // 
            // rbGenerate
            // 
            this.rbGenerate.AutoSize = true;
            this.rbGenerate.Location = new System.Drawing.Point(24, 29);
            this.rbGenerate.Name = "rbGenerate";
            this.rbGenerate.Size = new System.Drawing.Size(272, 17);
            this.rbGenerate.TabIndex = 1;
            this.rbGenerate.TabStop = true;
            this.rbGenerate.Text = "Generate file   (must be running on FIM Sync Server)";
            this.rbGenerate.UseVisualStyleBackColor = true;
            this.rbGenerate.CheckedChanged += new System.EventHandler(this.rbGenerate_CheckedChanged);
            // 
            // ofdCSfile
            // 
            this.ofdCSfile.DefaultExt = "xml";
            this.ofdCSfile.Filter = "XML Files (.xml)|*.xml";
            // 
            // sfdReport
            // 
            this.sfdReport.DefaultExt = "html";
            this.sfdReport.Filter = "xml files (*.xml)|*.xml";
            // 
            // frmGetData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 300);
            this.Controls.Add(this.gbSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmGetData";
            this.Text = "Get Data";
            this.VisibleChanged += new System.EventHandler(this.frmGetData_VisibleChanged);
            this.gbSource.ResumeLayout(false);
            this.gbSource.PerformLayout();
            this.gbDataSelection.ResumeLayout(false);
            this.gbDataSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSource;
        private System.Windows.Forms.RadioButton rbFile;
        private System.Windows.Forms.RadioButton rbGenerate;
        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.OpenFileDialog ofdCSfile;
        private System.Windows.Forms.Label lblMAname;
        private System.Windows.Forms.ComboBox cbbMAs;
        private System.Windows.Forms.GroupBox gbDataSelection;
        private System.Windows.Forms.RadioButton rbExport;
        private System.Windows.Forms.RadioButton rbImport;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.SaveFileDialog sfdReport;
        private System.Windows.Forms.RadioButton rbImportError;
        private System.Windows.Forms.RadioButton rbExportError;
        private System.Windows.Forms.Button btnShowExamples;
        private System.Windows.Forms.CheckBox cbSystem;
    }
}