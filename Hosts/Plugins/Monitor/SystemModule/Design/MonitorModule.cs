using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Monitor.SystemModule.Design
{
    public sealed class MonitorModule : PresentationModule
    {
        public override Type[] GetDisplay()
        {
            return new[] { typeof(MonitorDisplayDesign) };
        }
    }
}