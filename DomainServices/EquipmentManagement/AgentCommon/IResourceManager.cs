using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace DomainServices.EquipmentManagement.AgentCommon
{
    public interface IResourceManager : ISourceDAL, ISourceTransferCRUD
    {
        ResourceDescriptor[] GetResourcesForUpload(ResourceDescriptor[] resourceDescriptors,
            out bool isEnoughFreeSpace);
        void CorrectResourceFileName(ResourceDescriptor descriptor);
        void DeleteAllSource();
        void DeleteResourcesUploaded(ResourceDescriptor[] resourceDescriptors);
    }
}
