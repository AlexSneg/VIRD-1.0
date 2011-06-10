using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Hosts.Plugins.PowerPointPresentation.SystemModule.Config;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.PowerPointPresentation.SystemModule.Design
{
    [Serializable]
    [XmlType("PowerPoint")]
    public class PowerPointPresentationSourceDesign : SoftwareSource, IAspectLock
    {
        public PowerPointPresentationSourceDesign()
        {
            AspectLock = true;
        }

        [XmlIgnore]
        public override ResourceDescriptor ResourceDescriptor
        {
            get { return base.ResourceDescriptor; }
            set
            {
                base.ResourceDescriptor = value;
                if (null == value) return;
                PowerPointResourceInfo info = value.ResourceInfo as PowerPointResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа PowerPointResourceInfo");
            }
        }

        [Category("Настройки")]
        [DisplayName("Aspect Lock")]
        [TypeConverter(typeof(YesNoConverter))]
        [DefaultValue(true)]
        [XmlAttribute("AspectLock")]
        public bool AspectLock
        {
            get; set;
        }

        [XmlIgnore]
        [DisplayName("Количество слайдов")]
        public int NumberOfSlides
        {
            get
            {
                if (ResourceDescriptor == null || ResourceDescriptor.ResourceInfo == null) return 0;
                return ((PowerPointResourceInfo) ResourceDescriptor.ResourceInfo).NumberOfSlides;
            }
        }
    }
}
