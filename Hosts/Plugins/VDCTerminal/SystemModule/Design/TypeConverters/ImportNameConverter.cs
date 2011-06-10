using System;
using System.ComponentModel;
using System.Globalization;

namespace Hosts.Plugins.VDCTerminal.SystemModule.Design.TypeConverters
{
    public class ImportNameConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return "(Импорт абонентов)";
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}