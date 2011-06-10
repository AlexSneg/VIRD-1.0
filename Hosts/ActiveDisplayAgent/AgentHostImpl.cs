using System;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Windows.Forms;

using Domain.PresentationShow.ShowAgent;

using DomainServices.EquipmentManagement.AgentCommon;
using DomainServices.EquipmentManagement.AgentManagement;

using TechnicalServices.Configuration.Agent;
using TechnicalServices.Configuration.LoadModules;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Logging.FileLogging;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

using UI.Common.CommonUI.Host;

namespace Hosts.ActiveDisplayAgent.AgentHost
{
    public class AgentHostImpl : ClientHost<XmlFileLogging, AgentConfiguration>, IDisposable
    {
        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_DISPLAY_REQUIRED = 0x00000002,
            // Legacy flag, should not be used.
            // ES_USER_PRESENT   = 0x00000004,
            ES_CONTINUOUS = 0x80000000,
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        //private const string ConnectedMessage = "Соединение с сервером установлено";
        private const string StartMessage = "Запуск";
        private const int Timeout = 3000;
        private const string TryConnectMessage = "Устанавливаем соединение с сервером";

        private AgentManager manager;
        private IResourceManager resourceManager;

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        protected override bool dontThrowExceptionAtLoadModuleConfiguration { get { return true; } }

        protected override ModuleConfiguration LoadModuleConfiguration(ModuleLoader loader, IEventLogging logging)
        {
            manager = new AgentManager(null /*loader.EventLog*/);
            manager.OpenMessageView();
            manager.WriteLine(StartMessage);
            while (true)
            {
                manager.WriteLine(TryConnectMessage);
                ModuleConfiguration result = base.LoadModuleConfiguration(loader, logging);
                if (result != null) return result;
                if (manager.Wait(Timeout)) return null;
            }
        }


        protected override AgentConfiguration CreateConfiguration(ModuleLoader loader, ModuleConfiguration config, IEventLogging logging)
        {
            if (config == null) throw new ModuleConfigurationLoadException();
            return new AgentConfiguration(loader, config, logging);
        }

        protected override void OnRun(AgentConfiguration config)
        {
            if (config != null)
            {
                try
                {
                    // Запрещаем отключение монитора при бездействии системы
                    SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED);

                    resourceManager = new ResourceManager(config);
                    using (ShowAgent agent = new ShowAgent(config, manager, resourceManager))
                    using (ServiceHost _host = new ServiceHost(agent))
                    {
                        if (!manager.Check(agent.CurrentDisplay))
                            throw new WrongAgentDesktopResolution();
                        _host.Open();
                        manager.CloseMessageView();
                        manager.Wait();
                        _host.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(manager.MainWindow, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Разрешаем отключение монитора при бездействии системы
                    SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
                }
            }
            manager.Dispose();
        }
    }
}