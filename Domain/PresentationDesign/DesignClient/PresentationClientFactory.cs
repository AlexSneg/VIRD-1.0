using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Configuration.Client;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Communication.Communication.Client;
using System.ServiceModel;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Domain.PresentationDesign.Client
{
    public static class PresentationClientFactory
    {
        //public static IPresentationWorker CreatePresentationWorker(bool isStandAlone, IPresentationNotifier callback)
        //{
        //    if (isStandAlone)
        //    {
        //        return new StandAlonePresentationWorker();
        //    }
        //    else
        //    {
        //        return (new DuplexClient<IPresentationWorker>(new InstanceContext(callback), 500)).Channel;
        //    }
        //}

        public static IPresentationClient CreateStandAlonePresentationWorker(ISourceDAL sourceDAL,
            IDeviceSourceDAL deviceSourceDAL, IClientConfiguration configuration,
            IPresentationNotifier notifier)
        {
            return new StandAlonePresentationWorker(sourceDAL, deviceSourceDAL, configuration, null /*notifier*/); // кажется в автономном режиме нотификация не нужна.
        }

        public static RemotePresentationClient CreatePresentationClient(ISourceDAL sourceDAL,
            IDeviceSourceDAL deviceSourceDAL, IPresentationNotifier callback, IClientConfiguration configuration)
        {
            return new RemotePresentationClient(callback, configuration, sourceDAL, deviceSourceDAL);
        }
    }
   
}
