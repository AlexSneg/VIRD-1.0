using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using System.Windows.Forms;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.GangSwitch.Player
{
    public class GangSwitchPlayerModule : DomainPlayerModule
    {
        public override Control CreateControlForDevice(IEventLogging logging, Device device, Control parent, IPlayerCommand playerProvider, IPresentationClient client)
        {
            GangSwitchPlayerControl control = new GangSwitchPlayerControl(device, playerProvider, logging);
            control.Parent = parent;
            parent.Controls.Add(control);
            control.CreateControl();
            return control;
        }
    }
}
