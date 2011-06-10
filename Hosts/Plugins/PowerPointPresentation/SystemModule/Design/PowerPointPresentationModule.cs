using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.PowerPointPresentation.SystemModule.Design
{
    public sealed class PowerPointPresentationModule : PresentationModule
    {
        public override Type[] GetSource()
        {
            return new Type[] { typeof(PowerPointPresentationSourceDesign) };
        }
    }
}
