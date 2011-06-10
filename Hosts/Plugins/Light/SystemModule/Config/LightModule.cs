using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Light.SystemModule.Config
{
    public sealed class LightModule : ConfigurationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(LightDeviceConfig) };
        }
    }
}