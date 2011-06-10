using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Interfaces
{
    public interface IPresentationClient : IDisposable
    {
        Presentation LoadPresentation(string fileName, out string[] deletedEquipment);
        void SaveSlideBulk(string fileName, SlideBulk slideBulk);
        SlideBulk LoadSlideBulk(string fileName, ResourceDescriptor[] resourceDescriptors,
            DeviceResourceDescriptor[] deviceResourceDescriptors);

        void SubscribeForMonitor(PresentationKey presentationKey);
        void UnSubscribeForMonitor(PresentationKey presentationKey);
        void SubscribeForGlobalMonitoring();
        void UnSubscribeForGlobalMonitoring();

        Slide[] LoadSlides(string uniqueName, int[] slideIds);
        PresentationInfoExt GetPresentationInfo(string uniqueName);
        PresentationInfoExt GetPresentationInfoByName(string presentationName);
        IList<PresentationInfoExt> GetPresentationInfoList();

        bool DeletePresentation(string uniqueName);

        SavePresentationResult SavePresentationChanges(PresentationInfo presentationInfo, Slide[] slides, out ResourceDescriptor[] notExistedResources, out DeviceResourceDescriptor[] notExistedDeviceResources, out int[] labelNotExists,
            out UserIdentity[] whoLock, out int[] slidesAlreadyExistsId);
        bool SaveSlideChanges(string uniqueName, Slide[] slides, out int[] notLockedSlide, out ResourceDescriptor[] notExistedResources, out DeviceResourceDescriptor[] notExistedDeviceResources, out int[] labelNotExists);

        bool AcquireLockForSlide(string uniqueName, int slideId, RequireLock requireLock);
        bool ReleaseLockForSlide(string uniqueName, int slideId);
        bool AcquireLockForPresentation(string uniqueName, RequireLock requireLock);
        bool ReleaseLockForPresentation(string uniqueName);
        LockingInfo[] GetLockingInfo(ObjectKey[] objectKeyArr);


        Dictionary<string,IList<ResourceDescriptor>> GetGlobalSources();
        Dictionary<string, IList<ResourceDescriptor>> GetLocalSources(string presentationUniqueName);
        Dictionary<string, IList<DeviceResourceDescriptor>> GetGlobalDeviceSources();

        bool IsResourceAvailable(ResourceDescriptor descriptor);
        bool IsResourceAvailable(DeviceResourceDescriptor descriptor);

        RemoveResult DeleteSource(ResourceDescriptor descriptor);

        FileSaveStatus SaveDeviceSource(DeviceResourceDescriptor descriptor);

        ResourceDescriptor CopySourceFromLocalToGlobal(ResourceDescriptor descriptor);
        ResourceDescriptor CopySourceFromGlobalToLocal(ResourceDescriptor descriptor, string uniqueName);
        void CopySourceFromLocalToLocal(string fromUniqueName, string toUniqueName);
        CreatePresentationResult CreatePresentation(PresentationInfo presentationInfo, out int[] labelNotExists);

        FilesGroup GetPresentationSchemaFilesForExport();

        FilesGroup GetPresentationForExport(string uniqueName);

        PresentationStatus GetPresentationStatus(string uniqueName, out UserIdentity identity);

        IClientResourceCRUD<ResourceDescriptor> GetResourceCrud();
        IClientResourceCRUD<FilesGroup> GetPresentationSchemaCrud();
        //IClientResourceCRUD<DeviceResourceDescriptor> GetDeviceResourceCrud();
        IClientResourceCRUD<FilesGroup> GetPresentationExportCrud();

        ISourceDAL SourceDAL { get; }
        IDeviceSourceDAL DeviceSourceDAL { get; }
    }
}
