using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace COB.SharePoint.Utilities.DeploymentWizard.UI
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }
        
        private void lnllblBlogLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open browser, please paste link into a browser you have opened.");
            }
        }

        private void VisitLink()
        {
            lnllblBlogLink.LinkVisited = true;
            System.Diagnostics.Process.Start("http://www.sharepointnutsandbolts.com");
            this.Dispose();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            centerFormTo(this, this.Owner);
            lnllblBlogLink.LinkArea = new LinkArea(39, 15);
        }

        private void btnAboutOK_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void centerFormTo(Form form, Form containerForm)
        {
            Point point = new Point();
            Size formSize = form.Size;
            Rectangle workingArea = Screen.GetWorkingArea(containerForm);
            Rectangle rect = containerForm.Bounds;
            point.X = ((rect.Left + rect.Right) - formSize.Width) / 2;
            if (point.X < workingArea.X)
            {
                point.X = workingArea.X;
            }
            else if ((point.X + formSize.Width) > (workingArea.X + workingArea.Width))
            {
                point.X = (workingArea.X + workingArea.Width) - formSize.Width;
            }
            point.Y = ((rect.Top + rect.Bottom) - formSize.Height) / 2;
            if (point.Y < workingArea.Y)
            {
                point.Y = workingArea.Y;
            }
            else if ((point.Y + formSize.Height) > (workingArea.Y + workingArea.Height))
            {
                point.Y = (workingArea.Y + workingArea.Height) - formSize.Height;
            }
            form.Location = point;
        } 
    }
}