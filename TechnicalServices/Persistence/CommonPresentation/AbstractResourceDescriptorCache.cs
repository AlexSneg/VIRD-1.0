using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Persistence.CommonPresentation
{
    internal abstract class AbstractResourceDescriptorCache<TResource> where TResource : ResourceDescriptorAbstract
    {
        protected readonly GlobalResources<TResource> _globalResources = new GlobalResources<TResource>();

        public Dictionary<string, IList<TResource>> GetGlobalSources()
        {
            return _globalResources.GetGlobalResources();
        }

        public void AddGlobalSources(Dictionary<string, IList<TResource>> globalSources)
        {
            _globalResources.Init(globalSources);
        }

        public virtual void AddResource(TResource descriptor)
        {
            _globalResources.AddResource(descriptor);
        }

        public virtual void DeleteResource(TResource descriptor)
        {
            _globalResources.DeleteResource(descriptor);
        }

    }
}
