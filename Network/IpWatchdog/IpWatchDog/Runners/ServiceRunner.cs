using System;
using System.ServiceProcess;

namespace IpWatchDog.Runners
{
    class ServiceRunner : ServiceBase, IRunner
    {
        IService _service;

        public ServiceRunner(IService service, string serviceName)
        {
            if (service == null) throw new ArgumentNullException("service");
            if (serviceName == null) throw new ArgumentNullException("serviceName");
            _service = service;
            ServiceName = serviceName;
        }

        public void Run()
        {
            ServiceBase.Run(this);
        }

        protected override void OnStart(string[] args)
        {
            _service.Start();
        }

        protected override void OnStop()
        {
            _service.Stop();
        }
    }
}
