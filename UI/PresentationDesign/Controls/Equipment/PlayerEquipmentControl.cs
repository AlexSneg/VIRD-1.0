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
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Entity;

namespace UI.PresentationDesign.DesignUI.Controls.Equipment
{
    public partial class PlayerEquipmentControl : UserControl
    {
        private PlayerEquipmentController m_Controller;
        private int m_SelectedItem = -1;

        public PlayerEquipmentControl()
        {
            InitializeComponent();
            groupView1.SmallImageView = true;
            groupView1.Enabled = true;
        }

        public void AssignController(PlayerEquipmentController ctrl)
        {
            m_Controller = ctrl;
            FillData();
            m_Controller.OnDeviceStateChanged += m_Controller_OnDeviceStateChanged;
            m_Controller.OnEquipmentFreezeChanged += m_Controller_OnEquipmentFreezeChanged;
            m_Controller.OnDeviceListChanged += m_Controller_OnDeviceListChanged;
        }
        
        void m_Controller_OnDeviceListChanged()
        {
            FillData();
        }

        private void m_Controller_OnDeviceStateChanged(string device, bool? isOnline)
        {
            //если устройство доступно, то пометим его красным
            for(int i = 0; i < this.groupView1.GroupViewItems.Count; i++)
                if (this.groupView1.GroupViewItems[i].Text == device)
                {
                    groupView1.MarkedItems[this.groupView1.GroupViewItems[i]] = isOnline;
                    //if (!isOnline)
                    //    groupView1.MarkedItems.Add(this.groupView1.GroupViewItems[i]);
                    //else
                    //    groupView1.MarkedItems.Remove(this.groupView1.GroupViewItems[i]);
                    break;
                }
            groupView1.Refresh();
        }

        private void m_Controller_OnEquipmentFreezeChanged(string device, FreezeStatus state)
        {
            //еизменилась заморозка настроек, внесем или вычеркнем из выделенных жирным шрифтом
            for (int i = 0; i < this.groupView1.GroupViewItems.Count; i++)
                if (this.groupView1.GroupViewItems[i].Text == device)
                {
                    if (state == FreezeStatus.Freeze)
                        groupView1.MarkedBoldItems.Add(this.groupView1.GroupViewItems[i]);
                    else
                        groupView1.MarkedBoldItems.Remove(this.groupView1.GroupViewItems[i]);
                    break;
                }
            groupView1.Refresh();
        }

        private void FillData()
        {
            this.groupView1.GroupViewItems.Clear();
            this.groupView1.MarkedItems.Clear();
            this.groupView1.MarkedBoldItems.Clear();
            foreach (ValueThree<Device, bool, FreezeStatus> p in m_Controller.Items)
            {
                GroupViewItem item = new GroupViewItem(
                    p.Value1.Type.Name,
                    Convert.ToInt32(p.Value2), true, p.Value1, p.Value1.Type.Name);
                this.groupView1.GroupViewItems.Add(item);
                //если у устройства установлен флажок "применить для всего сценария" то выделить жирным
                if (p.Value3 == FreezeStatus.Freeze) this.groupView1.MarkedBoldItems.Add(item);
            }
            //первое заполнение, пользователь еще не выбирал устройство
            if (this.m_SelectedItem == -1) this.m_SelectedItem = 0;
            this.groupView1.SelectedItem = this.m_SelectedItem;
            if (this.m_SelectedItem != -1 && groupView1.SelectedItem != -1)
                this.m_Controller.ChangeSelectedItem(
                    (Device)groupView1.GroupViewItems[groupView1.SelectedItem].Tag);
        }

        private void groupView1_GroupViewItemSelected(object sender, EventArgs e)
        {
            int oldSelection = this.m_SelectedItem;
            this.m_SelectedItem = groupView1.SelectedItem;
            if (this.m_SelectedItem != oldSelection)
            {
                this.m_Controller.ChangeSelectedItem(
                    (Device)groupView1.GroupViewItems[groupView1.SelectedItem].Tag);
            }
        }
    }
}
