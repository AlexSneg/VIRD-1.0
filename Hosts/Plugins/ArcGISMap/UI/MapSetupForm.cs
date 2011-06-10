using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hosts.Plugins.ArcGISMap.SystemModule.Design;
using Hosts.Plugins.ArcGISMap.UI.Controls;

namespace Hosts.Plugins.ArcGISMap.UI
{
    public partial class MapSetupForm : Form
    {
        public MapSetupForm()
        {
            InitializeComponent();
        }

        MapControl Map
        {
            get
            {
                return this.map;
            }
            set
            {
                if (value != null)
                {
                    this.map = value;
                    splitContainer.Panel1.Controls.Clear();
                    splitContainer.Panel1.Controls.Add(this.map);
                }
                else
                {
                    //TODO: Придумать, что делать в случае пустой карты.
                }
            }
        }
        ArcGISMapSourceDesign source;

        public MapSetupForm(ArcGISMapSourceDesign source)
        {
            InitializeComponent();

            this.source = source;
            this.Map = source.Map;
            AddLayer("Красный слой", 1);
            AddLayer("Синий слой", 2);
            AddLayer("Сетка", 3);
        }

        private void AddLayer(string nodeName, int tag)
        {
            TreeNode node = treeView.Nodes.Add(nodeName);
            node.Tag = tag;
            node.Checked = true;

        }
        
        private void MapMoveClick(object sender, EventArgs e)
        {
            if (sender == upButton)
            {
                map.MoveUp();
            }
            else if (sender == downButton)
            {
                map.MoveDown();

            }
            else if (sender == leftButton)
            {
                map.MoveLeft();
            }
            else if (sender == rigthButton)
            {
                map.MoveRigth();
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
            map.SetScale(intVal / 100.0);
        }

        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
          switch((int)e.Node.Tag)
          {
              case 1: map.RedLayerVisible = e.Node.Checked; break;
              case 2: map.BlueLayerVisible = e.Node.Checked; break;
              case 3: map.GridVisible = e.Node.Checked; break;
              default: throw new Exception("Соответствующий слой отсутсвует.");
           }
          map.UpdateLayers();
        }

    }
}
