using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.PowerPointPresentation.SystemModule.Config
{
    public sealed class PowerPointPresentationModule : ConfigurationModule
    {
        public override Type[] GetSource()
        {
            return new Type[] {typeof(PowerPointPresentationSourceConfig)};
        }

        public override Type[] GetExtensionType()
        {
            return new Type[] { typeof(PowerPointResourceInfo) };
        }
    }
}
