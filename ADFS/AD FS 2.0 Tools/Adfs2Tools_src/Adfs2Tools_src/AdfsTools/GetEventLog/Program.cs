using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.ComponentModel;

namespace GetEventLog
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: GetEventLog activityId machines");
                Console.WriteLine("Example: GetEventLog \"0724A8D0-D873-4CB3-8762-C9A70084DD98\" \"server1.company.com,server2.company.com\"");
                return;
            }

            string activityId = args[0];
            string machineNames = args[1];
            
            string adfsAdminLogName = "AD FS 2.0/Admin";

            string[] machineNamesArray = machineNames.Split(new char[1] { ',' });
            foreach (string machineName in machineNamesArray)
            {
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("Getting AD FS event logs with correlation activity ID " + activityId + " from " + machineName);

                string query = "*[System/Correlation/@ActivityID=\"{" + activityId + "}\"]";
                EventLogQuery eventLogQuery = new EventLogQuery(adfsAdminLogName, PathType.LogName, query);
                eventLogQuery.Session = new EventLogSession(machineName);
                EventLogReader eventLogReader = new EventLogReader(eventLogQuery);

                for (EventRecord eventRecord = eventLogReader.ReadEvent(); eventRecord != null; eventRecord = eventLogReader.ReadEvent())
                {
                    //Console.WriteLine("eventRecord.ActivityId=" + eventRecord.ActivityId);
                    Console.WriteLine();
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    Console.WriteLine("Event ID: " + eventRecord.Id);
                    Console.WriteLine("Logged: " + eventRecord.TimeCreated);
                    Console.WriteLine(eventRecord.FormatDescription());
                }
            }
        }
    }
}
