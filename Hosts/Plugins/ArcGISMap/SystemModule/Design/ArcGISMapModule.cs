using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.ArcGISMap.SystemModule.Design
{
    public sealed class ArcGISMapModule : PresentationModule
    {
        public override Type[] GetSource()
        {
            return new[] { typeof(ArcGISMapSourceDesign) };
        }
    }
}