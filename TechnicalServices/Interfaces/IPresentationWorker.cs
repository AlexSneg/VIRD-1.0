using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Interfaces
{
    public interface IPresentationWorker : IPresentationWorkerCommon, IPing, ISourceTransferCRUD, IPresentationTransfer
    {
        void SubscribeForMonitor(PresentationKey presentationKey, UserIdentity identity);
        void UnSubscribeForMonitor(PresentationKey presentationKey, UserIdentity identity);
        void SubscribeForGlobalMonitoring(UserIdentity identity);
        void UnSubscribeForGlobalMonitoring(UserIdentity identity);
    }


}