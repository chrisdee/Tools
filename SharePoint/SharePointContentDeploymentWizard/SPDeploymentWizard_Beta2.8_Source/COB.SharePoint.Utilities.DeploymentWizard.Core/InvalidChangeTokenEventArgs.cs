using System;
using System.Collections.Generic;
using System.Text;

namespace COB.SharePoint.Utilities.DeploymentWizard.Core
{
    public class InvalidChangeTokenEventArgs : EventArgs
    {
        public string EventMessage
        {
            get; set;
        }
    }
}
