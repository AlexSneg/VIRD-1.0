using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;
using Hosts.Plugins.VDCTerminal.SystemModule.Design;

namespace Hosts.Plugins.VDCTerminal.SystemModule.Config
{
    public sealed class VDCTerminalModule : ConfigurationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(VDCTerminalDeviceConfig) };
        }

        public override Type[] GetSource()
        {
            return new[] { typeof(VDCTerminalSourceConfig) };
        }

        public override Type[] GetExtensionType()
        {
            return new[] { typeof(VDCTerminalResourceInfo) };
        }
    }
}