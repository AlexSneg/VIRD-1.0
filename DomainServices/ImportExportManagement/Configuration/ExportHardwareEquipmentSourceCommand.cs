using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Common;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace DomainServices.ImportExportClientManagement.Configuration
{
    internal class ExportHardwareEquipmentSourceCommand : Command
    {
        private readonly IPresentationClient _remotePresentationClient;
        private readonly IPresentationClient _standalonePresentationClient;
        private readonly IClientResourceCRUD<ResourceDescriptor> _standaloneClientResourceCRUD;

        public ExportHardwareEquipmentSourceCommand(string commandName,
            IPresentationClient remotePresentationService,
            IPresentationClient standalonePresentationClient)
            : base(commandName)
        {
            _remotePresentationClient = remotePresentationService;
            _standalonePresentationClient = standalonePresentationClient;
            _standaloneClientResourceCRUD = _standalonePresentationClient.GetResourceCrud();

        }

        #region Overrides of Command

        protected override bool OnExecute()
        {
            Dictionary<string, IList<ResourceDescriptor>> globalResourceDescriptors =
                _remotePresentationClient.GetGlobalSources();
            foreach (IList<ResourceDescriptor> list in globalResourceDescriptors.Values)
            {
                foreach (ResourceDescriptor descriptor in list)
                {
                    if (!descriptor.ResourceInfo.IsHardware) continue;
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
                    if (FileSaveStatus.Ok != _standaloneClientResourceCRUD.SaveSource(descriptor, out otherResourceId))
                        throw new ApplicationException(
                            string.Format("Не удалось сохранить глобальный источник {0}",
                                descriptor.ResourceInfo.Name));
                }
            }

            Dictionary<string, IList<DeviceResourceDescriptor>> globalDeviceResourceDescriptors =
                    _remotePresentationClient.GetGlobalDeviceSources();
            foreach (IList<DeviceResourceDescriptor> list in globalDeviceResourceDescriptors.Values)
            {
                foreach (DeviceResourceDescriptor descriptor in list)
                {
                    //string newResourceId;
                    if (FileSaveStatus.Ok != _standalonePresentationClient.SaveDeviceSource(descriptor))
                        throw new ApplicationException(
                            string.Format("Не удалось сохранить глобальный источник {0}",
                                descriptor.ResourceInfo.Name));
                }
            }

            return true;
        }

        protected override void OnRollBack()
        {
        }

        protected override void OnCommit()
        {
        }

        #endregion
    }
}
