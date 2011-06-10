using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

using Hosts.Plugins.VideoCamera.SystemModule.Design;

using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.VideoCamera.SystemModule.Config
{
    [Serializable]
    [XmlType("VideoCameraDevice")]
    public class VideoCameraDeviceConfig : DeviceTypeAsSource, ICustomTypeDescriptor
    {
        private decimal _highZoomBoundary;
        private decimal _lowZoomBoundary;
        private int _presetAmount;

        public VideoCameraDeviceConfig()
        {
        }

        public VideoCameraDeviceConfig(string name)
        {
            Name = name;
            Type = "Видеокамера";
        }

        /// <summary>
        /// Количество поддерживаемых камерой пресетов (1..100)
        /// Обязательное значение
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Количество пресетов")]
        [XmlAttribute("PresetAmount")]
        public int PresetAmount
        {
            get { return _presetAmount; }
            set { _presetAmount = ValidationHelper.CheckRange(value, 0, 100, "Количество пресетов"); }
        }

        /// <summary>
        /// Признак купольной камеры
        /// Обязательное значение
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Купольная камера")]
        [XmlAttribute("IsDomical")]
        [TypeConverter(typeof (YesNoConverter))]
        public bool IsDomical { get; set; }

        /// <summary>
        /// Возможность точно управлять углами и увеличением камеры
        /// Обязательное значение
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Точное управление")]
        [XmlAttribute("HasPreciseControl")]
        [TypeConverter(typeof (YesNoConverter))]
        [RefreshProperties(RefreshProperties.All)]
        public bool HasPreciseControl { get; set; }

        /// <summary>
        /// Нижний порог зума (0..100)
        /// Доступно только при HasPreciseControl = True
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Нижний порог масштабирования")]
        [XmlAttribute("LowZoomBoundary")]
        [PreciseControlRequired]
        public decimal LowZoomBoundary
        {
            get { return _lowZoomBoundary; }
            set { _lowZoomBoundary = ValidationHelper.CheckRange(value, 0, 100, "Нижний порог масштабирования"); }
        }

        /// <summary>
        /// Верхний порог зума (0..50)
        /// Доступно только при HasPreciseControl = True
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Верхний порог масштабирования")]
        [XmlAttribute("HighZoomBoundary")]
        [PreciseControlRequired]
        public decimal HighZoomBoundary
        {
            get { return _highZoomBoundary; }
            set
            {
                _highZoomBoundary = ValidationHelper.CheckRange(value, LowZoomBoundary, 100,
                                                                "Верхний порог масштабирования");
            }
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return true; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool Visible
        {
            get { return false; }
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
                    if (propertyDescriptor.Attributes[typeof (PreciseControlRequiredAttribute)] == null)
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

        public override Device CreateNewDevice()
        {
            return new VideoCameraDeviceDesign {Type = this};
        }

        protected override ResourceInfo CreateNewResourceInfoProtected()
        {
            return new VideoCameraDeviceResourceInfo();
        }
    }

    public class PreciseControlRequiredAttribute : Attribute
    {
    }
}