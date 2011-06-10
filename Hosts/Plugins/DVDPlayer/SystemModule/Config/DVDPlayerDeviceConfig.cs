using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Hosts.Plugins.DVDPlayer.SystemModule.Design;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.DVDPlayer.SystemModule.Config
{
    [Serializable]
    [XmlType("DVDPlayerDevice")]
    public class DVDPlayerDeviceConfig : DeviceTypeAsSource
    {
        public DVDPlayerDeviceConfig(string name)
            : this()
        {
            Name = name;
            Type = "DVD";
        }

        public DVDPlayerDeviceConfig()
        {
        }

        public override Device CreateNewDevice()
        {
            return new DVDPlayerDeviceDesign { Type = this };
        }

        /// <summary>
        /// Интерфейс управления (RS232/IR)
        /// Обязательное значение
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Интерфейс управления")]
        [XmlAttribute("InterfaceType")]
        public InterfaceTypeEnum InterfaceType { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return true; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool Visible
        {
            get { return false; }
        }
    }

    public enum InterfaceTypeEnum
    {
        RS232, IR
    }

    public enum DVDState
    {
        [Description("Playback")]
        Playback,
        [Description("Stopped")]
        Stopped,
        [Description("Pause")]
        Pause,
        [Description("FFWD")]
        FFWD,
        [Description("REW")]
        REW,
        [Description("NO CD")]
        NO_CD
    }
}