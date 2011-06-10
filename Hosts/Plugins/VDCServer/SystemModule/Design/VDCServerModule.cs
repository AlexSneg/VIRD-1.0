using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.VDCServer.SystemModule.Design
{
    public sealed class VDCServerModule : PresentationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(VDCServerDeviceDesign) };
        }
    }
}