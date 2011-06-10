using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hosts.Plugins.BusinessGraphics.Player
{
    public partial class ChartManageControl : UserControl
    {
        BusinessGraphicsController controller;

        public ChartManageControl(BusinessGraphicsController controller)
        {
            InitializeComponent();

            this.controller = controller;
            interactiveCheckBox.Checked = controller.IsInteractive;
            interactiveCheckBox.CheckedChanged += interactiveCheckBox_CheckedChanged;
            timer1.Enabled = true;
        }

        private void interactiveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            controller.EnableInteractive(interactiveCheckBox.Checked);
        }

        private void makeDefaultButton_Click(object sender, EventArgs e)
        {
            controller.MakeDefault();
            interactiveCheckBox.Checked = controller.IsInteractive;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            controller.UpdateControlState();
            interactiveCheckBox.Checked = controller.IsInteractive;
        }
    }
}
