using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using System.Windows.Forms;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.BusinessGraphics.Player
{
    public class BusinessGraphicsPlayerModule : DomainPlayerModule
    {
        public override Control CreateControlForSource(IEventLogging logging, Source source, Control parent, IPlayerCommand playerProvider, IPresentationClient client)
        {
            BusinessGraphicsController controller = new BusinessGraphicsController(source, playerProvider);
            ChartManageControl control = new ChartManageControl(controller);
            control.Parent = parent;
            parent.Controls.Add(control);
            control.CreateControl();
            return control;
        }
    }
}
