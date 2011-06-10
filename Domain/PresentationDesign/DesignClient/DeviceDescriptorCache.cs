using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Domain.PresentationDesign.Client
{
    internal class DeviceDescriptorCache : IDisposable
    {
        private Dictionary<string, List<DeviceResourceDescriptor>> _resources;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private IPresentationNotifier _presentationNotifier;

        public DeviceDescriptorCache(IPresentationNotifier presentationNotifier)
        {
            IsInitialized = false;
            _presentationNotifier = presentationNotifier;
            _presentationNotifier.OnDeviceResourceAdded += new EventHandler<TechnicalServices.Common.Classes.NotifierEventArg<DeviceResourceDescriptor>>(presentationNotifier_OnDeviceResourceAdded);
            _presentationNotifier.OnDeviceResourceDeleted += new EventHandler<TechnicalServices.Common.Classes.NotifierEventArg<DeviceResourceDescriptor>>(presentationNotifier_OnDeviceResourceDeleted);
            _presentationNotifier.OnDeviceResourceUpdated += new EventHandler<TechnicalServices.Common.Classes.NotifierEventArg<DeviceResourceDescriptor>>(presentationNotifier_OnDeviceResourceUpdated);
        }

        public bool IsInitialized
        {
            get; private set;
        }

        public void Init(Dictionary<string, IList<DeviceResourceDescriptor>> resources)
        {
            _lock.EnterWriteLock();
            try
            {
                _resources = resources.ToDictionary(kv=>kv.Key, kv=>new List<DeviceResourceDescriptor>(kv.Value));
                IsInitialized = true;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void AddResource(DeviceResourceDescriptor resource)
        {
            _lock.EnterWriteLock();
            try
            {
                if (IsInitialized)
                {
                    Add(resource);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void UpdateResource(DeviceResourceDescriptor resource)
        {
            _lock.EnterWriteLock();
            try
            {
                if (IsInitialized)
                {
                    Add(resource);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void DeleteResource(DeviceResourceDescriptor resource)
        {
            _lock.EnterWriteLock();
            try
            {
                if (IsInitialized)
                {
                    List<DeviceResourceDescriptor> list;
                    if (_resources.TryGetValue(resource.ResourceInfo.Type, out list))
                    {
                        int index = list.FindIndex(drd => drd.ResourceInfo.Equals(resource.ResourceInfo));
                        if (index >= 0)
                        {
                            list.RemoveAt(index);
                        }
                    }
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public Dictionary<string, IList<DeviceResourceDescriptor>> GetResources()
        {
            _lock.EnterReadLock();
            try
            {
                if (!IsInitialized)
                    return null;
                return _resources.ToDictionary<KeyValuePair<string, List<DeviceResourceDescriptor>>, string, IList<DeviceResourceDescriptor>>(kv => kv.Key, kv => kv.Value);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        private void Add(DeviceResourceDescriptor resource)
        {
            List<DeviceResourceDescriptor> list;
            if (!_resources.TryGetValue(resource.ResourceInfo.Type, out list))
            {
                _resources[resource.ResourceInfo.Type] = list = new List<DeviceResourceDescriptor>();
            }
            int index = list.FindIndex(drd => drd.ResourceInfo.Equals(resource.ResourceInfo));
            if (index >= 0)
            {
                list[index] = resource;
            }
            else
            {
                list.Add(resource);
            }
        }

        void presentationNotifier_OnDeviceResourceUpdated(object sender, TechnicalServices.Common.Classes.NotifierEventArg<DeviceResourceDescriptor> e)
        {
            AddResource(e.Data);
        }

        void presentationNotifier_OnDeviceResourceDeleted(object sender, TechnicalServices.Common.Classes.NotifierEventArg<DeviceResourceDescriptor> e)
        {
            DeleteResource(e.Data);
        }

        void presentationNotifier_OnDeviceResourceAdded(object sender, TechnicalServices.Common.Classes.NotifierEventArg<DeviceResourceDescriptor> e)
        {
            AddResource(e.Data);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _presentationNotifier.OnDeviceResourceAdded -= presentationNotifier_OnDeviceResourceAdded;
            _presentationNotifier.OnDeviceResourceDeleted -= presentationNotifier_OnDeviceResourceDeleted;
            _presentationNotifier.OnDeviceResourceUpdated -= presentationNotifier_OnDeviceResourceUpdated;
            _resources.Clear();
            _resources = null;
        }

        #endregion
    }
}