using System.ComponentModel;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.Jupiter.SystemModule.Design
{
    public class JupiterVideoInTypeConverter : TypeConverter
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
            JupiterWindow wnd = context.Instance as JupiterWindow;
            return new StandardValuesCollection(JupiterModule._windowMapping[wnd].getAvailableInputs(wnd));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
        {
            return value != null ? value.ToString() : null;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            int result;
            int.TryParse((string) value, out result);
            return result;
        }
    }
}