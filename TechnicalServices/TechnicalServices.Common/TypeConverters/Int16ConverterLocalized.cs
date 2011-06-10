using System;
using System.ComponentModel;
using System.Globalization;

namespace TechnicalServices.Common.TypeConverters
{
    public class Int16ConverterLocalized : Int16Converter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                Int16 result;
                if (Int16.TryParse((string) value, NumberStyles.Integer, culture, out result))
                {
                    return result;
                }
                throw new FormatException(String.Format("Значение поля {0} не может быть \"{1}\"",
                                                        context.PropertyDescriptor.DisplayName, value));
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}