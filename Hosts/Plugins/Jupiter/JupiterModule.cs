using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.Jupiter.Configurator;
using Hosts.Plugins.Jupiter.Designer;
using Hosts.Plugins.Jupiter.Server;
using Hosts.Plugins.Jupiter.SystemModule;
using Hosts.Plugins.Jupiter.Visualizator;
using Hosts.Plugins.Jupiter.Player;

namespace Hosts.Plugins.Jupiter
{
    public sealed class JupiterModule :
        ModuleGeneric
            <JupiterSystemModule, JupiterDesignerModule, JupiterConfiguratorModule, JupiterVisualizatorModule,
            JupiterServerModule, JupiterPlayerModule>
    {
    }
}