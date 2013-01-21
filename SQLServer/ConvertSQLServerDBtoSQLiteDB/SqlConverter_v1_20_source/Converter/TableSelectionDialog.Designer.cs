namespace Converter
{
    partial class TableSelectionDialog
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
            this.grdTables = new System.Windows.Forms.DataGridView();
            this.colInclude = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colTableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdTables)).BeginInit();
            this.SuspendLayout();
            // 
            // grdTables
            // 
            this.grdTables.AllowUserToAddRows = false;
            this.grdTables.AllowUserToDeleteRows = false;
            this.grdTables.AllowUserToResizeColumns = false;
            this.grdTables.AllowUserToResizeRows = false;
            this.grdTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTables.BackgroundColor = System.Drawing.Color.White;
            this.grdTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInclude,
            this.colTableName});
            this.grdTables.Location = new System.Drawing.Point(12, 12);
            this.grdTables.Name = "grdTables";
            this.grdTables.RowHeadersVisible = false;
            this.grdTables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdTables.Size = new System.Drawing.Size(407, 328);
            this.grdTables.TabIndex = 0;
            // 
            // colInclude
            // 
            this.colInclude.HeaderText = "";
            this.colInclude.Name = "colInclude";
            this.colInclude.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colInclude.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colInclude.Width = 30;
            // 
            // colTableName
            // 
            this.colTableName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTableName.HeaderText = "Table Name";
            this.colTableName.Name = "colTableName";
            this.colTableName.ReadOnly = true;
            this.colTableName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.Location = new System.Drawing.Point(12, 346);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 1;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeselectAll.Location = new System.Drawing.Point(93, 346);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(89, 23);
            this.btnDeselectAll.TabIndex = 2;
            this.btnDeselectAll.Text = "Deselect All";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(343, 346);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(261, 346);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // TableSelectionDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(433, 377);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDeselectAll);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.grdTables);
            this.Name = "TableSelectionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Tables To Convert";
            ((System.ComponentModel.ISupportInitialize)(this.grdTables)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdTables;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colInclude;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTableName;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnDeselectAll;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}