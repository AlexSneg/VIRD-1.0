using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Hosts.Plugins.ArcGISMap.SystemModule.Design;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.ArcGISMap.SystemModule.Config
{
    [Serializable]
    [XmlType("ArcGISMap")]
    public class ArcGISMapSourceConfig : SourceType
    {
        public ArcGISMapSourceConfig()
        {
            
        }

        public ArcGISMapSourceConfig(string name)
        {
            Name = name;
            Type = "ArcGIS-карта";
        }

        public override Source CreateNewSource(Slide slide, ModuleConfiguration moduleConfiguration, Dictionary<String, IList<DeviceResourceDescriptor>> resources, Display display)
        {
            return new ArcGISMapSourceDesign 
            { 
                Type = this
            };
        }

        protected override ResourceInfo CreateNewResourceInfoProtected()
        {
            return new ArcGISMapResourceInfo();
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