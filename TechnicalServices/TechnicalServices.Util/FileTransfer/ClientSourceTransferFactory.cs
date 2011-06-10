using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Util.FileTransfer
{
    public static class ClientSourceTransferFactory
    {
        public static IClientResourceCRUD<ResourceDescriptor> CreateClientFileTransfer(bool isStandAlone, ISourceTransferCRUD contract, IResourceEx<ResourceDescriptor> resourceDAL)
        {
            if (isStandAlone)
                return new ClientSideStandAloneSourceTransfer(resourceDAL);
            else
            {
                return new ClientSideSourceTransfer(contract, resourceDAL);
            }
        }
    }
}