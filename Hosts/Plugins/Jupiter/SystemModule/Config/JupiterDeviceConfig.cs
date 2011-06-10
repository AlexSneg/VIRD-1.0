using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Hosts.Plugins.Jupiter.SystemModule.Design;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.Jupiter.SystemModule.Config
{
    [Serializable]
    [XmlType("JupiterDevice")]
    public class JupiterDeviceConfig : DeviceType
    {
        public JupiterDeviceConfig()
        {
        }

        public JupiterDeviceConfig(string name)
            : this()
        {
            Name = name;
            Type = "Видеостена";
        }

        public override Device CreateNewDevice()
        {
            return new JupiterDeviceDesign {Type = this};
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return true;}
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool Visible
        {
            get { return true; }
        }
    }
}