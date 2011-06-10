using System;
using System.ComponentModel;
using System.Xml.Serialization;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.VDCTerminal.SystemModule.Design
{
    [Serializable]
    [XmlType("VDCTerminalSource")]
    public class VDCTerminalSourceDesign : HardwareSource
    {


        [XmlIgnore]
        public override ResourceDescriptor ResourceDescriptor
        {
            get { return base.ResourceDescriptor; }
            set
            {
                base.ResourceDescriptor = value;
                if (null == value) return;
                VDCTerminalResourceInfo info = value.ResourceInfo as VDCTerminalResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа VDCTerminalResourceInfo");
            }
        }
    }
}