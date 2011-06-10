using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Windows.Forms;
using TechnicalServices.Interfaces.ConfigModule.Player;

namespace Hosts.Plugins.VDCServer.Player
{
    public class VDCServerPlayerModule : DomainPlayerModule
    {
        public override Control CreateControlForDevice(IEventLogging logging, Device device, Control parent, IPlayerCommand playerProvider, IPresentationClient client)
        {
            VDCServerPlayerControl control = new VDCServerPlayerControl(logging, device, playerProvider);
            control.Parent = parent;
            parent.Controls.Add(control);
            control.CreateControl();
            return control;
        }
    }
}
