using System;
using System.ServiceProcess;
using IpWatchDog.Log;

namespace IpWatchDog
{
    class ServiceController : IService
    {
        System.ServiceProcess.ServiceController _controller;
        ILog _log;

        public ServiceController(ILog log)
        {
            _controller = new System.ServiceProcess.ServiceController(StringConstants.ServiceName);
            _log = log;
        }

        public void Start()
        {
            try
            {
                _log.Write(LogLevel.Info, "Starting service...");
                const int timeout = 10000;
                _controller.Start();

                var targetStatus = ServiceControllerStatus.Running;
                _controller.WaitForStatus(targetStatus, TimeSpan.FromMilliseconds(timeout));

                if (_controller.Status == targetStatus)
                {
                    _log.Write(LogLevel.Info, "Started");
                }
                else
                {
                    _log.Write(LogLevel.Warning, "Failed, service status is {0}", _controller.Status);
                }
            }
            catch (Exception ex)
            {
                _log.Write(LogLevel.Error, "Could not start service. {0}", ex);
            }
        }

        public void Stop()
        {
            try
            {
                _log.Write(LogLevel.Info, "Stopping service...");
                const int timeout = 10000;
                _controller.Stop();

                var targetStatus = ServiceControllerStatus.Stopped;
                _controller.WaitForStatus(targetStatus, TimeSpan.FromMilliseconds(timeout));

                if (_controller.Status == targetStatus)
                {
                    _log.Write(LogLevel.Info, "Stopped");
                }
                else
                {
                    _log.Write(LogLevel.Warning, "Failed, service status is {0}", _controller.Status);
                }
            }
            catch (Exception ex)
            {
                _log.Write(LogLevel.Error, "Could not stop service. {0}", ex);
            }
        }

    }
}
