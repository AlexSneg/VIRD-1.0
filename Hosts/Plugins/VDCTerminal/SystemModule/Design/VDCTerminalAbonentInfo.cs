using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.CommonPersistence.Configuration;


namespace Hosts.Plugins.VDCTerminal.SystemModule.Design
{
    [Serializable]
    [DataContract]
    public class VDCTerminalAbonentInfo: ICollectionItemValidation, ICloneable
    {
        /// <summary>
        /// Название абонента
        /// Обязательный параметр
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Имя")]
        [XmlAttribute("Name")]
        public string Name
        {
            get { return _name; }
            set 
            {
                ValidationHelper.CheckLength(value, 50, "Наименование");
                _name = ValidationHelper.CheckIsNullOrEmpty( value, "Наименование"); 
            }
        }
        [DataMember]
        private string _name = string.Empty;

        /// <summary>
        /// Номер1 (IP-адрес / телефонный номер)
        /// Обязательный параметр
        /// По умолчанию: 0.0.0.0
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Номер1")]
        [DefaultValue("0.0.0.0")]
        [XmlAttribute("Number1")]
        public string Number1
        {
            get { return _number1; }
            set { _number1 = ValidationHelper.CheckIsNullOrEmpty( value, "Номер 1"); }
        }
        [DataMember]
        private string _number1 = "0.0.0.0";

        /// <summary>
        /// Номер2 (IP-адрес / телефонный номер)
        /// Необязательный параметр, используется при недоступности Номер1
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Номер2")]
        [XmlAttribute("Number2")]
        public string Number2 
        {
            get { return _number2; }
            set { _number2 = value; }
        }
        [DataMember]
        private string _number2 = string.Empty;

        /// <summary>
        /// Тип соединения
        /// Передается через Crestron терминалу ВКС для правильной интерпретации поля Номер
        /// Обязательный параметр
        /// По умолчанию: Auto
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Тип соединения")]
        [DefaultValue(ConnectionTypeEnum.Auto)]
        [XmlAttribute("ConnectionType")]
        [DataMember]
        public ConnectionTypeEnum ConnectionType { get; set; }

        /// <summary>
        /// Качество исходящего видеопотока
        /// Обязательный параметр
        /// По умолчанию: Auto
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Качество соединения")]
        [DefaultValue(ConnectionQualityEnum.Auto)]
        [XmlAttribute("ConnectionQuality")]
        [TypeConverter(typeof(CommonEnumConverter))]
        [DataMember]
        public ConnectionQualityEnum ConnectionQuality { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name)) return "Абонент";
            return Name;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #region ICollectionValidation Members

        bool ICollectionItemValidation.ValidateItem(out string errorMessage)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                errorMessage = "Не заполнено обязательное поле 'Наименование'.";
                return false;
            }
            if (string.IsNullOrEmpty(this.Number1))
            {
                errorMessage = "Не заполнено обязательное поле 'Номер 1'.";
                return false;
            }
            errorMessage="OK";
            return true;
        }

        #endregion
    }

    [Serializable]
    public enum ConnectionTypeEnum
    {
        Auto, IP, ISDN
    }

    [Serializable]
    public enum ConnectionQualityEnum
    {
        Auto,
        [Description("128 kbps")]  _128_kbps,
        [Description("256 kbps")]  _256_kbps,
        [Description("384 kbps")]  _384_kbps,
        [Description("512 kbps")]  _512_kbps,
        [Description("640 kbps")]  _640_kbps,
        [Description("768 kbps")]  _768_kbps,
        [Description("896 kbps")]  _896_kbps,
        [Description("1024 kbps")] _1024_kbps,
        [Description("1280 kbps")] _1280_kbps,
        [Description("1536 kbps")] _1536_kbps,
        [Description("1792 kbps")] _1792_kbps,
        [Description("2048 kbps")] _2048_kbps
    }
}