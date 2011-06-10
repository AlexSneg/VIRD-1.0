using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Hosts.Plugins.Video.Player
{
    internal partial class PlayerControl : UserControl
    {
        private PlayerController m_Controller = null;
        private readonly object _timerSync = new object();

        internal PlayerControl()
        {
        }

        internal PlayerControl(PlayerController ctrl)
        {
            m_Controller = ctrl;
            InitializeComponent();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            m_Controller.Play();
            UpdateView();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(_timerSync))
            {
                try
                {
                    if (!this.Enabled)
                        return;
                    m_Controller.GetStatus();
                    UpdateView();
                }
                finally
                {
                    Monitor.Exit(_timerSync);
                }
            }
        }

        private void UpdateView()
        {
            this.statusLabel.Text = m_Controller.Status;
            this.playButton.Text = m_Controller.Action;
            this.timeLabel.Text = GetTimeAsString(m_Controller.CurrentPosition);
            UpdateTrackBar();
        }

        private void UpdateTrackBar()
        {
            trackBar.ValueChanged -= trackBar_ValueChanged;
            trackBar.Value = m_Controller.CurrentPosition;
            trackBar.ValueChanged += trackBar_ValueChanged;
        }

        private void PlayerControl_Load(object sender, EventArgs e)
        {
            endTimeLabel.Text = GetTimeAsString(m_Controller.Duration);
            trackBar.Maximum = m_Controller.Duration;
        }

        private static string GetTimeAsString(int seconds)
        {
            DateTime dateTime = new DateTime(
                TimeSpan.FromSeconds(seconds).Ticks);
            return dateTime.ToString("HH:mm:ss");
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            m_Controller.Seek(trackBar.Value);
        }
    }
}
