using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Common;

namespace Hosts.Plugins.VDCServer.SystemModule.Design
{
    [Serializable]
    [DisplayName("Абонент")]
    public class VDCServerAbonentInfo : ICollectionItemValidation, ICloneable
    {
        public VDCServerAbonentInfo()
        {
        }

        /// <summary>
        /// Создание копии.
        /// </summary>
        /// <param name="abonent"></param>
        public VDCServerAbonentInfo(VDCServerAbonentInfo abonent)
        {
            _name = abonent.Name;
            _number1 = abonent.Number1;
            _number2 = abonent.Number2;
            ConnectionQuality = abonent.ConnectionQuality;
            ConnectionType = abonent.ConnectionType;
        }

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
            set {
                ValidationHelper.CheckIsNullOrEmpty(value, "Имя абонента");
                _name = ValidationHelper.CheckLength(value, 50, "имени абонента"); 
            }
        }
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
            set {
                ValidationHelper.CheckIsNullOrEmpty(value, "Номер 1 абонента");
                _number1 = ValidationHelper.CheckLength(value, 50, "номера абонента"); 
            }
        }
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
            set { _number2 = ValidationHelper.CheckLength(value, 50, "номера абонента"); }
        }
        private string _number2 = string.Empty;

        /// <summary>
        /// Тип соединения
        /// Передается через Crestron терминалу ВКС для правильной интерпретации поля Номер
        /// Обязательный параметр
        /// По умолчанию: Auto
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Тип соединения")]
        [DefaultValue(ConnectionTypeVDCServerEnum.Auto)]
        [XmlAttribute("ConnectionType")]
        public ConnectionTypeVDCServerEnum ConnectionType { get; set; }

        /// <summary>
        /// Качество исходящего видеопотока
        /// Обязательный параметр
        /// По умолчанию: Auto
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Качество соединения")]
        [DefaultValue(ConnectionQualityVDCServerEnum.Auto)]
        [XmlAttribute("ConnectionQuality")]
        [TypeConverter(typeof(CommonEnumConverter))]
        public ConnectionQualityVDCServerEnum ConnectionQuality { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name)?"Абонент":Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is VDCServerAbonentInfo)
                return ((VDCServerAbonentInfo)obj).Name == this.Name;
            return base.Equals(obj);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #region ICollectionItemValidation Members

        bool ICollectionItemValidation.ValidateItem(out string errorMessage)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                errorMessage = "Имя абонента должно быть заполнено.";
                return false;
            }
            if (string.IsNullOrEmpty(this.Name))
            {
                errorMessage = "Номер 1 абонента должен быть заполнен.";
                return false;
            }
            errorMessage = "OK";
            return true;
        }

        #endregion
    }

    public enum ConnectionTypeVDCServerEnum
    {
        Auto, IP, ISDN
    }

    public enum ConnectionQualityVDCServerEnum
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
    }}
