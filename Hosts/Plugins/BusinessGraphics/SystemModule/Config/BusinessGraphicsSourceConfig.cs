using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Hosts.Plugins.BusinessGraphics.SystemModule.Design;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Config
{
    [Serializable]
    [XmlType("BusinessGraphics")]
    public class BusinessGraphicsSourceConfig : SourceType
    {
        public BusinessGraphicsSourceConfig()
        {
            
        }

        public BusinessGraphicsSourceConfig(string name)
        {
            Name = name;
            Type = "Бизнес-графика";
        }

        public override Source CreateNewSource(Slide slide, ModuleConfiguration moduleConfiguration, Dictionary<String, IList<DeviceResourceDescriptor>> resources, Display display)
        {
            return new BusinessGraphicsSourceDesign 
            { 
                Type = this, 
                H = Convert.ToDouble(((DisplayTypeUriCapture)display.Type).HeightM),
                B = Convert.ToDouble(((DisplayTypeUriCapture)display.Type).WidthM),
                L = Convert.ToDouble(((DisplayTypeUriCapture)display.Type).DistanceM),
                Bp = Convert.ToDouble(((DisplayTypeUriCapture)display.Type).Width),
                Hp = Convert.ToDouble(((DisplayTypeUriCapture)display.Type).Height)
            };
        }

        protected override ResourceInfo CreateNewResourceInfoProtected()
        {
            return new BusinessGraphicsResourceInfo();
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return false; }
        }

        public override DeviceType CreateDeviceType()
        {
            throw new NotImplementedException();
        }
    }
}