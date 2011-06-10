using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Hosts.Plugins.VDCServer.SystemModule.Design.TypeConverters
{
    /// <summary>
    /// TypeConverter для выбора фокуса.
    /// </summary>
    public class ActiveMemberConverter: TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            //true means show a combobox
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            //true will limit to list
            //false will show the list, but allow free-form entry
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            //return new StandardValuesCollection(BusinessGraphicsSourceDesign.DiagramStyleList);
            return new StandardValuesCollection(((VDCServerDeviceDesign)context.Instance).Members.Select(m=>m.ToString()).ToList());
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            foreach (VDCServerAbonentInfo member in ((VDCServerDeviceDesign)context.Instance).Members)
            {
                if(member.ToString()==value.ToString()) return member;
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
