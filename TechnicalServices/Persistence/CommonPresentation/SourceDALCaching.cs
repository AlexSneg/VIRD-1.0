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
    public class SourceDALCaching : SourceDAL
    {
        private readonly ResourceDescriptorCache _cache = new ResourceDescriptorCache();
        public SourceDALCaching(IConfiguration configuration, bool isStandalone)
            : base(configuration, isStandalone)
        {}

        public override Dictionary<string, IList<ResourceDescriptor>> GetGlobalSources()
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                Dictionary<string, IList<ResourceDescriptor>> globalSources = _cache.GetGlobalSources();
                if (globalSources == null || globalSources.Count == 0)
                {
                    globalSources = base.GetGlobalSources();
                    _cache.AddGlobalSources(globalSources);
                }
                else
                {
                    CheckAndDeleteResourceIfNotExists(globalSources);
                }
                return globalSources;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public override Dictionary<string, IList<ResourceDescriptor>> GetLocalSources(string presentationUniqueName)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                Dictionary<string, IList<ResourceDescriptor>> localSources = _cache.GetLocalSources(presentationUniqueName);
                if (localSources == null || localSources.Count == 0)
                {
                    localSources = base.GetLocalSources(presentationUniqueName);
                    _cache.AddLocalSources(presentationUniqueName, localSources);
                }
                else
                {
                    CheckAndDeleteResourceIfNotExists(localSources);
                }
                return localSources;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        private void CheckAndDeleteResourceIfNotExists(IDictionary<string, IList<ResourceDescriptor>> sources)
        {
            Dictionary<string, List<ResourceDescriptor>> toDelete = new Dictionary<string, List<ResourceDescriptor>>(sources.Count);
            foreach (KeyValuePair<string, IList<ResourceDescriptor>> pair in sources)
            {
                foreach (ResourceDescriptor resourceDescriptor in pair.Value)
                {
                    if (!IsExists(resourceDescriptor))
                    {
                        List<ResourceDescriptor> list;
                        if (!toDelete.TryGetValue(pair.Key, out list))
                        {
                            toDelete[pair.Key] = list = new List<ResourceDescriptor>();
                        }
                        list.Add(resourceDescriptor);
                    }
                }
            }
            if (toDelete.Count != 0)
            {
                LockCookie lockCookie = _sync.UpgradeToWriterLock(Timeout.Infinite);
                try
                {
                    foreach (KeyValuePair<string, List<ResourceDescriptor>> pair in toDelete)
                    {
                        IList<ResourceDescriptor> list;
                        if (sources.TryGetValue(pair.Key, out list))
                        {
                            foreach (ResourceDescriptor resourceDescriptor in pair.Value)
                            {
                                list.Remove(resourceDescriptor);
                                _cache.DeleteResource(resourceDescriptor);
                            }
                        }
                    }
                }
                finally
                {
                    _sync.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }

        public override ResourceDescriptor SaveSource(UserIdentity sender, ResourceDescriptor resourceDescriptor, Dictionary<string, string> fileDic)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {

                ResourceDescriptor stored = base.SaveSource(sender, resourceDescriptor, fileDic);
                if (null != stored /*&& !(resourceDescriptor is BackgroundImageDescriptor)*/)
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

        public override bool DeleteSource(UserIdentity sender, ResourceDescriptor descriptor)
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

        public override void DeleteLocalSourceFolder(string uniqueName)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                base.DeleteLocalSourceFolder(uniqueName);
                _cache.DeleteLocalResources(uniqueName);
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }
    }
}
