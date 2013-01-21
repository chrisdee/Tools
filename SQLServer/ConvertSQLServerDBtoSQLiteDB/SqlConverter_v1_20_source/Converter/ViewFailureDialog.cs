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
    public partial class ViewFailureDialog : Form
    {
        public ViewFailureDialog()
        {
            InitializeComponent();
        }

        public ViewSchema View
        {
            get { return _view; }
            set
            {
                _view = value;
                this.Text = "SQL Error: "+_view.ViewName;
                txtSQL.Text = _view.ViewSQL;
            }
        }

        public string ViewSQL
        {
            get { return txtSQL.Text; }
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private ViewSchema _view;
    }
}