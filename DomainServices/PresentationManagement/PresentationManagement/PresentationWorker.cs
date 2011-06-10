using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TechnicalServices.Common;
using TechnicalServices.Common.Locking;
using TechnicalServices.Common.Notification;
using TechnicalServices.Configuration.Server;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPresentation;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util;

namespace DomainServices.PresentationManagement
{
    #region старый код

    // пока заремарено чтобы можно было исправлять ошибки глядя в старый код
    //public class PresentationWorker : IPresentationWorker
    //{
    //    private readonly IConfiguration _configuration;
    //    private readonly ILockService _lockService;
    //    private readonly IPresentationDAL _presentationDAL;
    //    private readonly ISourceDAL _sourceDAL;
    //    private readonly IResourceRemoteCRUD _resourceRemoteCRUD;

    //    private readonly NotificationManager<PresentationKey>.NotificationStore<UserIdentity, IPresentationNotifier> _presentationNotifier;
    //    private readonly NotificationManager<IPresentationWorker>.NotificationStore<UserIdentity, IPresentationNotifier> _globalNotifier;

    //    public PresentationWorker(IConfiguration configuration, ILockService lockService)
    //    {
    //        _configuration = configuration;
    //        _lockService = lockService;// new LockingService();
    //        _presentationDAL = new PresentationDALCaching(_configuration);
    //        _sourceDAL = new SourceDAL(_configuration);

    //        _presentationDAL.Init(_sourceDAL);

    //        _resourceRemoteCRUD = new ServerSideFileTransfer(_sourceDAL);
    //        _presentationNotifier =
    //            NotificationManager<PresentationKey>.Instance.RegisterDuplexService
    //            <UserIdentity, IPresentationNotifier>
    //            (NotifierBehaviour.OneInstancePerKey);
    //        _globalNotifier = NotificationManager<IPresentationWorker>.Instance.RegisterDuplexService
    //            <UserIdentity, IPresentationNotifier>
    //            (NotifierBehaviour.OneInstance);
    //        _lockService.AddItem += new StorageAction<ObjectKey, LockingInfo>(_lockService_AddItem);
    //        _lockService.RemoveItem += new StorageAction<ObjectKey, LockingInfo>(_lockService_RemoveItem);

    //        _sourceDAL.OnResourceAdded += new EventHandler<SourceEventArg>(_sourceDAL_OnResourceAdded);
    //        _sourceDAL.OnResourceDeleted += new EventHandler<SourceEventArg>(_sourceDAL_OnResourceDeleted);

    //        _presentationDAL.OnPresentationAdded += new EventHandler<PresentationEventArg>(_presentationDAL_OnPresentationAdded);
    //        _presentationDAL.OnPresentationDeleted += new EventHandler<PresentationEventArg>(_presentationDAL_OnPresentationDeleted);
    //        //_lockService.PresentationDAL = _presentationDAL;
    //        //_presentationDAL.LockService = _lockService;
    //    }

    //    void _presentationDAL_OnPresentationDeleted(object sender, PresentationEventArg e)
    //    {
    //        _globalNotifier.Notify(this, "PresentationDeleted", e.PresentationInfo);
    //    }

    //    void _presentationDAL_OnPresentationAdded(object sender, PresentationEventArg e)
    //    {
    //        _globalNotifier.Notify(this, "PresentationAdded", e.PresentationInfo);
    //    }

    //    void _sourceDAL_OnResourceDeleted(object sender, SourceEventArg e)
    //    {
    //        ResourceDescriptor descriptor = e.ResourceDescriptor;
    //        if (descriptor.IsLocal && !String.IsNullOrEmpty(descriptor.PresentationUniqueName))
    //        {
    //            _presentationNotifier.Notify(ObjectKeyCreator.CreatePresentationKey(descriptor.PresentationUniqueName),
    //                "ResourceDeleted", descriptor);
    //        }
    //        else
    //        {
    //            _globalNotifier.Notify(this, "ResourceDeleted",
    //                descriptor);
    //        }
    //    }

    //    void _sourceDAL_OnResourceAdded(object sender, SourceEventArg e)
    //    {
    //        ResourceDescriptor descriptor = e.ResourceDescriptor;
    //        if (descriptor.IsLocal && !string.IsNullOrEmpty(descriptor.PresentationUniqueName))
    //        {
    //            _presentationNotifier.Notify(ObjectKeyCreator.CreatePresentationKey(descriptor.PresentationUniqueName),
    //                "ResourceAdded", descriptor);
    //        }
    //        else
    //        {
    //            _globalNotifier.Notify(this, "ResourceAdded",
    //                descriptor);
    //        }
    //    }

    //    void _lockService_RemoveItem(ObjectKey key, LockingInfo value)
    //    {
    //        PresentationKey presentationKey = GetPresentationKey(key);
    //        if (presentationKey == null) return;
    //        //_presentationNotifier.Unsubscribe(presentationKey, value.UserIdentity);
    //        if (key.GetObjectType() == ObjectType.Presentation)
    //            _globalNotifier.Notify(this, "ObjectUnLocked", value);
    //        else
    //            _presentationNotifier.Notify(presentationKey, "ObjectUnLocked", value);
    //    }

    //    void _lockService_AddItem(ObjectKey key, LockingInfo value)
    //    {
    //        PresentationKey presentationKey = GetPresentationKey(key);
    //        if (presentationKey == null) return;
    //        if (key.GetObjectType() == ObjectType.Presentation)
    //            _globalNotifier.Notify(this, "ObjectLocked" ,value);
    //        else
    //            _presentationNotifier.Notify(presentationKey, "ObjectLocked", value);
    //        //_presentationNotifier.SubscribeForMonitor(presentationKey, value.UserIdentity);
    //    }

    //    private PresentationKey GetPresentationKey(ObjectKey key)
    //    {
    //        PresentationKey presentationKey = null;
    //        if (key is PresentationKey)
    //            presentationKey = key as PresentationKey;
    //        else if (key is SlideKey)
    //            presentationKey = (PresentationKey)((SlideKey)key).PresentationKey;
    //        return presentationKey;
    //    }

    //    public IList<PresentationInfoExt> GetPresentationInfoList()
    //    {
    //        List<PresentationInfoExt> extArr = new List<PresentationInfoExt>();
    //        foreach (PresentationInfo info in _presentationDAL.GetPresentationInfoList())
    //        {
    //            extArr.Add(new PresentationInfoExt(info,
    //                                               _lockService.GetLockInfo(
    //                                                   ObjectKeyCreator.CreatePresentationKey(info.UniqueName))));
    //        }
    //        return extArr;
    //    }

    //    public PresentationInfoExt GetPresentationInfo(string uniqueName)
    //    {
    //        return new PresentationInfoExt(_presentationDAL.GetPresentationInfo(uniqueName),
    //                                               _lockService.GetLockInfo(
    //                                                   ObjectKeyCreator.CreatePresentationKey(uniqueName)));
    //    }

    //    public byte[] GetPresentation(string uniqueName)
    //    {
    //        if (string.IsNullOrEmpty(uniqueName)) return null;
    //        Presentation presentation = _presentationDAL.GetPresentation(uniqueName, _sourceDAL);
    //        return BinarySerializer.Serialize<Presentation>(presentation);
    //    }

    //    public Slide[] LoadSlides(string presentationUniqueName, int[] slideIdArr)
    //    {
    //        return _presentationDAL.LoadSlides(presentationUniqueName,
    //                                           slideIdArr, _sourceDAL);
    //    }

    //    public bool CreatePresentation(PresentationInfo presentationInfo)
    //    {
    //        if (null != _presentationDAL.GetPresentationInfo(presentationInfo.UniqueName)) return false;
    //        return _presentationDAL.SavePresentation(
    //            presentationInfo.CreatePresentationStub());
    //    }

    //    public bool SavePresentationChanges(UserIdentity userIdentity,
    //        PresentationInfo presentationInfo, Slide[] newSlideArr,
    //        out ResourceDescriptor[] resourcesNotExists)
    //    {
    //        resourcesNotExists = new ResourceDescriptor[] { };
    //        // необходим лок уровня презентации
    //        LockingInfo info = _lockService.GetLockInfo(ObjectKeyCreator.CreatePresentationKey(presentationInfo));
    //        if (info == null || !info.UserIdentity.Equals(userIdentity)) return false;
    //        Presentation presentationStored = _presentationDAL.GetPresentation(
    //            presentationInfo.UniqueName, _sourceDAL);
    //        if (presentationStored == null) return false;
    //        resourcesNotExists = GetNotExistedResource(newSlideArr, presentationInfo);
    //        if (resourcesNotExists.Length != 0) return false;
    //        presentationStored = Merge(userIdentity, presentationInfo, newSlideArr, presentationStored);
    //        if (presentationStored == null) return false;
    //        bool isSuccess = _presentationDAL.SavePresentation(presentationStored);
    //        if (isSuccess)
    //        {
    //            PresentationKey presentationKey = ObjectKeyCreator.CreatePresentationKey(presentationStored);
    //            ObjectChanged(new List<ObjectInfo>() { new ObjectInfo(userIdentity, presentationKey) }, presentationKey, true);
    //        }
    //        return isSuccess;
    //    }

    //    private void ObjectChanged(IList<ObjectInfo> objectInfoList,
    //        PresentationKey presentationKey, bool presentationLevel)
    //    {
    //        if (presentationLevel)
    //            _globalNotifier.Notify(this, "ObjectChanged", objectInfoList);
    //        else
    //            _presentationNotifier.Notify(presentationKey,
    //            "ObjectChanged", objectInfoList);
    //    }

    //    private ResourceDescriptor[] GetNotExistedResource(Presentation presentation)
    //    {
    //        List<ResourceDescriptor> resourcesAbsent = new List<ResourceDescriptor>();
    //        foreach (ResourceDescriptor descriptor in presentation.GetResource())
    //        {
    //            if (!_sourceDAL.IsExists(descriptor))
    //                resourcesAbsent.Add(descriptor);
    //        }
    //        return resourcesAbsent.ToArray();
    //    }

    //    public bool SaveSlideChanges(UserIdentity userIdentity,
    //        string presentationUniqueName, Slide[] slideToSave,
    //        out int[] slideIdNotLocked,
    //        out ResourceDescriptor[] resourcesNotExists)
    //    {
    //        resourcesNotExists = null;
    //        slideIdNotLocked = null;
    //        List<int> slideNotLocked = new List<int>(slideToSave.Length);
    //        //Presentation pres = BinarySerializer.Deserialize<Presentation>(presentation);
    //        // проверка что слайды залочены данным пользователем
    //        foreach (Slide slide in slideToSave)
    //        {
    //            LockingInfo info = _lockService.GetLockInfo(ObjectKeyCreator.CreateSlideKey(presentationUniqueName, slide.Id));
    //            if (info == null || !info.UserIdentity.Equals(userIdentity))
    //                slideNotLocked.Add(slide.Id);
    //        }
    //        slideIdNotLocked = slideNotLocked.ToArray();
    //        Presentation presentation = _presentationDAL.GetPresentation(presentationUniqueName, _sourceDAL);
    //        if (presentation == null) return false;
    //        resourcesNotExists = GetNotExistedResource(slideToSave, new PresentationInfo(presentation));
    //        if (slideIdNotLocked.Length != 0 || resourcesNotExists.Length != 0)
    //            return false;
    //        presentation = Merge(presentation, slideToSave);
    //        if (presentation == null) return false;
    //        bool isSuccess = _presentationDAL.SavePresentation(presentation);
    //        if (isSuccess)
    //        {
    //            List<ObjectInfo> objectInfoList = new List<ObjectInfo>();
    //            foreach (Slide slide in slideToSave)
    //            {
    //                objectInfoList.Add(new ObjectInfo(userIdentity,
    //                    ObjectKeyCreator.CreateSlideKey(presentationUniqueName, slide.Id)));
    //            }
    //            PresentationKey presentationKey = ObjectKeyCreator.CreatePresentationKey(presentationUniqueName);
    //            ObjectChanged(objectInfoList, presentationKey, false);
    //        }
    //        return isSuccess;
    //    }

    //    private ResourceDescriptor[] GetNotExistedResource(
    //        IEnumerable<Slide> slideArr, PresentationInfo presentationInfo)
    //    {
    //        List<ResourceDescriptor> resourcesAbsent = new List<ResourceDescriptor>();
    //        foreach (Slide slide in slideArr)
    //        {
    //            foreach (ResourceDescriptor descriptor in slide.GetResource(presentationInfo))
    //            {
    //                if (!_sourceDAL.IsExists(descriptor))
    //                    resourcesAbsent.Add(descriptor);
    //            }
    //        }
    //        return resourcesAbsent.ToArray();
    //    }

    //    public bool DeletePresentation(string uniqueName)
    //    {
    //        PresentationInfo presentationInfo = _presentationDAL.GetPresentationInfo(uniqueName);
    //        if (presentationInfo == null) return false;
    //        // проверка на блокировки презентации и сладов
    //        List<ObjectKey> objectKeys = new List<ObjectKey>() { ObjectKeyCreator.CreatePresentationKey(presentationInfo) };
    //        foreach (SlideInfo slideInfo in presentationInfo.SlideInfoList)
    //        {
    //            objectKeys.Add(ObjectKeyCreator.CreateSlideKey(uniqueName, slideInfo.Id));
    //        }
    //        foreach (ObjectKey objectKey in objectKeys)
    //        {
    //            if (_lockService.GetLockInfo(objectKey) != null)
    //                return false;
    //        }
    //        bool isSuccess = _presentationDAL.DeletePresentation(uniqueName);
    //        _sourceDAL.DeleteLocalSourceFolder(uniqueName);
    //        return isSuccess;
    //    }

    //    public PresentationStatus GetPresentationStatus(string uniqueName, out UserIdentity userIdentity)
    //    {
    //        userIdentity = null;
    //        PresentationInfo presentationInfo = _presentationDAL.GetPresentationInfo(uniqueName);
    //        if (presentationInfo == null) return PresentationStatus.Deleted;
    //        LockingInfo info = _lockService.GetLockInfo(ObjectKeyCreator.CreatePresentationKey(presentationInfo));
    //        if (info != null)
    //        {
    //            userIdentity = info.UserIdentity;
    //            switch (info.RequireLock)
    //            {
    //                case RequireLock.ForEdit:
    //                    return PresentationStatus.LockedForEdit;
    //                case RequireLock.ForShow:
    //                    return PresentationStatus.LockedForShow;
    //                default:
    //                    return PresentationStatus.Unknown;
    //            }
    //        }
    //        foreach (SlideInfo slideInfo in presentationInfo.SlideInfoList)
    //        {
    //            if ((info = _lockService.GetLockInfo(ObjectKeyCreator.CreateSlideKey(presentationInfo.UniqueName, slideInfo.Id))) != null)
    //            {
    //                userIdentity = info.UserIdentity;
    //                return PresentationStatus.SlideLocked;
    //            }
    //        }
    //        return PresentationStatus.ExistsAndUnLocked;
    //    }

    //    public bool AcquireLockForPresentation(UserIdentity userIdentity, string uniqueName, RequireLock requireLock)
    //    {
    //        if (null == _presentationDAL.GetPresentationInfo(uniqueName)) return false;
    //        return _lockService.AcquireLock(userIdentity, ObjectKeyCreator.CreatePresentationKey(uniqueName), requireLock);
    //    }

    //    public bool ReleaseLockForPresentation(UserIdentity userIdentity, string uniqueName)
    //    {
    //        return _lockService.ReleaseLock(userIdentity, ObjectKeyCreator.CreatePresentationKey(uniqueName));
    //    }

    //    public bool AcquireLockForSlide(UserIdentity userIdentity, string presentationUniqueName, int slideId, RequireLock requireLock)
    //    {
    //        PresentationInfo presentationInfo = _presentationDAL.GetPresentationInfo(presentationUniqueName);
    //        if (null == presentationInfo) return false;
    //        if (null == presentationInfo.SlideInfoList.Find(sl => sl.Id == slideId)) return false;
    //        return _lockService.AcquireLock(userIdentity,
    //            ObjectKeyCreator.CreateSlideKey(presentationUniqueName, slideId), requireLock);
    //    }

    //    public bool ReleaseLockForSlide(UserIdentity userIdentity, string presentationUniqueName, int slideId)
    //    {
    //        return _lockService.ReleaseLock(userIdentity,
    //            ObjectKeyCreator.CreateSlideKey(presentationUniqueName, slideId));
    //    }

    //    public LockingInfo[] GetLockingInfo(ObjectKey[] objectKeyArr)
    //    {
    //        List<LockingInfo> lockingInfoArr = new List<LockingInfo>(objectKeyArr.Length);
    //        foreach (ObjectKey objectKey in objectKeyArr)
    //        {
    //            LockingInfo info = _lockService.GetLockInfo(objectKey);
    //            if (info != null)
    //                lockingInfoArr.Add(info);
    //        }
    //        return lockingInfoArr.ToArray();
    //    }

    //    public LockingInfo[] GetLockingInfo(string uniqueName)
    //    {
    //        PresentationInfo presentationInfo = _presentationDAL.GetPresentationInfo(uniqueName);
    //        List<ObjectKey> objectKeys = new List<ObjectKey>() { ObjectKeyCreator.CreatePresentationKey(uniqueName) };
    //        foreach (SlideInfo slideInfo in presentationInfo.SlideInfoList)
    //        {
    //            objectKeys.Add(ObjectKeyCreator.CreateSlideKey(uniqueName, slideInfo.Id));
    //        }
    //        LockingInfo[] lockingInfos = GetLockingInfo(objectKeys.ToArray());
    //        List<LockingInfo> lockingList = new List<LockingInfo>();
    //        foreach (LockingInfo info in lockingInfos)
    //        {
    //            if (info != null)
    //                lockingList.Add(info);
    //        }
    //        return lockingList.ToArray();
    //    }

    //    private Presentation Merge(Presentation presentation, Slide[] slides)
    //    {
    //        foreach (Slide slide in slides)
    //        {
    //            Slide storedSlide = presentation.SlideList.Find(
    //                sl => sl.Id == slide.Id);
    //            if (storedSlide == null) return null;
    //            storedSlide.SaveSlideLevelChanges(slide);
    //            storedSlide.ModifiedUtc = DateTime.Now;
    //        }
    //        return presentation;
    //    }

    //    private Presentation Merge(UserIdentity identity,
    //        PresentationInfo presentationInfo,
    //        Slide[] newSlideArr,
    //        Presentation presentationStored)
    //    {
    //        foreach (Slide slide in newSlideArr)
    //        {
    //            Slide slideStored = presentationStored.SlideList.Find(
    //                sl => sl.Id == slide.Id);
    //            if (slideStored != null) return null;
    //            presentationStored.SlideList.Add(slide);
    //        }
    //        // анализ удаленных слайдов
    //        List<Slide> slideListDeleted = presentationStored.SlideList.FindAll(
    //            sl=>!presentationInfo.SlideInfoList.Exists(sli=>sli.Id == sl.Id));
    //        bool anySlideLocked = slideListDeleted.Any(
    //            sl => null != _lockService.GetLockInfo(
    //                ObjectKeyCreator.CreateSlideKey(presentationInfo.UniqueName, sl.Id)));
    //        if (anySlideLocked) return null;
    //        // удаляем слайды
    //        presentationStored.SlideList.RemoveAll(slideListDeleted.Contains);
    //        presentationStored.SavePresentationLevelChanges(presentationInfo);
    //        return presentationStored;
    //    }

    //    #region Implementation of ISourceDAL

    //    //public void SaveSource(FileTransferObject obj)
    //    //{
    //    //    _sourceDAL.SaveSource(obj);
    //    //}

    //    //public string SaveSourceWithAnotherName(FileTransferObject obj)
    //    //{
    //    //    return _sourceDAL.SaveSourceWithAnotherName(obj);
    //    //}

    //    public bool DeleteSource(ResourceDescriptor descriptor)
    //    {
    //        PresentationInfo[] presentationInfos = _presentationDAL.GetPresentationWhichContainsSource(descriptor);
    //        if (presentationInfos != null && presentationInfos.Length != 0) return false;
    //        return _sourceDAL.DeleteSource(descriptor);
    //    }

    //    public FileSaveStatus GetSourceStatus(ResourceDescriptor descriptor)
    //    {
    //        return _sourceDAL.GetSourceStatus(descriptor);
    //    }

    //    public ResourceDescriptor CopySourceFromGlobalToLocal(ResourceDescriptor resourceDescriptor, string presentationUniqueName)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public ResourceDescriptor CopySourceFromLocalToGlobal(ResourceDescriptor resourceDescriptor)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    //public FileSaveStatus CreateSource(FileTransferObject obj)
    //    //{
    //    //    return _sourceDAL.CreateSource(obj);
    //    //}

    //    //public FileTransferObject? GetSource(ResourceDescriptor resourceDescriptor)
    //    //{
    //    //    return _sourceDAL.GetSource(resourceDescriptor);
    //    //}

    //    public Dictionary<string, IList<ResourceDescriptor>> GetGlobalSources()
    //    {
    //        return _sourceDAL.GetGlobalSources();
    //    }
    //    //public ResourceDescriptor[] GetGlobalSources()
    //    //{
    //    //    List<ResourceDescriptor> descriptors = new List<ResourceDescriptor>();
    //    //    foreach (KeyValuePair<string, IList<ResourceDescriptor>> pair in _sourceDAL.GetGlobalSources())
    //    //    {
    //    //        descriptors.AddRange(pair.Value);
    //    //    }
    //    //    return descriptors.ToArray();
    //    //}


    //    public Dictionary<string, IList<ResourceDescriptor>> GetLocalSources(PresentationInfo info)
    //    {
    //        return _sourceDAL.GetLocalSources(info);
    //    }

    //    //public ResourceDescriptor[] GetLocalSources(PresentationInfo info)
    //    //{
    //    //    List<ResourceDescriptor> descriptors = new List<ResourceDescriptor>();
    //    //    foreach (KeyValuePair<string, IList<ResourceDescriptor>> pair in _sourceDAL.GetLocalSources(info))
    //    //    {
    //    //        descriptors.AddRange(pair.Value);
    //    //    }
    //    //    return descriptors.ToArray();
    //    //}

    //    public PresentationInfo[] GetPresentationWhichContainsSource(ResourceDescriptor resourceDescriptor)
    //    {
    //        return _presentationDAL.GetPresentationWhichContainsSource(resourceDescriptor);
    //    }

    //    public void SubscribeForMonitor(PresentationKey presentationKey, UserIdentity identity)
    //    {
    //        _presentationNotifier.Subscribe(presentationKey, identity);
    //    }

    //    public void UnSubscribeForMonitor(PresentationKey presentationKey, UserIdentity identity)
    //    {
    //        _presentationNotifier.Unsubscribe(presentationKey, identity);
    //    }

    //    public void SubscribeForGlobalMonitoring(UserIdentity identity)
    //    {
    //        _globalNotifier.Subscribe(this, identity);
    //    }

    //    public void UnSubscribeForGlobalMonitoring(UserIdentity identity)
    //    {
    //        _globalNotifier.Unsubscribe(this, identity);
    //    }

    //    #endregion

    //    #region Implementation of IPing

    //    public void Ping(UserIdentity identity)
    //    {
    //        _presentationNotifier.RefreshSubscribers(identity);
    //        _globalNotifier.RefreshSubscribers(identity);
    //    }

    //    #endregion

    //    #region Implementation of IResourceRemoteCRUD

    //    public FileSaveStatus Send(UserIdentity userIdentity, FileTransferObject obj, SourceStatus status)
    //    {
    //        return _resourceRemoteCRUD.Send(userIdentity, obj, status);
    //    }

    //    public FileTransferObject? Receive(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor)
    //    {
    //        return _resourceRemoteCRUD.Receive(userIdentity, resourceDescriptor);
    //    }

    //    #endregion
    //}

    #endregion

    public class PresentationWorker : BasePresentationWorker, IPresentationWorker
    {
        private readonly ServerSideSourceTransfer _serverSideSourceTransfer;
        private readonly PresentationExportHelper _serverSidePresentationTransfer;

        private readonly NotificationManager<PresentationKey>.NotificationStore<UserIdentity, IPresentationNotifier> _presentationNotifier;
        private readonly NotificationManager<IPresentationWorker>.NotificationStore<UserIdentity, IPresentationNotifier> _globalNotifier;

        public PresentationWorker(IServerConfiguration configuration, ILockService lockService)
        {
            _configuration = configuration;
            _lockService = lockService;// new LockingService();
            _presentationDAL = new PresentationDALCaching(_configuration);
            _sourceDAL = new SourceDALCaching(_configuration, false);           //new SourceDAL(_configuration);
            ((SourceDAL)_sourceDAL).CreateHardwareSources();

            _deviceSourceDAL = new DeviceSourceDALCaching(_configuration, false);
            ((DeviceSourceDAL)_deviceSourceDAL).CreateHardwareSources();

            _presentationDAL.Init(_sourceDAL, _deviceSourceDAL);

            _serverSideSourceTransfer = new ServerSideSourceTransfer(_sourceDAL);
            _serverSidePresentationTransfer = new PresentationExportHelper((IServerConfiguration)_configuration, _presentationDAL);
            _presentationNotifier =
                NotificationManager<PresentationKey>.Instance.RegisterDuplexService
                <UserIdentity, IPresentationNotifier>
                (NotifierBehaviour.OneInstancePerKey);
            _globalNotifier = NotificationManager<IPresentationWorker>.Instance.RegisterDuplexService
                <UserIdentity, IPresentationNotifier>
                (NotifierBehaviour.OneInstance);

            Init();
            //_lockService.AddItem += new StorageAction<ObjectKey, LockingInfo>(_lockService_AddItem);
            //_lockService.RemoveItem += new StorageAction<ObjectKey, LockingInfo>(_lockService_RemoveItem);

            //_sourceDAL.OnResourceAdded += new EventHandler<SourceEventArg>(_sourceDAL_OnResourceAdded);
            //_sourceDAL.OnResourceDeleted += new EventHandler<SourceEventArg>(_sourceDAL_OnResourceDeleted);

            //_presentationDAL.OnPresentationAdded += new EventHandler<PresentationEventArg>(_presentationDAL_OnPresentationAdded);
            //_presentationDAL.OnPresentationDeleted += new EventHandler<PresentationEventArg>(_presentationDAL_OnPresentationDeleted);
            //_lockService.PresentationDAL = _presentationDAL;
            //_presentationDAL.LockService = _lockService;
        }

        protected override void LabelStorageAdapter_OnUpdate(object sender, LabelEventArg e)
        {
            _globalNotifier.Notify(null, this, "LabelUpdated", e.Label);
        }

        protected override void LabelStorageAdapter_OnDelete(object sender, LabelEventArg e)
        {
            _globalNotifier.Notify(null, this, "LabelDeleted", e.Label);
        }

        protected override void LabelStorageAdapter_OnAdd(object sender, LabelEventArg e)
        {
            _globalNotifier.Notify(null, this, "LabelAdded", e.Label);
        }

        override protected void _presentationDAL_OnPresentationDeleted(object sender, PresentationEventArg e)
        {
            _globalNotifier.Notify(e.UserIdentity, this, "PresentationDeleted", e.UserIdentity, e.PresentationInfo);
        }

        override protected void _presentationDAL_OnPresentationAdded(object sender, PresentationEventArg e)
        {
            _globalNotifier.Notify(e.UserIdentity, this, "PresentationAdded", e.UserIdentity, e.PresentationInfo);
        }

        override protected void _sourceDAL_OnResourceDeleted(object sender, SourceEventArg<ResourceDescriptor> e)
        {
            ResourceDescriptor descriptor = e.ResourceDescriptor;
            if (descriptor.IsLocal && !String.IsNullOrEmpty(descriptor.PresentationUniqueName))
            {
                _presentationNotifier.Notify(e.UserIdentity, ObjectKeyCreator.CreatePresentationKey(descriptor.PresentationUniqueName),
                    "ResourceDeleted", e.UserIdentity, descriptor);
            }
            else
            {
                _globalNotifier.Notify(e.UserIdentity, this, "ResourceDeleted", e.UserIdentity,
                    descriptor);
            }
        }

        override protected void _sourceDAL_OnResourceAdded(object sender, SourceEventArg<ResourceDescriptor> e)
        {
            ResourceDescriptor descriptor = e.ResourceDescriptor;
            if (descriptor.IsLocal && !string.IsNullOrEmpty(descriptor.PresentationUniqueName))
            {
                _presentationNotifier.Notify(e.UserIdentity, ObjectKeyCreator.CreatePresentationKey(descriptor.PresentationUniqueName),
                    "ResourceAdded", e.UserIdentity, descriptor);
            }
            else
            {
                _globalNotifier.Notify(e.UserIdentity, this, "ResourceAdded", e.UserIdentity,
                    descriptor);
            }
        }

        protected override void _sourceDAL_OnResourceUpdated(object sender, SourceEventArg<ResourceDescriptor> e)
        {
            ResourceDescriptor descriptor = e.ResourceDescriptor;
            if (descriptor.IsLocal && !string.IsNullOrEmpty(descriptor.PresentationUniqueName))
            {
                _presentationNotifier.Notify(e.UserIdentity, ObjectKeyCreator.CreatePresentationKey(descriptor.PresentationUniqueName),
                    "ResourceUpdated", e.UserIdentity, descriptor);
            }
            else
            {
                _globalNotifier.Notify(e.UserIdentity, this, "ResourceUpdated", e.UserIdentity,
                    descriptor);
            }
        }

        protected override void _deviceResourceDAL_OnResourceAdded(object sender, SourceEventArg<DeviceResourceDescriptor> e)
        {
            DeviceResourceDescriptor descriptor = e.ResourceDescriptor;
            _globalNotifier.Notify(e.UserIdentity, this, "DeviceResourceAdded", e.UserIdentity, descriptor);
        }

        protected override void _deviceResourceDAL_OnResourceDeleted(object sender, SourceEventArg<DeviceResourceDescriptor> e)
        {
            DeviceResourceDescriptor descriptor = e.ResourceDescriptor;
            _globalNotifier.Notify(e.UserIdentity, this, "DeviceResourceDeleted", e.UserIdentity, descriptor);
        }

        protected override void _deviceSourceDAL_OnResourceUpdated(object sender, SourceEventArg<DeviceResourceDescriptor> e)
        {
            DeviceResourceDescriptor descriptor = e.ResourceDescriptor;
            _globalNotifier.Notify(e.UserIdentity, this, "DeviceResourceUpdated", e.UserIdentity, descriptor);
        }

        override protected void _lockService_RemoveItem(UserIdentity userIdentity, ObjectKey key, LockingInfo value)
        {
            PresentationKey presentationKey = GetPresentationKey(key);
            if (presentationKey == null) return;
            //_presentationNotifier.Unsubscribe(presentationKey, value.UserIdentity);
            if (key.GetObjectType() == ObjectType.Presentation)
                _globalNotifier.Notify(userIdentity, this, "ObjectUnLocked", value);
            else
            {
                _presentationNotifier.Notify(userIdentity, presentationKey, "ObjectUnLocked", value);
            }
        }

        override protected void _lockService_AddItem(UserIdentity userIdentity, ObjectKey key, LockingInfo value)
        {
            PresentationKey presentationKey = GetPresentationKey(key);
            if (presentationKey == null) return;
            if (key.GetObjectType() == ObjectType.Presentation)
                _globalNotifier.Notify(userIdentity, this, "ObjectLocked", value);
            else
            {
                _presentationNotifier.Notify(userIdentity, presentationKey, "ObjectLocked", value);
            }
            //_presentationNotifier.SubscribeForMonitor(presentationKey, value.UserIdentity);
        }

        #region Implementation of ISourceDAL

        public override void SubscribeForMonitor(PresentationKey presentationKey, UserIdentity identity)
        {
            _presentationNotifier.Subscribe(presentationKey, identity);
        }

        public override void UnSubscribeForMonitor(PresentationKey presentationKey, UserIdentity identity)
        {
            _presentationNotifier.Unsubscribe(presentationKey, identity);
        }

        public override void SubscribeForGlobalMonitoring(UserIdentity identity)
        {
            _globalNotifier.Subscribe(this, identity);
        }

        public override void UnSubscribeForGlobalMonitoring(UserIdentity identity)
        {
            _globalNotifier.Unsubscribe(this, identity);
        }

        #endregion

        protected override void ObjectChanged(UserIdentity userIdentity, IList<ObjectInfo> objectInfoList,
    PresentationInfo presentationInfo, bool presentationLevel)
        {
            base.ObjectChanged(userIdentity, objectInfoList, presentationInfo, presentationLevel);
            PresentationKey presentationKey = new PresentationKey(presentationInfo.UniqueName);
            if (presentationLevel)
                _globalNotifier.Notify(userIdentity, this, "ObjectChanged", userIdentity, objectInfoList);
            else
                _presentationNotifier.Notify(userIdentity, presentationKey,
                "ObjectChanged", userIdentity, objectInfoList);
        }


        #region Implementation of IPing

        public override void Ping(UserIdentity identity)
        {
            _presentationNotifier.RefreshSubscribers(identity);
            _globalNotifier.RefreshSubscribers(identity);
        }

        #endregion

        #region Implementation of IFileTransfer

        public FileSaveStatus Send(UserIdentity userIdentity, FileTransferObject obj)
        {
            try
            {
                if (_serverSideSourceTransfer.Contains(userIdentity))
                    return _serverSideSourceTransfer.Send(userIdentity, obj);
                else if (_serverSidePresentationTransfer.Contains(userIdentity))
                    return _serverSidePresentationTransfer.Send(userIdentity, obj);
                else
                    return FileSaveStatus.Abort;
            }
            catch (Exception ex)
            {
                _configuration.EventLog.WriteError(string.Format("PresentationWorker.Send: userIdentity:{0}, Resource:{1}, \n{2}",
                    userIdentity, obj.FileId, ex));
                return FileSaveStatus.Abort;
            }
        }

        public FileTransferObject? Receive(UserIdentity userIdentity, string resourceId)
        {
            try
            {
                if (_serverSideSourceTransfer.Contains(userIdentity))
                    return _serverSideSourceTransfer.Receive(userIdentity, resourceId);
                else if (_serverSidePresentationTransfer.Contains(userIdentity))
                    return _serverSidePresentationTransfer.Receive(userIdentity, resourceId);
                else
                    return null;

            }
            catch (Exception ex)
            {
                _configuration.EventLog.WriteError(string.Format("PresentationWorker.Receive: userIdentity:{0}, Resource:{1}, \n{2}",
                    userIdentity, resourceId, ex));
                return null;
            }
        }

        public void Terminate(UserIdentity userIdentity)
        {
            try
            {
                if (_serverSideSourceTransfer.Contains(userIdentity))
                    _serverSideSourceTransfer.Terminate(userIdentity);
                else if (_serverSidePresentationTransfer.Contains(userIdentity))
                    _serverSidePresentationTransfer.Terminate(userIdentity);

            }
            catch (Exception ex)
            {
                _configuration.EventLog.WriteError(string.Format("PresentationWorker.Terminate: userIdentity:{0} \n{1}",
                    userIdentity, ex));
            }
        }

        #endregion

        #region ISourceTransferCRUD

        public override FileSaveStatus InitSourceUpload(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor, SourceStatus status, out string otherResourceId)
        {
            return _serverSideSourceTransfer.InitSourceUpload(userIdentity, resourceDescriptor, status, out otherResourceId);
        }

        public override ResourceDescriptor InitSourceDownload(UserIdentity identity, ResourceDescriptor resourceDescriptor)
        {
            return _serverSideSourceTransfer.InitSourceDownload(identity, resourceDescriptor);
        }
        
        public int ForwardMoveNeeded()
        {
            return _serverSideSourceTransfer.ForwardMoveNeeded();
        }

        public double GetCurrentSpeed()
        {
            return _serverSideSourceTransfer.GetCurrentSpeed();
        }

        public string GetCurrentFile()
        {
            return _serverSideSourceTransfer.GetCurrentFile();
        }

        
        public override FileSaveStatus SaveSource(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor, SourceStatus status, out string otherResourceId)
        {
            return _serverSideSourceTransfer.SaveSource(userIdentity, resourceDescriptor, status, out otherResourceId);
        }

        public override void DoneSourceTransfer(UserIdentity userIdentity)
        {
            _serverSideSourceTransfer.DoneSourceTransfer(userIdentity);
        }

        #endregion

        #region IPresentationTransfer

        public override FilesGroup GetPresentationForExport(string uniqueName)
        {
            return _serverSidePresentationTransfer.GetPresentationForExport(uniqueName);
        }

        public override FilesGroup GetPresentationSchemaFilesForExport()
        {
            return _serverSidePresentationTransfer.GetPresentationSchemaFilesForExport();
        }

        public override FilesGroup InitPresentationSchemaExport(UserIdentity userIdentity, FilesGroup filesGroup)
        {
            return _serverSidePresentationTransfer.InitPresentationSchemaExport(userIdentity, filesGroup);
        }

        public override FilesGroup InitPresentationExport(UserIdentity userIdentity, FilesGroup filesGroup)
        {
            return _serverSidePresentationTransfer.InitPresentationExport(userIdentity, filesGroup);
        }

        public override void DonePresentationTransfer(UserIdentity userIdentity)
        {
            _serverSidePresentationTransfer.DonePresentationTransfer(userIdentity);
        }

        #endregion

        protected override bool IsStandAlone
        {
            get { return false; }
        }

        public bool IsUserParticipateInResourceTransfer(UserIdentity userIdentity)
        {
            return _serverSideSourceTransfer.Contains(userIdentity)
                || _serverSidePresentationTransfer.Contains(userIdentity);
        }

    }

}
