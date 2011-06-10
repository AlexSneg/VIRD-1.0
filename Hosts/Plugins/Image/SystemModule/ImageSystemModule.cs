using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Image.SystemModule
{
    public sealed class ImageSystemModule :
        SystemModule<Config.ImageModule, Design.ImageModule>
    {
    }
}