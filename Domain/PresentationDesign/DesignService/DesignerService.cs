using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;

using Domain.Administration.AdministrationCommon;
using Domain.PresentationDesign.DesignCommon;

using DomainServices.PresentationManagement;

using TechnicalServices.Common;
using TechnicalServices.Common.Utils;
using TechnicalServices.Configuration.Server;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Licensing;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Domain.PresentationDesign.DesignService
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Single,
        IncludeExceptionDetailInFaults = true)]
    public class DesignerService : IDesignerService, ILoginService, IPresentationService, IDisposable
    {
        private readonly IAdministrationService _administrationService;
        private readonly IServerConfiguration _config;
        private readonly ConfigurationExportHelper _configFileExportHelper;
        private readonly ILoginService _loginService;
        private readonly PresentationWorker _presentationWorker;


        public DesignerService(IServerConfiguration config, PresentationWorker worker, ILoginService loginService,
                               IAdministrationService administrationService)
        {
            Debug.Assert(config != null, "IServerConfiguration не может быть null");
            Debug.Assert(worker != null, "IPresentationWorker не может быть null");
            Debug.Assert(loginService != null, "ILoginService не может быть null");
            Debug.Assert(administrationService != null, "IAdministrationService не может быть null");

            _config = config;
            _loginService = loginService;
            _presentationWorker = worker;
            _administrationService = administrationService;
            _configFileExportHelper = new ConfigurationExportHelper(config);
        }

        #region IDesignerService

        public ISystemParameters GetSystemParameters()
        {
            return _administrationService.LoadSystemParameters();
        }

        public ModuleConfiguration GetModuleConfiguration()
        {
            Label[] labels = _config.LabelStorageAdapter.GetLabelStorage();
            ModuleConfiguration moduleConfiguration = _config.ModuleConfiguration;
            moduleConfiguration.LabelList.Clear();
            moduleConfiguration.LabelList.AddRange(labels);
            return moduleConfiguration;
        }

        //public LabelStorage GetLabelStorage()
        //{
        //    return _config.ModuleConfiguration.LabelStorage;
        //}

        public ICollection<string> CheckModuleConfiguration()
        {
            List<string> list = new List<string>();
            foreach (IModule module in _config.ModuleList)
                list.Add(module.GetType().Assembly.FullName);
            return list;
        }

        public bool CheckVersion(string version)
        {
            var v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return (v.ToString() == version.ToString());
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region Implementation of ISourceTransferCRUD

        public FileSaveStatus InitSourceUpload(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor,
                                               SourceStatus status, out string otherResourceId)
        {
            return _presentationWorker.InitSourceUpload(userIdentity, resourceDescriptor, status, out otherResourceId);
        }

        public FileSaveStatus SaveSource(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor,
                                         SourceStatus status, out string otherResourceId)
        {
            return _presentationWorker.SaveSource(userIdentity, resourceDescriptor, status, out otherResourceId);
        }

        public void DoneSourceTransfer(UserIdentity userIdentity)
        {
            _presentationWorker.DoneSourceTransfer(userIdentity);
        }

        public ResourceDescriptor InitSourceDownload(UserIdentity identity, ResourceDescriptor resourceDescriptor)
        {
            return _presentationWorker.InitSourceDownload(identity, resourceDescriptor);
        }
        public int ForwardMoveNeeded()
        {
            return _presentationWorker.ForwardMoveNeeded();
        }

        public double GetCurrentSpeed()
        {
            return _presentationWorker.GetCurrentSpeed();
        }

        public string GetCurrentFile()
        {
            return _presentationWorker.GetCurrentFile();
        }

        #endregion

        #region Implementation of IConfigurationTransfer

        public FilesGroup GetConfigFilesForExport()
        {
            return _configFileExportHelper.GetConfigFilesForExport();
        }

        public FilesGroup InitConfigurationExport(UserIdentity userIdentity, FilesGroup filesGroup)
        {
            return _configFileExportHelper.InitConfigurationExport(userIdentity, filesGroup);
        }

        public void DoneConfigurationExport(UserIdentity userIdentity)
        {
            _configFileExportHelper.DoneConfigurationExport(userIdentity);
        }

        #endregion

        #region Implementation of IPresentationTransfer

        public FilesGroup GetPresentationForExport(string uniqueName)
        {
            return _presentationWorker.GetPresentationForExport(uniqueName);
        }

        public FilesGroup InitPresentationExport(UserIdentity userIdentity, FilesGroup filesGroup)
        {
            return _presentationWorker.InitPresentationExport(userIdentity, filesGroup);
        }

        public FilesGroup GetPresentationSchemaFilesForExport()
        {
            return _presentationWorker.GetPresentationSchemaFilesForExport();
        }

        public FilesGroup InitPresentationSchemaExport(UserIdentity userIdentity, FilesGroup filesGroup)
        {
            return _presentationWorker.InitPresentationSchemaExport(userIdentity, filesGroup);
        }

        public void DonePresentationTransfer(UserIdentity userIdentity)
        {
            _presentationWorker.DonePresentationTransfer(userIdentity);
        }

        #endregion

        private static void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckHasp();
        }

        #region Implementation of ILoginService

        public UserIdentity Login(string name, byte[] hash, string hostName)
        {
            WatchDog.WatchDogAction(_config.EventLog.WriteError, CheckLicense);
            return _loginService.Login(name, hash, hostName);
        }

        public void Logoff(UserIdentity user)
        {
            _loginService.Logoff(user);
        }

        public IList<UserIdentity> GetUserLoginCollection()
        {
            return _loginService.GetUserLoginCollection();
        }

        void ILoginService.Subscribe(UserIdentity user)
        {
            _loginService.Subscribe(user);
        }

        void ILoginService.UnSubscribe(UserIdentity user)
        {
            _loginService.UnSubscribe(user);
        }

        #endregion

        #region Implementation of IPresentationWorker

        public IList<PresentationInfoExt> GetPresentationInfoList()
        {
            return _presentationWorker.GetPresentationInfoList();
        }

        public PresentationInfoExt GetPresentationInfo(string uniqueName)
        {
            return _presentationWorker.GetPresentationInfo(uniqueName);
        }

        public PresentationInfoExt GetPresentationInfoByName(string presentationName)
        {
            return _presentationWorker.GetPresentationInfoByName(presentationName);
        }

        public byte[] GetPresentation(string uniqueName)
        {
            return _presentationWorker.GetPresentation(uniqueName);
        }

        //public byte[] GetPresentationForEdit(UserIdentity userIdentity, string uniqueName)
        //{
        //    return _presentationWorker.GetPresentationForEdit(userIdentity, uniqueName);
        //}

        public Slide[] LoadSlides(string presentationUniqueName, int[] slideIdArr)
        {
            return _presentationWorker.LoadSlides(presentationUniqueName, slideIdArr);
        }

        //public bool CreatePresentation(byte[] presentation)
        //{
        //    return _presentationWorker.CreatePresentation(presentation);
        //}

        public CreatePresentationResult CreatePresentation(UserIdentity sender,
                                                           PresentationInfo presentationInfo, out int[] labelNotExists)
        {
            return _presentationWorker.CreatePresentation(sender, presentationInfo, out labelNotExists);
        }

        //public bool SavePresentationChanges(UserIdentity userIdentity, byte[] presentation,
        //                                    out ResourceDescriptor[] resourcesNotExists)
        //{
        //    return _presentationWorker.SavePresentationChanges(userIdentity, presentation, out resourcesNotExists);
        //}

        public SavePresentationResult SavePresentationChanges(UserIdentity userIdentity,
                                                              PresentationInfo presentationInfo, Slide[] newSlideArr,
                                                              out ResourceDescriptor[] resourcesNotExists,
                                                              out DeviceResourceDescriptor[] deviceResourcesNotExists,
                                                              out int[] labelNotExists,
                                                              out UserIdentity[] whoLock,
                                                              out int[] slidesAlreadyExistsId)
        {
            return _presentationWorker.SavePresentationChanges(userIdentity, presentationInfo,
                                                               newSlideArr, out resourcesNotExists,
                                                               out deviceResourcesNotExists, out labelNotExists,
                                                               out whoLock, out slidesAlreadyExistsId);
        }

        public bool SaveSlideChanges(UserIdentity userIdentity, string presentationUniqueName, Slide[] slideToSave,
                                     out int[] slideIdNotLocked, out ResourceDescriptor[] resourcesNotExists,
                                     out DeviceResourceDescriptor[] deviceResourcesNotExists, out int[] labelNotExists)
        {
            return _presentationWorker.SaveSlideChanges(userIdentity,
                                                        presentationUniqueName,
                                                        slideToSave,
                                                        out slideIdNotLocked,
                                                        out resourcesNotExists, out deviceResourcesNotExists,
                                                        out labelNotExists);
        }

        public bool DeletePresentation(UserIdentity sender, string uniqueName)
        {
            return _presentationWorker.DeletePresentation(sender, uniqueName);
        }

        public PresentationStatus GetPresentationStatus(string uniqueName, out UserIdentity userIdentity)
        {
            return _presentationWorker.GetPresentationStatus(uniqueName, out userIdentity);
        }

        public bool AcquireLockForPresentation(UserIdentity userIdentity, string uniqueName, RequireLock requireLock)
        {
            return _presentationWorker.AcquireLockForPresentation(OperationContext.Current.Channel, userIdentity,
                                                                  uniqueName, requireLock);
        }

        public bool ReleaseLockForPresentation(UserIdentity userIdentity, string uniqueName)
        {
            return _presentationWorker.ReleaseLockForPresentation(userIdentity, uniqueName);
        }

        public bool AcquireLockForSlide(UserIdentity userIdentity, string presentationUniqueName, int slideId,
                                        RequireLock requireLock)
        {
            return _presentationWorker.AcquireLockForSlide(OperationContext.Current.Channel, userIdentity,
                                                           presentationUniqueName, slideId, requireLock);
        }

        public bool ReleaseLockForSlide(UserIdentity userIdentity, string presentationUniqueName, int slideId)
        {
            return _presentationWorker.ReleaseLockForSlide(userIdentity, presentationUniqueName, slideId);
        }

        public LockingInfo[] GetLockingInfo(ObjectKey[] objectKeyArr)
        {
            return _presentationWorker.GetLockingInfo(objectKeyArr);
        }

        public LockingInfo[] GetLockingInfo(string uniqueName)
        {
            return _presentationWorker.GetLockingInfo(uniqueName);
        }

        public RemoveResult DeleteSource(UserIdentity sender, ResourceDescriptor descriptor)
        {
            return _presentationWorker.DeleteSource(sender, descriptor);
        }

        public RemoveResult DeleteDeviceSource(UserIdentity sender, DeviceResourceDescriptor descriptor)
        {
            return _presentationWorker.DeleteDeviceSource(sender, descriptor);
        }

        public FileSaveStatus SaveDeviceSource(UserIdentity userIdentity, DeviceResourceDescriptor resourceDescriptor, SourceStatus status)
        {
            string otherResourceId;
            return _presentationWorker.SaveDeviceSource(userIdentity, resourceDescriptor, status, out otherResourceId);
        }

        public FileSaveStatus GetSourceStatus(ResourceDescriptor descriptor)
        {
            return _presentationWorker.GetSourceStatus(descriptor);
        }

        public bool IsResourceAvailable(ResourceDescriptor descriptor)
        {
            return _presentationWorker.IsResourceAvailable(descriptor);
        }

        public FileSaveStatus GetDeviceSourceStatus(DeviceResourceDescriptor descriptor)
        {
            return _presentationWorker.GetDeviceSourceStatus(descriptor);
        }

        public bool IsResourceAvailable(DeviceResourceDescriptor descriptor)
        {
            return _presentationWorker.IsResourceAvailable(descriptor);
        }

        public ResourceDescriptor CopySourceFromGlobalToLocal(UserIdentity sender, ResourceDescriptor resourceDescriptor,
                                                              string presentationUniqueName)
        {
            return _presentationWorker.CopySourceFromGlobalToLocal(sender, resourceDescriptor, presentationUniqueName);
        }

        public ResourceDescriptor CopySourceFromLocalToGlobal(UserIdentity sender, ResourceDescriptor resourceDescriptor)
        {
            return _presentationWorker.CopySourceFromLocalToGlobal(sender, resourceDescriptor);
        }

        public void CopySourceFromLocalToLocal(UserIdentity userIdentity, string fromUniqueName, string toUniqueName)
        {
            _presentationWorker.CopySourceFromLocalToLocal(userIdentity, fromUniqueName, toUniqueName);
        }

        //public FileSaveStatus CreateSource(FileTransferObject obj)
        //{
        //    return _presentationWorker.CreateSource(obj);
        //}

        //public void SaveSource(FileTransferObject obj)
        //{
        //    _presentationWorker.SaveSource(obj);
        //}

        //public FileTransferObject? GetSource(ResourceDescriptor resourceDescriptor)
        //{
        //    return _presentationWorker.GetSource(resourceDescriptor);
        //}

        public Dictionary<string, IList<ResourceDescriptor>> GetGlobalSources()
        {
            return _presentationWorker.GetGlobalSources();
        }

        public Dictionary<string, IList<ResourceDescriptor>> GetLocalSources(string presentationUniqueName)
        {
            return _presentationWorker.GetLocalSources(presentationUniqueName);
        }

        public Dictionary<string, IList<DeviceResourceDescriptor>> GetGlobalDeviceSources()
        {
            return _presentationWorker.GetGlobalDeviceSources();
        }

        public PresentationInfo[] GetPresentationWhichContainsSource(ResourceDescriptor resourceDescriptor)
        {
            return _presentationWorker.GetPresentationWhichContainsSource(resourceDescriptor);
        }

        public PresentationInfo[] GetPresentationWhichContainsDeviceSource(DeviceResourceDescriptor resourceDescriptor)
        {
            return _presentationWorker.GetPresentationWhichContainsDeviceSource(resourceDescriptor);
        }

        public void SubscribeForMonitor(PresentationKey presentationKey, UserIdentity identity)
        {
            _presentationWorker.SubscribeForMonitor(presentationKey, identity);
        }

        public void UnSubscribeForMonitor(PresentationKey presentationKey, UserIdentity identity)
        {
            _presentationWorker.UnSubscribeForMonitor(presentationKey, identity);
        }

        public void SubscribeForGlobalMonitoring(UserIdentity identity)
        {
            _presentationWorker.SubscribeForGlobalMonitoring(identity);
        }

        public void UnSubscribeForGlobalMonitoring(UserIdentity identity)
        {
            _presentationWorker.UnSubscribeForGlobalMonitoring(identity);
        }

        public PresentationInfo[] GetPresentationWhichContainsLabel(int labelId)
        {
            return _presentationWorker.GetPresentationWhichContainsLabel(labelId);
        }

        #endregion

        #region Implementation of IPing

        public void Ping(UserIdentity identity)
        {
            if (OperationContext.Current.Channel is ILoginNotifier)
                _loginService.Ping(identity);
            if (OperationContext.Current.Channel is IPresentationNotifier)
                _presentationWorker.Ping(identity);
        }

        #endregion

        #region Implementation of IFileTransfer

        public FileSaveStatus Send(UserIdentity userIdentity, FileTransferObject obj)
        {
            if (_presentationWorker.IsUserParticipateInResourceTransfer(userIdentity))
                return _presentationWorker.Send(userIdentity, obj);
            if (_configFileExportHelper.IsUserParticipateInResourceTransfer(userIdentity))
                return _configFileExportHelper.Send(userIdentity, obj);
            return FileSaveStatus.Abort;
        }

        public FileTransferObject? Receive(UserIdentity userIdentity, string resourceId)
        {
            if (_presentationWorker.IsUserParticipateInResourceTransfer(userIdentity))
                return _presentationWorker.Receive(userIdentity, resourceId);
            if (_configFileExportHelper.IsUserParticipateInResourceTransfer(userIdentity))
                return _configFileExportHelper.Receive(userIdentity, resourceId);
            return null;
        }

        public void Terminate(UserIdentity userIdentity)
        {
            if (_presentationWorker.IsUserParticipateInResourceTransfer(userIdentity))
                _presentationWorker.Terminate(userIdentity);
            else if (_configFileExportHelper.IsUserParticipateInResourceTransfer(userIdentity))
                _configFileExportHelper.Terminate(userIdentity);
        }

        #endregion
    }
}