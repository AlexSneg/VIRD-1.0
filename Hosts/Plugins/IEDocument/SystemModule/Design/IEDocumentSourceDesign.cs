using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Hosts.Plugins.IEDocument.SystemModule.Config;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Persistence.CommonPersistence.Configuration;

namespace Hosts.Plugins.IEDocument.SystemModule.Design
{
    [Serializable]
    [XmlType("IEDocument")]
    public class IEDocumentSourceDesign : Source //SoftwareSource //, IAspectLock
    {
        public IEDocumentSourceDesign()
        {
            //AspectLock = true;
        }

        [Browsable(false)]
        [XmlIgnore]
        public override string Model
        {
            get { return base.Model; }
        }

        [XmlIgnore]
        public override ResourceDescriptor ResourceDescriptor
        {
            get { return base.ResourceDescriptor; }
            set
            {
                base.ResourceDescriptor = value;
                if (null == value) return;
                IEResourceInfo info = value.ResourceInfo as IEResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа IEResourceInfo");
            }
        }

        //TO DO: Удалить
        //[Category("Настройки")]
        //[DisplayName("Aspect Lock")]
        //[TypeConverter(typeof(YesNoConverter))]
        //[DefaultValue(true)]
        //[XmlAttribute("AspectLock")]
        //public bool AspectLock
        //{
        //    get;
        //    set;
        //}

        //[XmlIgnore]
        //[DisplayName("Количество слайдов")]
        //public int NumberOfSlides
        //{
        //    get
        //    {
        //        if (ResourceDescriptor == null || ResourceDescriptor.ResourceInfo == null) return 0;
        //        return ((IEResourceInfo)ResourceDescriptor.ResourceInfo).NumberOfSlides;
        //    }
        //}

        /// <summary>
        /// Сетевой адрес компьютера (IP, порт и т.п.)
        /// Получаем из ResourceInfo
        /// </summary>
        [Category("Настройки")]
        [DisplayName("URL")]
        [XmlIgnore]
        public string Url
        {
            get { return ((IEResourceInfo)ResourceDescriptor.ResourceInfo).Url; }
        }

        int zoom = 100;
        [Category("Настройки")]
        [DisplayName("Zoom")]
        [DefaultValue(100)]
        [XmlAttribute("Zoom")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.Int32ConverterLocalized, TechnicalServices.Common")]
        public int Zoom
        {
            get { return zoom; }
            set
            {
                zoom = ValidationHelper.CheckRange(value, 10, 500, "Zoom");
            }
        }
    }
}
