/*
MIT License

Copyright (c) 2017 David Cassady

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace csReporter
{
    public partial class frmFilter : Form
    {
        #region Form Variables

            CustomConcurrentBag<csObject> csObjects = new CustomConcurrentBag<csObject>();
            ConcurrentBag<csObject> matchingCSobjects = new ConcurrentBag<csObject>();
            FilterObject filter;
            string inputFileName;
            string outputFileName;
            ToolTip tTipInfo = new ToolTip();
            List<string> csAddlengths = new List<string>();
            List<string> xmlLoadlenghts = new List<string>();
            List<string> subStringlengths = new List<string>();
            bool pendingImportDeltasExist;
            bool unappliedExportDeltasExist;
            bool escrowedExportDeltasExist;
            bool unconfirmedExportDeltasExist;
            bool synchronizationHologramExist;
            bool pendingImportHologramExist;
            bool unappliedExportHologramExist;
            bool escrowedExportHologramExist;
            bool unconfirmedExportHologramExist;
            bool generateCSVReport;
            bool lowMemProcessing;
            bool makeReport;
            List<string> sysAttribs = new List<string>();
            List<string> changingAttribs = new List<string>();
            List<string> nonChangingAttribs = new List<string>();
            List<string> errorAttribs = new List<string>();
            List<string> mvAttribs = new List<string>();
            //timer that runs once after form_Shown event
            System.Windows.Forms.Timer oneTime = new System.Windows.Forms.Timer();
        
            //delegates for processing form while reading/writing files
            private delegate void delCloseForm(frmProgressBar frmProgress);
            private delegate void delUpdateBar(frmProgressBar frmProgress, int value);
            private delegate void delSetText(frmProgressBar frmProgress, String value);

            private delegate void delSetCount(int count);
            private delegate void delUpdateLBs(string[] valuess);
        
            private bool ADdata = false;
            public static volatile bool stopProcessing = false;
            List<string> knownADattribs = new List<string>() { "accountExpires", "objectSid", "groupType", "pwdLastSet", "userAccountControl", "lastLogonTimestamp", "createTimeStamp" };

        #endregion

        #region Form Contructor/Events
            public frmFilter(string fileName)
            {
                InitializeComponent();

                tTipInfo.AutoPopDelay =10000;
                tTipInfo.SetToolTip(this.cbADMA, "Auto-formats known Active Directory attributes so they are readable");

                inputFileName = fileName;
                
                csObjects.FinishedLoading += new LoadCompletedEventHandler(this.fileLoadComplete);

                this.Enabled = false;
            }
            private void frmFilter_Shown(object sender, EventArgs e)
            {
                Cursor = Cursors.WaitCursor;
                oneTime.Interval = 500;
                oneTime.Tick += oneTime_Tick;
                oneTime.Start();
            }
            private void oneTime_Tick(object sender, EventArgs e)
            {
                oneTime.Stop();
                //Cursor = Cursors.WaitCursor;
                //call function to parse file and populate UI filters
                processFile();

                System.GC.Collect();
                Cursor = Cursors.Default;
            }
            private void rbPendingImport_CheckedChanged(object sender, EventArgs e)
            {
                if (rbPendingImport.Checked)
                {
                    if (!lowMemProcessing && csObjects.Count < 1)
                    {
                        MessageBox.Show("You must first select a file");
                        rbPendingImport.Checked = false;
                        return;
                    }
                    Cursor = Cursors.WaitCursor;
                    cbNonChanging.Enabled = true;
                    reCalcMatching(FilterLevel.State);
                    Cursor = Cursors.Default;
                }
            }
            private void rbUnappliedExport_CheckedChanged(object sender, EventArgs e)
            {
                if (rbUnappliedExport.Checked)
                {
                    if (!lowMemProcessing && csObjects.Count < 1)
                    {
                        MessageBox.Show("You must first select a file");
                        rbUnappliedExport.Checked = false;
                        return;
                    }
                    Cursor = Cursors.WaitCursor;
                    cbNonChanging.Enabled = true;
                    reCalcMatching(FilterLevel.State);
                    Cursor = Cursors.Default;
                }
            }
            private void rbSynchronized_CheckedChanged(object sender, EventArgs e)
            {
                if (rbSynchronized.Checked)
                {
                    if (!lowMemProcessing && csObjects.Count < 1)
                    {
                        MessageBox.Show("You must first select a file");
                        rbSynchronized.Checked = false;
                        return;
                    }
                    Cursor = Cursors.WaitCursor;
                    cbNonChanging.Enabled = false;
                    reCalcMatching(FilterLevel.State);
                    Cursor = Cursors.Default;
                }
            }
            private void rbEscrowedExport_CheckedChanged(object sender, EventArgs e)
            {
                if (rbEscrowedExport.Checked)
                {
                    if (!lowMemProcessing && csObjects.Count < 1)
                    {
                        MessageBox.Show("You must first select a file");
                        rbEscrowedExport.Checked = false;
                        return;
                    }
                    Cursor = Cursors.WaitCursor;
                    cbNonChanging.Enabled = true;
                    reCalcMatching(FilterLevel.State);
                    Cursor = Cursors.Default;
                }

            }
            private void rbUnconfirmedExport_CheckedChanged(object sender, EventArgs e)
            {
                if (rbUnconfirmedExport.Checked)
                {
                    if (!lowMemProcessing && csObjects.Count < 1)
                    {
                        MessageBox.Show("You must first select a file");
                        rbUnconfirmedExport.Checked = false;
                        return;
                    }
                    Cursor = Cursors.WaitCursor;
                    cbNonChanging.Enabled = true;
                    reCalcMatching(FilterLevel.State);
                    Cursor = Cursors.Default;
                }
            }
            private void lbObjectType_SelectedValueChanged(object sender, EventArgs e)
            {
                Cursor = Cursors.WaitCursor;
                if (lbObjectType.SelectedItems.Count == 0)
                {
                    reCalcMatching(FilterLevel.State);
                }
                else if (rbSynchronized.Checked)
                {
                    reCalcMatching(FilterLevel.Operation);
                }
                else
                {
                    reCalcMatching(FilterLevel.ObjectType);
                }
                Cursor = Cursors.Default;
            }
            private void lbOperation_SelectedValueChanged(object sender, EventArgs e)
            {
                Cursor = Cursors.WaitCursor;
                if (lbOperation.SelectedItems.Count == 0)
                {
                    reCalcMatching(FilterLevel.ObjectType);
                }
                else
                {
                    reCalcMatching(FilterLevel.Operation);
                }
                Cursor = Cursors.Default;
            }
            private void lbAttribute_SelectedValueChanged(object sender, EventArgs e)
            {
                if (lbAttribute.SelectedItems.Count == 0)
                {
                    filter.ReportAttributes = lbAttribute.Items.Cast<string>().ToList();
                }
                else
                {
                    filter.ReportAttributes = lbAttribute.SelectedItems.Cast<string>().ToList();
                }
            }
            private void btnAddFilter_Click(object sender, EventArgs e)
            {
                if (dgvAdvanced.DataSource == null)
                {
                    //set binding for datagrid
                    dgvAdvanced.DataSource = filter.AttributeFilters;
                }
                //starts with Is covers Is present, Is not present, Is changing, Is not changing
                if (cbbAttributes.Text.Length > 0 && cbbComparators.Text.Length > 0 && (cbbValue.Text.Length > 0 || cbbComparators.Text.StartsWith("Is")))
                {
                    //verify uniqueness then add to filter
                    if (!filter.AttributeFilters.Contains(new FilterAttribute(cbbAttributes.Text, cbbComparators.Text, cbbValue.Text)))
                    {
                        Cursor = Cursors.WaitCursor;
                        filter.AttributeFilters.Add(new FilterAttribute(cbbAttributes.Text, cbbComparators.Text, cbbValue.Text));
                        cbbAttributes.SelectedIndex = -1;
                        cbbComparators.SelectedIndex = -1;
                        cbbValue.Text = "";
                        reCalcMatching(FilterLevel.AttributeValue);
                        Cursor = Cursors.Default;
                    }
                    else
                    {
                        MessageBox.Show("This filter is already added and cannot be added again");
                    }
                }
                else
                {
                    MessageBox.Show("Filter is not complete.  Ensure an attribute, operation, and value are present");
                }
            }
            private void btnRemoveFilter_Click(object sender, EventArgs e)
            {
                if (dgvAdvanced.SelectedRows.Count == 1)
                {
                    Cursor = Cursors.WaitCursor;
                    FilterAttribute tempFilter = new FilterAttribute(dgvAdvanced.SelectedRows[0].Cells[0].Value.ToString(), dgvAdvanced.SelectedRows[0].Cells[1].Value.ToString(), dgvAdvanced.SelectedRows[0].Cells[2].Value.ToString());
                    filter.AttributeFilters.Remove(tempFilter);
                    cbbAttributes.SelectedIndex = -1;
                    cbbComparators.SelectedIndex = -1;
                    cbbValue.Text = "";
                    if (filter.AttributeFilters.Count > 0)
                    {
                        reCalcMatching(FilterLevel.AttributeValue);
                    }
                    else
                    {
                        reCalcMatching(FilterLevel.Operation);
                    }
                    Cursor = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("Please select a filter from the grid below");
                }
            }
            private void cbbComparators_SelectedIndexChanged(object sender, EventArgs e)
            {
                //starts with Is covers Is present, Is not present, Is changing, Is not changing
                if (cbbAttributes.Text == "<Disconnect Time>" || cbbAttributes.Text == "<Connect Time>" || cbbComparators.Text.StartsWith("Is"))
                {
                    if (cbbComparators.Text.StartsWith("Is"))
                    {
                        cbbValue.Text = "";
                    }
                    cbbValue.Enabled = false;
                }
                else if (!cbbValue.Enabled)
                {
                    cbbValue.Enabled = true;
                }
            }
            private void cbADMA_CheckedChanged(object sender, EventArgs e)
            {
                ADdata = cbADMA.Checked;

                if (ADdata)
                {
                    Cursor = Cursors.WaitCursor;
                    foreach (csObject csObj in csObjects)
                    {
                        //Set attribute AD values
                        SetAttribADvals(csObj.PendingImportHologram);
                        SetAttribADvals(csObj.UnappliedExportHologram);
                        SetAttribADvals(csObj.SynchronizedHologram);
                        SetAttribADvals(csObj.EscrowedExportHologram);
                        SetAttribADvals(csObj.UnconfirmedExportHologram);
                    }
                    Cursor = Cursors.Default;
                }
            }
            private void cbNonChanging_CheckedChanged(object sender, EventArgs e)
            {
                Cursor = Cursors.WaitCursor;
                SetFilterAttributes();
                UpdateAttributeUI();
                Cursor = Cursors.Default;
            }
            private void btnCreateReport_Click(object sender, EventArgs e)
            {
                if (lowMemProcessing || matchingCSobjects.Count > 0)
                {
                    try
                    {
                        frmReport frmRep = new frmReport(filter.AvailableAttributes);
                        frmRep.Owner = this;
                        if (frmRep.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        {
                            //abort generating a report
                            frmRep.Dispose();
                            return;
                        }
                        frmRep.Dispose();
                        switch (generateCSVReport)
                        {
                            case true:
                                sfdReport.Filter = "csv files (*.csv)|*.csv";
                                break;
                            default:
                                sfdReport.Filter = "html files (*.html)|*.html";
                                break;
                        }
                        if (sfdReport.ShowDialog() != DialogResult.OK)
                        {
                            //abort generating a report
                            return;
                        }
                        outputFileName = sfdReport.FileName;
                        makeReport = true;
                        if (lowMemProcessing)
                        {
                            processFile();
                        }
                        else
                        {
                            frmProgressBar frmProgress = new frmProgressBar();
                            Thread worker;
                            switch (generateCSVReport)
                            {
                                case true:
                                    worker = new Thread(BuildCSVReport);
                                    break;
                                default:
                                    worker = new Thread(BuildHTMLReport);
                                    break;
                            }
                            worker.Start(frmProgress);
                            if (frmProgress.ShowDialog() != DialogResult.OK)
                            {
                                MessageBox.Show("Progress window closed before report finish generating.");

                                if (File.Exists(outputFileName))
                                {
                                    File.Delete(outputFileName);
                                }
                                stopProcessing = false;
                            }
                            frmProgress.Dispose();
                        }
                        if (File.Exists(outputFileName))
                        {
                            System.Diagnostics.Process.Start(outputFileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.handleException(ex, "Error occurred while generating report.");
                        Application.Exit();
                    }
                }
                else
                {
                    MessageBox.Show("Unwilling to generate a report with 0 objects");
                }
            }
            private void frmFilter_FormClosed(object sender, FormClosedEventArgs e)
            {
                if (!this.Owner.Visible)
                {
                    Application.Exit();
                }
            }
            private void btnBack_Click(object sender, EventArgs e)
            {
                ShowGetDataForm();
            }
            private void cbSystemAttribs_CheckedChanged(object sender, EventArgs e)
            {
                Cursor = Cursors.WaitCursor;
                SetFilterAttributes();
                UpdateAttributeUI();
                Cursor = Cursors.Default;
            }
            private void cbbAttributes_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (sysAttribs.Contains(cbbAttributes.Text))
                {
                    switch (cbbAttributes.Text)
                    {
                        case "<Connect Time>":
                            {
                                cbbValue.DropDownStyle = ComboBoxStyle.DropDown;
                                //getDateFilterValue
                                FrmFilterDate dateFilter = new FrmFilterDate();
                                if (dateFilter.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                                {
                                    cbbValue.Items.Clear();
                                    cbbValue.Enabled = false;
                                    cbbComparators.Items.Clear();
                                    cbbComparators.Items.Add("Before");
                                    cbbComparators.Items.Add("After");
                                    cbbComparators.Items.Add("Equals");
                                    cbbComparators.Items.Add("Does not equal");
                                    cbbComparators.Items.Add("Is present");
                                    cbbComparators.Items.Add("Is not present");
                                }
                                else
                                {
                                    cbbValue.Text = "";
                                    cbbAttributes.SelectedIndex = -1;
                                }
                            }
                            break;
                        case "<Disconnect Time>":
                            {
                                cbbValue.DropDownStyle = ComboBoxStyle.DropDown;
                                //getDateFilterValue
                                FrmFilterDate dateFilter = new FrmFilterDate();
                                if (dateFilter.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                                {
                                    cbbValue.Items.Clear();
                                    cbbValue.Enabled = false;
                                    cbbComparators.Items.Clear();
                                    cbbComparators.Items.Add("Before");
                                    cbbComparators.Items.Add("After");
                                    cbbComparators.Items.Add("Equals");
                                    cbbComparators.Items.Add("Does not equal");
                                    cbbComparators.Items.Add("Is present");
                                    cbbComparators.Items.Add("Is not present");
                                }
                                else
                                {
                                    cbbValue.Text = "";
                                    cbbAttributes.SelectedIndex = -1;
                                }
                            }
                            break;
                        case "<Connector State>":
                            cbbValue.Text = "";
                            cbbValue.Items.Clear();
                            cbbValue.DropDownStyle = ComboBoxStyle.DropDownList;
                            cbbValue.Items.Add("normal");
                            cbbValue.Items.Add("explicit");
                            cbbValue.Items.Add("stay");
                            cbbComparators.Items.Clear();
                            cbbComparators.Items.Add("Equals");
                            cbbComparators.Items.Add("Does not equal");
                            break;
                        case "<Connector>":
                            cbbValue.Text = "";
                            cbbValue.Items.Clear();
                            cbbValue.DropDownStyle = ComboBoxStyle.DropDownList;
                            cbbValue.Items.Add(Boolean.TrueString);
                            cbbValue.Items.Add(Boolean.FalseString);
                            cbbComparators.Items.Clear();
                            cbbComparators.Items.Add("Equals");
                            break;
                        case "<Connector Operation>":
                            cbbValue.Text = "";
                            cbbValue.Items.Clear();
                            cbbValue.DropDownStyle = ComboBoxStyle.DropDownList;
                            cbbValue.Items.Add("provisioning-rules");
                            cbbValue.Items.Add("join-rules");
                            cbbValue.Items.Add("joiner-rule");
                            cbbValue.Items.Add("projection-rules");
                            cbbComparators.Items.Clear();
                            cbbComparators.Items.Add("Equals");
                            cbbComparators.Items.Add("Does not equal");
                            break;
                        case "<DN>":
                            cbbValue.DropDownStyle = ComboBoxStyle.DropDown;
                            cbbValue.Text = "";
                            cbbValue.Items.Clear();
                            cbbComparators.Items.Clear();
                            cbbComparators.Items.Add("Equals");
                            cbbComparators.Items.Add("Does not equal");
                            cbbComparators.Items.Add("Starts with");
                            cbbComparators.Items.Add("Does not start with");
                            cbbComparators.Items.Add("Ends with");
                            cbbComparators.Items.Add("Does not end with");
                            cbbComparators.Items.Add("Contains");
                            cbbComparators.Items.Add("Does not contain");
                            cbbComparators.Items.Add("Is present");
                            cbbComparators.Items.Add("Is not present");
                            break;
                    }
                }
                else
                {
                    //reset
                    cbbValue.Enabled = true;
                    cbbValue.DropDownStyle = ComboBoxStyle.DropDown;
                    cbbValue.Text = "";
                    cbbValue.Items.Clear();
                    cbbComparators.Items.Clear();
                    cbbComparators.Items.Add("Equals");
                    cbbComparators.Items.Add("Does not equal");
                    cbbComparators.Items.Add("Starts with");
                    cbbComparators.Items.Add("Does not start with");
                    cbbComparators.Items.Add("Ends with");
                    cbbComparators.Items.Add("Does not end with");
                    cbbComparators.Items.Add("Contains");
                    cbbComparators.Items.Add("Does not contain");
                    cbbComparators.Items.Add("Is present");
                    cbbComparators.Items.Add("Is not present");
                    if (filter.FilterState != State.Synchronized)
                    {
                        cbbComparators.Items.Add("Is changing");
                        cbbComparators.Items.Add("Is not changing");
                    }
                    if (mvAttribs.Contains(cbbAttributes.Text))
                    {
                        cbbComparators.Items.Add(">");
                        cbbComparators.Items.Add("<");
                        cbbComparators.Items.Add("=");
                    }
                }
            }        
        #endregion
        
        #region Helper Functions

        #region Filtering functions
            private void UpdateMatching(State filterAction)
            {
                try
                {
                    lbAttribute.Items.Clear();
                    lbOperation.Items.Clear();
                    lbObjectType.Items.Clear();
                    cbbAttributes.Items.Clear();
                    cbbComparators.SelectedIndex = -1;
                    cbbValue.Text = "";
                    ConcurrentBag<string> types = new ConcurrentBag<string>();


                    filter.FilterState = filterAction;
                    filter.Level = FilterLevel.State;
                    filter.ObjectTypes.Clear();
                    filter.Operations.Clear();
                    filter.AttributeFilters.Clear();

                    if (lowMemProcessing)
                    {
                        processFile();
                    }
                    else
                    {
                        Parallel.ForEach(csObjects, obj =>
                            {
                                if (MatchFilter(obj))
                                {
                                    matchingCSobjects.Add(obj);
                                    types.Add(obj.ObjectType);
                                }
                            }
                        );
                        lbObjectType.Items.AddRange(types.Distinct().ToArray());
                    }

                }
                catch (Exception ex)
                {
                    ExceptionHandler.handleException(ex, "Error occurred evaluating filter criteria.\r\nPending Action: " + filterAction);
                    Application.Exit();
                }
            }
            private void UpdateMatching(State filterAction, List<string> objTypes)
            {
                try
                {
                    ConcurrentBag<string> ops = new ConcurrentBag<string>();

                    if (filterAction != State.Synchronized)
                    {
                        lbAttribute.Items.Clear();
                        lbOperation.Items.Clear();
                    }
                    cbbAttributes.Items.Clear();
                    cbbComparators.SelectedIndex = -1;
                    cbbValue.Text = "";

                    filter.FilterState = filterAction;
                    filter.Level = FilterLevel.ObjectType;
                    filter.ObjectTypes = objTypes;
                    filter.Operations.Clear();
                    filter.AttributeFilters.Clear();

                    if (lowMemProcessing)
                    {
                        processFile();
                    }
                    else
                    {
                        Parallel.ForEach(csObjects, obj =>
                            {
                                if (MatchFilter(obj))
                                {
                                    matchingCSobjects.Add(obj);
                                    if (filter.FilterState != State.Synchronized)
                                    {
                                        ops.Add(obj.Delta(filter.FilterState).Operation.ToString());
                                    }
                                }
                            }
                        );

                        if (filter.FilterState != State.Synchronized)
                        {
                            lbOperation.Items.AddRange(ops.Distinct().ToArray());
                        }
                    }
                }
                catch (Exception ex)
                {
                    string strMessage = "Error occurred evaluating filter criteria.\r\nPending Action: " + filterAction + "\r\nObject Types: ";
                    foreach (string strTemp in objTypes)
                    {
                        strMessage += strTemp + " ";
                    }
                    ExceptionHandler.handleException(ex, strMessage);
                    Application.Exit();
                }
            }
            private void UpdateMatching(State filterAction, List<string> objTypes, List<operation> op)
            {
                try
                {
                    cbbAttributes.Items.Clear();
                    cbbComparators.SelectedIndex = -1;
                    cbbValue.Text = "";

                    filter.FilterState = filterAction;
                    filter.Level = FilterLevel.Operation;
                    filter.ObjectTypes = objTypes;
                    filter.Operations = op;
                    filter.AttributeFilters.Clear();

                    if (lowMemProcessing)
                    {
                        processFile();
                    }
                    else
                    {
                        Parallel.ForEach(csObjects, obj =>
                            {
                                if (MatchFilter(obj))
                                {
                                    matchingCSobjects.Add(obj);
                                }
                            }
                        );
                    }
                }
                catch (Exception ex)
                {
                    string strMessage = "Error occurred evaluating filter criteria.\r\nPending Action: " + filterAction + "\r\nObject Types: ";
                    foreach (string strTemp in objTypes)
                    {
                        strMessage += strTemp + " ";
                    }
                    strMessage += "\r\nOperation: " + op;
                    ExceptionHandler.handleException(ex, strMessage);
                    Application.Exit();
                }
            }
            private void UpdateMatching(State filterAction, List<string> objTypes, List<operation> op, BindingList<FilterAttribute> attribFilters)
            {
                try
                {
                    cbbAttributes.Items.Clear();
                    cbbComparators.SelectedIndex = -1;
                    cbbValue.Text = "";

                    filter.FilterState = filterAction;
                    filter.Level = FilterLevel.AttributeValue;
                    filter.ObjectTypes = objTypes;
                    filter.Operations = op;
                    filter.AttributeFilters = attribFilters;

                    if (lowMemProcessing)
                    {
                        processFile();
                    }
                    else
                    {
                        Parallel.ForEach(csObjects, obj =>
                            {
                                if (MatchFilter(obj))
                                {
                                    matchingCSobjects.Add(obj);
                                }
                            }
                        );
                    }
                }
                catch (Exception ex)
                {
                    string strMessage = "Error occurred evaluating filter criteria.\r\nPending Action: " + filterAction + "\r\nObject Types: ";
                    foreach (string strTemp in objTypes)
                    {
                        strMessage += strTemp + " ";
                    }
                    strMessage += "\r\nOperation: " + op;
                    strMessage += "\r\nAttribute Filters:";
                    foreach (FilterAttribute filAtrib in attribFilters)
                    {
                        strMessage += "\r\n" + filAtrib.Attribute + " " + filAtrib.Operation + " " + filAtrib.Value;
                    }
                    ExceptionHandler.handleException(ex, strMessage);
                    Application.Exit();
                }
            }
            //State should be None to call this method
            private void UpdateMatching(List<string> objTypes, BindingList<FilterAttribute> attribFilters)
            {
                try
                {
                    cbbAttributes.Items.Clear();
                    cbbComparators.SelectedIndex = -1;
                    cbbValue.Text = "";

                    filter.FilterState = State.Synchronized;
                    filter.Level = FilterLevel.AttributeValue;
                    filter.ObjectTypes = objTypes;
                    filter.Operations.Clear();
                    filter.AttributeFilters = attribFilters;

                    if (lowMemProcessing)
                    {
                        processFile();
                    }
                    else
                    {
                        Parallel.ForEach(csObjects, obj =>
                            {
                                if (MatchFilter(obj))
                                {
                                    matchingCSobjects.Add(obj);
                                }
                            }
                        );
                    }
                }
                catch (Exception ex)
                {
                    string strMessage = "Error occurred evaluating filter criteria.\r\nPending Action: None\r\nObject Types: ";
                    foreach (string strTemp in objTypes)
                    {
                        strMessage += strTemp + " ";
                    }
                    strMessage += "\r\nAttribute Filters:";
                    foreach (FilterAttribute filAtrib in attribFilters)
                    {
                        strMessage += "\r\n" + filAtrib.Attribute + " " + filAtrib.Operation + " " + filAtrib.Value;
                    }
                    ExceptionHandler.handleException(ex, strMessage);
                    Application.Exit();
                }
            }
            private bool MatchFilter(csObject csObj)
            {
                switch (filter.Level)
                {
                    case FilterLevel.State:
                        if (csObj.Delta(filter.FilterState) != null && csObj.Delta(filter.FilterState).Operation != operation.none)
                        {
                            return true;
                        }
                        else if (filter.FilterState == State.Synchronized && csObj.SynchronizedHologram != null && csObj.SynchronizedHologram.Attributes.Count > 0)
                        {
                            return true;
                        }
                        break;
                    case FilterLevel.ObjectType:
                        if (csObj.Delta(filter.FilterState) != null && csObj.Delta(filter.FilterState).Operation != operation.none && filter.ObjectTypes.Contains(csObj.ObjectType))
                        {
                            return true;
                        }
                        else if (filter.FilterState == State.Synchronized && csObj.SynchronizedHologram != null && csObj.SynchronizedHologram.Attributes.Count > 0 && filter.ObjectTypes.Contains(csObj.ObjectType))
                        {
                            return true;
                        }
                        break;
                    case FilterLevel.Operation:
                        if (csObj.Delta(filter.FilterState) != null && csObj.Delta(filter.FilterState).Operation != operation.none && filter.ObjectTypes.Contains(csObj.ObjectType) && filter.Operations.Contains(csObj.Delta(filter.FilterState).Operation))
                        {
                            return true;
                        }
                        break;
                    case FilterLevel.AttributeValue:
                        switch (filter.FilterState)
                        {
                            case State.Synchronized:
                                if (filter.ObjectTypes.Contains(csObj.ObjectType) && MatchAttribFilter(csObj.SynchronizedHologram))
                                {
                                    return true; ;
                                }
                                break;
                            default:
                                if (filter.ObjectTypes.Contains(csObj.ObjectType))
                                {
                                    if (cbNonChanging.Checked && MatchAttribFilter(csObj.Delta(filter.FilterState), csObj.SynchronizedHologram, false))
                                    {
                                        return true;
                                    }
                                    else if (MatchAttribFilter(csObj.Delta(filter.FilterState), csObj.Hologram(filter.FilterState), true))
                                    {
                                        return true;
                                    }
                                }
                                break;
                        }
                        break;
                }
                return false;
            }
            private bool MatchAttribFilter(Delta delta, Entry hologram, bool deltaAttribs)
            {
                if (delta != null && delta.Operation != operation.none && filter.Operations.Contains(delta.Operation))
                {
                    switch (deltaAttribs)
                    {
                        case false:
                            return MatchAttribFilter(hologram);
                        case true:
                            return MatchAttribFilter(delta, hologram);
                    }
                }
                return false;
            }
            private bool MatchAttribFilter(Delta delta, Entry hologram)
            {
                if (hologram != null && hologram.Attributes.Count > 0)
                {
                    foreach (FilterAttribute fa in filter.AttributeFilters)
                    {
                        if (sysAttribs.Contains(fa.Attribute))
                        {
                            return SystemAttribFilter(fa, hologram.Parent);
                        }
                        else
                        {
                            switch (fa.Operation)
                            {
                                case "Equals":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || hologram.DN.ToUpperInvariant() != fa.Value.ToUpperInvariant())
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!delta.AttributeNames.Contains(fa.Attribute) || !hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.All(vals => vals.ToUpperInvariant() != fa.Value.ToUpperInvariant()))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.All(vals => vals.ToUpperInvariant() != fa.Value.ToUpperInvariant()))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Does not equal":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || hologram.DN.ToUpperInvariant() == fa.Value.ToUpperInvariant())
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!delta.AttributeNames.Contains(fa.Attribute) || !hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.Any(vals => vals.ToUpperInvariant() == fa.Value.ToUpperInvariant()))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.Any(vals => vals.ToUpperInvariant() == fa.Value.ToUpperInvariant()))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Starts with":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || !hologram.DN.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant()))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!delta.AttributeNames.Contains(fa.Attribute) || !hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.All(vals => !vals.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.All(vals => !vals.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Does not start with":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || hologram.DN.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant()))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!delta.AttributeNames.Contains(fa.Attribute) || !hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.Any(vals => vals.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.Any(vals => vals.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Ends with":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || !hologram.DN.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant()))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!delta.AttributeNames.Contains(fa.Attribute) || !hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.All(vals => !vals.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.All(vals => !vals.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Does not end with":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || hologram.DN.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant()))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!delta.AttributeNames.Contains(fa.Attribute) || !hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.Any(vals => vals.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.Any(vals => vals.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Contains":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || !hologram.DN.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant()))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!delta.AttributeNames.Contains(fa.Attribute) || !hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.All(vals => !vals.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.All(vals => !vals.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Does not contain":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || hologram.DN.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant()))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!delta.AttributeNames.Contains(fa.Attribute) || !hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.Any(vals => vals.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.Any(vals => vals.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Is present":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null)
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                    }
                                    break;
                                case "Is not present":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN != null)
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                    }
                                    break;
                                case "Is changing":
                                    if (!delta.AttributeNames.Contains(fa.Attribute))
                                    {
                                        return false;
                                    }
                                    break;
                                case "Is not changing":
                                    if (delta.AttributeNames.Contains(fa.Attribute))
                                    {
                                        return false;
                                    }
                                    break;
                                case ">":                                    
                                        if (!delta.AttributeNames.Contains(fa.Attribute) || !hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.Count <= Convert.ToInt32(fa.Value))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.Count <= Convert.ToInt32(fa.Value))
                                            {
                                                return false;
                                            }
                                        }
                                    break;
                                case "<":
                                    if (!delta.AttributeNames.Contains(fa.Attribute) || !hologram.AttributeNames.Contains(fa.Attribute))
                                    {
                                        return false;
                                    }
                                    if (ADdata && knownADattribs.Contains(fa.Attribute))
                                    {
                                        if (hologram.AttributeByName(fa.Attribute).ADStringValues.Count >= Convert.ToInt32(fa.Value))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (hologram.AttributeByName(fa.Attribute).StringValues.Count >= Convert.ToInt32(fa.Value))
                                        {
                                            return false;
                                        }
                                    }
                                    break;
                                case "=":
                                    if (!delta.AttributeNames.Contains(fa.Attribute) || !hologram.AttributeNames.Contains(fa.Attribute))
                                    {
                                        return false;
                                    }
                                    if (ADdata && knownADattribs.Contains(fa.Attribute))
                                    {
                                        if (hologram.AttributeByName(fa.Attribute).ADStringValues.Count != Convert.ToInt32(fa.Value))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (hologram.AttributeByName(fa.Attribute).StringValues.Count != Convert.ToInt32(fa.Value))
                                        {
                                            return false;
                                        }
                                    }
                                    break;
                                default:
                                    //show regular exception box
                                    ExceptionHandler.handleException(null, "Requested filter operation does not exist on delta.  Please remove this filter from the list.\r\n\r\nInvalid filter: " + fa.Attribute + " " + fa.Operation + " " + fa.Value);
                                    break;
                            }
                        }
                    }
                    return true;
                }
                return false;
            }        
            private bool MatchAttribFilter(Entry hologram)
            {
                if (hologram != null && hologram.Attributes.Count > 0)
                {
                    foreach (FilterAttribute fa in filter.AttributeFilters)
                    {
                        if (sysAttribs.Contains(fa.Attribute))
                        {
                            return SystemAttribFilter(fa, hologram.Parent);
                        }
                        else
                        {
                            switch (fa.Operation)
                            {
                                case "Equals":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || hologram.DN.ToUpperInvariant() != fa.Value.ToUpperInvariant())
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.All(vals => vals.ToUpperInvariant() != fa.Value.ToUpperInvariant()))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.All(vals => vals.ToUpperInvariant() != fa.Value.ToUpperInvariant()))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Does not equal":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || hologram.DN.ToUpperInvariant() == fa.Value.ToUpperInvariant())
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.Any(vals => vals.ToUpperInvariant() == fa.Value.ToUpperInvariant()))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.Any(vals => vals.ToUpperInvariant() == fa.Value.ToUpperInvariant()))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Starts with":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || !hologram.DN.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant()))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.All(vals => !vals.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.All(vals => !vals.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Does not start with":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || hologram.DN.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant()))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.Any(vals => vals.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.Any(vals => vals.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Ends with":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || !hologram.DN.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant()))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.All(vals => !vals.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.All(vals => !vals.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Does not end with":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || hologram.DN.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant()))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.Any(vals => vals.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.Any(vals => vals.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Contains":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || !hologram.DN.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant()))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.All(vals => !vals.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.All(vals => !vals.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Does not contain":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null || hologram.DN.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant()))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                        if (ADdata && knownADattribs.Contains(fa.Attribute))
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).ADStringValues.Any(vals => vals.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (hologram.AttributeByName(fa.Attribute).StringValues.Any(vals => vals.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant())))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    break;
                                case "Is present":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN == null)
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                    }
                                    break;
                                case "Is not present":
                                    if (fa.Attribute == "<DN>")
                                    {
                                        if (hologram.DN != null)
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (hologram.AttributeNames.Contains(fa.Attribute))
                                        {
                                            return false;
                                        }
                                    }
                                    break;
                                case ">":
                                    if (!hologram.AttributeNames.Contains(fa.Attribute))
                                    {
                                        return false;
                                    }
                                    if (ADdata && knownADattribs.Contains(fa.Attribute))
                                    {
                                        if (hologram.AttributeByName(fa.Attribute).ADStringValues.Count <= Convert.ToInt32(fa.Value))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (hologram.AttributeByName(fa.Attribute).StringValues.Count <= Convert.ToInt32(fa.Value))
                                        {
                                            return false;
                                        }
                                    }
                                    break;
                                case "<":
                                    if (!hologram.AttributeNames.Contains(fa.Attribute))
                                    {
                                        return false;
                                    }
                                    if (ADdata && knownADattribs.Contains(fa.Attribute))
                                    {
                                        if (hologram.AttributeByName(fa.Attribute).ADStringValues.Count >= Convert.ToInt32(fa.Value))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (hologram.AttributeByName(fa.Attribute).StringValues.Count >= Convert.ToInt32(fa.Value))
                                        {
                                            return false;
                                        }
                                    }
                                    break;
                                case "=":
                                    if (!hologram.AttributeNames.Contains(fa.Attribute))
                                    {
                                        return false;
                                    }
                                    if (ADdata && knownADattribs.Contains(fa.Attribute))
                                    {
                                        if (hologram.AttributeByName(fa.Attribute).ADStringValues.Count != Convert.ToInt32(fa.Value))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (hologram.AttributeByName(fa.Attribute).StringValues.Count != Convert.ToInt32(fa.Value))
                                        {
                                            return false;
                                        }
                                    }
                                    break;
                                default:
                                    //show regular exception box
                                    ExceptionHandler.handleException(null, "Requested filter operation does not exist on synchronized hologram.  Please remove this filter from the list.\r\n\r\nInvalid filter: " + fa.Attribute + " " + fa.Operation + " " + fa.Value);
                                    break;
                            }
                        }
                    }
                    return true;
                }
                return false;
            }
            private bool SystemAttribFilter(FilterAttribute fa, csObject obj)
            {
                switch (fa.Operation)
                {
                    case "Equals":
                        switch (fa.Attribute)
                        {
                            case "<DN>":
                                if (obj.csDN == null || obj.csDN.ToUpperInvariant() != fa.Value.ToUpperInvariant())
                                {
                                    return false;
                                }
                                break;
                            case "<Connector State>":
                                if (obj.ConnectorState == "" || obj.ConnectorState != fa.Value)
                                {
                                    return false;
                                }
                                break;
                            case "<Connector>":
                                if ((bool)obj.Connector != Convert.ToBoolean(fa.Value))
                                {
                                    return false;
                                }
                                break;
                            case "<Connector Operation>":
                                if (obj.ConnectionOperation == "" || obj.ConnectionOperation != fa.Value)
                                {
                                    return false;
                                }
                                break;
                            case "<Disconnect Time>":
                                if (obj.DisconnectionTime == DateTime.MinValue || obj.DisconnectionTime.Date != DateTime.Parse(fa.Value).Date)
                                {
                                    return false;
                                }
                                break;
                            case "<Connect Time>":
                                if (obj.ConnectionTime == DateTime.MinValue || obj.ConnectionTime.Date != DateTime.Parse(fa.Value).Date)
                                {
                                    return false;
                                }
                                break;
                        }
                        break;
                    case "Does not equal":
                        switch (fa.Attribute)
                        {
                            case "<DN>":
                                if (obj.csDN == null || obj.csDN.ToUpperInvariant() == fa.Value.ToUpperInvariant())
                                {
                                    return false;
                                }
                                break;
                            case "<Connector State>":
                                if (obj.ConnectorState == "" || obj.ConnectorState == fa.Value)
                                {
                                    return false;
                                }
                                break;
                            case "<Connector Operation>":
                                if (obj.ConnectionOperation == "" || obj.ConnectionOperation == fa.Value)
                                {
                                    return false;
                                }
                                break;
                            case "<Disconnect Time>":
                                if (obj.DisconnectionTime == DateTime.MinValue || obj.DisconnectionTime.Date == DateTime.Parse(fa.Value).Date)
                                {
                                    return false;
                                }
                                break;
                            case "<Connect Time>":
                                if (obj.ConnectionTime == DateTime.MinValue || obj.ConnectionTime.Date == DateTime.Parse(fa.Value).Date)
                                {
                                    return false;
                                }
                                break;
                        }
                        break;
                    case "Starts with":
                        switch (fa.Attribute)
                        {
                            case "<DN>":
                                if (obj.csDN == null || !obj.csDN.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant()))
                                {
                                    return false;
                                }
                                break;
                        }
                        break;
                    case "Does not start with":
                        switch (fa.Attribute)
                        {
                            case "<DN>":
                                if (obj.csDN == null || obj.csDN.ToUpperInvariant().StartsWith(fa.Value.ToUpperInvariant()))
                                {
                                    return false;
                                }
                                break;
                        }
                        break;
                    case "Ends with":
                        switch (fa.Attribute)
                        {
                            case "<DN>":
                                if (obj.csDN == null || !obj.csDN.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant()))
                                {
                                    return false;
                                }
                                break;
                        }
                        break;
                    case "Does not end with":
                        switch (fa.Attribute)
                        {
                            case "<DN>":
                                if (obj.csDN == null || obj.csDN.ToUpperInvariant().EndsWith(fa.Value.ToUpperInvariant()))
                                {
                                    return false;
                                }
                                break;
                        }
                        break;
                    case "Contains":
                       
                        switch (fa.Attribute)
                        {
                            case "<DN>":
                                if (obj.csDN == null || !obj.csDN.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant()))
                                {
                                    return false;
                                }
                                break;
                        }
                        break;
                    case "Does not contain":
                        switch (fa.Attribute)
                        {
                            case "<DN>":
                                if (obj.csDN == null || obj.csDN.ToUpperInvariant().Contains(fa.Value.ToUpperInvariant()))
                                {
                                    return false;
                                }
                                break;
                        }
                        break;
                    case "Is present":
                        switch(fa.Attribute)
                        {
                            case "<DN>":
                                if (obj.csDN == null)
                                {
                                    return false;
                                }
                                break;
                            case "<Connect Time>":
                                if (obj.ConnectionTime == DateTime.MinValue)
                                {
                                    return false;
                                }
                                break;
                            case "<Disconnect Time>":
                                if (obj.DisconnectionTime == DateTime.MinValue)
                                {
                                    return false;
                                }
                                break;
                        }
                        break;
                    case "Is not present":
                        switch (fa.Attribute)
                        {
                            case "<DN>":
                                if (obj.csDN != null)
                                {
                                    return false;
                                }
                                break;
                            case "<Connect Time>":
                                if (obj.ConnectionTime != DateTime.MinValue)
                                {
                                    return false;
                                }
                                break;
                            case "<Disconnect Time>":
                                if (obj.DisconnectionTime != DateTime.MinValue)
                                {
                                    return false;
                                }
                                break;
                        }
                        break;
                    case "Before":
                        switch (fa.Attribute)
                        {
                            case "<Disconnect Time>":
                                if (obj.DisconnectionTime == DateTime.MinValue || obj.DisconnectionTime.Date >= DateTime.Parse(fa.Value).Date)
                                {
                                    return false;
                                }
                                break;
                            case "<Connect Time>":
                                if (obj.ConnectionTime == DateTime.MinValue || obj.ConnectionTime.Date >= DateTime.Parse(fa.Value).Date)
                                {
                                    return false;
                                }
                                break;
                        }
                        break;
                    case "After":
                        switch (fa.Attribute)
                        {
                            case "<Disconnect Time>":
                                if (obj.DisconnectionTime == DateTime.MinValue || obj.DisconnectionTime.Date <= DateTime.Parse(fa.Value).Date)
                                {
                                    return false;
                                }
                                break;
                            case "<Connect Time>":
                                if (obj.ConnectionTime == DateTime.MinValue || obj.ConnectionTime.Date <= DateTime.Parse(fa.Value).Date)
                                {
                                    return false;
                                }
                                break;
                        }
                        break;
                    default:
                        //show regular exception box
                        ExceptionHandler.handleException(null, "Requested filter operation does not exist on system attributes.  Please remove this filter from the list.\r\n\r\nInvalid filter: " + fa.Attribute + " " + fa.Operation + " " + fa.Value);
                        break;
                }
                return true;
            }
            private void reCalcMatching(FilterLevel level)
            {
                matchingCSobjects = new ConcurrentBag<csObject>();
                System.GC.Collect();
                State currentFilterState = GetFilterState();

                switch (level)
                {
                    case FilterLevel.State:
                        UpdateMatching(currentFilterState);
                        break;
                    case FilterLevel.ObjectType:
                        UpdateMatching(currentFilterState, lbObjectType.SelectedItems.Cast<string>().ToList());
                        break;
                    case FilterLevel.Operation:
                        if (currentFilterState == State.Synchronized)
                        {
                            UpdateMatching(currentFilterState, lbObjectType.SelectedItems.Cast<string>().ToList());
                        }
                        else
                        {
                            UpdateMatching(currentFilterState, lbObjectType.SelectedItems.Cast<string>().ToList(), GetSelectedOps());
                        }
                        break;
                    case FilterLevel.AttributeValue:
                        if (currentFilterState == State.Synchronized)
                        {
                            UpdateMatching(lbObjectType.SelectedItems.Cast<string>().ToList(), filter.AttributeFilters);
                        }
                        else
                        {
                            UpdateMatching(currentFilterState, lbObjectType.SelectedItems.Cast<string>().ToList(), GetSelectedOps(), filter.AttributeFilters);
                        }
                        break;
                }
                if (!lowMemProcessing)
                {
                    lblCount.Text = "Matching Count: " + matchingCSobjects.Count;
                    UpdateAttributeLists();
                }
                SetFilterAttributes();
                UpdateAttributeUI();
            }
            private State GetFilterState()
            {
                if (rbSynchronized.Checked)
                {
                    return State.Synchronized;
                }
                else if (rbUnappliedExport.Checked)
                {
                    return State.UnappliedExport;
                }
                else if (rbPendingImport.Checked)
                {
                    return State.PendingImport;
                }
                else if (rbEscrowedExport.Checked)
                {
                    return State.EscrowedExport;
                }
                else
                {
                    return State.UnconfirmedExport;
                }
            }
            private List<operation> GetSelectedOps()
            {
                List<operation> selectedOps = new List<operation>();

                    foreach (string op in lbOperation.SelectedItems.Cast<string>())
                    {
                        switch (op)
                        {
                            case "delete-add":
                                selectedOps.Add(operation.deleteAdd);
                                break;
                            case "add":
                                selectedOps.Add(operation.add);
                                break;
                            case "replace":
                                selectedOps.Add(operation.replace);
                                break;
                            case "update":
                                selectedOps.Add(operation.update);
                                break;
                            case "delete":
                                selectedOps.Add(operation.delete);
                                break;
                            case "none":
                                selectedOps.Add(operation.none);
                                break;
                        }
                    }
                    return selectedOps;
            }
        #endregion

        #region Reporting functions

            #region Reporting - CSV
            private void BuildCSVReport(object objForm)
            {
                frmProgressBar frmProgress = (frmProgressBar)objForm;
                while (!frmProgress.Visible)
                {
                    Thread.SpinWait(200);
                }
                this.methSetText(frmProgress, "Generating CSV report");
                this.methUpdateBar(frmProgress, 0);
                using (StreamWriter outFile = new StreamWriter(outputFileName, false, Encoding.UTF8))
                {
                    try
                    {
                        WriteCSVReportHeaders(outFile);
                        //CS-DN     Object Type     Operation       Current Attribute       New Attribute
                        int counter = 0;
                        foreach (csObject obj in matchingCSobjects)
                        {
                            if (frmFilter.stopProcessing)
                            {
                                break;
                            }
                            this.methUpdateBar(frmProgress, (counter * 100) / matchingCSobjects.Count);
                            WriteCSVObjectReport(outFile, obj);
                            counter++;
                        }
                    }
                    catch (IOException ex)
                    {
                        if (ex.Message.Contains("it is being used by another process"))
                        {
                            //show clean messagebox to notify user
                            MessageBox.Show("The selected report file is in use by another process.  Please close the file and try again.");
                        }
                        else
                        {
                            //show regular exception box
                            ExceptionHandler.handleException(ex, "Error occurred while creating to CSV file");
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.handleException(ex, "Error occurred while creating CSV file");
                    }
                    finally
                    {
                        this.methCloseForm(frmProgress);
                    }
                }
            }
            private void WriteCSVReportHeaders(StreamWriter writer)
            {
                writer.Write("Criteria\r\n");
                writer.Write(",Data Type:," + filter.FilterState + "\r\n");
                if (filter.ObjectTypes.Count > 0)
                {
                    writer.Write(",Object Types:,\"" + String.Join("\n", filter.ObjectTypes.ToArray()) + "\"\r\n");
                }
                if (filter.Operations.Count > 0)
                {
                    writer.Write(",Operations:,\"" + String.Join("\n", filter.Operations.ToArray()) + "\"\r\n");
                }
                if (filter.ReportAttributes.Count > 0)
                {
                    writer.Write(",Attributes:,\"" + String.Join("\n", filter.ReportAttributes.ToArray()) + "\"\r\n");
                }
                if (filter.AttributeFilters.Count > 0)
                {
                    writer.Write(",Attribute Filters:,\"");
                    string strFilters = "";
                    foreach (FilterAttribute FA in filter.AttributeFilters)
                    {
                        strFilters += FA.Attribute + " " + FA.Operation + " " + FA.Value + "\n";
                    }
                    strFilters.Remove(strFilters.Length - 1, 1);
                    writer.Write(strFilters + "\"\r\n");
                }
                writer.Write(",Object Count:," + matchingCSobjects.Count.ToString() + "\r\n\r\n\r\n");

                if (filter.FilterState == State.Synchronized)
                {
                    writer.Write("CS distinguished name,Object Type"); ;
                    foreach (string Attrib in filter.ReportAttributes)
                    {
                        if (Attrib != "<DN>")
                        {
                            writer.Write("," + Attrib);
                        }
                    }
                }
                else
                {
                    writer.Write("CS distinguished name,Object Type,Operation");
                    foreach (string Attrib in filter.ReportAttributes)
                    {
                        if (Attrib != "<DN>" && (sysAttribs.Contains(Attrib) || errorAttribs.Contains(Attrib)))
                        {
                            writer.Write("," + Attrib);
                        }
                        else
                        {
                            writer.Write(",current " + Attrib + "," + "new " + Attrib);
                        }
                    }
                }
                writer.Write("\r\n");
            }
            private void WriteCSVObjectReport(StreamWriter writer, csObject obj)
            {
                if (filter.FilterState == State.Synchronized)
                {
                    writer.Write("\"" + obj.csDN + "\"," + obj.ObjectType);
                    writer.Write(",");

                    foreach (string attrib in filter.ReportAttributes)
                    {
                        try
                        {
                            if (sysAttribs.Contains(attrib))
                            {
                                switch (attrib)
                                {
                                    case "<Connector>":
                                        if (obj.Connector != null)
                                        {
                                            writer.Write(obj.Connector);
                                        }
                                        break;
                                    case "<Connect Time>":
                                        if (obj.ConnectionTime != null)
                                        {
                                            writer.Write(obj.ConnectionTime.ToString("g"));
                                        }
                                        break;
                                    case "<Connector Operation>":
                                        if (obj.ConnectionOperation != "")
                                        {
                                            writer.Write(obj.ConnectionOperation);
                                        }
                                        break;
                                    case "<Disconnect Time>":
                                        if (obj.DisconnectionTime != null)
                                        {
                                            writer.Write(obj.DisconnectionTime.ToString("g"));
                                        }
                                        break;
                                    case "<Connector State>":
                                        if (obj.ConnectorState != "")
                                        {
                                            writer.Write(obj.ConnectorState);
                                        }
                                        break;
                                }
                            }
                            else if (obj.ExportError != null && errorAttribs.Contains(attrib))
                            {
                                switch (attrib)
                                {
                                    case "<ExportErrorDetails>":
                                        StringBuilder errorInfo = new StringBuilder();
                                        if (obj.ExportError.DateOccurred != null)
                                        {
                                            errorInfo.Append("Date Occurred: " + obj.ExportError.DateOccurred + "\n");
                                        }
                                        if (obj.ExportError.FirstOccurred != null)
                                        {
                                            errorInfo.Append("First Occurred: " + obj.ExportError.FirstOccurred + "\n");
                                        }
                                        if (obj.ExportError.RetryCount != null)
                                        {
                                            errorInfo.Append("Retry Count: " + obj.ExportError.RetryCount + "\n");
                                        }
                                        if (obj.ExportError.ErrorType != null)
                                        {
                                            errorInfo.Append("Error Type: " + obj.ExportError.ErrorType + "\n");
                                        }
                                        if (obj.ExportError.ErrorCode != null)
                                        {
                                            errorInfo.Append("Error Code: " + obj.ExportError.ErrorCode + "\n");
                                        }
                                        if (obj.ExportError.ErrorLiteral != null)
                                        {
                                            errorInfo.Append("Error Literal: " + obj.ExportError.ErrorLiteral + "\n");
                                        }
                                        if (obj.ExportError.ServerErrorDetail != null)
                                        {
                                            errorInfo.Append("Server Error Detail: " + obj.ExportError.ServerErrorDetail + "\n");
                                        }
                                        errorInfo.Replace("\r\n", "");
                                        errorInfo.Replace("\"", "'");
                                        errorInfo.Insert(0, "\"");
                                        errorInfo.Append("\"");
                                        writer.Write(errorInfo.ToString());
                                        break;
                                }
                            }
                            else
                            {
                                Attribute shAttrib = GetMatchingAttribute(attrib, obj.SynchronizedHologram.Attributes);
                                writer.Write(AddAttribToReportCSV(shAttrib));
                            }
                            writer.Write(",");
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.handleException(ex, "Error occurred processing an attribute on a object for CSV file.\r\n\r\nDN=" + obj.csDN + "\r\n\r\nAttributeName=" + attrib);
                        }
                    }
                }
                else
                {
                    writer.Write("\"" + obj.csDN + "\"," + obj.ObjectType + "," + filter.FilterState.ToString() + "-" + obj.Delta(filter.FilterState).Operation);
                    writer.Write(",");
                    foreach (string attrib in filter.ReportAttributes)
                    {
                        try
                        {
                            if (sysAttribs.Contains(attrib))
                            {
                                switch (attrib)
                                {
                                    case "<Connector>":
                                        if (obj.Connector != null)
                                        {
                                            writer.Write(obj.Connector);
                                        }
                                        break;
                                    case "<Connect Time>":
                                        if (obj.ConnectionTime != null)
                                        {
                                            writer.Write(obj.ConnectionTime.ToString("g"));
                                        }
                                        break;
                                    case "<Connector Operation>":
                                        if (obj.ConnectionOperation != "")
                                        {
                                            writer.Write(obj.ConnectionOperation);
                                        }
                                        break;
                                    case "<Disconnect Time>":
                                        if (obj.DisconnectionTime != null)
                                        {
                                            writer.Write(obj.DisconnectionTime.ToString("g"));
                                        }
                                        break;
                                    case "<Connector State>":
                                        if (obj.ConnectorState != "")
                                        {
                                            writer.Write(obj.ConnectorState);
                                        }
                                        break;
                                }
                            }
                            else if (obj.ExportError != null && errorAttribs.Contains(attrib))
                            {
                                switch (attrib)
                                {
                                    case "<ExportErrorDetails>":
                                        StringBuilder errorInfo = new StringBuilder();
                                        if (obj.ExportError.DateOccurred != null)
                                        {
                                            errorInfo.Append("Date Occurred: " + obj.ExportError.DateOccurred + "\n");
                                        }
                                        if (obj.ExportError.FirstOccurred != null)
                                        {
                                            errorInfo.Append("First Occurred: " + obj.ExportError.FirstOccurred + "\n");
                                        }
                                        if (obj.ExportError.RetryCount != null)
                                        {
                                            errorInfo.Append("Retry Count: " + obj.ExportError.RetryCount + "\n");
                                        }
                                        if (obj.ExportError.ErrorType != null)
                                        {
                                            errorInfo.Append("Error Type: " + obj.ExportError.ErrorType + "\n");
                                        }
                                        if (obj.ExportError.ErrorCode != null)
                                        {
                                            errorInfo.Append("Error Code: " + obj.ExportError.ErrorCode + "\n");
                                        }
                                        if (obj.ExportError.ErrorLiteral != null)
                                        {
                                            errorInfo.Append("Error Literal: " + obj.ExportError.ErrorLiteral + "\n");
                                        }
                                        if (obj.ExportError.ServerErrorDetail != null)
                                        {
                                            errorInfo.Append("Server Error Detail: " + obj.ExportError.ServerErrorDetail + "\n");
                                        }
                                        errorInfo.Replace("\r\n", "");
                                        errorInfo.Replace("\"", "'");
                                        errorInfo.Insert(0, "\"");
                                        errorInfo.Append("\"");
                                        writer.Write(errorInfo.ToString());
                                        break;
                                }
                            }
                            else
                            {
                                Attribute pihAttrib = null;
                                Attribute shAttrib = null;
                                if (obj.Hologram(filter.FilterState) != null)
                                {
                                    pihAttrib = GetMatchingAttribute(attrib, obj.Hologram(filter.FilterState).Attributes);
                                }
                                if (obj.SynchronizedHologram != null)
                                {
                                    shAttrib = GetMatchingAttribute(attrib, obj.SynchronizedHologram.Attributes);
                                }
                                if (obj.Delta(filter.FilterState).AttributeNames.Contains(attrib))
                                {
                                    writer.Write(AddAttribToReportCSV(pihAttrib, shAttrib));
                                }
                                else if (shAttrib != null)
                                {
                                    writer.Write(AddAttribToReportCSV("(No Change)", shAttrib));
                                }
                                else
                                {
                                    writer.Write(",");
                                }
                            }
                            writer.Write(",");
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.handleException(ex, "Error occurred processing an attribute on an object for CSV file.\r\n\r\nDN=" + obj.csDN + "\r\n\r\nAttributeName=" + attrib);
                        }
                    }
                }
                writer.Write("\r\n");
            }
            private string AddAttribToReportCSV(Attribute attribute, Attribute syncdAttrib)
            {
                StringBuilder strOutput = new StringBuilder("");

                try
                {
                    if (syncdAttrib != null && attribute != null)
                    {
                        if (ADdata && knownADattribs.Contains(attribute.Name))
                        {
                            strOutput.Append(syncdAttrib.ADStringValues[0] + "," + attribute.ADStringValues[0]);
                        }
                        else
                        {
                            strOutput.Append("\"");
                            foreach (string val in syncdAttrib.StringValues)
                            {
                                string strTemp = val;
                                if (Regex.IsMatch(strTemp, @"^[^A-Za-z]"))
                                {
                                    strTemp = strTemp.Insert(0, "'");
                                }
                                strOutput.Append(strTemp + "\n");
                            }
                            strOutput.Remove(strOutput.Length - 1, 1);
                            strOutput.Append("\",\"");

                            foreach (string val in attribute.StringValues)
                            {
                                string strTemp = val;
                                if (Regex.IsMatch(strTemp, @"^[^A-Za-z]"))
                                {
                                    strTemp = strTemp.Insert(0, "'");
                                }
                                strOutput.Append(strTemp + "\n");
                            }
                            strOutput.Remove(strOutput.Length - 1, 1);
                            strOutput.Append("\"");
                        }
                    }
                    else if (syncdAttrib == null && attribute != null)
                    {
                        if (ADdata && knownADattribs.Contains(attribute.Name))
                        {
                            strOutput.Append("," + attribute.ADStringValues[0]);
                        }
                        else
                        {
                            strOutput.Append(",\"");
                            foreach (string val in attribute.StringValues)
                            {
                                string strTemp = val;
                                if (Regex.IsMatch(strTemp, @"^[^A-Za-z]"))
                                {
                                    strTemp = strTemp.Insert(0, "'");
                                }
                                strOutput.Append(strTemp + "\n");
                            }
                            strOutput.Remove(strOutput.Length - 1, 1);
                            strOutput.Append("\"");
                        }
                    }
                    else if (syncdAttrib != null && attribute == null)
                    {
                        if (ADdata && knownADattribs.Contains(syncdAttrib.Name))
                        {
                            strOutput.Append(syncdAttrib.ADStringValues[0] + ",(Deleted)");
                        }
                        else
                        {
                            strOutput.Append("\"");
                            foreach (string val in syncdAttrib.StringValues)
                            {
                                string strTemp = val;
                                if (Regex.IsMatch(strTemp, @"^[^A-Za-z]"))
                                {
                                    strTemp = strTemp.Insert(0, "'");
                                }
                                strOutput.Append(strTemp + "\n");
                            }
                            strOutput.Remove(strOutput.Length - 1, 1);
                            strOutput.Append("\",(Deleted)");
                        }
                    }
                    else if (syncdAttrib == null && attribute == null)
                    {
                        strOutput.Append(",");
                    }                
                }
                catch (Exception ex)
                {
                    string errorMessage = "";
                    if (attribute != null)
                    {
                        errorMessage = "Error getting attribute values\r\nAttributeName=" + attribute.Name;
                    }
                    else if (syncdAttrib != null)
                    {
                        errorMessage = "Error getting attribute values\r\nAttributeName=" + syncdAttrib.Name;
                    }
                    ExceptionHandler.handleException(ex, errorMessage);
                    Application.Exit();
                }
                return strOutput.ToString();
            }
            private string AddAttribToReportCSV(string attribValue, Attribute syncdAttrib)
            {
                StringBuilder strOutput = new StringBuilder("");

                try
                {
                    if (ADdata && knownADattribs.Contains(syncdAttrib.Name))
                    {
                        strOutput.Append(syncdAttrib.ADStringValues[0] + "," + attribValue);
                    }
                    else
                    {
                        strOutput.Append("\"");
                        string strTemp = "";
                        foreach (string val in syncdAttrib.StringValues)
                        {
                            strTemp = val;
                            if (Regex.IsMatch(strTemp, @"^[^A-Za-z]"))
                            {
                                strTemp = strTemp.Insert(0, "'");
                            }
                            strOutput.Append(strTemp + "\n");
                        }
                        strOutput.Remove(strOutput.Length - 1, 1);
                        strOutput.Append("\",\"");

                        strTemp = attribValue;
                        if (Regex.IsMatch(strTemp, @"^[^A-Za-z]"))
                        {
                            strTemp = strTemp.Insert(0, "'");
                        }
                        strOutput.Append(strTemp + "\n");
                        strOutput.Remove(strOutput.Length - 1, 1);
                        strOutput.Append("\"");
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = "";
                    if (attribValue != null)
                    {
                        errorMessage = "Error getting attribute values\r\nAttribValue=" + attribValue;
                    }
                    else if (syncdAttrib != null)
                    {
                        errorMessage = "Error getting attribute values\r\nAttributeName=" + syncdAttrib.Name;
                    }
                    ExceptionHandler.handleException(ex, errorMessage);
                    Application.Exit();
                }
                return strOutput.ToString();
            }
            private string AddAttribToReportCSV(Attribute syncdAttrib)
            {
                StringBuilder strOutput = new StringBuilder("");

                try
                {
                    if (syncdAttrib != null)
                    {
                        if (ADdata && knownADattribs.Contains(syncdAttrib.Name))
                        {
                            strOutput.Append(syncdAttrib.ADStringValues[0]);
                        }
                        else
                        {
                            strOutput.Append("\"");
                            foreach (string val in syncdAttrib.StringValues)
                            {
                                string strTemp = val;
                                if (Regex.IsMatch(strTemp, @"^[^A-Za-z]"))
                                {
                                    strTemp = strTemp.Insert(0, "'");
                                }
                                strOutput.Append(strTemp + "\n");
                            }
                            strOutput.Remove(strOutput.Length - 1, 1);
                            strOutput.Append("\"");
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = "Error getting attribute values";
                    if (syncdAttrib != null)
                    {
                        errorMessage = "\r\nAttributeName=" + syncdAttrib.Name;
                    }
                    ExceptionHandler.handleException(ex, errorMessage);
                    Application.Exit();
                }
                return strOutput.ToString();
            }
            #endregion

            #region Reporting - HTML
            private void BuildHTMLReport(object objForm)
            {
                frmProgressBar frmProgress = (frmProgressBar)objForm;
                while (!frmProgress.Visible)
                {
                    Thread.SpinWait(200);
                }
                this.methSetText(frmProgress, "Generating HTML report");
                this.methUpdateBar(frmProgress, 0);
                using (StreamWriter outFile = new StreamWriter(outputFileName))
                {
                    try
                    {
                        WriteHTMLReportHeaders(outFile);
                        int counter = 0;
                        foreach (csObject obj in matchingCSobjects)
                        {
                            if (frmFilter.stopProcessing)
                            {
                                break;
                            }
                            this.methUpdateBar(frmProgress, (counter * 100) / matchingCSobjects.Count);
                            WriteHTMLObjectReport(outFile, obj);
                            counter++;
                        }

                    }
                    catch (IOException ex)
                    {
                        if (ex.Message.Contains("it is being used by another process"))
                        {
                            //show clean messagebox to notify user
                            MessageBox.Show("The selected report file is in use by another process.  Please close the file and try again.");
                        }
                        else
                        {
                            //show regular exception box
                            ExceptionHandler.handleException(ex, "Error occurred while creating to HTML file");
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.handleException(ex, "Error occurred while creating HTML file");
                        Application.Exit();
                    }
                    finally
                    {
                        this.methCloseForm(frmProgress);
                    }
                }
            }
            private void WriteHTMLReportHeaders(StreamWriter writer)
            {
                writer.Write("<HTML>\r\n");

                writer.Write(@"<head>
    <meta charset=""UTF-8"">
    <style>
    table { 
        border-collapse: collapse;
    }
    table td, th {
        border: 1px solid black;
    }
    </style>
    </head>");

                //add logo to report
                System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                Stream myStream = myAssembly.GetManifestResourceStream("csReporter.csrLogo.png");
                Bitmap logo = new Bitmap(myStream);
                string strLogo = "";
                using (MemoryStream ms = new MemoryStream())
                {
                    logo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();

                    strLogo = Convert.ToBase64String(imageBytes);
                }
                writer.Write("<img src=\"data:image/png;base64," + strLogo + "\" alt=\"CSRLogo.png\" /><br><br><br><br><br>\r\n");
                writer.Write("<Table cellpadding=\"10\">\r\n");
                writer.Write("<TR><TH>Criteria</TH><TR>\r\n");
                writer.Write("<TR><TD style=\"border-style: none;\" /><TD>Data Type:</TD><TD>" + filter.FilterState + "</TD></TR>\r\n");
                if (filter.ObjectTypes.Count > 0)
                {
                    writer.Write("<TR><TD style=\"border-style: none;\" /><TD valign=\"top\">Object Types:</TD><TD>" + String.Join("<BR>", filter.ObjectTypes.ToArray()) + "</TD></TR>\r\n");
                }
                if (filter.Operations.Count > 0)
                {
                    writer.Write("<TR><TD style=\"border-style: none;\" /><TD valign=\"top\">Operations:</TD><TD>" + String.Join("<BR>", filter.Operations.ToArray()) + "</TD></TR>\r\n");
                }
                if (filter.ReportAttributes.Count > 0)
                {
                    writer.Write("<TR><TD style=\"border-style: none;\" /><TD valign=\"top\">Attributes:</TD><TD>" + String.Join("<BR>", filter.ReportAttributes.ToArray()) + "</TD></TR>\r\n");
                }
                if (filter.AttributeFilters.Count > 0)
                {
                    writer.Write("<TR><TD style=\"border-style: none;\" /><TD valign=\"top\">Attribute Filters:</TD><TD>");
                    foreach (FilterAttribute FA in filter.AttributeFilters)
                    {
                        writer.Write(FA.Attribute + " " + FA.Operation + " " + FA.Value + "<BR>");
                    }
                    writer.Write("</TD></TR>\r\n");
                }

                writer.Write("<TR><TD><B>Object Count:</B> " + matchingCSobjects.Count.ToString() + "</TD></TR>\r\n");

                writer.Write("</Table><BR><BR><Table cellpadding=\"10\">\r\n");

                writer.Write("<TR><TH>CS distinguished name</TH><TH>Object Type</TH>");
                if (filter.FilterState == State.Synchronized)
                {
                    writer.Write("<TH>Attribute</TH><TH>Current Value</TH></TR>\r\n");
                }
                else
                {
                    writer.Write("<TH>Operation</TH><TH>Attribute</TH><TH>Current Value</TH><TH>New Value</TH></TR>\r\n");
                }
                //CS-DN     Object Type     Operation       Attribute       Current Value       New Value

            }
            private void WriteHTMLObjectReport(StreamWriter writer, csObject obj)
            {
                if (frmFilter.stopProcessing)
                {
                    return;
                }
                if (filter.FilterState == State.Synchronized)
                {
                    writer.Write("<TR><TD>" + obj.csDN + "</TD><TD>" + obj.ObjectType + "</TD></TR>\r\n");

                    foreach (string attrib in filter.ReportAttributes)
                    {
                        string attribName = attrib.Replace("<", "&lt;");
                        attribName = attribName.Replace(">", "&gt;");
                        writer.Write("<TR><TD style=\"border-style: none;\" /><TD style=\"border-style: none;\" /><TD valign=\"top\" nowrap>" + attribName + "</TD>");
                        if (sysAttribs.Contains(attrib))
                        {
                            StringBuilder systemAttrib = new StringBuilder("<TD valign=\"top\" nowrap>");
                            switch (attrib)
                            {
                                case "<Connector>":
                                    if (obj.Connector != null)
                                    {
                                        systemAttrib.Append(obj.Connector);
                                    }
                                    break;
                                case "<Connect Time>":
                                    if (obj.ConnectionTime != null)
                                    {
                                        systemAttrib.Append(obj.ConnectionTime.ToString("g"));
                                    }
                                    break;
                                case "<Connector Operation>":
                                    if (obj.ConnectionOperation != "")
                                    {
                                        systemAttrib.Append(obj.ConnectionOperation);
                                    }
                                    break;
                                case "<Disconnect Time>":
                                    if (obj.DisconnectionTime != null)
                                    {
                                        systemAttrib.Append(obj.DisconnectionTime.ToString("g"));
                                    }
                                    break;
                                case "<Connector State>":
                                    if (obj.ConnectorState != "")
                                    {
                                        systemAttrib.Append(obj.ConnectorState);
                                    }
                                    break;
                            }
                            writer.Write(systemAttrib.ToString() + "</TD></TR>\r\n");
                        }
                        else if (obj.ExportError != null && errorAttribs.Contains(attrib))
                        {
                            switch (attrib)
                            {
                                case "<ExportErrorDetails>":
                                    StringBuilder errorInfo = new StringBuilder("<TD valign=\"top\" nowrap>");
                                    if (obj.ExportError.DateOccurred != null)
                                    {
                                        errorInfo.Append("Date Occurred: " + obj.ExportError.DateOccurred.ToString("g") + "<BR>\r\n");
                                    }
                                    if (obj.ExportError.FirstOccurred != null)
                                    {
                                        errorInfo.Append("First Occurred: " + obj.ExportError.FirstOccurred.ToString("g") + "<BR>\r\n");
                                    }
                                    if (obj.ExportError.RetryCount != null)
                                    {
                                        errorInfo.Append("Retry Count: " + obj.ExportError.RetryCount + "<BR>\r\n");
                                    }
                                    if (obj.ExportError.ErrorType != null)
                                    {
                                        errorInfo.Append("Error Type: " + obj.ExportError.ErrorType.Replace(" ", "&nbsp;") + "<BR>\r\n");
                                    }
                                    if (obj.ExportError.ErrorCode != null)
                                    {
                                        string temp = obj.ExportError.ErrorCode.Replace(" ", "&nbsp;");
                                        temp = temp.Replace("\n", "<BR>");
                                        errorInfo.Append("Error Code: " + temp + "<BR>\r\n");
                                    }
                                    if (obj.ExportError.ErrorLiteral != null)
                                    {
                                        string temp = obj.ExportError.ErrorLiteral.Replace(" ", "&nbsp;");
                                        temp = temp.Replace("\n", "<BR>");
                                        errorInfo.Append("Error Literal: " + temp + "<BR>\r\n");
                                    }
                                    if (obj.ExportError.ServerErrorDetail != null)
                                    {
                                        string temp = obj.ExportError.ServerErrorDetail.Replace(" ", "&nbsp;");
                                        temp = temp.Replace("\n", "<BR>>");
                                        errorInfo.Append("Server Error Detail: " + temp + "<BR>\r\n");
                                    }
                                    writer.Write(errorInfo.ToString() + "</TD></TR>\r\n");
                                    break;
                            }
                        }
                        else
                        {
                            Attribute shAttrib = GetMatchingAttribute(attrib, obj.SynchronizedHologram.Attributes);
                            writer.Write(AddAttribToReportHTML(shAttrib));
                        }
                    }
                }
                else
                {
                    writer.Write("<TR><TD>" + obj.csDN + "</TD><TD>" + obj.ObjectType + "</TD><TD>" + filter.FilterState.ToString() + "-" + obj.Delta(filter.FilterState).Operation + "</TD></TR>\r\n");

                    foreach (string attrib in filter.ReportAttributes)
                    {
                        string attribName = attrib.Replace("<", "&lt;");
                        attribName = attribName.Replace(">", "&gt;");
                        try
                        {
                            if (sysAttribs.Contains(attrib))
                            {
                                writer.Write("<TR><TD style=\"border-style: none;\" /><TD style=\"border-style: none;\" /><TD style=\"border-style: none;\" /><TD valign=\"top\" nowrap>" + attribName + "</TD>");
                                StringBuilder systemAttrib = new StringBuilder("<TD valign=\"top\" nowrap>");
                                switch (attrib)
                                {
                                    case "<Connector>":
                                        if (obj.Connector != null)
                                        {
                                            systemAttrib.Append(obj.Connector);
                                        }
                                        break;
                                    case "<Connect Time>":
                                        if (obj.ConnectionTime != null)
                                        {
                                            systemAttrib.Append(obj.ConnectionTime.ToString("g"));
                                        }
                                        break;
                                    case "<Connector Operation>":
                                        if (obj.ConnectionOperation != "")
                                        {
                                            systemAttrib.Append(obj.ConnectionOperation);
                                        }
                                        break;
                                    case "<Disconnect Time>":
                                        if (obj.DisconnectionTime != null)
                                        {
                                            systemAttrib.Append(obj.DisconnectionTime.ToString("g"));
                                        }
                                        break;
                                    case "<Connector State>":
                                        if (obj.ConnectorState != "")
                                        {
                                            systemAttrib.Append(obj.ConnectorState);
                                        }
                                        break;
                                }
                                writer.Write(systemAttrib + "</TD><TD /></TR>\r\n");
                            }
                            else if (obj.ExportError != null && errorAttribs.Contains(attrib))
                            {
                                writer.Write("<TR><TD style=\"border-style: none;\" /><TD style=\"border-style: none;\" /><TD style=\"border-style: none;\" /><TD valign=\"top\" nowrap>" + attribName + "</TD>");
                                switch (attrib)
                                {
                                    case "<ExportErrorDetails>":
                                        StringBuilder errorInfo = new StringBuilder("<TD valign=\"top\" nowrap>");
                                        if (obj.ExportError.DateOccurred != null)
                                        {
                                            errorInfo.Append("Date Occurred: " + obj.ExportError.DateOccurred.ToString("g") + "<BR>\r\n");
                                        }
                                        if (obj.ExportError.FirstOccurred != null)
                                        {
                                            errorInfo.Append("First Occurred: " + obj.ExportError.FirstOccurred.ToString("g") + "<BR>\r\n");
                                        }
                                        if (obj.ExportError.RetryCount != null)
                                        {
                                            errorInfo.Append("Retry Count: " + obj.ExportError.RetryCount.Replace(" ", "&nbsp;") + "<BR>\r\n");
                                        }
                                        if (obj.ExportError.ErrorType != null)
                                        {
                                            errorInfo.Append("Error Type: " + obj.ExportError.ErrorType.Replace(" ", "&nbsp;") + "<BR>\r\n");
                                        }
                                        if (obj.ExportError.ErrorCode != null)
                                        {
                                            string temp = obj.ExportError.ErrorCode.Replace(" ", "&nbsp;");
                                            temp = temp.Replace("\n", "<BR>");
                                            errorInfo.Append("Error Code: " + temp + "<BR>\r\n");
                                        }
                                        if (obj.ExportError.ErrorLiteral != null)
                                        {
                                            string temp = obj.ExportError.ErrorLiteral.Replace(" ", "&nbsp;");
                                            temp = temp.Replace("\n", "<BR>");
                                            errorInfo.Append("Error Literal: " + temp + "<BR>\r\n");
                                        }
                                        if (obj.ExportError.ServerErrorDetail != null)
                                        {
                                            string temp = obj.ExportError.ServerErrorDetail.Replace(" ", "&nbsp;");
                                            temp = temp.Replace("\n", "<BR>");
                                            errorInfo.Append("Server Error Detail: " + temp + "<BR>\r\n");
                                        }
                                        writer.Write(errorInfo.ToString() + "</TD><TD /></TR>\r\n");
                                        break;
                                }
                            }
                            else
                            {
                                Attribute uehAttrib = null;
                                Attribute shAttrib = null;
                                if (obj.Hologram(filter.FilterState) != null)
                                {
                                    uehAttrib = GetMatchingAttribute(attrib, obj.Hologram(filter.FilterState).Attributes);
                                }
                                if (obj.SynchronizedHologram != null)
                                {
                                    shAttrib = GetMatchingAttribute(attrib, obj.SynchronizedHologram.Attributes);
                                }
                                if (obj.Delta(filter.FilterState).AttributeNames.Contains(attrib))
                                {
                                    writer.Write("<TR><TD style=\"border-style: none;\" /><TD style=\"border-style: none;\" /><TD style=\"border-style: none;\" /><TD valign=\"top\" nowrap>" + attribName + "</TD>");
                                    writer.Write(AddAttribToReportHTML(uehAttrib, shAttrib));
                                }
                                else if (shAttrib != null)
                                {
                                    writer.Write("<TR><TD style=\"border-style: none;\" /><TD style=\"border-style: none;\" /><TD style=\"border-style: none;\" /><TD valign=\"top\" nowrap>" + attribName + "</TD>");
                                    writer.Write(AddAttribToReportHTML("(No Change)", shAttrib));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.handleException(ex, "Error occurred processing an attribute on an object for HTML file.\r\n\r\nDN=" + obj.csDN + "\r\n\r\nAttributeName=" + attrib);
                        }
                    }
                }
            }
            private void WriteHTMLEndReport(StreamWriter writer)
            {
                //wrap up HTML tags
                writer.Write("</Table></HTML>");
            }
            private string AddAttribToReportHTML(Attribute attribute, Attribute syncdAttrib)
            {
                StringBuilder strOutput = new StringBuilder("");
                try
                {
                    if (syncdAttrib != null && attribute != null)
                    {
                        if (ADdata && knownADattribs.Contains(attribute.Name))
                        {
                            strOutput.Append("<TD valign=\"top\" nowrap>" + syncdAttrib.ADStringValues[0]
                                + "</TD><TD valign=\"top\" nowrap>" + attribute.ADStringValues[0] + "</TD></TR>\r\n");
                        }
                        else
                        {
                            strOutput.Append("<TD valign=\"top\" nowrap>");
                            foreach (string val in syncdAttrib.StringValues)
                            {
                                string strTemp = val.Replace(" ", "&nbsp;");
                                strOutput.Append(strTemp + "<BR>\r\n");
                            }
                            strOutput.Append("</TD><TD valign=\"top\" nowrap>");
                            foreach (string val in attribute.StringValues)
                            {
                                string strTemp = val.Replace(" ", "&nbsp;");
                                strOutput.Append(strTemp + "<BR>\r\n");
                            }
                            strOutput.Append("</TD></TR>\r\n");
                        }
                    }
                    else if (syncdAttrib == null && attribute != null)
                    {
                        if (ADdata && knownADattribs.Contains(attribute.Name))
                        {
                            strOutput.Append("<TD /><TD valign=\"top\" nowrap>" + attribute.ADStringValues[0] + "</TD></TR>\r\n");
                        }
                        else
                        {
                            strOutput.Append("<TD /><TD valign=\"top\" nowrap>");
                            foreach (string val in attribute.StringValues)
                            {
                                string strTemp = val.Replace(" ", "&nbsp;");
                                strOutput.Append(strTemp + "<BR>\r\n");
                            }
                            strOutput.Append("</TD></TR>\r\n");
                        }
                    }
                    else if (syncdAttrib != null && attribute == null)
                    {
                        if (ADdata && knownADattribs.Contains(syncdAttrib.Name))
                        {
                            strOutput.Append("<TD valign=\"top\" nowrap>" + syncdAttrib.ADStringValues[0] + "</TD><TD><b><i>(Deleted)</i></b></TD></TR>\r\n");
                        }
                        else
                        {
                            strOutput.Append("<TD valign=\"top\" nowrap>");
                            foreach (string val in syncdAttrib.StringValues)
                            {
                                string strTemp = val.Replace(" ", "&nbsp;");
                                strOutput.Append(strTemp + "<BR>\r\n");
                            }
                            strOutput.Append("</TD><TD><b><i>(Deleted)</i></b></TD></TR>\r\n");
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = "";
                    if (attribute != null)
                    {
                        errorMessage = "Error getting attribute values\r\nAttributeName=" + attribute.Name;
                    }
                    else if (syncdAttrib != null)
                    {
                        errorMessage = "Error getting attribute values\r\nAttributeName=" + syncdAttrib.Name;
                    }
                    ExceptionHandler.handleException(ex, errorMessage);
                    Application.Exit();
                }
                return strOutput.ToString();
            }
            private string AddAttribToReportHTML(string attribValue, Attribute syncdAttrib)
            {
                StringBuilder strOutput = new StringBuilder("");
                try
                {
                    if (ADdata && knownADattribs.Contains(syncdAttrib.Name))
                    {
                        strOutput.Append("<TD valign=\"top\" nowrap>" + syncdAttrib.ADStringValues[0]
                            + "</TD><TD valign=\"top\" nowrap><b><i>" + attribValue + "</i></b></TD></TR>\r\n");
                    }
                    else
                    {
                        strOutput.Append("<TD valign=\"top\" nowrap>");
                        string strTemp = "";
                        foreach (string val in syncdAttrib.StringValues)
                        {
                            strTemp = val.Replace(" ", "&nbsp;");
                            strOutput.Append(strTemp + "<BR>\r\n");
                        }
                        strOutput.Append("</TD><TD valign=\"top\" nowrap><b><i>");
                        strTemp = attribValue.Replace(" ", "&nbsp;");
                        strOutput.Append(strTemp + "<BR>\r\n");
                        strOutput.Append("</i></b></TD></TR>\r\n");
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = "";
                    if (attribValue != null)
                    {
                        errorMessage = "Error getting attribute values\r\nAttributeValue=" + attribValue;
                    }
                    else if (syncdAttrib != null)
                    {
                        errorMessage = "Error getting attribute values\r\nAttributeName=" + syncdAttrib.Name;
                    }
                    ExceptionHandler.handleException(ex, errorMessage);
                    Application.Exit();
                }
                return strOutput.ToString();
            }
            private string AddAttribToReportHTML(Attribute syncdAttrib)
            {
                StringBuilder strOutput = new StringBuilder("");

                try
                {
                    if (syncdAttrib != null)
                    {
                        if (ADdata && knownADattribs.Contains(syncdAttrib.Name))
                        {
                            strOutput.Append("<TD valign=\"top\" nowrap>" + syncdAttrib.ADStringValues[0] + "</TD></TR>\r\n");
                        }
                        else
                        {
                            strOutput.Append("<TD valign=\"top\" nowrap>");
                            foreach (string val in syncdAttrib.StringValues)
                            {
                                string strTemp = val.Replace(" ", "&nbsp;");
                                strOutput.Append(strTemp + "<BR>\r\n");
                            }
                            strOutput.Append("</TD></TR>\r\n");
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = "Error getting attribute values";
                    if (syncdAttrib != null)
                    {
                        errorMessage = "\r\nAttributeName=" + syncdAttrib.Name;
                    }
                    ExceptionHandler.handleException(ex, errorMessage);
                    Application.Exit();
                }
                return strOutput.ToString();
            }
            #endregion

            private Attribute GetMatchingAttribute(string inputName, List<Attribute> attribList)
            {
                return attribList.Find(attrib => attrib.Name == inputName);
            }
        #endregion

        #region Parsing functions
            private void processFile()
            {
                try
                {
                    FileInfo file = new FileInfo(inputFileName);
                    long fileLength = file.Length;
                    int fileLengthMB = (int)(fileLength / 1048572);
                    //if file larger than 150MB use low memory option with progress bar
                    if (fileLengthMB > 150)
                    {
                        lowMemProcessing = true;
                        frmProgressBar frmProgress = new frmProgressBar();
                        Thread worker = new Thread(delegate() { parseFileLowMem(new object[] { frmProgress, fileLength, filter, ADdata }); });
                        worker.Start();
                        DialogResult result = frmProgress.ShowDialog();
                        this.Enabled = true;
                        if (result != DialogResult.OK)
                        {
                            MessageBox.Show("Progress window closed before processing was completed.\r\n\r\nNo data was processed", "Processing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            frmProgress.Dispose();

                            //reset for future use
                            stopProcessing = false;
                            return;
                        }
                        frmProgress.Dispose();
                    }
                    //if file larger than 10MB use progress bar
                    else if (fileLengthMB > 10)
                    {
                        frmProgressBar frmProgress = new frmProgressBar();
                        Thread worker = new Thread(delegate() { parseFileToMem(new object[] { frmProgress, fileLength }); });
                        worker.Start();
                        DialogResult result = frmProgress.ShowDialog();
                        if (result != DialogResult.OK)
                        {
                            MessageBox.Show("Progress window closed before processing was completed.\r\n\r\nNo data was processed", "Processing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            frmProgress.Dispose();
                            ShowGetDataForm();
                            return;
                        }
                        frmProgress.Dispose();
                    }
                    else
                    {
                        parseFileToMem();
                    }
                    if (filter == null)
                    {
                        filter = new FilterObject();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.handleException(ex, "Error occurred loading file.");
                    Application.Exit();
                }
            }
            private void parseFileToMem()
            {
                FileStream fsRead = null;
                BufferedStream bsRead = null;
                XmlReader xmlRead = null;
                List<Task> createTasks = new List<Task>();
                try
                {
                    using (fsRead = File.Open(inputFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (bsRead = new BufferedStream(fsRead, 512000))
                        {
                            XmlReaderSettings xmlSettings = new XmlReaderSettings();
                            xmlSettings.IgnoreWhitespace = true;
                            using (xmlRead = XmlReader.Create(bsRead, xmlSettings))
                            {
                                while (!xmlRead.EOF)
                                {
                                    if (xmlRead.Name == "cs-object" && xmlRead.NodeType == XmlNodeType.Element)
                                    {
                                        string line = xmlRead.ReadOuterXml();
                                        createTasks.Add(Task.Factory.StartNew(() => createAddcsObject(line)));
                                        //createAddcsObject(line);
                                    }
                                    else
                                    {
                                        xmlRead.Read();
                                    }
                                }
                            }
                        }
                    }
                    Task.WaitAll(createTasks.ToArray());
                }
                catch (Exception ex)
                {
                    ExceptionHandler.handleException(ex, "Error occurred parsing file.  Ensure the file follows proper XML syntax.  Regenerate file if necessary");
                    Application.Exit();
                }
                getFilteringOptions();
                csObjects.LoadingComplete = true;
                showFilterMessages();
                System.GC.Collect();
            }
            private void parseFileToMem(object[] input)
            {
                frmProgressBar frmProgress = (frmProgressBar)input[0];
                while (!frmProgress.Visible)
                {
                    Thread.SpinWait(200);
                }
                long fileLength = (long)input[1];
                this.methUpdateBar(frmProgress, 0);
                this.methSetText(frmProgress, "Processing XML file");

                FileStream fsRead = null;
                BufferedStream bsRead = null;
                XmlReader xmlRead = null;
                List<Task> createTasks = new List<Task>();
                try
                {
                    using (fsRead = File.Open(inputFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (bsRead = new BufferedStream(fsRead, 512000))
                        {
                            XmlReaderSettings xmlSettings = new XmlReaderSettings();
                            xmlSettings.IgnoreWhitespace = true;
                            using (xmlRead = XmlReader.Create(bsRead, xmlSettings))
                            {
                                long readSoFar = 0;
                                //update interval is 5%
                                long updateInterval = Convert.ToInt64(fileLength * .05);
                                //stopProcessing signals thread to end/abort and exit
                                while (!xmlRead.EOF && !stopProcessing)
                                {
                                    if (xmlRead.Name == "cs-object" && xmlRead.NodeType == XmlNodeType.Element)
                                    {
                                        string line = xmlRead.ReadOuterXml();
                                        createTasks.Add(Task.Factory.StartNew(() => createAddcsObject(line)));
                                        readSoFar = bsRead.Position;
                                        if (!stopProcessing && readSoFar >= updateInterval)
                                        {
                                            int temp = (int)((readSoFar / fileLength) * 200);
                                            this.methUpdateBar(frmProgress, (int)((readSoFar * 100) / fileLength));
                                            //increases updateInterval by 5%
                                            updateInterval += Convert.ToInt64(fileLength * .05);
                                        }
                                    }
                                    else
                                    {
                                        xmlRead.Read();
                                    }
                                }
                            }
                        }
                    }
                    Task.WaitAll(createTasks.ToArray());
                }
                catch (Exception ex)
                {
                    ExceptionHandler.handleException(ex, "Error occurred parsing file.  Ensure the file follows proper XML syntax.  Regenerate file if necessary");
                    Application.Exit();
                }
                if (!stopProcessing)
                {
                    getFilteringOptions();
                    this.methCloseForm(frmProgress);
                    csObjects.LoadingComplete = true;
                    showFilterMessages();
                    System.GC.Collect();
                }
            }
            private void parseFileLowMem(object[] input)
            {
                frmProgressBar frmProgress = (frmProgressBar)input[0];
                while (!frmProgress.Visible)
                {
                    Thread.SpinWait(200);
                }
                long fileLength = (long)input[1];
                FilterObject filterRef = null;
                bool needADdata = false;
                if (input[2] != null)
                {
                    filterRef = (FilterObject)input[2];
                }
                if (input[3] != null)
                {
                    needADdata = (bool)input[3];
                }
                //Local variables to hold new values while filtering with the existing global variables
                List<string> newSysAttribs = new List<string>();
                List<string> newChangingAttribs = new List<string>();
                List<string> newNonChangingAttribs = new List<string>();
                List<string> newErrorAttribs = new List<string>();
                newChangingAttribs.Clear();
                newNonChangingAttribs.Clear();
                newSysAttribs.Clear();
                newSysAttribs.Add("<DN>");
                mvAttribs.Clear();
                int counterCSObjects = 0;
                int counterMatchObjects = 0;
                List<string> objTypes = new List<string>();
                List<string> ops = new List<string>();
                FileStream fsRead = null;
                BufferedStream bsRead = null;
                XmlReader xmlRead = null;
                this.methUpdateBar(frmProgress, 0);
                this.methSetText(frmProgress, "Processing XML file");
                StreamWriter outFile = null;
                if (makeReport)
                {
                    outFile = new StreamWriter(outputFileName);
                }

                try
                {
                    using (fsRead = File.Open(inputFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (bsRead = new BufferedStream(fsRead, 512000))
                        {
                            XmlReaderSettings xmlSettings = new XmlReaderSettings();
                            xmlSettings.IgnoreWhitespace = true;
                            using (xmlRead = XmlReader.Create(bsRead, xmlSettings))
                            {
                                long readSoFar = 0;
                                //update interval is 5%
                                long updateInterval = Convert.ToInt64(fileLength * .05);

                                //stopProcessing signals thread to end/abort and exit
                                while (!xmlRead.EOF && !stopProcessing)
                                {
                                    if (xmlRead.Name == "cs-object" && xmlRead.NodeType == XmlNodeType.Element)
                                    {
                                        csObject tmpCSO = new csObject(xmlRead.ReadSubtree());
                                        mvAttribs.AddRange(tmpCSO.MVAttribNames);
                                        mvAttribs = mvAttribs.Distinct().ToList();
                                        //if filter exists check for match
                                        if (filterRef != null)
                                        {
                                            if (needADdata)
                                            {
                                                //Set attribute AD values
                                                SetAttribADvals(tmpCSO.PendingImportHologram);
                                                SetAttribADvals(tmpCSO.UnappliedExportHologram);
                                                SetAttribADvals(tmpCSO.SynchronizedHologram);
                                                SetAttribADvals(tmpCSO.EscrowedExportHologram);
                                                SetAttribADvals(tmpCSO.UnconfirmedExportHologram);
                                            }
                                            //check for match
                                            if (MatchFilter(tmpCSO))
                                            {
                                                if (makeReport)
                                                {
                                                    if (outFile == null)
                                                    {
                                                        ExceptionHandler.handleException("While attempting to make a report, output StreamWriter is null.");
                                                    }
                                                    switch (generateCSVReport)
                                                    {
                                                        case true:
                                                            //making a report, only write headers if counterMatchObjects == 0
                                                            if (counterMatchObjects == 0)
                                                            {
                                                                WriteCSVReportHeaders(outFile);
                                                            }
                                                            WriteCSVObjectReport(outFile, tmpCSO);
                                                            break;
                                                        default:
                                                            //making a report, only write headers if counterMatchObjects == 0
                                                            if (counterMatchObjects == 0)
                                                            {
                                                                WriteHTMLReportHeaders(outFile);
                                                            }
                                                            WriteHTMLObjectReport(outFile, tmpCSO);                                                                
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    //update objTypes and operations
                                                    if (filter.Level == FilterLevel.State)
                                                    {
                                                        objTypes.Add(tmpCSO.ObjectType);
                                                    }
                                                    if (filter.Level == FilterLevel.ObjectType && filter.FilterState != State.Synchronized)
                                                    {
                                                        ops.Add(tmpCSO.Delta(filter.FilterState).Operation.ToString());
                                                    }
                                                    //update attribute lists
                                                    if (filter.FilterState != State.Synchronized)
                                                    {
                                                        newChangingAttribs.AddRange(tmpCSO.Delta(filter.FilterState).AttributeNames);
                                                    }
                                                    if (tmpCSO.SynchronizedHologram != null)
                                                    {
                                                        newNonChangingAttribs.AddRange(tmpCSO.SynchronizedHologram.AttributeNames);
                                                    }
                                                    if (tmpCSO.ConnectorState != "")
                                                    {
                                                        newSysAttribs.Add("<Connector State>");
                                                    }
                                                    if (tmpCSO.DisconnectionTime != DateTime.MinValue)
                                                    {
                                                        newSysAttribs.Add("<Disconnect Time>");
                                                    }
                                                    if (tmpCSO.ConnectionOperation != "")
                                                    {
                                                        newSysAttribs.Add("<Connector Operation>");
                                                    }
                                                    if (tmpCSO.ConnectionTime != DateTime.MinValue)
                                                    {
                                                        newSysAttribs.Add("<Connect Time>");
                                                    }
                                                    if (tmpCSO.Connector.HasValue)
                                                    {
                                                        newSysAttribs.Add("<Connector>");
                                                    }
                                                    if (tmpCSO.ExportError != null)
                                                    {
                                                        newErrorAttribs.Add("<ExportErrorDetails>");
                                                    }
                                                }
                                                //if match update match counter
                                                counterMatchObjects++;
                                            }
                                        }
                                        //No filter selected - for initial loading of a file
                                        else
                                        {
                                            //update attribute lists
                                            if (tmpCSO.SynchronizedHologram != null)
                                            {
                                                newNonChangingAttribs.AddRange(tmpCSO.SynchronizedHologram.AttributeNames);
                                            }
                                            if (tmpCSO.ConnectorState != "")
                                            {
                                                newSysAttribs.Add("<Connector State>");
                                            }
                                            if (tmpCSO.DisconnectionTime != DateTime.MinValue)
                                            {
                                                newSysAttribs.Add("<Disconnect Time>");
                                            }
                                            if (tmpCSO.ConnectionOperation != "")
                                            {
                                                newSysAttribs.Add("<Connector Operation>");
                                            }
                                            if (tmpCSO.ConnectionTime != DateTime.MinValue)
                                            {
                                                newSysAttribs.Add("<Connect Time>");
                                            }
                                            if (tmpCSO.Connector.HasValue)
                                            {
                                                newSysAttribs.Add("<Connector>");
                                            }
                                            if (tmpCSO.ExportError != null)
                                            {
                                                newErrorAttribs.Add("<ExportErrorDetails>");
                                            }
                                        }
                                        //update total object counter
                                        counterCSObjects++;
                                        readSoFar = bsRead.Position;
                                        if (!stopProcessing && readSoFar >= updateInterval)
                                        {
                                            int temp = (int)((readSoFar / fileLength) * 200);
                                            this.methUpdateBar(frmProgress, (int)((readSoFar * 100) / fileLength));
                                            //increases updateInterval by 5%
                                            updateInterval += Convert.ToInt64(fileLength * .05);
                                        }
                                    }
                                    else
                                    {
                                        xmlRead.Read();
                                    }
                                }
                                if (makeReport && outFile != null)
                                {
                                    if (!generateCSVReport)
                                    {
                                        WriteHTMLEndReport(outFile);
                                    }
                                    outFile.Close();
                                    outFile.Dispose();
                                    makeReport = false;
                                }
                                if (filter != null && !stopProcessing)
                                {
                                    this.SetMatchObjectCount(counterMatchObjects);
                                    //update objTypes and operations list boxes
                                    if (filter.Level == FilterLevel.State)
                                    {
                                        UpdateObjectLB(objTypes.Distinct().ToArray());
                                    }
                                    if (filter.Level == FilterLevel.ObjectType && filter.FilterState != State.Synchronized)
                                    {
                                        UpdateOperationLB(ops.Distinct().ToArray());
                                    }
                                }
                            }
                        }
                    }
                    newChangingAttribs = newChangingAttribs.Distinct().ToList();
                    newNonChangingAttribs = newNonChangingAttribs.Distinct().ToList();
                    newNonChangingAttribs.Remove("DN");
                    newSysAttribs = newSysAttribs.Distinct().ToList();
                    newErrorAttribs = newErrorAttribs.Distinct().ToList();

                    //likely thoursands or hundreds of thousands of items in lists removed
                    //clean up excess address space
                    newChangingAttribs.TrimExcess();
                    newNonChangingAttribs.TrimExcess();
                    newSysAttribs.TrimExcess();
                    newErrorAttribs.TrimExcess();

                    //Done filtering, update global variables with new values
                    changingAttribs = newChangingAttribs;
                    nonChangingAttribs = newNonChangingAttribs;
                    sysAttribs = newSysAttribs;
                    errorAttribs = newErrorAttribs;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.handleException(ex, "Error occurred parsing file.  Ensure the file follows proper XML syntax.  Regenerate file if necessary");
                    Application.Exit();
                }
                if (!stopProcessing)
                {
                    this.SetTotalObjectCount(counterCSObjects);
                    this.methCloseForm(frmProgress);
                }
            }
            private void createAddcsObject(object xmlStr)
            {
                csObjects.Add(new csObject((string)xmlStr));
            }
        #endregion

        #region Progress functions
            private void methCloseForm(frmProgressBar frmProgress)
            {
                if (frmProgress.InvokeRequired)
                {
                    delCloseForm frm = new delCloseForm(methCloseForm);
                    this.Invoke(frm, frmProgress);
                }
                else
                {
                    frmProgress.DialogResult = DialogResult.OK;
                }
            }
            private void methUpdateBar(frmProgressBar frmProgress, int value)
            {
                if (frmProgress.InvokeRequired)
                {
                    delUpdateBar bar = new delUpdateBar(methUpdateBar);
                    this.Invoke(bar, frmProgress, value);
                }
                else
                {
                    frmProgress.updateBar(value);
                }
            }
            private void methSetText(frmProgressBar frmProgress, string value)
            {
                if (frmProgress.InvokeRequired)
                {
                    delSetText setTex = new delSetText(methSetText);
                    this.Invoke(setTex, frmProgress, value);
                }
                else
                {
                    frmProgress.setLblText(value);
                }
            }
        #endregion

        #region UI Filtering Controls
            private void getFilteringOptions()
            {
                foreach (csObject obj in csObjects)
                {
                    if (!pendingImportDeltasExist && obj.PendingImport != null)
                    {
                        pendingImportDeltasExist = true;
                    }
                    if (!unappliedExportDeltasExist && obj.UnappliedExport != null)
                    {
                        unappliedExportDeltasExist = true;
                    }
                    if (!escrowedExportDeltasExist && obj.EscrowedExport != null)
                    {
                        escrowedExportDeltasExist = true;
                    }
                    if (!unconfirmedExportDeltasExist && obj.UnconfirmedExport != null)
                    {
                        unconfirmedExportDeltasExist = true;
                    }
                    if (!synchronizationHologramExist && obj.SynchronizedHologram != null)
                    {
                        synchronizationHologramExist = true;
                    }
                    if (!pendingImportHologramExist && obj.PendingImportHologram != null)
                    {
                        pendingImportHologramExist = true;
                    }
                    if (!unappliedExportHologramExist && obj.UnappliedExportHologram != null)
                    {
                        unappliedExportHologramExist = true;
                    }
                    if (!escrowedExportHologramExist && obj.EscrowedExportHologram != null)
                    {
                        escrowedExportHologramExist = true;
                    }
                    if (!unconfirmedExportHologramExist && obj.UnconfirmedExportHologram != null)
                    {
                        unconfirmedExportHologramExist = true;
                    }
                    mvAttribs.AddRange(obj.MVAttribNames);
                }
                mvAttribs = mvAttribs.Distinct().ToList();
            }
            private void showFilterMessages()
            {
                if (csObjects.Count == 0)
                {
                    MessageBox.Show("No cs-objects found.  Verify file was created using csexport.exe");
                }
                else
                {
                    if (!pendingImportDeltasExist)
                    {
                        MessageBox.Show("No deltas exist with pending-import changes.  Filtering on pending-imports will be disabled.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (!pendingImportHologramExist)
                    {
                        MessageBox.Show("Deltas exist with pending-import changes, but pending-import holograms do not exist.  Pending-import holograms are required for attribute values"
                            + "\r\n\r\nFiltering on pending-imports will be disabled.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    if (!unappliedExportDeltasExist)
                    {
                        MessageBox.Show("No deltas exist with unapplied-export changes.  Filtering on unapplied-exports will be disabled.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (!unappliedExportHologramExist)
                    {
                        MessageBox.Show("Deltas exist with unapplied-export changes, but unapplied-export holograms do no exist.  Unapplied-export holograms are required for attribute values"
                            + "\r\n\r\nFiltering on unapplied-exports will be disabled.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    if (!escrowedExportDeltasExist)
                    {
                        MessageBox.Show("No deltas exist with escrowed-export changes.  Filtering on escrowed-exports will be disabled.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (!escrowedExportHologramExist)
                    {
                        MessageBox.Show("Deltas exist with escrowed-export changes, but escrowed-export holograms do no exist.  Escrowed-export holograms are required for attribute values"
                            + "\r\n\r\nFiltering on escrowed-exports will be disabled.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    if (!unconfirmedExportDeltasExist)
                    {
                        MessageBox.Show("No deltas exist with unconfirmed-export changes.  Filtering on unconfirmed-exports will be disabled.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (!unconfirmedExportHologramExist)
                    {
                        MessageBox.Show("Deltas exist with unconfirmed-export changes, but unconfirmed-export holograms do no exist.  Unconfirmed-export holograms are required for attribute values"
                            + "\r\n\r\nFiltering on unconfirmed-exports will be disabled.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    if (!synchronizationHologramExist)
                    {
                        MessageBox.Show("Sync holograms do no exist.  Filtering on current data will be disabled."
                            + "\r\n\r\nAll reports generated will be missing current attribute values.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            private void setFilterOptions()
            {
                if (csObjects.Count == 0)
                {
                    btnAddFilter.Enabled = false;
                    btnRemoveFilter.Enabled = false;
                    btnCreateReport.Enabled = false;
                    cbADMA.Enabled = false;
                    cbNonChanging.Enabled = false;
                    cbSystemAttribs.Enabled = false;
                    cbbAttributes.Enabled = false;
                    cbbComparators.Enabled = false;
                    cbbValue.Enabled = false;
                    lbAttribute.Enabled = false;
                    lbObjectType.Enabled = false;
                    lbOperation.Enabled = false;
                }
                if (!pendingImportDeltasExist || !pendingImportHologramExist)
                {
                    rbPendingImport.Enabled = false;
                }
                else
                {
                    rbPendingImport.Enabled = true;
                }
                if (!unappliedExportDeltasExist || !unappliedExportHologramExist)
                {
                    rbUnappliedExport.Enabled = false;
                }
                else
                {
                    rbUnappliedExport.Enabled = true;
                }
                if (!unconfirmedExportDeltasExist || !unconfirmedExportHologramExist)
                {
                    rbUnconfirmedExport.Enabled = false;
                }
                else
                {
                    rbUnconfirmedExport.Enabled = true;
                }
                if (!escrowedExportDeltasExist || !escrowedExportHologramExist)
                {
                    rbEscrowedExport.Enabled = false;
                }
                else
                {
                    rbEscrowedExport.Enabled = true;
                }
                if (!synchronizationHologramExist)
                {
                    rbSynchronized.Enabled = false;
                }
                else
                {
                    rbSynchronized.Enabled = true;
                }
            }
            private void fileLoadComplete(Object sender, EventArgs e)
            {
                if (this.InvokeRequired)
                {
                    LoadCompletedEventHandler LCEH = new LoadCompletedEventHandler(fileLoadComplete);
                    this.Invoke(LCEH, sender, e);
                }
                else
                {
                    lblTotalCount.Text = "Total Count: " + csObjects.Count;
                    setFilterOptions();
                    this.Enabled = true;
                }
            }
            private void SetTotalObjectCount(int count)
            {
                if (lblTotalCount.InvokeRequired)
                {
                    delSetCount setCount = new delSetCount(SetTotalObjectCount);
                    this.Invoke(setCount, count);
                }
                else
                {
                    lblTotalCount.Text = "Total Count: " + count;
                }
            }
            private void SetMatchObjectCount(int count)
            {
                if (lblCount.InvokeRequired)
                {
                    delSetCount setCount = new delSetCount(SetMatchObjectCount);
                    this.Invoke(setCount, count);
                }
                else
                {
                    lblCount.Text = "Match Count: " + count;
                }
            }
            private void UpdateObjectLB(string[] values)
            {
                if (lbObjectType.InvokeRequired)
                {
                    delUpdateLBs updateObjLB = new delUpdateLBs(UpdateObjectLB);
                    this.Invoke(updateObjLB, (object)values);
                }
                else
                {
                    lbObjectType.Items.AddRange(values);
                }
            }
            private void UpdateOperationLB(string[] values)
            {
                if (lbOperation.InvokeRequired)
                {
                    delUpdateLBs updateOpLB = new delUpdateLBs(UpdateOperationLB);
                    this.Invoke(updateOpLB, (object)values);
                }
                else
                {
                    lbOperation.Items.AddRange(values);
                }
            }
        #endregion 

        private void ClearFilterOptions()
        {
            rbUnappliedExport.Checked = false;
            rbPendingImport.Checked = false;
            rbSynchronized.Checked = false;
            rbEscrowedExport.Checked = false;
            rbUnconfirmedExport.Checked = false;
            lbObjectType.Items.Clear();
            lbOperation.Items.Clear();
            lbAttribute.Items.Clear();
            lblCount.Text = "Matching Count:";
            cbADMA.Checked = false;
            if (filter != null)
            {
                filter.Clear();
            }
            cbbAttributes.SelectedIndex = -1;
            cbbComparators.SelectedIndex = -1;
            cbbValue.Text = "";
        }
        private void SetAttribADvals(Entry hologram)
        {
            if (hologram != null)
            {
                foreach (Attribute attrib in hologram.Attributes)
                {
                    try
                    {
                        switch (attrib.Name)
                        {
                            case "accountExpires":
                                Int64 expiration = Convert.ToInt64(attrib.Values[0].Value, 16);
                                if (expiration != 0 && expiration != 9223372036854775807)
                                {
                                    DateTime curAcctExpires = DateTime.FromFileTimeUtc(expiration);
                                    hologram.Attributes[hologram.AttribIndexByName(attrib.Name)].Values[0].ADvalue = curAcctExpires.ToUniversalTime().ToString("g");
                                }
                                else
                                {
                                    hologram.Attributes[hologram.AttribIndexByName(attrib.Name)].Values[0].ADvalue = "Never";
                                }
                                break;
                            case "objectSid":
                                SecurityIdentifier sid = new SecurityIdentifier(System.Convert.FromBase64String(attrib.Values[0].Value).ToArray<byte>(), 0);
                                hologram.Attributes[hologram.AttribIndexByName(attrib.Name)].Values[0].ADvalue = sid.ToString();
                                break;
                            case "pwdLastSet":
                                Int64 lastSet = Convert.ToInt64(attrib.Values[0].Value, 16);
                                if (lastSet != 0)
                                {
                                    DateTime pwdLastSet = DateTime.FromFileTimeUtc(lastSet);
                                    hologram.Attributes[hologram.AttribIndexByName(attrib.Name)].Values[0].ADvalue = pwdLastSet.ToUniversalTime().ToString("g");
                                }
                                else
                                {
                                    hologram.Attributes[hologram.AttribIndexByName(attrib.Name)].Values[0].ADvalue = "Never";
                                }
                                break;
                            case "groupType":
                                long type = Convert.ToInt64(attrib.Values[0].Value, 16);
                                hologram.Attributes[hologram.AttribIndexByName(attrib.Name)].Values[0].ADvalue = type.ToString();
                                break;
                            case "userAccountControl":
                                int UAC = 0;
                                if (int.TryParse(attrib.Values[0].Value.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out UAC))
                                {
                                    hologram.Attributes[hologram.AttribIndexByName(attrib.Name)].Values[0].ADvalue = UAC.ToString();
                                }
                                else
                                {
                                    hologram.Attributes[hologram.AttribIndexByName(attrib.Name)].Values[0].ADvalue = "Error parsing current value";
                                }
                                break;
                            case "lastLogonTimestamp":
                                long time = 0;
                                if (long.TryParse(attrib.Values[0].Value.Replace("0x", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out time))
                                {
                                    hologram.Attributes[hologram.AttribIndexByName(attrib.Name)].Values[0].ADvalue = DateTime.FromFileTime(time).ToUniversalTime().ToString("g");
                                }
                                else
                                {
                                    hologram.Attributes[hologram.AttribIndexByName(attrib.Name)].Values[0].ADvalue = "Error parsing current value";
                                }
                                break;
                            case "createTimeStamp":
                                DateTime create;
                                if (DateTime.TryParseExact(attrib.Values[0].Value, "yyyyMMddHHmmss.0Z", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out create))
                                {
                                    hologram.Attributes[hologram.AttribIndexByName(attrib.Name)].Values[0].ADvalue = create.ToLocalTime().ToString("g");
                                }
                                else
                                {
                                    hologram.Attributes[hologram.AttribIndexByName(attrib.Name)].Values[0].ADvalue = "Error parsing current value";
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.handleException(ex, "Error occurred computing AD values.\r\nDN=" + hologram.DN + "\r\nAttribute=" + attrib.Name);
                        Application.Exit();
                    }
                }
            }
        }
        private void ShowGetDataForm()
        {
            ClearFilterOptions();
            stopProcessing = false;
            matchingCSobjects = null;
            csObjects = null;
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            matchingCSobjects = new ConcurrentBag<csObject>();
            csObjects = new CustomConcurrentBag<csObject>();
            this.Hide();
            this.Owner.Show();
        }
        private void UpdateAttributeLists()
        {
            try
            {
                changingAttribs.Clear();
                nonChangingAttribs.Clear();
                sysAttribs.Clear();
                sysAttribs.Add("<DN>");
                foreach (csObject obj in matchingCSobjects)
                {
                    if (filter.FilterState != State.Synchronized)
                    {
                        changingAttribs.AddRange(obj.Delta(filter.FilterState).AttributeNames);
                    }
                    if (obj.SynchronizedHologram != null)
                    {
                        nonChangingAttribs.AddRange(obj.SynchronizedHologram.AttributeNames);
                    }
                    if (obj.ConnectorState != "")
                    {
                        sysAttribs.Add("<Connector State>");
                    }
                    if (obj.DisconnectionTime != DateTime.MinValue)
                    {
                        sysAttribs.Add("<Disconnect Time>");
                    }
                    if (obj.ConnectionOperation != "")
                    {
                        sysAttribs.Add("<Connector Operation>");
                    }
                    if (obj.ConnectionTime != DateTime.MinValue)
                    {
                        sysAttribs.Add("<Connect Time>");
                    }
                    if (obj.Connector.HasValue)
                    {
                        sysAttribs.Add("<Connector>");
                    }
                    if (obj.ExportError != null)
                    {
                        errorAttribs.Add("<ExportErrorDetails>");
                    }
                }
                changingAttribs = changingAttribs.Distinct().ToList();
                nonChangingAttribs = nonChangingAttribs.Distinct().ToList();
                nonChangingAttribs.Remove("DN");
                sysAttribs = sysAttribs.Distinct().ToList();
                errorAttribs = errorAttribs.Distinct().ToList();

                //likely thoursands or hundreds of thousands of items in lists removed
                //clean up excess address space
                changingAttribs.TrimExcess();
                nonChangingAttribs.TrimExcess();
                sysAttribs.TrimExcess();
                errorAttribs.TrimExcess();
            }
            catch (Exception ex)
            {
                ExceptionHandler.handleException(ex, "Error occurred in updatingAttributeLists.");
                Application.Exit();
            }

        }
        private void SetFilterAttributes()
        {
            filter.AvailableAttributes.Clear();
            if (filter.FilterState == State.Synchronized || cbNonChanging.Checked)
            {
                filter.AvailableAttributes.AddRange(nonChangingAttribs);
            }
            if (filter.FilterState!= State.Synchronized)
            {
                filter.AvailableAttributes.AddRange(changingAttribs);
            }
            if (cbSystemAttribs.Checked)
            {
                filter.AvailableAttributes.AddRange(sysAttribs);
            }
            filter.AvailableAttributes = filter.AvailableAttributes.Distinct().ToList();
            filter.AvailableAttributes.Sort();
        }
        private void UpdateAttributeUI()
        {
            lbAttribute.Items.Clear();
            cbbAttributes.Items.Clear();
            switch (filter.Level)
            {
                case  FilterLevel.State:
                    lbAttribute.Items.AddRange(filter.AvailableAttributes.ToArray());
                    break;
                case FilterLevel.ObjectType:
                    lbAttribute.Items.AddRange(filter.AvailableAttributes.ToArray());
                    if (filter.FilterState == State.Synchronized)
                    {
                        cbbAttributes.Items.AddRange(filter.AvailableAttributes.ToArray());
                    }
                    break;
                case FilterLevel.Operation:
                    lbAttribute.Items.AddRange(filter.AvailableAttributes.ToArray());
                    cbbAttributes.Items.AddRange(filter.AvailableAttributes.ToArray());
                    break;
                case FilterLevel.AttributeValue:
                    lbAttribute.Items.AddRange(filter.AvailableAttributes.ToArray());
                    cbbAttributes.Items.AddRange(filter.AvailableAttributes.ToArray());
                    break;
            }
            if (errorAttribs.Count > 0)
            {
                lbAttribute.Items.AddRange(errorAttribs.ToArray());
                filter.AvailableAttributes.AddRange(errorAttribs.ToArray());
            }
        }

        #region public methods
            public void SetDateFilter(DateTime value)
            {
                cbbValue.Text = value.Date.ToShortDateString();
            }
            public void SetReportAttributes(List<string> reportAttributes)
            {
                filter.ReportAttributes = reportAttributes;
            }
            public void SetReportType(bool typeCSV)
            {
                generateCSVReport = typeCSV;
            }
        #endregion

        #endregion

    }
}