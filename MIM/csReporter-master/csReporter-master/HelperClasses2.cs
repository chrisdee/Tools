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


//using System;
//using System.Collections.Generic;
//using System.Collections.Concurrent;
//using System.ComponentModel;
//using System.Text;
//using System.Xml;



//namespace csReporter
//{
//    static class ExceptionHandler
//    {
//        private static string getErrorMessage(Exception ex, int depth)
//        {
//            string erMsg = "\r\n\r\n";
//            if (depth > 0)
//            {
//                erMsg += "****Inner Exception****\r\n";
//            }
//            if (depth < 10)
//            {
//                if (ex.Message != string.Empty)
//                {
//                    erMsg += "Exeception message:\r\n" + ex.Message;
//                }
//                erMsg += "\r\n\r\nException Type: " + ex.GetType().ToString();
//                if (ex.StackTrace != null)
//                {
//                    erMsg += "\r\n\r\nStack trace:\r\n" + ex.StackTrace;
//                }
//                if (ex.InnerException != null)
//                {
//                    erMsg += getErrorMessage(ex.InnerException, ++depth);
//                }
//            }
//            else
//            {
//                erMsg = "\r\n\r\n######## More than 10 exceptions.  Too many to display.########";
//            }
//            return erMsg;
//        }

//        public static void handleException(Exception ex, string bannerMsg)
//        {
//            string errorInfo = getErrorMessage(ex, 0);
//            frmError errorForm = new frmError(bannerMsg, errorInfo);
//            errorForm.ShowDialog();
//            errorForm.Dispose();
//        }
//    }

//    public delegate void LoadCompletedEventHandler(object sender, EventArgs e);

//    class CustomConcurrentBag<T> : ConcurrentBag<T>
//    {
//        private bool loadingComplete;
//        public event LoadCompletedEventHandler FinishedLoading;

//        public bool LoadingComplete
//        {
//            get
//            {
//                return loadingComplete;
//            }
//            set
//            {
//                loadingComplete = value;
//                if (this.FinishedLoading != null)
//                {
//                    FinishedLoading(this, EventArgs.Empty);
//                }
//            }
//        }
//    }
//}