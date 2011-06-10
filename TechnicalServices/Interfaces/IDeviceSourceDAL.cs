using System;
using System.Collections.Generic;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Interfaces
{
    public interface IDeviceSourceDAL : IResourceEx<DeviceResourceDescriptor>
    {
        bool DeleteSource(UserIdentity sender, DeviceResourceDescriptor descriptor);
        FileSaveStatus GetSourceStatus(DeviceResourceDescriptor descriptor);
        bool IsResourceAvailable(DeviceResourceDescriptor descriptor);
        Dictionary<string, IList<DeviceResourceDescriptor>> GetGlobalSources();
        event EventHandler<SourceEventArg<DeviceResourceDescriptor>> OnResourceAdded;
        event EventHandler<SourceEventArg<DeviceResourceDescriptor>> OnResourceDeleted;
        event EventHandler<SourceEventArg<DeviceResourceDescriptor>> OnResourceUpdated;
    }
}
