using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Monitor.SystemModule.Config
{
    public sealed class MonitorModule : ConfigurationModule
    {
        public override Type[] GetDisplay()
        {
            return new[] { typeof(MonitorDisplayConfig) };
        }
    }
}