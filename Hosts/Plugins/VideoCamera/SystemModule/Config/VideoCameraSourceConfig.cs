using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;

using Hosts.Plugins.VideoCamera.SystemModule.Design;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.VideoCamera.SystemModule.Config
{
    [Serializable]
    [XmlType("VideoCameraSource")]
    public class VideoCameraSourceConfig : HardwareSourceType
    {
        public VideoCameraSourceConfig()
        {
            
        }

		 public VideoCameraSourceConfig(string name)
		 {
			 Name = name;
             Type = "Видеокамера";
         }

         public override Source CreateNewSource(Slide slide, ModuleConfiguration moduleConfiguration, Dictionary<String, IList<DeviceResourceDescriptor>> resources, Display display)
        {
            Source source = new VideoCameraSourceDesign {Type = this};
            VideoCameraDeviceConfig deviceConfig = (VideoCameraDeviceConfig)moduleConfiguration.DeviceList.SingleOrDefault(
                dt=>dt.GetType() == typeof(VideoCameraDeviceConfig) && dt.UID == this.UID);
            if (deviceConfig != null)
            {
                Device device =
                    slide.DeviceList.SingleOrDefault(
                        dev => dev.Type.Name.Equals(deviceConfig.Name) && dev.Type.Type.Equals(deviceConfig.Type));
                if (device == null)
                {
                    device = deviceConfig.CreateNewDevice(resources);
                    slide.DeviceList.Add(device);
                }
                source.Device = device;
            }
            return source;
        }

        protected override ResourceInfo CreateNewResourceInfoProtected()
        {
            return new VideoCameraResourceInfo();
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return true; }
        }

        public override DeviceType CreateDeviceType()
        {
            return new VideoCameraDeviceConfig(Name);
        }
    }
}
