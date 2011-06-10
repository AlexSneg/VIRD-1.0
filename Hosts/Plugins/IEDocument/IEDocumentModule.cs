using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DomainServices.EnvironmentConfiguration.ConfigModule;
using Hosts.Plugins.IEDocument.Player;
using Hosts.Plugins.IEDocument.Server;
using Hosts.Plugins.IEDocument.SystemModule;
using Hosts.Plugins.IEDocument.Designer;
using Hosts.Plugins.IEDocument.Configurator;
using Hosts.Plugins.IEDocument.Visualizator;


namespace Hosts.Plugins.IEDocument
{
    public sealed class IEDocumentModule : ModuleGeneric<
        IEDocumentSystemModule, IEDocumentDesignerModule,
        IEDocumentConfiguratorModule, IEDocumentVisualizatorModule,
        IEDocumentServerModule, IEDocumentPlayerModule>
    {
    }
}
