using System.Threading;
using IpWatchDog.Log;

namespace IpWatchDog
{
    class IpWatchDogService : IService
    {
        IIpPersistor _persistor;
        IIpRetriever _retriever;
        IIpNotifier _notifier;
        AppConfig _config;

        Timer _timer;
        object _isBusy = new object();
        bool _stopRequested;
        ILog _log;
        string _currentIp;

        public IpWatchDogService(ILog log, AppConfig config, IIpPersistor persistor, IIpRetriever retriever, IIpNotifier notifier)
        {
            _log = log;
            _persistor = persistor;
            _retriever = retriever;
            _notifier = notifier;
            _config = config;
        }

        public void Start()
        {
            _currentIp = _persistor.GetIp();
            _log.Write(LogLevel.Info, "IP Watchdog service is starting. Current IP is {0}", _currentIp ?? "undefined");
            
            _timer = new Timer(CheckIp, null, 0, _config.PollingTimeoutSeconds*1000);
        }

        public void Stop()
        {
            _log.Write(LogLevel.Info, "IP Watchdog service is stopping");
            _timer.Dispose();
            _timer = null;
            _stopRequested = true;
            if (!Monitor.TryEnter(_isBusy, 5000))
            {
                _log.Write(LogLevel.Warning, "IP checking process is still running and will be forcefully terminated");
            }

            _stopRequested = false;
        }

        private void CheckIp(object unused)
        {
            lock (_isBusy)
            {
                if (_stopRequested) return;
                CheckIp();
            }
        }

        private void CheckIp()
        {
            var newIp = _retriever.GetIp();

            if (newIp == null) return;
            if (newIp == _currentIp) return;

            if (_currentIp == null)
            {
                _log.Write(LogLevel.Info, "Currrent IP is {0}", newIp);
            }
            else
            {
                _notifier.OnIpChanged(_currentIp, newIp);
            }

            _currentIp = newIp;
            _persistor.SaveIp(_currentIp);
        }
    }
}
