using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.StandardSource.Configurator;
using Hosts.Plugins.StandardSource.Designer;
using Hosts.Plugins.StandardSource.Player;
using Hosts.Plugins.StandardSource.Server;
using Hosts.Plugins.StandardSource.SystemModule;
using Hosts.Plugins.StandardSource.Visualizator;

namespace Hosts.Plugins.StandardSource
{
    public sealed class StandardSourceModule :
        ModuleGeneric
            <StandardSourceSystemModule, StandardSourceDesignerModule, StandardSourceConfiguratorModule,
            StandardSourceVisualizatorModule, StandardSourceServerModule, StandardSourcePlayerModule>
    {
    }
}