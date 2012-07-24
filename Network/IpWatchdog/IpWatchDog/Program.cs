using System;
using System.ServiceProcess;
using IpWatchDog.Runners;
using IpWatchDog.Log;
using IpWatchDog.Install;

namespace IpWatchDog
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("This is a service. Run with -h switch to see usage");
                Run(false);
                return;
            }

            switch (args[0])
            {
                case "-c":
                case "-console":
                    Run(true);
                    break;

                case "-i":
                case "-install":
                    Install(false);
                    break;

                case "-u":
                case "-uninstall":
                    Install(true);
                    break;

                case "-s":
                case "-start":
                    StartService(true);
                    break;

                case "-p":
                case "-stop":
                    StartService(false);
                    break;

                default:
                    Usage();
                    break;
            }
        }

        private static void Run(bool isConsole)
        {
            new Configurator(isConsole).CreateRunner().Run();
        }

        private static void Install(bool undo)
        {
            new Configurator(true).CreateInstaller().Install(undo);
        }

        private static void StartService(bool start)
        {
            var controller = new Configurator(true).CreateServiceController();

            if (start)
            {
                controller.Start();
            }
            else
            {
                controller.Stop();
            }
        }

        private static void Usage()
        {
            Console.WriteLine(@"IP Watchdog service: monitors computer's external IP and sends e-mail when it changes.

Command line arguments:
    -i or -install : install the service (must be an administrator)
    -u or -uninstall: uninstall the service (must be an administrator)
    -s or -start: start the service (must be an administrator)
    -p or -stop: stop running service (must be an administrator)
    -c or -console: run in console mode");
        }
    }
}
