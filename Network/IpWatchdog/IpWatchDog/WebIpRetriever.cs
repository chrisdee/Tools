using System;
using System.IO;
using System.Net;
using IpWatchDog.Log;

namespace IpWatchDog
{
    class WebIpRetriever : IIpRetriever
    {
        ILog _log;

        public WebIpRetriever(ILog log)
        {
            log = _log;
        }

        public string GetIp()
        {
            try
            {
                var request = HttpWebRequest.Create("http://checkip.dyndns.org/");
                request.Method = "GET";
                
                var response = request.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var answer = reader.ReadToEnd(); // should have better handling here for very long responses
                    return ExtractIp(answer);
                }
            }
            catch (Exception ex)
            {
                _log.Write(LogLevel.Warning, "Could not retrieve current IP from web. {0}", ex);
                return null;
            }
        }

        private string ExtractIp(string answer)
        {
            var pattern = "Current IP Address: ";
            var idx = answer.IndexOf(pattern);
            if (idx == -1) return null;
            var start = idx + pattern.Length;
            var end = answer.IndexOf("<", start);
            if (end == -1) return null;
            return answer.Substring(start, end - start);
        }
    }
}
