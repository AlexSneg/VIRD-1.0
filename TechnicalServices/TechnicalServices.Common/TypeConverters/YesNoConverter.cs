using System;
using System.ComponentModel;
using System.Globalization;

namespace TechnicalServices.Common.TypeConverters
{
    public class YesNoConverter : BooleanConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            return (bool)value ? "Да" : "Нет";
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return (string)value == "Да";
        }
    }
}