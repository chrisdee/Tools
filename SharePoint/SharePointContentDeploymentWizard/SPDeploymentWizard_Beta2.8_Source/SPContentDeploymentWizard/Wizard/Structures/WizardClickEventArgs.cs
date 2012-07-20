using System;

namespace WizardBase
{
    public class WizardClickEventArgs : EventArgs
    {
        private bool cancel;

        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
    }
}