using System;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;

using Domain.Administration.AdministrationService;
using Domain.PresentationDesign.DesignService;
using Domain.PresentationShow.ShowService;

using DomainServices.PresentationManagement;

using TechnicalServices.Common.Utils;
using TechnicalServices.Configuration.LoadModules;
using TechnicalServices.Configuration.Server;
using TechnicalServices.Interfaces;
using TechnicalServices.Licensing;
using TechnicalServices.Locking.Locking;
using TechnicalServices.Logging.SystemEventLogging;
using TechnicalServices.Security.Security;

namespace Hosts.VisualizationCore.VisualizationCoreHost
{
    public partial class VisualizationCoreService : ServiceBase
    {
        private readonly AutoResetEvent _exit = new AutoResetEvent(false);
        private ServiceHost _administrationHost;
        private AdministrationService _administrationService;
        private ServerConfiguration _config;
        private ServiceHost _designerHost;
        private DesignerService _designService;
        private ModuleLoader _loader;

        private LockingService _lockService;
        private EventLogging _logging;
        private LoginService _loginService;
        private ServiceHost _showHost;

        private ShowService _showService;

        private Thread _watchdog;
        private PresentationWorker _worker;

        public VisualizationCoreService()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += DomainUnhandledException;
        }

        private void DomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (_logging != null) _logging.WriteError(e.ExceptionObject.ToString());
        }

        private static void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckHasp();
        }

        protected override void OnStart(string[] args)
        {
            _logging = new SystemEventLogging();
            _logging.WriteInformation("Система запущена");
            try
            {
                CheckLicense();

                _watchdog = new Thread(watchdogProc);
                _watchdog.Start();

                //System.Diagnostics.Debugger.Break();
                if (ServiceHandle != IntPtr.Zero) RequestAdditionalTime(1000 * 60 * 15);

                _loader = new ModuleLoader(_logging);
                KnownTypeProvider.ModuleList = _loader.ModuleList.ToArray();
                _config = new ServerConfiguration(_loader, _logging);
                _loginService = new LoginService(_config);
                _lockService = new LockingService(_loginService.FindSystemIdentity(), _config.EventLog);
                _worker = new PresentationWorker(_config, _lockService);

                _administrationService = new AdministrationService(_config, _loginService, _worker);

                _designService = new DesignerService(_config, _worker, _loginService, _administrationService);
                _showService = new ShowService(_config, _worker, _loginService.FindSystemIdentity());


                _designerHost = new ServiceHost(_designService);
                _showHost = new ServiceHost(_showService);
                _administrationHost = new ServiceHost(_administrationService);

                _designerHost.Open();
                _showHost.Open();
                _administrationHost.Open();
            }
            catch (Exception ex)
            {
                _logging.WriteError(ex.ToString());
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (_showHost != null)
                    if (_showHost.State != CommunicationState.Closed)
                        _showHost.Close();
                if (_designerHost != null)
                    if (_designerHost.State != CommunicationState.Closed)
                        _designerHost.Close();
                if (_administrationHost != null)
                    if (_administrationHost.State != CommunicationState.Closed)
                        _administrationHost.Close();

                if (_designService != null)
                    _designService.Dispose();
                if (_showService != null)
                    _showService.Dispose();
                if (_administrationService != null)
                    _administrationService.Dispose();

                _exit.Set();
                _loader.Dispose();
                _logging.WriteInformation("Система остановлена");
                _logging.Dispose();
            }
            catch (Exception ex)
            {
                _logging.WriteError(ex.ToString());
            }
        }

        private void watchdogProc(object data)
        {
            WatchDog.WatchDogHandler(this, _logging.WriteError, _exit);
        }
    }
}