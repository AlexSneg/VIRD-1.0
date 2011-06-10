using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Hosts.Plugins.VNC.SystemModule.Design;

namespace Hosts.Plugins.VNC.Player
{
    internal partial class VNCPlayerControl : UserControl
    {
        private readonly PlayerController m_Controller;
        private readonly object _timerSync = new object();

        internal VNCPlayerControl()
        {
            InitializeComponent();
        }

        internal VNCPlayerControl(PlayerController ctrl)
        {
            m_Controller = ctrl;
            InitializeComponent();
        }

        private void controlButton_Click(object sender, EventArgs e)
        {
            m_Controller.Connect();
            UpdateView();
        }

        private void UpdateView()
        {
            ConnectionStatus connectionStatus = m_Controller.Status;
            this.statusLabel.Text = connectionStatus == ConnectionStatus.Connected
                                        ?
                                            "Connected"
                                        : "Disconnected";
            this.controlButton.Text = connectionStatus == ConnectionStatus.Connected
                                          ?
                                              "Disconnect"
                                          :
                                              "Connect";
        }

        private void VNCPlayerControl_Load(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(_timerSync))
            {
                try
                {
                    m_Controller.GetStatus();
                    UpdateView();
                }
                finally
                {
                    Monitor.Exit(_timerSync);
                }
            }
        }
    }
}
