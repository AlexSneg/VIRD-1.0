using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;
using Hosts.Plugins.ArcGISMap.SystemModule.Design;
using System.Collections.Generic;

namespace Hosts.Plugins.ArcGISMap.SystemModule.Config
{
    public sealed class ArcGISMapModule : ConfigurationModule
    {
        public override Type[] GetSource()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof(ArcGISMapSourceConfig));
            return list.ToArray();
        }

        public override Type[] GetExtensionType()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof(ArcGISMapResourceInfo));
            return list.ToArray();
        }

    }
}