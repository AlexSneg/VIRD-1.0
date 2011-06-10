using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Domain.PresentationDesign.DesignCommon;
using DomainServices.ImportExportCommon;
using TechnicalServices.Common;
using TechnicalServices.Configuration.Client;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using Command = TechnicalServices.Common.Command;

namespace DomainServices.ImportExportClientManagement.Presentation
{
    public class ExportPresentation
    {
        private readonly IClientConfiguration _config;
        //private readonly IResourceEx<ResourceDescriptor> _standaloneSourceDAL;
        //private readonly IResourceEx<DeviceResourceDescriptor> _standaloneDeviceResourceEx;
        private readonly IPresentationClient _standalonePresentationClient;
        private readonly IPresentationClient _remotePresentationClient;
        private readonly IExportPresentationController _exportPresentationController;

        public ExportPresentation(IClientConfiguration config,
            IPresentationClient remotePresentationClient,
            IPresentationClient standalonePresentationClient,
            IExportPresentationController exportPresentationController)
        {
            _config = config;
            //_standaloneSourceDAL = standaloneSourceDAL;
            //_standaloneDeviceResourceEx = standaloneDeviceResourceEx;
            _standalonePresentationClient = standalonePresentationClient;
            _remotePresentationClient = remotePresentationClient;
            _exportPresentationController = exportPresentationController;
        }

        public void Export(PresentationInfo[] presentationInfos,
            IDesignerService designerService)
        {
            if (presentationInfos == null || presentationInfos.Length == 0) return;
            try
            {
                //if (!ExportPresentationController.Instanse.ConfirmExport(presentationInfos.Select(pi => pi.Name)))
                //    return;
                string newPresentationName;
                if (!_exportPresentationController.ConfirmExport(_config.ScenarioFolder, "*.xml", presentationInfos.Select(pi => pi.Name), out newPresentationName))
                    return;

                CommandInvoker invoker = new CommandInvoker();

                //IContinue isContinue =
                //    ExportPresentationController.Instanse.GetUserInteractive(presentationInfos.Length == 1);
                IContinue isContinue =
                    _exportPresentationController.GetUserInteractive(presentationInfos.Length == 1);

                // сначала экспортируем схемы для презентации
                ExportPresentationSchemaFilesCommand exportPresentationSchemaFilesCommand =
                    new ExportPresentationSchemaFilesCommand(string.Format("экспорт файлов-схем для сценария"),
                                                             _remotePresentationClient, _config.ScenarioFolder);

                invoker.AddCommand(exportPresentationSchemaFilesCommand);

                // теперь для каждой презентации своя команда
                //List<TechnicalServices.Common.Command> presentationExportCommandList =
                //    new List<TechnicalServices.Common.Command>(presentationInfos.Length);
                foreach (PresentationInfo presentationInfo in presentationInfos)
                {
                    //presentationExportCommandList.Add(
                    Command exportPresentationCommand =
                    new ExportPresentationCommand(string.Format("Экспорт сценария {0}", presentationInfo.Name),
                                                      presentationInfo,
                                                      presentationInfos.Length == 1 ? newPresentationName : null,
                                                      _remotePresentationClient,
                                                      _standalonePresentationClient,
                                                      new Func<string, bool>(isContinue.Continue));
                    invoker.AddCommand(exportPresentationCommand);
                }

                // экспорт глобальных сорсов
                ExportGlobalSourcesCommand exportGlobalSourcesCommand = new
                    ExportGlobalSourcesCommand("Экспорт глобальных источников",
                                               _remotePresentationClient,
                                               _standalonePresentationClient);

                invoker.AddCommand(exportGlobalSourcesCommand);

                // выполняем
                bool isSuccess = CommandInvoker.Execute(invoker);

                //ExportPresentationController.Instanse.SuccessMessage("Экспорт презентаций успешно завершен");
                if (isSuccess)
                    _exportPresentationController.SuccessMessage("Экспорт сценариев успешно завершен");
            }
            catch (InterruptOperationException)
            {
                _exportPresentationController.ErrorMessage(string.Format("Экспорт сценариев был прерван пользователем"));
                //ExportPresentationController.Instanse.ErrorMessage(
                //    string.Format("Экспорт сценариев был прерван пользователем"));
            }
            catch (Exception ex)
            {
                _exportPresentationController.ErrorMessage(string.Format("При экспорте сценариев произошла неизвестная ошибка: {0}", ex));
                //ExportPresentationController.Instanse.ErrorMessage(
                //    string.Format("При экспорте сценариев произошла неизвестная ошибка: {0}", ex));
            }

        }
    }
}
