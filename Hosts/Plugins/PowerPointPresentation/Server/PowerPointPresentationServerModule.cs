using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainServices.EnvironmentConfiguration.ConfigModule.Server;
using TechnicalServices.Licensing;

namespace Hosts.Plugins.PowerPointPresentation.Server
{
    public sealed class PowerPointPresentationServerModule : ServerModule
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int)Feature.PowerPointPresentation);
        }
    }
}
