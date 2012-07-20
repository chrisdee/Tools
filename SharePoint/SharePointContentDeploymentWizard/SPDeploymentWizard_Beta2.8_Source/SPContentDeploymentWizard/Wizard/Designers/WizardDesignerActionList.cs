using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;

namespace WizardBase
{
    internal class WizardDesignerActionList : DesignerActionList
    {
        public WizardDesignerActionList(IComponent component) : base(component)
        {
        }

        protected internal virtual void AddFinishStep()
        {
            WizardControl wizardControl = WizardControl;
            if (wizardControl == null)
            {
                return;
            }
            IDesignerHost service = (IDesignerHost) GetService(typeof (IDesignerHost));
            if (service == null)
            {
                return;
            }
            FinishStep step = (FinishStep) service.CreateComponent(typeof (FinishStep));
            if (wizardControl.WizardSteps.Count != 0)
            {
                wizardControl.WizardSteps.Insert(wizardControl.CurrentStepIndex, step);
                RemoveWizardFromSelection();
                SelectWizard();
                return;
            }
            wizardControl.WizardSteps.Add(step);
            RemoveWizardFromSelection();
            SelectWizard();
        }

        protected internal virtual void AddCustomStep()
        {
            WizardControl wizardControl = WizardControl;
            if (wizardControl == null)
            {
                return;
            }
            IDesignerHost service = (IDesignerHost) GetService(typeof (IDesignerHost));
            if (service == null)
            {
                return;
            }
            IntermediateStep step = (IntermediateStep) service.CreateComponent(typeof (IntermediateStep));
            if (wizardControl.WizardSteps.Count != 0)
            {
                wizardControl.WizardSteps.Insert(wizardControl.CurrentStepIndex, step);
                RemoveWizardFromSelection();
                SelectWizard();
            }
            else
            {
                wizardControl.WizardSteps.Add(step);
                RemoveWizardFromSelection();
                SelectWizard();
            }
        }

        protected internal virtual void AddStartStep()
        {
            WizardControl wizardControl = WizardControl;
            if (wizardControl == null)
            {
                return;
            }
            IDesignerHost service = (IDesignerHost) GetService(typeof (IDesignerHost));
            if (service == null)
            {
                return;
            }
            StartStep step = (StartStep) service.CreateComponent(typeof (StartStep));
            if (wizardControl.WizardSteps.Count != 0)
            {
                wizardControl.WizardSteps.Insert(wizardControl.CurrentStepIndex, step);
                RemoveWizardFromSelection();
                SelectWizard();
                return;
            }
            else
            {
                wizardControl.WizardSteps.Add(step);
                RemoveWizardFromSelection();
                SelectWizard();
                return;
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            WizardControl wizardControl = WizardControl;
            if (wizardControl == null)
            {
                return new DesignerActionItemCollection();
            }
            if (wizardControl.CurrentStepIndex != -1)
            {
                return AddStepMenu(wizardControl);
            }
            return new DesignerActionItemCollection();
        }

        private DesignerActionItemCollection AddStepMenu(WizardControl wizardControl)
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionHeaderItem("Add Steps"));
            items.Add(new DesignerActionPropertyItem("WizardSteps", "New Wizard Step", "Add Steps"));
            items.Add(new DesignerActionMethodItem(this, "AddStartStep", "Add Start Step", "Add Steps", true));
            items.Add(new DesignerActionMethodItem(this, "AddCustomStep", "Add Custom Step", "Add Steps", true));
            items.Add(new DesignerActionMethodItem(this, "AddFinishStep", "Add Finish Step", "Add Steps", true));
            if (wizardControl.CurrentStepIndex == -1)
            {
                return items;
            }
            items.Add(new DesignerActionHeaderItem("Remove Step"));
            items.Add(new DesignerActionMethodItem(this, "RemoveStep", "Remove Step", "Remove Step", true));
            items.Add(new DesignerActionMethodItem(this, "RemoveAllSteps", "Remove All Steps", "Remove Step", true));
            if (wizardControl.WizardSteps.Count >= 1)
            {
                items.Add(new DesignerActionHeaderItem("Step navigation"));
                if (wizardControl.CurrentStepIndex > 0)
                {
                    items.Add(new DesignerActionMethodItem(this, "PreviousStep", "Previous Step", "Step navigation", true));
                }
                if (wizardControl.CurrentStepIndex != (wizardControl.WizardSteps.Count - 1))
                {
                    items.Add(new DesignerActionMethodItem(this, "NextStep", "Next Step", "Step navigation", true));
                }
            }
            items.Add(new DesignerActionHeaderItem("Layout"));
            items.Add(new DesignerActionPropertyItem("DockStyle", "Dock editor", "Layout"));
            return items;
        }

        protected internal virtual void NextStep()
        {
            WizardControl wizardControl = WizardControl;
            if (wizardControl == null)
            {
                return;
            }
            wizardControl.CurrentStepIndex++;
            RemoveWizardFromSelection();
            SelectWizard();
        }

        protected internal virtual void PreviousStep()
        {
            WizardControl wizardControl = WizardControl;
            if (wizardControl == null)
            {
                return;
            }
            wizardControl.CurrentStepIndex--;
            RemoveWizardFromSelection();
            SelectWizard();
        }

        protected internal virtual void RemoveAllSteps()
        {
            WizardControl wizardControl = WizardControl;
            if (wizardControl == null)
            {
                return;
            }
            if (MessageBox.Show(wizardControl.FindForm(), "Are you sure you want to remove all the steps?", "Remove Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            IDesignerHost service = (IDesignerHost) GetService(typeof (IDesignerHost));
            if (service == null)
            {
                return;
            }
            WizardStep[] array = new WizardStep[wizardControl.WizardSteps.Count];
            wizardControl.WizardSteps.CopyTo(array, 0);
            wizardControl.WizardSteps.Clear();
            WizardStep[] stepArray2 = array;
            for (int index = 0; index < stepArray2.Length; index++)
            {
                WizardStep component = stepArray2[index];
                service.DestroyComponent(component);
                index++;
            }
            SelectWizard();
        }

        protected internal virtual void RemoveStep()
        {
            WizardControl wizardControl = WizardControl;
            if (wizardControl == null)
            {
                return;
            }
            IDesignerHost service = (IDesignerHost) GetService(typeof (IDesignerHost));
            if (service == null)
            {
                return;
            }
            if (MessageBox.Show(wizardControl.FindForm(), "Are you sure you want to remove the step?", "Remove Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                WizardStep step = wizardControl.WizardSteps[wizardControl.CurrentStepIndex];
                wizardControl.WizardSteps.Remove(step);
                service.DestroyComponent(step);
                step.Dispose();
            }
            SelectWizard();
        }

        protected void RemoveWizardFromSelection()
        {
            WizardControl wizardControl = WizardControl;
            if (wizardControl == null)
            {
                return;
            }
            ISelectionService service = (ISelectionService) GetService(typeof (ISelectionService));
            if (service == null)
            {
                return;
            }
            object[] components = new object[] {wizardControl};
            service.SetSelectedComponents(components, SelectionTypes.Remove);
            return;
        }

        protected void SelectWizard()
        {
            WizardControl wizardControl = WizardControl;
            if (wizardControl == null)
            {
                return;
            }
            ISelectionService service = (ISelectionService) GetService(typeof (ISelectionService));
            if (service == null)
            {
                return;
            }
            object[] components = new object[] {wizardControl};
            service.SetSelectedComponents(components, SelectionTypes.Replace);
            return;
        }

        protected virtual WizardControl WizardControl
        {
            get { return (WizardControl) Component; }
        }

        [TypeConverter(typeof (WizardStepCollectionConverter)), Editor(typeof (WizardStepCollectionEditor), typeof (UITypeEditor))]
        public WizardStepCollection WizardSteps
        {
            get
            {
                WizardControl wizardControl = WizardControl;
                if (wizardControl == null)
                {
                    return null;
                }
                return wizardControl.WizardSteps;
            }
        }

        [TypeConverter(typeof (WizardCollectionEnumConverter)), Editor(typeof (WizardStepCollectionEnumEditor), typeof (UITypeEditor))]
        public WizardStepCollection WizardStepsEnum
        {
            get { return WizardSteps; }
        }

        public virtual DockStyle DockStyle
        {
            get { return WizardControl.Dock; }
            set { WizardControl.Dock = value; }
        }
    }
}