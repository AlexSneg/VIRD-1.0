using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Administration.AdministrationClient;
//using Domain.PresentationDesign.Client;
using Domain.PresentationDesign.Client;
using Syncfusion.Windows.Forms;
using TechnicalServices.Configuration.LoadModules;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using TechnicalServices.Logging.FileLogging;
using TechnicalServices.Configuration.Administration;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using UI.Administration.AdministrationUI;
using UI.Administration.AdministrationUI.Forms;
using UI.Common.CommonUI;
using UI.Common.CommonUI.Host;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Controllers;
using UI.PresentationDesign.DesignUI.Helpers;
using TechnicalServices.Interfaces;


namespace Hosts.Administration.AdministrationHost
{
    public class AdministrationHostImpl : ClientHost<XmlFileLogging, AdministrationConfiguration>, IDisposable
    {
        private Mutex administrationMutex;
        protected override AdministrationConfiguration CreateConfiguration(ModuleLoader loader, ModuleConfiguration config, IEventLogging logging)
        {
            if (config == null) throw new ModuleConfigurationLoadException();
            return new AdministrationConfiguration(loader, config, logging);
        }

        protected override void OnRun(AdministrationConfiguration config)
        {
            //PresentationController.Configuration = config;//TODO

            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += Application_ApplicationExit;

            DesignerClient instance = DesignerClient.Instance;
            instance.Initialize(config, false);
            AdministrationClient administrationClient = AdministrationClient.Instance;

            if (!administrationClient.Initialize(config))
            {
                MessageBoxAdv.Show("Невозможно связаться с сервером. Модуль администрирования не может работать в автономном режиме.", "Ошибка связи");
                Application.Exit();
                return;
            }

            administrationMutex = new Mutex(false, "Created::AdministrationClient");
            if (!administrationMutex.WaitOne(1, true))
            {
                //already creating
                MessageBoxAdv.Show("Модуль администрирования уже запущен на вашем компьютере", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DesignerClient.Instance.Done();
                Application.Exit();
                return;
            }

            using (LoginForm dlg = new LoginForm())
            {
                AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.UnauthenticatedPrincipal);
                if (!instance.Authenticate(dlg, UserRole.Administrator))
                {
                    DesignerClient.Instance.Done();
                    Application.Exit();
                    return;
                }

            }
            //UI.Administration.AdministrationUI.Forms.AdministrationForm
            
            AdministrationForm form = new AdministrationForm(config.EventLog);
            //form.Text = "Административная консоль";
            Application.Run(form);
        }


        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            DesignerClient.Instance.Done();
            //administrationMutex.ReleaseMutex();
        }

        #region IDisposable Members

        public void Dispose()
        {
            //administrationMutex.ReleaseMutex();
        }

        #endregion


    }
}
