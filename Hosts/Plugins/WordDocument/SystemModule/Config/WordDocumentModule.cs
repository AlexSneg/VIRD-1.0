using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.WordDocument.SystemModule.Config
{
    public sealed class WordDocumentModule : ConfigurationModule
    {
        public override Type[] GetSource()
        {
            return new Type[] {typeof(WordDocumentSourceConfig)};
        }

        public override Type[] GetExtensionType()
        {
            return new Type[] { typeof(WordResourceInfo) };
        }
    }
}
