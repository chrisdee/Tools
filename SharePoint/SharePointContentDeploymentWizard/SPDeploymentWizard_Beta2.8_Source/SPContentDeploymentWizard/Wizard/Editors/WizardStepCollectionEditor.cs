using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace WizardBase
{
    internal class WizardStepCollectionEditor : CollectionEditor
    {
        public WizardStepCollectionEditor(Type type) : base(type)
        {
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] {typeof (StartStep), typeof (IntermediateStep), typeof (FinishStep)};
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            WizardStepCollection steps = (WizardStepCollection) value;
            WizardControl owner = steps.Owner;
            IDesignerHost container = (IDesignerHost) context.Container;
            int count = steps.Count;
            object obj2 = base.EditValue(context, provider, value);
            if (steps.Count >= count)
            {
                return obj2;
            }
            SelectWizard(owner, container);
            return obj2;
        }

        private void SelectWizard(WizardControl wizardControl, IDesignerHost host)
        {
            if (wizardControl == null)
            {
                return;
            }
            if (host == null)
            {
                return;
            }
            while (true)
            {
                WizardDesigner designer = (WizardDesigner) host.GetDesigner(wizardControl);
                if (designer == null)
                {
                    return;
                }
                ISelectionService service = (ISelectionService) host.GetService(typeof (ISelectionService));
                if (service == null)
                {
                    return;
                }
                object[] components = new object[] {wizardControl};
                service.SetSelectedComponents(components, SelectionTypes.Replace);
                return;
            }
        }
    }
}