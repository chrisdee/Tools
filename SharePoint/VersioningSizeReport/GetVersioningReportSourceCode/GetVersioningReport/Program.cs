using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.IO;

namespace SharePointReporting
{
    class GetVersioningReport
    {
        static void Main(string[] args)
        {
            string site;
            StreamWriter SW;
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Enter the Web Application URL:");
                    site = Console.ReadLine();
                }
                else
                {
                    site = args[0];
                }

                SPSite tmpRoot = new SPSite(site);
                SPSiteCollection tmpRootColl = tmpRoot.WebApplication.Sites;

                //objects for the CSV file generation
                SW = File.AppendText("c:\\VersioningReport.csv");

                //Write the CSV Header
                SW.WriteLine("Site Name, Library, File Name, File URL, Last Modified, No. of Versions, Latest Version Size -KB,Total Versions Size - MB");

                //Enumerate through each site collection
                foreach (SPSite tmpSite in tmpRootColl)
                {
                    //Enumerate through each sub-site
                    foreach (SPWeb tmpWeb in tmpSite.AllWebs)
                    {
                        //Enumerate through each List
                        foreach (SPList tmpList in tmpWeb.Lists)
                        {
                            //Get only Document Libraries & Exclude specific libraries
                            if (tmpList.BaseType == SPBaseType.DocumentLibrary &  tmpList.Title!="Workflows" & tmpList.Title!= "Master Page Gallery" & tmpList.Title!="Style Library" & tmpList.Title!="Pages")
                            {
                                  foreach (SPListItem tmpSPListItem in tmpList.Items)
                                    {

                                        if (tmpSPListItem.Versions.Count > 5)
                                        {
                                            
                                            SPListItemVersionCollection tmpVerisionCollection = tmpSPListItem.Versions;

                                            //Get the versioning details
                                            foreach (SPListItemVersion tmpVersion in tmpVerisionCollection)
                                             {
                                                int versionID = tmpVersion.VersionId;
                                                string strVersionLabel = tmpVersion.VersionLabel;
                                             }

                                            //Get the versioning Size details
                                            double versionSize = 0;
                                            SPFile tmpFile = tmpWeb.GetFile(tmpWeb.Url + "/" + tmpSPListItem.File.Url);

                                            foreach (SPFileVersion tmpSPFileVersion in tmpFile.Versions)
                                            {
                                                versionSize = versionSize + tmpSPFileVersion.Size;
                                            }
                                            //Convert to MB
                                            versionSize= Math.Round(((versionSize/1024)/1024),2);

                                            string siteName;
                                            if (tmpWeb.IsRootWeb)
                                            {
                                                siteName= tmpWeb.Title +" - Root";
                                            }
                                            else
                                            {
                                                siteName=tmpSite.RootWeb.Title + " - " + tmpWeb.Title;
                                            }

                                            //Log the data to a CSV file where versioning size > 0MB!
                                            if (versionSize > 0)
                                            {
                                                SW.WriteLine(siteName + "," + tmpList.Title + "," + tmpSPListItem.Name + "," + tmpWeb.Url + "/" + tmpSPListItem.Url + "," + tmpSPListItem["Modified"].ToString() + "," + tmpSPListItem.Versions.Count + "," + (tmpSPListItem.File.Length / 1024) + "," + versionSize );
                                            }
                                        }
                                }
                            }
                        }
                     }

                }

                //Close the CSV file object 
                SW.Close();

                //Dispose of the Root Site Object
                tmpRoot.Dispose();
                
                //Just to pause
                Console.WriteLine(@"Versioning Report Generated Successfull at c:\VersioningReport.csv. Press ""Enter"" key to Exit");
                Console.ReadLine();
            }

            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Get Versioning Report", ex.Message);
            }
        }
    }
}
