using System;
using System.Collections.Generic;
using System.Reflection;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Interfaces
{
    public static class KnownTypeProvider
    {
        private static IModule[] _moduleList;

        public static IModule[] ModuleList
        {
            set { _moduleList = value; }
        }

        public static IEnumerable<Type> GetKnownTypesForResourceDescriptor(ICustomAttributeProvider provider)
        {
            List<Type> result = new List<Type>();
            foreach (IModule module in _moduleList)
            {
                result.AddRange(module.SystemModule.Configuration.GetExtensionType());
                result.AddRange(module.SystemModule.Presentation.GetExtensionType());
            }
            result.Add(typeof(BackgroundImageDescriptor));
            result.Add(typeof(ResourceFileInfo));
            return result;
        }

        public static IEnumerable<Type> GetAllKnownTypes(ICustomAttributeProvider provider)
        {
            HashSet<Type> result = new HashSet<Type>();
            foreach (IModule module in _moduleList)
            {
                result.UnionWith(module.SystemModule.Configuration.GetDevice());
                result.UnionWith(module.SystemModule.Configuration.GetSource());
                result.UnionWith(module.SystemModule.Configuration.GetDisplay());
                result.UnionWith(module.SystemModule.Configuration.GetMappingType());

                result.UnionWith(module.SystemModule.Presentation.GetDevice());
                result.UnionWith(module.SystemModule.Presentation.GetSource());
                result.UnionWith(module.SystemModule.Presentation.GetDisplay());
                result.UnionWith(module.SystemModule.Presentation.GetWindow());
            }
            result.UnionWith(GetKnownTypesForResourceDescriptor(provider));
            result.UnionWith(GetObjectKeyKnownType(provider));
            return result;
        }

        public static IEnumerable<Type> GetObjectKeyKnownType(ICustomAttributeProvider provider)
        {
            HashSet<Type> result = new HashSet<Type>();
            result.Add(typeof (PresentationKey));
            result.Add(typeof(SlideKey));
            return result;
        }

        //public static IEnumerable<Type> GetKnownTypeForWindow(ICustomAttributeProvider provider)
        //{
        //    HashSet<Type> result = new HashSet<Type>();
        //    foreach (IModule module in _moduleList)
        //    {
        //        result.UnionWith(module.SystemModule.Presentation.GetSource());
        //        result.UnionWith(module.SystemModule.Presentation.GetWindow());
        //    }
        //    result.UnionWith(GetKnownTypesForResourceDescriptor(provider));
        //    return result;
        //}

    }
}