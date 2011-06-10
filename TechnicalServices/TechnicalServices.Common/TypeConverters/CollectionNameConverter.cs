using System;
using System.ComponentModel;
using System.Globalization;

namespace TechnicalServices.Common.TypeConverters
{
    public class CollectionNameConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return string.Empty;
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}