using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Hosts.Plugins.DVDPlayer.SystemModule.Design;

using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Common;

namespace Hosts.Plugins.DVDPlayer.SystemModule.Config
{
	[Serializable]
	[XmlType("DVDPlayerSource")]
    public class DVDPlayerSourceConfig : HardwareSourceType, ICollectionItemValidation
	{
		public DVDPlayerSourceConfig()
		{
		}

		public DVDPlayerSourceConfig(string name)
		{
			Name = name;
            Type = "DVD";
        }

        public override Source CreateNewSource(Slide slide, ModuleConfiguration moduleConfiguration, Dictionary<String, IList<DeviceResourceDescriptor>> resources, Display display)
		{
			Source source = new DVDPlayerSourceDesign { Type = this };
			DVDPlayerDeviceConfig deviceConfig = (DVDPlayerDeviceConfig)moduleConfiguration.DeviceList.SingleOrDefault(
				 dt => dt.GetType() == typeof(DVDPlayerDeviceConfig) && dt.UID == this.UID);
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
			return new DVDPlayerResourceInfo();
		}

		[XmlIgnore]
		[Browsable(false)]
		public override bool IsHardware
		{
			get { return true; }
		}

        public override DeviceType CreateDeviceType()
        {
            return new DVDPlayerDeviceConfig(Name);
        }

        #region ICollectionItemValidation Members

        bool ICollectionItemValidation.ValidateItem(out string errorMessage)
        {
            errorMessage = "OK";
            return true;
        }

        #endregion
    }
}