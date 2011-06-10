using System;
using System.Xml.Serialization;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.VideoCamera.SystemModule.Design
{
    [Serializable]
    [XmlType("VideoCameraSource")]
    public class VideoCameraSourceDesign : HardwareSource
    {
        [XmlIgnore]
        public override ResourceDescriptor ResourceDescriptor
        {
            get { return base.ResourceDescriptor; }
            set
            {
                base.ResourceDescriptor = value;
                if (null == value) return;
                VideoCameraResourceInfo info = value.ResourceInfo as VideoCameraResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа VideoCameraResourceInfo");
            }
        }
    }
}
