using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using Hosts.Plugins.Jupiter.SystemModule.Design;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.Jupiter.Player
{
    internal class JupiterPlayerController : PlayerPlaginsHardController<JupiterDeviceDesign, IJupiterPlayerView>
    {
        public JupiterPlayerController(Device device, IPlayerCommand playerProvider, IJupiterPlayerView view, IEventLogging logging)
            : base((JupiterDeviceDesign)device, playerProvider, logging, view)
        {
            View.UpdateView(true, Device.OnOffState, Device.PictureMute, Device.Brightness);
            UpdateView();
        }

        protected override void UpdateView()
        {
            bool isSuccess; int errorCount = 0;
            bool power = ExecuteCommandBool("GetPower", out isSuccess);
            errorCount += Convert.ToInt32(!isSuccess);
            bool picMute = ExecuteCommandBool("GetPicMute", out isSuccess);
            errorCount += Convert.ToInt32(!isSuccess);
            int brightness = ExecuteCommandInt32("GetBrightness", out isSuccess);
            errorCount += Convert.ToInt32(!isSuccess);
            isSuccess = !Convert.ToBoolean(errorCount);
            View.UpdateView(isSuccess, power, picMute, brightness);
        }

    }
}
