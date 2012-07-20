using System;
using System.Collections.Generic;
using System.Text;

namespace COB.SharePoint.Utilities.DeploymentWizard.Core
{
    public enum ResultType
    {
        Success,
        Failure
    }

    public class DeploymentOperationResult
    {
        public ResultType Outcome
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string ChangeToken
        {
            get;
            set;
        }

        internal DeploymentOperationResult(ResultType Result) : this(Result, string.Empty) 
        {
        }

        internal DeploymentOperationResult(ResultType Result, string Information)
        {
            Outcome = Result;
            Message = Information;
        }

    }
}
