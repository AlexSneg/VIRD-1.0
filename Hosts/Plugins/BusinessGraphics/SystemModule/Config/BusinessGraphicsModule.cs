using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;
using Hosts.Plugins.BusinessGraphics.SystemModule.Design;
using System.Collections.Generic;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Config
{
    public sealed class BusinessGraphicsModule : ConfigurationModule
    {
        public override Type[] GetSource()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof(BusinessGraphicsSourceConfig));
            return list.ToArray();
        }

        public override Type[] GetExtensionType()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof(BusinessGraphicsResourceInfo));
            list.Add(typeof(StyleResourceInfo));
            return list.ToArray();
        }

    }
}