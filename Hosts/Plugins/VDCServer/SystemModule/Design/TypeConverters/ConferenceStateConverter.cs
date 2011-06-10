using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace Hosts.Plugins.VDCServer.SystemModule.Design.TypeConverters
{
    /// <summary>
    /// Состоянии конференции (активна/неактивна).
    /// </summary>
    public class ConferenceStateConverter : BooleanConverter
    {
        string OnString = "Активна";
        string OffString = "Не активна";

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            return (bool)value ? OnString : OffString;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return (string)value == OnString;
        }
    }
}
