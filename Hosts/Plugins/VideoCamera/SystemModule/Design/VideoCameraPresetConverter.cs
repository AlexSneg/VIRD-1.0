using System;
using System.ComponentModel;
using System.Globalization;

namespace Hosts.Plugins.VideoCamera.SystemModule.Design
{
    public class VideoCameraPresetConverter : TypeConverter
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

        private int GetPresetAmount(ITypeDescriptorContext context)
        {
            if (context == null) return 0;

            VideoCameraDeviceDesign design = (VideoCameraDeviceDesign)context.Instance;
            if (design == null) return 0;

            return design.PresetAmount;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            int presetAmount = GetPresetAmount(context);
            if (presetAmount == 0) return null;

            int[] presetList = new int[presetAmount+1];
            for (int i = 0; i <= presetAmount; i++)
                presetList[i] = i;

            return new StandardValuesCollection(presetList);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return destType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            return ((value != null) && (value is int) && ((int)value >= 1) ? value.ToString() : null);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
        {
            return srcType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            int buf;
            Int32.TryParse((string) value, out buf);
            return buf;
        }
    }
}