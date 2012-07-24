using System;

namespace IpWatchDog.Log
{
    class ConsoleLog : ILog
    {
        public void Write(LogLevel level, string format, params object[] args)
        {
            Console.Write(level.ToString().ToUpper());
            Console.Write(": ");
            Console.WriteLine(format, args);
        }
    }
}
