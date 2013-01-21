using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DbAccess;

namespace Converter
{
    /// <summary>
    /// The dialog allows the user to select which tables to include in the 
    /// converstion process.
    /// </summary>
    public partial class TableSelectionDialog : Form
    {
        #region Constructors
        public TableSelectionDialog()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Returns the list of included table schema objects.
        /// </summary>
        public List<TableSchema> IncludedTables
        {
            get
            {
                List<TableSchema> res = new List<TableSchema>();
                foreach (DataGridViewRow row in grdTables.Rows)
                {
                    bool include = (bool)row.Cells[0].Value;
                    if (include)
                        res.Add((TableSchema)row.Tag);
                } // foreach

                return res;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Opens the table selection dialog and uses the specified schema list in order
        /// to update the tables grid.
        /// </summary>
        /// <param name="schema">The DB schema to display in the grid</param>
        /// <param name="owner">The owner form</param>
        /// <returns>dialog result according to user decision.</returns>
        public DialogResult ShowTables(List<TableSchema> schema, IWin32Window owner)
        {
            UpdateGuiFromSchema(schema);
            return this.ShowDialog(owner);
        }
        #endregion

        #region Event Handlers
        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grdTables.Rows)
            {
                // Uncheck the [V] for this row.
                row.Cells[0].Value = false;
            } // foreach
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grdTables.Rows)
            {
                // Check the [V] for this row.
                row.Cells[0].Value = true;
            } // foreach
        }
        #endregion

        #region Private Methods
        private void UpdateGuiFromSchema(List<TableSchema> schema)
        {
            grdTables.Rows.Clear();
            foreach (TableSchema table in schema)
            {
                grdTables.Rows.Add(true, table.TableName);
                grdTables.Rows[grdTables.Rows.Count - 1].Tag = table;
            } // foreach
        }
        #endregion
    }
}