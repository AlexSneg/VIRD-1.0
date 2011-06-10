using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces;
using Hosts.Plugins.Light.SystemModule.Config;

namespace Hosts.Plugins.Light.Player
{
    public partial class LightPlayerControl : DeviceHardPluginBaseControl, ILightPlayerView
    {
        private int _countSwitch;
        private Dictionary<string, int> _lightGroups;

        public LightPlayerControl()
        {
            InitializeComponent();
        }
        public LightPlayerControl(Device device, IPlayerCommand playerProvider, IEventLogging logging)
            : this()
        {
            _countSwitch = 0;
            _lightGroups = new Dictionary<string, int>();
            InitializeController(new LightPlayerController(device, playerProvider, this, logging));
            SetControlPlayerTimerEnable(true, 3000);
        }

        public void AddLightGroup(LightUnitDesign unitState)
        {
            _countSwitch++;
            if (!_lightGroups.ContainsKey(unitState.Name))
                _lightGroups.Add(unitState.Name, _countSwitch);
            _lightGroups[unitState.Name] = _countSwitch;
            //создание контролов
            LightGroupControl cntrl = new LightGroupControl(unitState, _countSwitch);
            cntrl.Location = new Point(1, (_countSwitch - 1) * 22 + 6);
            cntrl.Dock = DockStyle.Top;
            cntrl.OnLightGroupStateChanged += sendPushCommandButtonEvent;
            gpDetail.Controls.Add(cntrl);
            cntrl.BringToFront();
            gpDetail.Refresh();
        }

        public void UpdateView(bool isAvailable, LightUnitDesign unitState)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool, LightUnitDesign>(UpdateView), isAvailable, unitState);
                return;
            }
            SetAvailableStatus(isAvailable);
            if (isAvailable)
            {
                Control[] res = this.Controls.Find("changeControl" + _lightGroups[unitState.Name].ToString(), true);
                if ((res != null) && (res.Length > 0))
                {
                    LightGroupControl cntrl = res[0] as LightGroupControl;
                    if (cntrl != null)
                        cntrl.UpdateValue(unitState.Brightness);
                }
            }
        }
    }
}
