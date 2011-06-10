using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.VNC.Configurator;
using Hosts.Plugins.VNC.Designer;
using Hosts.Plugins.VNC.Server;
using Hosts.Plugins.VNC.SystemModule;
using Hosts.Plugins.VNC.Visualizator;
using Hosts.Plugins.VNC.Player;

namespace Hosts.Plugins.VNC
{
    public sealed class VNCModule :
        ModuleGeneric<VNCSystemModule, VNCDesignerModule, VNCConfiguratorModule, VNCVisualizatorModule, VNCServerModule, VNCPlayerModule>
    {
    }
}