using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Hosts.Plugins.DVDPlayer.SystemModule.Config;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.DVDPlayer.SystemModule.Design
{
    [Serializable]
    [XmlType("DVDPlayerDevice")]
    public class DVDPlayerDeviceDesign : DeviceAsSource, ICustomTypeDescriptor
    {
        /// <summary>
        /// Интерфейс управления (RS232/IR)
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Интерфейс управления")]
        [XmlIgnore, ReadOnly(true)]
        public InterfaceTypeEnum InterfaceType
        {
            get { return ((DVDPlayerDeviceConfig) Type).InterfaceType; }
        }

        /// <summary>
        /// Тип носителя (CD/DVD)
        /// Обязательное значение
        /// По умолчанию: DVD
        /// </summary>
        [DefaultValue(MediumTypeEnum.DVD)]
        [Category("Настройки")]
        [DisplayName("Тип носителя")]
        [XmlAttribute("MediumType")]
        public MediumTypeEnum MediumType { get; set; }

        /// <summary>
        /// Название диска
        /// Необязательное значение
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Название диска")]
        [XmlAttribute("DiskLabel")]
        public string DiskLabel
        {
            get { return _diskLabel; }
            set { _diskLabel = ValidationHelper.CheckLength(value, 128, "названия диска"); }
        }
        private string _diskLabel;

        /// <summary>
        /// Команда управления, которую надо исполнить при выдаче сцены
        /// Обязательное значение
        /// По умолчанию: None
        /// </summary>
        [DefaultValue(SceneAssociatedCommandEnum.None)]
        [Category("Настройки")]
        [DisplayName("Команда управления")]
        [XmlAttribute("SceneAssociatedCommand")]
        [TypeConverter(typeof(CommonEnumConverter))]
        public SceneAssociatedCommandEnum SceneAssociatedCommand { get; set; }

        /// <summary>
        /// Состояние устройства (On/Off)
        /// Обязательное значение
        /// По умолчанию: On
        /// </summary>
        [DefaultValue(true)]
        [Category("Настройки")]
        [DisplayName("Состояние устройства")]
        [XmlAttribute("IsPlayerOn")]
        [TypeConverter(typeof(OnOffConverter))]
        public bool IsPlayerOn
        {
            get { return _isPlayerOn; }
            set { _isPlayerOn = value; }
        }
        private bool _isPlayerOn = true;

        /// <summary>
        /// Количество глав (только для DVD)
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        [InterfaceTypeRequired(InterfaceTypeEnum.RS232)]
        public int DVDChapterAmount
        {
            get { return _DVDChapterAmount; }
            set { _DVDChapterAmount = value; }
        }
        private int _DVDChapterAmount = 50;

        /// <summary>
        /// Глава (только для DVD)
        /// По умолчанию: 1
        /// </summary>
        [DefaultValue(1)]
        [Category("Настройки")]
        [DisplayName("Глава")]
        [XmlAttribute("DVDChapter")]
        //[InterfaceTypeRequired(InterfaceTypeEnum.RS232)]
        public int DVDChapter
        {
            get { return _dvdChapter; }
            set { _dvdChapter = (value < 0 ? _dvdChapter : (value > DVDChapterAmount ? _dvdChapter : value)); }
        }
        private int _dvdChapter = 1;

        /// <summary>
        /// Количество треков
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        [InterfaceTypeRequired(InterfaceTypeEnum.RS232)]
        public int TrackAmount
        {
            get { return _trackAmount; }
            set { _trackAmount = value; }
        }
        private int _trackAmount = 50;

        /// <summary>
        /// Трек
        /// По умолчанию: 1
        /// </summary>
        [DefaultValue(1)]
        [Category("Настройки")]
        [DisplayName("Трек")]
        [XmlAttribute("Track")]
        //[InterfaceTypeRequired(InterfaceTypeEnum.RS232)]
        public int Track
        {
            get { return _track; }
            set { _track = (value < 0 ? _track : (value > TrackAmount ? _track : value)); }
        }
        private int _track = 1;

        /// <summary>
        /// Общее время проигрывания
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        [InterfaceTypeRequired(InterfaceTypeEnum.RS232)]
        public string TotalPlaybackTime
        {
            get { return _totalPlaybackTime; }
            set { _totalPlaybackTime = value; }
        }
        private string _totalPlaybackTime = "00:00:00";

        /// <summary>
        /// Текущее время проигрывания
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        [InterfaceTypeRequired(InterfaceTypeEnum.RS232)]
        public string ElapsedTime
        {
            get
            {
                TimeSpan span = TimeSpan.FromSeconds(0);
                return string.Format("{0:D2}:{1:D2}:{2:D2}", span.Hours, span.Minutes, span.Seconds);
            }
        }

        /// <summary>
        /// Команды управления DVD плеером
        /// </summary>
        /// <param name="command">команда управления</param>
        public void SetPlayerCommand(PlayerCommandEnum command)
        {
        }

        /// <summary>
        /// Текущее состояние DVD плеера
        /// </summary>
        /// <returns>текущее состояние</returns>
        public PlayerStateEnum GetPlayerState()
        {
            return PlayerStateEnum.Unknown; 
        }

        /// <summary>
        /// Выбор главы и трека
        /// </summary>
        /// <param name="chapter">глава (только для DVD)</param>
        /// <param name="track">трек</param>
        public void SetChapterAndTrack(int chapter, int track)
        {
        }

        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }

        public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection propColl;
            if (attributes == null)
                propColl = TypeDescriptor.GetProperties(this, true);
            else
                propColl = TypeDescriptor.GetProperties(this, attributes, true);
            // в зависимости от типа интерфейса доступны различные свойства
            List<PropertyDescriptor> newColl = new List<PropertyDescriptor>(propColl.Count);
            foreach (PropertyDescriptor propertyDescriptor in propColl)
            {
                if (MediumType == MediumTypeEnum.CD && propertyDescriptor.Name == "DVDChapter") continue;
                InterfaceTypeRequiredAttribute attr = (InterfaceTypeRequiredAttribute) propertyDescriptor.Attributes[typeof(InterfaceTypeRequiredAttribute)];
                if ((attr == null) || (attr.InterfaceType == InterfaceType))
                    newColl.Add(propertyDescriptor);
            }
            propColl = new PropertyDescriptorCollection(newColl.ToArray(), true);
            return propColl;
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }

    public class InterfaceTypeRequiredAttribute : Attribute
    {
        public InterfaceTypeRequiredAttribute(InterfaceTypeEnum type)
        {
            InterfaceType = type;
        }

        public InterfaceTypeEnum InterfaceType { get; private set; }
    }

    public enum MediumTypeEnum
    {
        DVD, CD
    }

    public enum SceneAssociatedCommandEnum
    {
        [Description("Нет")] None,
        Play, Pause
    }

    public enum PlayerCommandEnum
    {
        Left, Right, Up, Down,
        Play, Stop, Pause,
        Previous, Next, FastBack, FastForward, Repeat, Return,
        Menu, Setup, Ok, Wait
    }

    public enum PlayerStateEnum
    {
        Unknown, 
        NO_CD,
        Playback, Stopped, Pause,
        FFWD, REW
    }
}