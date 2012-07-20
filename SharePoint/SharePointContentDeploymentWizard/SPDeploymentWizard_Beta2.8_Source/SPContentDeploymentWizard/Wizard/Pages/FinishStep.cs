using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using COB.SharePoint.Utilities.DeploymentWizard.UI;

namespace WizardBase
{
    [ToolboxItem(false), Designer(typeof (WizardStepDesigner)), DefaultEvent("Click")]
    public class FinishStep : WizardStep
    {
        public FinishStep()
        {
            BackColor = SystemColors.ControlLightLight;
            BackgroundImage = Resources.back;
        }

        internal override void Reset()
        {
            BackColor = SystemColors.ControlLightLight;
            BackgroundImage = Resources.back;
            BackgroundImageLayout = ImageLayout.Tile;
        }

        [DefaultValue(typeof (Color), "ControlLightLight")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }
    }
}