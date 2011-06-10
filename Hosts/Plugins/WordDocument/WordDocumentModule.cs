using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DomainServices.EnvironmentConfiguration.ConfigModule;
using Hosts.Plugins.WordDocument.Player;
using Hosts.Plugins.WordDocument.Server;
using Hosts.Plugins.WordDocument.SystemModule;
using Hosts.Plugins.WordDocument.Designer;
using Hosts.Plugins.WordDocument.Configurator;
using Hosts.Plugins.WordDocument.Visualizator;


namespace Hosts.Plugins.WordDocument
{
    public sealed class WordDocumentModule : ModuleGeneric<
        WordDocumentSystemModule, WordDocumentDesignerModule,
        WordDocumentConfiguratorModule, WordDocumentVisualizatorModule,
        WordDocumentServerModule, WordDocumentPlayerModule>
    {
    }
}
