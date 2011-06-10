using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace Hosts.Plugins.AudioMixer.SystemModule.Design.TypeConverters
{
    class SceneConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return "(...)";
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
