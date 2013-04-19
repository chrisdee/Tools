using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DIFS
{
    [Serializable]
    public class ImportSettings
    {
        // The main import settings
        public bool LoggingToFile { get; set; }
        public string SourceFolder { get; set; }
        public ImportDestination Destination = new ImportDestination();

        // The Names of fields in the dataset
        public string fieldSourceFileNameAndPath = "FileNameAndPath";
        public string fieldDestinationWebUrl = "DestinationWebUrl";
        public string fieldDestinationFolderUrl = "DestinationFolderUrl";
        public string fieldDestinationServerUrl = "DestinationServerUrl";
        public string fieldDestinationFileName = "DestinationFileName";
        public string fieldDestinationSubDirectories = "DestinationSubDirectories";
        public string fieldException = "Exception";
        public string fieldFileSystemCreated = "FileSystemCreated";
        public string fieldFileSystemModified = "FileSystemModified";
        public string fieldFileSystemCreatedBy = "FileSystemCreatedBy";
        public string fieldFileSystemModifiedBy = "FileSystemModifiedBy";

        // The type of import
        public enum ImportTypes { FileSystemFolder, XMLDataSet, CSVFile };
        public ImportTypes ImportType = ImportTypes.FileSystemFolder;

        // The authentication settings
        public AuthenticationSettings authenticationsettings = new AuthenticationSettings();

        // Settings relating to the type of import
        public string SourceFile;

        // Mappings
        public ImportMapping[] ImportMappings;

        
    }

    public class ImportDestination
    {
        public string DestinationWebUrl { get; set; }
        public string DestinationFolderUrl { get; set; }
        public string DestinationServerUrl { get; set; }
        public string DestinationLibraryName { get; set; }
    }

    public class ImportMapping
    {
        public string InternalName { get; set; }
        public string DataColumn { get; set; }
        public enum FormatTypes { None, ADUserToSPUser };
        public FormatTypes FormatType = FormatTypes.None;
        public string FormatMask;           
    }

    

}
