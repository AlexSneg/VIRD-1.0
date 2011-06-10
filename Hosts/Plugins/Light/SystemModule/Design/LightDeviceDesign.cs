using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Hosts.Plugins.Light.SystemModule.Config;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.Light.SystemModule.Design
{
    [Serializable]
    [XmlType("Light")]
    public class LightDeviceDesign : Device, ICustomTypeDescriptor
    {
        /// <summary>
        /// Группы освещения
        /// </summary>
        [Browsable(false)]
        [XmlArray("LightUnitList")]
			//[TypeConverter(typeof(TechnicalServices.Common.TypeConverters.CollectionNameConverter))]
				public List<LightUnitDesign> UnitList
        {
            get
            {
                if (Type != null)
                    foreach (LightUnitConfig unitConfig in ((LightDeviceConfig)Type).UnitList)
                    {
                        LightUnitDesign unitDesign = _unitList.Where(s => s.Name == unitConfig.Name).FirstOrDefault();
                        if (unitDesign == null)
                            _unitList.Add(new LightUnitDesign(unitConfig));
                        else
                            unitDesign.IsAdjustable = unitConfig.IsAdjustable;
                    }
                return _unitList;
            }
            set { _unitList = value; }
        }
        private List<LightUnitDesign> _unitList = new List<LightUnitDesign>();

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
            // копируем существующие и добавляем динамические свойства
            List<PropertyDescriptor> newColl = new List<PropertyDescriptor>();
            foreach (PropertyDescriptor propertyDescriptor in propColl)
                newColl.Add(propertyDescriptor);
            foreach (LightUnitDesign unit in UnitList)
                newColl.Add(new LightUnitDescriptor(unit, attributes));
            return new PropertyDescriptorCollection(newColl.ToArray(), true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }
}