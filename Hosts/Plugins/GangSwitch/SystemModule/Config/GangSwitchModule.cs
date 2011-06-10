using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.GangSwitch.SystemModule.Config
{
    public sealed class GangSwitchModule : ConfigurationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(GangSwitchDeviceConfig) };
        }
    }
}