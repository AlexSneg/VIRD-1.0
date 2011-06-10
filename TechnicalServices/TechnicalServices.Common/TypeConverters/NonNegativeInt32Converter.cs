using System;
using System.ComponentModel;
using System.Globalization;

namespace TechnicalServices.Common.TypeConverters
{
    public class NonNegativeInt32Converter : Int32Converter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                Int32 result;
                if (Int32.TryParse((string) value, NumberStyles.Integer, culture, out result))
                {
                    if (result < 0)
                        throw new FormatException(String.Format("Значение поля {0} не может быть меньше нуля",
                                                                context.PropertyDescriptor.DisplayName));
                    return result;
                }
                throw new FormatException(String.Format("Значение поля {0} не может быть \"{1}\"",
                                                        context.PropertyDescriptor.DisplayName, value));
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}