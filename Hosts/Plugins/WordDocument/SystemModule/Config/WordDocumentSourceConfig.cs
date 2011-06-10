using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Hosts.Plugins.WordDocument.Common;
using Hosts.Plugins.WordDocument.SystemModule.Design;
using TechnicalServices.Exceptions;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.WordDocument.SystemModule.Config
{
    [Serializable]
    [XmlType("WordDocument")]
    public class WordDocumentSourceConfig : SourceType
    {
        public WordDocumentSourceConfig()
        {

        }

        public WordDocumentSourceConfig(string name)
        {
            Name = name;
            Type = "Документ Word";
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return false; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsSupportPreview
        {
            get { return true; }
        }

        public override Source CreateNewSource(Slide slide, ModuleConfiguration moduleConfiguration, Dictionary<string, IList<DeviceResourceDescriptor>> resources, Display display)
        {
            WordDocumentSourceDesign source = new WordDocumentSourceDesign() { Type = this };
            return source;
        }

        protected override ResourceInfo CreateNewResourceInfoProtected()
        {
            return new WordResourceInfo();
        }

        public override DeviceType CreateDeviceType()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract]
    [Serializable]
    public class WordResourceInfo : ResourceFileInfo
    {
        public override string Filter
        {
            get
            {
                return @"Документ Word(*.doc)|*.doc";
            }
        }

        [DataMember]
        [ReadOnly(true)]
        [DisplayName("Количество страниц")]
        [XmlAttribute("NumberOfPages")]
        public int NumberOfPages
        {
            get;
            set;
        }

        protected override void Init(ResourceFileProperty property, ResourceFileAttribute resourceFileAttribute)
        {
            try
            {
                if (string.IsNullOrEmpty(property.ResourceFullFileName)) return;
                using (Hosts.Plugins.WordDocument.Common.WordDocument Word = Hosts.Plugins.WordDocument.Common.WordDocument.OpenFile(property.ResourceFullFileName))
                {
                    NumberOfPages = Word.NumberOfPages;
                }
            }
            catch(Exception ex)
            {
                throw new InvalidResourceException(string.Format("Файл {0} не является документом Word", Path.GetFileName(property.ResourceFullFileName)), ex);
            }
        }
    }
}
