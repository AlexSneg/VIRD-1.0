using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.StandardSource.SystemModule.Design
{
    [DataContract]
    [Serializable]
    public class StandardSourceResourceInfo : ResourceInfoForHardwareSource
    {
        [Browsable(false)]
        [XmlIgnore]
        public override DeviceType DeviceType
        {
            get { return base.DeviceType; }
            set { base.DeviceType = value; }
        }
    }
}
