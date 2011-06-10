using System;
using System.ComponentModel;
using System.Xml.Serialization;

using Hosts.Plugins.VDCTerminal.SystemModule.Design;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.VDCTerminal.SystemModule.Config
{
    [Serializable]
    [XmlType("VDCTerminalDevice")]
    public class VDCTerminalDeviceConfig : DeviceTypeAsSource
    {
        public VDCTerminalDeviceConfig()
        {
        }

        public VDCTerminalDeviceConfig(string name)
        {
            Name = name;
            Type = "Терминал ВКС";
        }

        public override Device CreateNewDevice()
        {
            return new VDCTerminalDeviceDesign { Type = this };
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
