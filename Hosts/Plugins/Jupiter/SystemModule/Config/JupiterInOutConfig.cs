using System;
using System.ComponentModel;
using System.Xml.Serialization;
using TechnicalServices.Common.TypeConverters;

namespace Hosts.Plugins.Jupiter.SystemModule.Config
{
    [Serializable]
    [XmlType("InOutConfig")]
    public class JupiterInOutConfig
    {
        public override string ToString()
        {
            return String.Format("Вход {0}", VideoIn);
        }

        /// <summary>
        /// Номер входа в контроллере видеостены (1..100)
        /// Обязательный уникальный параметр
        /// Заполняется Системой автоматически: Max(VideoIn)+1
        /// Можно заполнять при помощи метода VerifyInOutConfig
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Вход видеостены")]
        [XmlAttribute("VideoIn")]
        [ReadOnly(true)]
        public short VideoIn { get; set; }

        /// <summary>
        /// Номер выхода коммутатора (0..100)
        /// Обязательный уникальный параметр
        /// По умолчанию: 0 (вход в контроллере видеостены не подключен к коммутатору)
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Выход коммутатора")]
        [XmlAttribute("SwitchOut")]
        public int SwitchOut { get; set; }

        /// <summary>
        /// Тип окна, в котором визуализируется источник (Video / RGB)
        /// Обязательный параметр
        /// По умолчанию: Video
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Тип окна")]
        [XmlAttribute("WindowType")]
        [TypeConverter(typeof(CommonEnumConverter))]
        public WindowTypeEnum WindowType { get; set; }

        /// <summary>
        /// Комментарий к входу
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Комментарий")]
        [XmlAttribute("Comment")]
        public string Comment { get; set; }
    }
    
    public enum WindowTypeEnum
    {
        [Description("Видео")]
        Video,
        RGB
    }
}