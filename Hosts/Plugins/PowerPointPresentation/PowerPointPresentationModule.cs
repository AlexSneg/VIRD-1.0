using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DomainServices.EnvironmentConfiguration.ConfigModule;
using Hosts.Plugins.PowerPointPresentation.Player;
using Hosts.Plugins.PowerPointPresentation.Server;
using Hosts.Plugins.PowerPointPresentation.SystemModule;
using Hosts.Plugins.PowerPointPresentation.Designer;
using Hosts.Plugins.PowerPointPresentation.Configurator;
using Hosts.Plugins.PowerPointPresentation.Visualizator;


namespace Hosts.Plugins.PowerPointPresentation
{
    public sealed class PowerPointPresentationModule : ModuleGeneric<
        PowerPointPresentationSystemModule, PowerPointPresentationDesignerModule,
        PowerPointPresentationConfiguratorModule, PowerPointPresentationVisualizatorModule,
        PowerPointPresentationServerModule, PowerPointPresentationPlayerModule>
    {
    }
}
