using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.VDCTerminal.SystemModule
{
    public sealed class VDCTerminalSystemModule :
        SystemModule<Config.VDCTerminalModule, Design.VDCTerminalModule>
    {
    }
}