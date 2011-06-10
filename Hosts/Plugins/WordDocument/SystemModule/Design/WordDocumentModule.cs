using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.WordDocument.SystemModule.Design
{
    public sealed class WordDocumentModule : PresentationModule
    {
        public override Type[] GetSource()
        {
            return new Type[] { typeof(WordDocumentSourceDesign) };
        }
    }
}
