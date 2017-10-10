namespace csReporter
{
    partial class frmFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFilter));
            this.lbObjectType = new System.Windows.Forms.ListBox();
            this.lbOperation = new System.Windows.Forms.ListBox();
            this.lbAttribute = new System.Windows.Forms.ListBox();
            this.lblOperation = new System.Windows.Forms.Label();
            this.lblAttributes = new System.Windows.Forms.Label();
            this.lblObjectType = new System.Windows.Forms.Label();
            this.gbFilterState = new System.Windows.Forms.GroupBox();
            this.rbUnconfirmedExport = new System.Windows.Forms.RadioButton();
            this.rbEscrowedExport = new System.Windows.Forms.RadioButton();
            this.rbSynchronized = new System.Windows.Forms.RadioButton();
            this.rbUnappliedExport = new System.Windows.Forms.RadioButton();
            this.rbPendingImport = new System.Windows.Forms.RadioButton();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnCreateReport = new System.Windows.Forms.Button();
            this.sfdReport = new System.Windows.Forms.SaveFileDialog();
            this.cbADMA = new System.Windows.Forms.CheckBox();
            this.cbbComparators = new System.Windows.Forms.ComboBox();
            this.btnAddFilter = new System.Windows.Forms.Button();
            this.cbbAttributes = new System.Windows.Forms.ComboBox();
            this.btnRemoveFilter = new System.Windows.Forms.Button();
            this.lblValue = new System.Windows.Forms.Label();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.lblComparator = new System.Windows.Forms.Label();
            this.dgvAdvanced = new System.Windows.Forms.DataGridView();
            this.btnBack = new System.Windows.Forms.Button();
            this.cbNonChanging = new System.Windows.Forms.CheckBox();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.cbSystemAttribs = new System.Windows.Forms.CheckBox();
            this.cbbValue = new System.Windows.Forms.ComboBox();
            this.gbFilterState.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdvanced)).BeginInit();
            this.SuspendLayout();
            // 
            // lbObjectType
            // 
            this.lbObjectType.FormattingEnabled = true;
            this.lbObjectType.Location = new System.Drawing.Point(180, 26);
            this.lbObjectType.Name = "lbObjectType";
            this.lbObjectType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbObjectType.Size = new System.Drawing.Size(114, 160);
            this.lbObjectType.Sorted = true;
            this.lbObjectType.TabIndex = 2;
            this.lbObjectType.SelectedValueChanged += new System.EventHandler(this.lbObjectType_SelectedValueChanged);
            // 
            // lbOperation
            // 
            this.lbOperation.FormattingEnabled = true;
            this.lbOperation.Location = new System.Drawing.Point(307, 26);
            this.lbOperation.Name = "lbOperation";
            this.lbOperation.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbOperation.Size = new System.Drawing.Size(114, 160);
            this.lbOperation.Sorted = true;
            this.lbOperation.TabIndex = 3;
            this.lbOperation.SelectedValueChanged += new System.EventHandler(this.lbOperation_SelectedValueChanged);
            // 
            // lbAttribute
            // 
            this.lbAttribute.FormattingEnabled = true;
            this.lbAttribute.Location = new System.Drawing.Point(867, 26);
            this.lbAttribute.Name = "lbAttribute";
            this.lbAttribute.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAttribute.Size = new System.Drawing.Size(182, 251);
            this.lbAttribute.Sorted = true;
            this.lbAttribute.TabIndex = 4;
            this.lbAttribute.SelectedValueChanged += new System.EventHandler(this.lbAttribute_SelectedValueChanged);
            // 
            // lblOperation
            // 
            this.lblOperation.AutoSize = true;
            this.lblOperation.Location = new System.Drawing.Point(304, 10);
            this.lblOperation.Name = "lblOperation";
            this.lblOperation.Size = new System.Drawing.Size(58, 13);
            this.lblOperation.TabIndex = 6;
            this.lblOperation.Text = "Operations";
            // 
            // lblAttributes
            // 
            this.lblAttributes.AutoSize = true;
            this.lblAttributes.Location = new System.Drawing.Point(864, 10);
            this.lblAttributes.Name = "lblAttributes";
            this.lblAttributes.Size = new System.Drawing.Size(108, 13);
            this.lblAttributes.TabIndex = 7;
            this.lblAttributes.Text = "Attributes to report on";
            // 
            // lblObjectType
            // 
            this.lblObjectType.AutoSize = true;
            this.lblObjectType.Location = new System.Drawing.Point(177, 10);
            this.lblObjectType.Name = "lblObjectType";
            this.lblObjectType.Size = new System.Drawing.Size(70, 13);
            this.lblObjectType.TabIndex = 8;
            this.lblObjectType.Text = "Object Types";
            // 
            // gbFilterState
            // 
            this.gbFilterState.Controls.Add(this.rbUnconfirmedExport);
            this.gbFilterState.Controls.Add(this.rbEscrowedExport);
            this.gbFilterState.Controls.Add(this.rbSynchronized);
            this.gbFilterState.Controls.Add(this.rbUnappliedExport);
            this.gbFilterState.Controls.Add(this.rbPendingImport);
            this.gbFilterState.Location = new System.Drawing.Point(12, 26);
            this.gbFilterState.Name = "gbFilterState";
            this.gbFilterState.Size = new System.Drawing.Size(149, 175);
            this.gbFilterState.TabIndex = 9;
            this.gbFilterState.TabStop = false;
            this.gbFilterState.Text = "Data Type";
            // 
            // rbUnconfirmedExport
            // 
            this.rbUnconfirmedExport.AutoSize = true;
            this.rbUnconfirmedExport.Location = new System.Drawing.Point(18, 146);
            this.rbUnconfirmedExport.Name = "rbUnconfirmedExport";
            this.rbUnconfirmedExport.Size = new System.Drawing.Size(118, 17);
            this.rbUnconfirmedExport.TabIndex = 7;
            this.rbUnconfirmedExport.Text = "Unconfirmed-Export";
            this.rbUnconfirmedExport.UseVisualStyleBackColor = true;
            this.rbUnconfirmedExport.CheckedChanged += new System.EventHandler(this.rbUnconfirmedExport_CheckedChanged);
            // 
            // rbEscrowedExport
            // 
            this.rbEscrowedExport.AutoSize = true;
            this.rbEscrowedExport.Location = new System.Drawing.Point(18, 116);
            this.rbEscrowedExport.Name = "rbEscrowedExport";
            this.rbEscrowedExport.Size = new System.Drawing.Size(105, 17);
            this.rbEscrowedExport.TabIndex = 6;
            this.rbEscrowedExport.Text = "Escrowed-Export";
            this.rbEscrowedExport.UseVisualStyleBackColor = true;
            this.rbEscrowedExport.CheckedChanged += new System.EventHandler(this.rbEscrowedExport_CheckedChanged);
            // 
            // rbSynchronized
            // 
            this.rbSynchronized.AutoSize = true;
            this.rbSynchronized.Location = new System.Drawing.Point(18, 84);
            this.rbSynchronized.Name = "rbSynchronized";
            this.rbSynchronized.Size = new System.Drawing.Size(89, 17);
            this.rbSynchronized.TabIndex = 5;
            this.rbSynchronized.Text = "Synchronized";
            this.rbSynchronized.UseVisualStyleBackColor = true;
            this.rbSynchronized.CheckedChanged += new System.EventHandler(this.rbSynchronized_CheckedChanged);
            // 
            // rbUnappliedExport
            // 
            this.rbUnappliedExport.AutoSize = true;
            this.rbUnappliedExport.Location = new System.Drawing.Point(18, 52);
            this.rbUnappliedExport.Name = "rbUnappliedExport";
            this.rbUnappliedExport.Size = new System.Drawing.Size(106, 17);
            this.rbUnappliedExport.TabIndex = 4;
            this.rbUnappliedExport.Text = "Unapplied-Export";
            this.rbUnappliedExport.UseVisualStyleBackColor = true;
            this.rbUnappliedExport.CheckedChanged += new System.EventHandler(this.rbUnappliedExport_CheckedChanged);
            // 
            // rbPendingImport
            // 
            this.rbPendingImport.AutoSize = true;
            this.rbPendingImport.Location = new System.Drawing.Point(18, 21);
            this.rbPendingImport.Name = "rbPendingImport";
            this.rbPendingImport.Size = new System.Drawing.Size(96, 17);
            this.rbPendingImport.TabIndex = 3;
            this.rbPendingImport.Text = "Pending-Import";
            this.rbPendingImport.UseVisualStyleBackColor = true;
            this.rbPendingImport.CheckedChanged += new System.EventHandler(this.rbPendingImport_CheckedChanged);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(9, 256);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(85, 13);
            this.lblCount.TabIndex = 11;
            this.lblCount.Text = "Matching Count:";
            // 
            // btnCreateReport
            // 
            this.btnCreateReport.Location = new System.Drawing.Point(331, 250);
            this.btnCreateReport.Name = "btnCreateReport";
            this.btnCreateReport.Size = new System.Drawing.Size(90, 25);
            this.btnCreateReport.TabIndex = 12;
            this.btnCreateReport.Text = "Create Report";
            this.btnCreateReport.UseVisualStyleBackColor = true;
            this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
            // 
            // sfdReport
            // 
            this.sfdReport.DefaultExt = "html";
            // 
            // cbADMA
            // 
            this.cbADMA.AutoSize = true;
            this.cbADMA.Location = new System.Drawing.Point(210, 198);
            this.cbADMA.Name = "cbADMA";
            this.cbADMA.Size = new System.Drawing.Size(169, 17);
            this.cbADMA.TabIndex = 13;
            this.cbADMA.Text = "Contains Active Directory data\r\n";
            this.cbADMA.UseVisualStyleBackColor = true;
            this.cbADMA.CheckedChanged += new System.EventHandler(this.cbADMA_CheckedChanged);
            // 
            // cbbComparators
            // 
            this.cbbComparators.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbComparators.FormattingEnabled = true;
            this.cbbComparators.Items.AddRange(new object[] {
            "Equals",
            "Does not equal",
            "Starts with",
            "Does not start with",
            "Ends with",
            "Does not end with",
            "Contains",
            "Does not contain",
            "Is present",
            "Is not present"});
            this.cbbComparators.Location = new System.Drawing.Point(635, 26);
            this.cbbComparators.Name = "cbbComparators";
            this.cbbComparators.Size = new System.Drawing.Size(129, 21);
            this.cbbComparators.TabIndex = 16;
            this.cbbComparators.SelectedIndexChanged += new System.EventHandler(this.cbbComparators_SelectedIndexChanged);
            // 
            // btnAddFilter
            // 
            this.btnAddFilter.Location = new System.Drawing.Point(717, 74);
            this.btnAddFilter.Name = "btnAddFilter";
            this.btnAddFilter.Size = new System.Drawing.Size(57, 20);
            this.btnAddFilter.TabIndex = 18;
            this.btnAddFilter.Text = "Add";
            this.btnAddFilter.UseVisualStyleBackColor = true;
            this.btnAddFilter.Click += new System.EventHandler(this.btnAddFilter_Click);
            // 
            // cbbAttributes
            // 
            this.cbbAttributes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbAttributes.FormattingEnabled = true;
            this.cbbAttributes.Location = new System.Drawing.Point(441, 26);
            this.cbbAttributes.Name = "cbbAttributes";
            this.cbbAttributes.Size = new System.Drawing.Size(184, 21);
            this.cbbAttributes.Sorted = true;
            this.cbbAttributes.TabIndex = 19;
            this.cbbAttributes.SelectedIndexChanged += new System.EventHandler(this.cbbAttributes_SelectedIndexChanged);
            // 
            // btnRemoveFilter
            // 
            this.btnRemoveFilter.Location = new System.Drawing.Point(789, 74);
            this.btnRemoveFilter.Name = "btnRemoveFilter";
            this.btnRemoveFilter.Size = new System.Drawing.Size(57, 20);
            this.btnRemoveFilter.TabIndex = 20;
            this.btnRemoveFilter.Text = "Remove";
            this.btnRemoveFilter.UseVisualStyleBackColor = true;
            this.btnRemoveFilter.Click += new System.EventHandler(this.btnRemoveFilter_Click);
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(438, 58);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(37, 13);
            this.lblValue.TabIndex = 21;
            this.lblValue.Text = "Value:";
            // 
            // lblAttribute
            // 
            this.lblAttribute.AutoSize = true;
            this.lblAttribute.Location = new System.Drawing.Point(438, 10);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(49, 13);
            this.lblAttribute.TabIndex = 22;
            this.lblAttribute.Text = "Attribute:";
            // 
            // lblComparator
            // 
            this.lblComparator.AutoSize = true;
            this.lblComparator.Location = new System.Drawing.Point(632, 10);
            this.lblComparator.Name = "lblComparator";
            this.lblComparator.Size = new System.Drawing.Size(56, 13);
            this.lblComparator.TabIndex = 23;
            this.lblComparator.Text = "Operation:";
            // 
            // dgvAdvanced
            // 
            this.dgvAdvanced.AllowUserToAddRows = false;
            this.dgvAdvanced.AllowUserToDeleteRows = false;
            this.dgvAdvanced.AllowUserToResizeRows = false;
            this.dgvAdvanced.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAdvanced.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdvanced.Location = new System.Drawing.Point(441, 153);
            this.dgvAdvanced.MultiSelect = false;
            this.dgvAdvanced.Name = "dgvAdvanced";
            this.dgvAdvanced.ReadOnly = true;
            this.dgvAdvanced.RowHeadersVisible = false;
            this.dgvAdvanced.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAdvanced.Size = new System.Drawing.Size(405, 123);
            this.dgvAdvanced.TabIndex = 24;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(180, 250);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(90, 25);
            this.btnBack.TabIndex = 25;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // cbNonChanging
            // 
            this.cbNonChanging.AutoSize = true;
            this.cbNonChanging.Location = new System.Drawing.Point(443, 101);
            this.cbNonChanging.Name = "cbNonChanging";
            this.cbNonChanging.Size = new System.Drawing.Size(175, 17);
            this.cbNonChanging.TabIndex = 27;
            this.cbNonChanging.Text = "Include non-changing attributes";
            this.cbNonChanging.UseVisualStyleBackColor = true;
            this.cbNonChanging.CheckedChanged += new System.EventHandler(this.cbNonChanging_CheckedChanged);
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Location = new System.Drawing.Point(9, 237);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(65, 13);
            this.lblTotalCount.TabIndex = 28;
            this.lblTotalCount.Text = "Total Count:";
            // 
            // cbSystemAttribs
            // 
            this.cbSystemAttribs.AutoSize = true;
            this.cbSystemAttribs.Location = new System.Drawing.Point(443, 124);
            this.cbSystemAttribs.Name = "cbSystemAttribs";
            this.cbSystemAttribs.Size = new System.Drawing.Size(142, 17);
            this.cbSystemAttribs.TabIndex = 29;
            this.cbSystemAttribs.Text = "Include system attributes";
            this.cbSystemAttribs.UseVisualStyleBackColor = true;
            this.cbSystemAttribs.CheckedChanged += new System.EventHandler(this.cbSystemAttribs_CheckedChanged);
            // 
            // cbbValue
            // 
            this.cbbValue.FormattingEnabled = true;
            this.cbbValue.Location = new System.Drawing.Point(443, 75);
            this.cbbValue.Name = "cbbValue";
            this.cbbValue.Size = new System.Drawing.Size(268, 21);
            this.cbbValue.TabIndex = 30;
            // 
            // frmFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 288);
            this.Controls.Add(this.cbbValue);
            this.Controls.Add(this.cbSystemAttribs);
            this.Controls.Add(this.lblTotalCount);
            this.Controls.Add(this.cbNonChanging);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.dgvAdvanced);
            this.Controls.Add(this.lblComparator);
            this.Controls.Add(this.lblAttribute);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.btnRemoveFilter);
            this.Controls.Add(this.cbbAttributes);
            this.Controls.Add(this.btnAddFilter);
            this.Controls.Add(this.cbbComparators);
            this.Controls.Add(this.cbADMA);
            this.Controls.Add(this.btnCreateReport);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.gbFilterState);
            this.Controls.Add(this.lblObjectType);
            this.Controls.Add(this.lblAttributes);
            this.Controls.Add(this.lblOperation);
            this.Controls.Add(this.lbAttribute);
            this.Controls.Add(this.lbOperation);
            this.Controls.Add(this.lbObjectType);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmFilter";
            this.Text = "Filter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmFilter_FormClosed);
            this.Shown += new System.EventHandler(this.frmFilter_Shown);
            this.gbFilterState.ResumeLayout(false);
            this.gbFilterState.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdvanced)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbObjectType;
        private System.Windows.Forms.ListBox lbOperation;
        private System.Windows.Forms.ListBox lbAttribute;
        private System.Windows.Forms.Label lblOperation;
        private System.Windows.Forms.Label lblAttributes;
        private System.Windows.Forms.Label lblObjectType;
        private System.Windows.Forms.GroupBox gbFilterState;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Button btnCreateReport;
        private System.Windows.Forms.SaveFileDialog sfdReport;
        private System.Windows.Forms.CheckBox cbADMA;
        private System.Windows.Forms.ComboBox cbbComparators;
        private System.Windows.Forms.Button btnAddFilter;
        private System.Windows.Forms.ComboBox cbbAttributes;
        private System.Windows.Forms.Button btnRemoveFilter;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Label lblAttribute;
        private System.Windows.Forms.Label lblComparator;
        public System.Windows.Forms.DataGridView dgvAdvanced;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.RadioButton rbPendingImport;
        private System.Windows.Forms.RadioButton rbSynchronized;
        private System.Windows.Forms.RadioButton rbUnappliedExport;
        private System.Windows.Forms.CheckBox cbNonChanging;
        private System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.CheckBox cbSystemAttribs;
        private System.Windows.Forms.ComboBox cbbValue;
        private System.Windows.Forms.RadioButton rbUnconfirmedExport;
        private System.Windows.Forms.RadioButton rbEscrowedExport;
    }
}

