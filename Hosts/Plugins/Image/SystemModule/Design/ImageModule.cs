using System;
using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Image.SystemModule.Design
{
    public sealed class ImageModule : PresentationModule
    {
        public override Type[] GetSource()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof (ImageSourceDesign));
            return list.ToArray();
        }
    }
}