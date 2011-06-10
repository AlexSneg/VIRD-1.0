using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Persistence.CommonPresentation
{
    internal class ResourceById<TResource> where TResource : ResourceDescriptorAbstract
    {
        private readonly Dictionary<string, TResource> _resourceByIdDic = new Dictionary<string, TResource>(10);

        public ResourceById() { }

        public ResourceById(IEnumerable<TResource> resourceDescriptors)
        {
            foreach (TResource descriptor in resourceDescriptors)
            {
                _resourceByIdDic[descriptor.ResourceInfo.Id] = descriptor;
            }
        }

        //public void Add(ResourceDescriptor resourceDescriptor)
        //{
        //    _resourceByIdDic[resourceDescriptor.ResourceInfo.Id] = resourceDescriptor;
        //}

        //public ResourceDescriptor GetResource(string id)
        //{
        //    ResourceDescriptor resourceDescriptor;
        //    if (_resourceByIdDic.TryGetValue(id, out resourceDescriptor))
        //        return resourceDescriptor;
        //    return null;
        //}

        public IList<TResource> GetResources()
        {
            return _resourceByIdDic.Values.ToList();
        }

        public void Add(TResource resourceDescriptor)
        {
            _resourceByIdDic[resourceDescriptor.ResourceInfo.Id] = resourceDescriptor;
        }

        public void Delete(TResource resourceDescriptor)
        {
            _resourceByIdDic.Remove(resourceDescriptor.ResourceInfo.Id);
        }

    }

    internal class GlobalResources<TResource> where TResource : ResourceDescriptorAbstract
    {
        private readonly Dictionary<string, ResourceById<TResource>> _globalResourcesCache = new Dictionary<string, ResourceById<TResource>>(10);

        public Dictionary<string, IList<TResource>> GetGlobalResources()
        {
            return _globalResourcesCache.ToDictionary(kv => kv.Key, kv => kv.Value.GetResources());
        }

        public void Init(Dictionary<string, IList<TResource>> globalSources)
        {
            _globalResourcesCache.Clear();
            foreach (KeyValuePair<string, IList<TResource>> pair in globalSources)
            {
                ResourceById<TResource> resourceById = new ResourceById<TResource>(pair.Value);
                _globalResourcesCache[pair.Key] = resourceById;
            }
        }

        public void AddResource(TResource resourceDescriptor)
        {
            ResourceById<TResource> resourceById;
            if (!_globalResourcesCache.TryGetValue(resourceDescriptor.ResourceInfo.Type, out resourceById))
            {
                _globalResourcesCache[resourceDescriptor.ResourceInfo.Type] = resourceById = new ResourceById<TResource>();
            }
            resourceById.Add(resourceDescriptor);
        }

        public void DeleteResource(TResource resourceDescriptor)
        {
            ResourceById<TResource> resourceById;
            if (_globalResourcesCache.TryGetValue(resourceDescriptor.ResourceInfo.Type, out resourceById))
            {
                resourceById.Delete(resourceDescriptor);
            }
        }
    }

}
