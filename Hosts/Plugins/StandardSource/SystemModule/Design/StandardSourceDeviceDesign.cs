using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Xml.Serialization;

namespace Hosts.Plugins.StandardSource.SystemModule.Design
{
    [Serializable]
    [XmlType("StandardSourceDevice")]
    public class StandardSourceDeviceDesign : DeviceAsSource
    {
    }
}
