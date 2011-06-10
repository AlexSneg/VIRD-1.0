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
    public partial class AudioMixerAllGroupFadersControl : UserControl
    {
        private readonly int _minFaderWidth = 32;
        private int _countSwitch;
        //цвета для груп фейдеров
        private Color[] _groupsColor = new Color[] { Color.FromArgb(224, 224, 244), Color.FromArgb(187, 212, 246) };
        private Dictionary<int, AudioMixerFaderControl> _faders;

        public AudioMixerAllGroupFadersControl()
        {
            _countSwitch = 0;
            InitializeComponent();
        }
        public void InitializeFaderGroups(List<AudioMixerFaderGroupDesign> groups)
        {
            _faders = new Dictionary<int, AudioMixerFaderControl>();
            foreach (AudioMixerFaderGroupDesign item in groups)
            {
                if (item.FaderList.Count > 0) _countSwitch++;
                AudioMixerGroupFaderControl gfControl = new AudioMixerGroupFaderControl();
                addDictionary(gfControl.InitializeFaderGroup(item, _groupsColor[_countSwitch % 2]));
                gfControl.Location = new System.Drawing.Point((_countSwitch - 1) * _minFaderWidth + 1, 0);
                gfControl.Dock = DockStyle.Left;
                gpDetail.Controls.Add(gfControl);
                gfControl.BringToFront();
            }
        }
        public int GetMinWidthControl(int countFaders)
        {
            return _minFaderWidth * countFaders + 15;//15- магическое число, возникает из-за того этот дочерний контрол всегда чуть меньше чем родительский
        }
        public List<AudioMixerFaderDesign> GetState
        {
            get
            {
                List<AudioMixerFaderDesign> result = new List<AudioMixerFaderDesign>();
                foreach (KeyValuePair<int, AudioMixerFaderControl> item in _faders)
                {
                    result.Add(item.Value.GetState);
                }
                return result;
            }
        }

        public void UpdateFaderValue(int instanceId, bool mute, int level)
        {
            if (_faders.ContainsKey(instanceId))
            {
                _faders[instanceId].UpdateFaderValue(mute, level);
            }
        }
        public event Action<string, IConvertible[]> OnAudioMixerFaderStateEvent;

        private void addDictionary(Dictionary<int, AudioMixerFaderControl> source)
        {
            foreach (KeyValuePair<int, AudioMixerFaderControl> item in source)
            {
                _faders.Add(item.Key, item.Value);
                item.Value.OnAudioMixerFaderStateEvent += _onAudioMixerFaderStateEvent;
            }
        }

        private void _onAudioMixerFaderStateEvent(string command, params IConvertible[] parameters)
        {
            if (OnAudioMixerFaderStateEvent != null)
                OnAudioMixerFaderStateEvent(command, parameters);
        }
    }
}
