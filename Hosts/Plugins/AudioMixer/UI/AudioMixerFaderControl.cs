using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hosts.Plugins.AudioMixer.SystemModule.Config;
using Syncfusion.Windows.Forms.Tools;
using System.Threading;

namespace Hosts.Plugins.AudioMixer.UI
{
    public partial class AudioMixerFaderControl : UserControl
    {
        private int _lockAutoUpdate = 1;
        private AudioMixerFaderDesign _unit;
        private int _instanceID { get { return Convert.ToInt32(_unit.InstanceID); } }

        public AudioMixerFaderControl()
        {
            InitializeComponent();
        }
        public AudioMixerFaderControl(AudioMixerFaderDesign unit)
            : this()
        {
            //при создании контрола всегда плокировать авто обновление
            alName.Text = unit.Name;
            alMaxValue.Text = unit.UpperBand.ToString() + unit.UnitString;
            alMinValue.Text = unit.LowerBand.ToString() + unit.UnitString;
            tbeTrack.Minimum = unit.LowerBand;
            tbeTrack.Maximum = unit.UpperBand;
            tbeTrack.Value = unit.BandValue;
            itbCurrentValue.IntegerValue = unit.BandValue;
            itbCurrentValue.MinValue = unit.LowerBand;
            itbCurrentValue.MaxValue = unit.UpperBand;
            cbMute.Checked = unit.Mute;
            this.Name = "AudioMixerFader" + Convert.ToInt32(unit.InstanceID);
            _unit = unit;
            Interlocked.Exchange(ref _lockAutoUpdate, 0);
        }

        public AudioMixerFaderDesign GetState
        {
            get 
            {
                AudioMixerFaderDesign result = new AudioMixerFaderDesign(_unit);
                result.Mute = cbMute.Checked;
                result.BandValue = (int)itbCurrentValue.IntegerValue;
                return result;
            }
        }

        public void UpdateFaderValue(bool mute, int level)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool, int>(UpdateFaderValue), mute, level);
                return;
            }
            //авто обновление можно делать только, если не редактируется текущее значение
            if (_lockAutoUpdate == 0)
            {
                try
                {
                    cbMute.Checked = mute;
                    tbeTrack.Value = _unit.GetBandValueDeNormalize(level);
                    itbCurrentValue.IntegerValue = _unit.GetBandValueDeNormalize(level);
                }
                finally
                {
                    //освободим лок, он мог установится, когда мы инициализировали данные
                    Interlocked.Exchange(ref _lockAutoUpdate, 0);
                }
            }
        }

        public event Action<string, IConvertible[]> OnAudioMixerFaderStateEvent;

        private void faderLevelChanged()
        {
            try
            {
                if (_unit != null)
                {
                    int value = _unit.GetBandValueNormalize((int)itbCurrentValue.IntegerValue);
                    int mute = Convert.ToInt32(cbMute.Checked);
                    if (OnAudioMixerFaderStateEvent != null)
                        OnAudioMixerFaderStateEvent((string)itbCurrentValue.Tag, new IConvertible[] { _instanceID, mute, value });
                }
            }
            finally
            {
                //после выполнения команды всегда отпустить lock чтобы могло заработать авто-обновление
                Interlocked.Exchange(ref _lockAutoUpdate, 0);
            }
        }

        private void AudioMixerFaderControl_Resize(object sender, EventArgs e)
        {
            Control cntrl = sender as Control;
            tbeTrack.Height = cntrl.Height - 65;
            tbeTrack.Refresh();
        }

        private void tbeTrack_ValueChanged(object sender, EventArgs e)
        {
            //если начинается обновление, то отключить автобновление
            Interlocked.Exchange(ref _lockAutoUpdate, 1);
            itbCurrentValue.IntegerValue = tbeTrack.Value;
        }

        private void itbCurrentValue_IntegerValueChanged(object sender, EventArgs e)
        {
            //если начинается обновление, то отключить автобновление
            Interlocked.Exchange(ref _lockAutoUpdate, 1);
            tbeTrack.Value = (int)itbCurrentValue.IntegerValue;
        }

        private void cbMute_CheckedChanged(object sender, Syncfusion.Windows.Forms.Tools.CheckedChangedEventArgs e)
        {
            CheckBoxAdv cntrl = sender as CheckBoxAdv;
            if ((cntrl != null) && (_unit != null))
            {
                //если мы начинаем отправку команды, то отключить автобновление
                Interlocked.Exchange(ref _lockAutoUpdate, 1);
                tbeTrack.Enabled = !cntrl.Checked;
                faderLevelChanged();
            }
        }

        private void tbeTrack_EnabledChanged(object sender, EventArgs e)
        {
            if (tbeTrack.Enabled)
                tbeTrack.ButtonColor = Color.FromArgb(109, 141, 189);
            else
                tbeTrack.ButtonColor = Color.FromArgb(224, 224, 224);
        }

        private void tbeTrack_MouseUp(object sender, MouseEventArgs e)
        {
            faderLevelChanged();
        }

        private void itbCurrentValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                faderLevelChanged();
            }
        }

        private void itbCurrentValue_Leave(object sender, EventArgs e)
        {
            faderLevelChanged();
        }

    }
}
