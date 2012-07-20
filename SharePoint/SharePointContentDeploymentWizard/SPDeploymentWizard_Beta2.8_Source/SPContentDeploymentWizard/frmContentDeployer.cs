using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Xml;
using System.Collections;
using System.IO;
using System.Security.Policy;
using System.Configuration;

using Microsoft.SharePoint.Deployment;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

using WizardBase;
using COB.SharePoint.Utilities.DeploymentWizard.Core;
using COB.SharePoint.Utilities.DeploymentWizard.Core.Helpers;

namespace COB.SharePoint.Utilities.DeploymentWizard.UI
{
    /// <summary>
    /// Created by Chris O'Brien (http://www.sharepointnutsandbolts.com), published at http://www.codeplex.com/SPDeploymentWizard. 
    /// 
    /// The excellent C# WinForms wizard framework kindly supplied by Manish Ranjan Kumar (http://www.codeproject.com/KB/cs/WizardDemo.aspx), 
    /// license found at http://www.codeproject.com/info/cpol10.aspx.
    /// 
    /// Uses a wizard interface/STSADM command to import/export SharePoint content - Wizard provides a treeview to allow granular 
    /// selection of content to be moved. The tool uses the SharePoint Content Deployment API to import/export content. 
    /// </summary>
    /// <created version="0.0.0.1" by="Chris O'Brien" date="15 February 2007">Initial version.</created> 
    /// <updated version="1.0.0.0" by="Chris O'Brien" date="05 December 2007">Beta 1 release.</updated> 
    /// <updated version="2.0.0.0" by="Chris O'Brien" date="28 February 2008">Beta 2 release. See 
    /// http://www.codeplex.com/SPDeploymentWizard/Release/ProjectReleases.aspx?ReleaseId=11148 for release notes.</updated> 
    /// <updated version="3.0.0.0" by="Chris O'Brien" date="22 June 2008">Release 1.0. See 
    /// http://www.codeplex.com/SPDeploymentWizard/Release/ProjectReleases.aspx?ReleaseId=14631 for release notes.</updated> 
    /// <updated version="3.0.1.0" by="Chris O'Brien" date="21 September 2008">Release 1.1 - tidied source code slightly for 
    /// public release, fixed bug when used with insuffcient SharePoint db permissions.</updated>
    /// <updated version="4.0.0.0" by="Chris O'Brien" date="21 March 2009">Alpha release with command-line support.</updated>
    /// <updated version="4.0.0.1" by="Alex Angas/Chris O'Brien" date="16 Oct 2009">Added ability to export/import list items between different lists.</updated>
    /// <updated version="4.0.1.0" by="Chris O'Brien" date="22 March 2010">Added try/catch to deal with threading issue introduced in previous release</updated>
    public partial class frmContentDeployer : Form
    {
        #region -- Private members --

        private frmBinding m_bindingMsgForm = new frmBinding();
        private WizardDeployment f_wizardDeployment = null;

        private const int f_ciWELCOME_STEP_INDEX = 0;
        private const int f_ciSITE_DETAILS_STEP_INDEX = 1;
        private const int f_ciIMPORT_STEP_INDEX = 2;
        private const int f_ciEXPORT_SELECT_STEP_INDEX = 3;
        private const int f_ciEXPORT_SETTINGS_STEP_INDEX = 4;
        private const int f_ciFINISH_STEP_INDEX = 5;

        private const int f_ciIMPORT_FILE_PATH_WRAP_THRESHOLD = 36;
        private const int f_ciIMPORT_LOG_FILE_PATH_WRAP_THRESHOLD = 70;
        private const int f_ciEXPORT_FILE_PATH_WRAP_THRESHOLD = 80;
        private const int f_ciEXPORT_LOG_FILE_PATH_WRAP_THRESHOLD = 80;
        private const int f_ciEXPORT_FOLDER_PATH_WRAP_THRESHOLD = 70;
        private const int f_cFINISH_SCREEN_WRAP_THRESHOLD = 60;
        private const int f_ciIMPORT_ITEM_ACTION_THRESHOLD = 80;

        private readonly string f_csINCLUDE_DESCS_ALL_TEXT = "Include all descendents";
        private readonly string f_csINCLUDE_DESCS_CONTENT_ONLY_TEXT = "Include content descendents";
        private readonly string f_csINCLUDE_DESCS_NONE_TEXT = "Exclude descendents";
        private readonly string f_csNODES_NOT_RETRIEVED_TEXT = "Please rebind to site to see these nodes - return to previous step by clicking 'Back', then click 'Next'.";
        private readonly string f_csDEFAULT_LISTS_NOT_FOR_EXPORT = "Cache Profiles, Content and Structure Reports, Converted Forms, Long Running Operation Status, Notification List, " +
            "Quick Deploy Items, Relationships List, Reporting Metadata, Reporting Templates, User Information List, Variation Labels, Workflows, Workflow Tasks, Workflow History, fpdatasources";
        private readonly string f_csEXPORTMETHOD_EXPORTCHANGES = "ExportChanges";
        private readonly string f_csEXPORTMETHOD_EXPORTALL = "ExportAll";

        private string[] f_aListsNotForExport = null;

        private string f_sSiteUrl = null;
        private string f_sExportFolderPath = null;
        private string f_sExportBaseFilename = null;
        private SPExportSettings f_exportSettings = null;
        private WizardExportSettings f_supplementaryExportSettings = null;
        private WizardAction f_action = WizardAction.None;

        private int f_iDisposedWebsCount = 0;
        private int f_iDisposedWebsFromMenuCount = 0;

        public delegate void SiteBindCompleteEventHandler(object sender, EventArgs e);
        public static event SiteBindCompleteEventHandler SiteBindCompleteEvent;

        private delegate void updateProgressOnDeployment(ProgressBar prgBar, SPDeploymentEventArgs e, object data);
        private delegate void updateOnDeploymentComplete(ProgressBar prgBar, WizardControl wizard);
        private delegate void updateOnItemImportedExported(Label lblUpdate, SPObjectImportedEventArgs e);

        private readonly string f_csVALIDATION_MESSAGE_BOX_CAPTION = "Validation error";
        private ImageList f_imageList = new ImageList();
        private readonly string f_csSITE_ICON_IDENTIFIER = "SiteIcon";
        private readonly string f_csLIST_ICON_IDENTIFIER = "ListIcon";
        private readonly string f_csFOLDER_ICON_IDENTIFIER = "FolderIcon";
        private readonly string f_csFILE_ICON_IDENTIFIER = "FileIcon";

        private TraceHelper f_traceHelper = null;
        private TraceSwitch f_traceSwitch = null;

        private enum WizardAction
        {
            Import,
            Export,
            None
        }

        #endregion

        #region -- Constructor --

        public frmContentDeployer()
        {
            f_traceSwitch = new TraceSwitch("COB.SharePoint.Utilities.ContentDeploymentWizard",
                "Trace switch for ContentDeploymentWizard");

            // initialise the trace helper for output formatting
            f_traceHelper = new TraceHelper(this);

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("frmContentDeployer: Entered frmContentDeployer().");
            }

            InitializeComponent();
            performDataBinding();

            f_aListsNotForExport = fetchListsNotForExport();

            prgExport.Enabled = false;
            prgImport.Enabled = true;

            if (f_traceSwitch.TraceInfo)
            {
                f_traceHelper.TraceInfo("frmContentDeployer: Initialised and bound controls.");
            }

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("frmContentDeployer: Exiting frmContentDeployer().");
            }
        }

        #endregion

        #region -- Initialisation/data-binding --

        private void performDataBinding()
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("performDataBinding: Entered performDataBinding().");
            }

            AutoCompleteStringCollection aSiteNames = new AutoCompleteStringCollection();
            SPFarm currentFarm = SPFarm.Local;
            if (currentFarm != null)
            {
                SPWebService service = currentFarm.Services.GetValue<SPWebService>(string.Empty);

                foreach (SPWebApplication webApp in service.WebApplications)
                {
                    if (!webApp.IsAdministrationWebApplication)
                    {
                        foreach (SPSite site in webApp.Sites)
                        {
                            aSiteNames.Add(site.Url);
                        }
                    }
                }
            }

            txtSiteUrl.AutoCompleteCustomSource = aSiteNames;

            wizardControl1.CurrentStepIndexChanged += new EventHandler(wizardControl1_CurrentStepIndexChanged);
            wizardControl1.NextButtonClick += new WizardBase.WizardNextButtonClickEventHandler(wizardControl1_NextButtonClick);
            wizardControl1.CancelButtonClick += new EventHandler(wizardControl1_CancelButtonClick);
            wizardControl1.FinishButtonClick += new EventHandler(wizardControl1_FinishButtonClick);
            lstExportItems.MouseClick += new MouseEventHandler(lstExportItems_MouseClick);
            ctxtMenuExportItem.Click += new EventHandler(ctxtMenuExportItem_Click);

            string[] a_sExportMethods = Enum.GetNames(typeof(SPExportMethodType));
            cboExportMethod.DataSource = a_sExportMethods;

            string[] a_sVersionOptions = Enum.GetNames(typeof(SPIncludeVersions));
            cboIncludeVersions.DataSource = a_sVersionOptions;

            string[] a_sSecurityOptions = Enum.GetNames(typeof(SPIncludeSecurity));
            cboIncludeSecurity.DataSource = a_sSecurityOptions;
            cboIncludeSecurityImport.DataSource = a_sSecurityOptions;

            string[] a_sVersionOverwriteOptions = Enum.GetNames(typeof(SPUpdateVersions));
            cboVersionOptions.DataSource = a_sVersionOverwriteOptions;

            string[] a_sUpdateUserInfoOptions = Enum.GetNames(typeof(SPImportUserInfoDateTimeOption));
            cboUserInfo.DataSource = a_sUpdateUserInfoOptions;

            trvContent.Nodes.Add("<Not yet bound to site>");

            loadImages();

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("performDataBinding: Exited performDataBinding().");
            }
        }

        private void loadImages()
        {
            f_imageList.Images.Add(f_csSITE_ICON_IDENTIFIER, (Image)Resources.SiteIcon);
            f_imageList.Images.Add(f_csLIST_ICON_IDENTIFIER, (Image)Resources.ListIcon);
            f_imageList.Images.Add(f_csFOLDER_ICON_IDENTIFIER, (Image)Resources.FolderIcon);
            f_imageList.Images.Add(f_csFILE_ICON_IDENTIFIER, (Image)Resources.FileIcon);
        }

        #endregion

        #region -- Treeview behaviour --

        public void doSiteTreeBind(SPSite site)
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("doSiteTreeBind: Entered doSiteTreeBind().");
            }

            trvContent.Nodes.Clear();
            trvContent.ImageList = f_imageList;

            trvContent.BeforeExpand += new TreeViewCancelEventHandler(trvContent_BeforeExpand);
            try
            {
                addNodesForWeb(site.RootWeb);
                trvContent.Nodes[0].Expand();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(
                    "Unable to list site nodes - the current user is not a site collection administrator for this site.\n\n" +
                    "Please restart the Wizard using a user which has appropriate permissions.",
                    "Insufficient permissions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("doSiteTreeBind: Exited doSiteTreeBind().");
            }
        }

        private void addNodesForWeb(SPWeb web)
        {
            addNodesForWeb(web, null);
        }

        private void addNodesForWeb(SPWeb web, TreeNode parentNode)
        {
            TreeNode rootWebNode = null;
            TreeNode webNode = null;
            TreeNode listNode = null;

            if (parentNode == null)
            {
                SPSite parentSite = web.Site;

                // add node for root web..
                parentNode = trvContent.Nodes.Add(web.Url);
                parentNode.Name = parentSite.ID.ToString();
                parentNode.Tag = getObjectData(parentSite);
                addContextMenuToTreeViewNode(parentNode);
            }

            // add immediate subwebs..
            foreach (SPWeb childWeb in web.Webs)
            {
                webNode = new TreeNode(childWeb.Url);
                webNode.Name = childWeb.ID.ToString();
                webNode.Tag = getObjectData(childWeb);
                webNode.ImageKey = f_csSITE_ICON_IDENTIFIER;
                webNode.SelectedImageKey = f_csSITE_ICON_IDENTIFIER;
                addContextMenuToTreeViewNode(webNode);
                webNode.Nodes.Add(f_csNODES_NOT_RETRIEVED_TEXT);

                parentNode.Nodes.Add(webNode);

                childWeb.Dispose();
            }

            // add immediate lists..
            foreach (SPList list in web.Lists)
            {
                if (listIsContent(list))
                {
                    listNode = new TreeNode(string.Format("{0} ({1})",
                        getListFriendlyName(list, false), list.ItemCount));
                    listNode.Name = list.ID.ToString();
                    listNode.Tag = getObjectData(list);
                    listNode.ImageKey = f_csLIST_ICON_IDENTIFIER;
                    listNode.SelectedImageKey = f_csLIST_ICON_IDENTIFIER;

                    addContextMenuToTreeViewNode(listNode);
                    if (list.ItemCount > 0)
                    {
                        listNode.Nodes.Add(f_csNODES_NOT_RETRIEVED_TEXT);
                    }

                    parentNode.Nodes.Add(listNode);
                }
            }
        }

        private void addNodesForList(SPList list, TreeNode parentNode)
        {
            addNodesForFolder(list.RootFolder, parentNode);
        }

        private void addNodesForFolder(SPFolder folder, TreeNode parentNode)
        {
            TreeNode currentNode = null;

            SPQuery query = new SPQuery();
            query.Folder = folder;
            using (SPWeb parentWeb = folder.ParentWeb)
            {
                SPListItemCollection folderItems = parentWeb.Lists[folder.ParentListId].GetItems(query);
                foreach (SPListItem item in folderItems)
                {
                    currentNode = new TreeNode(item.Name);
                    currentNode.Name = item.UniqueId.ToString();
                    currentNode.Tag = getObjectData(item);

                    if (item.Folder != null)
                    {
                        currentNode.ImageKey = f_csFOLDER_ICON_IDENTIFIER;
                        currentNode.SelectedImageKey = f_csFOLDER_ICON_IDENTIFIER;
                    }
                    else
                    {
                        currentNode.ImageKey = f_csFILE_ICON_IDENTIFIER;
                        currentNode.SelectedImageKey = f_csFILE_ICON_IDENTIFIER;
                    }

                    addContextMenuToTreeViewNode(currentNode);

                    parentNode.Nodes.Add(currentNode);

                    // also recurse child folders..
                    if (item.Folder != null)
                    {
                        addNodesForFolder(item.Folder, currentNode);
                    }
                }
            }
        }

        void trvContent_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // get reference to site, web, list etc..
            SPObjectData objectData = (SPObjectData)e.Node.Tag;

            if (objectData.ObjectType != SPDeploymentObjectType.Site)
            {
                lblRetrievingData.Visible = true;
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();

                populateObjectChildNodes(e.Node, objectData);

                OnSiteBindComplete();

                this.Cursor = Cursors.Default;
                lblRetrievingData.Visible = false;
            }
        }

        private void populateObjectChildNodes(TreeNode parentNode, SPObjectData objectData)
        {
            // only add nodes if reference to object's SPWeb is valid and has not been disposed. If it is 
            // not valid, this indicates node has already been expanded and child nodes added..
            if (objectData.Web != null)
            {
                parentNode.Nodes.Clear();
                SPWeb currentWeb = objectData.Web;

                switch (objectData.ObjectType)
                {
                    case SPDeploymentObjectType.Web:
                        // get SPWeb object then pass to addNodesForWeb..
                        addNodesForWeb(currentWeb, parentNode);
                        currentWeb.Dispose();
                        // set objectData.Web to null for next pass as well as do disposal..
                        objectData.Web = null;
                        disposeWebs(parentNode, false);
                        break;
                    case SPDeploymentObjectType.List:
                        SPList currentList = currentWeb.Lists[objectData.ID];
                        addNodesForList(currentList, parentNode);
                        currentWeb.Dispose();
                        // set objectData.Web to null for next pass as well as do disposal..
                        objectData.Web = null;
                        disposeWebs(parentNode, false);
                        break;
                    case SPDeploymentObjectType.Folder:

                        break;
                    default:
                        break;
                }

                parentNode.Expand();
            }
        }

        private void addContextMenuToTreeViewNode(TreeNode node)
        {
            ContextMenuStrip ctxtMenu = new ContextMenuStrip();
            ctxtMenu.Items.AddRange(getContextMenuItemsForNode(node));
            node.ContextMenuStrip = ctxtMenu;
        }

        private void addNodesFromWeb(TreeView trvControl, TreeNode parentNode, SPWeb currentWeb)
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("addNodesFromWeb: Entered addNodesFromWeb().");
            }

            // add nodes from current web..
            TreeNode webNode = new TreeNode(currentWeb.Url);

            // slightly different behaviour if the site is actually the RootWeb for the site..
            if (parentNode == null)
            {
                webNode.Tag = getObjectData(currentWeb.Site);
                webNode.Expand();
            }
            else
            {
                webNode.Tag = getObjectData(currentWeb);
            }

            addContextMenuToTreeViewNode(webNode);

            if (parentNode == null)
            {
                trvControl.Nodes.Add(webNode);
            }
            else
            {
                parentNode.Nodes.Add(webNode);
            }

            // recurse child webs..
            foreach (SPWeb childWeb in currentWeb.Webs)
            {
                addNodesFromWeb(trvContent, webNode, childWeb);
                childWeb.Dispose();
            }

            SPListCollection colLists = currentWeb.Lists;
            foreach (SPList list in colLists)
            {
                if (listIsContent(list))
                {
                    addListNode(trvContent, webNode, list);
                }
            }

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("addNodesFromWeb: Exited addNodesFromWeb().");
            }
        }

        /// <summary>
        /// Get child nodes for context menu - also wires event handlers and sets property 
        /// for 'include descendents' choice. TODO: This method ** desperately ** needs refactoring, written in the middle of the night..
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private ToolStripMenuItem[] getContextMenuSubItems(TreeNode node)
        {
            List<ToolStripMenuItem> colMenuItems = new List<ToolStripMenuItem>();

            SPObjectData exportObject = (SPObjectData)node.Tag;
            ToolStripMenuItem incAllDescsItem = new ToolStripMenuItem();

            // we clone the existing SPObjectData for the menu item here and set the 'include 
            // decendents' property depending on what the option represents..
            if (exportObject.ObjectType == SPDeploymentObjectType.Site)
            {
                // add site item..
                ToolStripMenuItem exportAsSiteItem = new ToolStripMenuItem();
                exportAsSiteItem.Text = "Export entire site";

                incAllDescsItem = new ToolStripMenuItem();
                incAllDescsItem.Text = f_csINCLUDE_DESCS_ALL_TEXT;
                incAllDescsItem.Click += new EventHandler(contextMenuSubItem_Click);

                SPObjectData objectDataSiteAllDescs = (SPObjectData)exportObject.Clone();
                objectDataSiteAllDescs.ObjectType = SPDeploymentObjectType.Site;
                objectDataSiteAllDescs.IncludeDescendents = SPIncludeDescendants.All;
                incAllDescsItem.Tag = objectDataSiteAllDescs;

                exportAsSiteItem.DropDown.Items.Add(incAllDescsItem);
                colMenuItems.Add(exportAsSiteItem);

                // site/content only..
                if (canExportContentOnly(exportObject))
                {
                    ToolStripMenuItem incContentDescsItem = new ToolStripMenuItem();
                    incContentDescsItem.Text = f_csINCLUDE_DESCS_CONTENT_ONLY_TEXT;
                    incContentDescsItem.Click += new EventHandler(contextMenuSubItem_Click);

                    // overwrite value of 'include descendents' property as appropriate to this menu item..
                    SPObjectData objectDataContentOnly = (SPObjectData)exportObject.Clone();
                    objectDataContentOnly.IncludeDescendents = SPIncludeDescendants.Content;
                    incContentDescsItem.Tag = objectDataContentOnly;

                    exportAsSiteItem.DropDown.Items.Add(incContentDescsItem);
                }

                // site/no descs..
                ToolStripMenuItem incNoDescsItem = new ToolStripMenuItem();
                incNoDescsItem.Text = f_csINCLUDE_DESCS_NONE_TEXT;
                incNoDescsItem.Click += new EventHandler(contextMenuSubItem_Click);

                // overwrite value of 'include descendents' property as appropriate to this menu item..
                SPObjectData objectDataNoDescs = (SPObjectData)exportObject.Clone();
                objectDataNoDescs.IncludeDescendents = SPIncludeDescendants.None;
                incNoDescsItem.Tag = objectDataNoDescs;

                exportAsSiteItem.DropDown.Items.Add(incNoDescsItem);

                // add web item..
                ToolStripMenuItem exportAsWebItem = new ToolStripMenuItem();
                exportAsWebItem.Text = "Export root web only";

                incAllDescsItem = new ToolStripMenuItem();
                incAllDescsItem.Text = f_csINCLUDE_DESCS_ALL_TEXT;
                incAllDescsItem.Click += new EventHandler(contextMenuSubItem_Click);

                SPObjectData objectDataWebAllDescs = (SPObjectData)exportObject.Clone();
                objectDataWebAllDescs.ObjectType = SPDeploymentObjectType.Web;
                objectDataWebAllDescs.IncludeDescendents = SPIncludeDescendants.All;
                incAllDescsItem.Tag = objectDataWebAllDescs;

                exportAsWebItem.DropDown.Items.Add(incAllDescsItem);
                colMenuItems.Add(exportAsWebItem);

                // web/content only..
                if (canExportContentOnly(exportObject))
                {
                    ToolStripMenuItem incContentDescsItem = new ToolStripMenuItem();
                    incContentDescsItem.Text = f_csINCLUDE_DESCS_CONTENT_ONLY_TEXT;
                    incContentDescsItem.Click += new EventHandler(contextMenuSubItem_Click);

                    // overwrite value of 'include descendents' property as appropriate to this menu item..
                    SPObjectData objectDataContentOnly = (SPObjectData)exportObject.Clone();
                    objectDataContentOnly.ObjectType = SPDeploymentObjectType.Web;
                    objectDataContentOnly.IncludeDescendents = SPIncludeDescendants.Content;
                    incContentDescsItem.Tag = objectDataContentOnly;

                    exportAsWebItem.DropDown.Items.Add(incContentDescsItem);
                }

                // web/no descs..
                incNoDescsItem = new ToolStripMenuItem();
                incNoDescsItem.Text = f_csINCLUDE_DESCS_NONE_TEXT;
                incNoDescsItem.Click += new EventHandler(contextMenuSubItem_Click);

                // overwrite value of 'include descendents' property as appropriate to this menu item..
                objectDataNoDescs = (SPObjectData)exportObject.Clone();
                objectDataNoDescs.ObjectType = SPDeploymentObjectType.Web;
                objectDataNoDescs.IncludeDescendents = SPIncludeDescendants.None;
                incNoDescsItem.Tag = objectDataNoDescs;

                exportAsWebItem.DropDown.Items.Add(incNoDescsItem);
            }
            else
            {
                // overwrite value of 'include descendents' property as appropriate to this menu item..
                SPObjectData objectDataAllDescs = (SPObjectData)exportObject.Clone();
                objectDataAllDescs.IncludeDescendents = SPIncludeDescendants.All;
                incAllDescsItem.Tag = objectDataAllDescs;
                incAllDescsItem = new ToolStripMenuItem();
                incAllDescsItem.Text = f_csINCLUDE_DESCS_ALL_TEXT;
                incAllDescsItem.Click += new EventHandler(contextMenuSubItem_Click);
                incAllDescsItem.Tag = objectDataAllDescs;

                colMenuItems.Add(incAllDescsItem);

                if (canExportContentOnly(exportObject))
                {
                    ToolStripMenuItem incContentDescsItem = new ToolStripMenuItem();
                    incContentDescsItem.Text = f_csINCLUDE_DESCS_CONTENT_ONLY_TEXT;
                    incContentDescsItem.Click += new EventHandler(contextMenuSubItem_Click);

                    // overwrite value of 'include descendents' property as appropriate to this menu item..
                    SPObjectData objectDataContentOnly = (SPObjectData)exportObject.Clone();
                    objectDataContentOnly.IncludeDescendents = SPIncludeDescendants.Content;
                    incContentDescsItem.Tag = objectDataContentOnly;

                    colMenuItems.Add(incContentDescsItem);
                }

                ToolStripMenuItem incNoDescsItem = new ToolStripMenuItem();
                incNoDescsItem.Text = f_csINCLUDE_DESCS_NONE_TEXT;
                incNoDescsItem.Click += new EventHandler(contextMenuSubItem_Click);

                // overwrite value of 'include descendents' property as appropriate to this menu item..
                SPObjectData objectDataNoDescs = (SPObjectData)exportObject.Clone();
                objectDataNoDescs.IncludeDescendents = SPIncludeDescendants.None;
                incNoDescsItem.Tag = objectDataNoDescs;

                colMenuItems.Add(incNoDescsItem);
            }

            return colMenuItems.ToArray();
        }

        private ToolStripMenuItem[] getContextMenuItemsForNode(TreeNode node)
        {
            ToolStripMenuItem exportMenuItem = new ToolStripMenuItem("Export");

            // check if node should have 'include descs' options..
            SPObjectData exportObject = (SPObjectData)node.Tag;
            exportMenuItem.Tag = exportObject;

            if (exportObjectCanHaveChildren(exportObject))
            {
                exportMenuItem.CheckOnClick = false;
                exportMenuItem.DropDown.Items.AddRange(getContextMenuSubItems(node));
            }
            else
            {
                exportMenuItem.Click += new EventHandler(contextMenuSubItem_Click);
                exportObject.IncludeDescendents = SPIncludeDescendants.None;
            }

            return new ToolStripMenuItem[] { exportMenuItem };
        }

        protected void OnSiteBindComplete()
        {
            if (SiteBindCompleteEvent != null)
            {
                SiteBindCompleteEvent(this, new EventArgs());
            }
        }

        private void addItemToExportListView(ToolStripItem menuItem)
        {
            bool bAdd = true;
            SPObjectData existingExportObject = null;

            foreach (ListViewItem existingItem in lstExportItems.Items)
            {
                existingExportObject = existingItem.Tag as SPObjectData;
                if ((existingExportObject != null) && (existingExportObject.ObjectType == SPDeploymentObjectType.Site))
                {
                    MessageBox.Show("Unable to add further objects to export when the parent site collection is already being exported!",
                        "Selection error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    bAdd = false;
                    break;
                }
            }

            if (bAdd)
            {
                SPObjectData exportObject = (SPObjectData)menuItem.Tag;
                lstExportItems.Items.Add(getListViewItemFromExportObject(exportObject));
            }
        }

        private ListViewItem getListViewItemFromExportObject(SPObjectData exportObject)
        {
            string sExportItemTitle = ((exportObject.ObjectType == SPDeploymentObjectType.ListItem) ||
                (exportObject.ObjectType == SPDeploymentObjectType.File) ||
                (exportObject.ObjectType == SPDeploymentObjectType.Folder)) ?
                exportObject.Title : exportObject.Url;
            ListViewItem exportItem = new ListViewItem(new string[] { sExportItemTitle, 
                exportObject.ObjectType.ToString(), 
                exportObject.IncludeDescendents.ToString() });

            exportItem.Tag = exportObject;
            return exportItem;
        }

        private void addListNode(TreeView trvContent, TreeNode parentNode, SPList list)
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose(string.Format("addListNode: Entered addListNode() with list '{0}'.",
                    list.Title));
            }

            TreeNode listRootNode = null;
            TreeNode currentNode = null;

            listRootNode = new TreeNode(string.Format("{0} ({1})",
                list.Title, list.ItemCount));
            listRootNode.Tag = getObjectData(list);

            addContextMenuToTreeViewNode(listRootNode);

            // now deal wih leaf items..
            foreach (SPListItem item in list.Items)
            {
                currentNode = new TreeNode(item.Name);
                currentNode.Tag = getObjectData(item);
                addContextMenuToTreeViewNode(currentNode);

                listRootNode.Nodes.Add(currentNode);
            }

            parentNode.Nodes.Add(listRootNode);

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("addListNode: Exited addListNode().");
            }
        }

        /// <summary>
        /// Handles click event of context menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void contextMenuSubItem_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = (ToolStripItem)sender;
            addItemToExportListView(menuItem);
        }

        #endregion

        #region -- Import/Export helper code --

        private MemoryStream collectExportSettings()
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("CollectExportSettings: Entered CollectExportSettings().");
            }

            // implementing new approach of genereating XML from control values here..
            MemoryStream memStream = new MemoryStream();
            XmlTextWriter xWriter = new XmlTextWriter(memStream, Encoding.UTF8);
            xWriter.WriteStartElement("ExportSettings");

            xWriter.WriteAttributeString("SiteUrl", trvContent.Nodes[0].Text);
            xWriter.WriteAttributeString("ExcludeDependencies", chkExcludeDependencies.Checked.ToString());
            xWriter.WriteAttributeString("ExportMethod", cboExportMethod.Text);
            xWriter.WriteAttributeString("IncludeVersions", cboIncludeVersions.Text);
            xWriter.WriteAttributeString("IncludeSecurity", cboIncludeSecurity.Text);
            xWriter.WriteAttributeString("FileCompression", (!chkDisableCompression.Checked).ToString());
            xWriter.WriteAttributeString("FileLocation", lblExportFolderValue.Text.Replace(Environment.NewLine, string.Empty));
            string sExportFilePath = txtExportBaseFilename.Text;
            if (!sExportFilePath.EndsWith(".cmp", StringComparison.CurrentCultureIgnoreCase))
            {
                sExportFilePath += ".cmp";
            }
            xWriter.WriteAttributeString("BaseFileName", sExportFilePath);

            // now write deployment objects..
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("CollectExportSettings: About to call addExportObjects().");
            }

            addExportObjects(xWriter);

            xWriter.WriteEndElement();
            xWriter.Flush();

            return memStream;
        }

        private MemoryStream collectImportSettings()
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("CollectImportSettings: Entered CollectImportSettings().");
            }

            string sImportBaseFilename = null;
            string sActualImportFolder = null;

            // determine BaseFilename etc..
            if (f_traceSwitch.TraceInfo)
            {
                f_traceHelper.TraceInfo("CollectImportSettings: Collecting import file(s) details.");
            }

            string sImportSingleFilePath = lblImportPathValue.Text.Replace(Environment.NewLine, string.Empty);
            int iPosLastSlash = sImportSingleFilePath.LastIndexOf(@"\");
            sImportBaseFilename = sImportSingleFilePath.Substring(iPosLastSlash + 1);
            sActualImportFolder = sImportSingleFilePath.Substring(0, iPosLastSlash);

            if (f_traceSwitch.TraceInfo)
            {
                f_traceHelper.TraceInfo(string.Format("CollectImportSettings: Determined that BaseFilename is '{0}' and ImportFolder is '{1}'.",
                    sImportBaseFilename, sActualImportFolder));
            }

            if (string.IsNullOrEmpty(sActualImportFolder))
            {
                if (f_traceSwitch.TraceWarning)
                {
                    f_traceHelper.TraceWarning("CollectImportSettings: sActualImportFolder was empty, unable to proceed.");
                }
            }


            // implementing new approach of genereating XML from control values here..
            MemoryStream memStream = new MemoryStream();
            XmlTextWriter xWriter = new XmlTextWriter(memStream, Encoding.UTF8);
            xWriter.WriteStartElement("ImportSettings");

            xWriter.WriteAttributeString("SiteUrl", txtSiteUrl.Text);
            xWriter.WriteAttributeString("ImportWebUrl", txtWebUrl.Text);
            xWriter.WriteAttributeString("FileLocation", sActualImportFolder);
            xWriter.WriteAttributeString("BaseFileName", sImportBaseFilename);
            xWriter.WriteAttributeString("IncludeSecurity", cboIncludeSecurityImport.Text);
            xWriter.WriteAttributeString("VersionOptions", cboVersionOptions.Text);
            xWriter.WriteAttributeString("RetainObjectIdentity", (chkRetainIDs.Checked) ? "True" : "False");
            xWriter.WriteAttributeString("UserInfoUpdate", cboUserInfo.Text);

            xWriter.WriteEndElement();
            xWriter.Flush();

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("CollectImportSettings: Leaving CollectImportSettings().");
            }

            return memStream;
        }

        private void doExport()
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("doExport: Entered doExport().");
            }

            bool bValidated = false;

            if (f_traceSwitch.TraceInfo)
            {
                f_traceHelper.TraceInfo("doExport: About to validate export settings.");
            }

            try
            {
                f_wizardDeployment.ValidateSettings();
                bValidated = true;
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo("doExport: Settings validated successfully.");
                }
            }
            catch (Exception e)
            {
                if (f_traceSwitch.TraceWarning)
                {
                    f_traceHelper.TraceWarning("doExport: Failed to validate export settings! Export will not be done, showing MessageBox.");
                }
                MessageBox.Show(string.Format("The export settings you chose are not valid. Please ensure, for example, you are not exporting a web " +
                    "and specific child objects in the same operation. Message = '{0}'.", e.Message), "Export settings validation error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (bValidated)
            {
                if (string.IsNullOrEmpty(f_wizardDeployment.ExportSettings.FileLocation))
                {
                    if (f_traceSwitch.TraceInfo)
                    {
                        f_traceHelper.TraceInfo("doExport: Filepath was empty, not proceeding.");
                    }

                    MessageBox.Show("You must choose the export path before exporting!", "Export path not selected",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    wizardControl1.BackButtonEnabled = false;
                    wizardControl1.NextButtonEnabled = false;
                    wizardControl1.CancelButtonEnabled = false;

                    lblItemAction.Text = "Export in progress..";
                    lblItemAction.Visible = true;

                    if (f_traceSwitch.TraceInfo)
                    {
                        f_traceHelper.TraceInfo(
                            string.Format(
                                "doExport: Validated export settings, about to start thread to perform export. " +
                                "Current thread ID is '{0}'.",
                                Thread.CurrentThread.ManagedThreadId));
                    }

                    ThreadStart threadDelegate = new ThreadStart(runExportTask);
                    Thread exportThread = new Thread(threadDelegate);
                    exportThread.Start();

                    if (f_traceSwitch.TraceInfo)
                    {
                        f_traceHelper.TraceInfo(string.Format("doExport: Started export worker thread, ID is '{0}'.",
                                                              exportThread.ManagedThreadId));
                    }
                }
            }

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("doExport: Exiting doExport().");
            }
        }

        private void doImport()
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("doImport: Entered doImport().");
            }

            bool bValidated = false;

            if (f_traceSwitch.TraceInfo)
            {
                f_traceHelper.TraceInfo("doImport: About to validate import settings.");
            }

            try
            {
                f_wizardDeployment.ValidateSettings();
                bValidated = true;
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo("doImport: Settings validated successfully.");
                }
            }
            catch (Exception e)
            {
                if (f_traceSwitch.TraceWarning)
                {
                    f_traceHelper.TraceWarning("doImport: Failed to validate export settings! Export will not be done, showing MessageBox.");
                }
                MessageBox.Show(string.Format("The import settings you chose are not valid. Message = '{0}'.", e.Message),
                    "Import settings validation error");
            }

            if (bValidated)
            {
                if (lblItemAction.Visible == false)
                {
                    lblItemAction.Text = "Starting import..";
                    lblItemAction.Visible = true;
                }

                wizardControl1.BackButtonEnabled = false;
                wizardControl1.NextButtonEnabled = false;
                wizardControl1.CancelButtonEnabled = false;

                ThreadStart threadDelegate = new ThreadStart(runImportTask);
                Thread importThread = new Thread(threadDelegate);
                importThread.Start();

                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo(string.Format("doImport: Started import worker thread, ID is '{0}'.",
                                                          importThread.ManagedThreadId));
                }
            }

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("doImport: Exiting doImport().");
            }
        }

        private void runImportTask()
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("runImportTask: Entered runImportTask().");
            }

            f_wizardDeployment.Started += new EventHandler<SPDeploymentEventArgs>(import_Started);
            f_wizardDeployment.ObjectImported += new EventHandler<SPObjectImportedEventArgs>(import_ObjectImported);
            f_wizardDeployment.ProgressUpdated += new EventHandler<SPDeploymentEventArgs>(import_ProgressUpdated);
            f_wizardDeployment.Completed += new EventHandler<SPDeploymentEventArgs>(import_Completed);

            if (f_traceSwitch.TraceInfo)
            {
                f_traceHelper.TraceInfo("runImportTask: Wired event handlers, about to call Run()..");
            }

            ImportOperationResult importResult = null;
            DialogResult actionDlgResult = DialogResult.None;

            try
            {
                importResult = f_wizardDeployment.RunImport();
            }
            catch (Exception e)
            {
                if (f_traceSwitch.TraceError)
                {
                    f_traceHelper.TraceError("runImportTask: Exception caught whilst running import: '{0}'.", e);
                }
            }

            if (importResult.Outcome == ResultType.Success)
            {
                actionDlgResult = MessageBox.Show(string.Format("Completed import of file '{0}'. The import log file will now be opened.",
                            f_wizardDeployment.ImportSettings.BaseFileName), "Result of import operation",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (importResult.Outcome == ResultType.Failure)
            {
                actionDlgResult = MessageBox.Show(string.Format("An error occurred whilst running the import. The import log file will now be " +
                    "opened. \n\nException details: \n\n{0}", importResult.Message),
                            "Import error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (actionDlgResult == DialogResult.OK)
            {
                openImportLogFile();
            }

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("runImportTask: Exiting runImportTask().");
            }
        }

        private void addExportObjects(XmlTextWriter xExportWriter)
        {
            xExportWriter.WriteStartElement("ExportObjects");

            foreach (ListViewItem deploymentItem in lstExportItems.Items)
            {
                SPObjectData deploymentObjectData = (SPObjectData)deploymentItem.Tag;

                xExportWriter.WriteStartElement("DeploymentObject");

                xExportWriter.WriteAttributeString("Id", deploymentObjectData.ID.ToString());
                xExportWriter.WriteAttributeString("Title", deploymentObjectData.Title);
                xExportWriter.WriteAttributeString("Url", deploymentObjectData.Url);
                xExportWriter.WriteAttributeString("Type", deploymentObjectData.ObjectType.ToString());
                xExportWriter.WriteAttributeString("IncludeDescendants", deploymentObjectData.IncludeDescendents.ToString());

                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo(string.Format("doExport: Added export item for '{0} ({1}).",
                       deploymentObjectData.Url, deploymentObjectData.ObjectType));
                }

                xExportWriter.WriteEndElement();
            }

            xExportWriter.WriteEndElement();
        }

        private void runExportTask()
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("runExportTask: Entered runExportTask().");
            }

            if (f_traceSwitch.TraceInfo)
            {
                f_traceHelper.TraceInfo("runExportTask: Initialising SPExport object with collected export settings.");
            }

            f_wizardDeployment.ProgressUpdated += new EventHandler<SPDeploymentEventArgs>(export_ProgressUpdated);
            f_wizardDeployment.Completed += new EventHandler<SPDeploymentEventArgs>(export_Completed);
            f_wizardDeployment.ValidChangeTokenNotFound += new EventHandler<InvalidChangeTokenEventArgs>(wizardDeployment_ValidChangeTokenNotFound);

            if (f_traceSwitch.TraceInfo)
            {
                f_traceHelper.TraceInfo("runExportTask: Wired event handlers, about to call Run()..");
            }

            DialogResult actionDlgResult;
            ExportOperationResult exportResult;

            try
            {
                exportResult = f_wizardDeployment.RunExport();

                if (f_traceSwitch.TraceInfo)
                {
                    string sMessage = (f_wizardDeployment.ExportSettings.ExportMethod == SPExportMethodType.ExportChanges) ?
                        string.Format("Incremental export completed successfully, change token is '{0}'.", exportResult.ChangeToken) :
                        "Full export completed successfully";
                    f_traceHelper.TraceInfo("runExportTask: {0}.", sMessage);
                }

                actionDlgResult = MessageBox.Show(string.Format("Completed export of file '{0}'! The export log file will now be opened.",
                    f_wizardDeployment.ExportSettings.BaseFileName), "Result of export operation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (actionDlgResult == DialogResult.OK)
                {
                    openExportLogFile();
                }
            }
            catch (Exception e)
            {
                if (f_traceSwitch.TraceError)
                {
                    f_traceHelper.TraceError("runExportTask: Exception caught whilst running export: '{0}'.", e);
                }

                string sMessage = ExceptionHelper.HandleDeploymentExportException(e, true);

                actionDlgResult = MessageBox.Show(sMessage,
                    "Export error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (actionDlgResult == DialogResult.OK)
                {
                    openExportLogFile();
                }
            }

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("runExportTask: Exiting runExportTask().");
            }
        }

        #endregion

        #region -- Import/Export UI handling --

        private void wizardDeployment_ValidChangeTokenNotFound(object sender, InvalidChangeTokenEventArgs e)
        {
            /* Theoretically this should never happen due to earlier validation. The only way this event will be raised is if a user has 
             * realised they can type into the comboboxes (a bug/design flaw!) and enters 'ExportChanges'.
             * We'll handle it anyway..
             */
            MessageBox.Show(
                    e.EventMessage,
                    "Unable to use incremental export", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        void import_ObjectImported(object sender, SPObjectImportedEventArgs e)
        {
            if (lblItemAction.InvokeRequired)
            {
                // COB 21 Mar 2010 - introduced try/catch here for strange threading bug..
                try
                {
                    updateOnItemImportedExported delUpdateLabel = new updateOnItemImportedExported(import_ObjectImported);
                    this.Invoke(delUpdateLabel, new object[] { lblItemAction, e });
                }
                catch (Exception)
                {
                }
            }
            else
            {
                lblItemAction.Text = getWrappedText("Importing " + e.TargetUrl, f_ciIMPORT_ITEM_ACTION_THRESHOLD);
            }
        }

        void import_Started(object sender, SPDeploymentEventArgs e)
        {
            setImportStarted(prgImport, e, ((WizardDeployment)sender).ImportSettings);
        }

        private void setImportStarted(ProgressBar prgBar, SPDeploymentEventArgs e, object data)
        {
            if (prgBar.InvokeRequired)
            {
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo(string.Format("setImportStarted: Invoke was required on progress bar named '{0}', " +
                        "about to invoke delegate.",
                        prgBar.Name));
                }

                // COB 21 Mar 2010 - introduced try/catch here for strange threading bug..
                try
                {
                    updateProgressOnDeployment delSetValue = new updateProgressOnDeployment(setImportStarted);
                    this.Invoke(delSetValue, new object[] { prgBar, e, data });
                }
                catch (Exception)
                {
                }
            }
            else
            {
                if (prgBar.Enabled == false)
                {
                    if (f_traceSwitch.TraceInfo)
                    {
                        f_traceHelper.TraceInfo(string.Format("** setImportStarted: Progress bar named '{0}', " +
                            "is currently disabled (indicating not yet been updated), about to enable and set Max Value to '{1}'.",
                            prgBar.Name, e.ObjectsTotal));
                    }
                    prgBar.Enabled = true;
                }

                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo(string.Format("** setImportStarted: About to update progress bar named '{0}' " +
                        "to value = '10' to indicate import started.",
                        prgBar.Name));
                }

                // patch by Alex Angas (@alexangas): Check if importing to list..
                SPImportSettings importSettings = (SPImportSettings)data;
                string webUrl = Microsoft.SharePoint.Utilities.SPEncode.UrlDecodeAsUrl(importSettings.WebUrl);
                if (webUrl.EndsWith("/"))
                {
                    webUrl = webUrl.Remove(webUrl.Length - 1, 1);
                }
                using (SPSite site = new SPSite(importSettings.SiteUrl))
                using (SPWeb web = site.OpenWeb(webUrl))
                {
                    SPList importList = null;
                    
                    try
                    {
                        importList = web.GetList(webUrl);
                    }
                    catch (FileNotFoundException)
                    {
                    }

                    if (importList != null)
                    {
                        // From Stefan Gossner: http://blogs.technet.com/stefan_gossner/archive/2007/08/30/deep-dive-into-the-sharepoint-content-deployment-and-migration-api-part-3.aspx
                        SPImportObjectCollection rootObjects = e.RootObjects;
                        foreach (SPImportObject io in rootObjects)
                        {
                            if (io.Type == SPDeploymentObjectType.ListItem)
                            {
                                io.TargetParentUrl = importList.RootFolder.ServerRelativeUrl;
                            }
                            if (io.Type == SPDeploymentObjectType.Folder)
                            {
                                io.TargetParentUrl = importList.RootFolder.ServerRelativeUrl;
                            }
                            if (io.Type == SPDeploymentObjectType.List)
                            {
                                io.TargetParentUrl = web.ServerRelativeUrl;
                            }
                        }
                        importSettings.WebUrl = importList.ParentWebUrl;
                    }
                }
                // end patch..

                prgBar.Minimum = 0;
                // since ObjectsTotal value doesn't seem to get correctly updated by Content Deployment framework, 
                // we'll leave at default of 100 of ObjectsTotal is still -1..
                if (e.ObjectsTotal != -1)
                {
                    prgBar.Maximum = e.ObjectsTotal;
                }
                prgBar.Value = 10;
            }
        }

        private void import_Completed(object sender, SPDeploymentEventArgs e)
        {
            setOperationCompleted(prgImport, this.wizardControl1);
        }

        private void import_ProgressUpdated(object sender, SPDeploymentEventArgs e)
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("import_ProgressUpdated: Entered ProgressUpdated event handler.");
            }

            setProgress(prgImport, e, null);

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("import_ProgressUpdated: Exiting ProgressUpdated event handler.");
            }
        }

        private string getDeploymentObjectsSummary(Hashtable htObjectCounts)
        {
            string sSummary = string.Empty;
            int iSiteCount = (int)htObjectCounts[SPDeploymentObjectType.Site];
            int iWebCount = (int)htObjectCounts[SPDeploymentObjectType.Web];
            int iListCount = (int)htObjectCounts[SPDeploymentObjectType.List];
            int iFolderCount = (int)htObjectCounts[SPDeploymentObjectType.Folder];
            int iListItemCount = (int)htObjectCounts[SPDeploymentObjectType.ListItem];
            StringBuilder sbSummary = new StringBuilder();

            if (iSiteCount > 0)
            {
                sSummary += (iSiteCount == 1) ? string.Format("{0} site", iSiteCount) : string.Format("{0} sites", iSiteCount);
            }
            if (iWebCount > 0)
            {
                if (sSummary.Length > 0)
                {
                    sSummary += ", ";
                }
                sSummary += (iWebCount == 1) ? string.Format("{0} web", iWebCount) : string.Format("{0} webs", iWebCount);
            }
            if (iListCount > 0)
            {
                if (sSummary.Length > 0)
                {
                    sSummary += ", ";
                }
                sSummary += (iListCount == 1) ? string.Format("{0} list", iListCount) : string.Format("{0} lists", iListCount);
            }
            if (iFolderCount > 0)
            {
                if (sSummary.Length > 0)
                {
                    sSummary += ", ";
                }
                sSummary += (iFolderCount == 1) ? string.Format("{0} folder", iFolderCount) : string.Format("{0} folders", iFolderCount);
            }
            if (iListItemCount > 0)
            {
                if (sSummary.Length > 0)
                {
                    sSummary += ", ";
                }
                sSummary += (iListItemCount == 1) ? string.Format("{0} list item", iListItemCount) : string.Format("{0} list items", iListItemCount);
            }
            return sSummary;
        }

        private string getExportObjectsSummary()
        {
            int iSiteCount = 0;
            int iWebCount = 0;
            int iListCount = 0;
            int iFolderCount = 0;
            int iListItemCount = 0;

            foreach (SPExportObject exportObject in f_wizardDeployment.ExportSettings.ExportObjects)
            {
                switch (exportObject.Type)
                {
                    case SPDeploymentObjectType.Site:
                        iSiteCount++;
                        break;
                    case SPDeploymentObjectType.Web:
                        iWebCount++;
                        break;
                    case SPDeploymentObjectType.List:
                        iListCount++;
                        break;
                    case SPDeploymentObjectType.Folder:
                        iFolderCount++;
                        break;
                    case SPDeploymentObjectType.ListItem:
                    case SPDeploymentObjectType.File:
                        iListItemCount++;
                        break;
                    default:
                        break;
                }
            }

            Hashtable htObjectCounts = new Hashtable();
            htObjectCounts.Add(SPDeploymentObjectType.Site, iSiteCount);
            htObjectCounts.Add(SPDeploymentObjectType.Web, iWebCount);
            htObjectCounts.Add(SPDeploymentObjectType.Folder, iFolderCount);
            htObjectCounts.Add(SPDeploymentObjectType.ListItem, iListItemCount);
            htObjectCounts.Add(SPDeploymentObjectType.List, iListCount);

            return getDeploymentObjectsSummary(htObjectCounts);
        }

        private void displayExportSettings()
        {
            pnlExportSettings.Visible = true;
            pnlImportSettings.Visible = false;

            string sExportLogFilePath = getWrappedText(f_wizardDeployment.ExportSettings.LogFilePath, f_cFINISH_SCREEN_WRAP_THRESHOLD);
            string sFullExportPath = string.Format("{0}\\{1}",
                f_wizardDeployment.ExportSettings.FileLocation, f_wizardDeployment.ExportSettings.BaseFileName);
            sFullExportPath = getWrappedText(sFullExportPath, f_cFINISH_SCREEN_WRAP_THRESHOLD);
            decimal dblNumExportPathLines = decimal.Divide(sFullExportPath.Length, f_cFINISH_SCREEN_WRAP_THRESHOLD);
            int iNumExportPathLines = Convert.ToInt32(Math.Ceiling(dblNumExportPathLines));
            if (iNumExportPathLines > 1)
            {
                // move these labels down slightly for better formatting..
                lblExportLogFilePath.Location = new Point(lblExportLogFilePath.Location.X,
                    lblExportLogFilePath.Location.Y + (iNumExportPathLines * 8));
                lblExportLogFilePathValue.Location = new Point(lblExportLogFilePathValue.Location.X,
                   lblExportLogFilePathValue.Location.Y + (iNumExportPathLines * 8));
            }

            lblExportSiteValue.Text = f_wizardDeployment.ExportSettings.SiteUrl;
            lblExportObjectsValue.Text = getExportObjectsSummary();
            lblIncludeSecurityExportValue.Text = f_wizardDeployment.ExportSettings.IncludeSecurity.ToString();
            lblIncludeVersionsExportValue.Text = f_wizardDeployment.ExportSettings.IncludeVersions.ToString();
            lblExportMethodValue.Text = f_wizardDeployment.ExportSettings.ExportMethod.ToString();
            lblExportPathValue.Text = sFullExportPath;
            lblExportLogFilePathValue.Text = sExportLogFilePath;
        }

        private void displayImportSettings()
        {
            pnlExportSettings.Visible = false;
            pnlImportSettings.Visible = true;

            string sFullImportPath = string.Format("{0}\\{1}",
                f_wizardDeployment.ImportSettings.FileLocation, f_wizardDeployment.ImportSettings.BaseFileName);
            sFullImportPath = getWrappedText(sFullImportPath, f_cFINISH_SCREEN_WRAP_THRESHOLD);
            string sImportLogFilePath = getWrappedText(f_wizardDeployment.ImportSettings.LogFilePath, f_cFINISH_SCREEN_WRAP_THRESHOLD);

            decimal dblNumImportPathLines = decimal.Divide(sFullImportPath.Length, f_cFINISH_SCREEN_WRAP_THRESHOLD);
            int iNumImportPathLines = Convert.ToInt32(Math.Ceiling(dblNumImportPathLines));
            if (iNumImportPathLines > 1)
            {
                // move these labels down slightly for better formatting..
                lblImportLogFilePathFinish.Location = new Point(lblImportLogFilePathFinish.Location.X,
                    lblImportLogFilePathFinish.Location.Y + (iNumImportPathLines * 8));
                lblImportLogFilePathValueFinish.Location = new Point(lblImportLogFilePathValueFinish.Location.X,
                   lblImportLogFilePathValueFinish.Location.Y + (iNumImportPathLines * 8));
            }

            lblImportSiteValue.Text = f_wizardDeployment.ImportSettings.SiteUrl;
            lblImportObjectsValue.Text = "<All contents of package>";
            lblIncludeSecurityImportValue.Text = f_wizardDeployment.ImportSettings.IncludeSecurity.ToString();
            lblRetainObjectIdentityValue.Text = f_wizardDeployment.ImportSettings.RetainObjectIdentity.ToString();
            lblUpdateVersionsValue.Text = f_wizardDeployment.ImportSettings.UpdateVersions.ToString();
            lblImportPathValueFinish.Text = sFullImportPath;
            lblImportLogFilePathValueFinish.Text = sImportLogFilePath;
        }

        private void collectSiteDetails()
        {
            f_sSiteUrl = txtSiteUrl.Text;
        }

        void export_ProgressUpdated(object sender, SPDeploymentEventArgs e)
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("export_ProgressUpdated: Entered ProgressUpdated event handler.");
            }

            setProgress(prgExport, e, null);

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("export_ProgressUpdated: Exiting ProgressUpdated event handler.");
            }
        }

        void export_Completed(object sender, SPDeploymentEventArgs e)
        {
            setOperationCompleted(prgExport, this.wizardControl1);
        }

        private void setOperationCompleted(ProgressBar prgBar, WizardControl wizard)
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose(string.Format("setOperationCompleted: Entered setOperationCompleted " +
                    "with progress bar '{0}' and wizard instance.",
                    prgBar.Name));
            }

            // only need to check one of the controls..
            if (prgBar.InvokeRequired)
            {
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo(string.Format("setOperationCompleted: Invoke was required on progress bar '{0}', " +
                        "about to invoke delegate.",
                        prgBar.Name));
                }

                updateOnDeploymentComplete delSetCompleted = new updateOnDeploymentComplete(setOperationCompleted);
                this.Invoke(delSetCompleted, new object[] { prgBar, wizard });
            }
            else
            {
                prgBar.Value = prgBar.Maximum;
                wizardControl1.CancelButtonEnabled = true;
                lblItemAction.Visible = false;
                prgBar.Value = 0;
                btnReturnToStart.Visible = true;
                lblExportLogFilePathValue.Enabled = true;
                lblExportPathValue.Enabled = true;
                lblImportPathValueFinish.Enabled = true;
                lblImportLogFilePathValueFinish.Enabled = true;

                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo(string.Format("setOperationCompleted: Filled progress bar '{0}' " +
                        "and set button state of wizard.", prgBar.Name));
                }
            }

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("setOperationCompleted: Exiting setOperationCompleted.");
            }
        }

        private void setProgress(ProgressBar prgBar, SPDeploymentEventArgs e, object data)
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose(string.Format("setProgress: Entered setProgress with progress bar named '{0}', " +
                    "items processed '{1}'. Thread ID is '{2}'.",
                    prgBar.Name, e.ObjectsProcessed, Thread.CurrentThread.ManagedThreadId));
            }

            if (prgBar.InvokeRequired)
            {
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo(string.Format("setProgress: Invoke was required on progress bar named '{0}', " +
                        "about to invoke delegate.",
                        prgBar.Name));
                }

                updateProgressOnDeployment delSetValue = new updateProgressOnDeployment(setProgress);
                this.Invoke(delSetValue, new object[] { prgBar, e, data });
            }
            else
            {
                if (prgBar.Enabled == false)
                {
                    if (f_traceSwitch.TraceInfo)
                    {
                        f_traceHelper.TraceInfo(string.Format("setProgress: Progress bar named '{0}', " +
                            "is currently disabled (indicating not yet been updated), about to enable and set Max Value to '{1}'.",
                            prgBar.Name, e.ObjectsTotal));
                    }
                    prgBar.Enabled = true;
                    prgBar.Visible = true;
                    prgBar.Minimum = 0;
                    // since ObjectsTotal value doesn't seem to get correctly updated by Content Deployment framework, 
                    // we'll leave at default of 100 if ObjectsTotal is still -1..
                    if (e.ObjectsTotal != -1)
                    {
                        prgBar.Maximum = e.ObjectsTotal;
                    }

                    if (e.ObjectsTotal < prgBar.Step)
                    {
                        prgBar.Step = Convert.ToInt32(e.ObjectsTotal / 10);
                    }
                }

                // if progress updated event has fired but ObjectsProcessed count is still 0, 
                // move the progress bar one step otherwise looks like jobs has failed to start properly..
                if (e.ObjectsProcessed == 0)
                {
                    prgBar.Value = prgBar.Step;
                }
                else
                {
                    // since ObjectsTotal value doesn't seem to get correctly updated by Content Deployment framework, 
                    // we'll leave at default of 100 of ObjectsTotal is still -1..
                    if (e.ObjectsTotal != -1)
                    {
                        prgBar.Maximum = e.ObjectsTotal;
                    }

                    /* also need to increase maximum value of ProgressBar here if we've gone past default of 100..
                     * this happens because the Microsoft API has a bug where ObjectsTotal is -1 for imports..
                     */
                    if (e.ObjectsProcessed > prgBar.Maximum)
                    {
                        prgBar.Maximum = e.ObjectsProcessed;
                    }

                    int iCheck = e.RootObjects.Count;

                    prgBar.Value = e.ObjectsProcessed;
                }

                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo(string.Format("setProgress: Updated value on progress bar named '{0}', " +
                        "to '{1}'.",
                        prgBar.Name, e.ObjectsProcessed));
                }
            }

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("setProgress: Exiting setProgress.");
            }
        }

        #endregion

        #region -- Control event handlers --

        void ctxtMenuExportItem_Click(object sender, EventArgs e)
        {
            if (lstExportItems.SelectedItems.Count == 1)
            {
                lstExportItems.SelectedItems[0].Remove();
            }
        }

        void lstExportItems_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && lstExportItems.SelectedItems.Count == 1)
            {
                ctxtMenuExportItem.Show(frmContentDeployer.MousePosition);
            }
        }

        void wizardControl1_CancelButtonClick(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// Handle step changed event of wizard control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void wizardControl1_CurrentStepIndexChanged(object sender, EventArgs e)
        {
            switch (wizardControl1.CurrentStepIndex)
            {
                case (f_ciWELCOME_STEP_INDEX):
                    if (wizardControl1.BackButtonEnabled == false)
                    {
                        wizardControl1.BackButtonEnabled = true;
                        wizardControl1.NextButtonEnabled = true;
                    }
                    break;
                case (f_ciFINISH_STEP_INDEX):
                    btnReturnToStart.Visible = false;
                    lblExportLogFilePathValue.Enabled = false;
                    lblExportPathValue.Enabled = false;
                    lblImportPathValueFinish.Enabled = false;
                    lblImportLogFilePathValueFinish.Enabled = false;

                    if (f_action == WizardAction.Export)
                    {
                        using (MemoryStream exportMemStream = collectExportSettings())
                        {
                            exportMemStream.Position = 0;

                            // pass settings to deployment class..
                            f_wizardDeployment = new WizardDeployment(new XmlTextReader(exportMemStream),
                                                                      DeploymentType.Export);

                            displayExportSettings();
                        }
                    }
                    else if (f_action == WizardAction.Import)
                    {
                        using (MemoryStream importMemStream = collectImportSettings())
                        {
                            importMemStream.Position = 0;

                            // pass settings to deployment class..
                            f_wizardDeployment = new WizardDeployment(new XmlTextReader(importMemStream),
                                                                      DeploymentType.Import);

                            displayImportSettings();
                        }
                    }

                    break;
                case (f_ciEXPORT_SETTINGS_STEP_INDEX):
                    // need to dispose of all SPWeb references used to build TreeView..
                    f_iDisposedWebsFromMenuCount = 0;
                    f_iDisposedWebsCount = 0;
                    disposeWebs(trvContent.Nodes[0], true);
                    break;
                default:

                    break;
            }
        }

        void wizardControl1_NextButtonClick(WizardBase.WizardControl sender, WizardBase.WizardNextButtonClickEventArgs args)
        {
            switch (wizardControl1.CurrentStepIndex)
            {
                case f_ciSITE_DETAILS_STEP_INDEX:
                    Cursor = Cursors.WaitCursor;
                    if (validateUserParams(f_ciSITE_DETAILS_STEP_INDEX))
                    {
                        if (rdoExport.Checked)
                        {
                            f_action = WizardAction.Export;

                            m_bindingMsgForm.Show(this);
                            m_bindingMsgForm.Refresh();

                            // bind treeview..
                            using (SPSite exportSite = new SPSite(f_sSiteUrl))
                            {
                                doSiteTreeBind(exportSite);
                            }

                            OnSiteBindComplete();
                            if (f_exportSettings != null)
                            {
                                addSavedExportItemsToExportListView(f_exportSettings);
                            }
                            args.NextStepIndex = f_ciEXPORT_SELECT_STEP_INDEX;
                        }
                        else
                        {
                            f_action = WizardAction.Import;
                            args.NextStepIndex = f_ciIMPORT_STEP_INDEX;
                        }
                    }
                    else
                    {
                        args.Cancel = true;
                    }
                    Cursor = Cursors.Default;
                    break;
                case f_ciIMPORT_STEP_INDEX:
                    if (validateUserParams(f_ciIMPORT_STEP_INDEX))
                    {
                        args.NextStepIndex = f_ciFINISH_STEP_INDEX;
                    }
                    else
                    {
                        args.Cancel = true;
                    }
                    break;
                case f_ciEXPORT_SELECT_STEP_INDEX:
                    if (validateUserParams(f_ciEXPORT_SELECT_STEP_INDEX))
                    {
                        args.NextStepIndex = f_ciEXPORT_SETTINGS_STEP_INDEX;
                    }
                    else
                    {
                        args.Cancel = true;
                    }
                    break;
                case f_ciEXPORT_SETTINGS_STEP_INDEX:
                    // clear down saved settings from file in case user uses back button..
                    f_exportSettings = null;
                    if (validateUserParams(f_ciEXPORT_SETTINGS_STEP_INDEX))
                    {
                        args.NextStepIndex = f_ciFINISH_STEP_INDEX;
                    }
                    else
                    {
                        args.Cancel = true;
                    }
                    break;
                default:
                    break;
            }
        }

        void wizardControl1_FinishButtonClick(object sender, EventArgs e)
        {
            switch (f_action)
            {
                case WizardAction.Export:
                    // reset progress bar from last run..
                    prgExport.Value = 0;
                    pnlProgress.Controls.Add(prgExport);
                    prgExport.Visible = true;
                    prgImport.Visible = false;
                    doExport();
                    break;
                case WizardAction.Import:
                    DialogResult result = MessageBox.Show(string.Format("You are about to import over your existing database - it is recommended " +
                        "you take a backup before proceeding.{0}{0}Press OK to continue, or Cancel to pause the wizard at this stage.", Environment.NewLine),
                         "Caution", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.OK)
                    {
                        prgImport.Value = 0;
                        pnlProgress.Controls.Add(prgImport);
                        prgImport.Visible = true;
                        prgExport.Visible = false;
                        doImport();
                    }
                    break;
                case WizardAction.None:
                    // cannot set next index step since standard event args is passed
                    break;
                default:
                    break;
            }
        }

        private void btnFilePathBrowse_Click(object sender, EventArgs e)
        {
            f_dlgSelectExportFolder.RootFolder = Environment.SpecialFolder.MyComputer;
            if (f_dlgSelectExportFolder.ShowDialog() == DialogResult.OK)
            {
                f_sExportFolderPath = f_dlgSelectExportFolder.SelectedPath;
                lblExportFolderValue.Text = getWrappedText(f_sExportFolderPath, f_ciEXPORT_FOLDER_PATH_WRAP_THRESHOLD);
                if (!string.IsNullOrEmpty(f_sExportBaseFilename))
                {
                    string sLogFilePath = f_sExportFolderPath + "\\" + f_sExportBaseFilename + ".log";
                    lblExportLogPathValue.Text = getWrappedText(sLogFilePath, f_ciEXPORT_FOLDER_PATH_WRAP_THRESHOLD);
                }
            }
        }

        private void btnImportPathBrowse_Click(object sender, EventArgs e)
        {
            f_dlgSelectImportSingleFile.FileName = string.Empty;
            f_dlgSelectImportSingleFile.Filter = @"Content Migration Package (*.cmp)|*.cmp";
            if (f_dlgSelectImportSingleFile.ShowDialog() == DialogResult.OK)
            {
                string sImportSingleFilePath = f_dlgSelectImportSingleFile.FileName;
                string sWrappedImportSingleFilePath = getWrappedText(sImportSingleFilePath, f_ciIMPORT_FILE_PATH_WRAP_THRESHOLD);
                lblImportPathValue.Text = sWrappedImportSingleFilePath;

                // set-up import log path..
                string sImportSingleFilePathNoExt = sImportSingleFilePath.Substring(0, sImportSingleFilePath.LastIndexOf("."));

                lblImportLogfilePathValue.Text = (getWrappedText(sImportSingleFilePathNoExt + ".Import.log", f_ciIMPORT_LOG_FILE_PATH_WRAP_THRESHOLD));
            }
        }

        private void txtExportBaseFilename_TextChanged(object sender, EventArgs e)
        {
            string sExportFolderPath = lblExportFolderValue.Text;
            if (!string.IsNullOrEmpty(sExportFolderPath))
            {
                string sExportFilePath = sExportFolderPath + "\\" + txtExportBaseFilename.Text + ".cmp";
                string sExportLogFilePath = sExportFolderPath + "\\" + txtExportBaseFilename.Text + ".Export.log";
                lblExportBaseFilenameValue.Text = getWrappedText(sExportFilePath, f_ciEXPORT_FILE_PATH_WRAP_THRESHOLD);
                lblExportLogPathValue.Text = getWrappedText(sExportLogFilePath, f_ciEXPORT_LOG_FILE_PATH_WRAP_THRESHOLD);
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAbout aboutForm = new frmAbout();
            aboutForm.Show(this);
            // centerFormTo(aboutForm, this);
        }

        private void btnReturnToStart_Click(object sender, EventArgs e)
        {
            wizardControl1.CurrentStepIndex = f_ciWELCOME_STEP_INDEX;
            btnReturnToStart.Visible = false;
        }

        private void rdoImport_CheckedChanged(object sender, EventArgs e)
        {
            txtWebUrl.Enabled = rdoImport.Checked;
            lblWebUrl.Enabled = rdoImport.Checked;
        }

        private void lblExportPathValue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string sPath = f_wizardDeployment.ExportSettings.FileLocation;
            try
            {
                Process.Start("explorer", sPath);
            }
            catch (Exception exc)
            {
                if (f_traceSwitch.TraceWarning)
                {
                    f_traceHelper.TraceWarning("lblExportPathValue_LinkClicked: Caught exception when attempting " +
                        "to open export Windows Explorer. Exception '{0}'.", exc);
                }
                MessageBox.Show("Unable to Windows Explorer from Wizard. Please browse to the following location manually:\n\n" +
                    sPath, "Internal error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lblExportLogFilePathValue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openExportLogFile();
        }

        private void openExportLogFile()
        {
            string sPath = f_wizardDeployment.ExportSettings.LogFilePath;

            try
            {
                Process.Start(sPath);
            }
            catch (Exception exc)
            {
                if (f_traceSwitch.TraceWarning)
                {
                    f_traceHelper.TraceWarning("lblExportLogFilePathValue_LinkClicked: Caught exception when attempting " +
                        "to open export log file. Exception '{0}'.", exc);
                }
                MessageBox.Show("Unable to open log file from Wizard. Please open the following file manually:\n\n" +
                    sPath, "Internal error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lblImportPathValueFinish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string sPath = f_wizardDeployment.ImportSettings.FileLocation;
            try
            {
                Process.Start("explorer", sPath);
            }
            catch (Exception exc)
            {
                if (f_traceSwitch.TraceWarning)
                {
                    f_traceHelper.TraceWarning("lblImportPathValueFinish_LinkClicked: Caught exception when attempting " +
                        "to open Windows Explorer. Exception '{0}'.", exc);
                }
                MessageBox.Show("Unable to Windows Explorer from Wizard. Please browse to the following location manually:\n\n" +
                    sPath, "Internal error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lblImportLogFilePathValueFinish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openImportLogFile();
        }

        private void openImportLogFile()
        {
            string sPath = f_wizardDeployment.ImportSettings.LogFilePath;

            try
            {
                Process.Start(sPath);
            }
            catch (Exception exc)
            {
                if (f_traceSwitch.TraceWarning)
                {
                    f_traceHelper.TraceWarning("lblImportLogFilePathValueFinish_LinkClicked: Caught exception when attempting " +
                        "to open import log file. Exception '{0}'.", exc);
                }
                MessageBox.Show("Unable to open log file from Wizard. Please open the following file manually:\n\n" +
                    sPath, "Internal error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("btnSaveSettings_Click: Entered.");
            }

            MemoryStream settingsStream = null;

            if (f_wizardDeployment.Type == DeploymentType.Export)
            {
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo("btnSaveSettings_Click: Collecting export settings.");
                }
                settingsStream = collectExportSettings();
            }
            if (f_wizardDeployment.Type == DeploymentType.Import)
            {
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo("btnSaveSettings_Click: Collecting import settings.");
                }
                settingsStream = collectImportSettings();
            }

            settingsStream.Position = 0;
            string sPath = string.Empty;

            if (f_dlgSaveSettingsFile.ShowDialog() == DialogResult.OK)
            {
                sPath = f_dlgSaveSettingsFile.FileName;
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo("btnSaveSettings_Click: Settings will be saved to '{0}'.", sPath);
                }

                using (FileStream fs = new FileStream(sPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    try
                    {
                        settingsStream.WriteTo(fs);
                        MessageBox.Show(
                            string.Format("{0} settings saved successfully to '{1}'", f_wizardDeployment.Type, sPath),
                            "File saved",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception exc)
                    {
                        string sMessage = string.Format("An error occurred saving {0} settings to location '{1}'.",
                                                        f_wizardDeployment.Type.ToString().ToLower(), sPath);
                        if (f_traceSwitch.TraceWarning)
                        {
                            f_traceHelper.TraceWarning("btnSaveSettings_Click: {0}");
                        }
                        MessageBox.Show(string.Format("{0}. Error message '{1}'.",
                                sMessage, exc.Message),
                                "Error saving file!",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo("btnSaveSettings_Click: Not saving file, user cancelled out of dialog.");
                }
            }

            settingsStream.Dispose();

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("btnSaveSettings_Click: Leaving.");
            }
        }

        private void btnLoadSettings_Click(object sender, EventArgs e)
        {

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("btnLoadSettings_Click: Entered.");
            }

            string sPath = string.Empty;

            if (f_dlgLoadSettingsFile.ShowDialog() == DialogResult.OK)
            {
                sPath = f_dlgLoadSettingsFile.FileName;
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo("btnLoadLoadingSettings_Click: Loading saved settings from '{0}'.", sPath);
                }

                // determine whether we are loading import or export settings, then get SPImportSettings/SPExportSettings, then set controls..

                Byte[] fileBytes = null;
                XmlTextReader xReader = null;
                using (xReader = new XmlTextReader(sPath))
                {
                    xReader.WhitespaceHandling = WhitespaceHandling.None;
                    xReader.MoveToContent();
                    string sXml = xReader.ReadOuterXml();
                    UTF8Encoding encoding = new UTF8Encoding();
                    fileBytes = encoding.GetBytes(sXml);
                }

                WizardOperationSettings settings = null;
                using (MemoryStream settingsMemStream = new MemoryStream(fileBytes))
                {
                    settingsMemStream.Position = 0;

                    using (xReader = new XmlTextReader(settingsMemStream))
                    {
                        settings = WizardDeployment.CollectSettings(xReader);
                    }
                }

                if (settings != null)
                {
                    if (settings is WizardExportSettings)
                    {
                        WizardExportSettings weSettings = (WizardExportSettings)settings;
                        loadSettings(weSettings);
                        f_exportSettings = (SPExportSettings)settings.Settings;
                        f_supplementaryExportSettings = weSettings;
                    }
                    if (settings is WizardImportSettings)
                    {
                        WizardImportSettings wiSettings = (WizardImportSettings)settings;
                        loadSettings(wiSettings);
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Unable to load settings from this file! XML root node was not of type 'ImportSettings' or 'ExportSettings.",
                        "Error loading settings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo("btnLoadSettings_Click: Not loading file, user cancelled out of dialog.");
                }
            }

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("btnLoadSettings_Click: Leaving.");
            }
        }

        private void loadSettings(WizardExportSettings weSettings)
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("loadSettings[SPExportSettings]: Entered.");
            }

            SPExportSettings exportSettings = (SPExportSettings)weSettings.Settings;

            rdoExport.Checked = true;
            txtSiteUrl.Text = exportSettings.SiteUrl;
            chkExcludeDependencies.Checked = exportSettings.ExcludeDependencies;

            cboExportMethod.SelectedIndex = cboExportMethod.FindStringExact(exportSettings.ExportMethod.ToString());
            cboIncludeVersions.SelectedIndex = cboIncludeVersions.FindStringExact(exportSettings.IncludeVersions.ToString());
            cboIncludeSecurity.SelectedIndex = cboIncludeSecurity.FindStringExact(exportSettings.IncludeSecurity.ToString());
            lblExportFolderValue.Text = exportSettings.FileLocation;
            txtExportBaseFilename.Text = exportSettings.BaseFileName;

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("loadSettings[SPExportSettings]: Leaving.");
            }
        }

        private void loadSettings(WizardImportSettings wiSettings)
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("loadSettings[SPImportSettings]: Entered.");
            }

            SPImportSettings importSettings = (SPImportSettings)wiSettings.Settings;

            rdoImport.Checked = true;
            txtSiteUrl.Text = importSettings.SiteUrl;
            txtWebUrl.Text = importSettings.WebUrl;
            lblImportPathValue.Text = string.Format("{0}\\{1}", importSettings.FileLocation, importSettings.BaseFileName);
            chkRetainIDs.Checked = importSettings.RetainObjectIdentity;
            cboIncludeSecurityImport.SelectedIndex =
                cboIncludeSecurityImport.FindStringExact(importSettings.IncludeSecurity.ToString());
            cboUserInfo.SelectedIndex = cboUserInfo.FindStringExact(importSettings.UserInfoDateTime.ToString());
            cboVersionOptions.SelectedIndex = cboVersionOptions.FindStringExact(importSettings.UpdateVersions.ToString());

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("loadSettings[SPImportSettings]: Leaving.");
            }
        }

        private void addSavedExportItemsToExportListView(SPExportSettings exportSettings)
        {
            lstExportItems.Items.Clear();

            List<SPObjectData> exportObjects = f_supplementaryExportSettings.SupplementaryData;
            foreach (SPObjectData exportObject in exportObjects)
            {
                lstExportItems.Items.Add(getListViewItemFromExportObject(exportObject));
            }
        }

        #endregion

        #region -- Private helper methods --

        private string getWrappedText(string sText, int iWrapThreshold)
        {
            sText = sText.Replace(Environment.NewLine, string.Empty);
            int iLineCount = sText.Length / iWrapThreshold;
            int iInsertLineBreakPosition = 0;

            for (int iCount = 0; iCount < iLineCount; iCount++)
            {
                iInsertLineBreakPosition = sText.LastIndexOf("\\", iWrapThreshold * (iCount + 1)) + 1;
                sText = sText.Insert(iInsertLineBreakPosition, Environment.NewLine);
            }

            return sText;
        }

        private string getListFriendlyName(SPList list, bool bFullName)
        {
            string sDefaultViewUrl = list.DefaultViewUrl;
            string sListUrl = list.Title;

            if (bFullName)
            {
                // build absolute URL..
                using (SPWeb parentWeb = list.ParentWeb)
                {
                    sListUrl = string.Format("{0}/{1}",
                        parentWeb.Url, sListUrl);
                }
            }

            return sListUrl;
        }

        private SPObjectData getObjectData(SPSite site)
        {
            return new SPObjectData(site.ID, site.PortalName, site.Url, SPDeploymentObjectType.Site, SPIncludeDescendants.All);
        }

        private SPObjectData getObjectData(SPWeb web)
        {
            return new SPObjectData(web.ID, web.Title, web.Url, SPDeploymentObjectType.Web,
                SPIncludeDescendants.All, web);
        }

        private SPObjectData getObjectData(SPList list)
        {
            return new SPObjectData(list.ID, list.Title, getListFriendlyName(list, true), SPDeploymentObjectType.List,
                SPIncludeDescendants.All, list.ParentWeb);
        }

        private SPObjectData getObjectData(SPListItem listItem)
        {
            SPObjectData listItemData = null;
            SPDeploymentObjectType objectType;
            string sListItemText = null;

            // also add file ID here for later processing for ListItems which have files..
            if (listItem.File != null)
            {
                using (SPWeb listParentWeb = listItem.Web)
                {
                    sListItemText = string.Format("{0}/{1}", listParentWeb.Url, listItem.Url);
                    objectType = SPDeploymentObjectType.File;
                }
            }
            else
            {
                sListItemText = string.Format("{0}/{1}", getListFriendlyName(listItem.ParentList, true), listItem.Name);

                if (listItem.Folder != null)
                {
                    objectType = SPDeploymentObjectType.Folder;
                }
                else
                {
                    objectType = SPDeploymentObjectType.ListItem;
                }
            }

            listItemData = new SPObjectData(listItem.UniqueId, sListItemText, listItem.Url,
                    objectType, SPIncludeDescendants.All);
            return listItemData;
        }

        private bool listIsContent(SPList list)
        {
            string sListTitle = list.Title;

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("listIsContent: Entered listIsContent with list '{0}'.", sListTitle);
            }

            bool bListIsContent = true;

            for (int iCount = 0; iCount < f_aListsNotForExport.Length; iCount++)
            {
                if (f_aListsNotForExport[iCount].Trim() == sListTitle)
                {
                    if (f_traceSwitch.TraceInfo)
                    {
                        f_traceHelper.TraceInfo("listIsContent: List '{0}' was found in lists to hide from treeview, this list will not be shown.", sListTitle);
                    }
                    bListIsContent = false;
                    break;
                }
            }

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("listIsContent: Returning '{0}'.", bListIsContent);
            }

            return bListIsContent;
        }

        private string[] fetchListsNotForExport()
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("fetchListsNotForExport: Entered fetchListsNotForExport().");
            }

            string sListsNotForExport = ConfigurationManager.AppSettings["ListsNotForExport"];

            if (string.IsNullOrEmpty(sListsNotForExport))
            {
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo("fetchListsNotForExport: No value specified in config for key 'ListsNotForExport' - this value " +
                        "specifies system lists to hide in treeview. Using default of '{0}'.", f_csDEFAULT_LISTS_NOT_FOR_EXPORT);
                }

                sListsNotForExport = f_csDEFAULT_LISTS_NOT_FOR_EXPORT;
            }
            else
            {
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo("fetchListsNotForExport: Found override value in config for lists to hide in treeview - " +
                        "the following lists will not be shown in treeview '{0}'.", sListsNotForExport);
                }
            }

            string[] aListsNotForExport = sListsNotForExport.Split(',');

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("performDataBinding: Returning string array with '{0}' items.", aListsNotForExport.Length);
            }

            return aListsNotForExport;
        }

        private bool canExportContentOnly(SPObjectData exportObject)
        {
            return (exportObject.ObjectType == SPDeploymentObjectType.Site ||
                exportObject.ObjectType == SPDeploymentObjectType.Web);
        }

        private bool exportObjectCanHaveChildren(SPObjectData exportObject)
        {
            return !(exportObject.ObjectType == SPDeploymentObjectType.File ||
                exportObject.ObjectType == SPDeploymentObjectType.ListItem);
        }

        #endregion

        #region -- Disposals --

        private void disposeWebs(TreeNode node, bool bRecurse)
        {
            SPObjectData objectData = (SPObjectData)node.Tag;
            disposeAssociatedWeb(objectData, false);

            if (node.ContextMenuStrip != null)
            {
                foreach (ToolStripMenuItem menuItem in node.ContextMenuStrip.Items)
                {
                    objectData = (SPObjectData)menuItem.Tag;
                    disposeAssociatedWeb(objectData, true);

                    foreach (ToolStripMenuItem childMenuItem in menuItem.DropDown.Items)
                    {
                        objectData = (SPObjectData)childMenuItem.Tag;
                        disposeAssociatedWeb(objectData, true);
                    }
                }
            }

            if (bRecurse)
            {
                foreach (TreeNode childNode in node.Nodes)
                {
                    disposeWebs(childNode, bRecurse);
                }
            }
        }

        private void disposeAssociatedWeb(SPObjectData objectData, bool bFromContextMenu)
        {
            if (objectData != null)
            {
                if (objectData.Web != null)
                {
                    objectData.Web.Dispose();
                    objectData.Web = null;
                    if (bFromContextMenu)
                    {
                        f_iDisposedWebsFromMenuCount++;
                    }
                    else
                    {
                        f_iDisposedWebsCount++;
                    }
                }
            }
        }

        #endregion

        #region -- Validation --

        private bool validateUserParams(int iStepIndex)
        {
            bool bValidated = false;

            switch (iStepIndex)
            {
                case f_ciSITE_DETAILS_STEP_INDEX:
                    if (validateSiteDetails())
                    {
                        if (!rdoImport.Checked && !rdoExport.Checked)
                        {
                            showValidationError("Please specify if you wish to import or export");
                        }
                        else
                        {
                            bValidated = true;
                        }
                    }
                    break;
                case f_ciIMPORT_STEP_INDEX:
                    if (string.IsNullOrEmpty(lblImportPathValue.Text))
                    {
                        showValidationError("Please specify the file or files you wish to import");
                    }
                    else
                    {
                        bValidated = true;
                    }
                    break;
                case f_ciEXPORT_SELECT_STEP_INDEX:
                    if (lstExportItems.Items.Count == 0)
                    {
                        showValidationError("Please select at least one item for export");
                    }
                    else
                    {
                        bValidated = true;
                    }
                    break;
                case f_ciEXPORT_SETTINGS_STEP_INDEX:
                    if (string.IsNullOrEmpty(lblExportFolderValue.Text))
                    {
                        showValidationError("Please browse to the folder you wish the export file to be saved to", btnFilePathBrowse);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txtExportBaseFilename.Text))
                        {
                            showValidationError("Please enter the base filename for the export file", txtExportBaseFilename);
                        }
                        else
                        {
                            bValidated = true;
                        }
                    }
                    break;
            }

            return bValidated;
        }

        private bool validateSiteDetails()
        {
            collectSiteDetails();

            bool bValidated = false;

            if (string.IsNullOrEmpty(f_sSiteUrl))
            {
                showValidationError("Please ensure the site URL is entered correctly");
            }
            else
            {
                bValidated = true;
            }

            if (bValidated)
            {
                bValidated = authenticateAgainstSite();
            }

            return bValidated;
        }

        // TODO: consider moving this code be moved to WizardDeployment so STSADM command also benefits from it?
        private bool authenticateAgainstSite()
        {
            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("authenticateAgainstSite: Entered authenticateAgainstSite().");
            }

            bool bAuthed = false;
            // create SPSite and dispose of it immediately, we just want to check the site is valid..
            SPSite siteForTest = null;

            try
            {
                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo("authenticateAgainstSite: Initialising SPSite object with URL '{0}'.",
                        f_sSiteUrl);
                }

                siteForTest = new SPSite(f_sSiteUrl);

                if (f_traceSwitch.TraceInfo)
                {
                    f_traceHelper.TraceInfo("authenticateAgainstSite: Successfully initialised, setting bAuthed to true.",
                        f_sSiteUrl);
                }

                bAuthed = true;
            }
            catch (FileNotFoundException noFileExc)
            {
                if (f_traceSwitch.TraceWarning)
                {
                    f_traceHelper.TraceWarning("authenticateAgainstSite: Caught FileNotFoundException, indicates unable " +
                        "to find site with this URL - will show validation error MessageBox and set bAuthed to false. Exception details: '{0}'.",
                        noFileExc);
                }

                showValidationError(string.Format("Unable to contact site at address '{0}', please check the URL.",
                    txtSiteUrl.Text), txtSiteUrl);
            }
            catch (UriFormatException uriExc)
            {
                if (f_traceSwitch.TraceWarning)
                {
                    f_traceHelper.TraceWarning("authenticateAgainstSite: Caught UriFormatException, indicates format " +
                        "of URL is invalid - will show validation error MessageBox and set bAuthed to false. Exception details: '{0}'.",
                        uriExc);
                }

                showValidationError("There is a problem with the format of the URL, please check.", txtSiteUrl);
            }
            finally
            {
                if (siteForTest != null)
                {
                    siteForTest.Dispose();
                }
            }

            if (f_traceSwitch.TraceVerbose)
            {
                f_traceHelper.TraceVerbose("authenticateAgainstSite: Returning '{0}'.",
                    bAuthed);
            }

            return bAuthed;
        }

        private void showValidationError(string sMessage)
        {
            showValidationError(sMessage, null);
        }

        private void showValidationError(string sMessage, Control focusControl)
        {
            MessageBox.Show(sMessage, f_csVALIDATION_MESSAGE_BOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            if (focusControl != null)
            {
                if (focusControl.CanFocus)
                {
                    focusControl.Focus();
                }
            }
        }

        #endregion

        private void cboExportMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            // perform a check to see if we have a valid change token stored..
            if (!string.IsNullOrEmpty(cboExportMethod.SelectedValue as string) && cboExportMethod.SelectedValue.ToString() == f_csEXPORTMETHOD_EXPORTCHANGES)
            {
                validateIncrementalExport();
            }
        }

        private void validateIncrementalExport()
        {
            bool bValidated = true;

            List<SPObjectData> exportObjects = new List<SPObjectData>();
            foreach (ListViewItem lvItem in lstExportItems.Items)
            {
                SPObjectData objectData = lvItem.Tag as SPObjectData;
                exportObjects.Add(objectData);
            }

            // first check if we don't have any SPSite/SPWeb objects - these are only objects where incremental export is valid..
            if (exportObjects.Find(objectIsSiteOrWeb) == null)
            {
                MessageBox.Show(
                    "No site or web objects are selected for export - SharePoint only supports incremental export for these items.\n\n" +
                    "A full export will be performed on this run.",
                    "Unable to use incremental export", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                bValidated = false;
            }
            else
            {
                string changeToken = IncrementalManager.GetLastToken(exportObjects);

                if (string.IsNullOrEmpty(changeToken))
                {
                    MessageBox.Show(
                        WizardDeployment.ChangeTokenNotFoundMessage,
                        "Unable to use incremental export", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    bValidated = false;
                }
            }

            if (!bValidated)
            {
                // also set dropdown back to full export..
                cboExportMethod.SelectedIndex = cboExportMethod.FindStringExact(f_csEXPORTMETHOD_EXPORTALL);
            }
        }

        private static bool objectIsSiteOrWeb(SPObjectData objectData)
        {
            return (objectData.ObjectType == SPDeploymentObjectType.Site || objectData.ObjectType == SPDeploymentObjectType.Web);
        }

    }
}