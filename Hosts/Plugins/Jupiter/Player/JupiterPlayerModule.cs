using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using System.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.Jupiter.Player
{
    public class JupiterPlayerModule : DomainPlayerModule
    {
        public override Control CreateControlForDevice(IEventLogging logging, Device device, Control parent, IPlayerCommand playerProvider, IPresentationClient client)
        {
            JupiterPlayerControl control = new JupiterPlayerControl(device, playerProvider, logging);
            control.Parent = parent;
            parent.Controls.Add(control);
            control.CreateControl();
            return control;
        }
    }
}
