using System;
using System.Threading;

namespace IpWatchDog.Runners
{
    class ConsoleRunner : IRunner
    {
        private IService _service;
        private bool _isRunning;
        private object _monitor = new object();

        public ConsoleRunner(IService service)
        {
            if (service == null) throw new ArgumentNullException("service");
            _service = service;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnCancel);
        }

        public void Run()
        {
            if (_isRunning) return;

            lock (_monitor)
            {
                _isRunning = true;
                _service.Start();
                Monitor.Wait(_monitor);
            }
        }

        void OnCancel(object sender, ConsoleCancelEventArgs e)
        {
            if (!_isRunning) return;

            lock (_monitor)
            {
                _service.Stop();
                _isRunning = false;
                Monitor.PulseAll(_monitor);
            }
        }
    }
}
