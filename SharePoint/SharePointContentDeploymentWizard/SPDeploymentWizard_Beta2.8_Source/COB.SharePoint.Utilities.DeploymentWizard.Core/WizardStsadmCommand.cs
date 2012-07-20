using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Xml;

using Microsoft.SharePoint.Deployment;
using Microsoft.SharePoint.StsAdmin;

using COB.SharePoint.Utilities.DeploymentWizard.Core.Helpers;

namespace COB.SharePoint.Utilities.DeploymentWizard.Core
{
    /// <summary>
    /// This STSADM command provides a scripting interface onto Content Deployment Wizard functionality.
    /// </summary>
    public class WizardStsadmCommand : ISPStsadmCommand
    {
        #region -- Private members --

        private TraceSwitch traceSwitch = new TraceSwitch("COB.SharePoint.Utilities.DeploymentWizard.Core",
                                                          "Trace switch for the core Wizard deployment engine.");

        private TraceHelper trace = null;

        private readonly string f_csSETTINGS_FILE_PARAM = "settingsFile";
        private readonly string f_csQUIET_PARAM = "quiet";
        private const string f_csIMPORT_COMMAND = "RunWizardImport";
        private const string f_csEXPORT_COMMAND = "RunWizardExport";
        private bool f_quiet = false;

        #endregion

        #region -- Constructor --

        public WizardStsadmCommand()
        {
            trace = new TraceHelper(this);
        }

        #endregion

        #region -- Core --

        public int Run(string command, StringDictionary keyValues, out string output)
        {
            if (traceSwitch.TraceVerbose)
            {
                trace.TraceVerbose("Run(): Entered.");
            }

#if DEBUG
            // comment back in to debug..
            //Console.WriteLine("Attach debugger and/or press enter to continue..");
            //Console.ReadLine();
#endif

            int iReturn = 0;
            string sResult = string.Empty;
            DeploymentType deploymentType;
            WizardDeployment wizardDeployment = null;

            // determine command type..
            switch (command)
            {
                case f_csIMPORT_COMMAND:
                    deploymentType = DeploymentType.Import;
                    break;
                case f_csEXPORT_COMMAND:
                    deploymentType = DeploymentType.Export;
                    break;
                default:
                    throw new ConfigurationErrorsException("Error - unexpected command! Supported commands are 'RunWizardImport' and 'RunWizardExport'.");
                    break;
            }

            // validate passed settings..
            string sValidationMessage = validateSettings(keyValues, deploymentType);

            if (string.IsNullOrEmpty(sValidationMessage))
            {
                string sSettingsFilePath = keyValues[f_csSETTINGS_FILE_PARAM];
                if (keyValues[f_csQUIET_PARAM] != null)
                {
                    f_quiet = true;
                }
                
                using (XmlTextReader xReader = new XmlTextReader(sSettingsFilePath))
                {
                    wizardDeployment = new WizardDeployment(xReader, deploymentType);
                 
                    // ask deployment API to validate settings..
                    try
                    {
                        wizardDeployment.ValidateSettings();
                        if (traceSwitch.TraceInfo)
                        {
                            trace.TraceInfo("Run(): Settings validated successfully.");
                        }
                    }
                    catch (Exception e)
                    {
                        if (traceSwitch.TraceWarning)
                        {
                            trace.TraceWarning("Run(): Failed to validate deployment settings! Deployment will not be done, showing error message.");
                        }

                        sResult = string.Format("Error - unable to validate the deployment settings you chose. Please ensure, for example, you are not exporting a web " +
                            "and specific child objects in the same operation. Message = '{0}'.", e.Message);
                    }

                    if (string.IsNullOrEmpty(sResult))
                    {
                        // now run job..
                        wizardDeployment.ProgressUpdated += new EventHandler<SPDeploymentEventArgs>(wizardDeployment_ProgressUpdated);

                        if (deploymentType == DeploymentType.Export)
                        {
                            wizardDeployment.ValidChangeTokenNotFound += new EventHandler<InvalidChangeTokenEventArgs>(wizardDeployment_ValidChangeTokenNotFound);
                            sResult = runExportTask(wizardDeployment);
                        }
                        else if (deploymentType == DeploymentType.Import)
                        {
                            sResult = runImportTask(wizardDeployment);
                        }    
                    }
                }
            }
            else
            {
                sResult = sValidationMessage;
            }

            if (traceSwitch.TraceVerbose)
            {
                trace.TraceVerbose("Run(): Returning '{0}'.", iReturn);
            }

            output = sResult;
            return iReturn;
        }

        private string runImportTask(WizardDeployment wizardDeployment)
        {
            if (traceSwitch.TraceVerbose)
            {
                trace.TraceVerbose("runImportTask: Entered.");
            }

            string sResult = string.Empty;
            ImportOperationResult importResult = null;
            Console.WriteLine("Running import..");
            
            try
            {
                importResult = wizardDeployment.RunImport();
            }
            catch (Exception e)
            {
                if (traceSwitch.TraceError)
                {
                    trace.TraceError("runImportTask: Exception caught whilst running import: '{0}'.", e);
                }

                sResult = string.Format("An unexpected exception occurred whilst preparing and running the import. " +
                    "\n\nException details: \n\n{0}\n\n", importResult.Message);
            }

            if (importResult.Outcome == ResultType.Success)
            {
                sResult = string.Format("Completed import of file '{0}'. Please see log file at '{1}' to check results.",
                    wizardDeployment.ImportSettings.BaseFileName, wizardDeployment.ImportSettings.LogFilePath);
            }
            else if (importResult.Outcome == ResultType.Failure)
            {
                sResult = string.Format("An error occurred whilst running the import. " +
                    "\n\nException details: \n\n{0}", importResult.Message);
            }

            if (traceSwitch.TraceVerbose)
            {
                trace.TraceVerbose("runImportTask: Returning '{0}'.", sResult);
            }

            return sResult;
        }

        void wizardDeployment_ProgressUpdated(object sender, SPDeploymentEventArgs e)
        {
            if (e.ObjectsTotal == -1)
            {
                // this condition indicates we are importing..
                if (!f_quiet)
                {
                    Console.WriteLine(string.Format("Imported {0} objects. Continuing..", e.ObjectsProcessed));
                }
            }
            else
            {
                if (!f_quiet)
                {
                    Console.WriteLine(string.Format("Exported {0} objects from a total of {1}. Continuing..",
                                                    e.ObjectsProcessed, e.ObjectsTotal));
                }
            }
        }

        void wizardDeployment_ValidChangeTokenNotFound(object sender, InvalidChangeTokenEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.EventMessage))
            {
                Console.WriteLine(string.Format("WARNING: {0}", e.EventMessage));
            }
        }

        private string runExportTask(WizardDeployment wizardDeployment)
        {
            if (traceSwitch.TraceVerbose)
            {
                trace.TraceVerbose("runExportTask: Entered.");
            }

            ExportOperationResult exportResult;
            string sResult = string.Empty;
            Console.WriteLine("Running export..");
            
            try
            {
                exportResult = wizardDeployment.RunExport();

                if (traceSwitch.TraceInfo)
                {
                    string sMessage = (wizardDeployment.ExportSettings.ExportMethod == SPExportMethodType.ExportChanges) ?
                        string.Format("Incremental export completed successfully, change token is '{0}'.", exportResult.ChangeToken) :
                        "Full export completed successfully";
                    trace.TraceInfo("runExportTask: {0}.", sMessage);
                }

                sResult = string.Format("Completed export of file '{0}'. Please see log file at '{1}' to check results.",
                    wizardDeployment.ExportSettings.BaseFileName, wizardDeployment.ExportSettings.LogFilePath);
            }
            catch (Exception e)
            {
                if (traceSwitch.TraceError)
                {
                    trace.TraceError("runExportTask: Exception caught whilst running export: '{0}'.", e);
                }

                sResult = ExceptionHelper.HandleDeploymentExportException(e, false);
            }

            if (traceSwitch.TraceVerbose)
            {
                trace.TraceVerbose("runExportTask: Returning '{0}'.", sResult);
            }

            return sResult;
        }

        #endregion

        #region -- Validation --

        private string validateSettings(StringDictionary keyValues, DeploymentType deploymentType)
        {
            string sMessage = null;

            string sSettingsFilePath = keyValues[f_csSETTINGS_FILE_PARAM];
            if (string.IsNullOrEmpty(sSettingsFilePath))
            {
                sMessage = "Error - no settings file was specifed! You must specify the path to an XML settings file " +
                           "in the 'settingsFile' parameter. This file should be generated by saving import or export " +
                           "settings in the Content Deployment Wizard";
            }

            if (!File.Exists(sSettingsFilePath))
            {
                sMessage = string.Format("Error - unable to find settings file at path '{0}'!", sSettingsFilePath);
            }

            using (XmlTextReader xReader = new XmlTextReader(sSettingsFilePath))
            {
                WizardOperationSettings settings = WizardDeployment.CollectSettings(xReader);
                if (settings is WizardExportSettings && deploymentType == DeploymentType.Import)
                {
                    sMessage = string.Format("Error - settings file '{0}' contains export settings but you selected the RunWizardImport " +
                        "command!", sSettingsFilePath);
                }

                if (settings is WizardImportSettings && deploymentType == DeploymentType.Export)
                {
                    sMessage = string.Format("Error - settings file '{0}' contains import settings but you selected the RunWizardExport " +
                        "command!", sSettingsFilePath);
                } 
            }
            
            return sMessage;
        }

        #endregion

        #region -- GetHelpMessage() --

        public string GetHelpMessage(string command)
        {
            string sHelpMsg = string.Empty;
            string sIndent = "        ";
            
            //string sExeName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
            //sHelpMsg = string.Format("{0} -o {1}\n", sExeName, command);

            switch (command)
            {
                case f_csIMPORT_COMMAND:
                    sHelpMsg +=
                        string.Format(
                            "-settingsFile <path to settings XML file containing import settings. This file should be created " +
                            "by saving settings from the Wizard UI>");
                    break;
                case f_csEXPORT_COMMAND:
                    sHelpMsg += string.Format("-settingsFile <path to settings XML file containing export settings. This file should be created " +
                        "by saving settings from the Wizard UI>");
                    break;
                default:
                    throw new ConfigurationErrorsException("Error - unexpected command! Supported commands are 'RunWizardImport' and 'RunWizardExport'.");
                    break;
            }

            sHelpMsg += string.Format("{0}{1}[-quiet ] <specifies whether to suppress verbose import/export messages>", Environment.NewLine, sIndent); 

            return sHelpMsg;
        }

        #endregion
    }
}