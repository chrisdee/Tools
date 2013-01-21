using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using DbAccess;

namespace Converter
{
    public partial class MainForm : Form
    {
        #region Constructor
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handler
        private void btnBrowseSQLitePath_Click(object sender, EventArgs e)
        {
            DialogResult res = saveFileDialog1.ShowDialog(this);
            if (res == DialogResult.Cancel)
                return;

            string fpath = saveFileDialog1.FileName;
            txtSQLitePath.Text = fpath;
            pbrProgress.Value = 0;
            lblMessage.Text = string.Empty;
        }

        private void cboDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSensitivity();
            pbrProgress.Value = 0;
            lblMessage.Text = string.Empty;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
            	string constr;
            	if (cbxIntegrated.Checked) {
            		constr = GetSqlServerConnectionString(txtSqlAddress.Text, "master");
            	} else {
            		constr = GetSqlServerConnectionString(txtSqlAddress.Text, "master", txtUserDB.Text, txtPassDB.Text);
            	}
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();

                    // Get the names of all DBs in the database server.
                    SqlCommand query = new SqlCommand(@"select distinct [name] from sysdatabases", conn);
                    using (SqlDataReader reader = query.ExecuteReader())
                    {
                        cboDatabases.Items.Clear();
                        while (reader.Read())
                            cboDatabases.Items.Add((string)reader[0]);
                        if (cboDatabases.Items.Count > 0)
                            cboDatabases.SelectedIndex = 0;
                    } // using
                } // using

                cboDatabases.Enabled = true;

                pbrProgress.Value = 0;
                lblMessage.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    ex.Message,
                    "Failed To Connect",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            } // catch
        }

        private void txtSQLitePath_TextChanged(object sender, EventArgs e)
        {
            UpdateSensitivity();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateSensitivity();

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = "SQL Server To SQLite DB Converter (" + version + ")";
        }

		private void txtSqlAddress_TextChanged(object sender, EventArgs e)
        {
            UpdateSensitivity();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SqlServerToSQLite.CancelConversion();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SqlServerToSQLite.IsActive)
            {
                SqlServerToSQLite.CancelConversion();
                _shouldExit = true;
                e.Cancel = true;
            }
            else
                e.Cancel = false;
        }

        private void cbxEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSensitivity();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            UpdateSensitivity();
        }

        private void ChkIntegratedCheckedChanged(object sender, EventArgs e)
        {
            if (cbxIntegrated.Checked)
            {
                lblPassword.Visible = false;
                lblUser.Visible = false;
                txtPassDB.Visible = false;
                txtUserDB.Visible = false;
            }
            else
            {
                lblPassword.Visible = true;
                lblUser.Visible = true;
                txtPassDB.Visible = true;
                txtUserDB.Visible = true;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
        	string sqlConnString;
        	if (cbxIntegrated.Checked) {
        		sqlConnString = GetSqlServerConnectionString(txtSqlAddress.Text, (string)cboDatabases.SelectedItem);
        	} else {
        		sqlConnString = GetSqlServerConnectionString(txtSqlAddress.Text, (string)cboDatabases.SelectedItem, txtUserDB.Text, txtPassDB.Text);
        	}
            bool createViews = cbxCreateViews.Checked;
        	
            string sqlitePath = txtSQLitePath.Text.Trim();
            this.Cursor = Cursors.WaitCursor;
            SqlConversionHandler handler = new SqlConversionHandler(delegate(bool done,
                bool success, int percent, string msg) {
                    Invoke(new MethodInvoker(delegate() {
                        UpdateSensitivity();
                        lblMessage.Text = msg;
                        pbrProgress.Value = percent;

                        if (done)
                        {
                            btnStart.Enabled = true;
                            this.Cursor = Cursors.Default;
                            UpdateSensitivity();

                            if (success)
                            {
                                MessageBox.Show(this,
                                    msg,
                                    "Conversion Finished",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                                pbrProgress.Value = 0;
                                lblMessage.Text = string.Empty;
                            }
                            else
                            {
                                if (!_shouldExit)
                                {
                                    MessageBox.Show(this,
                                        msg,
                                        "Conversion Failed",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                                    pbrProgress.Value = 0;
                                    lblMessage.Text = string.Empty;
                                }
                                else
                                    Application.Exit();
                            }
                        }
                    }));
            });
            SqlTableSelectionHandler selectionHandler = new SqlTableSelectionHandler(delegate(List<TableSchema> schema)
            {
                List<TableSchema> updated = null;
                Invoke(new MethodInvoker(delegate
                {
                    // Allow the user to select which tables to include by showing him the 
                    // table selection dialog.
                    TableSelectionDialog dlg = new TableSelectionDialog();
                    DialogResult res = dlg.ShowTables(schema, this);
                    if (res == DialogResult.OK)
                        updated = dlg.IncludedTables;
                }));
                return updated;
            });

            FailedViewDefinitionHandler viewFailureHandler = new FailedViewDefinitionHandler(delegate(ViewSchema vs)
            {
                string updated = null;
                Invoke(new MethodInvoker(delegate
                {
                    ViewFailureDialog dlg = new ViewFailureDialog();
                    dlg.View = vs;
                    DialogResult res = dlg.ShowDialog(this);
                    if (res == DialogResult.OK)
                        updated = dlg.ViewSQL;
                    else
                        updated = null;
                }));

                return updated;
            });

            string password = txtPassword.Text.Trim();
            if (!cbxEncrypt.Checked)
                password = null;
            SqlServerToSQLite.ConvertSqlServerToSQLiteDatabase(sqlConnString, sqlitePath, password, handler, 
                selectionHandler, viewFailureHandler, cbxTriggers.Checked, createViews);
        }

        #endregion

        #region Private Methods
        private void UpdateSensitivity()
        {
            if (txtSQLitePath.Text.Trim().Length > 0 && cboDatabases.Enabled &&
                (!cbxEncrypt.Checked || txtPassword.Text.Trim().Length > 0))
                btnStart.Enabled = true && !SqlServerToSQLite.IsActive;
            else
                btnStart.Enabled = false;

            btnSet.Enabled = txtSqlAddress.Text.Trim().Length > 0 && !SqlServerToSQLite.IsActive;
            btnCancel.Visible = SqlServerToSQLite.IsActive;
            txtSqlAddress.Enabled = !SqlServerToSQLite.IsActive;
            txtSQLitePath.Enabled = !SqlServerToSQLite.IsActive;
            btnBrowseSQLitePath.Enabled = !SqlServerToSQLite.IsActive;
            cbxEncrypt.Enabled = !SqlServerToSQLite.IsActive;
            cboDatabases.Enabled = cboDatabases.Items.Count > 0 && !SqlServerToSQLite.IsActive;
            txtPassword.Enabled = cbxEncrypt.Checked && cbxEncrypt.Enabled;
            cbxIntegrated.Enabled = !SqlServerToSQLite.IsActive;
            cbxCreateViews.Enabled = !SqlServerToSQLite.IsActive;
            cbxTriggers.Enabled = !SqlServerToSQLite.IsActive;
            txtPassDB.Enabled = !SqlServerToSQLite.IsActive;
            txtUserDB.Enabled = !SqlServerToSQLite.IsActive;
        }

        private static string GetSqlServerConnectionString(string address, string db)
        {
            string res = @"Data Source=" + address.Trim() +
                    ";Initial Catalog="+db.Trim()+";Integrated Security=SSPI;";
            return res;
        }
        private static string GetSqlServerConnectionString(string address, string db, string user, string pass)
        {
            string res = @"Data Source=" + address.Trim() +
            	";Initial Catalog="+db.Trim()+";User ID=" + user.Trim() + ";Password=" + pass.Trim();
            return res;
        }
        #endregion

        #region Private Variables
        private bool _shouldExit = false;
        #endregion        
    }
}