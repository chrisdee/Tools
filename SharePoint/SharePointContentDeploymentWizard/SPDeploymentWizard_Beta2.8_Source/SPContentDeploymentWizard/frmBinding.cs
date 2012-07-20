using System;
using System.Drawing;
using System.Windows.Forms;

namespace COB.SharePoint.Utilities.DeploymentWizard.UI
{
    public partial class frmBinding : Form
    {
        private int f_iDotCount = 0;

        public frmBinding()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmBinding_Load);
            
            frmContentDeployer.SiteBindCompleteEvent += new frmContentDeployer.SiteBindCompleteEventHandler(frmContentDeployer_SiteBindCompleteEvent);
        }

        void timerSiteBindProgress_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("Tick event");
            lblProgressDots.Text += ".";
            f_iDotCount++;

            if (f_iDotCount == 10)
            {
                lblProgressDots.Text = string.Empty;
            }
        }

        void frmBinding_Load(object sender, EventArgs e)
        {
            centerFormTo(this, this.Owner);
        }

        void frmContentDeployer_SiteBindCompleteEvent(object sender, EventArgs e)
        {
            this.Hide();
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