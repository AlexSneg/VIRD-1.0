using System;
using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Image.SystemModule.Config
{
    public sealed class ImageModule : ConfigurationModule
    {
        public override Type[] GetSource()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof (ImageSourceConfig));
            return list.ToArray();
        }

        public override Type[] GetExtensionType()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof (ImageResourceInfo));
            return list.ToArray();
        }
    }
}