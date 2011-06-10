using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TechnicalServices.Persistence.CommonPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Common.TypeConverters
{
    public class ResourceDescriptorConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
                                           CultureInfo culture, object value)
        {
            string val = value as string;
            ResourceDescriptor descriptor = null;
            if (val != null && context != null && context.Instance != null)
            {
                bool isLocal = val.Contains(ResourceDescriptor.LocalString);
                descriptor = GetDescriptor(!isLocal ? val : val.Replace(ResourceDescriptor.LocalString, String.Empty), isLocal, context);
                if (descriptor != null)
                    return descriptor;
            }
            descriptor = CurrentResourceDescriptor(context);
            if (descriptor != null) return descriptor;
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)) return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }

        private ResourceDescriptor CurrentResourceDescriptor(ITypeDescriptorContext context)
        {
            if (context == null || context.Instance == null) return null;
            Source source = context.Instance as Source;
            if (source == null) return null;
            return source.ResourceDescriptor;
        }

        private ResourceDescriptor GetDescriptor(string name, bool IsLocal, ITypeDescriptorContext context)
        {
            Source source = context.Instance as Source;
            if (source != null && source.ResourceDescriptor != null)
            {
                IResourceProvider resourceProvider = (IResourceProvider)source.ResourceDescriptor.Site.GetService(typeof(IResourceProvider));
                if (resourceProvider != null)
                {
                    ResourceDescriptor[] descriptors = resourceProvider.GetResourcesByType(
                        source.Type.Type, false);
                    //return descriptors.SingleOrDefault(
                    //    rd => rd.ResourceInfo.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && rd.IsLocal == IsLocal);
                    //Workaround по https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-2029
                    return descriptors.First(
                        rd => rd.ResourceInfo.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && rd.IsLocal == IsLocal);
                }
            }
            return null;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            Source source = context.Instance as Source;
            if (source != null && source.ResourceDescriptor != null)
            {
                IResourceProvider resourceProvider = (IResourceProvider)source.ResourceDescriptor.Site.GetService(typeof(IResourceProvider));
                if (resourceProvider != null)
                {
                    ResourceDescriptor[] descriptors = resourceProvider.GetResourcesByType(source.Type.Type, true).Distinct(resourcesByIdComparer).ToArray();
                    return new StandardValuesCollection(descriptors);
                }
            }
            return new StandardValuesCollection(new ResourceDescriptor[] { });
        }

        /// <summary>
        /// Компарер ресурсов по идентификатором.
        /// </summary>
        private class ResourcesByIdComparer : System.Collections.Generic.IEqualityComparer<ResourceDescriptor> // Workaround для https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-2029
        {

            #region IEqualityComparer<ResourceDescriptor> Members

            bool System.Collections.Generic.IEqualityComparer<ResourceDescriptor>.Equals(ResourceDescriptor x, ResourceDescriptor y)
            {
                return x.Id.Equals(y.Id);
            }

            int System.Collections.Generic.IEqualityComparer<ResourceDescriptor>.GetHashCode(ResourceDescriptor obj)
            {
                return obj.Id.GetHashCode();
            }

            #endregion
        };
        ResourcesByIdComparer resourcesByIdComparer = new ResourcesByIdComparer();


        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            object obj = ConvertFrom(context, Thread.CurrentThread.CurrentCulture, value);
            ResourceDescriptor resourceDescriptor = obj as ResourceDescriptor;
            if (resourceDescriptor != null &&
                GetStandardValues(context).Cast<ResourceDescriptor>().Contains(resourceDescriptor))
            {
                return true;
            }
            return base.IsValid(context, value);
        }

    }
}