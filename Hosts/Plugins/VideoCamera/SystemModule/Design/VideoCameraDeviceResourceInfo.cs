using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.VideoCamera.SystemModule.Design
{
    [DataContract]
    [Serializable]
    public class PresetStore
    {
        public PresetStore()
        {
        }

        public PresetStore(int preset, int pan, int tilt, decimal zoom)
        {
            Preset = preset;
            Pan = pan;
            Tilt = tilt;
            Zoom = zoom;
        }

        [DataMember]
        [XmlAttribute("Preset")]
        public int Preset { get; set; }
        [DataMember]
        [XmlAttribute("Pan")]
        public int Pan { get; set; }
        [DataMember]
        [XmlAttribute("Tilt")]
        public int Tilt { get; set; }
        [DataMember]
        [XmlAttribute("Zoom")]
        public decimal Zoom { get; set; }
    }


    [DataContract]
    [Serializable]
    public class VideoCameraDeviceResourceInfo : ResourceInfo
    {
        [DataMember]
        private List<PresetStore> _presetStore = new List<PresetStore>();

        [XmlArray("PresetStore")]
        [TypeConverter(typeof(TechnicalServices.Common.TypeConverters.CollectionNameConverter))]
        public List<PresetStore> PresetStores
        {
            get { return _presetStore; }
        }
    }
}
