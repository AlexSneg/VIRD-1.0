using System;
using System.ComponentModel;
using System.Xml.Serialization;

using Hosts.Plugins.StandardSource.SystemModule.Design;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.StandardSource.SystemModule.Config
{
    [Serializable]
    [XmlType("StandardSourceDevice")]
    public class StandardSourceDeviceConfig : DeviceTypeAsSource
    {
        public StandardSourceDeviceConfig()
        {
        }

        public StandardSourceDeviceConfig(string name)
        {
            Name = name;
            Type = "Стандартный источник изображения";
        }

        public override Device CreateNewDevice()
        {
            return new StandardSourceDeviceDesign { Type = this };
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return true; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool Visible
        {
            get { return false; }
        }
    }
}
