using System;
using System.Configuration;

namespace IpWatchDog
{
    class AppConfig
    {
        public int PollingTimeoutSeconds
        {
            get { return Int32.Parse(Config("PollingTimeoutSeconds")); }
        }

        public string MailFrom
        {
            get { return Config("MailFrom"); }
        }

        public string MailTo
        {
            get { return Config("MailTo"); }
        }

        public string SmtpHost
        {
            get { return Config("SmtpHost"); }
        }

        public string Subject
        {
            get { return Config("Subject"); }
        }

        private string Config(string arg)
        {
            return ConfigurationManager.AppSettings[arg];
        }
    }
}
