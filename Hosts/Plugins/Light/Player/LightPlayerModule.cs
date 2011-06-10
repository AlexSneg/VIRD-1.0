using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using System.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.Light.Player
{
    public class LightPlayerModule : DomainPlayerModule
    {
        public override Control CreateControlForDevice(IEventLogging logging, Device device, Control parent, IPlayerCommand playerProvider, IPresentationClient client)
        {
            LightPlayerControl control = new LightPlayerControl(device, playerProvider, logging);
            control.Parent = parent;
            parent.Controls.Add(control);
            control.CreateControl();
            return control;
        }
    }
}
