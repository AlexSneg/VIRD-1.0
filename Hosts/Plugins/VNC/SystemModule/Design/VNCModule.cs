using System;
using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.VNC.SystemModule.Design
{
    public sealed class VNCModule : PresentationModule
    {
        public override Type[] GetSource()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof (VNCSourceDesign));
            return list.ToArray();
        }
    }
}