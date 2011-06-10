using System;
using System.Xml.Serialization;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.DVDPlayer.SystemModule.Design
{
    [Serializable]
    [XmlType("DVDPlayerSource")]
    public class DVDPlayerSourceDesign : HardwareSource
    {
        [XmlIgnore]
        public override ResourceDescriptor ResourceDescriptor
        {
            get { return base.ResourceDescriptor; }
            set
            {
                base.ResourceDescriptor = value;
                if (null == value) return;
                DVDPlayerResourceInfo info = value.ResourceInfo as DVDPlayerResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа DVDPlayerResourceInfo");
            }
        }
    }
}