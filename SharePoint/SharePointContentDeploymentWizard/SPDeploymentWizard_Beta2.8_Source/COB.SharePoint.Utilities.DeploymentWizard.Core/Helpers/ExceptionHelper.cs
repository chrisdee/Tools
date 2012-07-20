using System;

namespace COB.SharePoint.Utilities.DeploymentWizard.Core.Helpers
{
    public class ExceptionHelper
    {
        /// <summary>
        /// Contains logic for handling export errors - this is centralized since all clients should implement 
        /// this handling.
        /// </summary>
        /// <remarks>
        /// This method is used to special case the scenario where the user has attempted to export to a network 
        /// location - unfortunately SharePoint doesn't give us a specific exception type here.
        /// </remarks>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string HandleDeploymentExportException(Exception e, bool LogFileWillOpen)
        {
            string sMessage = string.Empty;

            if (e is ArgumentException && e.Message == "Illegal characters in path.")
            {
                sMessage = string.Format("An error occurred whilst running the export - this could be because you " +
                    "are attempting to export to a network location rather than a local drive, or the folder " +
                    "path is too long. Please amend and retry. \n\nException details: \n\n{0}", e);
            }
            else
            {
                string sLogFilePhrase = (LogFileWillOpen) ? "The export log file will now be opened." : string.Empty;
                sMessage = string.Format("An error occurred whilst running the export. {0}\n\nException details: \n\n{1}", 
                    sLogFilePhrase, e);
            }

            return sMessage;
        }
    }
}