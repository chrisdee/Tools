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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace csReporter
{
    public partial class frmProgressBar : Form
    {

        //Graphics percentage;
        bool closing;

        public frmProgressBar()
        {
            InitializeComponent();

            //percentage = pbProcessing.CreateGraphics();
            closing = false;
        }
        
        public void updateBar(int val)
        {
            if (!closing)
            {
                pbProcessing.Value = val;
                lblProgress.Text = val.ToString() + "%";
            }
        }

        public void setLblText(string val)
        {
            lblAction.Text = val;
        }


        private void frmProgressBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                frmFilter.stopProcessing = true;
            }
            closing = true;
        }
    }
}
