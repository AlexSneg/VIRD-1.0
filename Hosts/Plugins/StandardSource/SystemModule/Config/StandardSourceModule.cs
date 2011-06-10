using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;
using Hosts.Plugins.StandardSource.SystemModule.Design;

namespace Hosts.Plugins.StandardSource.SystemModule.Config
{
    public sealed class StandardSourceModule : ConfigurationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(StandardSourceDeviceConfig) };
        }

        public override Type[] GetSource()
        {
            return new[] { typeof(StandardSourceSourceConfig) };
        }

        public override Type[] GetExtensionType()
        {
            return new[] { typeof(StandardSourceResourceInfo) };
        }
    }
}