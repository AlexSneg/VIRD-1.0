using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

using Hosts.Plugins.Video.SystemModule.Config;

using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.Video.SystemModule.Design
{
    public enum PlayState
    {
        Play,
        Pause
    }

    [Serializable]
    [XmlType("Video")]
    public class VideoSourceDesign : SoftwareSource, ISourceSize
    {
        private bool _aspectLock = true;

        /// <summary>
        /// Ширина изображения (в пикселах)
        /// Получаем из ResourceInfo
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public int Width
        {
            get
            {
                if (ResourceDescriptor == null || ResourceDescriptor.ResourceInfo == null) return 0;
                return ((VideoResourceInfo) ResourceDescriptor.ResourceInfo).Width;
            }
        }

        /// <summary>
        /// Высота изображения (в пикселах)
        /// Получаем из ResourceInfo
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public int Height
        {
            get
            {
                if (ResourceDescriptor == null || ResourceDescriptor.ResourceInfo == null) return 0;
                return ((VideoResourceInfo) ResourceDescriptor.ResourceInfo).Height;
            }
        }

        /// <summary>
        /// Базовое разрешение для пользователя как соотношение ширины и высоты изображения (в пикселах)
        /// Получаем из ResourceInfo
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Базовое разрешение")]
        public string Resolution
        {
            get { return string.Format("{0} / {1}", Width, Height); }
        }

        /// <summary>
        /// Общее время показа для пользователя
        /// Получаем из ResourceInfo
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Общее время показа")]
        [XmlIgnore]
        public string DurationFriendly
        {
            get
            {
                if (ResourceDescriptor == null || ResourceDescriptor.ResourceInfo == null) return "00:00:00";
                return ((VideoResourceInfo) ResourceDescriptor.ResourceInfo).DurationFriendly;
            }
        }

        /// <summary>
        /// Продолжительность видеофайла (в секундах)
        /// Получаем из ResourceInfo
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public int Duration
        {
            get
            {
                if (ResourceDescriptor == null || ResourceDescriptor.ResourceInfo == null) return 0;
                return ((VideoResourceInfo)ResourceDescriptor.ResourceInfo).Duration;
            }
        }

        /// <summary>
        /// Текущее состояние воспроизведения файла
        /// Обязательный параметр
        /// По умолчанию: Play
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Состояние файла")]
        [XmlAttribute("State")]
        public PlayState State { get; set; }

        /// <summary>
        /// Смещение времени от начала (в секундах), начиная с которого показывается файл
        /// Обязательный параметр
        /// По умолчанию: 0
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("StartTimeShift")]
        public int StartTimeShift { get; set; }

        /// <summary>
        /// Текущее время показа для пользователя
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Текущее время показа")]
        [XmlIgnore]
        public string StartTimeShiftFriendly
        {
            get
            {
                TimeSpan span = TimeSpan.FromSeconds(StartTimeShift);
                return string.Format("{0:D2}:{1:D2}:{2:D2}",
                                     span.Hours, span.Minutes, span.Seconds);
            }
            set
            {
                DateTime dateTime;
                if (DateTime.TryParseExact(value, "hh:mm:ss", null,
                                           DateTimeStyles.NoCurrentDateDefault, out dateTime))
                {
                    StartTimeShift = (int) (dateTime - new DateTime(1, 1, 1)).TotalSeconds;
                }
            }
        }

        /// <summary>
        /// Признак блокирования отношения ширины изображение к высоте
        /// Обязательный параметр
        /// По умолчанию: True
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Aspect Lock")]
        [TypeConverter(typeof(YesNoConverter))]
        [DefaultValue(true)]
        [XmlAttribute("AspectLock")]
        public bool AspectLock
        {
            get { return _aspectLock; }
            set { _aspectLock = value; }
        }

        [XmlIgnore]
        public override ResourceDescriptor ResourceDescriptor
        {
            get { return base.ResourceDescriptor; }
            set
            {
                base.ResourceDescriptor = value;
                if (null == value) return;
                VideoResourceInfo info = value.ResourceInfo as VideoResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа VideoResourceInfo");
            }
        }

        #region ISourceSize Members


        public void SetSize(System.Drawing.Size newSize)
        {
            //nop ?
        }

        #endregion
    }
}