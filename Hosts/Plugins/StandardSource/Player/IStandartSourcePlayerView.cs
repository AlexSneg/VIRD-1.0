using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;

namespace Hosts.Plugins.StandardSource.Player
{
    internal interface IStandartSourcePlayerView : IPlayerPlaginRGBBaseView
    {
        /// <summary> обновить состояние оборудования</summary>
        void UpdateView(bool? state);
    }
}
