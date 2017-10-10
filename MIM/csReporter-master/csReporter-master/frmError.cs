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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace csReporter
{
    public partial class frmError : Form
    {
        public frmError(string errorBanner, string errorMessage)
        {
            InitializeComponent();
            errorBanner = errorBanner.Replace(".  ", ".\r\n");
            tbErrorBanner.Text = errorBanner;
            Size bannerSize = TextRenderer.MeasureText(tbErrorBanner.Text, tbErrorBanner.Font);
            if (bannerSize.Width < tbErrorBanner.ClientSize.Width)
            {
                bannerSize.Width = tbErrorBanner.ClientSize.Width;
            }
            tbErrorBanner.ClientSize = new Size(bannerSize.Width, bannerSize.Height);


            tbErrorInfo.Text = errorMessage;
            Size textSize = TextRenderer.MeasureText(tbErrorInfo.Text, tbErrorInfo.Font);
            tbErrorInfo.ClientSize = new Size(bannerSize.Width, textSize.Height);
            bool needSB = tbErrorInfo.ClientSize.Width < textSize.Width;
            if (needSB)
            {
                tbErrorInfo.ScrollBars = ScrollBars.Vertical;
            }
        }
    }
}
