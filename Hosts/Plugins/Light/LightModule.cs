using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.Light.Configurator;
using Hosts.Plugins.Light.Designer;
using Hosts.Plugins.Light.Player;
using Hosts.Plugins.Light.Server;
using Hosts.Plugins.Light.SystemModule;
using Hosts.Plugins.Light.Visualizator;

namespace Hosts.Plugins.Light
{
    public sealed class LightModule :
        ModuleGeneric
            <LightSystemModule, LightDesignerModule, LightConfiguratorModule,
            LightVisualizatorModule, LightServerModule, LightPlayerModule>
    {
    }
}