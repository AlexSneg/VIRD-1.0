using System;
using System.Windows.Forms;

using Domain.PresentationDesign.Client;
using Domain.PresentationShow.ShowClient;

using TechnicalServices.Configuration.LoadModules;
using TechnicalServices.Configuration.Player;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using TechnicalServices.Logging.FileLogging;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

using UI.Common.CommonUI;
using UI.Common.CommonUI.Host;

using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Forms;
using UI.PresentationDesign.DesignUI.Controllers;
using Syncfusion.Windows.Forms;
using System.Diagnostics;
using TechnicalServices.Common.Classes;
using System.Threading;
using TechnicalServices.Interfaces;

namespace Hosts.Player.PlayerHost
{
    public class PlayerHostImpl : ClientHost<XmlFileLogging, PlayerConfiguration>, IDisposable
    {
        private static bool _isPlayerOpened = false;

        protected override PlayerConfiguration CreateConfiguration(ModuleLoader loader, ModuleConfiguration config, IEventLogging logging)
        {
            if (config == null) throw new ModuleConfigurationLoadException();
            return new PlayerConfiguration(loader, config, logging);
        }

        protected override void OnRun(PlayerConfiguration config)
        {
            //инициализация контроллера источников
            PresentationController.Configuration = config;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ApplicationExit += Application_ApplicationExit;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            DesignerClient instance = DesignerClient.Instance;
            instance.Initialize(config, false); // TODO: Как определять standalone для плеера??

            ShowClient showClient = ShowClient.Instance;
            if (!showClient.InitializeFromPlayer(config))
            {
                MessageBoxAdv.Show("Невозможно связаться с сервером. Модуль управления сценариями не может работать в автономном режиме.", "Ошибка связи");
                Application.Exit();
                return;
            }

            DesignerClient.Instance.PresentationNotifier.OnStateChanged += new EventHandler<NotifierEventArg<System.ServiceModel.CommunicationState>>(PresentationNotifier_OnStateChanged);

            bool canStart = false;
            int tries = 5;

            do
            {
                _isPlayerOpened = ShowClient.Instance.StartPlayer();
                if (!_isPlayerOpened)
                    Thread.Sleep(100);
                tries--;
            }
            while (!_isPlayerOpened && tries > 0);

            if (!_isPlayerOpened)
            {
                MessageBoxAdv.Show("Модуль управления сценариями уже запущен в системе. Нельзя запустить несколько копий одновременно.", "Ошибка");
                Application.Exit();
                return;
            }

            using (LoginForm dlg = new LoginForm())
            {
                if (!instance.Authenticate(dlg, UserRole.Operator))
                {
                    Application.Exit();
                    return;
                }
            }
            showClient.SubscribeForNotification();

            PresentationListForm form = new PresentationListForm(false);
            form.Text = "ВИРД - Показ сценариев";
            form.Controller.LoadPresentationList();
            Application.Run(form);
        }

        void PresentationNotifier_OnStateChanged(object sender, NotifierEventArg<System.ServiceModel.CommunicationState> e)
        {
            if (e.Data == System.ServiceModel.CommunicationState.Faulted)
            {
                int n = Application.OpenForms.Count - 1;
                if (n < 0)
                {
                    Syncfusion.Windows.Forms.MessageBoxAdv.Show("Связь с сервером потеряна.\r\nПриложение будет закрыто.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    Application.OpenForms[n].Invoke(new MethodInvoker(() =>
                    {
                        Syncfusion.Windows.Forms.MessageBoxAdv.Show(Application.OpenForms[n], "Связь с сервером потеряна.\r\nПриложение будет закрыто.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);                        
                    }));
                Process.GetCurrentProcess().Kill(); 
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

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (_isPlayerOpened)
                ShowClient.Instance.StopPlayer();
            DesignerClient.Instance.Done();
            ShowClient.Instance.Done();
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}