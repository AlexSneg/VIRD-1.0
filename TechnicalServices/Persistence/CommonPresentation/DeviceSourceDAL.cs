using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Persistence.CommonPresentation
{
    public class DeviceSourceDAL : AbstractSourceDAL<DeviceResourceDescriptor>, IDeviceSourceDAL
    {
        public DeviceSourceDAL(IConfiguration configuration, bool isStandalone) : base(configuration, isStandalone)
        {
            if (!Directory.Exists(_configuration.DeviceResourceFolder))
                throw new ApplicationException(String.Format("Нет директории: {0}", _configuration.DeviceResourceFolder));

            //// маппинг типов сорсов с типами ResourceInfo
            //foreach (DeviceType deviceType in _configuration.ModuleConfiguration.DeviceList)
            //{
            //    IEnumerable<IModule> modules = _configuration.ModuleList.Where(
            //        module => module.SystemModule.Configuration.GetDevice().Contains(deviceType.GetType()));
            //    if (modules.Count() == 0) continue;
            //    ResourceInfoExtraTypeDic[deviceType.Type] =
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
            foreach (DeviceType deviceType in _configuration.ModuleConfiguration.DeviceList)
            {
                if (!deviceType.IsHardware) continue;
                ResourceInfo resourceInfo = deviceType.CreateNewResourceInfo(_configuration.ModuleConfiguration);
                if (resourceInfo == null) continue;
                if (resourceInfos.Exists(
                    ri => ri.Type.Equals(resourceInfo.Type, StringComparison.InvariantCultureIgnoreCase)
                    && ri.Name.Equals(resourceInfo.Name, StringComparison.InvariantCultureIgnoreCase))) continue;
                resourceInfos.Add(resourceInfo);
            }
            List<ResourceInfo> existed = new List<ResourceInfo>(resourceInfos.Count);
            foreach (ResourceInfo resourceInfo in resourceInfos)
            {
                DeviceResourceDescriptor resourceDescriptor = new DeviceResourceDescriptor(resourceInfo);
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
                    new DeviceResourceDescriptor(resourceInfo));
                if (!string.IsNullOrEmpty(file))
                    resourceInfo.SaveToFile(file, ExtraTypes);
            }
        }
        #endregion


        #region Overrides of AbstractSourceDAL<DeviceResourceDescriptor>

        protected override string GetPath(DeviceResourceDescriptor descriptor)
        {
            string directory = Path.Combine(GetGlobalSourceFolder(), descriptor.ResourceInfo.Type);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            return Path.GetFullPath(directory);
        }

        protected override string GetGlobalSourceFolder()
        {
            return _configuration.DeviceResourceFolder;
        }

        protected override string ComposeResourceFullFileName(DeviceResourceDescriptor descriptor, string resourceId)
        {
            string fullFileName = GetResourceFileName(descriptor, resourceId);
            if (_configuration.IsClient && File.Exists(fullFileName))
                return fullFileName;
            return globalResourcePrefix
                + ((ResourceFileInfo)descriptor.ResourceInfo).ResourceFileList.Find(rfp => rfp.Id.Equals(resourceId)).ResourceFileName;
        }

        protected override DeviceResourceDescriptor CreateGlobalResourceDescriptor(ResourceInfo resourceInfo)
        {
            return new DeviceResourceDescriptor(resourceInfo);
        }

        protected override DeviceResourceDescriptor CreateResourceDescriptor(DeviceResourceDescriptor descriptor, ResourceInfo resourceInfo)
        {
            return new DeviceResourceDescriptor(resourceInfo);
        }

        public override List<DeviceResourceDescriptor> SearchByName(DeviceResourceDescriptor descriptor)
        {
            IList<DeviceResourceDescriptor> deviceResourceDescriptors;
            if (!GetGlobalSources().TryGetValue(descriptor.ResourceInfo.Type, out deviceResourceDescriptors))
                return new List<DeviceResourceDescriptor>();
            List<DeviceResourceDescriptor> list = deviceResourceDescriptors.Where(
                rd => !rd.ResourceInfo.Id.Equals(descriptor.ResourceInfo.Id, StringComparison.InvariantCultureIgnoreCase) 
                    && rd.ResourceInfo.Name.Equals(descriptor.ResourceInfo.Name, StringComparison.InvariantCultureIgnoreCase)).ToList();
            return list;
        }

        protected override bool IsExistsInConfiguration(ResourceInfo resourceInfo)
        {
            return
                _configuration.ModuleConfiguration.DeviceList.Any(
                    device =>
                    device.Type.Equals(resourceInfo.Type, StringComparison.InvariantCultureIgnoreCase) &&
                    device.Name.Equals(resourceInfo.DeviceName, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion

        public event Action<double, string> OnUploadSpeed;
        public void UploadSpeed(double speed, string display)
        {
            if (OnUploadSpeed != null)
            {
                OnUploadSpeed(speed, display);
            }
        }

    }
}
