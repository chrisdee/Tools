using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Administration;

namespace COB.SharePoint.Utilities.DeploymentWizard.Core
{
    public class ChangeTokenStore : SPPersistedObject
    {
        public ChangeTokenStore()
        {
        }

        public ChangeTokenStore(string name, SPPersistedObject parent, Guid id) : base(name, parent, id)
        {
        }

        [Persisted()] public List<string> Items = new List<string>();
        
    }
}
