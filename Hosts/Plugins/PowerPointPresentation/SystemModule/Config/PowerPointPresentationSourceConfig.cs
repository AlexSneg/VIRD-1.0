using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Hosts.Plugins.PowerPointPresentation.Common;
using Hosts.Plugins.PowerPointPresentation.SystemModule.Design;
using TechnicalServices.Exceptions;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.PowerPointPresentation.SystemModule.Config
{
    [Serializable]
    [XmlType("PowerPoint")]
    public class PowerPointPresentationSourceConfig : SourceType
    {
        public PowerPointPresentationSourceConfig()
        {

        }

        public PowerPointPresentationSourceConfig(string name)
        {
            Name = name;
            Type = "Презентация PowerPoint";
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return false; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsSupportPreview
        {
            get { return true; }
        }

        public override Source CreateNewSource(Slide slide, ModuleConfiguration moduleConfiguration, Dictionary<string, IList<DeviceResourceDescriptor>> resources, Display display)
        {
            PowerPointPresentationSourceDesign source = new PowerPointPresentationSourceDesign() { Type = this };
            return source;
        }

        protected override ResourceInfo CreateNewResourceInfoProtected()
        {
            return new PowerPointResourceInfo();
        }

        public override DeviceType CreateDeviceType()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract]
    [Serializable]
    public class PowerPointResourceInfo : ResourceFileInfo
    {
        public override string Filter
        {
            get
            {
                return @"Презентация PowerPoint(*.ppt)|*.ppt";
            }
        }

        [DataMember]
        [ReadOnly(true)]
        [DisplayName("Количество слайдов")]
        [XmlAttribute("NumberOfSlides")]
        public int NumberOfSlides
        {
            get;
            set;
        }

        protected override void Init(ResourceFileProperty property, ResourceFileAttribute resourceFileAttribute)
        {
            try
            {
                if (string.IsNullOrEmpty(property.ResourceFullFileName)) return;
                using (PowerPoint powerPoint = PowerPoint.OpenFile(property.ResourceFullFileName))
                {
                    NumberOfSlides = powerPoint.NumberOfSlides;
                }
            }
            catch(Exception ex)
            {
                throw new InvalidResourceException(string.Format("Файл {0} не является презентацией PowerPoint", Path.GetFileName(property.ResourceFullFileName)), ex);
            }
        }
    }
}
