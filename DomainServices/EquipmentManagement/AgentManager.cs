using System;
using System.Threading;
using System.Windows.Forms;
using DomainServices.EquipmentManagement.AgentCommon;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

using UI.Agent.AgentUI;
using UI.Common.CommonUI.Helpers;

namespace DomainServices.EquipmentManagement.AgentManagement
{
    public class AgentManager : IAgentManager, IDisposable
    {
        private readonly AutoResetEvent _flag;
        private readonly IEventLogging _logging;
        private readonly Thread _proc;
        private MainForm _form;
        private readonly IBackgroundProvider _backgroundProvider;

        public AgentManager(IEventLogging logging)
        {
            _logging = logging;
            _flag = new AutoResetEvent(false);
            _proc = new Thread(ThreadProc);
            _backgroundProvider = new BackgroundProvider();
            _proc.Start(_flag);
            _flag.WaitOne(-1);
        }

        #region IAgentManager Members

        public void SetBackgroudImage(string imageFileName)
        {
            _form.SetBackgroundImage(imageFileName);
        }

        public bool Wait(int tick)
        {
            return _flag.WaitOne(tick);
        }

        public bool Wait()
        {
            return Wait(-1);
        }

        public IWin32Window MainWindow
        {
            get { return _form; }
        }

        public void OpenMessageView()
        {
            _form.OpenMessageView();
        }

        public void CloseMessageView()
        {
            _form.CloseMessageView();
        }

        public void WriteLine(string message)
        {
            _form.WriteLine(message);
        }

        #endregion

        public void Dispose()
        {
            Application.Exit();
            _proc.Join();
            _backgroundProvider.Dispose();
        }

        private void ThreadProc(object flag)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (_form = new MainForm((EventWaitHandle)flag, _backgroundProvider))
                Application.Run(_form);
        }

        public bool Check(DisplayType display)
        {
            return Screen.PrimaryScreen.Bounds.Width == display.Width &&
                   Screen.PrimaryScreen.Bounds.Height == display.Height;
        }
    }
}