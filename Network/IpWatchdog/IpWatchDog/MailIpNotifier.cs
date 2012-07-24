using System;
using System.Net.Mail;
using IpWatchDog.Log;

namespace IpWatchDog
{
    class MailIpNotifier : IIpNotifier
    {
        ILog _log;
        AppConfig _config;

        public MailIpNotifier(ILog log, AppConfig config)
        {
            _log = log;
            _config = config;
        }

        public void OnIpChanged(string oldIp, string newIp)
        {
            string msg = GetMessage(oldIp, newIp);
            _log.Write(LogLevel.Warning, msg);

            try
            {
                var smtpClient = new SmtpClient(_config.SmtpHost);
                smtpClient.Send(
                    _config.MailFrom,
                    _config.MailTo,
                    "IP change",
                    msg);
            }
            catch (Exception ex)
            {
                _log.Write(LogLevel.Error, "Error sending e-mail. {0}", ex);
            }
        }

        private static string GetMessage(string oldIp, string newIp)
        {
            return String.Format("IP changed from {0} to {1}", oldIp, newIp);
        }
    }
}
