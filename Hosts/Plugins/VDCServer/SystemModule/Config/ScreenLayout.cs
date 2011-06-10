using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using TechnicalServices.Common;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.CommonPersistence.Configuration;

namespace Hosts.Plugins.VDCServer.SystemModule.Config
{
    [Serializable]
    [XmlType("ScreenLayout")]
    public class ScreenLayout : ICloneable, ICollectionItemValidation
    {
        private string _layoutName;
        private int _layoutNumber;
        private Bitmap _layoutPicture;

        /// <summary>
        /// Номер.
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Номер")]
        [XmlAttribute("LayoutNumber")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt32Converter, TechnicalServices.Common")]
        public int LayoutNumber
        {
            get { return _layoutNumber; }
            set { _layoutNumber = value; }
        }

        /// <summary>
        /// Название.
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Название")]
        [XmlAttribute("LayoutName")]
        public string LayoutName
        {
            get { return _layoutName; }
            set { _layoutName = ValidationHelper.CheckLength(value, 50, "названия раскладки"); }
        }

        /// <summary>
        /// Пиктограмма.
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Пиктограмма")]
        //[XmlAttribute("LayoutPicture")]
        [XmlIgnore]
        [TypeConverter(typeof(CollectionNameConverter))]
        public Bitmap LayoutPicture
        {
            get { return _layoutPicture; }
            set
            {
                ValidationHelper.CheckSize(value.Size, new Size(64,64), "изображения");
                _layoutPicture = value;
            }
        }

        /// <summary>
        /// Возвращает и устанавливает картинку в виде массива байтов.
        /// Используется только для XML-сериализации.
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("LayoutPictureBytes", DataType = "base64Binary")]
        public byte[] LayoutPictureBytes
        {
            get
            {
                MemoryStream stream = new MemoryStream();
                _layoutPicture.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
            set { _layoutPicture = (Bitmap) Image.FromStream(new MemoryStream(value)); }
        }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        public override string ToString()
        {
            return LayoutName;
        }

        public bool ValidateItem(out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(LayoutName))
            {
                errorMessage = "Не задано название раскладки";
                return false;
            }
            if (LayoutPicture == null)
            {
                errorMessage = "Не задана пиктограмма";
                return false;
            }
            return true;
        }
    }
}