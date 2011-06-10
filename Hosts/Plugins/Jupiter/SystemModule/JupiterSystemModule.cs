using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.Jupiter.SystemModule
{
    public sealed class JupiterSystemModule :
        SystemModule<Config.JupiterModule, Design.JupiterModule>
    {
    }
}