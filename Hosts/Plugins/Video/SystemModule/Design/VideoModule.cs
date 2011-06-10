using System;
using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Video.SystemModule.Design
{
    public sealed class VideoModule : PresentationModule
    {
        public override Type[] GetSource()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof (VideoSourceDesign));
            return list.ToArray();
        }
    }
}