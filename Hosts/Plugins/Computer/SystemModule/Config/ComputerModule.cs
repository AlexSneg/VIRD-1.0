using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Computer.SystemModule.Config
{
    public sealed class ComputerModule : ConfigurationModule
    {
        public override Type[] GetDisplay()
        {
            return new[] { typeof(ComputerDisplayConfig) };
        }
    }
}