using DomainServices.EnvironmentConfiguration.ConfigModule.Designer;
using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using TechnicalServices.Licensing;

namespace Hosts.Plugins.BusinessGraphics.Designer
{
    public sealed class BusinessGraphicsDesignerModule : DesignerModule
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.BusinessGraphics);
        }
    }
}