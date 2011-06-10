using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.IEDocument.SystemModule.Config
{
    public sealed class IEDocumentModule : ConfigurationModule
    {
        public override Type[] GetSource()
        {
            return new Type[] {typeof(IEDocumentSourceConfig)};
        }

        public override Type[] GetExtensionType()
        {
            return new Type[] { typeof(IEResourceInfo) };
        }
    }
}
