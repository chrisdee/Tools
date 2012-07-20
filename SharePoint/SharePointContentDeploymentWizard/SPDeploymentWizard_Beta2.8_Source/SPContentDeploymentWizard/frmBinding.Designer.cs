namespace COB.SharePoint.Utilities.DeploymentWizard.UI
{
    partial class frmBinding
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
            this.lblBindingOnChildForm = new System.Windows.Forms.Label();
            this.pnlBindingMessageForm = new System.Windows.Forms.Panel();
            this.lblProgressDots = new System.Windows.Forms.Label();
            this.timerSiteBindProgress = new System.Windows.Forms.Timer(this.components);
            this.pnlBindingMessageForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBindingOnChildForm
            // 
            this.lblBindingOnChildForm.AutoSize = true;
            this.lblBindingOnChildForm.Location = new System.Drawing.Point(48, 53);
            this.lblBindingOnChildForm.Name = "lblBindingOnChildForm";
            this.lblBindingOnChildForm.Size = new System.Drawing.Size(141, 13);
            this.lblBindingOnChildForm.TabIndex = 0;
            this.lblBindingOnChildForm.Text = "Binding to site - please wait..";
            // 
            // pnlBindingMessageForm
            // 
            this.pnlBindingMessageForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlBindingMessageForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBindingMessageForm.Controls.Add(this.lblProgressDots);
            this.pnlBindingMessageForm.Controls.Add(this.lblBindingOnChildForm);
            this.pnlBindingMessageForm.Location = new System.Drawing.Point(0, 0);
            this.pnlBindingMessageForm.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBindingMessageForm.Name = "pnlBindingMessageForm";
            this.pnlBindingMessageForm.Size = new System.Drawing.Size(243, 137);
            this.pnlBindingMessageForm.TabIndex = 1;
            // 
            // lblProgressDots
            // 
            this.lblProgressDots.AutoSize = true;
            this.lblProgressDots.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressDots.Location = new System.Drawing.Point(69, 76);
            this.lblProgressDots.Name = "lblProgressDots";
            this.lblProgressDots.Size = new System.Drawing.Size(0, 13);
            this.lblProgressDots.TabIndex = 1;
            // 
            // timerSiteBindProgress
            // 
            this.timerSiteBindProgress.Tick += new System.EventHandler(this.timerSiteBindProgress_Tick);
            // 
            // frmBinding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(243, 137);
            this.Controls.Add(this.pnlBindingMessageForm);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(400, 350);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBinding";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Please wait..";
            this.TopMost = true;
            this.pnlBindingMessageForm.ResumeLayout(false);
            this.pnlBindingMessageForm.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBindingOnChildForm;
        private System.Windows.Forms.Panel pnlBindingMessageForm;
        private System.Windows.Forms.Label lblProgressDots;
        private System.Windows.Forms.Timer timerSiteBindProgress;
    }
}