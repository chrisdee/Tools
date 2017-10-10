namespace csReporter
{
    partial class frmReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReport));
            this.lbAttribute = new System.Windows.Forms.ListBox();
            this.gbFormat = new System.Windows.Forms.GroupBox();
            this.rbCSV = new System.Windows.Forms.RadioButton();
            this.rbHTML = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbAttribute
            // 
            this.lbAttribute.FormattingEnabled = true;
            this.lbAttribute.Location = new System.Drawing.Point(12, 12);
            this.lbAttribute.Name = "lbAttribute";
            this.lbAttribute.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAttribute.Size = new System.Drawing.Size(182, 316);
            this.lbAttribute.Sorted = true;
            this.lbAttribute.TabIndex = 5;
            // 
            // gbFormat
            // 
            this.gbFormat.Controls.Add(this.rbCSV);
            this.gbFormat.Controls.Add(this.rbHTML);
            this.gbFormat.Location = new System.Drawing.Point(209, 12);
            this.gbFormat.Name = "gbFormat";
            this.gbFormat.Size = new System.Drawing.Size(88, 100);
            this.gbFormat.TabIndex = 6;
            this.gbFormat.TabStop = false;
            this.gbFormat.Text = "Format";
            // 
            // rbCSV
            // 
            this.rbCSV.AutoSize = true;
            this.rbCSV.Location = new System.Drawing.Point(15, 62);
            this.rbCSV.Name = "rbCSV";
            this.rbCSV.Size = new System.Drawing.Size(46, 17);
            this.rbCSV.TabIndex = 1;
            this.rbCSV.TabStop = true;
            this.rbCSV.Text = "CSV";
            this.rbCSV.UseVisualStyleBackColor = true;
            // 
            // rbHTML
            // 
            this.rbHTML.AutoSize = true;
            this.rbHTML.Location = new System.Drawing.Point(15, 29);
            this.rbHTML.Name = "rbHTML";
            this.rbHTML.Size = new System.Drawing.Size(55, 17);
            this.rbHTML.TabIndex = 0;
            this.rbHTML.TabStop = true;
            this.rbHTML.Text = "HTML";
            this.rbHTML.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(55, 348);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 25);
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(165, 348);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 389);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbFormat);
            this.Controls.Add(this.lbAttribute);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReport";
            this.Text = "Report Settings";
            this.gbFormat.ResumeLayout(false);
            this.gbFormat.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbAttribute;
        private System.Windows.Forms.GroupBox gbFormat;
        private System.Windows.Forms.RadioButton rbCSV;
        private System.Windows.Forms.RadioButton rbHTML;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}