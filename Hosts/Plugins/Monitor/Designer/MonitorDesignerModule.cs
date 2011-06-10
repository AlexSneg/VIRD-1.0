using DomainServices.EnvironmentConfiguration.ConfigModule.Designer;
using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using TechnicalServices.Licensing;

namespace Hosts.Plugins.Monitor.Designer
{
    public sealed class MonitorDesignerModule : DesignerModule
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.Monitor);
        }
    }
}