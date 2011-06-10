using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using System.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.VideoCamera.Player
{
    public class VideoCameraPlayerModule : DomainPlayerModule
    {
        public override Control CreateControlForSource(IEventLogging logging, Source source, Control parent, IPlayerCommand playerCommand, IPresentationClient client)
        {
            VideoCameraPlayerCommonControl control = new VideoCameraPlayerCommonControl(source, playerCommand, logging, client);
            control.Parent = parent;
            parent.Controls.Add(control);
            control.CreateControl();
            return control;
        }
    }
}
