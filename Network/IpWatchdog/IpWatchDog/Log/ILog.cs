namespace IpWatchDog.Log
{
    interface ILog
    {
        void Write(LogLevel level, string format, params object[] args);
    }
}
