using System;
using System.Collections.Generic;
using System.Text;

namespace COB.SharePoint.Utilities.DeploymentWizard.Core
{
    public class ExportOperationResult : DeploymentOperationResult
    {
        public string f_changeToken = string.Empty;

        public string ChangeToken
        {
            get
            {
                return f_changeToken;
            }
        }

        internal ExportOperationResult(ResultType Result) :
            base(Result)
        {
        }

        internal ExportOperationResult(ResultType Result, string ChangeToken) :
            base(Result)
        {
            f_changeToken = ChangeToken;
        }

        internal ExportOperationResult(ResultType Result, string Information, string ChangeToken) :
            base(Result, Information)
        {
            f_changeToken = ChangeToken;
        }
    }
}
