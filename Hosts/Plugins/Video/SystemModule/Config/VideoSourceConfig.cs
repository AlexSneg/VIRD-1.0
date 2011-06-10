using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using Hosts.Plugins.Video.SystemModule.Design;
using Hosts.Plugins.Video.UI;

using TechnicalServices.Exceptions;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.Video.SystemModule.Config
{
    [Serializable]
    [XmlType("Video")]
    public class VideoSourceConfig : SourceType
    {
        public VideoSourceConfig()
        {
            
        }

        public VideoSourceConfig(string name)
        {
            Name = name;
            Type = "Видеофайл";
        }

        public override Source CreateNewSource(Slide slide, ModuleConfiguration moduleConfiguration, Dictionary<String, IList<DeviceResourceDescriptor>> resources, Display display)
        {
            return new VideoSourceDesign {Type = this};
        }

        protected override ResourceInfo CreateNewResourceInfoProtected()
        {
            return new VideoResourceInfo();
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
    public class VideoResourceInfo : ResourceFileInfo
    {
        private const string FileTypeItem1 = @"Video Files (*.mpg;*.mpeg;*.m2v;*.avi)|*.mpg;*.mpeg;*.m2v;*.avi";
        private const string FileTypeItem2 = @"MP4 File (*.m4a;*.mp4)|*.m4a;*.mp4";
        private const string FileTypeItem3 = @"Windows Media File (*.wmv)|*.wmv";
        private const string FileTypeItem4 = @"All Files (*.*)|*.*";

        private const string FilterFileTypes =
            FileTypeItem1 + "|" + FileTypeItem2 + "|" + FileTypeItem3 + "|" + FileTypeItem4;

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

        /// <summary>
        /// Продолжительность видеофайла (в секундах)
        /// Обязательный параметр
        /// </summary>
        [DataMember]
        [XmlAttribute("Duration")]
        [Browsable(false)]
        public int Duration { get; set; }

        /// <summary>
        /// Общее время показа для пользователя
        /// </summary>
        [XmlIgnore]
        [ReadOnly(true)]
        [Category("Настройки")]
        [DisplayName("Общее время показа")]
        public string DurationFriendly
        {
            get
            {
                TimeSpan span = TimeSpan.FromSeconds(Duration);
                return string.Format("{0:D}:{1:D2}:{2:D2}",
                                     span.Hours, span.Minutes, span.Seconds);
            }
        }

        [Browsable(false)]
        [XmlIgnore]
        public override string Filter
        {
            get { return FilterFileTypes; }
        }

        protected override void Init(ResourceFileProperty property, ResourceFileAttribute resourceFileAttribute)
        {
            //AutoResetEvent ready = new AutoResetEvent(false); 
            // инициализируем доп параметры
            if (string.IsNullOrEmpty(property.ResourceFullFileName))
            {
                Width = 0;
                Height = 0;
                Duration = 0;
            }
            else
            {
                try
                {
                    using (WMPlayerForm form = new WMPlayerForm())
                    {
                        //form.Show(new WindowWrapper(ptr));
                        form.FileName = property.ResourceFullFileName;
                        form.ShowDialog();
                        //form.StartPlay(resourceFullFileName);
                        //Thread.Sleep(10000);
                        Width = form.VideoWidth;
                        Height = form.VideoHeight;
                        Duration = form.Duration;
                        form.Close();
                    }
                    if (Width == 0 || Height == 0 || Duration == 0)
                        throw new InvalidResourceException(
                            "Файл не распознан как видео-файл, нет необходимого кодека или не установлен WMP 10 версии и выше");
                }
                catch (Exception)
                {
                    throw new InvalidResourceException("Файл не распознан как видео-файл или нет необходимого кодека");
                }
                //using (WMPlayer wmPlayer = WMPlayer.Create(resourceFullFileName))
                //{
                //    Width = wmPlayer.Width;
                //    Height = wmPlayer.Height;
                //    Duration = wmPlayer.Duration;
                //}
            }
        }
    }
}