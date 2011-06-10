using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.GangSwitch.SystemModule.Design
{
    public sealed class GangSwitchModule : PresentationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(GangSwitchDeviceDesign) };
        }
    }
}