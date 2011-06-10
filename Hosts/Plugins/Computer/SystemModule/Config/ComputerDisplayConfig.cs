using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Hosts.Plugins.Computer.SystemModule.Design;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.CommonPersistence.Configuration;

namespace Hosts.Plugins.Computer.SystemModule.Config
{
    [Serializable]
    [XmlType("ComputerDisplay")]
    public class ComputerDisplayConfig : DisplayTypeUriCapture
    {
        public ComputerDisplayConfig(string name)
            : this()
        {
            Name = name;
            Type = "Дисплей активный";
            AgentUID = "ComputerName";
        }

        public ComputerDisplayConfig()
        {
            UID = -1;
            Address = String.Empty;
            DistanceM = 2;
            HeightM = 0.4m;
            WidthM = 0.5m;
            Width = 1024;
            Height = 768;
        }

        public override Display CreateNewDisplay()
        {
            return new ComputerDisplayDesign { Type = this };
        }

        public override Mapping CreateMapping(SourceType source)
        {
            // На обычном компе нельзя показывать аппаратные источники
            if (source.IsHardware) return null;

            return new Mapping() { Source = source };
        }

        [Category("Общие параметры")]
        [DisplayName("ID оборудования")]
        [XmlAttribute("UID")]
        [ReadOnly(true)]
        public override int UID
        {
            get { return base.UID; }
            set { base.UID = value; }
        }

        /// <summary>
        /// Частота вертикальной развертки (в герцах) (1..256)
        /// Обязательный параметр
        /// По умолчанию: 60
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Развертка (Гц)")]
        [DefaultValue(60)]
        [XmlAttribute("RefreshRate")]
        public int RefreshRate
        {
            get { return _refreshRate; }
            set 
            { _refreshRate = ValidationHelper.CheckRange(value, 1, 256, "Развертка (Гц)"); ; 
            }
        }
        private int _refreshRate = 60;
        /*
                /// <summary>
                /// Ширина дисплея (в метрах)
                /// Обязательный параметр
                /// По умолчанию: 0.5
                /// </summary>
                [Browsable(false)]
                [DefaultValue(typeof(decimal), "0.5")]
                [XmlAttribute("WidthM")]
                public decimal WidthM
                {
                    get { return _widthM; }
                    set { _widthM = value; }
                }
                private decimal _widthM = 0.5m;

                /// <summary>
                /// Высота дисплея (в метрах)
                /// Обязательный параметр
                /// По умолчанию: 0.4
                /// </summary>
                [Browsable(false)]
                [DefaultValue(typeof(decimal), "0.4")]
                [XmlAttribute("HeightM")]
                public decimal HeightM
                {
                    get { return _heightM; }
                    set { _heightM = value; }
                }
                private decimal _heightM = 0.4m;

                /// <summary>
                /// Линейный размер дисплея (в метрах)
                /// </summary>
                [Category("Настройки")]
                [DisplayName("Размер (м)")]
                [XmlIgnore]
                [TypeConverter(typeof(ExpandableObjectConverter))]
                public Size<decimal> SizeM
                {
                    get { return new Size<decimal> { X = WidthM, Y = HeightM }; }
                    set
                    {
                        WidthM = value.X;
                        HeightM = value.Y;
                    }
                }

                /// <summary>
                /// Удаленность дисплея от зрителей (в метрах) (1..32)
                /// Обязательный параметр
                /// По умолчанию: 2
                /// </summary>
                [Category("Настройки")]
                [DisplayName("Удаленность (м)")]
                [DefaultValue(2)]
                [XmlAttribute("DistanceM")]
                public int DistanceM
                {
                    get { return _distanceM; }
                    set { _distanceM = value; }
                }
                private int _distanceM = 2;
        */
    }
}