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
using Action = Syncfusion.Windows.Forms.Tools.Action;

namespace Hosts.Plugins.WordDocument.Player
{
    internal partial class PlayerControl : UserControl
    {
        private PlayerController m_Controller = null;
        private readonly object _timerSync = new object();
        private const string _infoFormat = "Страница {0} из {1}";
        private int _lockAutoUpdate = 1;

        public PlayerControl()
        {
            InitializeComponent();
        }

        private void InitZoom(int LowZoomBoundary, int HighZoomBoundary, int startZoom)
        {
            itbZoom.MaxValue = (int)HighZoomBoundary;
            itbZoom.MinValue = (int)LowZoomBoundary;
            tbeZoom.Maximum = (int)HighZoomBoundary;
            tbeZoom.Minimum = (int)LowZoomBoundary;
            alZoomMin.Text = LowZoomBoundary.ToString();
            alZoomMax.Text = HighZoomBoundary.ToString();
            //itbZoom.IntegerValue = startZoom;
            //tbeZoom.Value = startZoom;
            //Interlocked.Exchange(ref _lockAutoUpdate, 0);
        }

        public PlayerControl(PlayerController controller)
            : this()
        {
            m_Controller = controller;
            InitZoom(10, 500, m_Controller.StartZoom);
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

        private void btnLeft_Click(object sender, EventArgs e)
        {
            m_Controller.LeftScroll();
            UpdateView();    
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            m_Controller.RightScroll();
            UpdateView();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            m_Controller.NextPage();
            UpdateView();
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            m_Controller.PrevPage();
            UpdateView();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            m_Controller.LastPage();
            UpdateView();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            m_Controller.FirstPage();
            UpdateView();
        }

        private void btnGoTo_Click(object sender, EventArgs e)
        {
            int index;
            if (Int32.TryParse(tbPageNumber.Text.Trim(), out index))
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
            lbInfo.Text = string.Format(_infoFormat, m_Controller.CurrentSlide, m_Controller.NumberOfPages);
            
            int intValue = Convert.ToInt32(tbeZoom.Value); 
            if (intValue != m_Controller.StartZoom)
            {
                tbeZoom.Value = m_Controller.StartZoom;
            }
        }

        private void itbZoom_IntegerValueChanged(object sender, EventArgs e)
        {
            //Interlocked.Exchange(ref _lockAutoUpdate, 1);
            //tbeZoom.Value = (int)itbZoom.IntegerValue;
        }

        private void itbZoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            IntegerTextBox cntrl = (sender as IntegerTextBox);
            if ((cntrl != null) && (e.KeyChar == Convert.ToChar(13)))
            {
                //sendNewPosition((string)cntrl.Tag);
                m_Controller.Zoom((int)itbZoom.IntegerValue);
            }
        }

        private void itbZoom_Leave(object sender, EventArgs e)
        {
            IntegerTextBox cntrl = (sender as IntegerTextBox);
            if (cntrl != null)
            {
                //sendNewPosition((string)cntrl.Tag);
                m_Controller.Zoom((int)itbZoom.IntegerValue);
            }
        }

        private void tbeZoom_MouseUp(object sender, MouseEventArgs e)
        {
            TrackBarEx cntrl = (sender as TrackBarEx);
            if (cntrl != null)
            {
                //sendNewPosition((string)cntrl.Tag);
                m_Controller.Zoom((int)tbeZoom.Value);
            }
        }

        private void tbeZoom_ValueChanged(object sender, EventArgs e)
        {
            itbZoom.IntegerValue = tbeZoom.Value;
        }
    }
}
