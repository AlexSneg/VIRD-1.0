using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.VDCTerminal.SystemModule.Design;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;

namespace Hosts.Plugins.VDCTerminal.Player
{
    interface IVDCTerminalView : IPlayerPlaginRGBBaseView
    {
        void InitializeData(VDCTerminalDeviceDesign device, 
            List<VDCTerminalAbonentInfo> abonents,
            VDCTerminalAbonentInfo abonent, string directNumber);
        void UpdateView(bool available, ConnectionStateEnum callState, string status, string error, 
            string incomingCall, bool privacy, bool dnd, bool content, bool pip, bool auto);
    }
}
