using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace ArchiveAuditLogs
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: ArchiveAuditLogs fromDate toDate machines");
                Console.WriteLine("Example: ArchiveAuditLogs 2011-10-05 2011-10-05 \"server1.company.com,server2.company.com\"");
                return;
            }

            DateTime fromDate = DateTime.Parse(args[0]);
            DateTime toDate = DateTime.Parse(args[1]);
            string machineNames = args[2];
            string securityLogName = "Security";
            string[] machineNamesArray = machineNames.Split(new char[1] { ',' });
            string fileName = "AdfsAudit-" + DateTime.Now.ToString("yyyyMMdd") + ".xml";

            StreamWriter file;
            if (!File.Exists(".\\" + fileName))
            {
                file = new StreamWriter(".\\" + fileName);
            }
            else
            {
                Console.WriteLine("File " + fileName + " already exists.");
                return;
            }

            try
            {
                foreach (string machineName in machineNamesArray)
                {
                    Console.WriteLine("Getting AD FS audit logs from " + machineName + " and writing into " + fileName);

                    string query = "*[System[" + 
                        "(Provider/@Name=\"AD FS 2.0 Auditing\") and " +
                        "(TimeCreated/@SystemTime <= \"" + toDate.ToString("yyyy-MM-ddTHH:mm:ss") + "\") and " +
                        "(TimeCreated/@SystemTime >= \"" + fromDate.ToString("yyyy-MM-ddTHH:mm:ss") + "\")" +
                        "]]";
                    //" and (TimeCreated/@SystemTime <= " + toDate.Ticks + ")]]";
                    //" and TimeCreated[timediff(@SystemTime) <= 86400000]]]";  
                   
                    Console.WriteLine("query=" + query);

                    /*
                     * This is not convinient, because it creates file on the target server
                    EventLogSession eventLogSession = new EventLogSession(machineName);
                    eventLogSession.ExportLogAndMessages("Security", PathType.LogName, query, fileName);
                    */
                    
                    EventLogQuery eventLogQuery = new EventLogQuery(securityLogName, PathType.LogName, query);
                    eventLogQuery.Session = new EventLogSession(machineName);
                    EventLogReader eventLogReader = new EventLogReader(eventLogQuery);

                    for (EventRecord eventRecord = eventLogReader.ReadEvent(); eventRecord != null; eventRecord = eventLogReader.ReadEvent())
                    {
                        file.WriteLine(eventRecord.ToXml());
                    }
                }
            }
            finally
            {
                file.Flush();
                file.Close();
                file.Dispose();
            }

        }
    }
}
