using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.BusinessGraphics.SystemModule
{
    public sealed class BusinessGraphicsSystemModule :
        SystemModule<Config.BusinessGraphicsModule, Design.BusinessGraphicsModule>
    {
    }
}