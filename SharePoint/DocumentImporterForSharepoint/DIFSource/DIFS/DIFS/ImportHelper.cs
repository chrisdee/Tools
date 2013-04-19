using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

namespace DIFS
{
    class ImportHelper
    {
        // For authentication to office 365
        public static MsOnlineClaimsHelper helper = null;


        // Main function call to populate a Tree View from SharePoint
        public static TreeNode SiteAndListTreeLoader(string strURL, AuthenticationSettings authenticationsettings, BackgroundWorker worker)
        {
            worker.ReportProgress(0, MethodBase.GetCurrentMethod().Name.ToString() + ":" + strURL);
            TreeNode treenode = new TreeNode(strURL);           
            try
            {
                // Get that the URL is valid
                if ((!strURL.ToLower().StartsWith("http://")) && (!strURL.ToLower().StartsWith("https://"))) { throw new Exception("The specified URL is not valid.  It should start with http: or https://"); } 
                ClientContext clientcontext = GetContext(strURL, authenticationsettings);
                Web web = clientcontext.Web;
                clientcontext.Load(web);
                clientcontext.ExecuteQuery();
                PopulateTreeView(web, treenode, clientcontext, worker);
            }
            catch (Exception ex)
            {
                worker.ReportProgress(0, MethodBase.GetCurrentMethod().Name.ToString() + ":Exception:" + ex.Message);

            }
            return treenode;
        }

        // Process an individual web and add it and it's children into the passed tree view node
        private static void PopulateTreeView(Web web, TreeNode currentnode, ClientContext clientcontext, BackgroundWorker worker)
        {

            worker.ReportProgress(0, MethodBase.GetCurrentMethod().Name.ToString() + ":" + web.Title);
            WebCollection webs = web.GetSubwebsForCurrentUser(null);
            clientcontext.Load(webs);
            clientcontext.ExecuteQuery();


            // Go through every sub web of the passed web
            foreach (Web subweb in webs)
            {
                clientcontext.Load(subweb);
                clientcontext.ExecuteQuery();

                // Create a node for the current web
                TreeNode subnode = new TreeNode();
                
                // Set the node title (That displays to the user)
                subnode.Text = subweb.Title.ToString();


                // Iterate any further sub webs and lists
                WebCollection furthersubwebs = subweb.Webs;
                ListCollection furtherlists = subweb.Lists;
                clientcontext.Load(furthersubwebs);
                clientcontext.Load(furtherlists);
                clientcontext.ExecuteQuery();

                // If there are any further subwebs then add them
                foreach (Web furthersubweb in furthersubwebs)
                {
                    PopulateTreeView(furthersubweb, subnode, clientcontext, worker);
                }

                // If there are any lists then add then
                foreach (List list in furtherlists)
                {
                    clientcontext.Load(list);
                    clientcontext.ExecuteQuery();
                    if (list.BaseType == BaseType.DocumentLibrary) 
                    { 
                        // At this point we are adding a list which is a document library and it is a valid destination
                        // we will therefore supply a bit more detail about the item in a TAG
                        TreeNode librarynode = new TreeNode();
                        librarynode.Text = list.Title;
                        Folder folder = list.RootFolder;
                        clientcontext.Load(folder);
                        clientcontext.ExecuteQuery();
                        ImportDestination importdestination = new ImportDestination();
                        importdestination.DestinationFolderUrl = folder.ServerRelativeUrl;
                        importdestination.DestinationWebUrl = subweb.ServerRelativeUrl;
                        importdestination.DestinationServerUrl = Get_ServerURL_From_URL(clientcontext.Url);
                        importdestination.DestinationLibraryName = list.Title;
                        librarynode.Tag = importdestination;
                        subnode.Nodes.Add(librarynode); 
                    }
                }

                clientcontext.ExecuteQuery();

                // Add the completed node to the tree view control
                currentnode.Nodes.Add(subnode);


            }

            // Go through every list of the passed web
            ListCollection lists = web.Lists;
            clientcontext.Load(lists);
            clientcontext.ExecuteQuery();
            foreach (List list in web.Lists)
            {
                clientcontext.Load(list);
                clientcontext.ExecuteQuery();
                // Now check to see if the list is a document library
                if (list.BaseType == BaseType.DocumentLibrary)
                {
                    // At this point we are adding a list which is a document library and it is a valid destination
                    // we will therefore supply a bit more detail about the item in a TAG
                    TreeNode librarynode = new TreeNode();
                    librarynode.Text = list.Title;
                    Folder folder = list.RootFolder;
                    clientcontext.Load(folder);
                    clientcontext.ExecuteQuery();
                    ImportDestination importdestination = new ImportDestination();
                    importdestination.DestinationFolderUrl = folder.ServerRelativeUrl;
                    importdestination.DestinationWebUrl = web.ServerRelativeUrl;
                    importdestination.DestinationServerUrl = Get_ServerURL_From_URL(clientcontext.Url);
                    importdestination.DestinationLibraryName = list.Title;
                    librarynode.Tag = importdestination;
                    currentnode.Nodes.Add(librarynode);
                }

            }

            // Now if after all this we have a web which has no document libraries or sub webs then we don't need it
            if (currentnode.Nodes.Count == 0) { currentnode.Remove(); }

        }

        // Get the Server Address from a URL
        private static string Get_ServerURL_From_URL(string strURL)
        {
            // We need to get just http://intranet or https://intranet from http://intranet/Blog

            if (strURL.IndexOf("/", 9) < 0 )
            {
                // We are already at the top level
            }
            else
            {
                // We need to get the top level.
                strURL = strURL.Substring(0, strURL.IndexOf("/", 9));
            }
            return strURL;
        }

        // Save a settings file
        public static void SaveSettings(string strFileNameAndPath, ImportSettings importsettings)
        {
            XmlSerializer xmlserializer = new XmlSerializer(typeof(ImportSettings));
            StreamWriter streamwriter = new StreamWriter(strFileNameAndPath);
            xmlserializer.Serialize(streamwriter, importsettings);
            streamwriter.Close();
        }

        // Load a settings file
        public static ImportSettings LoadSettings(string strFileNameAndPath)
        {
            ImportSettings importsettings = new ImportSettings();

            XmlSerializer xmlserializer = new XmlSerializer(typeof(ImportSettings));
            FileStream filestream = new FileStream(strFileNameAndPath, FileMode.Open);

            importsettings = (ImportSettings)xmlserializer.Deserialize(filestream);
            filestream.Close();

            return importsettings;
        }


        // Get a context from the passed authentication settings
        public static ClientContext GetContext(string strURL, AuthenticationSettings authenticationsettings)
        {
            ClientContext clientcontext = new ClientContext(strURL);

            /* Using authentication settings supplied by settings object */

            if (authenticationsettings.AuthenticationType == AuthenticationSettings.AuthenticationTypes.Current)
            {
                clientcontext.Credentials = CredentialCache.DefaultCredentials; 
            }
            if (authenticationsettings.AuthenticationType == AuthenticationSettings.AuthenticationTypes.Specified)
            {
                clientcontext.Credentials = new NetworkCredential(authenticationsettings.username, authenticationsettings.password, authenticationsettings.domain);
            }
            if (authenticationsettings.AuthenticationType == AuthenticationSettings.AuthenticationTypes.Forms)
            {
                clientcontext.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
                clientcontext.FormsAuthenticationLoginInfo = new FormsAuthenticationLoginInfo(authenticationsettings.username, authenticationsettings.password);
            }
            if (authenticationsettings.AuthenticationType == AuthenticationSettings.AuthenticationTypes.Office365)
            {
                // To get the authentication for Office 365 the site url needs to end with a /
                string strHelperSite;
                if (strURL.EndsWith("/"))
                {
                    strHelperSite = strURL;
                }
                else
                {
                    strHelperSite = strURL+"/";
                }

                helper = new MsOnlineClaimsHelper(
                authenticationsettings.username,
                authenticationsettings.password,
                strHelperSite);

                clientcontext.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest);

            }
             
            return clientcontext;


        }

        private static void ctx_ExecutingWebRequest(object sender, WebRequestEventArgs e)
        {
            
            e.WebRequestExecutor.WebRequest.CookieContainer = helper.CookieContainer;
            e.WebRequestExecutor.WebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)"; 
        }


        // Get the fields from the SharePoint list for the UI
        public static Control[] RefreshSharePointFields(string strURL, AuthenticationSettings authenticationsettings, string strDocumentLibrary)
        {


            FieldCollection fieldcollection = null;
            // Get that the URL is valid
            if ((!strURL.ToLower().StartsWith("http://")) && (!strURL.ToLower().StartsWith("https://"))) { throw new Exception("The specified URL is not valid.  It should start with http: or https://"); }
            ClientContext clientcontext = GetContext(strURL, authenticationsettings);
            Web web = clientcontext.Web;
            clientcontext.Load(web);
            ListCollection lists = web.Lists;
            clientcontext.Load(lists);
            clientcontext.ExecuteQuery();

            foreach (List list in lists)
            {
                fieldcollection = list.Fields;
                clientcontext.Load(list);
                clientcontext.Load(fieldcollection);
                clientcontext.ExecuteQuery(); 
 
 

                if (list.Title == strDocumentLibrary)
                {
                    break;
                }
            }



            Control[] controls = new Control[fieldcollection.Count];
            //foreach (Field field in fieldcollection)
            for (int i = 0; i < fieldcollection.Count ; i++)
            {
                Label label = new System.Windows.Forms.Label();
                label.Text = fieldcollection[i].Title;
                controls[i] = label;
            }
            return controls;


        }
/*        public static FieldCollection RefreshSharePointFields(string strURL, AuthenticationSettings authenticationsettings, string strDocumentLibrary)
        {

            FieldCollection fieldcollection = null;

            try
            {
                // Get that the URL is valid
                if ((!strURL.ToLower().StartsWith("http://")) && (!strURL.ToLower().StartsWith("https://"))) { throw new Exception("The specified URL is not valid.  It should start with http: or https://"); }
                ClientContext clientcontext = GetContext(strURL, authenticationsettings);
                Web web = clientcontext.Web;
                clientcontext.Load(web);
                ListCollection lists = web.Lists;
                clientcontext.Load(lists);
                clientcontext.ExecuteQuery();

                foreach (List list in lists)
                {
                    clientcontext.Load(list);
                    fieldcollection = list.Fields;
                    
                    clientcontext.ExecuteQuery();

                    if (list.Title == strDocumentLibrary)
                    {
                        return fieldcollection;
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return fieldcollection;
        }
*/

    }
}
