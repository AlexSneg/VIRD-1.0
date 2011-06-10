using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using Hosts.Plugins.GangSwitch.SystemModule.Config;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.GangSwitch.Player
{
    public partial class GangSwitchPlayerControl : DeviceHardPluginBaseControl, IGangSwitchPlayerView
    {
        private int _countSwitch;

        public GangSwitchPlayerControl()
        {
            InitializeComponent();
        }
        public GangSwitchPlayerControl(Device device, IPlayerCommand playerProvider, IEventLogging logging)
            : this()
        {
            _countSwitch = 0;
            InitializeController(new GangSwitchPlayerController(device, playerProvider, this, logging));
            SetControlPlayerTimerEnable(true, 3000);
        }
        
        public void AddSwitch(GangSwitchUnitDesign unitState, GangSwitchUnitConfig unitValue)
        {
            _countSwitch++;
            SwitchPlayerControl cntrl = new SwitchPlayerControl(unitState, unitValue, _countSwitch);
            cntrl.Location = new Point(1, (_countSwitch - 1) * 22 + 1);
            cntrl.Dock = DockStyle.Top;
            cntrl.Name = "switchNo" + _countSwitch.ToString();
            cntrl.PushCommandButtonEvent += sendPushCommandButtonEvent;
            gpDetail.Controls.Add(cntrl);
            cntrl.BringToFront();
            gpDetail.Refresh();
        }

        public void UpdateView(bool available, bool[] switchState)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool, bool[]>(UpdateView), available, switchState);
                return;
            }

            SetAvailableStatus(available);
            if (available)
            {
                for (int i = 0; i < switchState.Length; i++)
                {
                    Control[] cntrls = gpDetail.Controls.Find("switchNo" + (i + 1).ToString(), true);
                    if ((cntrls != null) && (cntrls.Length > 0) && (cntrls[0] is SwitchPlayerControl))
                    {
                        ((SwitchPlayerControl)(cntrls[0])).UpdateState(switchState[i]);
                    }
                }
            }
        }

    }
}
