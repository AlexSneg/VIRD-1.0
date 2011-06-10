using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

using TechnicalServices.Entity;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Common;

namespace Hosts.Plugins.Light.SystemModule.Config
{
    [Serializable]
    [XmlType("LightUnit")]
    public class LightUnitConfig: ICollectionItemValidation, ICloneable
    {
        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Название группы освещения
        /// Обязательный параметр
        /// Название уникальное в пределах одного экземпляра освещения
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Название группы")]
        [XmlAttribute("Name")]
        public string Name 
        { 
            get
            {
                return _name;
            }
            set
            {
                value = ValidationHelper.CheckLength(value, 128, "Название группы освещения");
                _name = ValidationHelper.CheckIsNullOrEmpty(value, "Название группы освещения");
            }
        
        }
        private string _name;

        /// <summary>
        /// Тип группы (нерегулируемая/регулируемая)
        /// Обязательный параметр
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Тип группы")]
        [XmlAttribute("IsAdjustable")]
        [TypeConverter(typeof(LightIsAdjustableConverter))]
        public bool IsAdjustable { get; set; }

        #region ICollectionItemValidation Members

        bool ICollectionItemValidation.ValidateItem(out string errorMessage)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                errorMessage = "Название группы должно быть заполнено.";
                return false;
            }
            errorMessage = "OK";
            return true;
        }

        #endregion
    }



    [Serializable]
    [XmlType("LightUnit")]
    public class LightUnitDesign
    {
        public LightUnitDesign()
        {
        }

        public LightUnitDesign(LightUnitConfig unitConfig) : this()
        {
            Name = unitConfig.Name;
            IsAdjustable = unitConfig.IsAdjustable;
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Название группы освещения
        /// Обязательный параметр
        /// Название уникальное в пределах одного экземпляра освещения
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Тип группы (нерегулируемая/регулируемая)
        /// Обязательный параметр
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public bool IsAdjustable { get; set; }

        /// <summary>
        /// Уровень яркости группы
        /// Для нерегулируемых: включена/выключена (0..1)
        /// Для регулируемых: уровень яркости (0..100)
        /// Обязательный параметр
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("Brightness")]
        public int Brightness { get; set; }
    }



    public class LightUnitDescriptor : PropertyDescriptor
    {
        private readonly LightUnitDesign unit;

        public LightUnitDescriptor(LightUnitDesign unit, Attribute[] attrs) : base(unit.Name, attrs)
        {
            this.unit = unit;
        }

        #region PropertyDescriptor Members

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return null; }
        }

        public override TypeConverter Converter
        {
            get { return unit.IsAdjustable ? null : new LightBrightnessConverter(); }
        }

        protected override void FillAttributes(IList attributeList)
        {
            attributeList.Add(new CategoryAttribute("Группы освещения"));
        }

        public override object GetValue(object component)
        {
            return unit.Brightness;
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return unit.Brightness.GetType(); }
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            unit.Brightness = Convert.ToInt32(value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        #endregion
    }



    public class LightIsAdjustableConverter : BooleanConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            return (bool)value ? "Регулируемая" : "Нерегулируемая";
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return (string)value == "Регулируемая";
        }
    }



    public class LightBrightnessConverter : BooleanConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            return Convert.ToBoolean(value) ? "Вкл" : "Выкл";
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return (string)value == "Вкл" ? 1 : 0;
        }
    }
}