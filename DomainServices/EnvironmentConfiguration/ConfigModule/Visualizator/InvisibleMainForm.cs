using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Util;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator
{
    /// <summary>
    /// Есть подозрение что все вызовы выполняемые в UI потоке надо обязательно перехватывать Exception - иначе приложение может зависнуть!!!
    /// </summary>
    public class InvisibleMainForm : Form
    {
        private readonly Dictionary<IntPtr, Form> _list = new Dictionary<IntPtr, Form>();
        private readonly IFormCreater _creator;

        internal InvisibleMainForm(IFormCreater creator)
        {
            InitializeComponent();
            _creator = creator;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            ControlBox = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
        }

        private void InvisibleMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (KeyValuePair<IntPtr, Form> item in _list)
            {
                item.Value.FormClosed -= ViewFormClosed;
                item.Value.Close();
                item.Value.Dispose();
            }
        }


        private void ViewFormClosed(object sender, EventArgs e)
        {
            _list.Remove(((Form) sender).Handle);
        }


        public IntPtr ShowWindow(DisplayType display, Window window)
        {
            if (InvokeRequired)
            {
                return this.AsyncInvokeResult<DisplayType, Window, IntPtr>(ShowWindow, display, window);
            }
            else
            {
                try
                {
                    bool needProcessing;
                    Form form = _creator.CreateForm(display, window, out needProcessing);
                    if (form == null) return IntPtr.Zero;
                    form.FormClosed += ViewFormClosed;
                    form.Top = window.Top;
                    form.Left = window.Left;
                    form.Width = window.Width;
                    form.Height = window.Height;
                    form.TopMost = true;
                    if (window is ActiveWindow && needProcessing)
                    {
                        ActiveWindow wnd = (ActiveWindow) window;
                        form.Text = wnd.TitleText;
                        form.FormBorderStyle = (wnd.BorderVisible) ? FormBorderStyle.FixedSingle : FormBorderStyle;
                    }
                    form.Show( /*this*/);
                    _list.Add(form.Handle, form);
                    return form.Handle;
                }
                catch(Exception ex)
                {
                    WriteErrorToFile(ex);
                    return IntPtr.Zero;
                }
            }
        }

        public void BringWindowToFront(IntPtr handle)
        {
            if (InvokeRequired)
            {
                this.AsyncInvoke(BringWindowToFront, handle);
            }
            else
            {
                try
                {
                    if (handle == IntPtr.Zero) return;
                    Control control = FromHandle(handle);
                    Form form = control as Form;
                    if (form == null) return;
                    form.BringToFront();
                }
                catch(Exception ex)
                {
                    WriteErrorToFile(ex);
                }
            }
        }

        public void SendWindowToBack(IntPtr handle)
        {
            if (InvokeRequired)
            {
                this.AsyncInvoke(SendWindowToBack, handle);
            }
            else
            {
                try
                {
                    if (handle == IntPtr.Zero) return;
                    Control control = FromHandle(handle);
                    Form form = control as Form;
                    if (form == null) return;
                    form.SendToBack();
                }
                catch(Exception ex)
                {
                    WriteErrorToFile(ex);
                }
            }
        }


        public void HideWindow(IntPtr handle)
        {
            if (InvokeRequired)
            {
                this.AsyncInvoke(HideWindow, handle);
            }
            else
            {
                try
                {
                    if (handle == IntPtr.Zero) return;
                    Control control = FromHandle(handle);
                    Form form = control as Form;
                    if (form == null) return;
                    form.Close();
                    form.Dispose();
                }
                catch(Exception ex)
                {
                    WriteErrorToFile(ex);
                }
            }
        }

        public bool IsAlive(IntPtr handle)
        {
            return _list.ContainsKey(handle);
        }

        public void CloseMainWindow()
        {
            if (InvokeRequired)
            {
                this.AsyncInvoke(CloseMainWindow);
            }
            else
            {
                try
                {
                    Close();
                }
                catch(Exception ex)
                {
                    WriteErrorToFile(ex);
                }
            }
        }

        public string DoCommand(IntPtr handle, string command)
        {
            if (InvokeRequired)
            {
                return this.AsyncInvokeResult<IntPtr, string, string>(DoCommand, handle, command);
            }
            else
            {
                try
                {
                    if (handle == IntPtr.Zero) return null;
                    Control control = FromHandle(handle);
                    IDoCommand form = control as IDoCommand;
                    if (form == null) return null;
                    return form.DoCommand(command);
                }
                catch(Exception ex)
                {
                    WriteErrorToFile(ex);
                    return null;
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // InvisibleMainForm
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "InvisibleMainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InvisibleMainForm_FormClosing);
            this.ResumeLayout(false);

        }

        private void WriteErrorToFile(Exception ex)
        {
            try
            {
                string path = string.Format("{0}_Exception.txt", DateTime.Now.ToString("dd.MM.yyyy_hh_mm_ss"));
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.WriteLine(string.Format("Exception {0}", ex));
                }
            }
            catch
            {}
        }
    }
}