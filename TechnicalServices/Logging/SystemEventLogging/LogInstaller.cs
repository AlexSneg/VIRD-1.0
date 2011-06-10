using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;

namespace TechnicalServices.Logging.SystemEventLogging
{
    [RunInstaller(true)]
    public class LogInstaller : Installer
    {
        private readonly EventLogInstaller _installer;

        public LogInstaller()
        {
            _installer = new EventLogInstaller();
            _installer.CategoryCount = 0;
            _installer.CategoryResourceFile = null;
            _installer.Log = "ВИРД";
            _installer.MessageResourceFile = null;
            _installer.ParameterResourceFile = null;
            _installer.Source = "ВИРД";

            Installers.Add(_installer);
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            if (EventLog.Exists(_installer.Log))
            {
                using (SystemEventLogging logging = new SystemEventLogging())
                    logging._eventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
            }
        }
    }
}