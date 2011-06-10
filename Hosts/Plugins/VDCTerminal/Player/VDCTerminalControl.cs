using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using Syncfusion.Windows.Forms;
using Hosts.Plugins.VDCTerminal.SystemModule.Design;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.VDCTerminal.Player
{
    public partial class VDCTerminalControl : SourceHardPluginBaseControl, IVDCTerminalView
    {
        /// <summary>обычно контроллер в базовом контроле объявлен, но здесь он используется для специфических задач </summary>
        private PlayerController _controller;
        private const string emptyAbonent = " ";
        public VDCTerminalControl()
        {
            InitializeComponent();
        }
        public VDCTerminalControl(Source source, IPlayerCommand playerCommand, IEventLogging logging, IPresentationClient client)
            : this()
        {
            CollapsedRGBOption = true;
            cbaAbonents.DisplayMember = "Name";
            cbaAbonents.ValueMember = "Number1";

            _controller = new PlayerController(client, source, this, playerCommand, logging);
            InitializeController(_controller);
            SetControlPlayerTimerEnable(true, 1000);
        }

        public void InitializeData(VDCTerminalDeviceDesign device,
            List<VDCTerminalAbonentInfo> abonents,
            VDCTerminalAbonentInfo abonent, string directNumber)
        {
            base.InitializeData();
            List<VDCTerminalAbonentInfo> abnsTemp = new List<VDCTerminalAbonentInfo>();
            abnsTemp.Add(new VDCTerminalAbonentInfo { Name = emptyAbonent, Number1 = emptyAbonent });
            abnsTemp.AddRange(abonents);
            cbaAbonents.DataSource = abnsTemp;
            string abnNumber = abonent != null ? abonent.Number1 : !string.IsNullOrEmpty(directNumber) ? directNumber : emptyAbonent;
            tbeAbobentNumber.Text = abnNumber.Trim();
            cbaAbonents.SelectedValue = abnNumber;

            string incomingCall = device.IncomingCallOwner == null ? null : device.IncomingCallOwner.Number1;
            UpdateView(true, device.ConnectionState, device.StateDescription, device.ErrorDescription,
                incomingCall, device.Privacy, device.DND, device.PeopleConnect, device.PiP, device.AutoResponse);
        }

        private delegate void UpdateViewDelegate(
            bool available, ConnectionStateEnum callState, string status, string error,
            string incomingCall, bool privacy, bool dnd, bool content, bool pip, bool auto);

        public void UpdateView(bool available, ConnectionStateEnum callState, string status, string error, 
            string incomingCall,  bool privacy, bool dnd, bool content, bool pip, bool auto)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateViewDelegate(UpdateView), available, callState, status, error,
                       incomingCall, privacy, dnd, content, pip, auto);
                return;
            }
            SetAvailableStatus(available);
            if (available)
            {
                baCallState.Image = callState == ConnectionStateEnum.Connected ?
                    global::Hosts.Plugins.VDCTerminal.Properties.Resources.Phone_off :
                    global::Hosts.Plugins.VDCTerminal.Properties.Resources.Phone;
                baCallState.Enabled = callState == ConnectionStateEnum.Connected ? true : false;
                alStatusDesc.Text = status + (error == string.Empty ? string.Empty : "/" + error);
                if (alStatusDesc.Text.Trim() == "/") alStatusDesc.Text = string.Empty;
                UpdateIncomingCallText(incomingCall);
                baIncomingCancel.Enabled = string.IsNullOrEmpty(incomingCall) ? false : true;
                baIncomingAnswer.Enabled = string.IsNullOrEmpty(incomingCall) ? false : true;
                SetButtonState(baPrivacy, privacy);
                SetButtonState(baDND, dnd);
                SetButtonState(baContent, content);
                SetButtonState(baPIP, pip);
                SetButtonState(baAuto, auto);
            }
        }

        private void UpdateIncomingCallText(string number1)
        {
            if (string.IsNullOrEmpty(number1))
                alIncomingDesc.Text = string.Empty;
            else
            {
                VDCTerminalAbonentInfo abn = _controller == null ? null : _controller.FindAbonentByNumber(number1);
                if (abn == null)
                {
                    if (number1.Equals("нет входящего звонка"))
                        alIncomingDesc.Text = number1;
                    else
                        alIncomingDesc.Text = "Звонит " + number1;
                }
                else
                {
                    alIncomingDesc.Text = "Звонит " + abn.Name + " (" + abn.Number1 + ")";
                }
            }

        }

        private void SetButtonState(ButtonAdv button, bool state)
        {
            button.State = state ? ButtonAdvState.Pressed : ButtonAdvState.Default;
            button.Refresh();
        }

        private void cbaAbonents_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbaAbonents.SelectedValue != null)
                tbeAbobentNumber.Text = cbaAbonents.SelectedValue.ToString().Trim();
        }

        private void tbeAbobentNumber_TextChanged(object sender, EventArgs e)
        {
            if (_controller != null)
            {
                VDCTerminalAbonentInfo abn = _controller.FindAbonentByNumber(tbeAbobentNumber.Text);
                if (abn != null)
                    cbaAbonents.SelectedValue = abn.Number1;
                else
                    cbaAbonents.SelectedValue = emptyAbonent;
            }
        }

        private void baSendCommand_Click(object sender, EventArgs e)
        {
            string cmd = (sender as ButtonAdv).Tag as string;
            if (!string.IsNullOrEmpty(cmd))
            {
                int state = (sender as ButtonAdv).State == ButtonAdvState.Pressed ? 1 : 0;
                sendPushCommandButtonEvent(cmd, state);
            }
        }

        private void baDial_Click(object sender, EventArgs e)
        {
            string cmd = (sender as ButtonAdv).Tag as string;
            if (!string.IsNullOrEmpty(cmd))
            {
                VDCTerminalAbonentInfo abn = _controller.FindAbonentByNumber(tbeAbobentNumber.Text);
                if (abn != null)
                {
                    sendPushCommandButtonEvent(cmd, abn.Name, abn.Number1, abn.Number2, abn.ConnectionType.ToString(), abn.ConnectionQuality.ToString());
                }
                else
                    sendPushCommandButtonEvent(cmd, string.Empty, tbeAbobentNumber.Text.Trim(), string.Empty, ConnectionTypeEnum.Auto.ToString(), ConnectionQualityEnum.Auto.ToString());
            }
        }

        private void baIncomingCancel_Click(object sender, EventArgs e)
        {
            string cmd = (sender as ButtonAdv).Tag as string;
            if (!string.IsNullOrEmpty(cmd))
            {
                sendPushCommandButtonEvent(cmd, 0);
            }
        }

        private void baIncomingAnswer_Click(object sender, EventArgs e)
        {
            string cmd = (sender as ButtonAdv).Tag as string;
            if (!string.IsNullOrEmpty(cmd))
            {
                sendPushCommandButtonEvent(cmd, 1);
            }
        }

        private void baCallState_Click(object sender, EventArgs e)
        {
            string cmd = (sender as ButtonAdv).Tag as string;
            if (!string.IsNullOrEmpty(cmd))
            {
                sendPushCommandButtonEvent(cmd, null);
            }
        }

    }
}
