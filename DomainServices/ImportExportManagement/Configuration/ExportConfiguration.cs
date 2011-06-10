using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.PresentationDesign.DesignCommon;
using TechnicalServices.Common;
using TechnicalServices.Configuration.Client;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;

namespace DomainServices.ImportExportClientManagement.Configuration
{
    public class ExportConfiguration
    {
        private readonly IClientConfiguration _config;
        private ExportConfigurationCommand _command = null;
        private readonly Action<string> _successMessageDelegate;
        private readonly Action<string> _errorMessageDelegate;
        private readonly Func<string, string, string> _getFileNameForConfigurationDelegate;
        private readonly IPresentationClient _standalonePresentationClient;
        private readonly IPresentationClient _remotePresentationClient;

        public ExportConfiguration(IClientConfiguration config,
            IPresentationClient standalonePresentationClient, IPresentationClient remotePresentationClient,
            Action<string> successMessageDelegate, Action<string> errorMessageDelegate,
            Func<string, string, string> getFileNameForConfigurationDelegate)
        {
            _config = config;
            _standalonePresentationClient = standalonePresentationClient;
            _remotePresentationClient = remotePresentationClient;
            _successMessageDelegate = successMessageDelegate;
            _errorMessageDelegate = errorMessageDelegate;
            _getFileNameForConfigurationDelegate = getFileNameForConfigurationDelegate;
        }

        public void Export(IDesignerService service)
        {
            // получаем имя файла - куда будет сохранена конфигурация
            string configFileName = GetConfigurationFile(_config);
            if (string.IsNullOrEmpty(configFileName)) return;

            //загружаем все необходимые файлы с сервера и сохраняем на клиенте
            try
            {
                LoadAndSaveConfiguration(_config.ConfigurationFolder, configFileName, service);
                //ExportConfigurationController.Instanse.SuccessMessage("Экспорт конфигурации успешно завершен");
                _successMessageDelegate.Invoke("Экспорт конфигурации успешно завершен");
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ImportExport.LoadAndSaveConfiguration: \n{0}", ex));
                //ExportConfigurationController.Instanse.ErrorMessage(string.Format("При экспорте конфигурации произошла неизвестная ошибка: {0}", ex));
                _errorMessageDelegate.Invoke(string.Format("При экспорте конфигурации произошла неизвестная ошибка: {0}", ex));
            }
        }

        private string GetConfigurationFile(IConfiguration config)
        {
            const string filter = "*.xml";
            //return ExportConfigurationController.Instanse.GetFileNameForConfiguration(
            //    config.ConfigurationFolder, filter);
            return _getFileNameForConfigurationDelegate.Invoke(
                config.ConfigurationFolder, filter);
        }

        private void LoadAndSaveConfiguration(string directory, string configName, IDesignerService service)
        {
            FilesGroup filesGroup = service.GetConfigFilesForExport();
            filesGroup.MainFile = configName;

            CommandInvoker invoker = new CommandInvoker();
            invoker.AddCommand(new ExportConfigurationCommand(string.Format("Экспорт файлов конфигурации: {0}", configName),
                                                             directory, filesGroup, service));
            invoker.AddCommand(
                new ExportHardwareEquipmentSourceCommand("Экспорт аппаратных источников",
                    _remotePresentationClient, _standalonePresentationClient));
            CommandInvoker.Execute(invoker);
        }
    }
}