using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using TechnicalServices.Common;
using TechnicalServices.Common.Locking;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util;

namespace DomainServices.PresentationManagement
{
    public abstract class BasePresentationWorker : IPresentationWorkerCommon
    {
        protected IConfiguration _configuration;
        protected ILockService _lockService;
        protected IPresentationDAL _presentationDAL;
        protected ISourceDAL _sourceDAL;
        protected IDeviceSourceDAL _deviceSourceDAL;

        protected abstract bool IsStandAlone { get; }

        protected void Init()
        {
            _lockService.AddItem += _lockService_AddItem;
            _lockService.RemoveItem += _lockService_RemoveItem;

            _sourceDAL.OnResourceAdded += _sourceDAL_OnResourceAdded;
            _sourceDAL.OnResourceDeleted += _sourceDAL_OnResourceDeleted;
            _sourceDAL.OnResourceUpdated += _sourceDAL_OnResourceUpdated;

            _deviceSourceDAL.OnResourceAdded += _deviceResourceDAL_OnResourceAdded;
            _deviceSourceDAL.OnResourceDeleted += _deviceResourceDAL_OnResourceDeleted;
            _deviceSourceDAL.OnResourceUpdated += _deviceSourceDAL_OnResourceUpdated;

            _presentationDAL.OnPresentationAdded += _presentationDAL_OnPresentationAdded;
            _presentationDAL.OnPresentationDeleted += _presentationDAL_OnPresentationDeleted;

            _configuration.LabelStorageAdapter.OnAdd += LabelStorageAdapter_OnAdd;
            _configuration.LabelStorageAdapter.OnDelete += LabelStorageAdapter_OnDelete;
            _configuration.LabelStorageAdapter.OnUpdate += LabelStorageAdapter_OnUpdate;
        }


        protected abstract void LabelStorageAdapter_OnUpdate(object sender, LabelEventArg e);

        protected abstract void LabelStorageAdapter_OnDelete(object sender, LabelEventArg e);

        protected abstract void LabelStorageAdapter_OnAdd(object sender, LabelEventArg e);

        protected abstract void _presentationDAL_OnPresentationDeleted(object sender, PresentationEventArg e);

        protected abstract void _presentationDAL_OnPresentationAdded(object sender, PresentationEventArg e);

        protected abstract void _sourceDAL_OnResourceDeleted(object sender, SourceEventArg<ResourceDescriptor> e);

        protected abstract void _sourceDAL_OnResourceAdded(object sender, SourceEventArg<ResourceDescriptor> e);

        protected abstract void _sourceDAL_OnResourceUpdated(object sender, SourceEventArg<ResourceDescriptor> e);

        protected abstract void _deviceResourceDAL_OnResourceAdded(object sender,
                                                                   SourceEventArg<DeviceResourceDescriptor> e);

        protected abstract void _deviceResourceDAL_OnResourceDeleted(object sender,
                                                                     SourceEventArg<DeviceResourceDescriptor> e);

        protected abstract void _deviceSourceDAL_OnResourceUpdated(object sender,
                                                           SourceEventArg<DeviceResourceDescriptor> e);

        protected abstract void _lockService_RemoveItem(UserIdentity userIdentity, ObjectKey key, LockingInfo value);

        protected abstract void _lockService_AddItem(UserIdentity userIdentity, ObjectKey key, LockingInfo value);

        private PresentationInfo GetPresentationInfoWithSlideLockInfo(string uniqueName)
        {
            PresentationInfo info = _presentationDAL.GetPresentationInfo(uniqueName);
            if (info == null) return null;
            foreach (SlideInfo slideInfo in info.SlideInfoList)
            {
                slideInfo.LockingInfo = _lockService.GetLockInfo(
                    ObjectKeyCreator.CreateSlideKey(info.UniqueName, slideInfo.Id));
            }
            return info;
        }

        private PresentationInfo GetPresentationInfoByNameWithSlideLockInfo(string presentationName)
        {
            PresentationInfo info = _presentationDAL.GetPresentationInfoByPresentationName(presentationName);
            if (info == null) return null;
            foreach (SlideInfo slideInfo in info.SlideInfoList)
            {
                slideInfo.LockingInfo = _lockService.GetLockInfo(
                    ObjectKeyCreator.CreateSlideKey(info.UniqueName, slideInfo.Id));
            }
            return info;
        }

        protected DeviceResourceDescriptor[] GetNotExistedDeviceResource(
            IEnumerable<Slide> slideArr, PresentationInfo presentationInfo)
        {
            List<DeviceResourceDescriptor> resourcesAbsent = new List<DeviceResourceDescriptor>();
            foreach (Slide slide in slideArr)
            {
                foreach (DeviceResourceDescriptor descriptor in slide.GetDeviceResource(presentationInfo))
                {
                    if (!_deviceSourceDAL.IsExists(descriptor))
                        resourcesAbsent.Add(descriptor);
                }
            }
            return resourcesAbsent.ToArray();
        }

        protected ResourceDescriptor[] GetNotExistedResource(IEnumerable<Slide> slideArr)
        {
            List<ResourceDescriptor> resourcesAbsent = new List<ResourceDescriptor>();
            foreach (Slide slide in slideArr)
            {
                foreach (ResourceDescriptor descriptor in slide.GetResource())
                {
                    if (!_sourceDAL.IsExists(descriptor))
                        resourcesAbsent.Add(descriptor);
                }
            }
            return resourcesAbsent.ToArray();
        }

        protected Presentation Merge(UserIdentity identity,
                                     PresentationInfo presentationInfo,
                                     Slide[] newSlideArr,
                                     Presentation presentationStored,
                                     out LockingInfo[] lockedSlides,
                                     out Slide[] slideAlreadyExists)
        {
            IEnumerable<Slide> existedSlides = presentationStored.SlideList.Where(
                sl => newSlideArr.Any(newsl=>newsl.Id == sl.Id));
            if (existedSlides.Count() != 0)
            {
                slideAlreadyExists = existedSlides.ToArray();
                lockedSlides = new LockingInfo[] {};
                return null;
            }

            foreach (Slide slide in newSlideArr)
            {
                //Slide slideStored = presentationStored.SlideList.Find(
                //    sl => sl.Id == slide.Id);
                //if (slideStored != null) return null;
                slide.State = SlideState.Normal;
                presentationStored.SlideList.Add(slide);
            }
            // анализ удаленных слайдов
            List<Slide> slideListDeleted = presentationStored.SlideList.FindAll(
                sl => !presentationInfo.SlideInfoList.Exists(sli => sli.Id == sl.Id));
            if (!IsStandAlone)
            {
                IEnumerable<LockingInfo> lockedSl = slideListDeleted.Select(
                    sl => _lockService.GetLockInfo(ObjectKeyCreator.CreateSlideKey(presentationInfo.UniqueName, sl.Id))).Where(
                    li=>li != null);

                if (lockedSl.Count() != 0)
                {
                    lockedSlides = lockedSl.ToArray();
                    slideAlreadyExists = new Slide[] {};
                    return null;
                }
            }
            // удаляем слайды
            presentationStored.SlideList.RemoveAll(slideListDeleted.Contains);
            presentationStored.SavePresentationLevelChanges(presentationInfo);
            slideAlreadyExists = new Slide[] { };
            lockedSlides = new LockingInfo[] { };
            return presentationStored;
        }

        protected Presentation Merge(Presentation presentation, Slide[] slides)
        {
            foreach (Slide slide in slides)
            {
                Slide storedSlide = presentation.SlideList.Find(
                    sl => sl.Id == slide.Id);
                if (storedSlide == null) return null;
                storedSlide.SaveSlideLevelChanges(slide);
                storedSlide.Modified = DateTime.Now;
            }
            return presentation;
        }


        protected virtual void ObjectChanged(UserIdentity userIdentity, IList<ObjectInfo> objectInfoList,
                                              PresentationInfo presentationInfo, bool presentationLevel)
        {
            PresentationChange(presentationInfo);
            if (!presentationLevel)
                SlideChanged(presentationInfo,
                             objectInfoList.Select(oi => oi.ObjectKey).OfType<SlideKey>().Select(sk => sk.Id));
        }

        protected PresentationKey GetPresentationKey(ObjectKey key)
        {
            PresentationKey presentationKey = null;
            if (key is PresentationKey)
                presentationKey = key as PresentationKey;
            else if (key is SlideKey)
                presentationKey = (PresentationKey)((SlideKey)key).PresentationKey;
            return presentationKey;
        }

        protected void PresentationChange(PresentationInfo info)
        {
            if (OnPresentationChanged != null)
            {
                OnPresentationChanged.Invoke(this, new PresentationChangedEventArgs(info));
            }
        }

        protected void SlideChanged(PresentationInfo info, IEnumerable<int> slideIds)
        {
            if (OnSlideChanged != null)
            {
                OnSlideChanged.Invoke(this, new SlideChangedEventArgs(info.UniqueName, slideIds));
            }
        }


        #region Implementation of IPing

        public virtual void Ping(UserIdentity identity)
        {
        }

        #endregion

        //#region Implementation of IFileTransfer

        //public virtual FileSaveStatus Send(UserIdentity userIdentity, FileTransferObject obj)
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual FileTransferObject? Receive(UserIdentity userIdentity, string resourceId)
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void Terminate(UserIdentity userIdentity)
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion

        #region Implementation of IPresentationWorker

        public PresentationInfo[] GetPresentationWhichContainsLabel(int labelId)
        {
            return _presentationDAL.GetPresentationWhichContainsLabel(labelId);
        }

        public IList<PresentationInfoExt> GetPresentationInfoList()
        {
            List<PresentationInfoExt> extArr = new List<PresentationInfoExt>();
            foreach (PresentationInfo info in _presentationDAL.GetPresentationInfoList())
            {
                extArr.Add(new PresentationInfoExt(info,
                                                   _lockService.GetLockInfo(
                                                       ObjectKeyCreator.CreatePresentationKey(info.UniqueName))));
                foreach (SlideInfo slideInfo in info.SlideInfoList)
                {
                    slideInfo.LockingInfo = _lockService.GetLockInfo(
                        ObjectKeyCreator.CreateSlideKey(info.UniqueName, slideInfo.Id));
                }
            }
            return extArr;
        }

        public PresentationInfoExt GetPresentationInfo(string uniqueName)
        {
            PresentationInfo presentationInfo = GetPresentationInfoWithSlideLockInfo(uniqueName);
            if (presentationInfo == null) return null;
            return new PresentationInfoExt(presentationInfo,
                                           _lockService.GetLockInfo(ObjectKeyCreator.CreatePresentationKey(uniqueName)));
        }

        public PresentationInfoExt GetPresentationInfoByName(string presentationName)
        {
            PresentationInfo presentationInfo = GetPresentationInfoByNameWithSlideLockInfo(presentationName);
            if (presentationInfo == null) return null;
            return new PresentationInfoExt(presentationInfo,
                                           _lockService.GetLockInfo(ObjectKeyCreator.CreatePresentationKey(presentationInfo.UniqueName)));
        }

        public byte[] GetPresentation(string uniqueName)
        {
            if (string.IsNullOrEmpty(uniqueName)) return null;
            string[] deletedEquipment;
            Presentation presentation = _presentationDAL.GetPresentation(uniqueName, _sourceDAL, _deviceSourceDAL, out deletedEquipment);
            return BinarySerializer.Serialize(presentation);
        }

        public Slide[] LoadSlides(string presentationUniqueName, int[] slideIdArr)
        {
            return _presentationDAL.LoadSlides(presentationUniqueName,
                                               slideIdArr, _sourceDAL, _deviceSourceDAL);
        }


        public CreatePresentationResult CreatePresentation(UserIdentity sender,
            PresentationInfo presentationInfo,
            out int[] labelNotExists)
        {
            labelNotExists = presentationInfo.GetUsedLabels().Except(
                _configuration.LabelStorageAdapter.GetLabelStorage().Select(lb => lb.Id)).ToArray();
            if (labelNotExists.Length != 0) return CreatePresentationResult.LabelNotExists;
            if (null != _presentationDAL.GetPresentationInfo(presentationInfo.UniqueName)) return CreatePresentationResult.SameUniqueNameExists;
            if (null != _presentationDAL.GetPresentationInfoByPresentationName(presentationInfo.Name))
                return CreatePresentationResult.SameNameExists;

            bool isSuccess = _presentationDAL.SavePresentation(sender,
                                                     presentationInfo.CreatePresentationStub());
            if (isSuccess) return CreatePresentationResult.Ok;
            else return CreatePresentationResult.SameNameExists;
        }

        public SavePresentationResult SavePresentationChanges(UserIdentity userIdentity, PresentationInfo presentationInfo,
            Slide[] newSlideArr, out ResourceDescriptor[] resourcesNotExists,
            out DeviceResourceDescriptor[] deviceResourcesNotExists, out int[] labelNotExists,
            out UserIdentity[] whoLock,
            out int[] slidesAlreadyExistsId)
        {
            resourcesNotExists = new ResourceDescriptor[] { };
            deviceResourcesNotExists = new DeviceResourceDescriptor[] {};
            whoLock = new UserIdentity[] { };
            slidesAlreadyExistsId = new int[] { };
            labelNotExists = presentationInfo.GetUsedLabels().Except(
                _configuration.LabelStorageAdapter.GetLabelStorage().Select(lb => lb.Id)).ToArray();
            if (labelNotExists.Length != 0) return SavePresentationResult.LabelNotExists;
            if (!IsStandAlone)
            {
                // необходим лок уровня презентации
                LockingInfo info = _lockService.GetLockInfo(ObjectKeyCreator.CreatePresentationKey(presentationInfo));
                if (info == null || !info.UserIdentity.Equals(userIdentity)) return SavePresentationResult.PresentationNotLocked;
            }
            string[] deletedEquipment;
            Presentation presentationStored = _presentationDAL.GetPresentation(
                presentationInfo.UniqueName, _sourceDAL, _deviceSourceDAL, out deletedEquipment);
            if (presentationStored == null) return SavePresentationResult.PresentationNotExists;
            resourcesNotExists = GetNotExistedResource(newSlideArr);
            deviceResourcesNotExists = GetNotExistedDeviceResource(newSlideArr, presentationInfo);
            if (resourcesNotExists.Length != 0 || deviceResourcesNotExists.Length != 0) return SavePresentationResult.ResourceNotExists;
            LockingInfo[] lockedSlides;
            Slide[] slideAlreadyExists;
            presentationStored = Merge(userIdentity, presentationInfo, newSlideArr, presentationStored,
                out lockedSlides, out slideAlreadyExists);
            if (presentationStored == null)
            {
                SavePresentationResult result = SavePresentationResult.Unknown;
                if (lockedSlides.Length != 0)
                {
                    whoLock = lockedSlides.Select(li => li.UserIdentity).ToArray();
                    result = SavePresentationResult.SlideLocked;
                }
                if (slideAlreadyExists.Length != 0)
                {
                    slidesAlreadyExistsId = slideAlreadyExists.Select(sl => sl.Id).ToArray();
                    result = SavePresentationResult.SlideAlreadyExists;
                }
                return result;
            }
            bool isSuccess = _presentationDAL.SavePresentation(userIdentity, presentationStored);
            if (isSuccess)
            {
                PresentationKey presentationKey = ObjectKeyCreator.CreatePresentationKey(presentationStored);
                ObjectChanged(userIdentity, new List<ObjectInfo> { new ObjectInfo(userIdentity, presentationKey) },
                              new PresentationInfo(presentationStored), true);
            }
            else
            {
                return SavePresentationResult.Unknown;
            }
            return SavePresentationResult.Ok;
        }

        public bool SaveSlideChanges(UserIdentity userIdentity, string presentationUniqueName,
            Slide[] slideToSave, out int[] slideIdNotLocked,
            out ResourceDescriptor[] resourcesNotExists,
            out DeviceResourceDescriptor[] deviceResourcesNotExists,
            out int[] labelNotExists)
        {
            resourcesNotExists = new ResourceDescriptor[] { };
            deviceResourcesNotExists = new DeviceResourceDescriptor[] {};
            labelNotExists =
                slideToSave.Select(sl => sl.LabelId).Where(id=>id>0).Distinct().Except(
                    _configuration.LabelStorageAdapter.GetLabelStorage().Select(lb => lb.Id)).ToArray();
            List<int> slideNotLocked = new List<int>(slideToSave.Length);
            slideIdNotLocked = slideNotLocked.ToArray();
            if (labelNotExists.Length != 0) return false;
            if (slideToSave.Length == 0) return true;
            if (!IsStandAlone)
            {
                //Presentation pres = BinarySerializer.Deserialize<Presentation>(presentation);
                // проверка что слайды залочены данным пользователем
                foreach (Slide slide in slideToSave)
                {
                    LockingInfo info =
                        _lockService.GetLockInfo(ObjectKeyCreator.CreateSlideKey(presentationUniqueName, slide.Id));
                    if (info == null || !info.UserIdentity.Equals(userIdentity))
                        slideNotLocked.Add(slide.Id);
                }
            }
            slideIdNotLocked = slideNotLocked.ToArray();
            string[] deletedEquipment;
            Presentation presentation = _presentationDAL.GetPresentation(presentationUniqueName,
                _sourceDAL, _deviceSourceDAL, out deletedEquipment);
            if (presentation == null) return false;
            PresentationInfo presentationInfo = new PresentationInfo(presentation);
            resourcesNotExists = GetNotExistedResource(slideToSave);
            deviceResourcesNotExists = GetNotExistedDeviceResource(slideToSave, presentationInfo);
            if (slideIdNotLocked.Length != 0 || resourcesNotExists.Length != 0 || deviceResourcesNotExists.Length != 0)
                return false;
            presentation = Merge(presentation, slideToSave);
            if (presentation == null) return false;
            bool isSuccess = _presentationDAL.SavePresentation(userIdentity, presentation);
            if (isSuccess)
            {
                List<ObjectInfo> objectInfoList = new List<ObjectInfo>();
                foreach (Slide slide in slideToSave)
                {
                    objectInfoList.Add(new ObjectInfo(userIdentity,
                                                      ObjectKeyCreator.CreateSlideKey(presentationUniqueName, slide.Id)));
                }
                PresentationKey presentationKey = ObjectKeyCreator.CreatePresentationKey(presentationUniqueName);
                ObjectChanged(userIdentity, objectInfoList, new PresentationInfo(presentation), false);
            }
            return isSuccess;
        }

        public bool DeletePresentation(UserIdentity sender, string uniqueName)
        {
            PresentationInfo presentationInfo = _presentationDAL.GetPresentationInfo(uniqueName);
            if (presentationInfo == null) return false;
            if (!IsStandAlone)
            {
                // проверка на блокировки презентации и сладов
                List<ObjectKey> objectKeys = new List<ObjectKey> { ObjectKeyCreator.CreatePresentationKey(presentationInfo) };
                foreach (SlideInfo slideInfo in presentationInfo.SlideInfoList)
                {
                    objectKeys.Add(ObjectKeyCreator.CreateSlideKey(uniqueName, slideInfo.Id));
                }
                foreach (ObjectKey objectKey in objectKeys)
                {
                    if (_lockService.GetLockInfo(objectKey) != null)
                        return false;
                }
            }
            bool isSuccess = _presentationDAL.DeletePresentation(sender, uniqueName);
            _sourceDAL.DeleteLocalSourceFolder(uniqueName);
            return isSuccess;
        }

        public PresentationStatus GetPresentationStatus(string uniqueName, out UserIdentity userIdentity)
        {
            userIdentity = null;
            PresentationInfo presentationInfo = _presentationDAL.GetPresentationInfo(uniqueName);
            if (presentationInfo == null) return PresentationStatus.Deleted;
            //if (!IsStandAlone)
            //{
            LockingInfo info = _lockService.GetLockInfo(ObjectKeyCreator.CreatePresentationKey(presentationInfo));
            if (info != null)
            {
                userIdentity = info.UserIdentity;
                switch (info.RequireLock)
                {
                    case RequireLock.ForEdit:
                        return PresentationStatus.LockedForEdit;
                    case RequireLock.ForShow:
                        return PresentationStatus.LockedForShow;
                    default:
                        return PresentationStatus.Unknown;
                }
            }
            foreach (SlideInfo slideInfo in presentationInfo.SlideInfoList)
            {
                if (
                    (info =
                     _lockService.GetLockInfo(ObjectKeyCreator.CreateSlideKey(presentationInfo.UniqueName,
                                                                              slideInfo.Id))) != null)
                {
                    userIdentity = info.UserIdentity;
                    return PresentationStatus.SlideLocked;
                }
            }
            //}
            return PresentationStatus.ExistsAndUnLocked;
        }

        public virtual bool AcquireLockForPresentation(ICommunicationObject communicationObject, UserIdentity userIdentity, string uniqueName,
                                                       RequireLock requireLock)
        {
            if (null == _presentationDAL.GetPresentationInfo(uniqueName)) return false;
            return _lockService.AcquireLock(communicationObject, userIdentity, ObjectKeyCreator.CreatePresentationKey(uniqueName),
                                            requireLock);
        }

        public virtual bool ReleaseLockForPresentation(UserIdentity userIdentity, string uniqueName)
        {
            return _lockService.ReleaseLock(userIdentity, ObjectKeyCreator.CreatePresentationKey(uniqueName));
        }

        public virtual bool AcquireLockForSlide(ICommunicationObject communicationObject, UserIdentity userIdentity, string presentationUniqueName, int slideId,
                                                RequireLock requireLock)
        {
            PresentationInfo presentationInfo = _presentationDAL.GetPresentationInfo(presentationUniqueName);
            if (null == presentationInfo) return false;
            if (null == presentationInfo.SlideInfoList.Find(sl => sl.Id == slideId)) return false;
            return _lockService.AcquireLock(communicationObject, userIdentity,
                                            ObjectKeyCreator.CreateSlideKey(presentationUniqueName, slideId),
                                            requireLock);
        }

        public virtual bool ReleaseLockForSlide(UserIdentity userIdentity, string presentationUniqueName, int slideId)
        {
            return _lockService.ReleaseLock(userIdentity,
                                            ObjectKeyCreator.CreateSlideKey(presentationUniqueName, slideId));
        }

        public virtual LockingInfo[] GetLockingInfo(ObjectKey[] objectKeyArr)
        {
            List<LockingInfo> lockingInfoArr = new List<LockingInfo>(objectKeyArr.Length);
            foreach (ObjectKey objectKey in objectKeyArr)
            {
                LockingInfo info = _lockService.GetLockInfo(objectKey);
                lockingInfoArr.AddNotNull(info);
            }
            return lockingInfoArr.ToArray();
        }

        public virtual LockingInfo[] GetLockingInfo(string uniqueName)
        {
            PresentationInfo presentationInfo = _presentationDAL.GetPresentationInfo(uniqueName);
            List<ObjectKey> objectKeys = new List<ObjectKey> { ObjectKeyCreator.CreatePresentationKey(uniqueName) };
            foreach (SlideInfo slideInfo in presentationInfo.SlideInfoList)
            {
                objectKeys.Add(ObjectKeyCreator.CreateSlideKey(uniqueName, slideInfo.Id));
            }
            LockingInfo[] lockingInfos = GetLockingInfo(objectKeys.ToArray());
            List<LockingInfo> lockingList = new List<LockingInfo>();
            foreach (LockingInfo info in lockingInfos)
            {
                lockingList.AddNotNull(info);
            }
            return lockingList.ToArray();
        }

        public RemoveResult DeleteSource(UserIdentity sender, ResourceDescriptor descriptor)
        {
            PresentationInfo[] presentationInfos = _presentationDAL.GetPresentationWhichContainsSource(descriptor);
            if (presentationInfos != null && presentationInfos.Length != 0) return RemoveResult.LinkedToPresentation;
            return _sourceDAL.DeleteSource(sender, descriptor) ? RemoveResult.Ok : RemoveResult.NotExists;
        }

        public RemoveResult DeleteDeviceSource(UserIdentity sender, DeviceResourceDescriptor descriptor)
        {
            PresentationInfo[] presentationInfos = _presentationDAL.GetPresentationWhichContainsSource(descriptor);
            if (presentationInfos != null && presentationInfos.Length != 0) return RemoveResult.LinkedToPresentation;
            return _deviceSourceDAL.DeleteSource(sender, descriptor) ? RemoveResult.Ok : RemoveResult.NotExists;
        }

        public FileSaveStatus SaveDeviceSource(UserIdentity userIdentity, DeviceResourceDescriptor resourceDescriptor, SourceStatus status, out string newResourceId)
        {
            newResourceId = null;
            if (status == SourceStatus.New && _deviceSourceDAL.IsExists(resourceDescriptor)) return FileSaveStatus.Exists;
            return null == _deviceSourceDAL.SaveSource(userIdentity, resourceDescriptor) ? FileSaveStatus.Exists : FileSaveStatus.Ok;
        }

        public FileSaveStatus GetSourceStatus(ResourceDescriptor descriptor)
        {
            return _sourceDAL.GetSourceStatus(descriptor);
        }

        public bool IsResourceAvailable(ResourceDescriptor descriptor)
        {
            return _sourceDAL.IsResourceAvailable(descriptor);
        }

        public FileSaveStatus GetDeviceSourceStatus(DeviceResourceDescriptor descriptor)
        {
            return _deviceSourceDAL.GetSourceStatus(descriptor);
        }

        public bool IsResourceAvailable(DeviceResourceDescriptor descriptor)
        {
            return _deviceSourceDAL.IsResourceAvailable(descriptor);
        }

        public ResourceDescriptor CopySourceFromGlobalToLocal(UserIdentity sender, ResourceDescriptor resourceDescriptor,
                                                              string presentationUniqueName)
        {
            return _sourceDAL.CopySourceFromGlobalToLocal(sender, resourceDescriptor, presentationUniqueName);
        }

        public ResourceDescriptor CopySourceFromLocalToGlobal(UserIdentity sender, ResourceDescriptor resourceDescriptor)
        {
            return _sourceDAL.CopySourceFromLocalToGlobal(sender, resourceDescriptor);
        }

        public void CopySourceFromLocalToLocal(UserIdentity userIdentity, string fromUniqueName, string toUniqueName)
        {
            _sourceDAL.CopySourceFromLocalToLocal(userIdentity, fromUniqueName, toUniqueName);
        }

        public Dictionary<string, IList<ResourceDescriptor>> GetGlobalSources()
        {
            return _sourceDAL.GetGlobalSources();
        }

        public Dictionary<string, IList<ResourceDescriptor>> GetLocalSources(string presentationUniqueName)
        {
            return _sourceDAL.GetLocalSources(presentationUniqueName);
        }

        public Dictionary<string, IList<DeviceResourceDescriptor>> GetGlobalDeviceSources()
        {
            return _deviceSourceDAL.GetGlobalSources();
        }

        public PresentationInfo[] GetPresentationWhichContainsSource(ResourceDescriptor resourceDescriptor)
        {
            return _presentationDAL.GetPresentationWhichContainsSource(resourceDescriptor);
        }

        public PresentationInfo[] GetPresentationWhichContainsDeviceSource(DeviceResourceDescriptor resourceDescriptor)
        {
            return _presentationDAL.GetPresentationWhichContainsSource(resourceDescriptor);
        }

        public virtual void SubscribeForMonitor(PresentationKey presentationKey, UserIdentity identity)
        {
        }

        public virtual void UnSubscribeForMonitor(PresentationKey presentationKey, UserIdentity identity)
        {
        }

        public virtual void SubscribeForGlobalMonitoring(UserIdentity identity)
        {
        }

        public virtual void UnSubscribeForGlobalMonitoring(UserIdentity identity)
        {
        }

        public ISourceDAL SourceDAL
        {
            get { return _sourceDAL; }
        }

        public IDeviceSourceDAL DeviceSourceDAL
        {
            get { return _deviceSourceDAL; }
        }

        public event EventHandler<PresentationChangedEventArgs> OnPresentationChanged;
        public event EventHandler<SlideChangedEventArgs> OnSlideChanged;

        #endregion

        #region Implementation of ISourceTransferCRUD

        public virtual FileSaveStatus InitSourceUpload(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor, SourceStatus status, out string newResourceId)
        {
            throw new System.NotImplementedException();
        }

        public virtual FileSaveStatus SaveSource(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor, SourceStatus status, out string newResourceId)
        {
            throw new System.NotImplementedException();
        }

        public virtual void DoneSourceTransfer(UserIdentity userIdentity)
        {
            throw new System.NotImplementedException();
        }

        public virtual ResourceDescriptor InitSourceDownload(UserIdentity identity, ResourceDescriptor resourceDescriptor)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Implementation of IPresentationTransfer

        public virtual FilesGroup GetPresentationForExport(string uniqueName)
        {
            throw new System.NotImplementedException();
        }

        public virtual FilesGroup InitPresentationExport(UserIdentity userIdentity, FilesGroup filesGroup)
        {
            throw new System.NotImplementedException();
        }

        public virtual FilesGroup GetPresentationSchemaFilesForExport()
        {
            throw new System.NotImplementedException();
        }

        public virtual FilesGroup InitPresentationSchemaExport(UserIdentity userIdentity, FilesGroup filesGroup)
        {
            throw new System.NotImplementedException();
        }

        public virtual void DonePresentationTransfer(UserIdentity userIdentity)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}