using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Hosts.Plugins.IEDocument.SystemModule.Design;
using TechnicalServices.Exceptions;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using Hosts.Plugins.IEDocument.Common;

namespace Hosts.Plugins.IEDocument.SystemModule.Config
{
    [Serializable]
    [XmlType("IEDocument")]
    public class IEDocumentSourceConfig : SourceType
    {
        public IEDocumentSourceConfig()
        {
            
        }

        public IEDocumentSourceConfig(string name)
        {
            Name = name;
            Type = "Internet Explorer";
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return false; }
        }

        //[XmlIgnore]
        //[Browsable(false)]
        //public override bool IsSupportPreview
        //{
        //    get { return true; }
        //}


        public override Source CreateNewSource(Slide slide, ModuleConfiguration moduleConfiguration, Dictionary<string, IList<DeviceResourceDescriptor>> resources, Display display)
        {
            //IEDocumentSourceDesign source = new IEDocumentSourceDesign() { Type = this };
            //return source;

            return new IEDocumentSourceDesign { Type = this };
        }

        protected override ResourceInfo CreateNewResourceInfoProtected()
        {
            return new IEResourceInfo();
        }

        public override DeviceType CreateDeviceType()
        {
            throw new NotImplementedException();
        }
    }

    //[DataContract]
    [Serializable]
    public class IEResourceInfo : ResourceInfo //ResourceFileInfo
    {
        public IEResourceInfo()
        {
            PasswordHash = new byte[0];
        }
        
        //TO DO: Удалить
        //public override string Filter
        //{
        //    get
        //    {
        //        return @"Internet Explorer(*.html)|*.html";
        //    }
        //}

        //[DataMember]
        //[ReadOnly(true)]
        //[DisplayName("Количество слайдов")]
        //[XmlAttribute("NumberOfSlides")]
        //public int NumberOfSlides
        //{
        //    get;
        //    set;
        //}

        //protected override void Init(ResourceFileProperty property, ResourceFileAttribute resourceFileAttribute)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(property.ResourceFullFileName)) return;
        //        ////using (IEDoc ieDoc = IEDoc.OpenFile(property.ResourceFullFileName))
        //        ////{
        //        ////    //NumberOfSlides = IE.NumberOfSlides;
        //        ////}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidResourceException(string.Format("Файл {0} не является документом IE", Path.GetFileName(property.ResourceFullFileName)), ex);
        //    }
        //}

        private string url = "localhost";
        /// <summary>
        /// Сетевой адрес компьютера (IP, порт и т.п.)
        /// Обязательный параметр
        /// По умолчанию: localhost
        /// </summary>
        [Category("Настройки")]
        [DisplayName("URL")]
        [XmlAttribute("Url")]
        [DefaultValue("localhost")]
        public string Url
        {
            get { return url; }
            set { url = ValidationHelper.CheckLength(ValidationHelper.CheckIsNullOrEmpty(value, "URL"), 250, "URL"); }
        }

        private string login = "";
        [DisplayName("Логин")]
        [XmlAttribute("Login")]
        [DefaultValue("")]
        public string Login
        {
            get { return login; }
            set { login = ValidationHelper.CheckLength(value, 128, "логина"); }
        }

        [PasswordPropertyText(true)]
        [DisplayName("Пароль")]
        [XmlIgnore]
        public string Password
        {
            get { return Encoding.Default.GetString(PasswordHash); }
            set
            {
                value = ValidationHelper.CheckLength(value, 128, "пароля");
                PasswordHash = Encoding.Default.GetBytes(value);
            }
        }

        /// <summary>
        /// Используется для сериализации пароля
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("Password", DataType = "base64Binary")]
        public byte[] PasswordHash { get; set; }

        private string postParams = "";
        [DisplayName("Параметры запроса")]
        [XmlAttribute("PostParams")]
        [DefaultValue("")]
        public string PostParams
        {
            get { return postParams; }
            set 
            {
                ParsePOSTParams parsePostParams = new ParsePOSTParams();
                string incorrectParams = parsePostParams.Verification(ValidationHelper.CheckLength(value, 256, "Параметров запроса"));
                if (incorrectParams != "")
                {
                    throw new Exception(String.Format("Некорректно введённые параметры: {0}", incorrectParams));
                }
                postParams = value;
            }
        }

        private string postParamsEncoding = "Win-1251";
        [DisplayName("Кодировка")]
        [XmlAttribute("PostParamsEncoding")]
        [TypeConverter(typeof(IEDocumentEncodingValues))]
        [DefaultValue("Win-1251")]
        public string PostParamsEncoding
        {
            get { return postParamsEncoding; }
            set { postParamsEncoding = ValidationHelper.CheckLength(value, 128, "кодировки"); }
        }
    }
}
