using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;

namespace Hosts.Plugins.IEDocument.Player
{
    internal partial class PlayerControl : UserControl
    {
        private PlayerController m_Controller = null;
        private readonly object _timerSync = new object();
        private const string _infoFormat = "Слайд {0} из {1}";

        public PlayerControl()
        {
            InitializeComponent();
        }

        private void InitZoom(int LowZoomBoundary, int HighZoomBoundary)
        {
            itbZoom.MaxValue = (int)HighZoomBoundary;
            itbZoom.MinValue = (int)LowZoomBoundary;
            tbeZoom.Maximum = (int)HighZoomBoundary;
            tbeZoom.Minimum = (int)LowZoomBoundary;
            alZoomMin.Text = LowZoomBoundary.ToString();
            alZoomMax.Text = HighZoomBoundary.ToString();
        }

        public PlayerControl(PlayerController controller)
            : this()
        {
            m_Controller = controller;
            InitZoom(10, 500);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            m_Controller.UpScroll();
            //UpdateView();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            m_Controller.DownScroll();
            //UpdateView();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            m_Controller.LeftScroll();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            m_Controller.RightScroll();
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
            //lbInfo.Text = string.Format(_infoFormat, m_Controller.CurrentSlide, m_Controller.NumberOfSlides);
            int intValue = Convert.ToInt32(tbeZoom.Value);
            if (intValue != m_Controller.ZoomProperty)
            {
                tbeZoom.Value = m_Controller.ZoomProperty;
            }
        }

        private void tbeZoom_ValueChanged(object sender, EventArgs e)
        {
            itbZoom.IntegerValue = tbeZoom.Value;
        }

        private void tbeZoom_MouseUp(object sender, MouseEventArgs e)
        {
            TrackBarEx cntrl = (sender as TrackBarEx);
            if (cntrl != null)
            {
                m_Controller.Zoom((int)tbeZoom.Value);
            }
        }

        private void itbZoom_Leave(object sender, EventArgs e)
        {
            IntegerTextBox cntrl = (sender as IntegerTextBox);
            if (cntrl != null)
            {
                m_Controller.Zoom((int)itbZoom.IntegerValue);
            }
        }

        private void itbZoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            IntegerTextBox cntrl = (sender as IntegerTextBox);
            if ((cntrl != null) && (e.KeyChar == Convert.ToChar(13)))
            {
                m_Controller.Zoom((int)itbZoom.IntegerValue);
            }
        }

        private void itbZoom_IntegerValueChanged(object sender, EventArgs e)
        {
            //tbeZoom.Value = (int)itbZoom.IntegerValue;
        }
    }
}
