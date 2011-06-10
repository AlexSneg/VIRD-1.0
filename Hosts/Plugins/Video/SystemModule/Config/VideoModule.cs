using System;
using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Video.SystemModule.Config
{
    public sealed class VideoModule : ConfigurationModule
    {
        public override Type[] GetSource()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof (VideoSourceConfig));
            return list.ToArray();
        }

        public override Type[] GetExtensionType()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof (VideoResourceInfo));
            return list.ToArray();
        }
    }
}