using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.Computer.Configurator;
using Hosts.Plugins.Computer.Designer;
using Hosts.Plugins.Computer.Player;
using Hosts.Plugins.Computer.Server;
using Hosts.Plugins.Computer.SystemModule;
using Hosts.Plugins.Computer.Visualizator;

namespace Hosts.Plugins.Computer
{
    public sealed class ComputerModule :
        ModuleGeneric
            <ComputerSystemModule, ComputerDesignerModule, ComputerConfiguratorModule, ComputerVisualizatorModule,
            ComputerServerModule, ComputerPlayerModule>
    {
    }
}