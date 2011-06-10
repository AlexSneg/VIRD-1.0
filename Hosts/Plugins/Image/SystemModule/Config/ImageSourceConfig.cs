using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using Hosts.Plugins.Image.SystemModule.Design;

using TechnicalServices.Exceptions;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.Image.SystemModule.Config
{
    [Serializable]
    [XmlType("Image")]
    public class ImageSourceConfig : SourceType
    {
        public ImageSourceConfig()
        {
            
        }

        public ImageSourceConfig(string name)
        {
            Name = name;
            Type = "Изображение";
        }
        protected override ResourceInfo CreateNewResourceInfoProtected()
        {
            return new ImageResourceInfo();
        }

        public override Source CreateNewSource(Slide slide, ModuleConfiguration moduleConfiguration, Dictionary<String, IList<DeviceResourceDescriptor>> resources, Display display)
        {
            ImageSourceDesign source = new ImageSourceDesign {Type = this};
            return source;
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return false; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsSupportPreview
        {
            get { return true; }
        }

        public override DeviceType CreateDeviceType()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract]
    [Serializable]
    public class ImageResourceInfo : ResourceFileInfo
    {
        private const string FileTypeItem1 = @"Bitmap Files (*.bmp)|*.bmp";
        private const string FileTypeItem2 = @"JPEG (*.jpg;*.jpeg;*.jpe;*.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif";
        private const string FileTypeItem3 = @"GIF (*.gif)|*.gif";
        private const string FileTypeItem4 = @"PNG (*.png)|*.png";
        private const string FileTypeItem5 = @"All Files (*.*)|*.*";

        private const string FilterFileTypes =
            FileTypeItem1 + "|" + FileTypeItem2 + "|" + FileTypeItem3 + "|" + FileTypeItem4 + "|" + FileTypeItem5;

        /// <summary>
        /// Ширина изображения (в пикселах)
        /// Обязательный параметр
        /// </summary>
        [DataMember]
        [XmlAttribute("Width")]
        [Browsable(false)]
        public int Width { get; set; }

        /// <summary>
        /// Высота изображения (в пикселах)
        /// Обязательный параметр
        /// </summary>
        [DataMember]
        [XmlAttribute("Height")]
        [Browsable(false)]
        public int Height { get; set; }

        /// <summary>
        /// Базовое разрешение для пользователя как соотношение ширины и высоты изображения (в пикселах)
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Базовое разрешение")]
        [ReadOnly(true)]
        [XmlIgnore]
        public string Resolution
        {
            get { return string.Format("{0} / {1}", Width, Height); }
        }

        [DataMember]
        [Browsable(false)]
        [XmlIgnore]
        public override string Filter
        {
            get { return FilterFileTypes; }
        }

        protected override void Init(ResourceFileProperty property, ResourceFileAttribute resourceFileAttribute)
        {
            // инициализируем разрешение
            if (string.IsNullOrEmpty(property.ResourceFullFileName))
            {
                Width = 0;
                Height = 0;
            }
            else
            {
                try
                {
                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(property.ResourceFullFileName))
                    {
                        Width = image.Width;
                        Height = image.Height;
                    }
                }
                catch (OutOfMemoryException ex)
                {
                    throw new InvalidResourceException("Файл не распознан как изображение", ex);
                }
            }
        }
    }
}