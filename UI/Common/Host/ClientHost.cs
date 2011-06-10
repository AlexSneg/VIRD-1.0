using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;

using Domain.PresentationDesign.DesignCommon;

using TechnicalServices.Communication.Communication.Client;
using TechnicalServices.Configuration.Common;
using TechnicalServices.Configuration.LoadModules;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

using UI.Common.CommonUI.Forms;

namespace UI.Common.CommonUI.Host
{
    public abstract class ClientHost<TLog, TConf>
        where TLog : IEventLogging, IDisposable, new()
        where TConf : IConfiguration
    {
        public void Run()
        {
            LoadingSequence((config) => { OnRun(config); });
        }

        protected void LoadingSequence(Action<TConf> act)
        {
            using (TLog logging = new TLog())
            using (ModuleLoader loader = new ModuleLoader(logging))
            {
                try
                {
                    if (loader.ModuleList.Count == 0) throw new InvalidModuleListException();
                    KnownTypeProvider.ModuleList = loader.ModuleList.ToArray();
                    ModuleConfiguration configuration = LoadModuleConfiguration(loader, logging);
                    TConf config = CreateConfiguration(loader, configuration, logging);
                    RefreshParameters(config);
                    act(config);
                }
                catch (NoConnectionException ex)
                {
                    logging.WriteError(ex.ToString());
                    using (MessageBoxForm dlg = new MessageBoxForm())
                    {
                        dlg.TopMost = true;
                        dlg.ShowForm(null, "Невозможно связаться с сервером. Модуль не может работать в автономном режиме.", "Ошибка связи",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, new[] { "OK" });
                        Application.Exit();
                    }
                }
                catch (Exception ex)
                {
                    logging.WriteError(ex.ToString());
                    using (MessageBoxForm dlg = new MessageBoxForm())
                    {
                        dlg.ShowForm(null, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error,
                                     new[] { "OK" });
                    }
                }
            }
        }

        protected virtual void RefreshParameters(TConf conf)
        {
            RefreshSystemParameters(conf);
        }

        protected abstract TConf CreateConfiguration(ModuleLoader loader, ModuleConfiguration config, IEventLogging logging);
        protected abstract void OnRun(TConf config);
        protected virtual bool dontThrowExceptionAtLoadModuleConfiguration { get { return false; } }
        protected virtual ModuleConfiguration LoadModuleConfiguration(ModuleLoader loader, IEventLogging logging)
        {
            try
            {
                using (SimpleClient<IDesignerService> service = new SimpleClient<IDesignerService>())
                {
                    service.Open();
                    ICollection<string> list = service.Channel.CheckModuleConfiguration();
                    var v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    if (!service.Channel.CheckVersion(v.ToString()))
                    {
                        throw new WrongModuleVersion();
                    }
                    CheckModuleInformation(loader, list);
                    return service.Channel.GetModuleConfiguration();
                }
            }

            catch (CommunicationException ex)
            {
                if (dontThrowExceptionAtLoadModuleConfiguration)
                {
                    logging.WriteError(ex.ToString());
                    return null;
                }
                else
                    throw new NoConnectionException();
            }
        }

        /// <summary>
        /// Проверка вынесена в отдельный метод, так возможно, что она будет еще меняться
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="serverList"></param>
        private static void CheckModuleInformation(ModuleLoader loader, ICollection<string> serverList)
        {
            //if (!loader.GetVersionList().SequenceEqual(serverList)) throw new WrongModuleVersion();
            // Оставляем только те сборки, которые загружены на сервере
            List<IModule> moduleList = new List<IModule>(loader.ModuleList.Count);
            foreach (IModule module in loader.ModuleList)
            {
                string versionInfo = module.GetType().Assembly.FullName;
                if (serverList.Contains(versionInfo)) moduleList.Add(module);
            }
            loader.ModuleList.Clear();
            loader.ModuleList.AddRange(moduleList);
            if (loader.ModuleList.Count != serverList.Count) throw new WrongModuleVersion();
        }

        private void RefreshSystemParameters(TConf config)
        {
            try
            {
                ISystemParameters systemParameters = LoadSystemParameters(config);
                if (systemParameters != null)
                    config.SaveSystemParameters(systemParameters);
            }
            catch (SystemParametersSaveException saveException)
            {
                config.EventLog.WriteError(
                    "ClientHost.RefreshSystemParameters :: Неудалось загрузить системные параметры. " +
                    saveException.Message);
            }
        }

        protected ISystemParameters LoadSystemParameters(TConf config)
        {
            try
            {
                using (SimpleClient<IDesignerService> chanel = new SimpleClient<IDesignerService>())
                {
                    chanel.Open();
                    return chanel.Channel.GetSystemParameters();
                }
            }
            catch (CommunicationException)
            {
                return null;
            }
        }
    }
}