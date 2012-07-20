using Microsoft.SharePoint.Deployment;

namespace COB.SharePoint.Utilities.DeploymentWizard.Core
{
    public class WizardImportSettings : WizardOperationSettings
    {
        public WizardImportSettings(SPImportSettings ImportSettings)
        {
            Settings = ImportSettings;
        }
    }
}
