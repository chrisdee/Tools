namespace csReporter
{
    partial class frmProgressBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProgressBar));
            this.pbProcessing = new System.Windows.Forms.ProgressBar();
            this.lblAction = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pbProcessing
            // 
            this.pbProcessing.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.pbProcessing.Location = new System.Drawing.Point(12, 55);
            this.pbProcessing.Name = "pbProcessing";
            this.pbProcessing.Size = new System.Drawing.Size(365, 23);
            this.pbProcessing.TabIndex = 0;
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAction.Location = new System.Drawing.Point(126, 18);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(0, 25);
            this.lblAction.TabIndex = 1;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(383, 59);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(39, 16);
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "100%";
            // 
            // frmProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 96);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.pbProcessing);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProgressBar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProgressBar_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        #endregion

        private System.Windows.Forms.ProgressBar pbProcessing;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.Label lblProgress;
    }
}