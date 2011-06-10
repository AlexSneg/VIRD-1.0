using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.DVDPlayer.Configurator;
using Hosts.Plugins.DVDPlayer.Designer;
using Hosts.Plugins.DVDPlayer.Server;
using Hosts.Plugins.DVDPlayer.SystemModule;
using Hosts.Plugins.DVDPlayer.Visualizator;
using Hosts.Plugins.DVDPlayer.Player;

namespace Hosts.Plugins.DVDPlayer
{
    public sealed class DVDPlayerModule :
        ModuleGeneric
            <DVDPlayerSystemModule, DVDPlayerDesignerModule, DVDPlayerConfiguratorModule, DVDVisualizatorModule,
            DVDPlayerServerModule, DVDPlayerPlayerModule>
    {
    }
}