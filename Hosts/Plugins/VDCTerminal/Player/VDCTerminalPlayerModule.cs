using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Windows.Forms;

namespace Hosts.Plugins.VDCTerminal.Player
{
    public class VDCTerminalPlayerModule : DomainPlayerModule
    {
        public override Control CreateControlForSource(IEventLogging logging, Source source, Control parent, IPlayerCommand playerCommand, IPresentationClient client)
        {
            VDCTerminalControl control = new VDCTerminalControl(source, playerCommand, logging, client);
            control.Parent = parent;
            parent.Controls.Add(control);
            control.CreateControl();
            return control;
        }
    }
}
