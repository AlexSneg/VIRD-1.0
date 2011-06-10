using System;
using System.ComponentModel;
using System.Xml.Serialization;

using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace Hosts.Plugins.Jupiter.SystemModule.Config
{
    [Serializable]
    [XmlType("JupiterMapping")]
    public class JupiterMapping : Mapping
    {
        [XmlAttribute("VideoIn")]
        [DefaultValue(0)]
        public int VideoIn { get; set; }
    }
}