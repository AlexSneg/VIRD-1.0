using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Computer.SystemModule.Design
{
    public sealed class ComputerModule : PresentationModule
    {
        public override Type[] GetDisplay()
        {
            return new[] { typeof(ComputerDisplayDesign) };
        }

        public override Type[] GetWindow()
        {
            return new[] { typeof(ComputerWindow) };
        }
    }
}