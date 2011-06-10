using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Light.SystemModule.Design
{
    public sealed class LightModule : PresentationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(LightDeviceDesign) };
        }
    }
}