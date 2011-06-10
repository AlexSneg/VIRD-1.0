using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Domain.PresentationDesign.Client;
using Domain.PresentationShow.ShowClient;

using Microsoft.Win32;

using Syncfusion.Windows.Forms;

using TechnicalServices.Configuration.Client;
using TechnicalServices.Configuration.LoadModules;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Licensing;
using TechnicalServices.Logging.FileLogging;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

using UI.Common.CommonUI;
using UI.Common.CommonUI.Host;
using UI.PresentationDesign.DesignUI;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Forms;
using TechnicalServices.Common.Classes;
using Timer = System.Threading.Timer;

namespace Hosts.PresentationDesigner.DesignerHost
{
    public class DesignerHostImpl : ClientHost<XmlFileLogging, ClientConfiguration>, IDisposable
    {
        private readonly string[] _args;
        private const int _interval = 10; //в секундах (10 сек)
        private Timer _haspCheckTimer = null;

        internal DesignerHostImpl(string[] args)
        {
            _args = args;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_haspCheckTimer != null)
            {
                _haspCheckTimer.Dispose();
                _haspCheckTimer = null;
            }
        }

        #endregion

        protected override bool dontThrowExceptionAtLoadModuleConfiguration { get { return true; } }

        protected override ClientConfiguration CreateConfiguration(ModuleLoader loader, ModuleConfiguration config, IEventLogging logging)
        {
            ClientConfiguration configuration = config != null
                                                    ?
                // Режим работы с сервером
                                                new ClientConfiguration(loader, config, logging)
                                                    :
                // Автономный режим
                                                new ClientConfiguration(loader, logging, true);

            return configuration;
        }

        private static void WriteCurrentPathInRegistry(IEventLogging log)
        {
            try
            {
                using (
                    RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\PolyMedia\PresentationDesigner",
                                                                         RegistryKeyPermissionCheck.ReadWriteSubTree))
                    if (key != null) key.SetValue("ExePath", Application.ExecutablePath);
            }
            catch (Exception ex)
            {
                log.WriteError(ex.ToString());
            }
        }


        protected override void OnRun(ClientConfiguration configuration)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU", false);
            WriteCurrentPathInRegistry(configuration.EventLog);

            try
            {
                // инициализация модуля для дизайнера, 
                // в данное время нужна для установки защиты 
                // на свойство IModule.DesignerModule
                foreach (IModule module in configuration.ModuleList)
                    module.DesignerModule.Init();
            }
            catch (LicenseInvalidException ex)
            {
                configuration.EventLog.WriteError(ex.ToString());
                throw new ApplicationException("Нарушение лицензии");
            }
            catch (HaspException ex)
            {
                configuration.EventLog.WriteError(ex.ToString());
                throw new ApplicationException("Нарушение лицензии");
            }

            // периодическая проверка ключа в автономном режиме
            if (configuration.IsStandalone)
            {
#if SECURITY
                _haspCheckTimer = new Timer(state =>
                {
                    try
                    {
                        LicenseChecker checker = new LicenseChecker();
                        checker.CheckHasp();
                    }
                    catch (LicenseInvalidException ex)
                    {
                        _haspCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        throw new ApplicationException("Нарушение лицензии");
                    }
                    catch (HaspException ex)
                    {
                        _haspCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        throw new ApplicationException("Нарушение лицензии");
                    }
                }, null, TimeSpan.FromSeconds(_interval), TimeSpan.FromSeconds(_interval));
#endif
            }

            //инициализация контроллера источников
            PresentationController.Configuration = configuration;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ApplicationExit += Application_ApplicationExit;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            DesignerClient instance = DesignerClient.Instance;
            instance.Initialize(configuration, configuration.IsStandalone);
            ShowClient.Instance.InitializeFromDisigner(configuration);
            instance.PresentationNotifier.OnStateChanged += PresentationNotifier_OnStateChanged;

            if (_args.Length == 0)
            {
                try
                {
                    if (configuration.IsStandalone)
                        MessageBoxAdv.Show("Ядро системы недоступно. Система будет работать в автономном режиме.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    using (LoginForm dlg = new LoginForm())
                    {
                        if (!instance.Authenticate(dlg, UserRole.Operator))
                        {
                            Application.Exit();
                            return;
                        }
                    }
                }
                catch (CommunicationException ex)
                {
                    configuration.EventLog.WriteError(ex.ToString());
                    MessageBoxAdv.Show("Связь с сервером потеряна.\r\nПриложение будет закрыто.", "Внимание",
                                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                    return;
                }
                ShowClient.Instance.SubscribeForNotification();

                PresentationListForm form = new PresentationListForm();
                form.Controller.LoadPresentationList();
                Application.Run(form);
            }
            else
            {
                if (_args.Length != 3)
                {
                    MessageBoxAdv.Show("Неверное число аргументов командной строки!", "Модуль подготовки сценариев");
                    Application.Exit();
                    return;
                }
                DataContractSerializer ser = new DataContractSerializer(typeof(UserIdentity));
                UserIdentity id =
                    (UserIdentity)
                    ser.ReadObject(new MemoryStream(Encoding.Default.GetBytes(_args[2].Replace('\'', '\"'))));
                //AppDomain.CurrentDomain.SetThreadPrincipal(id);
                if (!instance.Authenticate(id))
                {
                    Application.Exit();
                    return;
                }

                PresentationInfo info = DesignerClient.Instance.PresentationWorker.GetPresentationInfo(_args[0]);
                if (info == null)
                {
                    MessageBoxAdv.Show("Невозможно найти указанную презентацию!", "Модуль подготовки сценариев");
                    Application.Exit();
                    return;
                }
                PresentationDesignerForm form = new PresentationDesignerForm(info) { StartedFromPlayer = true };
                form.Init(true);
                form.NavigateToSlide(Convert.ToInt32(_args[1]));
                Application.Run(form);
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBoxAdv.Show(
                "Необрабатываемое программное исключение. Работа приложения будет прекращена\r\n" + e.Exception,
                "Ошибка", MessageBoxButtons.OK);
            Process.GetCurrentProcess().Kill();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBoxAdv.Show(
                "Необрабатываемое программное исключение. Работа приложения будет прекращена\r\n" + e.ExceptionObject,
                "Ошибка", MessageBoxButtons.OK);
            Process.GetCurrentProcess().Kill();
        }

        private static void PresentationNotifier_OnStateChanged(object sender, NotifierEventArg<CommunicationState> e)
        {
            //обрыв связи
            if (e.Data == CommunicationState.Faulted)
            {
                foreach (Form f in Application.OpenForms)
                    f.Invoke(new MethodInvoker(delegate { f.Enabled = false; }));

                MessageBox.Show("Связь с сервером потеряна.\r\nПриложение будет закрыто.",
                                "Модуль подготовки презентации", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                Process.GetCurrentProcess().Kill();
            }
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            DesignerClient.Instance.Done();
            ShowClient.Instance.Done();
        }
    }
}