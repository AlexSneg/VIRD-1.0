using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

using Hosts.Plugins.StandardSource.SystemModule.Design;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.StandardSource.SystemModule.Config
{
    [Serializable]
    [XmlType("StandardSourceSource")]
    public class StandardSourceSourceConfig : HardwareSourceType, ICustomTypeDescriptor
    {
        public StandardSourceSourceConfig()
        {
        }

        public StandardSourceSourceConfig(string name)
        {
            Name = name;
            Type = "Стандартный источник изображения";
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return true; }
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

            List<PropertyDescriptor> newColl = new List<PropertyDescriptor>(propColl.Count);
            foreach (PropertyDescriptor propertyDescriptor in propColl)
            {
                if (propertyDescriptor.Name == "DeviceType") continue;
                newColl.Add(propertyDescriptor);
            }
            return new PropertyDescriptorCollection(newColl.ToArray(), true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion

        public override Source CreateNewSource(Slide slide, ModuleConfiguration moduleConfiguration, Dictionary<String, IList<DeviceResourceDescriptor>> resources, Display display)
        {
            Source source = new StandardSourceSourceDesign {Type = this};
            StandardSourceDeviceConfig deviceConfig =
                (StandardSourceDeviceConfig) moduleConfiguration.DeviceList.SingleOrDefault(
                                                 dt =>
                                                 dt.GetType() == typeof (StandardSourceDeviceConfig) && dt.UID == UID);
            if (deviceConfig != null)
            {
                Device device =
                    slide.DeviceList.SingleOrDefault(
                        dev => dev.Type.Name.Equals(deviceConfig.Name) && dev.Type.Type.Equals(deviceConfig.Type));
                if (device == null)
                {
                    device = deviceConfig.CreateNewDevice(resources);
                    slide.DeviceList.Add(device);
                }
                source.Device = device;
            }
            return source;
        }

        protected override ResourceInfo CreateNewResourceInfoProtected()
        {
            return new StandardSourceResourceInfo();
        }

        public override DeviceType CreateDeviceType()
        {
            return new StandardSourceDeviceConfig(Name);
        }
    }
}
