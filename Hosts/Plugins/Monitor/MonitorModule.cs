using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.Monitor.Configurator;
using Hosts.Plugins.Monitor.Designer;
using Hosts.Plugins.Monitor.Player;
using Hosts.Plugins.Monitor.Server;
using Hosts.Plugins.Monitor.SystemModule;
using Hosts.Plugins.Monitor.Visualizator;

namespace Hosts.Plugins.Monitor
{
    public sealed class MonitorModule :
        ModuleGeneric
            <MonitorSystemModule, MonitorDesignerModule, MonitorConfiguratorModule, MonitorVisualizatorModule,
            MonitorServerModule, MonitorPlayerModule>
    {
    }
}