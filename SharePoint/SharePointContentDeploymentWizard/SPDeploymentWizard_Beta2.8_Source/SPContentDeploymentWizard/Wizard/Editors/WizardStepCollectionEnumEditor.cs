using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WizardBase
{
    internal class WizardStepCollectionEnumEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService service = (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService));
            if (service == null)
            {
                return value;
            }
            WizardStepCollection steps = (WizardStepCollection) value;
            ListBox control = new ListBox();
            control.Tag = new object[] {context, provider, value};
            control.Dock = DockStyle.Fill;
            control.HorizontalScrollbar = true;
            control.SelectedIndexChanged += new EventHandler(OnListBoxSelectedIndexChanged);
            for (int i = 0; i < steps.Count; i++)
            {
                control.Items.Add(steps[i].Name);
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        private void OnListBoxSelectedIndexChanged(object sender, EventArgs args)
        {
            ListBox box = sender as ListBox;
            if (box == null)
            {
                return;
            }
            IServiceProvider provider = (IServiceProvider) ((object[]) box.Tag)[1];
            WizardStepCollection steps = (WizardStepCollection) ((object[]) box.Tag)[2];
            IWindowsFormsEditorService service = (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService));
            if (service == null)
            {
                return;
            }
            steps.Owner.CurrentStepIndex = box.SelectedIndex;
            service.CloseDropDown();
            box.Dispose();
        }
    }
}