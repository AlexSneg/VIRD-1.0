using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.ArcGISMap.SystemModule
{
    public sealed class ArcGISMapSystemModule :
        SystemModule<Config.ArcGISMapModule, Design.ArcGISMapModule>
    {
    }
}