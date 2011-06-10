using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using Syncfusion.Windows.Forms;
using Hosts.Plugins.VDCServer.SystemModule.Design;
using Hosts.Plugins.VDCServer.SystemModule.Config;

namespace Hosts.Plugins.VDCServer.Player
{
    public partial class VDCServerPlayerControl : DeviceHardPluginBaseControl, IVDCServerPlayerView
    {
        private List<VDCServerAbonentInfo> _abonentDictionary = null;
        private List<VDCServerAbonentInfo> _members = null;
        public VDCServerPlayerControl()
        {
            InitializeComponent();
        }
        public VDCServerPlayerControl(IEventLogging logging, Device device, IPlayerCommand playerProvider)
            : this()
        {
            InitializeController(new VDCServerPlayerController(device, playerProvider, this, logging));
            SetControlPlayerTimerEnable(true, 1000);
        }
        
        protected override void sendPushCommandButtonEvent(string command, params IConvertible[] parameters)
        {
            if (!IsHardwareAvailable)
            {
                MessageBoxAdv.Show("Нет связи с устройством", "Предупреждение", MessageBoxButtons.OK);
                return;
            }
            base.sendPushCommandButtonEvent(command, parameters);
        }

        public void InitializeData(VDCServerDeviceDesign device, VDCServerDeviceConfig config)
        {
            _abonentDictionary = device.AbonentList;
            _members = device.Members;
            
            alName.Text = device.ConferenceName;
            alComment.Text = string.IsNullOrEmpty(device.Comments) ? "комментарий отсутсвует" : device.Comments;
            UpdateView(true, device.IsConferenceActoive, _members);
            alVoiceSwitched.Text = device.VoiceSwitched ? "Да" : "Нет";
            if ((_members != null) && (_members.Count > 0) && (device.ActiveMember != null))
                lbMembers.SelectedItem = device.ActiveMember.Name;

            cbaSettings.DataSource = config.ScreenLayoutList;
            baApply.Enabled = !(cbaSettings.SelectedValue == null);
            if (device.Layout != null)
            {
                cbaSettings.SelectedValue = device.Layout.LayoutNumber;
                if (device.Layout.LayoutPicture != null)
                    pictureBox1.Image = device.Layout.LayoutPicture;
            }
        }

        public void UpdateView(bool available, bool status, List<VDCServerAbonentInfo> members)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool, bool, List<VDCServerAbonentInfo>>(UpdateView), available, status, members);
                return;
            }
            SetAvailableStatus(available);
            if (available)
            {
                alStatus.Text = status ? "Активна" : "Неактивна";
                baStatus.BackgroundImage = status ?
                    global::Hosts.Plugins.VDCServer.Properties.Resources.Phone_off :
                    global::Hosts.Plugins.VDCServer.Properties.Resources.Phone;
                baRemoveMember.Enabled = baAddMember.Enabled = baFocus.Enabled = status;
                baStatus.State = status ? ButtonAdvState.Pressed : ButtonAdvState.Default;
                if (members != null) _members = members;
                UpdateMembersControl();
            }
        }

        private void UpdateMembersControl()
        {
            if (_members != null)
            {
                string curAbn = (string)lbAbonents.SelectedItem;
                lbAbonents.Items.Clear();
                foreach (VDCServerAbonentInfo item in _abonentDictionary.Where(abn => !_members.Any(mem => mem.Name.Equals(abn.Name))))
                {
                    lbAbonents.Items.Add(item.Name);
                }
                lbAbonents.SelectedItem = curAbn;
                curAbn = (string)lbMembers.SelectedItem;
                lbMembers.Items.Clear();
                foreach (VDCServerAbonentInfo item in _members)
                {
                    lbMembers.Items.Add(item.Name);
                }
                lbMembers.SelectedItem = curAbn;
            }
        }

        private void gpMain_Resize(object sender, EventArgs e)
        {
            Control parent = sender as Control;
            lbMembers.Width = lbAbonents.Width = (parent.Width - 26) / 2;
            baAddMember.Left = lbMembers.Width + 2;
            baRemoveMember.Left = lbMembers.Width + 2;
            baRemoveMember.Top = lbMembers.Height - baRemoveMember.Height;
            cbaSettings.Width = parent.Width - 95;
        }

        #region отправка команд
        /// <summary>
        /// создание/завершение конференции
        /// </summary>
        private void baStatus_Click(object sender, EventArgs e)
        {
            string[] cmds = ((string)baStatus.Tag).Split(',');
            //если нажата, то команда завершения конференции, иначе команда начала
            string cmd = baStatus.State == ButtonAdvState.Pressed ? cmds[1] : cmds[0];
            if (baStatus.State == ButtonAdvState.Pressed)
            {
                //создать конференцию
                if (cbaSettings.SelectedValue != null)
                    sendPushCommandButtonEvent(cmd, (int)cbaSettings.SelectedValue);
                else
                    MessageBoxAdv.Show(this, "Не задана раскладка", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else
            {
                //закончить конференцию
                sendPushCommandButtonEvent(cmd, alName.Text);
            }
        }
        /// <summary>
        /// апдейт конференции, а именно смена раскладки
        /// </summary>
        private void baApply_Click(object sender, EventArgs e)
        {
            sendPushCommandButtonEvent((string)baApply.Tag, (int)cbaSettings.SelectedValue);
        }
        /// <summary>
        /// поставить фокус на участника
        /// </summary>
        private void baFocus_Click(object sender, EventArgs e)
        {
            string member = lbMembers.SelectedItem as string;
            if (!string.IsNullOrEmpty(member))
                sendPushCommandButtonEvent((string)baFocus.Tag, alName.Text, member);
        }
        /// <summary>
        /// добавить участника
        /// </summary>
        private void baAddMember_Click(object sender, EventArgs e)
        {
            string member = lbAbonents.SelectedItem as string;
            if (!string.IsNullOrEmpty(member))
            {
                sendPushCommandButtonEvent((string)baAddMember.Tag, member);
            }
        }
        /// <summary>
        /// удалить участника
        /// </summary>
        private void baRemoveMember_Click(object sender, EventArgs e)
        {
            string member = lbMembers.SelectedItem as string;
            if (!string.IsNullOrEmpty(member))
                sendPushCommandButtonEvent((string)baRemoveMember.Tag, alName.Text, member);
        }
        #endregion

        private void cbaSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<ScreenLayout> screenLayouts = cbaSettings.DataSource as List<ScreenLayout>;
            int layoutNumber = (int)cbaSettings.SelectedValue;
            pictureBox1.Image = screenLayouts.Find(lay => lay.LayoutNumber == layoutNumber).LayoutPicture;
            baApply.Enabled = !(cbaSettings.SelectedValue == null);
        }

    }
}
