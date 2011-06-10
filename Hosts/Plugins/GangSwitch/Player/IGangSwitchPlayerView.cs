using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.GangSwitch.SystemModule.Config;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;

namespace Hosts.Plugins.GangSwitch.Player
{
    internal interface IGangSwitchPlayerView : IPlayerPlaginHardBaseView
    {
        /// <summary>Добавление и инициализация очередного переключателя </summary>
        void AddSwitch(GangSwitchUnitDesign unitState, GangSwitchUnitConfig unitValue);
        /// <summary>обновить состояние переключателей </summary>
        void UpdateView(bool available, bool[] switchState);
    }
}
