using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

using TechnicalServices.Interfaces;

namespace UI.Agent.AgentUI
{
    public partial class MainForm : Form
    {
        private const int MaxLines = 100;
        private readonly IBackgroundProvider _backgroundProvider;
        private readonly EventWaitHandle _exitFlag;
        private ListBox _messageList;

        protected internal MainForm()
        {
            InitializeComponent();
        }

        public MainForm(EventWaitHandle flag, IBackgroundProvider backgroundProvider)
            : this()
        {
            _exitFlag = flag;
            _backgroundProvider = backgroundProvider;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _exitFlag.Set();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _exitFlag.Set();
        }

        public void SetBackgroundImage(string imageFile)
        {
            if (InvokeRequired)
            {
                Action<string> callback = SetBackgroundImage;
                Invoke(callback, imageFile);
            }
            else
            {
                SuspendLayout();
                _backgroundProvider.SetBackgroundImage(imageFile, this, this.Width, this.Height);
                ResumeLayout();
            }
        }

        public void OpenMessageView()
        {
            if (InvokeRequired)
            {
                Action callback = OpenMessageView;
                Invoke(callback);
            }
            else
            {
                SuspendLayout();
                _messageList = new ListBox();
                _messageList.Name = "_messageList";
                _messageList.Dock = DockStyle.Bottom;
                _messageList.Height = 100;
                Controls.Add(_messageList);
                ResumeLayout();
            }
        }

        public void CloseMessageView()
        {
            if (InvokeRequired)
            {
                Action callback = CloseMessageView;
                Invoke(callback);
            }
            else
            {
                SuspendLayout();
                Controls.Remove(_messageList);
                _messageList.Dispose();
                _messageList = null;
                ResumeLayout();
            }
        }

        public void WriteLine(string message)
        {
            if (_messageList == null) return;
            if (_messageList.InvokeRequired)
            {
                Action<string> callback = WriteLine;
                Invoke(callback, message);
            }
            else
            {
                DateTime date = DateTime.Now;
                _messageList.Items.Add(string.Format("{0}: {1}", date, message));
                if (_messageList.Items.Count > MaxLines) _messageList.Items.RemoveAt(0);
                _messageList.SelectedIndex = _messageList.Items.Count - 1;
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Process.Start("explorer.exe");
        //}

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (BackgroundImage != null)
            {
                BackgroundImage.Dispose();
                BackgroundImage = null;
            }
        }
    }
}