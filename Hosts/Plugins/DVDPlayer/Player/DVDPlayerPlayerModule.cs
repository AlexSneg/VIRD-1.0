using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.DVDPlayer.Player
{
    public class DVDPlayerPlayerModule : DomainPlayerModule
    {
        public override System.Windows.Forms.Control CreateControlForSource(IEventLogging logging, Source source, Control parent, IPlayerCommand playerCommand, IPresentationClient client)
        {
            DVDPlayerControl2 control = new DVDPlayerControl2(source, playerCommand, logging, client);
            control.Parent = parent;
            parent.Controls.Add(control);
            control.CreateControl();
            return control;
        }
    }
}
