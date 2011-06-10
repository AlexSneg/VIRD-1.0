using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DomainServices.PresentationManagement;
using TechnicalServices.Common;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Locking.Locking;
using TechnicalServices.Persistence.CommonPresentation;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util;
using TechnicalServices.Util.FileTransfer;

namespace Domain.PresentationDesign.Client
{
    #region старый код
    //напишем заново, так будет лучше.
    //public class StandAlonePresentationWorker: IPresentationWorker
    //{
    //    List<Presentation> prList = new List<Presentation>();
    //    private readonly ISourceDAL _sourceDAL;
    //    private readonly IPresentationDAL _presentationDAL;

    //    public StandAlonePresentationWorker(ISourceDAL sourceDAL, IConfiguration configuration)
    //    {
    //        _sourceDAL = sourceDAL;
    //        _presentationDAL = new PresentationDAL(configuration);
    //        _presentationDAL.Init(_sourceDAL);
    //        //for (int i = 0; i < 20; ++i)
    //        //    prList.Add(Util.CreateTestPresentation("Сценарий" + i));
    //    }

    //    #region IPresentationWorker Members

    //    public IList<TechnicalServices.Persistence.SystemPersistence.Presentation.PresentationInfoExt> GetPresentationInfoList()
    //    {
    //        //#region Create test presentaions
    //        //List<PresentationInfoExt> result = new List<PresentationInfoExt>();
    //        //foreach(Presentation p in prList)
    //        //    result.Add(new PresentationInfoExt(new PresentationInfo(p), null));

    //        //return result;
    //        //#endregion
    //        return new List<PresentationInfoExt>();
    //    }

    //    public PresentationInfoExt GetPresentationInfo(string uniqueName)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public byte[] GetPresentation(string uniqueName)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public Slide[] LoadSlides(string presentationUniqueName, int[] slideIdArr)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public bool CreatePresentation(PresentationInfo presentationInfo)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public bool SavePresentationChanges(UserIdentity userIdentity, PresentationInfo presentationInfo, Slide[] newSlideArr, out ResourceDescriptor[] resourcesNotExists)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public bool SaveSlideChanges(TechnicalServices.Entity.UserIdentity userIdentity,
    //        string presentationUniqueName, Slide[] slideToSave,
    //        out int[] slideIdNotLocked,
    //        out ResourceDescriptor[] resourcesNotExists)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool DeletePresentation(string uniqueName)
    //    {
    //        return true;
    //    }

    //    public TechnicalServices.Entity.PresentationStatus GetPresentationStatus(string uniqueName, out TechnicalServices.Entity.UserIdentity userIdentity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool AcquireLockForPresentation(TechnicalServices.Entity.UserIdentity userIdentity, string uniqueName, TechnicalServices.Entity.RequireLock requireLock)
    //    {
    //        return true;
    //    }

    //    public bool ReleaseLockForPresentation(TechnicalServices.Entity.UserIdentity userIdentity, string uniqueName)
    //    {
    //        return true;
    //    }

    //    public bool AcquireLockForSlide(TechnicalServices.Entity.UserIdentity userIdentity, string presentationUniqueName, int slideId, TechnicalServices.Entity.RequireLock requireLock)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool ReleaseLockForSlide(TechnicalServices.Entity.UserIdentity userIdentity, string presentationUniqueName, int slideId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public TechnicalServices.Entity.LockingInfo[] GetLockingInfo(TechnicalServices.Entity.ObjectKey[] objectKeyArr)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public TechnicalServices.Entity.LockingInfo[] GetLockingInfo(string uniqueName)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool DeleteSource(TechnicalServices.Persistence.SystemPersistence.Presentation.ResourceDescriptor descriptor)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public TechnicalServices.Entity.FileSaveStatus GetSourceStatus(TechnicalServices.Persistence.SystemPersistence.Presentation.ResourceDescriptor descriptor)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //public TechnicalServices.Entity.FileSaveStatus CreateSource(TechnicalServices.Common.FileTransferObject obj)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}

    //    //public void SaveSource(FileTransferObject obj)
    //    //{
    //    //    throw new System.NotImplementedException();
    //    //}

    //    //public TechnicalServices.Common.FileTransferObject? GetSource(TechnicalServices.Persistence.SystemPersistence.Presentation.ResourceDescriptor resourceDescriptor)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}

    //    public Dictionary<string, IList<ResourceDescriptor>> GetGlobalSources()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    //public TechnicalServices.Persistence.SystemPersistence.Presentation.ResourceDescriptor[] GetGlobalSources()
    //    //{
    //    //    throw new NotImplementedException();
    //    //}

    //    public Dictionary<string, IList<ResourceDescriptor>> GetLocalSources(TechnicalServices.Persistence.SystemPersistence.Presentation.PresentationInfo info)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //public TechnicalServices.Persistence.SystemPersistence.Presentation.ResourceDescriptor[] GetLocalSources(TechnicalServices.Persistence.SystemPersistence.Presentation.PresentationInfo info)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}

    //    public TechnicalServices.Persistence.SystemPersistence.Presentation.PresentationInfo[] GetPresentationWhichContainsSource(TechnicalServices.Persistence.SystemPersistence.Presentation.ResourceDescriptor resourceDescriptor)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void SubscribeForMonitor(TechnicalServices.Entity.PresentationKey presentationKey, TechnicalServices.Entity.UserIdentity identity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void UnSubscribeForMonitor(TechnicalServices.Entity.PresentationKey presentationKey, TechnicalServices.Entity.UserIdentity identity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void SubscribeForGlobalMonitoring(TechnicalServices.Entity.UserIdentity identity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void UnSubscribeForGlobalMonitoring(TechnicalServices.Entity.UserIdentity identity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    #endregion

    //    #region IPing Members

    //    public void Ping(TechnicalServices.Entity.UserIdentity identity)
    //    {
    //    }

    //    #endregion

    //    #region Implementation of IResourceRemoteCRUD

    //    public FileSaveStatus Send(UserIdentity userIdentity, FileTransferObject obj, SourceStatus status)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public FileTransferObject? Receive(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    #endregion
    //}

    //public class Util
    //{
    //    private static int maxSlides = 10;
    //    private static int maxSources = 20;
    //    private static int maxDisplays = 10;
    //    private static int maxDevices = 10;
    //    private static string[] images = new string[]
    //        {@"images\test1.png",
    //        @"images\test2.png",
    //        @"images\test3.png",
    //        @"images\test4.png"};

    //    public static Presentation CreateTestPresentation(string pname)
    //    {
    //        Presentation pres = new Presentation { Name = pname, CreationDate = DateTime.Now, LastChangeDate = DateTime.Now };

    //        pres.SlideList.AddRange(CreateSlideList());
    //        pres.StartSlide = pres.SlideList[0];

    //        return pres;
    //    }

    //    private static List<Slide> CreateSlideList()
    //    {
    //        List<Slide> slideArr = new List<Slide>();
    //        for (int i = 0; i < maxSlides; i++)
    //        {
    //            Slide slide = new Slide()
    //            {
    //                Id = i,
    //                Label = String.Format("Label{0}", i),
    //                Name = String.Format("Слайд{0}", i + 1),
    //                Comment = "Тестовый слайд",
    //                Time = new TimeSpan(DateTime.Today.Ticks),
    //                ModifiedUtc = DateTime.Now
    //            };
    //            slide.SourceList.AddRange(CreateSourceList());
    //            slide.DisplayList.AddRange(CreateDisplayList(slide.SourceList));
    //            slide.DeviceList.AddRange(CreateDeviceList());
    //            slideArr.Add(slide);
    //        }

    //        //for (int i = 0; i < maxSlides; i++)
    //        //{
    //        //    if (i < maxSlides - 1)
    //        //    {
    //        //        slideArr[i].LinkList.Add(new Link()
    //        //        {
    //        //            IsDefault = true,
    //        //            NextSlide = slideArr[i + 1]
    //        //        });
    //        //    }
    //        //}

    //        return slideArr;
    //    }

    //    private static List<Device> CreateDeviceList()
    //    {
    //        List<Device> deviceList = new List<Device>();
    //        return deviceList;
    //    }

    //    private static IEnumerable<Display> CreateDisplayList(List<Source> sourceArr)
    //    {
    //        List<Display> displayArr = new List<Display>();
    //        return displayArr;
    //    }

    //    private static List<Source> CreateSourceList()
    //    {
    //        List<Source> sourceArr = new List<Source>();
    //        return sourceArr;
    //    }


    //}
    #endregion


    public class StandAlonePresentationWorker : BasePresentationWorker, IPresentationClient
    {
        private readonly IPresentationNotifier _notifier;

        public StandAlonePresentationWorker(ISourceDAL sourceDAL, IDeviceSourceDAL deviceSourceDAL,
            IConfiguration configuration, IPresentationNotifier notifier)
        {
            _configuration = configuration;
            _sourceDAL = sourceDAL;
            _deviceSourceDAL = deviceSourceDAL;
            _lockService = new LockingService(new TechnicalServices.Entity.UserIdentity(UserInfo.Empty, true, TimeSpan.MinValue, "localhost"), _configuration.EventLog);
            _presentationDAL = new StandAlonePresentationDAL(configuration);    //PresentationDAL
            _presentationDAL.Init(_sourceDAL, _deviceSourceDAL);
            _notifier = notifier;
            Init();
        }

        private UserIdentity UserIdentity { get { return Thread.CurrentPrincipal as UserIdentity; } }

        #region Overrides of BasePresentationWorker

        protected override bool IsStandAlone
        {
            get { return true;}
        }

        protected override void LabelStorageAdapter_OnUpdate(object sender, LabelEventArg e)
        {
            if (_notifier != null)
                _notifier.LabelUpdated(e.Label);
        }

        protected override void LabelStorageAdapter_OnDelete(object sender, LabelEventArg e)
        {
            if (_notifier != null)
                _notifier.LabelDeleted(e.Label);
        }

        protected override void LabelStorageAdapter_OnAdd(object sender, LabelEventArg e)
        {
            if (_notifier != null)
                _notifier.LabelAdded(e.Label);
        }

        protected override void _presentationDAL_OnPresentationDeleted(object sender, PresentationEventArg e)
        {
            if (_notifier != null)
                _notifier.PresentationDeleted(e.UserIdentity, e.PresentationInfo);
        }

        protected override void _presentationDAL_OnPresentationAdded(object sender, PresentationEventArg e)
        {
            if (_notifier != null)
                _notifier.PresentationAdded(e.UserIdentity, e.PresentationInfo);
        }

        protected override void _sourceDAL_OnResourceDeleted(object sender, SourceEventArg<ResourceDescriptor> e)
        {
            if (_notifier != null)
                _notifier.ResourceDeleted(e.UserIdentity, e.ResourceDescriptor);
        }

        protected override void _sourceDAL_OnResourceAdded(object sender, SourceEventArg<ResourceDescriptor> e)
        {
            if (_notifier != null)
                _notifier.ResourceAdded(e.UserIdentity, e.ResourceDescriptor);
        }

        protected override void _sourceDAL_OnResourceUpdated(object sender, SourceEventArg<ResourceDescriptor> e)
        {
            if (_notifier != null)
                _notifier.ResourceUpdated(e.UserIdentity, e.ResourceDescriptor);
        }

        protected override void _deviceResourceDAL_OnResourceAdded(object sender, SourceEventArg<DeviceResourceDescriptor> e)
        {
            if (_notifier != null)
                _notifier.DeviceResourceAdded(e.UserIdentity, e.ResourceDescriptor);
        }

        protected override void _deviceResourceDAL_OnResourceDeleted(object sender, SourceEventArg<DeviceResourceDescriptor> e)
        {
            if (_notifier != null)
                _notifier.DeviceResourceDeleted(e.UserIdentity, e.ResourceDescriptor);
        }

        protected override void _deviceSourceDAL_OnResourceUpdated(object sender, SourceEventArg<DeviceResourceDescriptor> e)
        {
            if (_notifier != null)
                _notifier.DeviceResourceUpdated(e.UserIdentity, e.ResourceDescriptor);
        }

        protected override void _lockService_RemoveItem(UserIdentity sender, ObjectKey key, LockingInfo value)
        {
            if (_notifier != null)
                _notifier.ObjectUnLocked(value);
        }

        protected override void _lockService_AddItem(UserIdentity sender, ObjectKey key, LockingInfo value)
        {
            if (_notifier != null)
                _notifier.ObjectLocked(value);
        }

        protected override void ObjectChanged(UserIdentity sender, IList<ObjectInfo> objectInfoList, PresentationInfo presentationInfo, bool presentationLevel)
        {
            base.ObjectChanged(sender, objectInfoList, presentationInfo, presentationLevel);
            if (_notifier != null)
                _notifier.ObjectChanged(sender, objectInfoList);
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
        }

        #endregion

        #region Implementation of IPresentationClient

        public Presentation LoadPresentation(string fileName, out string[] deletedEquipment)
        {
            return _presentationDAL.LoadPresentation(fileName, _sourceDAL, _deviceSourceDAL, out deletedEquipment);
        }

        public void SaveSlideBulk(string fileName, SlideBulk slideBulk)
        {
            _presentationDAL.SaveSlideBulk(fileName, slideBulk);
        }

        public SlideBulk LoadSlideBulk(string fileName, ResourceDescriptor[] resourceDescriptors, DeviceResourceDescriptor[] deviceResourceDescriptors)
        {
            return _presentationDAL.LoadSlideBulk(fileName, resourceDescriptors, deviceResourceDescriptors);
        }

        public void SubscribeForMonitor(PresentationKey presentationKey)
        {
        }

        public void UnSubscribeForMonitor(PresentationKey presentationKey)
        {
        }

        public void SubscribeForGlobalMonitoring()
        {
        }

        public void UnSubscribeForGlobalMonitoring()
        {
        }

        public bool DeletePresentation(string uniqueName)
        {
            return base.DeletePresentation(UserIdentity, uniqueName);
        }

        public SavePresentationResult SavePresentationChanges(PresentationInfo presentationInfo, Slide[] slides, out ResourceDescriptor[] notExistedResources, out DeviceResourceDescriptor[] notExistedDeviceResources, out int[] labelNotExists,
            out UserIdentity[] whoLock,
            out int[] slidesAlreadyExistsId)
        {
            return base.SavePresentationChanges(UserIdentity, presentationInfo, slides, out notExistedResources, out notExistedDeviceResources, out labelNotExists,
                out whoLock, out slidesAlreadyExistsId);
        }

        public bool SaveSlideChanges(string uniqueName, Slide[] slides, out int[] notLockedSlide, out ResourceDescriptor[] notExistedResources, out DeviceResourceDescriptor[] notExistedDeviceResources, out int[] labelNotExists)
        {
            return base.SaveSlideChanges(UserIdentity, uniqueName, slides, out notLockedSlide, out notExistedResources, out notExistedDeviceResources, out labelNotExists);
        }

        public bool AcquireLockForSlide(string uniqueName, int slideId, RequireLock requireLock)
        {
            return true;
            //return base.AcquireLockForSlide(UserIdentity, uniqueName, slideId, requireLock);
        }

        public bool ReleaseLockForSlide(string uniqueName, int slideId)
        {
            return true;
            //return base.ReleaseLockForSlide(UserIdentity, uniqueName, slideId);
        }

        public bool AcquireLockForPresentation(string uniqueName, RequireLock requireLock)
        {
            return true;
            //return base.AcquireLockForPresentation(UserIdentity, uniqueName, requireLock);
        }

        public bool ReleaseLockForPresentation(string uniqueName)
        {
            return true;
            //return base.ReleaseLockForPresentation(UserIdentity, uniqueName);
        }

        public RemoveResult DeleteSource(ResourceDescriptor descriptor)
        {
            return base.DeleteSource(UserIdentity, descriptor);
        }

        public ResourceDescriptor CopySourceFromLocalToGlobal(ResourceDescriptor descriptor)
        {
            return base.CopySourceFromLocalToGlobal(UserIdentity, descriptor);
        }

        public ResourceDescriptor CopySourceFromGlobalToLocal(ResourceDescriptor descriptor, string uniqueName)
        {
            return base.CopySourceFromGlobalToLocal(UserIdentity, descriptor, uniqueName);
        }

        public void CopySourceFromLocalToLocal(string fromUniqueName, string toUniqueName)
        {
            base.CopySourceFromLocalToLocal(UserIdentity, fromUniqueName, toUniqueName);
        }

        public FileSaveStatus SaveDeviceSource(DeviceResourceDescriptor descriptor)
        {
            string newResourceId;
            return base.SaveDeviceSource(UserIdentity, descriptor, SourceStatus.Update, out newResourceId);
        }

        public CreatePresentationResult CreatePresentation(PresentationInfo presentationInfo, out int[] labelNotExists)
        {
            return base.CreatePresentation(UserIdentity, presentationInfo, out labelNotExists);
        }

        public IClientResourceCRUD<ResourceDescriptor> GetResourceCrud()
        {
            return new ClientSideStandAloneSourceTransfer(_sourceDAL);
        }

        public IClientResourceCRUD<FilesGroup> GetPresentationSchemaCrud()
        {
            throw new System.NotImplementedException();
        }

        public IClientResourceCRUD<FilesGroup> GetPresentationExportCrud()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

}
