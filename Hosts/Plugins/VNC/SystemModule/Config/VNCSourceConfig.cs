using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

using Hosts.Plugins.VNC.SystemModule.Design;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.VNC.SystemModule.Config
{
    [Serializable]
    [XmlType("VNC")]
    public class VNCSourceConfig : SourceType
    {
        public VNCSourceConfig()
        {
            
        }

        public VNCSourceConfig(string name)
        {
            Name = name;
            Type = "Удаленный рабочий стол";
        }

        public override Source CreateNewSource(Slide slide, ModuleConfiguration moduleConfiguration, Dictionary<String, IList<DeviceResourceDescriptor>> resources, Display display)
        {
            return new VNCSourceDesign {Type = this};
        }

        protected override ResourceInfo CreateNewResourceInfoProtected()
        {
            return new VNCResourceInfo();
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return false; }
        }

        public override DeviceType CreateDeviceType()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class VNCResourceInfo : ResourceInfo
    {
        public VNCResourceInfo()
        {
            PasswordHash = new byte[0];
        }

        private string _uri = "localhost";

        /// <summary>
        /// Сетевой адрес компьютера (IP, порт и т.п.)
        /// Обязательный параметр
        /// По умолчанию: localhost
        /// </summary>
        [Category("Настройки")]
        [DisplayName("URI")]
        [XmlAttribute("Uri")]
        public string Uri
        {
            get { return _uri; }
            set { _uri = ValidationHelper.CheckLength(ValidationHelper.CheckIsNullOrEmpty(value, "URI"), 128, "URI"); }
        }

        /// <summary>
        /// Логин для удаленного доступа
        /// В требованиях отсутствует
        /// </summary>
        //[DisplayName("Логин")]
        //[XmlAttribute("Login")]
        //public string Login { get; set; }

        /// <summary>
        /// Пароль для удаленного доступа
        /// </summary>
        [PasswordPropertyText(true)]
        [Category("Настройки")]
        [DisplayName("Пароль")]
        [XmlIgnore]
        public string Password
        {
            get { return Encoding.Default.GetString(PasswordHash); }
            set
            {
                value = ValidationHelper.CheckLength(value, 50, "пароля");
                PasswordHash = Encoding.Default.GetBytes(value);
            }

        }
        /// <summary>
        /// Используется для сериализации пароля
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("Password", DataType = "base64Binary")]
        public byte[] PasswordHash { get; set; }

        //public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        //{
        //    PropertyDescriptorCollection propColl = base.GetProperties(attributes);
        //    foreach (DictionaryEntry dictionaryEntry in propColl)
        //    {
        //        dictionaryEntry.
        //    }
        //}
    }
}
