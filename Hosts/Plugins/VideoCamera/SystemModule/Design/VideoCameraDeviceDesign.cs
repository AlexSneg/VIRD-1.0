using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Hosts.Plugins.VideoCamera.SystemModule.Config;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.VideoCamera.SystemModule.Design
{
    [Serializable]
    [XmlType("VideoCameraDevice")]
    public class VideoCameraDeviceDesign : DeviceAsSource, ICustomTypeDescriptor
    {
        /// <summary>
        /// Количество поддерживаемых камерой пресетов (1..100)
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Количество пресетов")]
        [XmlIgnore, ReadOnly(true)]
        public int PresetAmount
        {
            get { return ((VideoCameraDeviceConfig)Type).PresetAmount; }
        }

        /// <summary>
        /// Признак купольной камеры
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Купольная камера")]
        [XmlIgnore, ReadOnly(true)]
        [TypeConverter(typeof(YesNoConverter))]
        public bool IsDomical
        {
            get { return ((VideoCameraDeviceConfig)Type).IsDomical; }
        }

        /// <summary>
        /// Возможность точно управлять углами и увеличением камеры
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Точное управление")]
        [XmlIgnore, ReadOnly(true)]
        [TypeConverter(typeof(YesNoConverter))]
        public bool HasPreciseControl
        {
            get { return ((VideoCameraDeviceConfig)Type).HasPreciseControl; }
        }

        /// <summary>
        /// Нижний порог зума (0..50)
        /// Доступно только при HasPreciseControl = True
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Нижний порог масштабирования")]
        [XmlIgnore, ReadOnly(true)]
        [PreciseControlRequired]
        public decimal LowZoomBoundary
        {
            get { return ((VideoCameraDeviceConfig)Type).LowZoomBoundary; }
        }

        /// <summary>
        /// Верхний порог зума (0..50)
        /// Доступно только при HasPreciseControl = True
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Верхний порог масштабирования")]
        [XmlIgnore, ReadOnly(true)]
        [PreciseControlRequired]
        public decimal HighZoomBoundary
        {
            get { return ((VideoCameraDeviceConfig)Type).HighZoomBoundary; }
        }

        /// <summary>
        /// Угол поворота (0..360)
        /// Доступно только при HasPreciseControl = True
        /// По умолчанию: 0
        /// </summary>
        [DefaultValue(0)]
        [Category("Настройки")]
        [DisplayName("Pan")]
        [XmlAttribute("Pan")]
        [PreciseControlRequired]
        public int Pan
        {
            get { return _pan; }
            set { _pan = (value < 0 ? 0 : (value > 360 ? 360 : value)); }
        }
        private int _pan;

        /// <summary>
        /// Угол наклона (-180..+180)
        /// Доступно только при HasPreciseControl = True
        /// По умолчанию: 0
        /// </summary>
        [DefaultValue(0)]
        [Category("Настройки")]
        [DisplayName("Tilt")]
        [XmlAttribute("Tilt")]
        [PreciseControlRequired]
        public int Tilt
        {
            get { return _tilt; }
            set { _tilt = (value < -180 ? -180 : (value > 180 ? 180 : value)); }
        }
        private int _tilt;

        /// <summary>
        /// Зум (LowZoomBoundary..HighZoomBoundary)
        /// Доступно только при HasPreciseControl = True
        /// По умолчанию: 1
        /// </summary>
        [DefaultValue(1)]
        [Category("Настройки")]
        [DisplayName("Zoom")]
        [XmlAttribute("Zoom")]
        [PreciseControlRequired]
        public decimal Zoom
        {
            get { return _zoom; }
            set {
                _zoom = Type == null ? value : (value < LowZoomBoundary ? LowZoomBoundary : (value > HighZoomBoundary ? HighZoomBoundary : value));
            }
        }
        private decimal _zoom = 1;

        /// <summary>
        /// Номер пресета (1..PresetAmount)
        /// Пресет содержит сохраненные значения Pan, Tilt, Zoom
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Пресет")]
        [XmlAttribute("Preset")]
        [TypeConverter(typeof(VideoCameraPresetConverter))]
        public int Preset
        {
            get { return _preset; }
            set 
            {
                if (value >= 0 || value <= PresetAmount)
                {
                    _preset = value;
                    if ((_preset != 0) && (HasPreciseControl))
                    {
                        LoadPreset(_preset);
                    }
                }
            }
        }
        private int _preset;

        /// <summary>
        /// Сохранить текущее положение камеры (значения Pan, Tilt, Zoom) в пресет с указанным номером
        /// </summary>
        public void SavePreset(int preset)
        {
            if (DeviceResourceDescriptor == null || DeviceResourceDescriptor.ResourceInfo == null)
                return;
            VideoCameraDeviceResourceInfo info = DeviceResourceDescriptor.ResourceInfo as VideoCameraDeviceResourceInfo;

            PresetStore store = info.PresetStores.Find(ps => ps.Preset == preset);
            if (store == null)
            {
                store = new PresetStore(preset, Pan, Tilt, Zoom);
                info.PresetStores.Add(store);
            }
            else
            {
                store.Pan = Pan;
                store.Tilt = Tilt;
                store.Zoom = Zoom;
            }
        }
        private void LoadPreset(int preset)
        {
            if (DeviceResourceDescriptor == null || DeviceResourceDescriptor.ResourceInfo == null)
                return;
            VideoCameraDeviceResourceInfo info = DeviceResourceDescriptor.ResourceInfo as VideoCameraDeviceResourceInfo;

            PresetStore store = info.PresetStores.Find(ps => ps.Preset == preset);
            if (store == null)
            {
                //Home();
                return;
            }
            //_preset = store.Preset;
            Tilt = store.Tilt;
            Pan = store.Pan;
            Zoom = store.Zoom;
        }

        /// <summary>
        /// Возврат камеры в начальное состояние
        /// При HasPreciseControl = True инициализация значений Pan, Tilt, Zoom значениями по умолчанию
        /// </summary>
        public void Home()
        {
            _pan = 0; _tilt = 0; _zoom = 1;
        }

        [Browsable(false)]
        [XmlIgnore]
        public override TechnicalServices.Persistence.SystemPersistence.Resource.DeviceResourceDescriptor DeviceResourceDescriptor
        {
            get
            {
                return base.DeviceResourceDescriptor;
            }
            set
            {
                base.DeviceResourceDescriptor = value;
                if (null == value) return;
                VideoCameraDeviceResourceInfo info = value.ResourceInfo as VideoCameraDeviceResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа VideoCameraDeviceResourceInfo");
            }
        }


        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }

        public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection propColl;
            if (attributes == null)
                propColl = TypeDescriptor.GetProperties(this, true);
            else
                propColl = TypeDescriptor.GetProperties(this, attributes, true);
            // если камера не имеет точного управления, то ряд свойств недоступен
            if (!HasPreciseControl)
            {
                List<PropertyDescriptor> newColl = new List<PropertyDescriptor>(propColl.Count);
                foreach (PropertyDescriptor propertyDescriptor in propColl)
                    if (propertyDescriptor.Attributes[typeof(PreciseControlRequiredAttribute)] == null)
                        newColl.Add(propertyDescriptor);
                propColl = new PropertyDescriptorCollection(newColl.ToArray(), true);
            }
            return propColl;
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }
}