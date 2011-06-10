using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Hosts.Plugins.Monitor.SystemModule.Config;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.Monitor.SystemModule.Design
{
    [Serializable]
    [XmlType("Monitor")]
    public class MonitorDisplayDesign : PassiveDisplay
    {
        protected override Window CreateWindowProtected(Source source, Slide slide)
        {
            return new Window();
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
            get { return ((MonitorDisplayConfig)Type).RefreshRate; }
        }
    }
}