using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.Light.SystemModule.Config;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;

namespace Hosts.Plugins.Light.Player
{
    internal interface ILightPlayerView : IPlayerPlaginHardBaseView
    {
        /// <summary>Добавление и инициализация очередной группы освещения </summary>
        void AddLightGroup(LightUnitDesign unitState);
        /// <summary>обновление состаяния контрола </summary>
        void UpdateView(bool isAvailable, LightUnitDesign unitState);
    }
}
