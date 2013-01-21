using System;
using System.Collections.Generic;
using System.Windows.Forms;
using log4net;
using log4net.Config;

// Configure LOG4NET Using configuration file.
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Converter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            BasicConfigurator.Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}