using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TechnicalServices.Common.TypeConverters
{
    public class ExpandableNameConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return string.Empty;
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
