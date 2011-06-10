using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.StandardSource.SystemModule.Design
{
    public sealed class StandardSourceModule : PresentationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(StandardSourceDeviceDesign) };
        }

        public override Type[] GetSource()
        {
            return new[] { typeof(StandardSourceSourceDesign) };
        }
    }
}