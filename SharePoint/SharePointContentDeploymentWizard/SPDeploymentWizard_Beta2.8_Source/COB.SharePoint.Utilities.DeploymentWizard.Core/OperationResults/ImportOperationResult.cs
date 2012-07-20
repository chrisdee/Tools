using System;
using System.Collections.Generic;
using System.Text;

namespace COB.SharePoint.Utilities.DeploymentWizard.Core
{
    public class ImportOperationResult : DeploymentOperationResult
    {
        internal ImportOperationResult(ResultType Result) : base(Result)
        {
        }

        internal ImportOperationResult(ResultType Result, string Message) : base (Result, Message)
        {
        }
    }
}
