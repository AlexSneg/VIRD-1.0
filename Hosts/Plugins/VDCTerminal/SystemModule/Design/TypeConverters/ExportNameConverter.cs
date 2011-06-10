using System;
using System.ComponentModel;
using System.Globalization;

namespace Hosts.Plugins.VDCTerminal.SystemModule.Design.TypeConverters
{
    public class ExportNameConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return "(Экспорт абонентов)";
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}