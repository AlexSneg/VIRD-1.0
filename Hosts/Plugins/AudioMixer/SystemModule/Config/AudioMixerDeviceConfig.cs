using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Xml.Serialization;

using Hosts.Plugins.AudioMixer.SystemModule.Design;
using TechnicalServices.Common;
using TechnicalServices.Common.Editor;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.CommonPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.AudioMixer.SystemModule.Config
{
    [Serializable]
    [XmlType("AudioMixer")]
    public class AudioMixerDeviceConfig : DeviceType, ICustomTypeDescriptor, ICollectionItemValidation
    {
        private List<AudioMixerFaderGroupConfig> _faderGroupList = new List<AudioMixerFaderGroupConfig>();
        private bool _hasMatrix = true;
        private List<AudioMixerInput> _inputList = new List<AudioMixerInput>();
        private string _instanceID = "0";
        private List<AudioMixerOutput> _outputList = new List<AudioMixerOutput>();

        public AudioMixerDeviceConfig()
        {
        }

        public AudioMixerDeviceConfig(string name)
        {
            Name = name;
            Type = "Аудиомикшер";
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return true; }
        }

        /// <summary>
        /// Наличие матрицы логического матричного микшера 
        /// Обязательный параметр
        /// По умолчанию: True
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Наличие матрицы")]
        [DefaultValue(true)]
        [XmlAttribute("HasMatrix")]
        [TypeConverter(typeof (YesNoConverter))]
        [RefreshProperties(RefreshProperties.All)]
        public bool HasMatrix
        {
            get { return _hasMatrix; }
            set { _hasMatrix = value; }
        }

        /// <summary>
        /// Instance ID матричного микшера
        /// Обязательный параметр
        /// По умолчанию: "0"
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Instance ID матричного микшера")]
        [DefaultValue("0")]
        [XmlAttribute("InstanceID")]
        [MatrixRequired]
        public string InstanceID
        {
            get { return _instanceID; }
            set
            {
                value = ValidationHelper.CheckIsNullOrEmpty(value, "Instance ID матричного микшера");
                _instanceID = ValidationHelper.CheckLength(value, 50, "Instance ID матричного микшера");
            }
        }

        /// <summary>
        /// Входы матрицы логического матричного микшера
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Входы")]
        [XmlArray("AudioMixerInputList")]
        [TypeConverter(typeof (CollectionNameConverter))]
        [MatrixRequired]
        [Editor(typeof(AudioMixerInputEditor), typeof(UITypeEditor))]
        [CollectionName("Входы")]
        [CollectionFormName("Входы")]
        public List<AudioMixerInput> InputList
        {
            get { return _inputList; }
            set { _inputList = value; }
        }

        /// <summary>
        /// Выходы матрицы логического матричного микшера
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Выходы")]
        [XmlArray("AudioMixerOutputList")]
        [TypeConverter(typeof (CollectionNameConverter))]
        [MatrixRequired]
        [Editor(typeof(AudioMixerOutputEditor), typeof(UITypeEditor))]
        [CollectionName("Выходы")]
        [CollectionFormName("Выходы")]
        public List<AudioMixerOutput> OutputList
        {
            get { return _outputList; }
            set { _outputList = value; }
        }

        /// <summary>
        /// Группы фейдеров
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Группы фейдеров")]
        [XmlArray("AudioMixerFaderGroupList")]
        [TypeConverter(typeof (CollectionNameConverter))]
        [Editor(typeof (AudioMixerFaderGroupEditor), typeof (UITypeEditor))]
        [CollectionFormName("Группы фейдеров")]
        [CollectionName("Группы фейдеров")]
        [PropertiesName("Параметры групп фейдеров")]
        public List<AudioMixerFaderGroupConfig> FaderGroupList
        {
            get { return _faderGroupList; }
            set { _faderGroupList = value; }
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
            PropertyDescriptorCollection propColl = attributes == null
                                                        ? TypeDescriptor.GetProperties(this, true)
                                                        : TypeDescriptor.GetProperties(this, attributes, true);
            // в зависимости от наличия матрицы доступны различные свойства
            List<PropertyDescriptor> newColl = new List<PropertyDescriptor>(propColl.Count);
            foreach (PropertyDescriptor propertyDescriptor in propColl)
            {
                MatrixRequiredAttribute attr =
                    (MatrixRequiredAttribute) propertyDescriptor.Attributes[typeof (MatrixRequiredAttribute)];
                if (HasMatrix || (attr == null))
                    newColl.Add(propertyDescriptor);
            }
            return new PropertyDescriptorCollection(newColl.ToArray(), true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion

        public override Device CreateNewDevice()
        {
            return new AudioMixerDeviceDesign {Type = this};
        }

        public bool ValidateItem(out string errorMessage)
        {
            foreach (IGrouping<string, AudioMixerInput> grouping in InputList.GroupBy(i => i.Name))
            {
                if (grouping.Count() > 1)
                {
                    errorMessage = string.Format("В списке есть входы с одинаковым названием '{0}'.", grouping.Key);
                    return false;
                }
            }

            foreach (IGrouping<string, AudioMixerOutput> grouping in OutputList.GroupBy(i => i.Name))
            {
                if (grouping.Count() > 1)
                {
                    errorMessage = string.Format("В списке есть выходы с одинаковым названием '{0}'.", grouping.Key);
                    return false;
                }
            }

            foreach (IGrouping<string, AudioMixerFaderGroupConfig> grouping in FaderGroupList.GroupBy(i => i.Name))
            {
                if (grouping.Count() > 1)
                {
                    errorMessage = string.Format("В списке есть группы фейдеров с одинаковым названием '{0}'.", grouping.Key);
                    return false;
                }
            }

            //for (int i = 0; i < FaderGroupList.Count; i++)
            //{
            //    for (int j = 0; j < FaderGroupList[i].FaderList.Count; j++)
            //        for (int k = 0; k < FaderGroupList.Count; k++)
            //        {
            //            if (i == k) continue;
            //            for (int l = 0; l < FaderGroupList[k].FaderList.Count; l++)
            //            {
            //                if (FaderGroupList[i].FaderList[j].InstanceID == FaderGroupList[k].FaderList[l].InstanceID)
            //                {
            //                    errorMessage = string.Format("В списке есть фейдеры с одинаковым InstanceID {4} (группа '{0}', фейдер '{1}' и группа '{2}', фейдер '{3}').",
            //                                                 FaderGroupList[i].Name, FaderGroupList[i].FaderList[j].Name,
            //                                                 FaderGroupList[k].Name, FaderGroupList[k].FaderList[l].Name,
            //                                                 FaderGroupList[i].FaderList[j].InstanceID);
            //                    return false;
            //                }
            //            }
            //        }
            //}


            errorMessage = null;
            return true;
        }
    }
}