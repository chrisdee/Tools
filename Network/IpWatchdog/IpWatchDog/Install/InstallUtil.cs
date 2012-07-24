using System;
using System.Collections;
using System.Configuration.Install;
using IpWatchDog.Log;

namespace IpWatchDog.Install
{
    class InstallUtil
    {
        ILog _log;

        public InstallUtil(ILog log)
        {
            _log = log;
        }

        public void Install(bool undo)
        {
            try 
            { 
                using (var installer = new AssemblyInstaller(typeof(Program).Assembly, new[] {"/LogToConsole=false"}))
                { 
                    IDictionary state = new Hashtable(); 
                    installer.UseNewContext = true;
                    Install(installer, state, undo);
                }
            }
            catch (Exception ex) 
            {
                _log.Write(LogLevel.Error, ex.ToString());
            } 
        }

        private void Install(AssemblyInstaller installer, IDictionary state, bool undo)
        {

            try
            {
                if (undo)
                {
                    _log.Write(LogLevel.Info, "Uninstalling {0}...", StringConstants.ServiceName);
                    installer.Uninstall(state);
                    _log.Write(LogLevel.Info, "{0} has been successfully removed from the system.", StringConstants.ServiceName);
                }
                else
                {
                    _log.Write(LogLevel.Info, "Installing {0}...", StringConstants.ServiceName);
                    installer.Install(state);

                    _log.Write(LogLevel.Info, "Commiting changes...");
                    installer.Commit(state);

                    _log.Write(LogLevel.Info, "Install succeeded.");
                }
            }
            catch (Exception ex)
            {
                _log.Write(LogLevel.Error, "An error occured during {1}. {0}", ex, undo?"uninstall" : "install");
                _log.Write(LogLevel.Info, "Trying to roll back...");
                TryRollback(installer, state);
            }
        }

        private void TryRollback(AssemblyInstaller installer, IDictionary state)
        {
            try { installer.Rollback(state); }
            catch (Exception ex )
            {
                _log.Write(LogLevel.Warning, "An error occured during rollback. {0}", ex);
            }

        }
    } 
}
