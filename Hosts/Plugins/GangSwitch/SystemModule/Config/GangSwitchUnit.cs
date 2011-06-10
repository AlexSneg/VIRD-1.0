using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using Hosts.Plugins.GangSwitch.SystemModule.Design;

using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Common;

namespace Hosts.Plugins.GangSwitch.SystemModule.Config
{
    [Serializable]
    [XmlType("GangSwitchUnit")]
    public class GangSwitchUnitConfig: ICollectionItemValidation, ICloneable
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
        /// Название переключателя
        /// Обязательный параметр
        /// Название уникальное в пределах одного экземпляра блока переключателей
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Название переключателя")]
        [XmlAttribute("Name")]
        public string Name {
            get
            {
                return _name;
            }
            set
            {
                value = ValidationHelper.CheckLength(value, 128, "Название переключателя");
                _name = ValidationHelper.CheckIsNullOrEmpty(value, "Название переключателя");
            }
        }
        private string _name;

        /// <summary>
        /// Название состояния «Вкл»
        /// Обязательный параметр
        /// Названия состояний «Вкл» и «Выкл» не должны совпадать
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Название состояния 'Вкл'")]
        [XmlAttribute("OnStateName")]
        public string OnStateName
        {
            get
            {
                return _onStateName;
            }
            set
            {
                value = ValidationHelper.CheckLength(value, 128, "Название состояния 'Вкл'");
                _onStateName = ValidationHelper.CheckIsNullOrEmpty(value, "Название состояния 'Вкл'");
                if (_onStateName == _offStateName)
                {
                    throw new ArgumentException("Названия состояний 'Вкл' и 'Выкл' должны отличаться.");
                }
            }
        }
        private string _onStateName;

        /// <summary>
        /// Название состояния «Выкл»
        /// Обязательный параметр
        /// Названия состояний «Вкл» и «Выкл» не должны совпадать
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Название состояния 'Выкл'")]
        [XmlAttribute("OffStateName")]
        public string OffStateName 
        {
            get
            {
                return _offStateName;
            }
            set
            {
                value = ValidationHelper.CheckLength(value, 128, "Название состояния 'Выкл'");
                _offStateName = ValidationHelper.CheckIsNullOrEmpty(value, "Название состояния 'Выкл'");
                if (_offStateName == _onStateName)
                {
                    throw new ArgumentException("Названия состояний 'Вкл' и 'Выкл' должны отличаться.");
                }
            }
        }
        private string _offStateName;

        #region ICollectionItemValidation Members

        bool ICollectionItemValidation.ValidateItem(out string errorMessage)
        {
            if (string.IsNullOrEmpty(Name))
            {
                errorMessage = "Среди переключателей имеется переключатель с незаполненным названием.";
                return false;
            }
            if (string.IsNullOrEmpty(OffStateName))
            {
                errorMessage = string.Format("У переключателя \"{0}\" не заполнено название состояния \"Выкл.\".", Name);
                return false;
            }
            if (string.IsNullOrEmpty(OnStateName))
            {
                errorMessage = string.Format("У переключателя \"{0}\" не заполнено название состояния \"Вкл.\".", Name);
                return false;
            }
            if (OnStateName == OffStateName)
            {
                errorMessage = string.Format("У переключателя \"{0}\" название состояния \"Вкл.\" совпадает с название состояния \"Выкл.\".", Name);
                return false;
            }
            errorMessage = "OK";
            return true;
        }

        #endregion
    }



    [Serializable]
    [XmlType("GangSwitchUnit")]
    public class GangSwitchUnitDesign
    {
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Название переключателя
        /// Обязательный параметр
        /// Название уникальное в пределах одного экземпляра блока переключателей
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Состояние (вкл/выкл)
        /// Обязательный параметр
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("OnOffState")]
        public bool OnOffState { get; set; }
    }



    public class GangSwitchUnitConverter : BooleanConverter
    {
        private string[] GetValues(ITypeDescriptorContext context)
        {
            string[] result = new[] { "Выкл", "Вкл" };
            if (context == null) return result;

            Object obj = context.Instance;
            if (context.Instance is IReadOnlyWrapper)
                obj = ((IReadOnlyWrapper)(context.Instance)).Wrapped;

            GangSwitchDeviceConfig config = (GangSwitchDeviceConfig)((GangSwitchDeviceDesign)obj).Type;
            GangSwitchUnitConfig unit = config.UnitList.Where(s => s.Name == context.PropertyDescriptor.Name).FirstOrDefault();
            if (unit == null) return result;

            return new[] { unit.OffStateName, unit.OnStateName };
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            return GetValues(context)[Convert.ToInt32((bool)value)];
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return (string)value == GetValues(context)[1];
        }
    }



    public class GangSwitchUnitDescriptor : PropertyDescriptor
    {
        private readonly GangSwitchUnitDesign unit;

        public GangSwitchUnitDescriptor(GangSwitchUnitDesign unit, Attribute[] attrs) : base(unit.Name, attrs)
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
            get { return new GangSwitchUnitConverter(); }
        }

        protected override void FillAttributes(System.Collections.IList attributeList)
        {
            attributeList.Add(new CategoryAttribute("Переключатели"));
        }

        public override object GetValue(object component)
        {
            return unit.OnOffState;
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return unit.OnOffState.GetType(); }
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            unit.OnOffState = (bool)value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        #endregion
    }
}