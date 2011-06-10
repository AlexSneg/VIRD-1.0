using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.VDCTerminal.SystemModule.Design
{
    public sealed class VDCTerminalModule : PresentationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(VDCTerminalDeviceDesign) };
        }

        public override Type[] GetSource()
        {
            return new[] { typeof(VDCTerminalSourceDesign) };
        }
    }
}