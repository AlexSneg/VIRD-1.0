using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TechnicalServices.Common.Classes;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Interfaces
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IPresentationNotifier))]
    public interface IPresentationService : IPing, ISourceTransferCRUD, IPresentationTransfer
    {
        [OperationContract]
        [ServiceKnownType(typeof(PresentationKey))]
        [ServiceKnownType(typeof(SlideKey))]
        IList<PresentationInfoExt> GetPresentationInfoList();
        [OperationContract]
        [ServiceKnownType(typeof(PresentationKey))]
        [ServiceKnownType(typeof(SlideKey))]
        PresentationInfoExt GetPresentationInfo(string uniqueName);
        [OperationContract]
        [ServiceKnownType(typeof(PresentationKey))]
        [ServiceKnownType(typeof(SlideKey))]
        PresentationInfoExt GetPresentationInfoByName(string presentationName);
        [OperationContract]
        byte[] GetPresentation(string uniqueName);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        Slide[] LoadSlides(string presentationUniqueName, int[] slideIdArr);
        [OperationContract]
        CreatePresentationResult CreatePresentation(UserIdentity sender, PresentationInfo presentationInfo,
            out int[] labelNotExists);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        SavePresentationResult SavePresentationChanges(UserIdentity userIdentity, PresentationInfo presentationInfo,
            Slide[] newSlideArr, out ResourceDescriptor[] resourcesNotExists,
            out DeviceResourceDescriptor[] deviceResourcesNotExists, out int[] labelNotExists,
            out UserIdentity[] whoLock, out int[] slidesAlreadyExistsId);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        bool SaveSlideChanges(UserIdentity userIdentity, string presentationUniqueName,
            Slide[] slideToSave, out int[] slideIdNotLocked,
            out ResourceDescriptor[] resourcesNotExists,
            out DeviceResourceDescriptor[] deviceResourcesNotExists,
            out int[] labelNotExists);
        [OperationContract]
        bool DeletePresentation(UserIdentity sender, string uniqueName);
        [OperationContract]
        PresentationStatus GetPresentationStatus(string uniqueName, out UserIdentity userIdentity);
        [OperationContract]
        bool AcquireLockForPresentation(UserIdentity userIdentity, string uniqueName, RequireLock requireLock);
        [OperationContract]
        bool ReleaseLockForPresentation(UserIdentity userIdentity, string uniqueName);
        [OperationContract]
        bool AcquireLockForSlide(UserIdentity userIdentity, string presentationUniqueName, int slideId, RequireLock requireLock);
        [OperationContract]
        bool ReleaseLockForSlide(UserIdentity userIdentity, string presentationUniqueName, int slideId);
        [OperationContract]
        [ServiceKnownType(typeof(PresentationKey))]
        [ServiceKnownType(typeof(SlideKey))]
        LockingInfo[] GetLockingInfo(ObjectKey[] objectKeyArr);
        [OperationContract(Name = "GetLockingInfoByPresentationName")]
        LockingInfo[] GetLockingInfo(string uniqueName);
        /// <summary>
        /// удаление сорса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="descriptor">описатель сорса</param>
        /// <returns></returns>
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        RemoveResult DeleteSource(UserIdentity sender, ResourceDescriptor descriptor);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        RemoveResult DeleteDeviceSource(UserIdentity sender, DeviceResourceDescriptor descriptor);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        FileSaveStatus SaveDeviceSource(UserIdentity userIdentity, DeviceResourceDescriptor resourceDescriptor, SourceStatus status);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        FileSaveStatus GetSourceStatus(ResourceDescriptor descriptor);
        [OperationContract(Name = "IsResourceAvailableForResourceDescriptor")]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        bool IsResourceAvailable(ResourceDescriptor descriptor);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        FileSaveStatus GetDeviceSourceStatus(DeviceResourceDescriptor descriptor);
        [OperationContract(Name = "IsResourceAvailableForDeviceResourceDescriptor")]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        bool IsResourceAvailable(DeviceResourceDescriptor descriptor);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        ResourceDescriptor CopySourceFromGlobalToLocal(UserIdentity sender,
                                                        ResourceDescriptor resourceDescriptor,
                                                       string presentationUniqueName);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        ResourceDescriptor CopySourceFromLocalToGlobal(UserIdentity sender,
            ResourceDescriptor resourceDescriptor);
        [OperationContract]
        void CopySourceFromLocalToLocal(UserIdentity userIdentity, string fromUniqueName, string toUniqueName);
        /// <summary>
        /// в качестве ключа - тип плагина
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        Dictionary<string, IList<ResourceDescriptor>> GetGlobalSources();
        /// <summary>
        /// в качестве ключа - тип плагина
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        Dictionary<string, IList<ResourceDescriptor>> GetLocalSources(string presentationUniqueName);
        //ResourceDescriptor[] GetLocalSources(PresentationInfo info);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        Dictionary<string, IList<DeviceResourceDescriptor>> GetGlobalDeviceSources();
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        PresentationInfo[] GetPresentationWhichContainsSource(ResourceDescriptor resourceDescriptor);
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        PresentationInfo[] GetPresentationWhichContainsDeviceSource(DeviceResourceDescriptor resourceDescriptor);
        [OperationContract]
        void SubscribeForMonitor(PresentationKey presentationKey, UserIdentity identity);
        [OperationContract]
        void UnSubscribeForMonitor(PresentationKey presentationKey, UserIdentity identity);
        [OperationContract]
        void SubscribeForGlobalMonitoring(UserIdentity identity);
        [OperationContract]
        void UnSubscribeForGlobalMonitoring(UserIdentity identity);
    }

    public interface IPresentationNotifier
    {
        [OperationContract(IsOneWay = true)]
        [ServiceKnownType(typeof(PresentationKey))]
        [ServiceKnownType(typeof(SlideKey))]
        void ObjectLocked(LockingInfo lockingInfo);
        [OperationContract(IsOneWay = true)]
        [ServiceKnownType(typeof(PresentationKey))]
        [ServiceKnownType(typeof(SlideKey))]
        void ObjectUnLocked(LockingInfo lockingInfo);
        [OperationContract(IsOneWay = true)]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        void ResourceAdded(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor);
        [OperationContract(IsOneWay = true)]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        void ResourceDeleted(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor);
        [OperationContract(IsOneWay = true)]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        void ResourceUpdated(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor);
        [OperationContract(IsOneWay = true)]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        void DeviceResourceAdded(UserIdentity userIdentity, DeviceResourceDescriptor resourceDescriptor);
        [OperationContract(IsOneWay = true)]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        void DeviceResourceDeleted(UserIdentity userIdentity, DeviceResourceDescriptor resourceDescriptor);
        [OperationContract(IsOneWay = true)]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        void DeviceResourceUpdated(UserIdentity userIdentity, DeviceResourceDescriptor resourceDescriptor);
        [OperationContract(IsOneWay = true)]
        void PresentationAdded(UserIdentity userIdentity, PresentationInfo presentationInfo);
        [OperationContract(IsOneWay = true)]
        void PresentationDeleted(UserIdentity userIdentity, PresentationInfo presentationInfo);
        [OperationContract(IsOneWay = true)]
        [ServiceKnownType(typeof(PresentationKey))]
        [ServiceKnownType(typeof(SlideKey))]
        void ObjectChanged(UserIdentity userIdentity, IList<ObjectInfo> objectInfoList);
        // label
        [OperationContract(IsOneWay = true)]
        void LabelAdded(Label label);
        [OperationContract(IsOneWay = true)]
        void LabelDeleted(Label label);
        [OperationContract(IsOneWay = true)]
        void LabelUpdated(Label label);

        event EventHandler<NotifierEventArg<TechnicalServices.Entity.LockingInfo>> OnObjectLocked;
        event EventHandler<NotifierEventArg<TechnicalServices.Entity.LockingInfo>> OnObjectUnLocked;
        event EventHandler<NotifierEventArg<ResourceDescriptor>> OnResourceAdded;
        event EventHandler<NotifierEventArg<ResourceDescriptor>> OnResourceDeleted;
        event EventHandler<NotifierEventArg<ResourceDescriptor>> OnResourceUpdated;
        event EventHandler<NotifierEventArg<DeviceResourceDescriptor>> OnDeviceResourceAdded;
        event EventHandler<NotifierEventArg<DeviceResourceDescriptor>> OnDeviceResourceDeleted;
        event EventHandler<NotifierEventArg<DeviceResourceDescriptor>> OnDeviceResourceUpdated;
        event EventHandler<NotifierEventArg<PresentationInfo>> OnPresentationAdded;
        event EventHandler<NotifierEventArg<PresentationInfo>> OnPresentationDeleted;
        event EventHandler<NotifierEventArg<IList<ObjectInfo>>> OnObjectChanged;
        event EventHandler<NotifierEventArg<CommunicationState>> OnStateChanged;
        event EventHandler<NotifierEventArg<Label>> OnLabelAdded;
        event EventHandler<NotifierEventArg<Label>> OnLabelDeleted;
        event EventHandler<NotifierEventArg<Label>> OnLabelUpdated;

    }

}
