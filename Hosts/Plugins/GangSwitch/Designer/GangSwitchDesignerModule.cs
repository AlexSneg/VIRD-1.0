﻿using DomainServices.EnvironmentConfiguration.ConfigModule.Designer;
using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using TechnicalServices.Licensing;

namespace Hosts.Plugins.GangSwitch.Designer
{
    public sealed class GangSwitchDesignerModule : DesignerModule
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.GangSwitch);
        }
    }
}