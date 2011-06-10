using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.VDCServer.SystemModule.Design;
using Hosts.Plugins.VDCServer.SystemModule.Config;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;

namespace Hosts.Plugins.VDCServer.Player
{
    internal interface IVDCServerPlayerView : IPlayerPlaginHardBaseView
    {
        /// <summary>инициализация данных контрола </summary>
        void InitializeData(VDCServerDeviceDesign device, VDCServerDeviceConfig config);
        /// <summary>обновить состояние </summary>
        void UpdateView(bool available, bool status, List<VDCServerAbonentInfo> members);
    }
}
