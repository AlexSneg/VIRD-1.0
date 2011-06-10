using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace TechnicalServices.Common.TypeConverters
{
    public class FontFamilyConverter : TypeConverter
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
            List<string> fonts = new List<string>();
            foreach (FontFamily font in FontFamily.Families)
                fonts.Add(font.Name);
            return new StandardValuesCollection(fonts);
        }
    }
}