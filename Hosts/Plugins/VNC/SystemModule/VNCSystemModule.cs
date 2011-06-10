using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.VNC.SystemModule
{
    public sealed class VNCSystemModule :
        SystemModule<Config.VNCModule, Design.VNCModule>
    {
    }
}