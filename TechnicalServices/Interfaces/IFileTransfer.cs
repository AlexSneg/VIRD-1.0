using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TechnicalServices.Common;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Interfaces
{
    [Serializable]
    public enum SourceStatus
    {
        New,
        Update
    }

    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IPresentationTransfer : IFileTransfer
    {
        [OperationContract]
        FilesGroup GetPresentationForExport(string uniqueName);
        [OperationContract]
        FilesGroup InitPresentationExport(UserIdentity userIdentity, FilesGroup filesGroup);

        [OperationContract]
        FilesGroup GetPresentationSchemaFilesForExport();
        [OperationContract]
        FilesGroup InitPresentationSchemaExport(UserIdentity userIdentity, FilesGroup filesGroup);
        [OperationContract]
        void DonePresentationTransfer(UserIdentity userIdentity);
    }

    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IConfigurationTransfer : IFileTransfer
    {

        [OperationContract]
        FilesGroup GetConfigFilesForExport();
        [OperationContract]
        FilesGroup InitConfigurationExport(UserIdentity userIdentity, FilesGroup filesGroup);
        [OperationContract]
        void DoneConfigurationExport(UserIdentity userIdentity);
    }

    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IFileTransfer
    {
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        FileSaveStatus Send(UserIdentity userIdentity, FileTransferObject obj);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        FileTransferObject? Receive(UserIdentity userIdentity, string resourceId);
        [OperationContract]
        void Terminate(UserIdentity userIdentity);
    }

    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface ISourceTransferCRUD : IFileTransfer
    {
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        FileSaveStatus InitSourceUpload(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor, SourceStatus status, out string otherResourceId);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        FileSaveStatus SaveSource(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor, SourceStatus status, out string otherResourceId);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        void DoneSourceTransfer(UserIdentity userIdentity);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        ResourceDescriptor InitSourceDownload(UserIdentity identity, ResourceDescriptor resourceDescriptor);
        [OperationContract]
        int ForwardMoveNeeded();
        [OperationContract]
        double GetCurrentSpeed();
        [OperationContract]
        string GetCurrentFile();
    }
}
