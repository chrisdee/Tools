using System;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Deployment;

namespace COB.SharePoint.Utilities.DeploymentWizard.Core
{
    public class SPObjectData : ICloneable
    {
        private SPDeploymentObjectType objectType;
        private SPIncludeDescendants includeDescendents;
        private Guid id = Guid.Empty;
        private Guid fileId = Guid.Empty;
        private string url = null;
        private string title = null;
        private SPWeb parentWeb = null;
        private bool excludeChildren;

        public SPWeb Web
        {
            get { return parentWeb; }
            set { parentWeb = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public Guid ID
        {
            get { return id; }
            set { id = value; }
        }

        public Guid FileID
        {
            get { return fileId; }
            set { fileId = value; }
        }

        public SPDeploymentObjectType ObjectType
        {
            get { return objectType; }
            set { objectType = value; }
        }
        
        public SPIncludeDescendants IncludeDescendents
        {
            get { return includeDescendents; }
            set { includeDescendents = value; }
        }

        public bool ExcludeChildren
        {
            get { return excludeChildren; }
            set { excludeChildren = value; }
        }

        public SPObjectData()
        {
        }

        public SPObjectData(Guid ID, string Title, string Url, SPDeploymentObjectType DeploymentObjectType,
            SPIncludeDescendants IncludeDescendents)
            : this(ID, Guid.Empty, Title, Url, DeploymentObjectType, IncludeDescendents, null)
        {
        }

        public SPObjectData(Guid ID, string Title, string Url, SPDeploymentObjectType DeploymentObjectType,
            SPIncludeDescendants IncludeDescendents, SPWeb Web)
            : this(ID, Guid.Empty, Title, Url, DeploymentObjectType, IncludeDescendents, Web)
        {
        }

        public SPObjectData(Guid ID, Guid FileID, string Title, string Url, SPDeploymentObjectType DeploymentObjectType, 
            SPIncludeDescendants IncludeDescendents, SPWeb Web)
        {
            id = ID;
            fileId = FileID;
            title = Title;
            url = Url;
            objectType = DeploymentObjectType;
            includeDescendents = IncludeDescendents;
            parentWeb = Web;
        }

        public override string ToString()
        {
            StringBuilder sbData = new StringBuilder();
            sbData.AppendLine(string.Format("ID = '{0}'", id));
            sbData.AppendLine(string.Format("FileID = '{0}'", fileId));
            sbData.AppendLine(string.Format("Title = '{0}'", title));
            sbData.AppendLine(string.Format("Object type = '{0}'", objectType));
            sbData.AppendLine(string.Format("Include descendents = '{0}'", includeDescendents));
            sbData.AppendLine(string.Format("Exclude children = '{0}'", excludeChildren));
            sbData.AppendLine(string.Format("Parent web reference = '{0}'", 
                (parentWeb == null) ? "<null>" : parentWeb.ID.ToString()));
            return sbData.ToString();
        }

        #region ICloneable Members

        /// <summary>
        /// Creates a shallow copy of the current instance. This is fine since all members are 
        /// either value types or behave like value types.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


    }   
}
