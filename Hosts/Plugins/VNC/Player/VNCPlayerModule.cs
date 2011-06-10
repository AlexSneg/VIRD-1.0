using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.VNC.Player
{
    public class VNCPlayerModule : DomainPlayerModule
    {
        public override System.Windows.Forms.Control CreateControlForSource(IEventLogging logging, TechnicalServices.Persistence.SystemPersistence.Presentation.Source source, System.Windows.Forms.Control parent, IPlayerCommand playerCommand, IPresentationClient client)
        {
            PlayerController controller = new PlayerController(source, playerCommand);
            VNCPlayerControl control = new VNCPlayerControl(controller);
            control.Parent = parent;
            parent.Controls.Add(control);
            control.CreateControl();
            return control;
        }
    }
}
