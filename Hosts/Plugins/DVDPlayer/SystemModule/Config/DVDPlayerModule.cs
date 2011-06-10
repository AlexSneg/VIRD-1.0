using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;
using Hosts.Plugins.DVDPlayer.SystemModule.Design;

namespace Hosts.Plugins.DVDPlayer.SystemModule.Config
{
    public sealed class DVDPlayerModule : ConfigurationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(DVDPlayerDeviceConfig) };
        }

        public override Type[] GetSource()
        {
            return new[] { typeof(DVDPlayerSourceConfig) };
        }

        public override Type[] GetExtensionType()
        {
            return new[] { typeof(DVDPlayerResourceInfo) };
        }
    }
}