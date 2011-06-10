using System;
using System.ComponentModel;
using System.Globalization;

namespace TechnicalServices.Common.TypeConverters
{
    public class NotNullStringConverter : StringConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null || (value is string && (string)value == ""))
                throw new FormatException(String.Format("Значение поля {0} не может быть пустым",
                                                        context.PropertyDescriptor.DisplayName));
            return base.ConvertFrom(context, culture, value);
        }
    }
}