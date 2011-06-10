using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.Jupiter.SystemModule.Design;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;

namespace Hosts.Plugins.Jupiter.Player
{
    internal interface IJupiterPlayerView : IPlayerPlaginHardBaseView
    {
        void UpdateView(bool available, bool power, bool picMute, int brightness);
    }
}
