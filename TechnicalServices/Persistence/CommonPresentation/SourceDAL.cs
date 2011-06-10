using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Linq;

using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Persistence.CommonPresentation
{
    public class SourceDAL : AbstractSourceDAL<ResourceDescriptor>, ISourceDAL
    {
        //private bool _isStandAlone;

        public SourceDAL(IConfiguration configuration, bool isStandalone)
            : base(configuration, isStandalone)
        {
            if (!Directory.Exists(_configuration.LocalSourceFolder))
                throw new ApplicationException(String.Format("Нет директории: {0}", _configuration.LocalSourceFolder));
            if (!Directory.Exists(_configuration.GlobalSourceFolder))
                throw new ApplicationException(String.Format("Нет директории: {0}", _configuration.GlobalSourceFolder));
            //// маппинг типов сорсов с типами ResourceInfo
            //foreach (SourceType sourceType in _configuration.ModuleConfiguration.SourceList)
            //{
            //    IEnumerable<IModule> modules = _configuration.ModuleList.Where(
            //        module => module.SystemModule.Configuration.GetSource().Contains(sourceType.GetType()));
            //    if (modules.Count() == 0) continue;
            //    ResourceInfoExtraTypeDic[sourceType.Type] =
            //        modules.SelectMany(
            //            module =>
            //            module.SystemModule.Configuration.GetExtensionType().Union(
            //                module.SystemModule.Presentation.GetExtensionType())).ToArray();
            //}
        }

        #region public
        /// <summary>
        /// создание один раз харварных ресурсов
        /// </summary>
        public virtual void CreateHardwareSources()
        {
            if (_configuration.IsClient) return;
            List<ResourceInfo> resourceInfos = new List<ResourceInfo>();
            foreach (SourceType sourceType in _configuration.ModuleConfiguration.SourceList)
            {
                if (!sourceType.IsHardware) continue;
                ResourceInfo resourceInfo = sourceType.CreateNewResourceInfo(_configuration.ModuleConfiguration);
                if (resourceInfos.Exists(
                    ri => ri.Type.Equals(resourceInfo.Type, StringComparison.InvariantCultureIgnoreCase)
                    && ri.Name.Equals(resourceInfo.Name, StringComparison.InvariantCultureIgnoreCase))) continue;
                resourceInfos.Add(resourceInfo);
            }
            List<ResourceInfo> existed = new List<ResourceInfo>(resourceInfos.Count);
            foreach (ResourceInfo resourceInfo in resourceInfos)
            {
                ResourceDescriptor resourceDescriptor = new ResourceDescriptor(false, null, resourceInfo);
                string path = GetPath(resourceDescriptor);
                string[] resources =
                    Directory.GetFiles(path).Where(
                        file => file.EndsWith(_resourceInfoExt, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                foreach (string resource in resources)
                {
                    ResourceInfo ri = resourceInfo.GetResourceInfo(resource, ExtraTypes);
                    existed.Add(ri);
                }
            }
            foreach (ResourceInfo info in existed)
            {
                resourceInfos.RemoveAll(ri => ri.Type.Equals(info.Type, StringComparison.InvariantCultureIgnoreCase)
                                              && ri.Name.Equals(info.Name, StringComparison.InvariantCultureIgnoreCase));
            }
            foreach (ResourceInfo resourceInfo in resourceInfos)
            {
                string file = GetResourceInfoFullFileName(
                    new ResourceDescriptor(false, null, resourceInfo));
                if (!string.IsNullOrEmpty(file))
                    resourceInfo.SaveToFile(file, ExtraTypes);
            }
        }
        #endregion

        #region ISourceDAL Members

        public Dictionary<string, IList<ResourceDescriptor>> GetAllSources(IPresentationDAL presentationDAL)
        {
            Dictionary<string, IList<ResourceDescriptor>> resourceDescriptors = new Dictionary<string, IList<ResourceDescriptor>>();
            //List<ResourceDescriptor> resourceDescriptors = new List<ResourceDescriptor>();
            IList<PresentationInfo> presentationInfos = presentationDAL.GetPresentationInfoList();
            foreach (PresentationInfo info in presentationInfos)
            {
                foreach (KeyValuePair<string, IList<ResourceDescriptor>> pair in GetLocalSources(info.UniqueName))
                {
                    resourceDescriptors[pair.Key] = pair.Value;
                }
                //ResourceDescriptor[] descriptors = GetLocalSources(info);
                //resourceDescriptors.AddRange(descriptors);
            }
            foreach (KeyValuePair<string, IList<ResourceDescriptor>> pair in GetGlobalSources())
            {
                foreach (ResourceDescriptor descriptor in pair.Value)
                {
                    AddNewResourceDescriptor(resourceDescriptors, descriptor);
                }
            }
            //resourceDescriptors.AddRange(GetGlobalSources());
            return resourceDescriptors;
        }

        public ResourceDescriptor GetStoredSource(string resourceId, string type, string presentationUniqueName)
        {
            string resourceFile = GetResourceInfoFullFileName(resourceId, type, presentationUniqueName);
            if (!File.Exists(resourceFile)) return null;
            ResourceInfo resourceInfo = GetResourceInfo(resourceFile,
                                                        type, _configuration.ModuleConfiguration);
            ResourceDescriptor descriptor;
            if (type.Equals(Constants.BackGroundImage))
            {
                // у нас BackGroundImage
                if (!(resourceInfo is ResourceFileInfo)) return null;
                descriptor = new BackgroundImageDescriptor(presentationUniqueName, (ResourceFileInfo)resourceInfo);
            }
            else
            {
                descriptor = new ResourceDescriptor(
                    !string.IsNullOrEmpty(presentationUniqueName),
                    presentationUniqueName, resourceInfo);
            }
            if (!IsExists(descriptor)) return null;
            return descriptor;
        }

        public void DeleteLocalSourceFolder(PresentationInfo presentationInfo)
        {
            DeleteLocalSourceFolder(presentationInfo.UniqueName);
        }

        public virtual void DeleteLocalSourceFolder(string uniqueName)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                string directory = GetLocalSourceFolder(uniqueName);
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        //public override bool IsExists(ResourceDescriptor descriptor)
        //{
        //    _sync.AcquireReaderLock(Timeout.Infinite);
        //    try
        //    {
        //        string resourceInfoFile = GetResourceInfoFullFileName(descriptor);
        //        bool fileExists = false;
        //        //if (descriptor is BackgroundImageDescriptor)
        //        //{
        //        //    string resourceFile = GetRealResourceFileName(descriptor,
        //        //        ((ResourceFileInfo)((BackgroundImageDescriptor)descriptor).ResourceInfo).
        //        //            MasterResourceProperty.Id);
        //        //    fileExists = File.Exists(resourceFile);
        //        //}
        //        //else
        //        {
        //            fileExists = File.Exists(resourceInfoFile);
        //        }
        //        if (descriptor is BackgroundImageDescriptor) return fileExists;
        //        if (fileExists) return fileExists;
        //        else
        //            return IsExistsWithSameName(descriptor);
        //    }
        //    finally
        //    {
        //        _sync.ReleaseReaderLock();
        //    }
        //}

        public ResourceDescriptor CopySourceFromGlobalToLocal(UserIdentity sender, ResourceDescriptor resourceDescriptor, string presentationUniqueName)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                ResourceInfo clone = MakeFullClone(resourceDescriptor).ResourceInfo;
                clone.Id = ResourceInfo.GenerateNewGuid();
                ResourceDescriptor newResourceDescriptor = new ResourceDescriptor(true, presentationUniqueName, clone);
                if (IsExistsWithSameName(newResourceDescriptor)) return null;
                return CopyResourceDescriptor(sender, resourceDescriptor, newResourceDescriptor);
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public ResourceDescriptor CopySourceFromLocalToGlobal(UserIdentity sender, ResourceDescriptor resourceDescriptor)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                ResourceInfo clone = MakeFullClone(resourceDescriptor).ResourceInfo;
                clone.Id = ResourceInfo.GenerateNewGuid();
                ResourceDescriptor newResourceDescriptor = new ResourceDescriptor(false, null, clone);
                if (IsExistsWithSameName(newResourceDescriptor)) return null;
                return CopyResourceDescriptor(sender, resourceDescriptor, newResourceDescriptor);
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public void CopySourceFromLocalToLocal(UserIdentity userIdentity, string fromUniqueName, string toUniqueName)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                Dictionary<string, IList<ResourceDescriptor>> resourceDescriptorses = GetLocalSources(fromUniqueName);
                foreach (IList<ResourceDescriptor> list in resourceDescriptorses.Values)
                {
                    foreach (ResourceDescriptor resourceDescriptor in list)
                    {
                        ResourceDescriptor newResourceDescriptor =
                            new ResourceDescriptor(true, toUniqueName, resourceDescriptor.ResourceInfo);
                        CopyResourceDescriptor(userIdentity, resourceDescriptor, newResourceDescriptor);
                    }
                }
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public virtual Dictionary<string, IList<ResourceDescriptor>> GetLocalSources(string presentationUniqueName) //PresentationInfo info
        {
            Dictionary<string, IList<ResourceDescriptor>> sourceDescriptors = new Dictionary<string, IList<ResourceDescriptor>>();
            //List<ResourceDescriptor> sourceDescriptors = new List<ResourceDescriptor>();
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                foreach (string directory in Directory.GetDirectories(GetLocalSourceFolder(presentationUniqueName)))
                {
                    // папку где лежат бэкграунд имиджи не рассматриваем
                    //if (Path.GetFileName(directory).Equals(Constants.BackGroundImage, StringComparison.InvariantCultureIgnoreCase))
                    //    continue;
                    foreach (string file in Directory.GetFiles(directory))
                    {
                        if (!file.EndsWith(_resourceInfoExt, StringComparison.InvariantCultureIgnoreCase)) continue;
                        ResourceInfo resourceInfo = GetResourceInfo(file, directory, _configuration.ModuleConfiguration);
                        //if (info == null) continue;
                        if (!resourceInfo.Type.Equals(Constants.BackGroundImage) && !IsExistsInConfiguration(resourceInfo))
                            continue;
                        ResourceDescriptor descriptor = null;
                        if (resourceInfo.Type.Equals(Constants.BackGroundImage) && (resourceInfo is ResourceFileInfo))
                        {
                            descriptor = new BackgroundImageDescriptor(presentationUniqueName, (ResourceFileInfo)resourceInfo);
                        }
                        else
                        {
                            descriptor = new ResourceDescriptor(true, presentationUniqueName, resourceInfo);
                        }
                        ResourceFileInfo resourceFileInfo = resourceInfo as ResourceFileInfo;
                        if (resourceFileInfo != null)
                        {
                            foreach (ResourceFileProperty property in resourceFileInfo.ResourceFileList)
                            {
                                property.ResourceFullFileName =
                                    ComposeResourceFullFileName(descriptor, property.Id);
                            }
                        }
                        AddNewResourceDescriptor(sourceDescriptors, descriptor);
                        //sourceDescriptors.Add(new ResourceDescriptor(true, info.UniqueName, resourceInfo));
                    }
                }
                //foreach (string file in Directory.GetFiles(GetLocalSourceFolder(info)))
                //{
                //    if (!file.EndsWith(_resourceInfoExt, StringComparison.InvariantCultureIgnoreCase)) continue;
                //    ResourceInfo resourceInfo = GetResourceInfo(file, directory);
                //    sourceDescriptors.Add(new ResourceDescriptor(file, String.Empty, true, info, null));
                //}
                return sourceDescriptors;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        //public override string GetResourceFileName(ResourceDescriptor resourceDescriptor, string resourceId)
        //{
        //    ResourceFileInfo info = resourceDescriptor.ResourceInfo as ResourceFileInfo;
        //    if (info == null) return null;
        //    BackgroundImageDescriptor backgroundImageDescriptor =
        //        resourceDescriptor as BackgroundImageDescriptor;
        //    if (backgroundImageDescriptor != null)
        //        return Path.Combine(GetPath(resourceDescriptor), info.MasterResourceProperty.ResourceFileName);
        //    return base.GetResourceFileName(resourceDescriptor, resourceId);
        //}

        public event Action<double, string> OnUploadSpeed;
        public void UploadSpeed(double speed, string display)
        {
            if (OnUploadSpeed != null)
            {
                OnUploadSpeed(speed, display);
            }
        }

        #endregion

        #region private
        private ResourceDescriptor CopyResourceDescriptor(UserIdentity sender, ResourceDescriptor resourceDescriptor, ResourceDescriptor newResourceDescriptor)
        {
            const string tempExt = ".copytemp";
            if (IsExists(newResourceDescriptor)) return null;
            ResourceFileInfo resourceFileInfo = resourceDescriptor.ResourceInfo as ResourceFileInfo;
            Dictionary<string, string> fileDic = new Dictionary<string, string>();

            //string fileName = GetResourceFileName(resourceDescriptor);
            if (resourceFileInfo != null)
            {
                foreach (ResourceFileProperty property in resourceFileInfo.ResourceFileList)
                {
                    string resourceFile = GetResourceFileName(resourceDescriptor, property.Id);
                    string tempFile = resourceFile + tempExt;
                    File.Copy(resourceFile, tempFile, true);
                    fileDic.Add(property.Id, tempFile);
                }
            }
            string newResourceId;
            SaveSource(sender, newResourceDescriptor, fileDic);
            return newResourceDescriptor;
        }

        private string GetLocalSourceFolder(string uniqueName)
        {
            string directory = Path.Combine(_configuration.LocalSourceFolder, uniqueName);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            return directory;
        }

        private string GetResourceInfoFullFileName(string resourceId, string type, string presentationUniqueName)
        {
            string directory;
            if (string.IsNullOrEmpty(presentationUniqueName))
            {
                //global
                directory = Path.Combine(GetGlobalSourceFolder(), type);
            }
            else
            {
                directory = Path.Combine(GetLocalSourceFolder(presentationUniqueName), type);
            }
            return GetResourceInfoFullFileName(directory, resourceId);
        }


        #endregion

        #region protected

        #endregion

        #region override

        protected override ResourceDescriptor CreateResourceDescriptor(ResourceDescriptor descriptor, ResourceInfo resourceInfo)
        {
            BackgroundImageDescriptor backgroundImageDescriptor = descriptor as BackgroundImageDescriptor;
            if (backgroundImageDescriptor != null)
                return new BackgroundImageDescriptor(backgroundImageDescriptor.PresentationUniqueName, (ResourceFileInfo)resourceInfo);
            return new ResourceDescriptor(descriptor.IsLocal, descriptor.PresentationUniqueName, resourceInfo);
        }

        public override List<ResourceDescriptor> SearchByName(ResourceDescriptor descriptor)
        {
            // для бэкграудных ресурсов - храним все не разбирая - есть такое имя или нет
            if (descriptor is BackgroundImageDescriptor) return null;
            IList<ResourceDescriptor> resourceDescriptors;
            Dictionary<string, IList<ResourceDescriptor>> dic = descriptor.IsLocal
                                                                    ? GetLocalSources(descriptor.PresentationUniqueName)
                                                                    : GetGlobalSources();
            if (!dic.TryGetValue(descriptor.ResourceInfo.Type, out resourceDescriptors))
                return new List<ResourceDescriptor>();
            List<ResourceDescriptor> list = resourceDescriptors.Where(
                rd => !rd.ResourceInfo.Id.Equals(descriptor.ResourceInfo.Id)
                    && rd.ResourceInfo.Name.Equals(descriptor.ResourceInfo.Name, StringComparison.InvariantCultureIgnoreCase)).ToList();
            return list;
        }

        //protected override void SaveToFile(ResourceDescriptor descriptor, bool isNew, UserIdentity sender)
        //{
        //    if (!(descriptor is BackgroundImageDescriptor))
        //        base.SaveToFile(descriptor, isNew, sender);
        //}

        //protected override bool IsExistsWithSameName(ResourceDescriptor resourceDescriptor)
        //{
        //    IList<ResourceDescriptor> resources;
        //    if (resourceDescriptor.IsLocal && !string.IsNullOrEmpty(resourceDescriptor.PresentationUniqueName))
        //    {
        //        GetLocalSources(resourceDescriptor.PresentationUniqueName).TryGetValue(
        //            resourceDescriptor.ResourceInfo.Type, out resources);
        //    }
        //    else
        //    {
        //        GetGlobalSources().TryGetValue(resourceDescriptor.ResourceInfo.Type, out resources);
        //    }
        //    if (resources == null) return false;
        //    return resources.Any(rd => rd.ResourceInfo.Name.Equals(
        //                                 resourceDescriptor.ResourceInfo.Name,
        //                                 StringComparison.InvariantCultureIgnoreCase));
        //}

        protected override string GetGlobalSourceFolder()
        {
            return _configuration.GlobalSourceFolder;
        }

        protected override string ComposeResourceFullFileName(ResourceDescriptor descriptor, string resourceId)
        {
            string fullFileName = GetResourceFileName(descriptor, resourceId);
            if (_configuration.IsClient && File.Exists(fullFileName))
                return fullFileName;
            return (descriptor.IsLocal ? localResourcePrefix : globalResourcePrefix)
                + ((ResourceFileInfo)descriptor.ResourceInfo).ResourceFileList.Find(rfp => rfp.Id.Equals(resourceId)).ResourceFileName;
        }

        protected override ResourceDescriptor CreateGlobalResourceDescriptor(ResourceInfo resourceInfo)
        {
            return new ResourceDescriptor(false, null, resourceInfo);
        }

        //protected override string GetResourceInfoFullFileName(ResourceDescriptor descriptor)
        //{
        //    if (descriptor is BackgroundImageDescriptor) return null;
        //    return base.GetResourceInfoFullFileName(descriptor);
        //}

        protected override string GetPath(ResourceDescriptor descriptor)
        {
            string directory;
            if (descriptor.IsLocal)
            {
                directory = GetLocalSourceFolder(descriptor.PresentationUniqueName);
                if (!String.IsNullOrEmpty(descriptor.ResourceInfo.Type))
                {
                    directory = Path.Combine(directory, descriptor.ResourceInfo.Type);
                }
            }
            else
            {
                directory = Path.Combine(_configuration.GlobalSourceFolder, descriptor.ResourceInfo.Type);
            }
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            return Path.GetFullPath(directory);
        }

        protected override bool IsExistsInConfiguration(ResourceInfo resourceInfo)
        {
            // заглушка, временная хотя хз. сделано только из-за бг. если ресурс невидимый, то загружаем его в любом случае
            if (resourceInfo is INonVisibleResource) return true;
            // такой сорс должен быть прописан в конфигурации
            if (resourceInfo.IsHardware)
            {
                return
                    _configuration.ModuleConfiguration.SourceList.Where(source => source.IsHardware).Any(
                        source =>
                        source.Type.Equals(resourceInfo.Type, StringComparison.InvariantCultureIgnoreCase) &&
                        source.Name.Equals(resourceInfo.SourceName, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                return
                    _configuration.ModuleConfiguration.SourceList.Where(source => !source.IsHardware).Any(
                        source => source.Type.Equals(resourceInfo.Type, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        #endregion
    }
}