using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Entity;
using System.ComponentModel;

namespace Hosts.Plugins.VideoCamera.SystemModule.Design
{

    [DataContract]
    [Serializable]
    public class VideoCameraResourceInfo : ResourceInfoForHardwareSource
    {
    }
}
