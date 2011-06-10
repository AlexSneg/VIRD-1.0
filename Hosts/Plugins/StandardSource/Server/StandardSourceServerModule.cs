using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using TechnicalServices.Licensing;

namespace Hosts.Plugins.StandardSource.Server
{
    public sealed class StandardSourceServerModule : ServerModule
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.StandardSource);
        }
    }
}