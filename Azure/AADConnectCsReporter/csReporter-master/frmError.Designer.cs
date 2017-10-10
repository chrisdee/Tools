namespace csReporter
{
    partial class frmError
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmError));
            this.tbErrorInfo = new System.Windows.Forms.TextBox();
            this.tbErrorBanner = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbErrorInfo
            // 
            this.tbErrorInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tbErrorInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbErrorInfo.Location = new System.Drawing.Point(12, 89);
            this.tbErrorInfo.Multiline = true;
            this.tbErrorInfo.Name = "tbErrorInfo";
            this.tbErrorInfo.ReadOnly = true;
            this.tbErrorInfo.Size = new System.Drawing.Size(271, 28);
            this.tbErrorInfo.TabIndex = 0;
            this.tbErrorInfo.TabStop = false;
            // 
            // tbErrorBanner
            // 
            this.tbErrorBanner.BackColor = System.Drawing.SystemColors.Control;
            this.tbErrorBanner.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbErrorBanner.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbErrorBanner.Location = new System.Drawing.Point(12, 12);
            this.tbErrorBanner.Multiline = true;
            this.tbErrorBanner.Name = "tbErrorBanner";
            this.tbErrorBanner.ReadOnly = true;
            this.tbErrorBanner.Size = new System.Drawing.Size(271, 28);
            this.tbErrorBanner.TabIndex = 1;
            this.tbErrorBanner.TabStop = false;
            this.tbErrorBanner.WordWrap = false;
            // 
            // frmError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(343, 134);
            this.Controls.Add(this.tbErrorBanner);
            this.Controls.Add(this.tbErrorInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmError";
            this.Text = "Error";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbErrorInfo;
        private System.Windows.Forms.TextBox tbErrorBanner;



    }
}