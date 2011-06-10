using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using TechnicalServices.Communication.Communication.Client;
using TechnicalServices.Configuration.Client;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util.FileTransfer;

namespace Domain.PresentationDesign.Client
{
    public class RemotePresentationClient : IPresentationClient
    {
        private DuplexClient<IPresentationService> _presentationClient;
        private IPresentationNotifier _presentationNotifier;
        private readonly IClientConfiguration _configuration;
        private readonly ISourceDAL _sourceDAL;
        private readonly IDeviceSourceDAL _deviceSourceDAL;
        private readonly DeviceDescriptorCache _deviceDescriptorCache;

        public RemotePresentationClient(IPresentationNotifier presentationNotifier,
            IClientConfiguration clientConfiguration, ISourceDAL sourceDAL, IDeviceSourceDAL deviceSourceDAL)
        {
            _sourceDAL = sourceDAL;
            _deviceSourceDAL = deviceSourceDAL;
            _presentationNotifier = presentationNotifier;
            _configuration = clientConfiguration;
            _presentationClient = new DuplexClient<IPresentationService>(new InstanceContext(presentationNotifier),
                clientConfiguration.PingInterval);
            _presentationClient.Open();
            _presentationClient.OnChanged += new EventHandler<ClientState>(_presentationClient_OnChanged);

            _deviceDescriptorCache = new DeviceDescriptorCache(presentationNotifier);
        }

        #region public

        public event EventHandler<ClientState> OnChanged;

        #endregion

        #region private

        void _presentationClient_OnChanged(object sender, ClientState e)
        {
            if (OnChanged != null)
            {
                OnChanged.Invoke(sender, e);
            }
        }

        private UserIdentity UserIdentity { get { return Thread.CurrentPrincipal as UserIdentity; } }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (_presentationClient != null)
            {
                _presentationClient.Dispose();
                _presentationClient = null;
            }
        }

        #endregion

        #region Implementation of IPresentationClient

        public Presentation LoadPresentation(string fileName, out string[] deletedEquipment)
        {
            throw new System.NotImplementedException();
        }

        public void SaveSlideBulk(string fileName, SlideBulk slideBulk)
        {
            throw new System.NotImplementedException();
        }

        public SlideBulk LoadSlideBulk(string fileName, ResourceDescriptor[] resourceDescriptors, DeviceResourceDescriptor[] deviceResourceDescriptors)
        {
            throw new System.NotImplementedException();
        }

        public void SubscribeForMonitor(PresentationKey presentationKey)
        {
            _presentationClient.Channel.SubscribeForMonitor(presentationKey, UserIdentity);
        }

        public void UnSubscribeForMonitor(PresentationKey presentationKey)
        {
            _presentationClient.Channel.UnSubscribeForMonitor(presentationKey, UserIdentity);
        }

        public void SubscribeForGlobalMonitoring()
        {
            _presentationClient.Channel.SubscribeForGlobalMonitoring(UserIdentity);
        }

        public void UnSubscribeForGlobalMonitoring()
        {
            _presentationClient.Channel.UnSubscribeForGlobalMonitoring(UserIdentity);
        }

        public Slide[] LoadSlides(string uniqueName, int[] slideIds)
        {
            return _presentationClient.Channel.LoadSlides(uniqueName, slideIds);
        }

        public PresentationInfoExt GetPresentationInfo(string uniqueName)
        {
            return _presentationClient.Channel.GetPresentationInfo(uniqueName);
        }

        public PresentationInfoExt GetPresentationInfoByName(string presentationName)
        {
            return _presentationClient.Channel.GetPresentationInfoByName(presentationName);
        }

        public IList<PresentationInfoExt> GetPresentationInfoList()
        {
            return _presentationClient.Channel.GetPresentationInfoList();
        }

        public bool DeletePresentation(string uniqueName)
        {
            return _presentationClient.Channel.DeletePresentation(UserIdentity, uniqueName);
        }

        public SavePresentationResult SavePresentationChanges(PresentationInfo presentationInfo, Slide[] slides, out ResourceDescriptor[] notExistedResources, out DeviceResourceDescriptor[] notExistedDeviceResources, out int[] labelNotExists,
            out UserIdentity[] whoLock, out int[] slidesAlreadyExistsId)
        {
            return _presentationClient.Channel.SavePresentationChanges(UserIdentity, presentationInfo, slides, out notExistedResources, out notExistedDeviceResources, out labelNotExists,
                out whoLock, out slidesAlreadyExistsId);
        }

        public bool SaveSlideChanges(string uniqueName, Slide[] slides, out int[] notLockedSlide, out ResourceDescriptor[] notExistedResources, out DeviceResourceDescriptor[] notExistedDeviceResources, out int[] labelNotExists)
        {
            return _presentationClient.Channel.SaveSlideChanges(UserIdentity, uniqueName, slides, out  notLockedSlide, out notExistedResources, out notExistedDeviceResources, out labelNotExists);
        }

        public bool AcquireLockForSlide(string uniqueName, int slideId, RequireLock requireLock)
        {
            return _presentationClient.Channel.AcquireLockForSlide(UserIdentity, uniqueName, slideId, requireLock);
        }

        public bool ReleaseLockForSlide(string uniqueName, int slideId)
        {
            return _presentationClient.Channel.ReleaseLockForSlide(UserIdentity, uniqueName, slideId);
        }

        public bool AcquireLockForPresentation(string uniqueName, RequireLock requireLock)
        {
            return _presentationClient.Channel.AcquireLockForPresentation(UserIdentity, uniqueName, requireLock);
        }

        public bool ReleaseLockForPresentation(string uniqueName)
        {
            return _presentationClient.Channel.ReleaseLockForPresentation(UserIdentity, uniqueName);
        }

        public LockingInfo[] GetLockingInfo(ObjectKey[] objectKeyArr)
        {
            return _presentationClient.Channel.GetLockingInfo(objectKeyArr);
        }

        public Dictionary<string, IList<ResourceDescriptor>> GetGlobalSources()
        {
            return _presentationClient.Channel.GetGlobalSources();
        }

        public Dictionary<string, IList<ResourceDescriptor>> GetLocalSources(string presentationUniqueName)
        {
            return _presentationClient.Channel.GetLocalSources(presentationUniqueName);
        }

        public Dictionary<string, IList<DeviceResourceDescriptor>> GetGlobalDeviceSources()
        {
            if (!_deviceDescriptorCache.IsInitialized)
            {
                _deviceDescriptorCache.Init(_presentationClient.Channel.GetGlobalDeviceSources());
            }
            return _deviceDescriptorCache.GetResources();
        }

        public bool IsResourceAvailable(ResourceDescriptor descriptor)
        {
            return _presentationClient.Channel.IsResourceAvailable(descriptor);
        }

        public bool IsResourceAvailable(DeviceResourceDescriptor descriptor)
        {
            return _presentationClient.Channel.IsResourceAvailable(descriptor);
        }

        public RemoveResult DeleteSource(ResourceDescriptor descriptor)
        {
            return _presentationClient.Channel.DeleteSource(UserIdentity, descriptor);
        }

        public ResourceDescriptor CopySourceFromLocalToGlobal(ResourceDescriptor descriptor)
        {
            return _presentationClient.Channel.CopySourceFromLocalToGlobal(UserIdentity, descriptor);
        }

        public ResourceDescriptor CopySourceFromGlobalToLocal(ResourceDescriptor descriptor, string uniqueName)
        {
            return _presentationClient.Channel.CopySourceFromGlobalToLocal(UserIdentity, descriptor, uniqueName);
        }

        public void CopySourceFromLocalToLocal(string fromUniqueName, string toUniqueName)
        {
            _presentationClient.Channel.CopySourceFromLocalToLocal(UserIdentity, fromUniqueName, toUniqueName);
        }

        public CreatePresentationResult CreatePresentation(PresentationInfo presentationInfo, out int[] labelNotExists)
        {
            return _presentationClient.Channel.CreatePresentation(UserIdentity, presentationInfo, out labelNotExists);
        }

        public FilesGroup GetPresentationSchemaFilesForExport()
        {
            return _presentationClient.Channel.GetPresentationSchemaFilesForExport();
        }

        public FilesGroup GetPresentationForExport(string uniqueName)
        {
            return _presentationClient.Channel.GetPresentationForExport(uniqueName);
        }

        public PresentationStatus GetPresentationStatus(string uniqueName, out UserIdentity identity)
        {
            return _presentationClient.Channel.GetPresentationStatus(uniqueName, out identity);
        }

        public IClientResourceCRUD<ResourceDescriptor> GetResourceCrud()
        {
            return new ClientSideSourceTransfer(_presentationClient.Channel, _sourceDAL);
        }

        public IClientResourceCRUD<FilesGroup> GetPresentationSchemaCrud()
        {
            return new ClientSidePresentationSchemaTransfer(_configuration.ScenarioFolder, _presentationClient.Channel);
        }

        //public IClientResourceCRUD<DeviceResourceDescriptor> GetDeviceResourceCrud()
        //{
        //    return new ClientSideDeviceSourceTransfer(_presentationClient.Channel, _deviceSourceDAL);
        //}

        public FileSaveStatus SaveDeviceSource(DeviceResourceDescriptor descriptor)
        {
            string newResourceId;
            FileSaveStatus saveStatus = _presentationClient.Channel.SaveDeviceSource(UserIdentity, descriptor, SourceStatus.Update);
            if (saveStatus == FileSaveStatus.Ok)
                _deviceDescriptorCache.AddResource(descriptor);
            return saveStatus;
        }

        public IClientResourceCRUD<FilesGroup> GetPresentationExportCrud()
        {
            return new ClientSidePresentationTransfer(_configuration.ScenarioFolder, _presentationClient.Channel);
        }

        public ISourceDAL SourceDAL
        {
            get { return _sourceDAL; }
        }

        public IDeviceSourceDAL DeviceSourceDAL
        {
            get { return _deviceSourceDAL; }
        }

        #endregion
    }
}
