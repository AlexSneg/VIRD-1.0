using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Hosts.Plugins.PowerPointPresentation.Player
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

        public PlayerControl(PlayerController controller)
            : this()
        {
            m_Controller = controller;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            m_Controller.Prev();
            UpdateView();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            m_Controller.Next();
            UpdateView();
        }

        private void btnGoTo_Click(object sender, EventArgs e)
        {
            int index;
            if (Int32.TryParse(tbSlideNumber.Text.Trim(), out index))
            {
                m_Controller.Goto(index);
                UpdateView();
            }
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
            lbInfo.Text = string.Format(_infoFormat, m_Controller.CurrentSlide, m_Controller.NumberOfSlides);
        }
    }
}
