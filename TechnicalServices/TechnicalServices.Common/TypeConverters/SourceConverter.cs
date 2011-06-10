using System;
using System.ComponentModel;
using System.Globalization;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace TechnicalServices.Common.TypeConverters
{
    public class SourceConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)) return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Source source = value as Source;
            if (source != null)
            {
                return source.Type.Type;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}