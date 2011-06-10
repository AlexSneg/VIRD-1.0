using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using UI.PresentationDesign.DesignUI.Controllers;
using Syncfusion.Windows.Forms.Tools;
using System.Collections;
using System.Reflection;
using System.Drawing.Drawing2D;
using Syncfusion.Drawing;
using System.Drawing.Text;
using UI.PresentationDesign.DesignUI.Helpers;

namespace UI.PresentationDesign.DesignUI.Controls.SourceList
{
    public partial class PlayerSourcesControl : UserControl
    {
        private PlayerSourcesController m_Controller;
        private object m_SelectedItem = null;
        private bool m_reflectOnGroupSelected = true;

        public PlayerSourcesControl()
        {
            InitializeComponent();
        }

        public void AssignController(PlayerSourcesController ctrl)
        {
            m_Controller = ctrl;
            m_Controller.OnSourcesChanged += new SourcesChanged(m_Controller_OnSourcesChanged);
            m_Controller.OnCurrentSourceChanged += new CurrentSourceNameChanged(m_Controller_OnCurrentSourceChanged);

            groupBar1.GroupBarItemSelected += new EventHandler(groupBar1_GroupBarItemSelected);
        }

        void groupBar1_GroupBarItemSelected(object sender, EventArgs e)
        {
            if (!m_reflectOnGroupSelected)
                return;
            GroupView view = groupBar1.GroupBarItems[groupBar1.SelectedItem].Client as GroupView;
            if (view.GroupViewItems.Count > 0)
            {
                view.SelectedItem = 0;
                m_Controller.ChangeSelectedItem(view.GroupViewItems[view.SelectedItem].Tag);
            }
            for (int i = 0; i < groupBar1.GroupBarItems.Count; i++)
                if (i != groupBar1.SelectedItem)
                    (groupBar1.GroupBarItems[i].Client as GroupView).SelectedItem = -1;
        }

        void m_Controller_OnCurrentSourceChanged(string sourceName)
        {
            try
            {
                m_reflectOnGroupSelected = false;
                for (int i = 0; i < groupBar1.GroupBarItems.Count; i++)
                {
                    for (int j = 0; j < (groupBar1.GroupBarItems[i].Client as GroupView).GroupViewItems.Count; j++)
                    {
                        if ((groupBar1.GroupBarItems[i].Client as GroupView).GroupViewItems[j].Text == sourceName)
                        {
                            groupBar1.SelectedItem = i;
                            (groupBar1.GroupBarItems[i].Client as GroupView).SelectedItem = j;
                            return;
                        }
                    }
                    (groupBar1.GroupBarItems[i].Client as GroupView).SelectedItem = -1;
                }
            }
            finally
            {
                m_reflectOnGroupSelected = true;
            }
        }

        private void clear()
        {
            foreach (GroupBarItem item in this.groupBar1.GroupBarItems)
            {
                item.Client.Dispose();
                item.Client = null;
            }
            this.groupBar1.GroupBarItems.Clear();
        }

        void m_Controller_OnSourcesChanged()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(m_Controller_OnSourcesChanged));
                return;
            }
            clear();
            foreach (var cat in m_Controller.Categories)
            {
                GroupBarItem item = new GroupBarItem() { Text = cat.Key };
                this.groupBar1.GroupBarItems.Add(item);
                SwitchableGroupView view = createGroupView();
                item.Client = view;
                foreach (var src in cat.Value)
                {
                    var newItem = new GroupViewItem() { Text = src.Key, ToolTipText = src.Key, Tag = src.Value };
                    view.GroupViewItems.Add(newItem);
                    view.MarkedItems[newItem] = m_Controller.States[cat.Key][src.Key];
                    //if (!m_Controller.States[cat.Key][src.Key])
                    //    view.MarkedItems.Add(newItem);
                }
            }
            if (this.groupBar1.GroupBarItems.Count > 0)
            {
                this.groupBar1.SelectedItem = 0;
                GroupView currView = (this.groupBar1.GroupBarItems[0].Client as GroupView);
                if (currView.GroupViewItems.Count > 0)
                {
                    this.m_Controller.ChangeSelectedItem(currView.GroupViewItems[0].Tag);
                    return;
                }
            }
            this.m_Controller.ChangeSelectedItem(null);
        }

        private SwitchableGroupView createGroupView()
        {
            SwitchableGroupView view = new SwitchableGroupView();
            view.SelectedHighlightItemColor = Color.LightBlue;
            view.BackColor = Color.White;
            view.SmallImageView = true;
            view.ShowToolTips = true;
            view.ButtonView = true;
            view.ThemesEnabled = true;
            view.MouseDown += new MouseEventHandler(view_MouseDown);
            return view;
        }

        void view_MouseDown(object sender, MouseEventArgs e)
        {
            GroupView view = sender as GroupView;
            view.SelectedItem = view.HighlightedItem;
            object oldSelection = this.m_SelectedItem;
            this.m_SelectedItem = view.SelectedItem == -1 ? null : view.GroupViewItems[view.SelectedItem].Tag;
            if (e.Button == MouseButtons.Right)
            {
                if (this.m_SelectedItem != null)
                    contextMenuStripEx1.Show(view, e.Location);
            }
            m_Controller.ChangeSelectedItem(this.m_SelectedItem);
        }

        private void propsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.m_SelectedItem != null)
                m_Controller.ShowProperties(this.m_SelectedItem);
        }
    }
}
