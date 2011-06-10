using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Hosts.Plugins.VDCServer.SystemModule.Config;

namespace Hosts.Plugins.VDCServer.SystemModule.Design.TypeConverters
{
    /// <summary>
    /// TypeConverter для выбора раскладки.
    /// </summary>
    public class ActiveLayoutConverter: TypeConverter
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
            return new StandardValuesCollection(
            GetScreenLayoutList(context)//.Select(m => m.ToString()).ToList() //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1682
            );
        }

        List<ScreenLayout> GetScreenLayoutList(ITypeDescriptorContext context)
        {
            return ((VDCServer.SystemModule.Config.VDCServerDeviceConfig)((VDCServerDeviceDesign)context.Instance).Type).ScreenLayoutList;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            foreach (ScreenLayout layout in GetScreenLayoutList(context))
            {
                if(layout.ToString()==value.ToString()) return layout;
            }
            return null;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }
    }
}
