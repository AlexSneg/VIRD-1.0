using System;
using System.ComponentModel;
using System.Xml.Serialization;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.Jupiter.SystemModule.Design
{
    [Serializable]
    [XmlType("JupiterDevice")]
    public class JupiterDeviceDesign : Device
    {
        /// <summary>
        /// Состояние (Вкл/Выкл)
        /// Обязательный параметр
        /// По умолчанию: True
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Состояние")]
        [DefaultValue(true)]
        [TypeConverter(typeof(OnOffConverter))]
        [XmlAttribute("OnOffState")]
        public bool OnOffState
        {
            get { return _onOffState; }
            set { _onOffState = value; }
        }
        private bool _onOffState = true;

        /// <summary>
        /// Режим Picture mute (Да = черный экран/Нет)
        /// Обязательный параметр
        /// По умолчанию: False
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Picture mute")]
        [DefaultValue(false)]
        [TypeConverter(typeof(YesNoConverter))]
        [XmlAttribute("PictureMute")]
        public bool PictureMute { get; set; }

        /// <summary>
        /// Яркость (0,20,40,60,80,100)
        /// Обязательный параметр
        /// По умолчанию: 60
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Яркость")]
        [DefaultValue(60)]
        [TypeConverter(typeof(JupiterBrightnessConverter))]
        [XmlAttribute("Brightness")]
        public int Brightness
        {
            get { return _brightness; }
            set { _brightness = value; }
        }
        private int _brightness = 60;

        /// <summary>
        /// Состояние видеостены (в порядке или есть неполадка)
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public bool CurrentState
        {
            get { return true; }
        }
    }
}