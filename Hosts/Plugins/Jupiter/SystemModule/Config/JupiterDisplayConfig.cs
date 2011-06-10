using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Xml.Serialization;

using Hosts.Plugins.Jupiter.SystemModule.Design;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPersistence.Presentation;

namespace Hosts.Plugins.Jupiter.SystemModule.Config
{
    [Serializable]
    [XmlType("JupiterDisplay")]
    public class JupiterDisplayConfig : DisplayTypeUriCapture, ISupportValidation
    {
        private readonly List<JupiterInOutConfig> _inOutConfigList = new List<JupiterInOutConfig>();

        public JupiterDisplayConfig(string name) : this()
        {
            Name = name;
            Type = "Видеостена";
            AgentUID = "ComputerName";
        }

        public JupiterDisplayConfig()
        {
            UID = -1;
            Address = String.Empty;
            DistanceM = 5;
            HeightM = 1;
            WidthM = 1;
            SegmentRows = 1;
            SegmentColumns = 1;
            SegmentWidth = 1024;
            SegmentHeight = 768;
            Width = 1024;
            Height = 768;
        }
        /// <summary>
        /// Разрешение сегмента (куба) (в пикселах)
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Разрешение сегмента (px)")]
        [XmlIgnore]
        [TypeConverter(typeof (ExpandableObjectConverter))]
        public Size<int> SegmentResolution
        {
            get { return _segmentResolution; }
            set
            {
                _segmentResolution = value;
            }
        }
        private Size<int> _segmentResolution=new Size<int>(800, 4096, 600, 4096);

        /// <summary>
        /// Общее доступное количество ресурса видеостены (0..500)
        /// Обязательный параметр
        /// По умолчанию: 0 (ограничения на количество ресурса нет)
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Количество ресурса")]
        [XmlAttribute("ResourceAvailability")]
        public int ResourceAvailability 
        {
            get
            {
                return _ResourceAvailability;
            }
            set
            {
                _ResourceAvailability = ValidationHelper.CheckRange(value, 0, 500, "Количество ресурса");
            }
        }
        int _ResourceAvailability;

        /// <summary>
        /// Входы видеостены
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Входы видеостены")]
        [XmlArray("InOutConfigList")]
        [Browsable(false)]
        [Editor(typeof (JupiterInOutConfigCollectionEditor), typeof (UITypeEditor))]
				[TypeConverter(typeof(TechnicalServices.Common.TypeConverters.CollectionNameConverter))]
				public List<JupiterInOutConfig> InOutConfigList
        {
            get { return _inOutConfigList; }
        }

        public event EventHandler InOutConfigListChanged;

        public void FireInOutConfigListChanged()
        {
            if (InOutConfigListChanged != null)
                InOutConfigListChanged(this, null);
        }

        /// <summary>
        /// Количество горизонтальных рядов сегментов (кубов) (1..20)
        /// Обязательный параметр
        /// По умолчанию: 1
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("SegmentRows")]
        public int SegmentRows 
        {
            get
            {
                return SegmentAmount.Y;
            }
            set
            {
                SegmentAmount.Y = value;
            }
        }

        /// <summary>
        /// Количество вертикальных рядов сегментов (кубов) (1..20)
        /// Обязательный параметр
        /// По умолчанию: 1
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("SegmentColumns")]
        public int SegmentColumns 
        {
            get
            {
                return SegmentAmount.X;
            }
            set
            {
                SegmentAmount.X = value;
            }
        }

        /// <summary>
        /// Количество сегментов (кубов)
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Количество сегментов")]
        [XmlIgnore]
        [TypeConverter(typeof (ExpandableObjectConverter))]
        public Size<int> SegmentAmount
        {
            get 
            { 
                return _segmentAmount;
            }
            set
            {
                _segmentAmount = value;
            }
        }
        private Size<int> _segmentAmount = new Size<int>(1, 20);

        /// <summary>
        /// Горизонтальное разрешение сегмента (куба) (в пикселах) (1..4096)
        /// Обязательный параметр
        /// По умолчанию: 1024
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("SegmentWidth")]
        public int SegmentWidth
        {
            get
            {
                return SegmentResolution.X;
            }
            set
            {
                SegmentResolution.X = value;
            }
        }

        /// <summary>
        /// Вертикальное разрешение сегмента (куба) (в пикселах) (1..4096)
        /// Обязательный параметр
        /// По умолчанию: 768
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("SegmentHeight")]
        public int SegmentHeight
        {
            get
            {
                return SegmentResolution.Y;
            }
            set
            {
                SegmentResolution.Y = value;
            }
        }

        /// <summary>
        /// Разрешение экрана (в пикселях)
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Разрешение (px)")]
        [XmlIgnore]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [ReadOnly(true)]
        [Browsable(false)]
        override public Size<int> Size
        {
            get
            {
                return new Size<int>
                {
                    X = SegmentAmount.X * SegmentResolution.X,
                    Y = SegmentAmount.Y * SegmentResolution.Y
                };
            }
        }

        public override Display CreateNewDisplay()
        {
            return new JupiterDisplayDesign {Type = this};
        }

        public override Mapping CreateMapping(SourceType source)
        {
            return new JupiterMapping {Source = source};
        }

        /// <summary>
        /// Проверка экземпляра JupiterInOutConfig перед добавлением в список
        /// Если VideoIn = 0 или такой VideoIn есть уже есть в списке, то VideoIn = Max(VideoIn) + 1
        /// </summary>
        public JupiterInOutConfig VerifyInOutConfig(JupiterInOutConfig obj)
        {
            if (InOutConfigList.Count == 0)
            {
                // список пуст
                if (obj.VideoIn == 0) obj.VideoIn = 1;
                return obj;
            }
            if (obj.VideoIn == 0)
            {
                // в списке есть элементы, VideoIn=0, ищем максимум
                obj.VideoIn = (short) (InOutConfigList.Max(s => s.VideoIn) + 1);
                return obj;
            }
            // в списке есть элементы, VideoIn!=0, проверяем совпадения
            if (InOutConfigList.Where(s => s.VideoIn == obj.VideoIn).FirstOrDefault() != null)
                obj.VideoIn = (short) (InOutConfigList.Max(s => s.VideoIn) + 1);
            return obj;
        }

        #region ISupportValidation Members

        bool ISupportValidation.EnsureValidate(out string errormessage)
        {
            for (int j = 0; j < InOutConfigList.Count; j++)
            {
                for (int i = 0; i < InOutConfigList.Count; i++)
                {
                    if (i != j
                        && InOutConfigList[i].VideoIn == InOutConfigList[j].VideoIn
                        && InOutConfigList[i].VideoIn != 0
                        && InOutConfigList[j].VideoIn != 0
                        )
                    {
                        errormessage = string.Format("В списке имеются два одинаковых входа.");
                        return false;
                    }
                    if (i != j 
                        && InOutConfigList[i].SwitchOut == InOutConfigList[j].SwitchOut
                        && InOutConfigList[i].SwitchOut != 0
                        && InOutConfigList[j].SwitchOut != 0)
                    {
                        errormessage = string.Format("Входы {0} и {1} подсоединены к одному и тому же выходу коммутатора.", InOutConfigList[i].ToString(), InOutConfigList[j].ToString());
                        return false;
                    }
                }
            }
            errormessage = "OK";
            return true;
        }

        #endregion
    }
}