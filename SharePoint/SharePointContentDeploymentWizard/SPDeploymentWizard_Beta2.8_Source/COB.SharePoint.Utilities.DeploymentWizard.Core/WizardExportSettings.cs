using System.Collections.Generic;
using Microsoft.SharePoint.Deployment;

namespace COB.SharePoint.Utilities.DeploymentWizard.Core
{
    public class WizardExportSettings : WizardOperationSettings
    {
        public WizardExportSettings(SPExportSettings ExportSettings, List<SPObjectData> MappedObjects)
        {
            Settings = ExportSettings;
            SupplementaryData = MappedObjects;
        }

        public List<SPObjectData> SupplementaryData
        {
            get; 
            set;
        }
    }
}
