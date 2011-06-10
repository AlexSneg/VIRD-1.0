using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util.FileTransfer;
using Command = TechnicalServices.Common.Command;
using System.IO;

namespace DomainServices.ImportExportClientManagement.Presentation
{
    internal class ExportPresentationCommand : Command
    {
        private readonly IPresentationClient _remotePresentationClient;
        private readonly IPresentationClient _standalonePresentationClient;
        private readonly PresentationInfo _presentationInfo;
        private readonly IClientResourceCRUD<FilesGroup> _clientPresentationCRUD;
        private readonly IClientResourceCRUD<ResourceDescriptor> _standaloneClientResourceCRUD;
        private readonly string _newPresentationName = null;
        //private readonly IPresentationWorker _presentationService;

        private readonly Func<string, bool> _delegateForDeletedPresentation;

        public ExportPresentationCommand(string commandName, PresentationInfo presentationInfo,
            string newPresentationName,
            IPresentationClient remotePresentationClient, IPresentationClient standalonePresentationClient,
            Func<string, bool> delegateForDeletedPresentation)
            : base(commandName)
        {
            _remotePresentationClient = remotePresentationClient;
            _standalonePresentationClient = standalonePresentationClient;
            _standaloneClientResourceCRUD = _standalonePresentationClient.GetResourceCrud();
            _clientPresentationCRUD = _remotePresentationClient.GetPresentationExportCrud();
            _presentationInfo = presentationInfo;
            _newPresentationName = newPresentationName;
            //_presentationService = presentationService;
            _delegateForDeletedPresentation = delegateForDeletedPresentation;
            //_clientPresentationCRUD = new ClientSidePresentationTransfer(directory, presentationClient);
            //_clientSourceStandalone = new ClientSideStandAloneSourceTransfer(standaloneResourceEx);
        }

        #region Overrides of Command

        protected override bool OnExecute()
        {
            // экспортируем сам файл презентации
            FilesGroup filesGroup = _remotePresentationClient.GetPresentationForExport(_presentationInfo.UniqueName);
            if (!string.IsNullOrEmpty(_newPresentationName) && filesGroup != null)
                filesGroup.MainFile = Path.GetFileName(_newPresentationName);
            bool isSuccess = _clientPresentationCRUD.GetSource(filesGroup, false);
            if (!isSuccess)
            {
                bool toContinue = _delegateForDeletedPresentation.Invoke(
                    string.Format("Невозможно экспортировать сценарий {0}: сценарий уже удален",_presentationInfo.Name));
                if (!toContinue)
                {
                    throw new InterruptOperationException(_presentationInfo.Name);
                }
                return false;
            }

            // сохраняем ресурсы
            Dictionary<string, IList<ResourceDescriptor>> localResourceDescriptors =
                _remotePresentationClient.GetLocalSources(_presentationInfo.UniqueName);

            List<ResourceDescriptor> resources = new List<ResourceDescriptor>();
            foreach (IList<ResourceDescriptor> list in localResourceDescriptors.Values)
            {
                foreach (ResourceDescriptor descriptor in list)
                {
                    // при выгрузке грохаем ресурсы с такими же именами, если есть - это осталось какое то старье
                    List<ResourceDescriptor> oldResources =
                        _standalonePresentationClient.SourceDAL.SearchByName(descriptor);
                    if (oldResources != null)
                    {
                        foreach (ResourceDescriptor oldResource in oldResources)
                        {
                            _standalonePresentationClient.SourceDAL.DeleteSource(
                                Thread.CurrentPrincipal as UserIdentity, oldResource);
                        }
                    }
                    string otherResourceId;
                    FileSaveStatus status = _standaloneClientResourceCRUD.SaveSource(descriptor, out otherResourceId);
                    if (status != FileSaveStatus.Ok)
                        throw new ApplicationException(
                            string.Format("При сохранении сценария {0} не удалось сохранить источник {1}",
                                          _presentationInfo.Name, descriptor.ResourceInfo.Name));
                }
            }
            return true;
        }

        protected override void OnRollBack()
        {
            if (_remotePresentationClient != null)
                _clientPresentationCRUD.RollBack();
        }

        protected override void OnCommit()
        {
            if (_remotePresentationClient != null)
                _clientPresentationCRUD.Commit();
        }

        #endregion
    }
}
