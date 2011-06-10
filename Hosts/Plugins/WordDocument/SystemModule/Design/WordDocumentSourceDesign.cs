using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Hosts.Plugins.WordDocument.SystemModule.Config;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Persistence.CommonPersistence.Configuration;


namespace Hosts.Plugins.WordDocument.SystemModule.Design
{
    [Serializable]
    [XmlType("WordDocument")]
    public class WordDocumentSourceDesign : SoftwareSource
    {
        public WordDocumentSourceDesign()
        {
            
        }

        [XmlIgnore]
        public override ResourceDescriptor ResourceDescriptor
        {
            get { return base.ResourceDescriptor; }
            set
            {
                base.ResourceDescriptor = value;
                if (null == value) return;
                WordResourceInfo info = value.ResourceInfo as WordResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа WordResourceInfo");
            }
        }

        [XmlIgnore]
        [DisplayName("Количество страниц")]
        public int NumberOfPages
        {
            get
            {
                if (ResourceDescriptor == null || ResourceDescriptor.ResourceInfo == null) return 0;
                return ((WordResourceInfo)ResourceDescriptor.ResourceInfo).NumberOfPages;
            }
        }

        int startPage = 1;
        [DisplayName("Номер Страницы")]
        [DefaultValue(1)]
        [XmlAttribute("StartPage")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.Int32ConverterLocalized, TechnicalServices.Common")]
        public int StartPage
        {
            get { return startPage; }
            set 
            {
                startPage = ValidationHelper.CheckRange(value, 1, int.MaxValue, "Номер Страницы");
            }
        }

        int startLine = 1;
        [DisplayName("Номер Строки")]
        [DefaultValue(1)]
        [XmlAttribute("StartLine")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.Int32ConverterLocalized, TechnicalServices.Common")]
        public int StartLine
        {
            get { return startLine; }
            set
            {
                startLine = ValidationHelper.CheckRange(value, 1, int.MaxValue, "Номер Строки");
            }
        }

        int startZoom = 100;
        [DisplayName("Zoom")]
        [DefaultValue(100)]
        [XmlAttribute("Zoom")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.Int32ConverterLocalized, TechnicalServices.Common")]
        public int StartZoom
        {
            get { return startZoom; }
            set 
            {
                startZoom = ValidationHelper.CheckRange(value, 10, 500, "Zoom");
            }
        }
    }
}
