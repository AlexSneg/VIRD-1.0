using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.Video.Configurator;
using Hosts.Plugins.Video.Designer;
using Hosts.Plugins.Video.Server;
using Hosts.Plugins.Video.SystemModule;
using Hosts.Plugins.Video.Visualizator;
using Hosts.Plugins.Video.Player;

namespace Hosts.Plugins.Video
{
    public sealed class VideoModule :
        ModuleGeneric
            <VideoSystemModule, VideoDesignerModule, VideoConfiguratorModule, VideoVisualizatorModule, VideoServerModule, VideoPlayerModule
            >
    {
    }
}