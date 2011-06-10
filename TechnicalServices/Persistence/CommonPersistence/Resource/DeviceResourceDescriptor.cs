using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TechnicalServices.Persistence.SystemPersistence.Resource
{
    [Serializable]
    [DataContract]
    public class DeviceResourceDescriptor : ResourceDescriptorAbstract
    {
        public DeviceResourceDescriptor(ResourceInfo resourceInfo) : base(resourceInfo)
        {}
    }
}
