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

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace csReporter
{
    public partial class frmGetData : Form
    {
        frmFilter reporter;
        string csExportFilePath;
        string generatedFile;

        public frmGetData()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (ofdCSfile.ShowDialog() == DialogResult.OK)
                {
                    if (ofdCSfile.FileName == string.Empty)
                    {
                        MessageBox.Show("No file selected");
                    }
                    else
                    {
                        tbFile.Text = ofdCSfile.SafeFileName;
                        reporter = new frmFilter(ofdCSfile.FileName);
                        this.Hide();
                        reporter.Owner = this;
                        reporter.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.handleException(ex, "Error occurred getting file name from dialog");
                Application.Exit();
            }
        }

        private void rbFile_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFile.Checked)
            {
                tbFile.Enabled = true;
                btnOpenFile.Enabled = true;
            }
            else
            {
                tbFile.Enabled = false;
                btnOpenFile.Enabled = false;
            }
        }

        private void rbGenerate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbGenerate.Checked)
                {
                    try
                    {
                        //get install path from registry
                        string InstallPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Forefront Identity Manager\2010\Synchronization Service", "Location", null);
                        if (InstallPath != null)
                        {
                            csExportFilePath = InstallPath + @"\Bin\csexport.exe";
                            if (!File.Exists(csExportFilePath))
                            {
                                MessageBox.Show("Unable to locate the csexport utility in the default folder\r\n\r\n" + csExportFilePath);
                                rbFile.Checked = true;
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("The FIM Sync Service doesn't appear to be installed on this system.");
                            rbFile.Checked = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.handleException(ex, "Error occurred checking if FIM is installed on this system."
                        + "  Automatic creation of csexport data is unavailable.");
                        rbFile.Checked = true;
                        return;
                    }
                    ManagementScope scope = new ManagementScope(@"\\.\root\MicrosoftIdentityIntegrationServer");
                    SelectQuery query = new SelectQuery("select * from MIIS_ManagementAgent");
                    string[] maNames;

                    try
                    {
                        using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                        {
                            ManagementObjectCollection MAs = searcher.Get();
                            maNames = (from ManagementObject MA in MAs select MA["Name"].ToString()).ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.handleException(ex, "Error getting management agent names via WMI."
                            + "  Automatic creation of csexport data is unavailable.");
                        rbFile.Checked = true;
                        return;
                    }
                    Array.Sort(maNames);
                    cbbMAs.Items.Clear();
                    cbbMAs.Items.AddRange(maNames);

                    cbbMAs.Enabled = true;
                    gbDataSelection.Enabled = true;
                    btnGenerate.Enabled = true;
                }
                else
                {
                    cbbMAs.Enabled = false;
                    gbDataSelection.Enabled = false;
                    btnGenerate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.handleException(ex, "Error checking for FIM install and getting management agent names via WMI."
                    + "  Automatic creation of csexport data is unavailable.");
                Application.Exit();
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cbbMAs.SelectedIndex == -1)
            {
                MessageBox.Show("You must select an MA from the list.");
                return;
            }
            RadioButton selectedRB;
            try { selectedRB = gbDataSelection.Controls.OfType<RadioButton>().First(r => r.Checked); }
            catch (InvalidOperationException)
            {
                MessageBox.Show("You must select the data you wish to report on");
                return;
            }

            if (sfdReport.ShowDialog() == DialogResult.OK)
            {
                Process proc = new Process();
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.FileName = csExportFilePath;
                generatedFile = sfdReport.FileName;
                string subElements = "";
                if (!cbSystem.Checked)
                {
                    subElements = " /o:bhd";
                }
                if (File.Exists(generatedFile)) { File.Delete(generatedFile); }
                switch (selectedRB.Name)
                {
                    case "rbAll":
                        proc.StartInfo.Arguments = "\"" + cbbMAs.SelectedItem.ToString() + "\" \"" + generatedFile + "\"" + subElements;
                        break;
                    case "rbExport":
                        proc.StartInfo.Arguments = "\"" + cbbMAs.SelectedItem.ToString() + "\" \"" + generatedFile + "\" /f:x" + subElements;
                        break;
                    case "rbImport":
                        proc.StartInfo.Arguments = "\"" + cbbMAs.SelectedItem.ToString() + "\" \"" + generatedFile + "\" /f:m" + subElements;
                        break;
                    case "rbImportError":
                        proc.StartInfo.Arguments = "\"" + cbbMAs.SelectedItem.ToString() + "\" \"" + generatedFile + "\" /f:i" + subElements + "e";
                        break;
                    case "rbExportError":
                        proc.StartInfo.Arguments = "\"" + cbbMAs.SelectedItem.ToString() + "\" \"" + generatedFile + "\" /f:e" + subElements + "e";
                        break;
                }
                proc.Start();
                proc.WaitForExit();
                reporter = new frmFilter(generatedFile);
                this.Hide();
                reporter.Owner = this;
                reporter.Show();
            }
        }

        private void btnShowExamples_Click(object sender, EventArgs e)
        {
            frmCSExamples examples = new frmCSExamples();
            examples.ShowDialog();
            examples.Dispose();
        }
        
        private void frmGetData_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && reporter != null)
            {
                reporter.Close();
                System.GC.Collect();
            }
        }

        private void cbSystem_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSystem.Checked)
            {
                MessageBox.Show("Including system attributes in the export will greatly increase the length of time required for the export to complete.");
            }
        }
    }
}
