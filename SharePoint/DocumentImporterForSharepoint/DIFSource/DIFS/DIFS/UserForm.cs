using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Net;


namespace DIFS
{
    public partial class UserForm : Form
    {

        ImportDocument importdocument;

        public UserForm()
        {
            InitializeComponent();
            InitializeBackgroundWorkers();
            textBoxVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            

        }

        private void InitializeBackgroundWorkers()
        {
            // Back ground worker for loading the destination TreeView
            bgwDestination.DoWork += new DoWorkEventHandler(bgwDestination_DoWork);
            bgwDestination.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwDestination_RunWorkerCompleted);
            bgwDestination.ProgressChanged += new ProgressChangedEventHandler(bgwDestination_ProgressChanged);
            bgwDestination.WorkerReportsProgress = true;

            // Back ground worker for import
            bgwImport.DoWork += new DoWorkEventHandler(bgwImport_DoWork);
            bgwImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwImport_RunWorkerCompleted);
            bgwImport.ProgressChanged += new ProgressChangedEventHandler(bgwImport_ProgressChanged);
            bgwImport.WorkerSupportsCancellation = true;
            bgwImport.WorkerReportsProgress = true;

        }

        // DoWork event
        private void bgwDestination_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            // Get the current authentication details
            AuthenticationSettings authenticationsettings = AuthenticationSettingsFromUI();



            // Call the helper
            e.Result = ImportHelper.SiteAndListTreeLoader(textBoxSharePointSite.Text, authenticationsettings, worker);
            
        }

        // DoWork event
        private void bgwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            // Get the import control
            ImportControl importcontrol = e.Argument as ImportControl;

            // Call the appropriate import method
            if (importcontrol.Resume)
            {
                importdocument.ImportItems(worker);
            }
            else
            {
                importdocument.LoadItems(worker);
                importdocument.ImportItems(worker);
            }

        }

        // RunWorkerCompleted Event
        private void bgwDestination_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tvLists.Nodes.Add(e.Result as TreeNode);
        }

        // RunWorkerCompleted Event
        private void bgwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        // ProgressChanged Event
        private void bgwDestination_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LogToUser(e.UserState.ToString());
        }

        // ProgressChanged Event
        private void bgwImport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // We will have been passed an object by the progress update.
            // this will be an ImportProgress object
            ImportProgress importprogress = e.UserState as ImportProgress;

            // If the status has changed we should update the UI
            if (importprogress.ImportStatusChange)
            {
                SetUIControlStatus(importprogress);
            }


            // We should update the UI with the last reported status
            SetUIProgressStatus(importprogress);



        }

        private void LogToUser(string strMessage)
        {
            this.textBoxStatus.AppendText(strMessage+Environment.NewLine);
            
        }

        private void buttonSourceFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialogSourceFolder.ShowDialog();
            if (!(folderBrowserDialogSourceFolder.SelectedPath == string.Empty))
            {
                textBoxSourceFolder.Text = folderBrowserDialogSourceFolder.SelectedPath;
            }
        }

        private void buttonSharePointSite_Click(object sender, EventArgs e)
        {
            try
            {
                DestinationStartedFromUI();
            }
            catch (Exception ex)
            {
                LogToUser(MethodBase.GetCurrentMethod().Name.ToString() + ":" + "Exception:" + ex.Message);
            }

        }

        private void buttonStartImport_Click(object sender, EventArgs e)
        {
            try
            {
                ImportStartedFromUI();
            }
            catch (Exception ex)
            {
                LogToUser(MethodBase.GetCurrentMethod().Name.ToString() + ":" + "Exception:" + ex.Message);
            }
            
        }

        private void tvLists_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ImportDestination importdestination = tvLists.SelectedNode.Tag as ImportDestination;
            textBoxDestinationFolderURL.Text = importdestination.DestinationFolderUrl;
            textBoxDestinationServerURL.Text = importdestination.DestinationServerUrl;
            textBoxDestinationWebURL.Text = importdestination.DestinationWebUrl;
            textBoxDestinationLibraryName.Text = importdestination.DestinationLibraryName;
        }
        
        protected void DestinationStartedFromUI()
        {


            // Clear the treeview
            tvLists.Nodes.Clear();

            // Repopulate
            bgwDestination.RunWorkerAsync();

        }

        // Start an import from the user interface
        protected void ImportStartedFromUI()
        {

            // Get the current import settings from the UI
            ImportSettings importsettings = ImportSettingsFromUI();

            // Initialise the import object
            importdocument = new ImportDocument(importsettings);

            // Set the import control to start a new import
            ImportControl importcontrol = new ImportControl();
            importcontrol.Resume = false;


            // Start a back ground thread that will import the documents into SharePoint whilst the user interface remains responsive.
            bgwImport.RunWorkerAsync(importcontrol);

            // Enable the pause button.
            buttonPauseImport.Enabled = true;

        }

        // Resume an import from the user interface
        protected void ImportResumedFromUI()
        {
            // Set the import control to resume
            ImportControl importcontrol = new ImportControl();
            importcontrol.Resume = true;

            // Background worked does not really have a concept of resume so we just re run the background worker.
            bgwImport.RunWorkerAsync(importcontrol);

        }

        // Pause an import from the user interface
        protected void ImportPausedFromUI()
        {
            if (bgwImport.IsBusy){bgwImport.CancelAsync();}
        }

        // Cancel an import from the user interface
        protected void ImportCancelledFromUI()
        {

            // Cancel the import
            importdocument.Cancel();

            // Update the UI to reflect the cancelled status
            SetUIProgressStatus(importdocument.importprogress);
            SetUIControlStatus(importdocument.importprogress);
            
        }

        // Cancel an import from the user interface

        // This function will return an Import Settings object based upon the current import settings showing in the UI
        private ImportSettings ImportSettingsFromUI()
        {
            // Create a settings object
            ImportSettings importsettings = new ImportSettings();

            // Get the basic settings
            importsettings.SourceFolder = textBoxSourceFolder.Text;
            importsettings.Destination.DestinationFolderUrl = textBoxDestinationFolderURL.Text;
            importsettings.Destination.DestinationServerUrl = textBoxDestinationServerURL.Text;
            importsettings.Destination.DestinationWebUrl = textBoxDestinationWebURL.Text;
            importsettings.Destination.DestinationLibraryName = textBoxDestinationLibraryName.Text;
            importsettings.LoggingToFile = checkBoxLoggingToFile.Checked;
            
            // Get the import type
            if (radioButtonFileSystemFolder.Checked)
            {
                importsettings.ImportType = ImportSettings.ImportTypes.FileSystemFolder;
            }
            
            if (radioButtonXMLDataSet.Checked)
            {
                importsettings.ImportType = ImportSettings.ImportTypes.XMLDataSet;
                importsettings.SourceFile = textBoxSourceFile.Text;
            }

            if (radioButtonCSVFile.Checked)
            {
                importsettings.ImportType = ImportSettings.ImportTypes.CSVFile;
                importsettings.SourceFile = textBoxSourceFile.Text;
            }

            // Get the authentication details
            importsettings.authenticationsettings = AuthenticationSettingsFromUI();


            // Get the mappings

            importsettings.ImportMappings = new ImportMapping[2];

            // First the file system mappings which have a dedicated UI experience for simplicity
            if (radioButtonCreatedFromFileSystem.Checked)
            {
                ImportMapping importmapping = new ImportMapping();
                importmapping.InternalName = "Created";
                importmapping.DataColumn = importsettings.fieldFileSystemCreated;
                importmapping.FormatType = ImportMapping.FormatTypes.None;
                importsettings.ImportMappings[0] = importmapping;
            }
            if (radioButtonModifiedFromFileSystem.Checked)
            {
                ImportMapping importmapping = new ImportMapping();
                importmapping.InternalName = "Modified";
                importmapping.DataColumn = importsettings.fieldFileSystemModified;
                importmapping.FormatType = ImportMapping.FormatTypes.None;
                importsettings.ImportMappings[1] = importmapping;
            }


            return importsettings;
        }

        private AuthenticationSettings AuthenticationSettingsFromUI()
        {

            // Create a settings object
            AuthenticationSettings authenticationsettings = new AuthenticationSettings();

            // Get the authentication type
            if (radioButtonAuthenticationModeCurrent.Checked)
            {
                authenticationsettings.AuthenticationType = AuthenticationSettings.AuthenticationTypes.Current;
            }
            if (radioButtonAuthenticationModeForms.Checked)
            {
                authenticationsettings.AuthenticationType = AuthenticationSettings.AuthenticationTypes.Forms;
                authenticationsettings.username = textBoxAuthenticationUsername.Text.Trim();
                authenticationsettings.password = textBoxAuthenticationPassword.Text;
            }
            if (radioButtonAuthenticationModeSpecified.Checked)
            {
                authenticationsettings.AuthenticationType = AuthenticationSettings.AuthenticationTypes.Specified;
                authenticationsettings.domain = textBoxAuthenticationDomain.Text.Trim();
                authenticationsettings.username = textBoxAuthenticationUsername.Text.Trim();
                authenticationsettings.password = textBoxAuthenticationPassword.Text;
            }
            if (radioButtonAuthenticationModeOffice365.Checked)
            {
                authenticationsettings.AuthenticationType = AuthenticationSettings.AuthenticationTypes.Office365;
                authenticationsettings.username = textBoxAuthenticationUsername.Text.Trim();
                authenticationsettings.password = textBoxAuthenticationPassword.Text;
            }

            return authenticationsettings;

        }

        private void ImportSettingsToUI(ImportSettings importsettings)
        {
            // Basic import settings
            textBoxSourceFolder.Text = importsettings.SourceFolder;
            textBoxDestinationFolderURL.Text = importsettings.Destination.DestinationFolderUrl;
            textBoxDestinationServerURL.Text = importsettings.Destination.DestinationServerUrl;
            textBoxDestinationWebURL.Text = importsettings.Destination.DestinationWebUrl;
            textBoxDestinationLibraryName.Text = importsettings.Destination.DestinationLibraryName;
            checkBoxLoggingToFile.Checked = importsettings.LoggingToFile;
            
            // Render the import type to the User Interface
            if (importsettings.ImportType == ImportSettings.ImportTypes.FileSystemFolder)
            {
                radioButtonFileSystemFolder.Checked = true;
            }

            if (importsettings.ImportType == ImportSettings.ImportTypes.XMLDataSet)
            {
                radioButtonXMLDataSet.Checked = true;
                textBoxSourceFile.Text = importsettings.SourceFile;
            }

            if (importsettings.ImportType == ImportSettings.ImportTypes.CSVFile)
            {
                radioButtonCSVFile.Checked = true;
                textBoxSourceFile.Text = importsettings.SourceFile;
            }

            // Render the authentication details to the UI
            if (importsettings.authenticationsettings.AuthenticationType == AuthenticationSettings.AuthenticationTypes.Current)
            {
                radioButtonAuthenticationModeCurrent.Checked = true;
                panelAuthenticationCredentials.Enabled = false;
            }
            if (importsettings.authenticationsettings.AuthenticationType == AuthenticationSettings.AuthenticationTypes.Specified)
            {
                radioButtonAuthenticationModeSpecified.Checked = true;
                panelAuthenticationCredentials.Enabled = true;
                textBoxAuthenticationDomain.Enabled = true;
                textBoxAuthenticationDomain.Text = importsettings.authenticationsettings.domain;
                textBoxAuthenticationUsername.Text = importsettings.authenticationsettings.username;
                textBoxAuthenticationPassword.Text = importsettings.authenticationsettings.password;
            }
            if (importsettings.authenticationsettings.AuthenticationType == AuthenticationSettings.AuthenticationTypes.Forms)
            {
                radioButtonAuthenticationModeForms.Checked = true;
                panelAuthenticationCredentials.Enabled = true;
                textBoxAuthenticationDomain.Enabled = false;
                textBoxAuthenticationUsername.Text = importsettings.authenticationsettings.username;
                textBoxAuthenticationPassword.Text = importsettings.authenticationsettings.password;
            }
            if (importsettings.authenticationsettings.AuthenticationType == AuthenticationSettings.AuthenticationTypes.Office365)
            {
                radioButtonAuthenticationModeOffice365.Checked = true;
                panelAuthenticationCredentials.Enabled = true;
                textBoxAuthenticationDomain.Enabled = false;
                textBoxAuthenticationUsername.Text = importsettings.authenticationsettings.username;
                textBoxAuthenticationPassword.Text = importsettings.authenticationsettings.password;
            }
            

            // Render the import mappings
            // First the file date and time which have a custom UI

            ImportMapping importmapping = new ImportMapping();
            importmapping.InternalName = "Created";
            importmapping.DataColumn = importsettings.fieldFileSystemCreated;
            importmapping.FormatType = ImportMapping.FormatTypes.None;

            if (importsettings.ImportMappings.Contains(importmapping))
            {
                radioButtonCreatedFromFileSystem.Checked = true;
            }

            importmapping.InternalName = "Modified";
            importmapping.DataColumn = importsettings.fieldFileSystemModified;
            importmapping.FormatType = ImportMapping.FormatTypes.None;

            if (importsettings.ImportMappings.Contains(importmapping))
            {
                radioButtonModifiedFromFileSystem.Checked = true;
            }

           
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                // We can save the settings to that name 
                SaveSettings(textBoxSettingsFile.Text);


            }
            catch (Exception ex)
            {
                LogToUser(MethodBase.GetCurrentMethod().Name.ToString() + ":" + "Exception:" + ex.Message);
            }

        }

        private void buttonSaveSettingsAs_Click(object sender, EventArgs e)
        {
            try
            {
                // We need to get the name of the file that the user wants to save the settings as
                saveFileDialogSaveSettings.ShowDialog();
                if (saveFileDialogSaveSettings.FileName != "")
                {

                    // Now we can save the settings to that name 
                    SaveSettings(saveFileDialogSaveSettings.FileName);

                    // Finally we can update the text box to show the user the name of the current setting file and allow save
                    textBoxSettingsFile.Text = saveFileDialogSaveSettings.FileName;
                    buttonSaveSettings.Enabled = true;

                }
            }
            catch (Exception ex)
            {
                LogToUser(MethodBase.GetCurrentMethod().Name.ToString() + ":" + "Exception:" + ex.Message);
            }
        }

        private void buttonLoadSettings_Click(object sender, EventArgs e)
        {
            try
            {
                LoadSettings();
            }
            catch (Exception ex)
            {
                LogToUser(MethodBase.GetCurrentMethod().Name.ToString() + ":" + "Exception:" + ex.Message);
            }
        }

        protected void LoadSettings()
        {
            // We will need to get the name of the settings file.
            // Setup the dialog box for opening an XML file
            openFileDialog.FileName = "";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Open Import Settings";
            openFileDialog.ShowDialog();
            if (File.Exists(openFileDialog.FileName))
            {

                // Now that we have the name of the settings file we can load it.
                ImportSettings importsettings = ImportHelper.LoadSettings(openFileDialog.FileName);

                // Once we have loaded the file we must place the settings into the UI.
                ImportSettingsToUI(importsettings);

                // Finally we can update the text box which shows the user the name of the settings file
                // and enable the save settings button 
                textBoxSettingsFile.Text = openFileDialog.FileName;
                buttonSaveSettings.Enabled = true;
            }
        }

        protected void SaveSettings(string strFileNameAndPath)
        {
            // We just need to call the helper function with the current settings displayed in the UI
            ImportHelper.SaveSettings(strFileNameAndPath, ImportSettingsFromUI());

        }

        private void buttonPauseImport_Click(object sender, EventArgs e)
        {
            try
            {
                ImportPausedFromUI();
            }
            catch (Exception ex)
            {
                LogToUser(MethodBase.GetCurrentMethod().Name.ToString() + ":" + "Exception:" + ex.Message);
            }

        }

        private void buttonResumeImport_Click(object sender, EventArgs e)
        {
            try
            {
                ImportResumedFromUI();
            }
            catch (Exception ex)
            {
                LogToUser(MethodBase.GetCurrentMethod().Name.ToString() + ":" + "Exception:" + ex.Message);
            }

        }

        protected void SetUIControlStatus(ImportProgress currentprogress)
        {

            if (currentprogress.ImportStatus == ImportProgress.Status.New)
            {
                groupBoxDestination.Enabled = true;
                groupBoxImportSettings.Enabled = true;
                groupBoxSelectDestination.Enabled = true;
                groupBoxLogging.Enabled = true;
                
                groupBoxSourceType.Enabled = true;
                groupBoxSourceFolder.Enabled = true;
                groupBoxSourceFile.Enabled = true;
                groupBoxMetaData.Enabled = true;
                groupBoxAuthentication.Enabled = true;
                groupBoxExceptions.Enabled = true;

                buttonStartImport.Enabled = true;
                buttonPauseImport.Enabled = false;
                buttonResumeImport.Enabled = false;
                buttonCancelImport.Enabled = false;

            }

            if (currentprogress.ImportStatus == ImportProgress.Status.Loading)
            {
                groupBoxDestination.Enabled = false;
                groupBoxImportSettings.Enabled = false;
                groupBoxSelectDestination.Enabled = false;
                groupBoxLogging.Enabled = false;

                groupBoxSourceType.Enabled = false;
                groupBoxSourceFolder.Enabled = false;
                groupBoxSourceFile.Enabled = false;
                groupBoxMetaData.Enabled = false;
                groupBoxAuthentication.Enabled = false;
                groupBoxExceptions.Enabled = false;

                buttonStartImport.Enabled = false;
                buttonPauseImport.Enabled = false;
                buttonResumeImport.Enabled = false;
                buttonCancelImport.Enabled = false;
            }

            if (currentprogress.ImportStatus == ImportProgress.Status.Importing)
            {
                groupBoxDestination.Enabled = false;
                groupBoxImportSettings.Enabled = false;
                groupBoxSelectDestination.Enabled = false;
                groupBoxLogging.Enabled = false;

                groupBoxSourceType.Enabled = false;
                groupBoxSourceFolder.Enabled = false;
                groupBoxSourceFile.Enabled = false;
                groupBoxMetaData.Enabled = false;
                groupBoxAuthentication.Enabled = false;
                groupBoxExceptions.Enabled = false;

                buttonStartImport.Enabled = false;
                buttonPauseImport.Enabled = true;
                buttonResumeImport.Enabled = false;
                buttonCancelImport.Enabled = false;
            }

            if (currentprogress.ImportStatus == ImportProgress.Status.Paused)
            {
                groupBoxDestination.Enabled = false;
                groupBoxImportSettings.Enabled = false;
                groupBoxSelectDestination.Enabled = false;
                groupBoxLogging.Enabled = false;

                groupBoxSourceType.Enabled = false;
                groupBoxSourceFolder.Enabled = false;
                groupBoxSourceFile.Enabled = false;
                groupBoxMetaData.Enabled = false;
                groupBoxAuthentication.Enabled = false;
                groupBoxExceptions.Enabled = false;

                buttonStartImport.Enabled = false;
                buttonPauseImport.Enabled = false;
                buttonResumeImport.Enabled = true;
                buttonCancelImport.Enabled = true;
            }

            if (currentprogress.ImportStatus == ImportProgress.Status.Completed)
            {
                groupBoxDestination.Enabled = true;
                groupBoxImportSettings.Enabled = true;
                groupBoxSelectDestination.Enabled = true;
                groupBoxLogging.Enabled = true;

                groupBoxSourceType.Enabled = true;
                groupBoxSourceFolder.Enabled = true;
                groupBoxSourceFile.Enabled = true;
                groupBoxMetaData.Enabled = true;
                groupBoxAuthentication.Enabled = true;
                groupBoxExceptions.Enabled = true;

                buttonStartImport.Enabled = true;
                buttonPauseImport.Enabled = false;
                buttonResumeImport.Enabled = false;
                buttonCancelImport.Enabled = true;
            }

        }

        protected void SetUIProgressStatus(ImportProgress currentprogress)
        {
            // We should inform the user of any activity
            LogToUser(currentprogress.ImportActivity);

            // We should update the number of items to be imported
            textItemsCount.Text = currentprogress.ImportItemsCount.ToString();

            // We should update the number of items that have been processed
            textBoxItemsProcessed.Text = currentprogress.ImportItemsProcessed.ToString();
            textBoxItemsFailed.Text = currentprogress.ImportItemsFailed.ToString();
            textBoxItemsSucceeded.Text = currentprogress.ImportItemsSucceeded.ToString();

            // We should update the progress bar
            progressBarImport.Value = currentprogress.ImportProgressPercentage;
            progressBarImport.Update();

        }

        private void buttonCancelImport_Click(object sender, EventArgs e)
        {
            try
            {
                ImportCancelledFromUI();
            }
            catch (Exception ex)
            {
                LogToUser(MethodBase.GetCurrentMethod().Name.ToString() + ":" + "Exception:" + ex.Message);
            }

        }

        private void buttonSaveExceptions_Click(object sender, EventArgs e)
        {
            try
            {
                SaveExceptions();
            }
            catch (Exception ex)
            {
                LogToUser(MethodBase.GetCurrentMethod().Name.ToString() + ":" + "Exception:" + ex.Message);
            }

        }

        protected void SaveExceptions()
        {
            LogToUser(MethodBase.GetCurrentMethod().Name.ToString() + ":Saving");
            importdocument.SaveExceptions();
            LogToUser(MethodBase.GetCurrentMethod().Name.ToString() + ":Saved");


        }

        // Enable and disable the group box for the selection of the data set
        private void radioButtonXMLDataSet_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonXMLDataSet.Checked)
            {
                ImportSourceXMLDataSet();
            }

        }

        // Allows the user to select the input XML Data set
        private void buttonSourceXMLDataSet_Click(object sender, EventArgs e)
        {

            
            // Show the dialog to the user
            openFileDialog.ShowDialog();
            textBoxSourceFile.Text = openFileDialog.FileName;
        }

        private void linkLabelHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ShowHelp();
            }
            catch(Exception ex)
            {
                LogToUser(MethodBase.GetCurrentMethod().Name.ToString() + ":" + "Exception:" + ex.Message);
            }
        }

        protected void ShowHelp()
        {
            System.Diagnostics.Process.Start("http://difs.codeplex.com/documentation");
        }

        private void radioButtonAuthenticationModeCurrent_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAuthenticationModeCurrent.Checked)
            {
                panelAuthenticationCredentials.Enabled = false;
            }
        }

        private void radioButtonAuthenticationModeSpecified_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAuthenticationModeSpecified.Checked)
            {
                panelAuthenticationCredentials.Enabled = true;
                textBoxAuthenticationDomain.Enabled = true;
            }
        }

        private void radioButtonAuthenticationModeForms_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAuthenticationModeForms.Checked)
            {
                panelAuthenticationCredentials.Enabled = true;
                textBoxAuthenticationDomain.Enabled = false;
            }
        }

        private void radioButtonFileSystemFolder_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonFileSystemFolder.Checked)
            {
                ImportSourceFileSystem();
            }

        }

        private void radioButtonCSVFile_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCSVFile.Checked)
            {
                ImportSourceCSV();
            }

        }

        protected void ImportSourceFileSystem()
        {
            // Hide the source file selection
            groupBoxSourceFile.Visible = false;
        }

        protected void ImportSourceXMLDataSet()
        {

            // Show the source file selection
            groupBoxSourceFile.Visible = true;

            // Setup the dialog box for opening a XML file
            openFileDialog.FileName = "";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Open XML Data Set";
        
        }

        protected void ImportSourceCSV()
        {

            // Show the source file selection 
            groupBoxSourceFile.Visible = true;

            // Setup the dialog box for opening a CSV file
            openFileDialog.FileName = "";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DefaultExt = ".csv";
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Open CSV File";

        }

        private void buttonRefreshSharePoint_Click(object sender, EventArgs e)
        {
            try
            {
                ImportSettings importsettings = ImportSettingsFromUI();
                Control[] controls = ImportHelper.RefreshSharePointFields(importsettings.Destination.DestinationServerUrl + importsettings.Destination.DestinationWebUrl, importsettings.authenticationsettings, importsettings.Destination.DestinationLibraryName);
                tableLayoutPanelAdditionalMetaData.RowCount = controls.GetLength(0);
                for (int i = 0; i <= controls.GetUpperBound(0); i++)
                {
                    tableLayoutPanelAdditionalMetaData.Controls.Add(controls[i], 0, i);
                }
                this.tableLayoutPanelAdditionalMetaData.ResumeLayout(false);
                this.tableLayoutPanelAdditionalMetaData.PerformLayout();
                this.ResumeLayout(false);



            }
            catch (Exception ex)
            {
                LogToUser(MethodBase.GetCurrentMethod().Name.ToString() + ":" + "Exception:" + ex.Message);
            }

            
        }

        private void radioButtonAuthenticationModeOffice365_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAuthenticationModeOffice365.Checked)
            {
                panelAuthenticationCredentials.Enabled = true;
                textBoxAuthenticationDomain.Enabled = false;
            }

        }




    }

    public class ImportControl
    {
        public bool Resume;

    }
}
