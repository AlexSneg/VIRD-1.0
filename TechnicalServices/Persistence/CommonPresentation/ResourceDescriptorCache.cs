using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Persistence.CommonPresentation
{
    internal class ResourceDescriptorCache : AbstractResourceDescriptorCache<ResourceDescriptor>
    {
        #region Nested

        private class LocalResources
        {
            private readonly Dictionary<string, Dictionary<string, ResourceById<ResourceDescriptor>>> _localResourcesCache = new Dictionary<string, Dictionary<string, ResourceById<ResourceDescriptor>>>(10);

            public Dictionary<string, IList<ResourceDescriptor>> GetLocalResources(string presentationUniqueName)
            {
                Dictionary<string, ResourceById<ResourceDescriptor>> item;
                if (!_localResourcesCache.TryGetValue(presentationUniqueName, out item)) return null;
                return item.ToDictionary(kv => kv.Key, kv => kv.Value.GetResources());
            }

            public void Init(string presentationUniqueName, Dictionary<string, IList<ResourceDescriptor>> localSources)
            {
                Dictionary<string, ResourceById<ResourceDescriptor>> item;
                if (_localResourcesCache.TryGetValue(presentationUniqueName, out item))
                {
                    item.Clear();
                }
                _localResourcesCache[presentationUniqueName] =
                    localSources.ToDictionary(kv => kv.Key, kv => new ResourceById<ResourceDescriptor>(kv.Value));
            }

            public void AddResource(ResourceDescriptor resourceDescriptor)
            {
                string uniquePresentationName = resourceDescriptor.PresentationUniqueName;
                Dictionary<string, ResourceById<ResourceDescriptor>> item;
                if (!_localResourcesCache.TryGetValue(uniquePresentationName, out item))
                {
                    _localResourcesCache[uniquePresentationName] = item =
                                                                   new Dictionary<string, ResourceById<ResourceDescriptor>>(10);
                }
                ResourceById<ResourceDescriptor> resourceById;
                if (!item.TryGetValue(resourceDescriptor.ResourceInfo.Type, out resourceById))
                {
                    item[resourceDescriptor.ResourceInfo.Type] = resourceById = new ResourceById<ResourceDescriptor>();
                }
                resourceById.Add(resourceDescriptor);
            }

            public void DeleteResource(ResourceDescriptor resourceDescriptor)
            {
                Dictionary<string, ResourceById<ResourceDescriptor>> item;
                if (_localResourcesCache.TryGetValue(resourceDescriptor.PresentationUniqueName, out item))
                {
                    ResourceById<ResourceDescriptor> resourceById;
                    if (item.TryGetValue(resourceDescriptor.ResourceInfo.Type, out resourceById))
                    {
                        resourceById.Delete(resourceDescriptor);
                    }
                }
            }

            public void DeleteLocalResources(string uniqueName)
            {
                _localResourcesCache.Remove(uniqueName);
            }
        }

        #endregion

        readonly LocalResources _localResources = new LocalResources();

        public Dictionary<string, IList<ResourceDescriptor>> GetLocalSources(string presentationUniqueName)
        {
            return _localResources.GetLocalResources(presentationUniqueName);
        }

        public void AddLocalSources(string presentationUniqueName, Dictionary<string, IList<ResourceDescriptor>> localSources)
        {
            _localResources.Init(presentationUniqueName, localSources);
        }

        public override void AddResource(ResourceDescriptor resourceDescriptor)
        {
            if (resourceDescriptor.IsLocal)
            {
                _localResources.AddResource(resourceDescriptor);
            }
            else
            {
                base.AddResource(resourceDescriptor);
            }
        }

        public override void DeleteResource(ResourceDescriptor resourceDescriptor)
        {
            if (resourceDescriptor.IsLocal)
            {
                _localResources.DeleteResource(resourceDescriptor);
            }
            else
            {
                base.DeleteResource(resourceDescriptor);
            }
        }

        public void DeleteLocalResources(string uniqueName)
        {
            _localResources.DeleteLocalResources(uniqueName);
        }
    }
}
