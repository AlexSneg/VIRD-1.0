using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using System.Windows.Forms;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.Video.Player
{
    public class VideoPlayerModule : DomainPlayerModule
    {
        public override Control CreateControlForSource(IEventLogging logging, Source source, Control parent, IPlayerCommand playerProvider, IPresentationClient client)
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
