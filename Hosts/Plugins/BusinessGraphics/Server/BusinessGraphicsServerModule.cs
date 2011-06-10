using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using TechnicalServices.Licensing;

namespace Hosts.Plugins.BusinessGraphics.Server
{
    public sealed class BusinessGraphicsServerModule : ServerModule
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.BusinessGraphics);
        }
    }
}