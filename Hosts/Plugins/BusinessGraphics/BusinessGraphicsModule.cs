using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.BusinessGraphics.Configurator;
using Hosts.Plugins.BusinessGraphics.Designer;
using Hosts.Plugins.BusinessGraphics.Server;
using Hosts.Plugins.BusinessGraphics.SystemModule;
using Hosts.Plugins.BusinessGraphics.Visualizator;
using Hosts.Plugins.BusinessGraphics.Player;

namespace Hosts.Plugins.BusinessGraphics
{
    public sealed class BusinessGraphicsModule :
        ModuleGeneric
            <BusinessGraphicsSystemModule, BusinessGraphicsDesignerModule, BusinessGraphicsConfiguratorModule, BusinessGraphicsVisualizatorModule, BusinessGraphicsServerModule, BusinessGraphicsPlayerModule
            >
    {
    }
}