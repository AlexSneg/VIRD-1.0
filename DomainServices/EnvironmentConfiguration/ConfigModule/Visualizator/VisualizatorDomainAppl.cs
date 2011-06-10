using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator
{
    public abstract class VisualizatorDomainAppl : VisualizatorDomain, IFormCreater
    {
        private readonly Thread _thread;
        protected readonly InvisibleMainForm _main;

        protected VisualizatorDomainAppl()
        {
            _thread = new Thread(WorkThread);
            if (!_thread.TrySetApartmentState(ApartmentState.STA))
                throw new Exception("Не удалось установить STA ApartmentState");
            _main = new InvisibleMainForm(this);
        }

        #region IFormCreater Members

        public abstract Form CreateForm(DisplayType display, Window window, out bool needProcessing);

        #endregion

        private void WorkThread()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.Run(_main);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string path = string.Format("{0}_Exception.txt", DateTime.Now.ToString("dd.MM.yyyy_hh_mm_ss"));
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine(string.Format("Sender {0} \n Exception {1}", sender, e.ExceptionObject));
            }
        }

        void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string path = string.Format("{0}_Exception.txt", DateTime.Now.ToString("dd.MM.yyyy_hh_mm_ss"));
            using(StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine(e.Exception.ToString());
            }
        }

        protected override bool OnInit()
        {
            _thread.Start();
            return true;
        }
        protected override void OnDone()
        {
            _main.CloseMainWindow();
            if (_thread != null && _thread.IsAlive)
                _thread.Join();
        }

        public override IntPtr ShowForm(DisplayType display, Window window)
        {
            return _main.ShowWindow(display, window);
        }

        public override void DestroyForm(IntPtr handle)
        {
            _main.HideWindow(handle);
        }

        public override void BringToFront(IntPtr handle)
        {
            _main.BringWindowToFront(handle);
        }

        public override void SendToBack(IntPtr handle)
        {
            _main.SendWindowToBack(handle);
        }

        public override string DoCommand(IntPtr handle, string command)
        {
            return _main.DoCommand(handle, command);
        }

        public override bool IsAlive(IntPtr handle)
        {
            return _main.IsAlive(handle);
        }
    }
}