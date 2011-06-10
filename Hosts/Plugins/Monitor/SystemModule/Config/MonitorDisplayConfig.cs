using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Hosts.Plugins.Monitor.SystemModule.Design;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.CommonPersistence.Configuration;

namespace Hosts.Plugins.Monitor.SystemModule.Config
{
    [Serializable]
    [XmlType("Monitor")]
    public class MonitorDisplayConfig : PassiveDisplayType
    {
        public MonitorDisplayConfig(string name) : this()
        {
            Name = name;
            Type = "Дисплей пассивный";
            Width = 1024;
            Height = 768;
        }

        public MonitorDisplayConfig()
        {
        }

        public override Display CreateNewDisplay()
        {
            return new MonitorDisplayDesign { Type = this };
        }

        public override Mapping CreateMapping(SourceType source)
        {
            // На видеопанеле можно показывать только аппаратные источники
            if (!source.IsHardware) return null;

            return new Mapping() {Source = source};
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return true; }
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
            {
                _refreshRate =  ValidationHelper.CheckRange(value, 1, 256, "Развертка (Гц)"); 
            }
        }
        private int _refreshRate = 60;

        [XmlIgnore]
        [Browsable(false)]
        public override bool SupportsMultiWindow
        {
            get
            {
                return false;
            }
        }
    }
}