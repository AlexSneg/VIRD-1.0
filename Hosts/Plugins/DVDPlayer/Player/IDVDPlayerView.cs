using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.DVDPlayer.SystemModule.Design;
using Hosts.Plugins.DVDPlayer.SystemModule.Config;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;

namespace Hosts.Plugins.DVDPlayer.Player
{
    internal interface IDVDPlayerView : IPlayerPlaginRGBBaseView
    {
        /// <summary>первоначальное инициализация контрола данными с девайса </summary>
        /// <param name="device">девайс состояние которого отображает контрол</param>
        void InitializeData(DVDPlayerDeviceDesign device);
        /// <summary>метод который обновляет изменяемые данные на контроле, 
        /// если будет обновляться больше данных, метод надо расширять </summary>
        void UpdateView(bool available, DVDPlayerDeviceDesign device, 
            bool IsPlayerOn, string elapsedTime,
            int trackCurrNumber, int chapterCurrNumber, DVDState state);
    }
}
