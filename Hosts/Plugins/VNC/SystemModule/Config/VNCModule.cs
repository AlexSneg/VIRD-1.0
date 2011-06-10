using System;
using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.VNC.SystemModule.Config
{
    public sealed class VNCModule : ConfigurationModule
    {
        public override Type[] GetSource()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof (VNCSourceConfig));
            return list.ToArray();
        }

        public override Type[] GetExtensionType()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof (VNCResourceInfo));
            return list.ToArray();
        }
    }
}