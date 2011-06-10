using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.ArcGISMap.SystemModule;
using Hosts.Plugins.ArcGISMap.Designer;
using Hosts.Plugins.ArcGISMap.Configurator;
using Hosts.Plugins.ArcGISMap.Visualizator;
using Hosts.Plugins.ArcGISMap.Server;
using Hosts.Plugins.ArcGISMap.Player;
using DomainServices.EnvironmentConfiguration.ConfigModule;

namespace Hosts.Plugins.ArcGISMap
{
    public class ArcGISMapModule :
        ModuleGeneric
            <ArcGISMapSystemModule, 
        ArcGISMapDesignerModule, 
        ArcGISMapConfiguratorModule, 
        ArcGISMapVisualizatorModule, 
        ArcGISMapServerModule, 
        ArcGISMapPlayerModule
            >
    {
    }
}
