using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.Image.Configurator;
using Hosts.Plugins.Image.Designer;
using Hosts.Plugins.Image.Server;
using Hosts.Plugins.Image.SystemModule;
using Hosts.Plugins.Image.Visualizator;
using Hosts.Plugins.Image.Player;

namespace Hosts.Plugins.Image
{
    public sealed class ImageModule :
        ModuleGeneric
            <ImageSystemModule, ImageDesignerModule, ImageConfiguratorModule, ImageVisualizatorModule, ImageServerModule, ImagePlayerModule
            >
    {
    }
}