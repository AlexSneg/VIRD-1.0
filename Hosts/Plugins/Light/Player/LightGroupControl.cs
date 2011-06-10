using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hosts.Plugins.Light.SystemModule.Config;
using Syncfusion.Windows.Forms.Tools;

namespace Hosts.Plugins.Light.Player
{
    public partial class LightGroupControl : UserControl
    {
        public LightGroupControl()
        {
            InitializeComponent();
        }
        public LightGroupControl(LightUnitDesign unitValue, int num)
            : this()
        {
            if (unitValue.IsAdjustable)
            {
                trackBarEx1.Visible = true;
                comboBoxAdv1.Visible = false;
                trackBarEx1.Tag = (string)trackBarEx1.Tag + num.ToString();
            }
            else
            {
                trackBarEx1.Visible = false;
                comboBoxAdv1.Visible = true;
                comboBoxAdv1.Tag = (string)comboBoxAdv1.Tag + num.ToString();
            }
            alName.Text = unitValue.Name;
            UpdateValue(unitValue.Brightness);
            this.Name = "changeControl" + num.ToString();
        }

        public void UpdateValue(int value)
        {
            if (trackBarEx1.Visible)
            {
                trackBarEx1.Value = value;
            }
            else if (comboBoxAdv1.Visible)
            {
                comboBoxAdv1.SelectedIndex = value;
            }
        }

        public event Action<string, IConvertible[]> OnLightGroupStateChanged;

        private void sendPushCommandButtonEvent(string command, params IConvertible[] parameters)
        {
            if (OnLightGroupStateChanged != null)
                OnLightGroupStateChanged(command, parameters);
        }

        private void comboBoxAdv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxAdv comb = sender as ComboBoxAdv;
            if ((comb != null) && (!string.IsNullOrEmpty((string)comb.Tag)))
            {
                string[] prms = ((string)comb.Tag).Split(',');
                if (OnLightGroupStateChanged != null)
                    sendPushCommandButtonEvent(prms[0], Convert.ToInt32(prms[1]), comb.SelectedIndex);
            }
        }

        private void LightGroupControl_MouseUp(object sender, MouseEventArgs e)
        {
            TrackBarEx comb = sender as TrackBarEx;
            if ((comb != null) && (!string.IsNullOrEmpty((string)comb.Tag)))
            {
                string[] prms = ((string)comb.Tag).Split(',');
                sendPushCommandButtonEvent(prms[0], Convert.ToInt32(prms[1]), comb.Value);
            }   
        }

        private void LightGroupControl_Resize(object sender, EventArgs e)
        {
            Control cntrl = sender as Control;
            trackBarEx1.Width = cntrl.Width - 95;
            comboBoxAdv1.Width = cntrl.Width - 95;
            trackBarEx1.Refresh();
            comboBoxAdv1.Refresh();
        }
    }
}
