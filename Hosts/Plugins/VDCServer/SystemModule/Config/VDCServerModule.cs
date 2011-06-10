using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;
using Hosts.Plugins.VDCServer.SystemModule.Design;

namespace Hosts.Plugins.VDCServer.SystemModule.Config
{
    public sealed class VDCServerModule : ConfigurationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(VDCServerDeviceConfig) };
        }

        public override Type[] GetExtensionType()
        {
            return new Type[] { typeof(VDCServerResourceInfo) };
        }
    }
}