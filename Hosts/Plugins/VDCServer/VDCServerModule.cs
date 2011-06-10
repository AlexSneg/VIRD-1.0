using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.VDCServer.Configurator;
using Hosts.Plugins.VDCServer.Designer;
using Hosts.Plugins.VDCServer.Player;
using Hosts.Plugins.VDCServer.Server;
using Hosts.Plugins.VDCServer.SystemModule;
using Hosts.Plugins.VDCServer.Visualizator;

namespace Hosts.Plugins.VDCServer
{
    public sealed class VDCServerModule :
        ModuleGeneric
            <VDCServerSystemModule, VDCServerDesignerModule, VDCServerConfiguratorModule,
            VDCServerVisualizatorModule, VDCServerServerModule, VDCServerPlayerModule>
    {
    }
}