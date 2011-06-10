using System;
using System.ComponentModel;
using System.Xml.Serialization;
using TechnicalServices.Persistence.CommonPersistence.Configuration;

namespace TechnicalServices.Persistence.SystemPersistence.Configuration
{

    [Serializable]
    public abstract class DisplayTypeUriCapture : DisplayTypeCapture
    {
        [XmlAttribute("AgentUID")]
        public string AgentUID
        {
            get { return _agentUID; }
            set { _agentUID = ValidationHelper.CheckLength(ValidationHelper.CheckIsNullOrEmpty(value, "AgentUID"), 128, "AgentUID"); }
        }

        private string _agentUID;

        /// <summary>
        /// Ширина дисплея (в метрах)
        /// Обязательный параметр
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("WidthM")]
        public decimal WidthM
        {
            get
            {
                return _sizeM.X;
            }
            set
            {
                _sizeM.X = value;
            }
        }

        /// <summary>
        /// Высота дисплея (в метрах)
        /// Обязательный параметр
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("HeightM")]
        public decimal HeightM
        {
            get
            {
                return _sizeM.Y;
            }
            set
            {
                _sizeM.Y = value;
            }
        }

        /// <summary>
        /// Линейный размер дисплея (в метрах)
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Размер (м)")]
        [XmlIgnore]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Size<decimal> SizeM
        {
            get { return _sizeM; }
            set { _sizeM = value; }
        }
        private Size<decimal> _sizeM = new Size<decimal>(0, 5);

        /// <summary>
        /// Удаленность дисплея от зрителей (в метрах) (1..32)
        /// Обязательный параметр
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Удаленность (м)")]
        [XmlAttribute("DistanceM")]
        public int DistanceM
        {
            get
            {
                return _DistanceM;
            }
            set
            {
                _DistanceM = ValidationHelper.CheckRange(value, 1, 32, "Удаленность (м)");
            }
        }
        int _DistanceM;

        /// Адрес компа.
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// !!!     ИСПОЛЬЗУЕТСЯ ТОЛЬКО ДЛЯ СЕРИАЛИЗАЦИИ В XML              !!! 
        /// !!! Увижу использование, кроме оговоренных случаев, ЯЙЦА ОТОРВУ !!! 
        /// !!! Имя атрибута соответствует имени свойства, которое          !!!
        /// !!! необходимо использовать                                     !!!
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        [XmlAttribute("Uri")]
        [DisplayName("URI")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NotNullStringConverter, TechnicalServices.Common")]
        public string Address { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public Uri Uri
        {
            get { return new Uri(Address); }
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return false; }
        }
    }
}