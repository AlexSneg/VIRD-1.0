using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Hosts.Plugins.Computer.SystemModule.Config;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.Computer.SystemModule.Design
{
    [Serializable]
    [XmlType("ComputerDisplay")]
    public class ComputerDisplayDesign : ActiveDisplay
    {
        public ComputerDisplayDesign()
        {
            IsVideoWall = false;
        }

        protected override Window CreateWindowProtected(Source source, Slide slide)
        {
            return new ComputerWindow();
        }

        /// <summary>
        /// Сетевой адрес компьютера
        /// Получаем из Конфигуратора
        /// По умолчанию: localhost
        /// </summary>
        [Category("Настройки")]
        [DisplayName("URI")]
        [XmlIgnore]
        public string Address
        {
            get { return ((ComputerDisplayConfig)Type).Address; }
        }

        /// <summary>
        /// Частота вертикальной развертки (в герцах) (1..256)
        /// Получаем из Конфигуратора
        /// По умолчанию: 60
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Развертка (Гц)")]
        [XmlIgnore]
        public int RefreshRate
        {
            get { return ((ComputerDisplayConfig)Type).RefreshRate; }
        }

        /// <summary>
        /// Ширина дисплея (в метрах)
        /// Получаем из Конфигуратора
        /// По умолчанию: 0.5
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public decimal WidthM
        {
            get { return ((ComputerDisplayConfig)Type).WidthM; }
        }

        /// <summary>
        /// Высота дисплея (в метрах)
        /// Получаем из Конфигуратора
        /// По умолчанию: 0.4
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public decimal HeightM
        {
            get { return ((ComputerDisplayConfig)Type).HeightM; }
        }

        /// <summary>
        /// Линейный размер дисплея (в метрах)
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Размер (м)")]
        [XmlIgnore]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Size<decimal> SizeM
        {
            get { return ((ComputerDisplayConfig)Type).SizeM; }
        }

        /// <summary>
        /// Удаленность дисплея от зрителей (в метрах) (1..32)
        /// Получаем из Конфигуратора
        /// По умолчанию: 2
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Удаленность (м)")]
        [XmlIgnore]
        public int DistanceM
        {
            get { return ((ComputerDisplayConfig)Type).DistanceM; }
        }
    }
}