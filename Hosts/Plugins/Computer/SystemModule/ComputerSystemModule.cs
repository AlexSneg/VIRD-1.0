using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Computer.SystemModule
{
    public sealed class ComputerSystemModule :
        SystemModule<Config.ComputerModule, Design.ComputerModule>
    {
    }
}