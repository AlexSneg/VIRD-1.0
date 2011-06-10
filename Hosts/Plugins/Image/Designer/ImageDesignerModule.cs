using System.Diagnostics;

using DomainServices.EnvironmentConfiguration.ConfigModule.Designer;
using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using TechnicalServices.Licensing;

namespace Hosts.Plugins.Image.Designer
{
    public sealed class ImageDesignerModule : DesignerModule
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.Image);
        }

        public override void Preview(string file)
        {
            Process.Start(file);
        }
    }
}