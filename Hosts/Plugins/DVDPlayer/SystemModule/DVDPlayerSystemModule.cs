using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.DVDPlayer.SystemModule
{
    public sealed class DVDPlayerSystemModule :
        SystemModule<Config.DVDPlayerModule, Design.DVDPlayerModule>
    {
    }
}