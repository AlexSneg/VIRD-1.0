using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Hosts.Plugins.GangSwitch.SystemModule.Config;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.GangSwitch.SystemModule.Design
{
    [Serializable]
    [XmlType("GangSwitch")]
    public class GangSwitchDeviceDesign : Device, ICustomTypeDescriptor
    {
        /// <summary>
        /// Переключатели
        /// </summary>
        [Browsable(false)]
        [XmlArray("GangSwitchUnitList")]
			[TypeConverter(typeof(TechnicalServices.Common.TypeConverters.CollectionNameConverter))]  
				public List<GangSwitchUnitDesign> UnitList
        {
            get
            {
                if (Type != null)
                    foreach (GangSwitchUnitConfig unit in ((GangSwitchDeviceConfig)Type).UnitList)
                        if (_unitList.Where(s => s.Name == unit.Name).FirstOrDefault() == null)
                            _unitList.Add(new GangSwitchUnitDesign { Name = unit.Name });
                return _unitList;
            }
            set { _unitList = value; }
        }
        private List<GangSwitchUnitDesign> _unitList = new List<GangSwitchUnitDesign>();

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
            foreach (GangSwitchUnitDesign unit in UnitList)
                newColl.Add(new GangSwitchUnitDescriptor(unit, attributes));
            return new PropertyDescriptorCollection(newColl.ToArray(), true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }
}