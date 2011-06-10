using DomainServices.EnvironmentConfiguration.ConfigModule.Designer;
using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using TechnicalServices.Licensing;

namespace Hosts.Plugins.ArcGISMap.Designer
{
    public sealed class ArcGISMapDesignerModule : DesignerModule
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.ArcGISMap);
        }
    }
}