using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.VideoCamera.Configurator;
using Hosts.Plugins.VideoCamera.Designer;
using Hosts.Plugins.VideoCamera.Player;
using Hosts.Plugins.VideoCamera.Server;
using Hosts.Plugins.VideoCamera.SystemModule;
using Hosts.Plugins.VideoCamera.Visualizator;

namespace Hosts.Plugins.VideoCamera
{
    public sealed class VideoCameraModule :
        ModuleGeneric
            <VideoCameraSystemModule, VideoCameraDesignerModule, VideoCameraConfiguratorModule,
            VideoCameraVisualizatorModule, VideoCameraServerModule, VideoCameraPlayerModule>
    {
    }
}