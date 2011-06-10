using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hosts.Plugins.ArcGISMap.Player
{
    public partial class MapManageControl : UserControl
    {
        public MapManageControl()
        {
            InitializeComponent();
        }

        ArcGISMapController controller;
        public MapManageControl(ArcGISMapController controller)
        {
            InitializeComponent();
            this.controller = controller;
        }

        private void MapMoveClick(object sender, EventArgs e)
        {
            if (sender == upButton)
            {
                controller.Move(MoveDirection.Up);
            }
            else if (sender == downButton)
            {
                controller.Move(MoveDirection.Down);
            }
            else if (sender == leftButton)
            {
                controller.Move(MoveDirection.Left);
            }
            else if (sender == rigthButton)
            {
                controller.Move(MoveDirection.Right);
            }
        }

        private void scaleCombo_TextChanged(object sender, EventArgs e)
        {
            string val = scaleCombo.Text.Trim().Replace("%", "");
            int intVal = 0;
            if (val.Length > 0)
            {
                intVal = int.Parse(val);
            }
            controller.Scale(intVal / 100.0);
        }
    }
}
