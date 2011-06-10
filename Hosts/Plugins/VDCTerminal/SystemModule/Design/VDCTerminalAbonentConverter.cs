using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Hosts.Plugins.VDCTerminal.SystemModule.Design
{
    public class VDCTerminalAbonentConverter : TypeConverter
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

        private List<VDCTerminalAbonentInfo> GetAbonentList(ITypeDescriptorContext context)
        {
            if (context == null) return null;
            // дотянемся до сорса
            System.Windows.Forms.GridItem gridItem = context as System.Windows.Forms.GridItem;
            if (gridItem == null) return null;
            VDCTerminalSourceDesign source = gridItem.Parent.Parent.Value as VDCTerminalSourceDesign;
            if (source == null || source.ResourceDescriptor == null || source.ResourceDescriptor.ResourceInfo == null) return null;

            //VDCTerminalDeviceDesign design = (VDCTerminalDeviceDesign)context.Instance;
            //if (design == null) return null;
            //if (design.DeviceResourceDescriptor == null) return null;
            //VDCTerminalResourceInfo resource = (VDCTerminalResourceInfo)design.DeviceResourceDescriptor.ResourceInfo;
            VDCTerminalResourceInfo resource = source.ResourceDescriptor.ResourceInfo as VDCTerminalResourceInfo;
            if (resource == null) return null;

            return resource.AbonentList;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<VDCTerminalAbonentInfo> abonList = GetAbonentList(context);
            if (abonList == null) return null;

            VDCTerminalAbonentInfo[] abonListClone = new VDCTerminalAbonentInfo[abonList.Count+1];
            abonList.CopyTo(abonListClone, 1);

            return new StandardValuesCollection(abonListClone);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return destType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            return (value != null ? value.ToString() : null);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
        {
            return srcType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null) return null;

            List<VDCTerminalAbonentInfo> abonList = GetAbonentList(context);
            if (abonList == null) return null;

            foreach (VDCTerminalAbonentInfo abon in abonList)
                if (abon.ToString() == (string)value)
                    return abon;

            return null;
        }
    }
}