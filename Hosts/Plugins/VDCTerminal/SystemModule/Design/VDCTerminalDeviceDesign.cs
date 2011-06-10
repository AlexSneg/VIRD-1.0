using System;
using System.ComponentModel;
using System.Xml.Serialization;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.CommonPersistence.Configuration;

namespace Hosts.Plugins.VDCTerminal.SystemModule.Design
{
    [Serializable]
    [XmlType("VDCTerminalDevice")]
    public class VDCTerminalDeviceDesign : DeviceAsSource
    {

        /// <summary>
        /// Абонент (выбор из справочника абонентов)
        /// Необязательный параметр
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Абонент")]
        [XmlElement("Abonent")]
        [TypeConverter(typeof(VDCTerminalAbonentConverter))]
        public VDCTerminalAbonentInfo Abonent { get; set; }

        /// <summary>
        /// Номер абонента (ввод вручную)
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Номер абонента")]
        [XmlAttribute("DirectNumber")]
        public string DirectNumber
        {
            get { return _directNumber; }
            set { _directNumber = ValidationHelper.CheckLength(value, 128, "номера абонента"); }
        }
        private string _directNumber;

        /// <summary>
        /// Текущее состояние соединения
        /// Обязательный параметр
        /// По умолчанию: Connected
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Состояние соединения")]
        [DefaultValue(ConnectionStateEnum.Connected)]
        [XmlAttribute("ConnectionState")]
        public ConnectionStateEnum ConnectionState { get; set; }

        /// <summary>
        /// Строка состояния от терминала (расширенное описание)
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public string StateDescription
        {
            get { return null; }
        }

        /// <summary>
        /// Описание ошибки от терминала ("Абонент занят" и т.п.)
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public string ErrorDescription
        {
            get { return null; }
        }

        /// <summary>
        /// Команда "Повторить набор"
        /// </summary>
        public void Redial()
        {
        }

        /// <summary>
        /// Индикация входящего звонка
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public string IncomingCall
        {
            get { return null; }
        }

        /// <summary>
        /// Абонент, осуществляющий входящий звонок
        /// От Crestron поступит IP или телефонный номер
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public VDCTerminalAbonentInfo IncomingCallOwner
        {
            get { return null; }
        }

        /// <summary>
        /// Команда "Ответить"
        /// </summary>
        public void AnswerCall()
        {
        }

        /// <summary>
        /// Режим передачи презентационных данных (On/Off)
        /// Обязательный параметр
        /// По умолчанию: Off
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Режим People + Content")]
        [DefaultValue(false)]
        [XmlAttribute("PeopleConnect")]
        [TypeConverter(typeof(OnOffConverter))]
        public bool PeopleConnect { get; set; }

        /// <summary>
        /// Состояние микрофона (On/Off)
        /// Обязательный параметр
        /// По умолчанию: Off
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Режим Privacy")]
        [DefaultValue(false)]
        [XmlAttribute("Privacy")]
        [TypeConverter(typeof(OnOffConverter))]
        public bool Privacy { get; set; }

        /// <summary>
        /// Режим "Не беспокоить" (On/Off)
        /// При входящем звонке не будет звукового сигнала с терминала
        /// Обязательный параметр
        /// По умолчанию: Off
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Режим DND")]
        [DefaultValue(false)]
        [XmlAttribute("DND")]
        [TypeConverter(typeof(OnOffConverter))]
        public bool DND { get; set; }

        /// <summary>
        /// Режим автоматического снятия трубки при входящем звонке (On/Off)
        /// Обязательный параметр
        /// По умолчанию: On
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Авто-ответ")]
        [DefaultValue(true)]
        [XmlAttribute("AutoResponse")]
        [TypeConverter(typeof(OnOffConverter))]
        public bool AutoResponse
        {
            get { return _autoResponse; }
            set { _autoResponse = value; }
        }
        private bool _autoResponse = true;

        /// <summary>
        /// Режим "Картинка в картинке" (On/Off)
        /// Обязательный параметр
        /// По умолчанию: On
        /// </summary>
        [Category("Настройки")]
        [DisplayName("PiP")]
        [DefaultValue(true)]
        [XmlAttribute("PiP")]
        [TypeConverter(typeof(OnOffConverter))]
        public bool PiP
        {
            get { return _piP; }
            set { _piP = value; }
        }
        private bool _piP = true;
    }

    public enum ConnectionStateEnum
    {
        Connected, Disconnected
    }
}