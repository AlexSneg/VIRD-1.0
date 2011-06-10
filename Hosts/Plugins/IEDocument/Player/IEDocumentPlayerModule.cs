using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;

namespace Hosts.Plugins.IEDocument.Player
{
    public class IEDocumentPlayerModule : DomainPlayerModule
    {
        public override System.Windows.Forms.Control CreateControlForSource(
            TechnicalServices.Interfaces.IEventLogging logging,
            TechnicalServices.Persistence.SystemPersistence.Presentation.Source source,
            System.Windows.Forms.Control parent,
            TechnicalServices.Interfaces.ConfigModule.Player.IPlayerCommand playerProvider,
            TechnicalServices.Interfaces.IPresentationClient client)
        {
            PlayerController controller = new PlayerController(source, playerProvider);
            PlayerControl control = new PlayerControl(controller);
            control.Parent = parent;
            parent.Controls.Add(control);
            control.CreateControl();
            return control;
        }
    }
}
