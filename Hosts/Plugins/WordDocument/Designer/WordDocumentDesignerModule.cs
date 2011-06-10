using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DomainServices.EnvironmentConfiguration.ConfigModule.Designer;
using DomainServices.EnvironmentConfiguration.ConfigModule.Server;
using TechnicalServices.Licensing;

namespace Hosts.Plugins.WordDocument.Designer
{
    public sealed class WordDocumentDesignerModule : DesignerModule
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int)Feature.WordDocument);
        }

        public override void Preview(string file)
        {
            Process.Start(file);
        }
    }
}
