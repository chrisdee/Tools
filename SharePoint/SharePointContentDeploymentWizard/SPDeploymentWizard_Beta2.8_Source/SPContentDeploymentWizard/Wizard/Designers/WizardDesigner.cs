using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using COB.SharePoint.Utilities.DeploymentWizard.UI;

namespace WizardBase
{
    internal class WizardDesigner : ParentControlDesigner
    {
        #region Private Fields

        private DesignerActionListCollection actionListCollection = new DesignerActionListCollection();
        private bool forwardOnDrag;
        private bool selected;
        private WizardDesignerActionList wizardDesignerActionList;

        #endregion

        internal static void SetStyle(WizardStep step)
        {
            step.BackgroundImageLayout = ImageLayout.Stretch;
            step.ApplingTheme = true;
            Image[] imageArray = CollectImages();
            if ((step is StartStep))
            {
                StartStep startStep = (StartStep) step;
                startStep.BindingImage = imageArray[0];
                startStep.Icon = imageArray[3];
            }
            if ((step is IntermediateStep))
            {
                IntermediateStep intermediateStep = (IntermediateStep) step;
                intermediateStep.ForeColor = Color.FromName("Black");
                intermediateStep.BindingImage = imageArray[2];
                intermediateStep.Icon = imageArray[3];
            }
            if ((step is FinishStep))
            {
                FinishStep finishStep = (FinishStep) step;
                finishStep.BackgroundImage = imageArray[1];
            }
            step.ApplingTheme = false;
        }

        private static Image[] CollectImages()
        {
            Image[] images = new Image[4];
            images[0] = Resources.left;
            images[1] = Resources.icon;
            images[2] = Resources.Top;
            images[3] = Resources.back;
            return images;
        }

        protected override void Dispose(bool disposing)
        {
            ISelectionService service = (ISelectionService) GetService(typeof (ISelectionService));
            if (service != null)
            {
                service.SelectionChanged -= new EventHandler(OnSelectionChanged);
            }
            WizardControl control = (WizardControl) Control;
            control.CurrentStepIndexChanged -= new EventHandler(RefreshComponent);
            control.WizardSteps.StepAdded -= new EventHandler(RefreshComponent);
            IDesignerHost host = (IDesignerHost) GetService(typeof (IDesignerHost));
            IEnumerator enumerator = control.WizardSteps.GetEnumerator();
            try
            {
                if (enumerator.MoveNext())
                {
                    WizardStep component = (WizardStep) enumerator.Current;
                    host.DestroyComponent(component);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private WizardStepDesigner GetCurrentWizardStepDesigner()
        {
            WizardControl control = Control as WizardControl;
            WizardStep component = null;
            IDesignerHost service = null;
            WizardStepDesigner designer = null;
            if (control != null && control.WizardSteps.Count >= 0)
            {
                component = control.WizardSteps[control.CurrentStepIndex];
                service = (IDesignerHost) GetService(typeof (IDesignerHost));
                designer = null;
            }
            if (service == null)
            {
                return designer;
            }
            if (component == null)
            {
                return designer;
            }
            designer = (WizardStepDesigner) service.GetDesigner(component);
            return designer;
        }

        protected override bool GetHitTest(Point point)
        {
            WizardControl control;
            if (!selected)
            {
                return false;
            }
            control = (WizardControl) Control;
            if (control.WizardSteps.Count <= 0)
            {
                return false;
            }
            return HitTestBack(control, point);
        }

        private static bool HitTestBack(WizardControl control, Point point)
        {
            if (!control.BackButton.Visible)
            {
                return HitTestNext(control, point);
            }
            Point pt = control.BackButton.PointToClient(point);
            Rectangle rect = control.BackButton.ClientRectangle;
            if (!rect.Contains(pt))
            {
                return HitTestNext(control, point);
            }
            return true;
        }

        private static bool HitTestNext(WizardControl control, Point point)
        {
            if (!control.NextButton.Visible)
            {
                return HitTestCancel(control, point);
            }
            Point pt = control.NextButton.PointToClient(point);
            Rectangle rect = control.NextButton.ClientRectangle;
            if (!rect.Contains(pt))
            {
                return HitTestCancel(control, point);
            }
            return true;
        }

        private static bool HitTestCancel(WizardControl control, Point point)
        {
            if (!control.CancelButton.Visible)
            {
                return HitTestHelp(control, point);
            }
            Point pt = control.CancelButton.PointToClient(point);
            Rectangle rect = control.CancelButton.ClientRectangle;
            if (!rect.Contains(pt))
            {
                return HitTestHelp(control, point);
            }
            return true;
        }

        private static bool HitTestHelp(WizardControl control, Point point)
        {
            if (!control.HelpButton.Visible)
            {
                return false;
            }
            Point pt = control.HelpButton.PointToClient(point);
            Rectangle rect = control.HelpButton.ClientRectangle;
            if (!rect.Contains(pt))
            {
                return false;
            }
            return true;
        }

        private WizardStepDesigner GetWizardStepDesigner(WizardStep step)
        {
            IDesignerHost service = (IDesignerHost) GetService(typeof (IDesignerHost));
            WizardStepDesigner designer = null;
            if (service == null)
            {
                return designer;
            }
            designer = (WizardStepDesigner) service.GetDesigner(step);
            return designer;
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            AutoResizeHandles = true;
            ISelectionService service = (ISelectionService) GetService(typeof (ISelectionService));
            if (service != null)
            {
                service.SelectionChanged += new EventHandler(OnSelectionChanged);
            }
            WizardControl control = (WizardControl) Control;
            wizardDesignerActionList = new WizardDesignerActionList(control);
            actionListCollection.Add(wizardDesignerActionList);
            control.CurrentStepIndexChanged += new EventHandler(RefreshComponent);
            control.WizardSteps.StepAdded += new EventHandler(RefreshComponent);
        }

        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            WizardControl control = Control as WizardControl;
            if (control == null)
            {
                return;
            }
            IDesignerHost service = (IDesignerHost) GetService(typeof (IDesignerHost));
            if (service == null)
            {
                return;
            }
            StartStep step = (StartStep) service.CreateComponent(typeof (StartStep));
            control.WizardSteps.Add(step);
            IntermediateStep step2 = (IntermediateStep) service.CreateComponent(typeof (IntermediateStep));
            control.WizardSteps.Add(step2);
            FinishStep step3 = (FinishStep) service.CreateComponent(typeof (FinishStep));
            control.WizardSteps.Add(step3);
        }

        protected override void OnDragComplete(DragEventArgs de)
        {
            if (forwardOnDrag)
            {
                forwardOnDrag = false;
                GetCurrentWizardStepDesigner().OnDragCompleteInternal(de);
            }
            else
            {
                base.OnDragComplete(de);
            }
        }

        protected override void OnDragDrop(DragEventArgs de)
        {
            if (forwardOnDrag)
            {
                forwardOnDrag = false;
                WizardStepDesigner currentWizardStepDesigner = GetCurrentWizardStepDesigner();
                if (currentWizardStepDesigner != null)
                {
                    currentWizardStepDesigner.OnDragDropInternal(de);
                }
            }
            else
            {
                de.Effect = DragDropEffects.None;
            }
        }

        protected override void OnDragEnter(DragEventArgs de)
        {
            WizardControl control = (WizardControl) Control;
            if (control.WizardSteps.Count <= 0)
            {
                base.OnDragEnter(de);
                return;
            }
            WizardStep step = control.WizardSteps[control.CurrentStepIndex];
            Point pt = step.PointToClient(new Point(de.X, de.Y));
            Rectangle clientRectangle = step.ClientRectangle;
            if (!clientRectangle.Contains(pt))
            {
                base.OnDragEnter(de);
                return;
            }
            GetWizardStepDesigner(step).OnDragEnterInternal(de);
            forwardOnDrag = true;
        }

        protected override void OnDragLeave(EventArgs e)
        {
            if (forwardOnDrag)
            {
                forwardOnDrag = false;
                WizardStepDesigner currentWizardStepDesigner = GetCurrentWizardStepDesigner();
                if (currentWizardStepDesigner == null)
                {
                    return;
                }
                currentWizardStepDesigner.OnDragLeaveInternal(e);
                return;
            }
            base.OnDragLeave(e);
        }

        protected override void OnDragOver(DragEventArgs de)
        {
            WizardControl control = Control as WizardControl;
            if (control == null || control.WizardSteps.Count <= 0)
            {
                de.Effect = DragDropEffects.None;
                return;
            }
            WizardStep step = control.WizardSteps[control.CurrentStepIndex];
            Point pt = step.PointToClient(new Point(de.X, de.Y));
            WizardStepDesigner wizardStepDesigner = GetWizardStepDesigner(step);
            Rectangle clientRectangle = step.ClientRectangle;
            if (!clientRectangle.Contains(pt))
            {
                if (!forwardOnDrag)
                {
                    de.Effect = DragDropEffects.None;
                    return;
                }
                forwardOnDrag = false;
                wizardStepDesigner.OnDragLeaveInternal(EventArgs.Empty);
                base.OnDragEnter(de);
                return;
            }
            else
            {
                if (!forwardOnDrag)
                {
                    base.OnDragLeave(EventArgs.Empty);
                    wizardStepDesigner.OnDragEnterInternal(de);
                    forwardOnDrag = true;
                    return;
                }
                wizardStepDesigner.OnDragOverInternal(de);
                return;
            }
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            if (forwardOnDrag)
            {
                WizardStepDesigner currentWizardStepDesigner = GetCurrentWizardStepDesigner();
                if (currentWizardStepDesigner == null)
                {
                    return;
                }
                currentWizardStepDesigner.OnGiveFeedbackInternal(e);
            }
            else
            {
                base.OnGiveFeedback(e);
            }
        }

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            WizardControl control = (WizardControl) Control;
            if (control == null)
            {
                return;
            }
            if (control.WizardSteps.Count != 0)
            {
                return;
            }
            Pen pen = new Pen(SystemColors.ControlDark);
            try
            {
                pen.DashStyle = DashStyle.Dash;
                Rectangle rect = control.Bounds;
                rect.Location = new Point(0, 0);
                rect.Width--;
                rect.Height--;
                pe.Graphics.DrawRectangle(pen, rect);
                return;
            }
            finally
            {
                pen.Dispose();
            }
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            ISelectionService service = (ISelectionService) GetService(typeof (ISelectionService));
            if (service == null)
            {
                return;
            }
            selected = false;
            ICollection selectedComponents = service.GetSelectedComponents();
            if (selectedComponents == null)
            {
                return;
            }
            WizardControl control = (WizardControl) Control;
            IEnumerator enumerator = selectedComponents.GetEnumerator();
            if (enumerator == null)
            {
                return;
            }
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (current == control)
                    {
                        selected = true;
                        break;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null) disposable.Dispose();
            }
        }

        private void RefreshComponent(object sender, EventArgs e)
        {
            DesignerActionUIService service = GetService(typeof (DesignerActionUIService)) as DesignerActionUIService;
            if (service == null)
            {
                return;
            }
            service.Refresh(Control);
        }

        public override DesignerActionListCollection ActionLists
        {
            get { return actionListCollection; }
        }

        public override ICollection AssociatedComponents
        {
            get { return ((WizardControl) Control).WizardSteps; }
        }

        protected Panel ButtonsPanel
        {
            get { return (Panel) Control.Controls["ButtonsPanel"]; }
        }

        protected Panel WizardStepsPanel
        {
            get { return (Panel) Control.Controls["WizardStepsPanel"]; }
        }
    }
}