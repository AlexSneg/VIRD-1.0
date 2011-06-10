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
using Hosts.Plugins.AudioMixer.UI;
using Hosts.Plugins.AudioMixer.SystemModule.Config;

namespace Hosts.Plugins.AudioMixer.Player
{
    public partial class AudioMixerPlayerOperativeControl : DeviceHardPluginBaseControl, IAudioMixerPlayerOperativeView
    {
        private int _countSwitch;

        public AudioMixerPlayerOperativeControl()
        {
            InitializeComponent();
        }
        public AudioMixerPlayerOperativeControl(Device device, IPlayerCommand playerProvider, IEventLogging logging)
            : this()
        {
            _countSwitch = 0;
            InitializeController(new AudioMixerPlayerController(device, playerProvider, this, logging));
            SetControlPlayerTimerEnable(true, 3000);
        }

        public void AddMixerFader(AudioMixerFaderDesign unit)
        {
            _countSwitch++;
            AudioMixerFaderControl cntrl = new AudioMixerFaderControl(unit);
            cntrl.Location = new System.Drawing.Point((_countSwitch - 1) * cntrl.Width + 1, 0);
            cntrl.Dock = DockStyle.Left;
            cntrl.OnAudioMixerFaderStateEvent += sendPushCommandButtonEvent;
            gpFillPanel.Controls.Add(cntrl);
            cntrl.BringToFront();
            gpFillPanel.Refresh();
        }

        public event Action DetailExecuteEvent;

        public void UpdateFaderValue(bool isAvailable, int instanceId, bool mute, int level)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool, int, bool, int>(UpdateFaderValue), isAvailable, instanceId, mute, level);
                return;
            }
            SetAvailableStatus(isAvailable);
            if (isAvailable)
            {
                Control[] cntrls = gpFillPanel.Controls.Find("AudioMixerFader" + instanceId.ToString(), false);
                if ((cntrls != null) && (cntrls.Length > 0))
                {
                    (cntrls[0] as AudioMixerFaderControl).UpdateFaderValue(mute, level);
                }
            }
        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DetailExecuteEvent != null)
                DetailExecuteEvent();
        }
    }
}
