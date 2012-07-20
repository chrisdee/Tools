namespace COB.SharePoint.Utilities.DeploymentWizard.UI
{
    partial class frmAbout
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
            this.lnllblBlogLink = new System.Windows.Forms.LinkLabel();
            this.btnAboutOK = new System.Windows.Forms.Button();
            this.pnlAbout = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lnllblBlogLink
            // 
            this.lnllblBlogLink.AutoSize = true;
            this.lnllblBlogLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnllblBlogLink.Location = new System.Drawing.Point(54, 49);
            this.lnllblBlogLink.Name = "lnllblBlogLink";
            this.lnllblBlogLink.Size = new System.Drawing.Size(168, 26);
            this.lnllblBlogLink.TabIndex = 0;
            this.lnllblBlogLink.TabStop = true;
            this.lnllblBlogLink.Text = "Content Deployment Wizard \r\ncreated by Chris O\'Brien";
            this.lnllblBlogLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnllblBlogLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnllblBlogLink_LinkClicked);
            // 
            // btnAboutOK
            // 
            this.btnAboutOK.Location = new System.Drawing.Point(96, 115);
            this.btnAboutOK.Name = "btnAboutOK";
            this.btnAboutOK.Size = new System.Drawing.Size(94, 25);
            this.btnAboutOK.TabIndex = 1;
            this.btnAboutOK.Text = "OK";
            this.btnAboutOK.UseVisualStyleBackColor = true;
            this.btnAboutOK.Click += new System.EventHandler(this.btnAboutOK_Click);
            // 
            // pnlAbout
            // 
            this.pnlAbout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAbout.Location = new System.Drawing.Point(0, 0);
            this.pnlAbout.Name = "pnlAbout";
            this.pnlAbout.Size = new System.Drawing.Size(282, 191);
            this.pnlAbout.TabIndex = 2;
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(282, 191);
            this.Controls.Add(this.btnAboutOK);
            this.Controls.Add(this.lnllblBlogLink);
            this.Controls.Add(this.pnlAbout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmAbout";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnllblBlogLink;
        private System.Windows.Forms.Button btnAboutOK;
        private System.Windows.Forms.Panel pnlAbout;
    }
}