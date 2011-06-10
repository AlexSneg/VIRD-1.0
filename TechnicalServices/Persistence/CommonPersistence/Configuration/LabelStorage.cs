using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace TechnicalServices.Persistence.CommonPersistence.Configuration
{
    [Serializable]
    [XmlRoot(Namespace = "urn:labelstorage-schema", ElementName = "LabelStorage")]
    public class LabelStorage: List<Label>
    {
        
    }
}
