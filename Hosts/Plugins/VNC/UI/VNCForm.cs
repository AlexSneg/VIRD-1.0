using System;
using System.Drawing;
using System.Windows.Forms;

using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;

using Hosts.Plugins.VNC.Common;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

using Label = System.Windows.Forms.Label;

namespace Hosts.Plugins.VNC.UI
{
    internal partial class VNCForm : Form, IDoCommand, IVNCAction
    {
        private const char _portDelimeter = ':';
        private string _host;
        private bool _isConnected;
        private bool _isRemoteControlEnabled;
        private Label _messageLabel;
        private string _password;
        protected int _croppingLeft = 0;
        protected int _croppingRight = 0;
        protected int _croppingTop = 0;
        protected int _croppingBottom = 0;

        public VNCForm()
        {
            InitializeComponent();
        }

        #region IVNCAction Members

        public void Connect()
        {
            ClearErrorMessage();
            try
            {
                int index = _host.IndexOf(_portDelimeter);
                int port = 0;
                string host = _host;
                if (index >= 0)
                {
                    host = _host.Substring(0, index);
                    if (!int.TryParse(_host.Substring(index + 1, _host.Length - index - 1), out port))
                        port = 0;
                }

                if (port > 0)
                    remoteDesktop.VncPort = port;
                remoteDesktop.Connect(host, 0, !_isRemoteControlEnabled, _croppingLeft, _croppingRight, _croppingTop, _croppingBottom);
            }
            catch (Exception ex)
            {
                PrintErrorMessage(string.Format("Ошибка при подключении \n{0}", ex.Message));
            }
        }

        public bool IsConnected()
        {
            return remoteDesktop.IsConnected;
        }

        public void Disconnect()
        {
            ClearErrorMessage();
            try
            {
                if (remoteDesktop.IsConnected)
                    remoteDesktop.Disconnect();
            }
            catch (Exception ex)
            {
                PrintErrorMessage(string.Format("Ошибка при отключении \n{0}", ex.Message));
            }
        }

        #endregion

        public void Init(string host, string password, bool isConnected,
                         bool isRemoteControlEnabled)
        {
            _host = host;
            _password = password;
            _isConnected = isConnected;
            _isRemoteControlEnabled = isRemoteControlEnabled;
            remoteDesktop.GetPassword = GetPassword;
        }

        private string GetPassword()
        {
            return _password;
        }

        private void VNCForm_Load(object sender, EventArgs e)
        {
            if (!_isConnected) return;
            Connect();
        }

        private void VNCForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
            remoteDesktop.Dispose();
        }

        protected void PrintErrorMessage(string errorMessage)
        {
            if (_messageLabel == null)
                _messageLabel = new Label();
            _messageLabel.BackColor = Color.AliceBlue;
            _messageLabel.Text = errorMessage;
            _messageLabel.TextAlign = ContentAlignment.MiddleCenter;
            _messageLabel.Dock = DockStyle.Fill;
            remoteDesktop.Hide();
            Controls.Add(_messageLabel);
            _messageLabel.Show();
        }

        protected void ClearErrorMessage()
        {
            if (_messageLabel == null) return;
            _messageLabel.Hide();
            remoteDesktop.Show();
            Controls.Remove(_messageLabel);
            _messageLabel = null;
        }

        internal bool Init(DisplayType display, Window window)
        {
            this.InitBorderTitle(window, remoteDesktop);
            ActiveWindow activeWindow = window as ActiveWindow;
            if (activeWindow != null)
            {
                _croppingBottom = activeWindow.CroppingBottom;
                _croppingLeft = activeWindow.CroppingLeft;
                _croppingRight = activeWindow.CroppingRight;
                _croppingTop = activeWindow.CroppingTop;
            }
            return true;
        }

        #region Implementation of IDoCommand

        public string DoCommand(string command)
        {
            return VNCCommand.Parse(command).DoCommand(this);
        }

        #endregion
    }
}