using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hosts.Plugins.GangSwitch.SystemModule.Config;
using Syncfusion.Windows.Forms.Tools;
using Action=Syncfusion.Windows.Forms.Tools.Action;

namespace Hosts.Plugins.GangSwitch.Player
{
    public partial class SwitchPlayerControl : UserControl
    {
        //сначала идет команда на On, потом на Off. В таком же порядке заполняется комбобокс
        private string[] _switchCommand = new string[] { "SwitchOn", "SwitchOff" };

        public SwitchPlayerControl()
        {
            InitializeComponent();
        }

        public SwitchPlayerControl(GangSwitchUnitDesign unitState, GangSwitchUnitConfig unitValue, int num) 
            : this()
        {
            comboBoxAdv1.Items.Clear();
            //всегда заполняется сначала значение для On, потом для Off - это важно, не меняйте порядок
            comboBoxAdv1.Items.AddRange(new string[] { unitValue.OnStateName, unitValue.OffStateName });
            comboBoxAdv1.SelectedIndex = Convert.ToInt32(!unitState.OnOffState);
            comboBoxAdv1.Tag = num;
            autoLabel1.Text = unitValue.Name;
        }
        public void UpdateState(bool state)
        {
            comboBoxAdv1.SelectedIndex = Convert.ToInt32(!state);
        }
        public event Action<string, IConvertible[]> PushCommandButtonEvent;

        private void sendPushCommandButtonEvent(string command, params IConvertible[] parameters)
        {
            if (PushCommandButtonEvent != null)
                PushCommandButtonEvent(command, parameters);
        }

        private void SwitchPlayerControl_Resize(object sender, EventArgs e)
        {
            Control cntrl = sender as Control;
            autoLabel1.Width = comboBoxAdv1.Left;
            comboBoxAdv1.Width = cntrl.Width - comboBoxAdv1.Left - 1;
        }

        private void comboBoxAdv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxAdv combo = sender as ComboBoxAdv;
            if ((combo != null) && (combo.Tag != null))
            {
                sendPushCommandButtonEvent(_switchCommand[combo.SelectedIndex], (int)combo.Tag);
            }
        }
    }
}
