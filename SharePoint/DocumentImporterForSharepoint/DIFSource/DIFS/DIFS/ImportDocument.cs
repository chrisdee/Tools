using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.SharePoint.Client;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Net;


namespace DIFS
{
    class ImportDocument
    {
        public ImportSettings importsettings;
        public DataTable importdatatable = new DataTable("importdatatable");
        public ImportProgress importprogress = new ImportProgress();

        private TextWriterTraceListener LoggingToFileTraceListener;
      
        // Constructor
        public ImportDocument(ImportSettings suppliedsettings)
        {
            // Settings - Initialise the settings
            importsettings = suppliedsettings;

            // Logging - Initialise the trace logs if required
            if (importsettings.LoggingToFile)
            {
                Trace.Listeners.Clear();
                string strLogFileName = Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location) + "." + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
                LoggingToFileTraceListener = new TextWriterTraceListener(strLogFileName);
                Trace.Listeners.Add(LoggingToFileTraceListener);
                Trace.AutoFlush = true;
            }
        }

        // Load the items that are to be imported into a Datatable
        public void LoadItems(BackgroundWorker worker)
        {
            // Call the appropriate function based upon the type of import
            if (importsettings.ImportType == ImportSettings.ImportTypes.FileSystemFolder)
            {
                LoadItemsFileSystemFolder(worker);
            }

            // Call the appropriate function based upon the type of import
            if (importsettings.ImportType == ImportSettings.ImportTypes.XMLDataSet)
            {
                LoadItemsXMLDataSet(worker);
            }

            // Call the appropriate function based upon the type of import
            if (importsettings.ImportType == ImportSettings.ImportTypes.CSVFile)
            {
                LoadItemsCSVFile(worker);
            }

        }

        // Iterate the file system folder to build the data table
        private void LoadItemsFileSystemFolder(BackgroundWorker worker)
        {
            // Tell the user what is happening
            importprogress.ImportActivity = MethodBase.GetCurrentMethod().Name.ToString();
            importprogress.ImportActivityLevel = 0;
            importprogress.ImportStatus = ImportProgress.Status.Loading;
            importprogress.ImportStatusChange = true;
            ImportProgress currentprogress = new ImportProgress(importprogress);
            ReportProgress(worker, currentprogress);

            // Ensure that the current data set contains the fields that we need.
            EnsureDataSet();

            // Iterate the file system
            LoadFileSystemDirectoryRecursive(worker, importsettings.SourceFolder);

        }

        // Ensure that the current data set contains the fields that we need to have to service the import
        private void EnsureDataSet()
        {
            // Add the columns that we need to the data table - We are using a data table since this will make it easier in future to 
            // drive the import from database sources
            DataColumn dcSourceFileNameAndPath = new DataColumn(importsettings.fieldSourceFileNameAndPath, typeof(System.String));
            importdatatable.Columns.Add(dcSourceFileNameAndPath);
            DataColumn dcDestinationServerUrl = new DataColumn(importsettings.fieldDestinationServerUrl, typeof(System.String));
            importdatatable.Columns.Add(dcDestinationServerUrl);
            DataColumn dcDestinationFolderUrl = new DataColumn(importsettings.fieldDestinationFolderUrl, typeof(System.String));
            importdatatable.Columns.Add(dcDestinationFolderUrl);
            DataColumn dcDestinationWebUrl = new DataColumn(importsettings.fieldDestinationWebUrl, typeof(System.String));
            importdatatable.Columns.Add(dcDestinationWebUrl);
            DataColumn dcDestinationSubDirectories = new DataColumn(importsettings.fieldDestinationSubDirectories, typeof(System.String));
            importdatatable.Columns.Add(dcDestinationSubDirectories);
            DataColumn dcDestinationFileName = new DataColumn(importsettings.fieldDestinationFileName, typeof(System.String));
            importdatatable.Columns.Add(dcDestinationFileName);
            DataColumn dcException = new DataColumn(importsettings.fieldException, typeof(System.String));
            importdatatable.Columns.Add(dcException);
            DataColumn dcFileSystemCreated = new DataColumn(importsettings.fieldFileSystemCreated, typeof(System.DateTime));
            importdatatable.Columns.Add(dcFileSystemCreated);
            DataColumn dcFileSystemModified = new DataColumn(importsettings.fieldFileSystemModified, typeof(System.DateTime));
            importdatatable.Columns.Add(dcFileSystemModified);
            DataColumn dcFileSystemCreatedBy = new DataColumn(importsettings.fieldFileSystemCreatedBy, typeof(System.Int32));
            importdatatable.Columns.Add(dcFileSystemCreatedBy);
            DataColumn dcFileSystemModifiedBy = new DataColumn(importsettings.fieldFileSystemModifiedBy, typeof(System.Int32));
            importdatatable.Columns.Add(dcFileSystemModifiedBy);

        }

        // Load an XML Data set into the in memory data table
        private void LoadItemsXMLDataSet(BackgroundWorker worker)
        {
            // Tell the user what is happening
            importprogress.ImportActivity = MethodBase.GetCurrentMethod().Name.ToString() + ":Loading:" + importsettings.SourceFile;
            importprogress.ImportActivityLevel = 0;
            importprogress.ImportStatus = ImportProgress.Status.Loading;
            importprogress.ImportStatusChange = true;
            ImportProgress currentprogress = new ImportProgress(importprogress);
            ReportProgress(worker, currentprogress);

            // Deserialize the XML into the current data table
            XmlSerializer xmlserializer = new XmlSerializer(typeof(DataTable));
            FileStream filestream = new FileStream(importsettings.SourceFile, FileMode.Open);
            importdatatable = (DataTable)xmlserializer.Deserialize(filestream); 
            filestream.Close();

            // Tell the user what has happened
            importprogress.ImportActivity = MethodBase.GetCurrentMethod().Name.ToString() + ":Loaded:" + importsettings.SourceFile;
            importprogress.ImportActivityLevel = 1;
            importprogress.ImportItemsCount = importdatatable.Rows.Count;
            importprogress.ImportStatusChange = false;
            currentprogress = new ImportProgress(importprogress);
            ReportProgress(worker, currentprogress);

        }

        // Load a CSV file into the in memory data table
        private void LoadItemsCSVFile(BackgroundWorker worker)
        {
            // Tell the user what is happening
            importprogress.ImportActivity = MethodBase.GetCurrentMethod().Name.ToString() + ":Loading:" + importsettings.SourceFile;
            importprogress.ImportActivityLevel = 0;
            importprogress.ImportStatus = ImportProgress.Status.Loading;
            importprogress.ImportStatusChange = true;
            ImportProgress currentprogress = new ImportProgress(importprogress);
            ReportProgress(worker, currentprogress);

            // Ensure that the current data set contains the fields that we need.
            EnsureDataSet();

            // Load the CSV file into the current data set
            string strLine;
            string[] strArray;
            string[] csvfields;
            char[] charArray = new char[] { ',' };
            FileStream filestream = new FileStream(importsettings.SourceFile, FileMode.Open);
            StreamReader streamreader = new StreamReader(filestream);

            // Read the first line from the file
            strLine = streamreader.ReadLine();


            // Split the header
            csvfields = strLine.Split(charArray);

            // Add any columns to the data table as required
            foreach (string strcolumnname in csvfields)
            {
                // If there is not already a data table entry for the column name then add it
                if (!importdatatable.Columns.Contains(strcolumnname))
                {
                    importdatatable.Columns.Add(strcolumnname);
                }
            }
            

            // Read the first data line
            strLine = streamreader.ReadLine();

            // Loop through the CSV file until the end
            while (strLine != null)
            {
                strArray = strLine.Split(charArray);
                DataRow importdatarow = importdatatable.NewRow();

                // Load any meta data
                for (int i = 0; i <= strArray.GetUpperBound(0); i++)
                {
                    // Use the field name from the header to tell what column should be populated.
                    importdatarow[csvfields[i]] = strArray[i].Trim();
                }

                // Get the details of the file
                FileInfo fileinfo = new FileInfo(importdatarow[importsettings.fieldSourceFileNameAndPath].ToString());

                // We must record the destination details
                importdatarow[importsettings.fieldDestinationFolderUrl] = importsettings.Destination.DestinationFolderUrl;
                importdatarow[importsettings.fieldDestinationServerUrl] = importsettings.Destination.DestinationServerUrl;
                importdatarow[importsettings.fieldDestinationWebUrl] = importsettings.Destination.DestinationWebUrl;
                //importdatarow[importsettings.fieldDestinationSubDirectories] = Get_FolderURL_FromDirectory(strDirectory);
                importdatarow[importsettings.fieldDestinationFileName] = fileinfo.Name;

                importdatatable.Rows.Add(importdatarow);
                strLine = streamreader.ReadLine();
            }
            streamreader.Close();
            filestream.Close();

            // Tell the user what has happened
            importprogress.ImportActivity = MethodBase.GetCurrentMethod().Name.ToString() + ":Loaded:" + importsettings.SourceFile;
            importprogress.ImportActivityLevel = 1;
            importprogress.ImportItemsCount = importdatatable.Rows.Count;
            importprogress.ImportStatusChange = false;
            currentprogress = new ImportProgress(importprogress);
            ReportProgress(worker, currentprogress);

        }

        // Iterate through subdirectories
        public void LoadFileSystemDirectoryRecursive(BackgroundWorker worker, string strDirectory)
        {
            foreach (string strFileName in Directory.GetFiles(strDirectory))
            {
                // We have a new data row for every file to import
                DataRow importdatarow = importdatatable.NewRow();

                // Get the details of the file
                FileInfo fileinfo = new FileInfo(strFileName);
                importdatarow[importsettings.fieldSourceFileNameAndPath] = fileinfo.FullName;

                // We must record the destination details
                importdatarow[importsettings.fieldDestinationFolderUrl] = importsettings.Destination.DestinationFolderUrl;
                importdatarow[importsettings.fieldDestinationServerUrl] = importsettings.Destination.DestinationServerUrl;
                importdatarow[importsettings.fieldDestinationWebUrl] = importsettings.Destination.DestinationWebUrl;
                importdatarow[importsettings.fieldDestinationSubDirectories] = Get_FolderURL_FromDirectory(strDirectory);
                importdatarow[importsettings.fieldDestinationFileName] = fileinfo.Name;

                // Add the entry to the data set - the data set will be iterated later during the import
                importdatatable.Rows.Add(importdatarow);
            }

            // Tell the user what is happening
            importprogress.ImportActivity = MethodBase.GetCurrentMethod().Name.ToString() + ":" + strDirectory;
            importprogress.ImportActivityLevel = 1;
            importprogress.ImportItemsCount = importdatatable.Rows.Count;
            importprogress.ImportStatusChange = false;
            ImportProgress currentprogress = new ImportProgress(importprogress);
            ReportProgress(worker, currentprogress);

            foreach  (string strSubDirectory in Directory.GetDirectories(strDirectory))
            {
                LoadFileSystemDirectoryRecursive(worker, strSubDirectory);                
            }

        }

        // Import the items into SharePoint
        public void ImportItems(BackgroundWorker worker)
        {
            // Tell the user what is happening
            importprogress.ImportActivity = MethodBase.GetCurrentMethod().Name.ToString();
            importprogress.ImportActivityLevel = 0;
            importprogress.ImportStatus = ImportProgress.Status.Importing;
            importprogress.ImportStatusChange = true;
            ImportProgress currentprogress = new ImportProgress(importprogress);
            ReportProgress(worker, currentprogress);

            for (int i = importprogress.ImportLastItemProcessed; i < importdatatable.Rows.Count; i++)
            {
                ImportRow(importdatatable.Rows[i], worker);
                if (worker.CancellationPending)
                {
                    importprogress.ImportActivity = MethodBase.GetCurrentMethod().Name.ToString() + "Import Paused";
                    importprogress.ImportActivityLevel = 0;
                    importprogress.ImportStatus = ImportProgress.Status.Paused;
                    importprogress.ImportStatusChange = true;
                    ImportProgress currentrowprogress = new ImportProgress(importprogress);
                    ReportProgress(worker, currentrowprogress);
                    break;
                }
            }

            // Once the for loop has been completed the import of the items is complete
            // we can therefore update the import progress
            if (!worker.CancellationPending)
            {
                importprogress.ImportActivity = MethodBase.GetCurrentMethod().Name.ToString() + "Completed";
                importprogress.ImportActivityLevel = 0;
                importprogress.ImportStatus = ImportProgress.Status.Completed;
                importprogress.ImportStatusChange = true;
                currentprogress = importprogress;
                ReportProgress(worker, currentprogress);
            }


        }

        // Cancel an import
        public void Cancel()
        {
            // Clear the properties
            importdatatable = new DataTable();
            importprogress = new ImportProgress();
            importsettings = new ImportSettings();
        }

        // Save exceptions
        public void SaveExceptions()
        {
            DataTable exceptionsdatatable = importdatatable.Clone();
            
            foreach (DataRow exceptionrow in importdatatable.Select("Exception is not NULL"))
            {
                exceptionsdatatable.ImportRow(exceptionrow);
            }
            XmlSerializer xmlserializer = new XmlSerializer(typeof(DataTable));
            TextWriter textwriter = new StreamWriter(System.Reflection.Assembly.GetEntryAssembly().Location.ToString() + ".Exceptions."+DateTime.Now.ToString("yyyyMMddHHmmss")+".xml");
            xmlserializer.Serialize(textwriter, exceptionsdatatable);
            textwriter.Close();

            exceptionsdatatable.Dispose();

        }

        // Import a row into the destination - Add the file and then set the meta data
        private void ImportRow(DataRow datarow, BackgroundWorker worker)
        {
            try
            {
                // Get the details for the current file
                string strContextURL = datarow[importsettings.fieldDestinationServerUrl].ToString() + datarow[importsettings.fieldDestinationWebUrl].ToString();
                string strDestination = datarow[importsettings.fieldDestinationFolderUrl].ToString() + datarow[importsettings.fieldDestinationSubDirectories].ToString() + "/"+datarow[importsettings.fieldDestinationFileName].ToString() ;
                string strRootFolder = datarow[importsettings.fieldDestinationFolderUrl].ToString();
                string strSource = datarow[importsettings.fieldSourceFileNameAndPath].ToString();
                string strSubDirectories = datarow[importsettings.fieldDestinationSubDirectories].ToString();

                // Get the file info object and store the meta data
                FileInfo fileinfo = new FileInfo(strSource);


                // Create a context
                var context = ImportHelper.GetContext(strContextURL, importsettings.authenticationsettings);
                   
                    
                // A folder may be needed.
                ProcessSubDirectories(context, strRootFolder, strSubDirectories);

                // Process the file
                ProcessFile(fileinfo, datarow);

                // First we need to add the file to SharePoint
                AddFile(context, fileinfo, strDestination);

                // Now we need to map any required meta data
                AddMetaData(context, datarow, strDestination);

                // Notch up another complete item
                importprogress.ImportItemsSucceeded = importprogress.ImportItemsSucceeded + 1;

                // Clear exception (in case there was a previous exception)
                datarow[importsettings.fieldException] = null;

                // Let the user know what we did
                importprogress.ImportActivity = MethodBase.GetCurrentMethod().Name.ToString() + ":Imported:" + strSource + ":To:" + strDestination;
                importprogress.ImportActivityLevel = 2;
                importprogress.ImportItemsProcessed = importprogress.ImportItemsProcessed + 1;
                importprogress.ImportStatusChange = false;

                float floatRatio = (float)importprogress.ImportItemsProcessed / (float)importprogress.ImportItemsCount;
                importprogress.ImportProgressPercentage = (int)(floatRatio * 100);
                ImportProgress currentprogress = new ImportProgress(importprogress);
                ReportProgress(worker, currentprogress);

            }

            catch (Exception ex)
            {
                importprogress.ImportItemsFailed = importprogress.ImportItemsFailed + 1;
                importprogress.ImportActivity = MethodBase.GetCurrentMethod().Name.ToString() + ":" + datarow[importsettings.fieldSourceFileNameAndPath].ToString() + ":Exception"+ex.Message;
                importprogress.ImportActivityLevel = 1;
                importprogress.ImportItemsProcessed = importprogress.ImportItemsProcessed + 1;
                importprogress.ImportStatusChange = false;
                float floatRatio = (float)importprogress.ImportItemsProcessed / (float)importprogress.ImportItemsCount;
                importprogress.ImportProgressPercentage = (int)(floatRatio * 100);
                ImportProgress currentprogress = new ImportProgress(importprogress);
                ReportProgress(worker, currentprogress);
                datarow[importsettings.fieldException] = ex.Message;

            }
        }

        // This function will process a subdirectory structure passed as a string
        private void ProcessSubDirectories(ClientContext context, string strRootFolder, string strSubDirectories)
        {
            // The subdirectories will be passed as a string like Folder1/Folder2
            // so the first challenge is to split these into the individual folders

            char[] charSeparators = new char[] {'/'};

            string[] strarrSubDirectories = strSubDirectories.Split(charSeparators,StringSplitOptions.RemoveEmptyEntries);

            // We will create the first level of directory in the root folder for the library
            string strCurrentRootFolder = strRootFolder;
            foreach (string strSubDirectory in strarrSubDirectories)
            {
                // Now we will have just "folder1" so we keep track of where we are in the structure and progress through in order
                AddFolder(context, strCurrentRootFolder, strSubDirectory);

                // We will increment the current root folder in case there are multiple levels
                strCurrentRootFolder = strCurrentRootFolder + "/" + strSubDirectory;


            }

        }

        // This function will process the file info object
        private void ProcessFile(FileInfo fileinfo, DataRow datarow)
        {
            datarow[importsettings.fieldFileSystemCreated] = fileinfo.CreationTime;
            datarow[importsettings.fieldFileSystemModified] = fileinfo.LastWriteTime;
        }

        // This function will add a folder to the given location if it does not already exist
        private void AddFolder(ClientContext context, string strRootFolder, string strFolder)
        {

            var rootFolder = context.Web.GetFolderByServerRelativeUrl(strRootFolder);
            context.Load(rootFolder, f => f.Folders, f => f.Name);
            context.ExecuteQuery();
            var parentFolder = rootFolder.Folders.ToList();

            if (!parentFolder.Any(f => f.Name.Equals(strFolder)))
            {
                string strNewFolder = strRootFolder + "/" + strFolder;
                rootFolder.Folders.Add(strNewFolder);
            }
            rootFolder.Update();
            context.ExecuteQuery();
        }

        // Add (Upload a file)
        private void AddFile(ClientContext context, FileInfo fileinfo, string strDestination)
        {

            // So we need to add the file to SharePoint

            // If we are using normal SharePoint then the Office 365 Helper will be null and we can use savebinarydirect
            if (!(importsettings.authenticationsettings.AuthenticationType == AuthenticationSettings.AuthenticationTypes.Office365))
            {
                Microsoft.SharePoint.Client.File.SaveBinaryDirect(context, strDestination, fileinfo.OpenRead(), true);
            }
            // Otherwise we are using Office 365 which for some reason does not support save binary direct
            else
            {
                /*
                "url" is the full destination path (including filename, i.e. mysite.sharepoint.com/.../Test.txt)  
                "cookie" is the CookieContainer generated from Wichtor's code  
                "data" is the byte array containing the files contents (used a FileStream to load) 
                */
                FileStream filestream = fileinfo.OpenRead();
                byte[] data = new byte[filestream.Length];
                filestream.Read(data, 0, data.Length);

                System.Net.ServicePointManager.Expect100Continue = false;
                HttpWebRequest request = HttpWebRequest.Create(context.Url + strDestination) as HttpWebRequest;
                request.Method = "PUT";
                request.Accept = "*/*";
                request.ContentType = "multipart/form-data; charset=utf-8";
                request.CookieContainer = ImportHelper.helper.CookieContainer;
                request.AllowAutoRedirect = false;
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
                request.Headers.Add("Accept-Language", "en-us");
                request.Headers.Add("Translate", "F");
                request.Headers.Add("Cache-Control", "no-cache");
                request.ContentLength = data.Length;
                using (Stream req = request.GetRequestStream())
                {
                    req.Write(data, 0, data.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream res = response.GetResponseStream();
                StreamReader rdr = new StreamReader(res);
                string rawResponse = rdr.ReadToEnd(); 

            }

            
        }

        // Meta data (Update the meta data on an uploaded file)
        private void AddMetaData(ClientContext context, DataRow datarow, String strDestination)
        {
            // Get a list item for the file we just added
            Microsoft.SharePoint.Client.File file  = context.Web.GetFileByServerRelativeUrl(strDestination);
            file.CheckOut();
            ListItem listitem = file.ListItemAllFields;
            context.Load(listitem);

            // Iterate the mappings in the import settings and apply
            foreach (ImportMapping importmapping in importsettings.ImportMappings)
            {
                try
                {
                    // If the import mapping does not require any formatting then just apply directly.
                    if (importmapping.FormatType == ImportMapping.FormatTypes.None)
                    {
                        listitem[importmapping.InternalName] = datarow[importmapping.DataColumn];
                    }
                }
                catch
                {
                   // Do nothing, just keep trying any other mappings
                }
            }
            
            listitem.Update();
            file.CheckIn(string.Empty, CheckinType.OverwriteCheckIn);
            context.ExecuteQuery();
        }

        // Report progress
        private void ReportProgress(BackgroundWorker worker, ImportProgress currentprogress)
        {
            // Always report progress
            worker.ReportProgress(0, currentprogress);

            // Log progress if required
            if (importsettings.LoggingToFile)
            {
                Trace.WriteLine(currentprogress.ImportActivity);
            }
        }

        // Get the ServerRelativeUrl that SharePoint requires from the absolute URL that our users input
        private string Get_ServerRelativeUrl_FromAbsUrl(string strAbsURL)
        {
            // An absolute URL will be of the format http://this/that or https://this/that
            strAbsURL = strAbsURL.ToLower();
            strAbsURL = strAbsURL.Replace("http://","");
            strAbsURL = strAbsURL.Replace("https://", "");
            strAbsURL = strAbsURL.Substring(strAbsURL.IndexOf("/"));
            return strAbsURL;
        }

        // Convert a file system directory name into a sharepoint destination
        private string Get_FolderURL_FromDirectory(string strDirectory)
        {
            // The top level folder is not considered.
            strDirectory = strDirectory.ToLower();
            strDirectory = strDirectory.Replace(importsettings.SourceFolder.ToLower(), "");

            // At this point c:\import will become "" and c:\import\sub will become "\sub"
            strDirectory = strDirectory.Replace("\\","/");

            // Now we will have /sub
            return strDirectory;
        }

    }

    public class ImportProgress
    {
        public int ImportItemsCount = 0 ;
        public int ImportProgressPercentage = 0;
        public int ImportItemsProcessed = 0;
        public int ImportItemsFailed = 0;
        public int ImportItemsSucceeded = 0;
        public int ImportLastItemProcessed = 0;
        public string ImportActivity = "";
        public int ImportActivityLevel = 0;
        public enum Status { New, Loading, Importing, Paused, Completed };
        public Status ImportStatus = Status.New;
        public bool ImportStatusChange = false;
        
        // Constructor
        public ImportProgress()
        {

        }

        public ImportProgress(ImportProgress suppliedprogress)
        {
            ImportItemsCount = suppliedprogress.ImportItemsCount;
            ImportProgressPercentage = suppliedprogress.ImportProgressPercentage;
            ImportItemsProcessed = suppliedprogress.ImportItemsProcessed;
            ImportItemsFailed = suppliedprogress.ImportItemsFailed;
            ImportItemsSucceeded = suppliedprogress.ImportItemsSucceeded;
            ImportLastItemProcessed = suppliedprogress.ImportLastItemProcessed;
            ImportActivity = suppliedprogress.ImportActivity;
            ImportActivityLevel = suppliedprogress.ImportActivityLevel;
            ImportStatus = suppliedprogress.ImportStatus;
            ImportStatusChange = suppliedprogress.ImportStatusChange;
        }

    }
}
