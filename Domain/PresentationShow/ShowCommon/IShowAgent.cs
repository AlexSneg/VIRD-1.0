using System;
using System.IO;
using System.ServiceModel;

using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Domain.PresentationShow.ShowCommon
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IShowAgent : IPing, ISourceTransferCRUD
    {
        [OperationContract]
        MemoryStream GetScreenShort(Guid imageFormat);

        [OperationContract]
        void CloseWindows();

        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        void ShowWindow(Window[] windows, BackgroundImageDescriptor backgroundImageDescriptor);

        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        ResourceDescriptor[] GetResourcesForUpload(ResourceDescriptor[] resourceDescriptors,
                                                   out bool isEnoughFreeSpace);

        //ResourceForUpload[] GetResourcesForUpload(ResourceDescriptor[] resourceDescriptors,
        //    out bool isEnoughFreeSpace);
        [OperationContract]
        void DeleteAllResources();

        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        void DeleteResourcesUploaded(ResourceDescriptor[] resourceDescriptors);

        [OperationContract]
        //[ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        string DoSourceCommand(string sourceId, string command);

        [OperationContract]
        void Pause();
    }
}