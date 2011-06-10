using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Jupiter.SystemModule.Config
{
    public sealed class JupiterModule : ConfigurationModule
    {
        public override Type[] GetDisplay()
        {
            return new[] { typeof(JupiterDisplayConfig) };
        }

        public override Type[] GetDevice()
        {
            return new[] { typeof(JupiterDeviceConfig) };
        }

        public override Type[] GetMappingType()
        {
            return new[] { typeof(JupiterMapping) };
        }
    }
}