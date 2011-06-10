using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.DVDPlayer.SystemModule.Design
{
    public sealed class DVDPlayerModule : PresentationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(DVDPlayerDeviceDesign) };
        }

        public override Type[] GetSource()
        {
            return new[] { typeof(DVDPlayerSourceDesign) };
        }
    }
}