using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Deployment;

namespace COB.SharePoint.Utilities.DeploymentWizard.Core
{
    public class IncrementalManager
    {
        private static readonly Guid tokensGuid = new Guid("f941e880-9f97-4722-bd1f-7e6ed9791d6f");
        private static readonly string persistedObjectName = "SPDeploymentWizard.SavedChangeTokens";
        private static string largestExportObjectID = string.Empty;
        
        public static void SaveToken(string ChangeToken)
        {
            ChangeTokenStore tokenStore = (ChangeTokenStore)SPFarm.Local.GetObject(tokensGuid);
            if (tokenStore == null)
            {
                tokenStore = new ChangeTokenStore(persistedObjectName, SPFarm.Local, tokensGuid);
            }

            if (!tokenStore.Items.Contains(ChangeToken))
            {
                tokenStore.Items.Add(ChangeToken);
                tokenStore.Update();
            }
        }

        public static List<string> ChangeTokens
        {
            get
            {
                ChangeTokenStore tokenStore = (ChangeTokenStore)SPFarm.Local.GetObject(tokensGuid);
                if (tokenStore == null)
                {
                    tokenStore = new ChangeTokenStore(persistedObjectName, SPFarm.Local, tokensGuid);
                }

                return tokenStore.Items;
            }
        }

        public static string GetLastToken(SPExportSettings ExportSettings)
        {
            // find the most recent token for the scope of the current export operation.. 
            string changeToken = string.Empty;

            if (ChangeTokens.Count > 0)
            {
                storeLargestScopedObject(ExportSettings);

                changeToken = ChangeTokens.FindLast(containsScopeID);
            }

            return changeToken;
        }

        public static string GetLastToken()
        {
            return ((ChangeTokens != null) && (ChangeTokens.Count > 0))
                       ? ChangeTokens[ChangeTokens.Count - 1]
                       :
                           string.Empty;
        }

        public static string GetLastToken(List<SPObjectData> ExportObjects)
        {
            // find the most recent token for the scope of the current export operation.. 
            string changeToken = string.Empty;

            if (ChangeTokens.Count > 0)
            {
                storeLargestScopedObject(ExportObjects);

                changeToken = ChangeTokens.FindLast(containsScopeID);
            }

            return changeToken;
        }

        public static void DeleteAllTokens()
        {
            ChangeTokenStore tokenStore = (ChangeTokenStore)SPFarm.Local.GetObject(tokensGuid);
            if (tokenStore == null)
            {
                tokenStore = new ChangeTokenStore(persistedObjectName, SPFarm.Local, tokensGuid);
            }

            tokenStore.Items.Clear();
            tokenStore.Update(true);
        }

        private static bool containsScopeID(string s)
        {
            return s.Contains(largestExportObjectID);
        }

        private static void storeLargestScopedObject(SPExportSettings ExportSettings)
        {
            List<SPObjectData> objects = new List<SPObjectData>();

            foreach (SPExportObject exportObject in ExportSettings.ExportObjects)
            {
                objects.Add(new SPObjectData(exportObject.Id, exportObject.Url, exportObject.Url, exportObject.Type, 
                    exportObject.IncludeDescendants));
            }

            storeLargestScopedObject(objects);
        }

        private static void storeLargestScopedObject(List<SPObjectData> exportObjects)
        {
            SPObjectData largestScopeObject = null;

            // would be nice to optimize this implementation if time..
            List<SPObjectData> listObjects = new List<SPObjectData>();
            List<SPObjectData> webObjects = new List<SPObjectData>();
            List<SPObjectData> siteObjects = new List<SPObjectData>();

            foreach (SPObjectData exportObject in exportObjects)
            {
                switch (exportObject.ObjectType)
                {
                    case SPDeploymentObjectType.List:
                        listObjects.Add(exportObject);
                        break;
                    case SPDeploymentObjectType.Web:
                        webObjects.Add(exportObject);
                        break;
                    case SPDeploymentObjectType.Site:
                        siteObjects.Add(exportObject);
                        break;
                    default:
                        break;
                }
            }

            if (siteObjects.Count > 0)
            {
                largestScopeObject = siteObjects[0];
            }
            if (largestScopeObject == null && webObjects.Count > 0)
            {
                largestScopeObject = webObjects[0];
            }
            if (largestScopeObject == null && listObjects.Count > 0)
            {
                largestScopeObject = listObjects[0];
            }

            largestExportObjectID = (largestScopeObject != null) ? largestScopeObject.ID.ToString() : string.Empty;
        }
    }
}
