using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using Hosts.Plugins.Jupiter.SystemModule.Design;
using Syncfusion.Windows.Forms;
using TechnicalServices.Interfaces;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;

namespace Hosts.Plugins.Jupiter.Player
{
    public partial class JupiterPlayerControl : DeviceHardPluginBaseControl, IJupiterPlayerView
    {
        private bool _updating = false;
        private string[] _buttonLabel = new string[] { "Включить", "Выключить"};
        private string[] _powerStateLabel = new string[] { "выключена", "включена" };
        private string[] _pictureMuteLabel = new string[] { "выключен", "включен" };

        public JupiterPlayerControl()
        {
            InitializeComponent();
        }
        public JupiterPlayerControl(Device device, IPlayerCommand playerProvider, IEventLogging logging)
            : this()
        {
            InitializeController(new JupiterPlayerController(device, playerProvider, this, logging));
            SetControlPlayerTimerEnable(true, 3000);
        }

        public void UpdateView(bool available, bool power, bool picMute, int brightness)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool, bool, bool, int>(UpdateView), available, power, picMute, brightness);
                return;
            }
            _updating = true;
            SetAvailableStatus(available);
            if (available)
            {
                alPowerState.Text = _powerStateLabel[Convert.ToInt32(power)];
                baPowerButton.Text = _buttonLabel[Convert.ToInt32(power)];
                baPowerButton.State = power ? ButtonAdvState.Pressed : ButtonAdvState.Default;

                alPictureMuteState.Text = _pictureMuteLabel[Convert.ToInt32(picMute)];
                baPictureMuteButton.Text = _buttonLabel[Convert.ToInt32(picMute)];
                baPictureMuteButton.State = picMute ? ButtonAdvState.Pressed : ButtonAdvState.Default;

                cbaBrightness.SelectedItem = brightness.ToString();
                cbaBrightness.ResumeLayout();
                this.Refresh();
            }
            _updating = false;
        }

        private void baPowerButton_Click(object sender, EventArgs e)
        {
            ButtonAdv btn = sender as ButtonAdv;
            if ((btn != null) && (!string.IsNullOrEmpty((string)btn.Tag)))
            {
                //если нажата значит включено
                int param = (btn.State == ButtonAdvState.Pressed ? 1 : 0);
                btn.Text = _buttonLabel[param];
                if (btn.Equals(baPowerButton))
                    alPowerState.Text = _powerStateLabel[param];
                else
                    alPictureMuteState.Text = _pictureMuteLabel[param];
                sendPushCommandButtonEvent((string)btn.Tag, param);
            }
        }

        private void cbaBrightness_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_updating)
                sendPushCommandButtonEvent((string)cbaBrightness.Tag, Convert.ToInt32((string)cbaBrightness.SelectedItem));
        }
    }
}
