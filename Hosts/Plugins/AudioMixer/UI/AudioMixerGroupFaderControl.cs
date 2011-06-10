using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hosts.Plugins.AudioMixer.SystemModule.Config;

namespace Hosts.Plugins.AudioMixer.UI
{
    public partial class AudioMixerGroupFaderControl : UserControl
    {
        private int _countSwitch;
        private int _width;

        public AudioMixerGroupFaderControl()
        {
            _countSwitch = 0;
            InitializeComponent();
        }
        public Dictionary<int, AudioMixerFaderControl> InitializeFaderGroup(AudioMixerFaderGroupDesign group, Color back)
        {
            Dictionary<int, AudioMixerFaderControl> faders = new Dictionary<int, AudioMixerFaderControl>();
            _width = 0;
            alName.Text = group.Name;
            foreach (AudioMixerFaderDesign item in group.FaderList)
            {
                _countSwitch++;
                AudioMixerFaderControl cntrl = new AudioMixerFaderControl(item);
                cntrl.Location = new System.Drawing.Point((_countSwitch - 1) * cntrl.Width + 1, 0);
                cntrl.Dock = DockStyle.Left;
                this.Controls.Add(cntrl);
                cntrl.BringToFront();
                cntrl.BackColor = back;
                faders.Add(Convert.ToInt32(item.InstanceID), cntrl);
                _width += cntrl.Width;
            }
            this.Width = _width;
            this.BackColor = back;
            return faders;
        }

    }
}
