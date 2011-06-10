using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Hosts.Plugins.Jupiter.SystemModule.Config;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Linq;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.Jupiter.SystemModule.Design
{
    [Serializable]
    [XmlType("JupiterWindow")]
    public class JupiterWindow : ActiveWindow
    {
        public JupiterWindow()
        {
        }
        /// <summary>
        /// Номер входа видеостены, на который будет направлен аппаратный источник для воспроизведения
        /// Обязательный параметр
        /// Виден только для аппаратных источников
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Вход видеостены")]
        [XmlAttribute("VideoIn")]
        [TypeConverter(typeof(JupiterVideoInTypeConverter))]
        public int VideoIn { get; set; }

        /// <summary>
        /// Тип окна, в котором визуализируется источник (Video / RGB)
        /// Определяется типом входа видеостены
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Тип окна")]
        [XmlIgnore]
        [TypeConverter(typeof(CommonEnumConverter))]
        public WindowTypeEnum WindowType
        {
            get
            {
                JupiterDisplayDesign display = JupiterModule._windowMapping[this];
                if (display != null)
                    return display.isVideoWindow(this) ? WindowTypeEnum.Video : WindowTypeEnum.RGB;
                return WindowTypeEnum.Video;
            }
        }
    }
}