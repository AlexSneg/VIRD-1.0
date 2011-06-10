using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.VDCTerminal.Configurator;
using Hosts.Plugins.VDCTerminal.Designer;
using Hosts.Plugins.VDCTerminal.Server;
using Hosts.Plugins.VDCTerminal.SystemModule;
using Hosts.Plugins.VDCTerminal.Visualizator;
using Hosts.Plugins.VDCTerminal.Player;

namespace Hosts.Plugins.VDCTerminal
{
    public sealed class VDCTerminalModule :
        ModuleGeneric
            <VDCTerminalSystemModule, VDCTerminalDesignerModule, VDCTerminalConfiguratorModule, VDCTerminalVisualizatorModule,
            VDCTerminalServerModule, VDCTerminalPlayerModule>
    {
    }
}