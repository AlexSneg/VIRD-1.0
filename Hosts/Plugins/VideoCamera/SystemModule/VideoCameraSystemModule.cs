using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.VideoCamera.SystemModule
{
    public sealed class VideoCameraSystemModule :
        SystemModule<Config.VideoCameraModule, Design.VideoCameraModule>
    {
    }
}