using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

using TechnicalServices.Persistence.CommonPersistence.Configuration;

namespace TechnicalServices.Common.TypeConverters
{
    public class ModelConverter : TypeConverter
    {
        private readonly Dictionary<Type, Preset[]> _list = new Dictionary<Type, Preset[]>();

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            Type type = context.Instance.GetType();
            if (!_list.ContainsKey(type)) _list.Add(type, Preset.GetPresetting(type.Assembly));
            return new StandardValuesCollection(Preset.GetPresettingNames(_list[type]));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof (string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            return (value != null ? value.ToString() : String.Empty);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof (string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
                throw new FormatException(String.Format("Значение поля {0} не может быть пустым",
                                                        context.PropertyDescriptor.DisplayName));
            return value.ToString();
        }
    }
}