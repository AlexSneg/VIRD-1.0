using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design
{
    public sealed class BusinessGraphicsModule : PresentationModule
    {
        public override Type[] GetSource()
        {
            return new[] { typeof(BusinessGraphicsSourceDesign) };
        }
    }
}