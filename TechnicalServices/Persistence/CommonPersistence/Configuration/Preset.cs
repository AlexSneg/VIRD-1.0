using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace TechnicalServices.Persistence.CommonPersistence.Configuration
{
    [Serializable]
    public class Preset
    {
        private readonly List<EquipmentCommand> _commandList = new List<EquipmentCommand>();
        private readonly List<NameValuePair> _propertyList = new List<NameValuePair>();

        [XmlElement("Name", typeof(string))]
        public string Name { get; set; }

        [XmlArray("PropertyList")]
        public List<NameValuePair> PropertyList
        {
            get { return _propertyList; }
        }

        [XmlArray("CommandList")]
        public List<EquipmentCommand> CommandList
        {
            get { return _commandList; }
        }

        static public Preset[] GetPresetting(Assembly asmbl)
        {
            List<Preset> result = new List<Preset>();
            foreach (string item in asmbl.GetManifestResourceNames())
            {
                ManifestResourceInfo resInfo = asmbl.GetManifestResourceInfo(item);
                if (resInfo == null) continue;
                using (Stream stream = asmbl.GetManifestResourceStream(item))
                {
                    if (stream == null) continue;
                    if (item.Contains(".Resource.") && item.EndsWith(".xml"))
                    {
                        try
                        {
                            Preset preset;
                            XmlSerializer serializer = new XmlSerializer(typeof(Preset));
                            using (XmlReader reader = XmlReader.Create(stream))
                                preset = (Preset)serializer.Deserialize(reader);
                            result.Add(preset);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            return result.ToArray();
        }

        static public string[] GetPresettingNames(Preset[] list)
        {
            List<string> result = new List<string>();
            foreach (Preset preset in list)
            {
                if (String.IsNullOrEmpty(preset.Name)) continue;
                result.Add(preset.Name);
            }
            result.Sort();
            return result.ToArray();
        }

        static public string GetDefaultPresetting(Preset[] list)
        {
            foreach (Preset preset in list)
            {
                if (String.IsNullOrEmpty(preset.Name))
                    return preset.Name;
            }
            return null;
        }

    }

    [Serializable]
    public class NameValuePair
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }
    }

}