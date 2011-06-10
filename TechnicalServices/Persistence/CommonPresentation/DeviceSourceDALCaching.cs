using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Persistence.CommonPresentation
{
    public class DeviceSourceDALCaching : DeviceSourceDAL
    {
        private readonly DeviceResourceDescriptorCache _cache = new DeviceResourceDescriptorCache();

        public DeviceSourceDALCaching(IConfiguration configuration, bool isStandalone)
            : base(configuration, isStandalone)
        {}

        public override Dictionary<string, IList<DeviceResourceDescriptor>> GetGlobalSources()
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                Dictionary<string, IList<DeviceResourceDescriptor>> globalSources = _cache.GetGlobalSources();
                if (globalSources == null || globalSources.Count == 0)
                {
                    globalSources = base.GetGlobalSources();
                    _cache.AddGlobalSources(globalSources);
                }
                return globalSources;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public override DeviceResourceDescriptor SaveSource(UserIdentity sender, DeviceResourceDescriptor resourceDescriptor, Dictionary<string, string> fileDic)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {

                DeviceResourceDescriptor stored
                    = base.SaveSource(sender, resourceDescriptor, fileDic);
                if (null != stored)
                {
                    _cache.AddResource(stored);
                }
                return stored;
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public override bool DeleteSource(UserIdentity sender, DeviceResourceDescriptor descriptor)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                bool isSuccess = base.DeleteSource(sender, descriptor);
                if (isSuccess)
                    _cache.DeleteResource(descriptor);
                return isSuccess;
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }
    }
}
