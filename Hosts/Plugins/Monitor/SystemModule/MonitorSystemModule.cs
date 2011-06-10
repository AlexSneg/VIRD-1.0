using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Monitor.SystemModule
{
    public sealed class MonitorSystemModule :
        SystemModule<Config.MonitorModule, Design.MonitorModule>
    {
    }
}