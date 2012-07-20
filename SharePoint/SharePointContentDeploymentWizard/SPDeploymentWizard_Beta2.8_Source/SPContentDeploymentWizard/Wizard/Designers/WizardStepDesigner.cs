using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WizardBase
{
    internal class WizardStepDesigner : ParentControlDesigner
    {
        public WizardStepDesigner()
        {
            AutoResizeHandles = true;
        }

        public override bool CanBeParentedTo(IDesigner parentDesigner)
        {
            if (parentDesigner == null)
            {
                return false;
            }
            return (parentDesigner.Component is WizardControl);
        }

        internal void OnDragCompleteInternal(DragEventArgs de)
        {
            OnDragComplete(de);
        }

        internal void OnDragDropInternal(DragEventArgs de)
        {
            OnDragDrop(de);
        }

        internal void OnDragEnterInternal(DragEventArgs de)
        {
            OnDragEnter(de);
        }

        internal void OnDragLeaveInternal(EventArgs e)
        {
            OnDragLeave(e);
        }

        internal void OnDragOverInternal(DragEventArgs e)
        {
            OnDragOver(e);
        }

        internal void OnGiveFeedbackInternal(GiveFeedbackEventArgs e)
        {
            OnGiveFeedback(e);
        }

        public override SelectionRules SelectionRules
        {
            get { return (base.SelectionRules & ~(SelectionRules.Moveable | SelectionRules.AllSizeable)); }
        }
    }
}