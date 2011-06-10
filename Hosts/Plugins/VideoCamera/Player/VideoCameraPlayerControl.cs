using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using Syncfusion.Windows.Forms.Tools;
using TechnicalServices.Entity;
using System.Threading;
using Action=Syncfusion.Windows.Forms.Tools.Action;

namespace Hosts.Plugins.VideoCamera.Player
{
    public partial class VideoCameraPlayerControl : SourceHardPluginBaseControl, IVideoCameraPlayerView
    {
        private int _lockAutoUpdate = 1;
        private bool _isDomical;
        private decimal _lowZoomBoundary;
        private decimal _highZoomBoundary;

        public VideoCameraPlayerControl()
        {
            InitializeComponent();
        }
        public void InitializeData(int presetAmount, int preset, bool isDomical,
            decimal LowZoomBoundary, decimal HighZoomBoundary)
        {
            _isDomical = isDomical;
            _lowZoomBoundary = LowZoomBoundary;
            _highZoomBoundary = HighZoomBoundary;
            InitializeData();
            InitPreset(presetAmount);
            InitZoom(LowZoomBoundary, HighZoomBoundary);
            if (isDomical)
                InvertTilt();
            cbaPreset.SelectedItem = preset.ToString();
            baSave.Enabled = cbaPreset.SelectedItem == null ? false : true;
            CollapsedRGBOption = true;
            Interlocked.Exchange(ref _lockAutoUpdate, 0);
        }

        

        public void UpdateView(ValueThree<int, int, int> state)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ValueThree<int, int, int>>(UpdateView), state);
                return;
            }
            //авто обновление можно делать только, если не редактируется текущее значение
            if (_lockAutoUpdate == 0)
            {
                try
                {
                    itbPan.IntegerValue = state.Value1;
                    itbTilt.IntegerValue = state.Value2;
                    itbZoom.IntegerValue = state.Value3;
                }
                finally
                {
                    //освободим лок, он мог установится, когда мы инициализировали данные
                    Interlocked.Exchange(ref _lockAutoUpdate, 0);
                }
            }
        }

        public ValueThree<int, int, int> GetState 
        {
            get
            {
                return new ValueThree<int, int, int>(
                    (int)itbPan.IntegerValue,
                    (int)itbTilt.IntegerValue,
                    (int)itbZoom.IntegerValue);
            }
        }

        protected override void sendPushCommandButtonEvent(string command, params IConvertible[] parameters)
        {
            try
            {
                base.sendPushCommandButtonEvent(command, parameters);
            }
            finally
            {
                //после выполнения команды всегда отпустить lock чтобы могло заработать авто-обновление
                Interlocked.Exchange(ref _lockAutoUpdate, 0);
            }
        }

        private void sendNewPosition(string command)
        {
            sendPushCommandButtonEvent(command,
                (int)itbPan.IntegerValue,
                (int)itbTilt.IntegerValue,
                (int)itbZoom.IntegerValue);
        }

        #region private members

        private void cbaPreset_ActionSend(object sender, EventArgs e)
        {
            baSave.Enabled = cbaPreset.SelectedItem == null ? false : true;
            string cmd = (sender as Control).Tag as string;
            if (!string.IsNullOrEmpty(cmd))
            {
                int numPreset = Convert.ToInt32(cbaPreset.SelectedItem);
                if (numPreset > 0)
                    sendPushCommandButtonEvent(cmd, numPreset);
            }
        }
        private void controlSendHomeCommand_Click(object sender, EventArgs e)
        {
            string cmd = (sender as Control).Tag as string;
            if (!string.IsNullOrEmpty(cmd))
            {
                sendPushCommandButtonEvent(cmd, null);
            }
        }

        private void InitPreset(int presetAmount)
        {
            string[] result = new string[presetAmount];
            for (int i = 1; i <= presetAmount; i++)
            {
                result[i - 1] = i.ToString();
            }
            cbaPreset.Items.AddRange(result);
        }
        private void InitZoom(decimal LowZoomBoundary, decimal HighZoomBoundary)
        {
            itbZoom.MaxValue = (int)HighZoomBoundary;
            itbZoom.MinValue = (int)LowZoomBoundary;
            tbeZoom.Maximum = (int)HighZoomBoundary;
            tbeZoom.Minimum = (int)LowZoomBoundary;
            alZoomMin.Text = LowZoomBoundary.ToString();
            alZoomMax.Text = HighZoomBoundary.ToString();
        }
        private void InvertTilt()
        {
            string max = alTiltMax.Text;
            alTiltMax.Text = alTiltMin.Text;
            alTiltMin.Text = max;
        }
        private void tbPan_ValueChanged(object sender, EventArgs e)
        {
            //если начинается обновление, то отключить автобновление
            Interlocked.Exchange(ref _lockAutoUpdate, 1);
            itbPan.IntegerValue = tbePan.Value;
        }

        private void tbTilt_ValueChanged(object sender, EventArgs e)
        {
            //если начинается обновление, то отключить автобновление
            Interlocked.Exchange(ref _lockAutoUpdate, 1);
            itbTilt.IntegerValueChanged -= itbTilt_IntegerValueChanged;
            itbTilt.IntegerValue = (_isDomical ? -1 : 1) * tbeTilt.Value;
            itbTilt.IntegerValueChanged += itbTilt_IntegerValueChanged;
        }

        private void tbZoom_ValueChanged(object sender, EventArgs e)
        {
            //если начинается обновление, то отключить автобновление
            Interlocked.Exchange(ref _lockAutoUpdate, 1);
            itbZoom.IntegerValue = tbeZoom.Value;
        }

        private void itbPan_IntegerValueChanged(object sender, EventArgs e)
        {
            //если начинается обновление, то отключить автобновление
            Interlocked.Exchange(ref _lockAutoUpdate, 1);
            tbePan.Value = (int)itbPan.IntegerValue;
        }

        private void itbTilt_IntegerValueChanged(object sender, EventArgs e)
        {
            //если начинается обновление, то отключить автобновление
            Interlocked.Exchange(ref _lockAutoUpdate, 1);
            tbeTilt.ValueChanged -= tbTilt_ValueChanged;
            tbeTilt.Value = (_isDomical ? -1 : 1) * (int)itbTilt.IntegerValue;
            tbeTilt.ValueChanged += tbTilt_ValueChanged;
        }

        private void itbZoom_IntegerValueChanged(object sender, EventArgs e)
        {
            //если начинается обновление, то отключить автобновление
            Interlocked.Exchange(ref _lockAutoUpdate, 1);
            tbeZoom.Value = (int)itbZoom.IntegerValue;
        }

        private void tbeControl_MouseUp(object sender, MouseEventArgs e)
        {
            TrackBarEx cntrl = (sender as TrackBarEx);
            if (cntrl != null)
            {
                sendNewPosition((string)cntrl.Tag);
            }
        }

        private void itbControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            IntegerTextBox cntrl = (sender as IntegerTextBox);
            if ((cntrl != null) && (e.KeyChar == Convert.ToChar(13)))
            {
                sendNewPosition((string)cntrl.Tag);
            }
        }

        private void itbControl_Leave(object sender, EventArgs e)
        {
            IntegerTextBox cntrl = (sender as IntegerTextBox);
            if (cntrl != null)
            {
                sendNewPosition((string)cntrl.Tag);
            }
        }
        #endregion

    }
}
