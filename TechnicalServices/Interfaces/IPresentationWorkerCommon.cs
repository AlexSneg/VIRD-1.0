using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Interfaces
{
    public class PresentationChangedEventArgs : EventArgs
    {
        private readonly PresentationInfo _info;
        public PresentationChangedEventArgs(PresentationInfo info)
        {
            _info = info;
        }

        public PresentationInfo PresentationInfo
        {
            get { return _info; }
        }
    }

    public class SlideChangedEventArgs : EventArgs
    {
        private readonly string _uniquePresentationName;
        private readonly List<int> _slideIds;

        public SlideChangedEventArgs(string uniquePresentationName, IEnumerable<int> slideIds)
        {
            _uniquePresentationName = uniquePresentationName;
            _slideIds = new List<int>(slideIds);
        }

        public string UniquePresentationName
        {
            get { return _uniquePresentationName; }
        }

        public int[] SlideIds
        {
            get { return _slideIds.ToArray(); }
        }
    }

    public interface IPresentationWorkerCommon
    {
        PresentationInfo[] GetPresentationWhichContainsLabel(int labelId);
        IList<PresentationInfoExt> GetPresentationInfoList();
        PresentationInfoExt GetPresentationInfo(string uniqueName);
        byte[] GetPresentation(string uniqueName);
        Slide[] LoadSlides(string presentationUniqueName, int[] slideIdArr);
        CreatePresentationResult CreatePresentation(UserIdentity sender,
                                                    PresentationInfo presentationInfo,
                                                    out int[] labelNotExists);

        SavePresentationResult SavePresentationChanges(UserIdentity userIdentity, PresentationInfo presentationInfo,
                                     Slide[] newSlideArr, out ResourceDescriptor[] resourcesNotExists,
                                     out DeviceResourceDescriptor[] deviceResourcesNotExists, out int[] labelNotExists,
                                     out UserIdentity[] whoLock, out int[] slidesAlreadyExistsId);

        bool SaveSlideChanges(UserIdentity userIdentity, string presentationUniqueName,
                              Slide[] slideToSave, out int[] slideIdNotLocked,
                              out ResourceDescriptor[] resourcesNotExists,
                              out DeviceResourceDescriptor[] deviceResourcesNotExists,
                              out int[] labelNotExists);
        bool DeletePresentation(UserIdentity sender, string uniqueName);
        PresentationStatus GetPresentationStatus(string uniqueName, out UserIdentity userIdentity);
        bool AcquireLockForPresentation(ICommunicationObject communicationObject, UserIdentity userIdentity, string uniqueName,
                                        RequireLock requireLock);
        bool ReleaseLockForPresentation(UserIdentity userIdentity, string uniqueName);
        bool AcquireLockForSlide(ICommunicationObject communicationObject, UserIdentity userIdentity, string presentationUniqueName, int slideId,
                                 RequireLock requireLock);
        bool ReleaseLockForSlide(UserIdentity userIdentity, string presentationUniqueName, int slideId);
        LockingInfo[] GetLockingInfo(ObjectKey[] objectKeyArr);
        LockingInfo[] GetLockingInfo(string uniqueName);
        RemoveResult DeleteSource(UserIdentity sender, ResourceDescriptor descriptor);
        RemoveResult DeleteDeviceSource(UserIdentity sender, DeviceResourceDescriptor descriptor);

        FileSaveStatus SaveDeviceSource(UserIdentity userIdentity, DeviceResourceDescriptor resourceDescriptor,
                                        SourceStatus status, out string newResourceId);
        FileSaveStatus GetSourceStatus(ResourceDescriptor descriptor);
        bool IsResourceAvailable(ResourceDescriptor descriptor);
        FileSaveStatus GetDeviceSourceStatus(DeviceResourceDescriptor descriptor);
        bool IsResourceAvailable(DeviceResourceDescriptor descriptor);
        ResourceDescriptor CopySourceFromGlobalToLocal(UserIdentity sender, ResourceDescriptor resourceDescriptor,
                                                       string presentationUniqueName);
        ResourceDescriptor CopySourceFromLocalToGlobal(UserIdentity sender, ResourceDescriptor resourceDescriptor);
        void CopySourceFromLocalToLocal(UserIdentity userIdentity, string fromUniqueName, string toUniqueName);
        Dictionary<string, IList<ResourceDescriptor>> GetGlobalSources();
        Dictionary<string, IList<ResourceDescriptor>> GetLocalSources(string presentationUniqueName);
        Dictionary<string, IList<DeviceResourceDescriptor>> GetGlobalDeviceSources();
        PresentationInfo[] GetPresentationWhichContainsSource(ResourceDescriptor resourceDescriptor);
        PresentationInfo[] GetPresentationWhichContainsDeviceSource(DeviceResourceDescriptor resourceDescriptor);
        ISourceDAL SourceDAL { get; }

        event EventHandler<PresentationChangedEventArgs> OnPresentationChanged;
        event EventHandler<SlideChangedEventArgs> OnSlideChanged;
    }
}
