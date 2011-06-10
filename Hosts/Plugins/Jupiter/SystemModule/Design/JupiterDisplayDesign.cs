using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Hosts.Plugins.Jupiter.SystemModule.Config;
using TechnicalServices.Common.ReadOnly;
using TechnicalServices.Persistence.CommonPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Linq;
using System.Drawing;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.Jupiter.SystemModule.Design
{
    [Serializable]
    [XmlType("JupiterDisplay")]
    public class JupiterDisplayDesign : ActiveDisplay, ISegmentationSupport
    {
        public JupiterDisplayDesign()
        {
            IsVideoWall = true;
        }

        public override void Validate(Slide slide)
        {
            var list = (
                from d in slide.DisplayList
                where d.Type.Equals(this.Type)
                from w in d.WindowList
                where isVideoWindow(w as JupiterWindow)
                select w as JupiterWindow).ToList();
            validateIntersection(slide, list);
            validateResourceConsumption(slide, list);
        }

        private void validateResourceConsumption(Slide slide, IEnumerable<JupiterWindow> list)
        {
            if (this.ResourceAvailability == 0)
                return;
            int resourceConsumpted = 0;
            for (int i = 0; i < this.SegmentColumns; i++)
                for (int j = 0; j < this.SegmentRows; j++)
                {
                    foreach (var wnd in list)
                    {
                        Rectangle rect = new Rectangle(this.SegmentWidth * i, this.SegmentHeight * j, this.SegmentWidth, this.SegmentHeight);
                        Rectangle wndRect = new Rectangle(wnd.Left, wnd.Top, wnd.Width, wnd.Height);
                        if (rect.IntersectsWith(wndRect))
                            resourceConsumpted++;
                    }
                }
            if (resourceConsumpted > this.ResourceAvailability)
                throw new Exception(String.Concat("Сохранение сцены ",
                    slide.Name,
                    " невозможно. Превышено общее количество единиц системного ресурса"));
        }

        private void validateIntersection(Slide slide, IList<JupiterWindow> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    Window current = list[i];
                    Window other = list[j];
                    Rectangle rect1 = new Rectangle(current.Left, current.Top, current.Width, current.Height);
                    Rectangle rect2 = new Rectangle(other.Left, other.Top, other.Width, other.Height);
                    string currentResourceInfoName = current.Source.ResourceDescriptor == null
                                                         ? string.Empty
                                                         : current.Source.ResourceDescriptor.ResourceInfo.Name;
                    string otherResourceInfoName = other.Source.ResourceDescriptor == null
                                     ? string.Empty
                                     : other.Source.ResourceDescriptor.ResourceInfo.Name;
                    if (rect1.IntersectsWith(rect2))
                        throw new Exception(String.Concat(
                            "Сохранение сцены ",
                            slide.Name,
                            " невозможно. На раскладке сцены окна с источниками ",
                            currentResourceInfoName,
                            " и ",
                            otherResourceInfoName,
                            " не должны перекрываться между собой"));
                }
            }
        }

        public override void AfterWindowAdded(Window wnd, Slide slide)
        {
            if (wnd is JupiterWindow)
            {
                JupiterModule._windowMapping[wnd as JupiterWindow] = this;
                JupiterModule._slideMapping[wnd as JupiterWindow] = slide;
            }
        }

        internal int[] getAvailableInputs(JupiterWindow wnd)
        {
            Slide slide = JupiterModule._slideMapping[wnd];
            var busyInputs = from d in slide.DisplayList where d.Type.Equals(this.Type) from w in d.WindowList where (w is JupiterWindow && w != wnd) select (w as JupiterWindow).VideoIn;
            var list = getAvailableInputs(wnd.Source);
            return (from i in list where !busyInputs.Contains(i) select i).ToArray();
        }

        protected override Window CreateWindowProtected(Source source, Slide slide)
        {
            if (source is HardwareSource)
            {
                //return new JupiterWindow(); // временно пока не реализована таблица совместимости
                var busyInputs = from d in slide.DisplayList where d.Type.Equals(this.Type) from w in d.WindowList where w is JupiterWindow select (w as JupiterWindow).VideoIn;
                JupiterWindow wnd = new JupiterWindow();
                AfterWindowAdded(wnd, slide);
                var list = getAvailableInputs(source);
                if (list.Count() == 0)
                    throw new Exception(String.Concat("Источник недопустим для данного дисплея. Добавление источника ", source.Name, " на раскладку невозможно"));
                var freeInputs = from io in list where !busyInputs.Contains(io) select io;
                if (freeInputs.Count() == 0)
                    throw new Exception(String.Concat("Свободные входы на видеостене отсутствуют. Добавление источника ", source.Name, " на раскладку невозможно"));
                wnd.VideoIn = freeInputs.First();
                return wnd;
            }
            return new ActiveWindow();
        }

        private IEnumerable<int> getAvailableInputs(Source source)
        {
            return (this.Type as JupiterDisplayConfig).MappingList.Where(x => x is JupiterMapping).Cast<JupiterMapping>().Where(x => x.Source.Equals(source.Type)).Select(x => x.VideoIn).ToList();
            //return from io in (this.Type as JupiterDisplayConfig).InOutConfigList where io.WindowType == WindowTypeEnum.RGB select io.VideoIn;
        }

        internal bool isVideoWindow(JupiterWindow wnd)
        {
            if (wnd == null)
                return false;
            var type = (from io in (this.Type as JupiterDisplayConfig).InOutConfigList where io.VideoIn == wnd.VideoIn select io.WindowType).First();
            return type == WindowTypeEnum.Video;
        }

        /// <summary>
        /// Сетевой адрес компьютера
        /// Получаем из Конфигуратора
        /// По умолчанию: localhost
        /// </summary>
        [Category("Настройки")]
        [DisplayName("URI")]
        [XmlIgnore]
        public string Address
        {
            get { return ((JupiterDisplayConfig)Type).Address; }
        }

        /// <summary>
        /// Количество горизонтальных рядов сегментов (кубов) (1..20)
        /// Получаем из Конфигуратора
        /// По умолчанию: 1
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public int SegmentRows
        {
            get { return ((JupiterDisplayConfig)Type).SegmentRows; }
        }

        /// <summary>
        /// Количество вертикальных рядов сегментов (кубов) (1..20)
        /// Получаем из Конфигуратора
        /// По умолчанию: 1
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public int SegmentColumns
        {
            get { return ((JupiterDisplayConfig)Type).SegmentColumns; }
        }

        /// <summary>
        /// Количество сегментов (кубов)
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Количество сегментов")]
        [XmlIgnore]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Size<int> SegmentAmount
        {
            get { return ((JupiterDisplayConfig)Type).SegmentAmount; }
        }

        /// <summary>
        /// Горизонтальное разрешение сегмента (куба) (в пикселах) (1..4096)
        /// Получаем из Конфигуратора
        /// По умолчанию: 1024
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public int SegmentWidth
        {
            get { return ((JupiterDisplayConfig)Type).SegmentWidth; }
        }

        /// <summary>
        /// Вертикальное разрешение сегмента (куба) (в пикселах) (1..4096)
        /// Получаем из Конфигуратора
        /// По умолчанию: 768
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public int SegmentHeight
        {
            get { return ((JupiterDisplayConfig)Type).SegmentHeight; }
        }

        /// <summary>
        /// Разрешение сегмента (куба) (в пикселах)
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Разрешение сегмента (px)")]
        [XmlIgnore]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Size<int> SegmentResolution
        {
            get { return ((JupiterDisplayConfig)Type).SegmentResolution; }
        }

        /// <summary>
        /// Ширина видеостены (в метрах) (1..20)
        /// Получаем из Конфигуратора
        /// По умолчанию: 1
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public decimal WidthM
        {
            get { return ((JupiterDisplayConfig)Type).WidthM; }
        }

        /// <summary>
        /// Высота видеостены (в метрах) (1..20)
        /// Получаем из Конфигуратора
        /// По умолчанию: 1
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public decimal HeightM
        {
            get { return ((JupiterDisplayConfig)Type).HeightM; }
        }

        /// <summary>
        /// Линейный размер видеостены (в метрах)
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Размер (м)")]
        [XmlIgnore]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Size<decimal> SizeM
        {
            get { return ((JupiterDisplayConfig)Type).SizeM; }
        }

        /// <summary>
        /// Удаленность видеостены от зрителей (в метрах) (1..32)
        /// Получаем из Конфигуратора
        /// По умолчанию: 5
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Удаленность (м)")]
        [XmlIgnore]
        public int DistanceM
        {
            get { return ((JupiterDisplayConfig)Type).DistanceM; }
        }

        /// <summary>
        /// Общее доступное количество ресурса видеостены (0..500)
        /// Получаем из Конфигуратора
        /// По умолчанию: 0 (ограничения на количество ресурса нет)
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Количество ресурса")]
        [XmlIgnore]
        public int ResourceAvailability
        {
            get { return ((JupiterDisplayConfig)Type).ResourceAvailability; }
        }

        /// <summary>
        /// Входы видеостены
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Входы видеостены")]
        [XmlIgnore]
        public ReadOnlyCollection InOutConfigList
        {
            get { return new ReadOnlyCollection(((JupiterDisplayConfig)Type).InOutConfigList); }
        }

        public override ResourceDescriptor[] ApplyAdditionalFilter(ResourceDescriptor[] descriptors, Window currentWindow)
        {
            ResourceDescriptor[] des = base.ApplyAdditionalFilter(descriptors, currentWindow);
            JupiterWindow jupiterWindow = currentWindow as JupiterWindow;
            if (jupiterWindow == null) return des;
            List<ResourceDescriptor> list = new List<ResourceDescriptor>(descriptors.Length);
            IEnumerable<JupiterMapping> mapping = this.Type.MappingList.OfType<JupiterMapping>().Where(jm=>jm.VideoIn == jupiterWindow.VideoIn);
            foreach (ResourceDescriptor descriptor in des)
            {
                int videoIn = jupiterWindow.VideoIn;
                if (descriptor != null && descriptor.ResourceInfo != null && descriptor.ResourceInfo.IsHardware && descriptor.ResourceInfo.SourceType != null)
                {
                    if (mapping.Any(map=>map.Source.Equals(descriptor.ResourceInfo.SourceType)))
                    {
                        list.Add(descriptor);
                    }
                }
            }
            return list.ToArray();
        }
    }
}