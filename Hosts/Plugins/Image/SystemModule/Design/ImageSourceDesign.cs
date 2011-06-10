using System;
using System.ComponentModel;
using System.Xml.Serialization;

using Hosts.Plugins.Image.SystemModule.Config;

using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Resource;


namespace Hosts.Plugins.Image.SystemModule.Design
{
    [Serializable]
    [XmlType("Image")]
    public class ImageSourceDesign : SoftwareSource, ISourceSize//, IDesignRenderSupport
    {
        public ImageSourceDesign()
            : base()
        {
        }

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
                return ((ImageResourceInfo)ResourceDescriptor.ResourceInfo).Width;
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
                return ((ImageResourceInfo)ResourceDescriptor.ResourceInfo).Height;
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
                ImageResourceInfo info = value.ResourceInfo as ImageResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа ImageResourceInfo");
            }
        }

        #region ISourceSize Members

        public void SetSize(System.Drawing.Size newSize)
        {
            //nop ?
        }

        #endregion

        #region IDesignRenderSupport Members

        public void Render(System.Drawing.Graphics gfx, System.Drawing.RectangleF area)
        {
        }

        public void UpdateReference(IServiceProvider provider)
        {
        }

        #endregion
    }
}