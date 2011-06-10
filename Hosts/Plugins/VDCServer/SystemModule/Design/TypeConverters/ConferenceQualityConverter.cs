using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Hosts.Plugins.VDCServer.SystemModule.Design.TypeConverters
{
    /// <summary>
    /// TypeConverter для качества соединения (выбор из списка).
    /// </summary>
    public class ConferenceQualityConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new[] 
            {
                "Auto",
                "128 kbps", 
                "256 kbps", 
                "384 kbps", 
                "512 kbps", 
                "640 kbps", 
                "768 kbps", 
                "896 kbps", 
                "1024 kbps", 
                "1280 kbps", 
                "1536 kbps", 
                "1792 kbps", 
                "2048 kbps"            
            });
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
        {
            return (value != null ? value.ToString() : null);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return value;
        }
    }
}
