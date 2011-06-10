using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using Hosts.Plugins.Computer.SystemModule.Config;

using TechnicalServices.ActiveDisplay.Util;
using TechnicalServices.Licensing;

namespace Hosts.Plugins.Computer.Server
{
    public sealed class ComputerServerModule : ComputerServerModule<ComputerModule, ComputerDisplayConfig>
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.Computer);
        }
    }
}