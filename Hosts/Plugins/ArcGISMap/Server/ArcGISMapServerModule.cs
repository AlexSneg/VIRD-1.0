using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using TechnicalServices.Licensing;

namespace Hosts.Plugins.ArcGISMap.Server
{
    public sealed class ArcGISMapServerModule : ServerModule
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.ArcGISMap);
        }
    }
}