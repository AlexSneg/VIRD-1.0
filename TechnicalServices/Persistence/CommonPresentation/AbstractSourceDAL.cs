using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using System.IO;

namespace TechnicalServices.Persistence.CommonPresentation
{
    public abstract class AbstractSourceDAL<TResource> where TResource : ResourceDescriptorAbstract
    {
        private List<Type> _extraTypes = null;
        protected const string localResourcePrefix = @"local//";
        protected const string globalResourcePrefix = @"global//";
        protected const string _resourceInfoExt = ".resource.xml";

        //protected readonly Dictionary<string, Type[]> ResourceInfoExtraTypeDic = new Dictionary<string, Type[]>();

        protected readonly bool _isStandalone;
        protected readonly IConfiguration _configuration;
        protected static readonly ReaderWriterLock _sync = new ReaderWriterLock();

        protected AbstractSourceDAL(IConfiguration configuration, bool isStandalone)
        {
            Debug.Assert(configuration != null);
            _configuration = configuration;
            _isStandalone = isStandalone;
        }

        #region protected

        protected virtual string GetResourceInfoFullFileName(TResource descriptor)
        {
            return GetResourceInfoFullFileName(GetPath(descriptor), descriptor.ResourceInfo);
        }

        protected static string GetResourceInfoFullFileName(string path, ResourceInfo resourceInfo)
        {
            return GetResourceInfoFullFileName(path, resourceInfo.Id);
        }

        protected static string GetResourceInfoFullFileName(string path, string resourceId)
        {
            return Path.Combine(path, resourceId + _resourceInfoExt);
        }

        protected void OnDelete(UserIdentity sender, TResource resourceDescriptor)
        {
            if (OnResourceDeleted != null)
            {
                OnResourceDeleted(this, new SourceEventArg<TResource>(resourceDescriptor, sender));
            }
        }

        protected void OnAdd(UserIdentity sender, TResource resourceDescriptor)
        {
            if (OnResourceAdded != null)
            {
                OnResourceAdded(this, new SourceEventArg<TResource>(resourceDescriptor, sender));
            }
        }

        protected void OnUpdate(UserIdentity sender, TResource descriptor)
        {
            if (OnResourceUpdated != null)
            {
                OnResourceUpdated(this, new SourceEventArg<TResource>(descriptor, sender));
            }
        }

        protected bool IsExistsWithSameName(TResource resource)
        {
            List<TResource> resourcesByName = SearchByName(resource);
            return resourcesByName != null && resourcesByName.Count > 0;
            //IList<TResource> resources;
            //GetGlobalSources().TryGetValue(resource.ResourceInfo.Type, out resources);
            //if (resources == null) return false;
            //return resources.Any(rd => rd.ResourceInfo.Name.Equals(
            //                             resource.ResourceInfo.Name,
            //                             StringComparison.InvariantCultureIgnoreCase));
        }

        protected ResourceInfo GetResourceInfo(string resourceInfoFile, string type, ModuleConfiguration config)
        {
            type = Path.GetFileName(type);
            ResourceInfo info;
            //if (!_configuration.ModuleConfiguration.ResourceInfoDic.TryGetValue(type, out info))
            //    info = new ResourceFileInfo() { Type = type };
            try
            {
                //Type[] types = GetExtraTypes(type);
                //if (types == null || types.Length == 0)
                //{
                //    _configuration.EventLog.WriteError(string.Format(
                //        "AbstractSourceDal.GetResourceInfo: Не надено ресурса в конфигурации для файла {0}, типа {1}", resourceInfoFile, type));
                //}
                info = ResourceInfoExt.LoadFromFile(resourceInfoFile, ExtraTypes);
                info.Init(config);
                return info;
            }
            catch (Exception ex)
            {
                _configuration.EventLog.WriteError(string.Format(
                    "AbstractSourceDal.GetResourceInfo: {0}", ex));
                return null;
            }
        }

        protected static void AddNewResourceDescriptor(Dictionary<string, IList<TResource>> sourceDescriptors,
            TResource descriptor)
        {
            IList<TResource> val;
            if (!sourceDescriptors.TryGetValue(descriptor.ResourceInfo.Type, out val))
            {
                sourceDescriptors[descriptor.ResourceInfo.Type] = val = new List<TResource>();
            }
            val.Add(descriptor);
        }

        protected virtual void SaveToFile(TResource descriptor, bool isNew, UserIdentity sender)
        {
            descriptor.ResourceInfo.SaveToFile(GetResourceInfoFullFileName(descriptor), ExtraTypes);
        }

        //protected TResource FindResourceUsingAnyUniqueField(TResource descriptor)
        //{
        //    string resourceFile = GetResourceInfoFullFileName(descriptor);
        //    ResourceInfo resourceInfo = null;
        //    if (File.Exists(resourceFile))
        //    {
        //        resourceInfo = GetResourceInfo(resourceFile, descriptor.ResourceInfo.Type,
        //                                                    _configuration.ModuleConfiguration);
        //    }
        //    else
        //    {
        //        resourceInfo = SearchByName(descriptor);
        //    }
        //    if (resourceInfo == null) return null;
        //    TResource newDescriptor = CreateResourceDescriptor(descriptor, resourceInfo);
        //    return newDescriptor;
        //}

        protected Type[] ExtraTypes
        {
            get
            {
                //Type[] types;
                //if (ResourceInfoExtraTypeDic.TryGetValue(type, out types)) return types;
                //return null;
                //foreach (IModule module in _configuration.ModuleList)
                //{
                //    if (module.)
                //}

                if (_extraTypes == null)
                {
                    _extraTypes = new List<Type>();
                    foreach (IModule module in _configuration.ModuleList)
                    {
                        _extraTypes.AddRange(
                            module.SystemModule.Configuration.GetExtensionType().Union(
                                module.SystemModule.Presentation.GetExtensionType()));
                    }
                }
                return _extraTypes.ToArray();
            }
        }

        protected abstract string GetPath(TResource resource);

        protected abstract string GetGlobalSourceFolder();

        protected abstract string ComposeResourceFullFileName(TResource descriptor, string resourceId);

        protected abstract TResource CreateGlobalResourceDescriptor(ResourceInfo resourceInfo);

        protected abstract TResource CreateResourceDescriptor(TResource descriptor, ResourceInfo resourceInfo);

        protected abstract bool IsExistsInConfiguration(ResourceInfo resourceInfo);

        #endregion

        #region public

        public abstract List<TResource> SearchByName(TResource descriptor);

        public event EventHandler<SourceEventArg<TResource>> OnResourceAdded;
        public event EventHandler<SourceEventArg<TResource>> OnResourceDeleted;
        public event EventHandler<SourceEventArg<TResource>> OnResourceUpdated;

        public TResource MakeFullClone(TResource descriptor)
        {
            ResourceInfo clone = descriptor.ResourceInfo.MakeClone(ExtraTypes);
            clone.Init(_configuration.ModuleConfiguration);
            return CreateResourceDescriptor(descriptor, clone);
        }

        public virtual bool DeleteSource(UserIdentity sender, TResource descriptor)
        {
            bool result = false;
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                string resourceInfoFileName = GetResourceInfoFullFileName(descriptor);
                if (File.Exists(resourceInfoFileName))
                {
                    File.Delete(resourceInfoFileName);
                    result = true;
                }
                ResourceFileInfo resourceFileInfo = descriptor.ResourceInfo as ResourceFileInfo;
                if (resourceFileInfo != null)
                {
                    foreach (string id in resourceFileInfo.ResourceFileList.Select(rfp => rfp.Id))
                    {
                        string resourceFileName = GetResourceFileName(descriptor, id);
                        File.Delete(resourceFileName);
                    }
                }
                if (result)
                    OnDelete(sender, descriptor);
                return result;
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public virtual Dictionary<string, IList<TResource>> GetGlobalSources()
        {
            Dictionary<string, IList<TResource>> sourceDescriptors = new Dictionary<string, IList<TResource>>();
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                foreach (string directory in Directory.GetDirectories(GetGlobalSourceFolder()))
                {
                    foreach (string file in Directory.GetFiles(directory))
                    {
                        if (!file.EndsWith(_resourceInfoExt, StringComparison.InvariantCultureIgnoreCase)) continue;
                        // файл - инфо о ресурсе
                        ResourceInfo info = GetResourceInfo(file, directory, _configuration.ModuleConfiguration);
                        // если загрузить не удалось или нет такого ресурса в конфигурации - пропускаем
                        if (info == null || !IsExistsInConfiguration(info)) continue;
                        TResource descriptor = CreateGlobalResourceDescriptor(info);
                        ResourceFileInfo resourceFileInfo = info as ResourceFileInfo;
                        if (resourceFileInfo != null)
                        {
                            foreach (ResourceFileProperty property in resourceFileInfo.ResourceFileList)
                            {
                                property.ResourceFullFileName =
                                    ComposeResourceFullFileName(descriptor, property.Id);
                            }
                        }
                        AddNewResourceDescriptor(sourceDescriptors, descriptor);
                        //sourceDescriptors.Add(new ResourceDescriptor(false, null, info));
                    }
                }
                return sourceDescriptors;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public FileSaveStatus GetSourceStatus(TResource descriptor)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                string fullFileName = GetResourceInfoFullFileName(descriptor);
                if (File.Exists(fullFileName))
                {
                    return FileSaveStatus.Exists;
                }
                else
                {
                    return FileSaveStatus.Ok;
                }
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public virtual bool IsExists(TResource resource)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                string resourceInfoFile = GetResourceInfoFullFileName(resource);
                if (File.Exists(resourceInfoFile)) return true;
                else return false;
                    //return IsExistsWithSameName(resource);
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public bool IsResourceExists(TResource descriptor, string resourceId)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                string fileName = GetResourceFileName(descriptor, resourceId);
                if (string.IsNullOrEmpty(fileName)) return true;
                return File.Exists(fileName);
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public bool IsResourceAvailable(TResource descriptor)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                ResourceInfo info = descriptor.ResourceInfo;
                if (!File.Exists(GetResourceFileName(descriptor))) return false;
                ResourceFileInfo resourceFileInfo = info as ResourceFileInfo;
                if (resourceFileInfo == null) return true;
                foreach (ResourceFileProperty property in resourceFileInfo.ResourceFileList)
                {
                    if (property.Required && !File.Exists(GetResourceFileName(descriptor, property.Id)))
                        return false;
                }
                return true;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public virtual string GetResourceFileName(TResource descriptor, string resourceId)
        {
            ResourceFileInfo info = descriptor.ResourceInfo as ResourceFileInfo;
            if (info == null) return null;
            string resId = string.IsNullOrEmpty(resourceId) ? info.MasterResourceProperty.Id : resourceId;
            return Path.Combine(GetPath(descriptor), info.Id + "." + resId);
        }

        public string GetResourceFileName(TResource descriptor)
        {
            return GetResourceFileName(descriptor, null);
        }

        public string GetRealResourceFileName(TResource descriptor, string resourceId)
        {
            ResourceFileInfo info = descriptor.ResourceInfo as ResourceFileInfo;
            if (info == null) return null;
            string resId = string.IsNullOrEmpty(resourceId) ? info.MasterResourceProperty.Id : resourceId;
            return Path.Combine(GetPath(descriptor), info.ResourceFileList.Find(rfp => rfp.Id.Equals(resId)).ResourceFileName);
        }

        public string GetRealResourceFileName(TResource descriptor)
        {
            return GetRealResourceFileName(descriptor, null);
        }

        public TResource GetStoredSource(TResource descriptor)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                //return FindResourceUsingAnyUniqueField(descriptor);
                if (descriptor == null) return null;
                if (!IsExists(descriptor)) return null;
                string resourceFile = GetResourceInfoFullFileName(descriptor);
                if (!File.Exists(resourceFile)) return null;
                ResourceInfo resourceInfo = GetResourceInfo(resourceFile, descriptor.ResourceInfo.Type, _configuration.ModuleConfiguration);
                TResource newDescriptor = CreateResourceDescriptor(descriptor, resourceInfo);
                return newDescriptor;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public TResource SaveSource(UserIdentity sender, TResource descriptor)
        {
            return SaveSource(sender, descriptor, new Dictionary<string, string>());
        }

        public virtual TResource SaveSource(UserIdentity sender, TResource descriptor, Dictionary<string, string> fileDic)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                TResource storedByGUID = GetStoredSource(descriptor);     //FindResourceUsingAnyUniqueField(descriptor);
                //// для клиента особая ситуевина - может быть ресурс с тем же именем - его НЕ смотрим!!!
                //TResource storedByName = null;
                //if (!_configuration.IsClient || _isStandalone)
                //{
                //    storedByName = SearchByName(descriptor);
                //}
                //if (storedByGUID != null && storedByName != null && !storedByGUID.Equals(storedByName)) return null;
                // для клиента особая ситуевина - может быть ресурс с тем же именем - его тогда надо грохать
                //if (storedByGUID == null && storedByName != null && _configuration.IsClient
                //    && !_isStandalone)
                //{
                //    DeleteSource(sender, storedByName);
                //    storedByName = null;
                //}
                bool isNew = storedByGUID == null;
                ResourceInfo info = descriptor.ResourceInfo;
                //if (storedByName != null) info.Id = storedByName.ResourceInfo.Id;
                ResourceFileInfo resourceFileInfo = info as ResourceFileInfo;
                if (resourceFileInfo != null)
                {
                    foreach (KeyValuePair<string, string> pair in fileDic)
                    {
                        string resourceFile = GetResourceFileName(descriptor, pair.Key);
                        string resourceTempFileName = pair.Value;
                        if (!string.IsNullOrEmpty(resourceFile) &&
                            !string.IsNullOrEmpty(resourceTempFileName) &&
                            !resourceFile.Equals(resourceTempFileName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            File.Delete(resourceFile);
                            File.Move(resourceTempFileName, resourceFile);
                        }
                        ResourceFileProperty resourceFileProperty =
                            resourceFileInfo.ResourceFileList.Find(rfp => rfp.Id.Equals(pair.Key));
                        resourceFileProperty.ModifiedUtc = DateTime.Now.ToFileTimeUtc();
                        //resourceFileProperty.ResourceFullFileName =
                        //    ComposeResourceFullFileName(descriptor, pair.Key);
                        resourceFileProperty.Newly = false;
                    }
                    foreach (ResourceFileProperty resourceFileProperty in resourceFileInfo.ResourceFileList)
                    {
                        resourceFileProperty.ResourceFullFileName =
                            ComposeResourceFullFileName(descriptor, resourceFileProperty.Id);
                    }
                }
                SaveToFile(descriptor, isNew, sender);
                if (isNew)
                    OnAdd(sender, descriptor);
                else
                    OnUpdate(sender, descriptor);
                return descriptor;
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        #endregion

    }
}
