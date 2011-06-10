using System;
using System.ComponentModel;

namespace TechnicalServices.Common.TypeConverters
{
    public class ResourceFileInfoFullFileNameTypeConverter : StringConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return false;
        }
    }
}