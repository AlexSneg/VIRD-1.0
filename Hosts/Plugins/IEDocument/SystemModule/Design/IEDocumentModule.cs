using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.IEDocument.SystemModule.Design
{
    public sealed class IEDocumentModule : PresentationModule
    {
        public override Type[] GetSource()
        {
            return new Type[] { typeof(IEDocumentSourceDesign) };
        }
    }
}
