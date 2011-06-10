using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Video.SystemModule
{
    public sealed class VideoSystemModule :
        SystemModule<Config.VideoModule, Design.VideoModule>
    {
    }
}