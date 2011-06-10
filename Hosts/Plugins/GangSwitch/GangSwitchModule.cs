using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.GangSwitch.Configurator;
using Hosts.Plugins.GangSwitch.Designer;
using Hosts.Plugins.GangSwitch.Player;
using Hosts.Plugins.GangSwitch.Server;
using Hosts.Plugins.GangSwitch.SystemModule;
using Hosts.Plugins.GangSwitch.Visualizator;

namespace Hosts.Plugins.GangSwitch
{
    public sealed class GangSwitchModule :
        ModuleGeneric
            <GangSwitchSystemModule, GangSwitchDesignerModule, GangSwitchConfiguratorModule,
            GangSwitchVisualizatorModule, GangSwitchServerModule, GangSwitchPlayerModule>
    {
    }
}