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
    public partial class AudioMixerFullControl : UserControl, IAudioMixerFullView
    {
        public AudioMixerFullControl()
        {
            InitializeComponent();
        }

        public void InitializeMatrix(bool hasMatrix, int matrixId,
            List<AudioMixerInput> inputs, List<AudioMixerOutput> outputs, 
            Func<AudioMixerInput, AudioMixerOutput, bool> getMatrixUnit)
        {
            if (hasMatrix)
            {
                audioMixerMatrixControl1.Visible = true;
                audioMixerMatrixControl1.InitializeMatrix(matrixId, inputs, outputs, getMatrixUnit);
                audioMixerMatrixControl1.OnAudioMixerMatrixStateEvent += sendPushCommandButtonEvent;
            }
            else
            {
                this.Height -= audioMixerMatrixControl1.Height;
                audioMixerMatrixControl1.Visible = false;
            }
        }

        public void InitializeFaderGroups(List<AudioMixerFaderGroupDesign> groups)
        {
            audioMixerAllGroupFadersControl1.InitializeFaderGroups(groups);
            audioMixerAllGroupFadersControl1.OnAudioMixerFaderStateEvent += sendPushCommandButtonEvent;
        }

        public int GetWidthControl(int countInputs, int countFaders)
        {
            return Math.Max(
                audioMixerAllGroupFadersControl1.GetMinWidthControl(countFaders),
                audioMixerMatrixControl1.GetMinWidthControl(countInputs));
        }

        public void UpdateFaderValue(int instanceId, bool mute, int level)
        {
            audioMixerAllGroupFadersControl1.UpdateFaderValue(instanceId, mute, level);
        }

        public void UpdateMixerTies(bool[][] tiesState)
        {
            audioMixerMatrixControl1.UpdateMixerTies(tiesState);
        }

        public List<AudioMixerMatrixUnit> GetMatrixState
        {
            get { return audioMixerMatrixControl1.GetState; }
        }

        public List<AudioMixerFaderDesign> GetFadersState
        {
            get { return audioMixerAllGroupFadersControl1.GetState; }
        }

        public event Action<string, IConvertible[]> PushCommandButtonEvent;
        private void sendPushCommandButtonEvent(string command, params IConvertible[] parameters)
        {
            if (PushCommandButtonEvent != null)
                PushCommandButtonEvent(command, parameters);
        }
        
        public void SaveFaders(List<AudioMixerFaderGroupDesign> groups)
        {
            List<AudioMixerFaderDesign> faders = this.GetFadersState;
            foreach (AudioMixerFaderGroupDesign group in groups)
            {
                for (int i = 0; i < group.FaderList.Count; i++)
                {
                    foreach (AudioMixerFaderDesign fader in faders)
                    {
                        if (group.FaderList[i].InstanceID == fader.InstanceID)
                            group.FaderList[i] = fader;
                    }
                }
            }
        }
    }
}
