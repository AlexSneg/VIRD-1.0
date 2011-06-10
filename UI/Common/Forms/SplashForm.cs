using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using System.Runtime.InteropServices;

namespace UI.PresentationDesign.DesignUI.Forms
{
    public partial class SplashForm : Office2007Form, IDisposable
    {
        public event EventHandler OnCancel;

        Form Owner = null;

        public int Progress
        {
            get
            {
                return this.progressBar1.Value;
            }
            set
            {
                this.InvokeUpdate(new MethodInvoker(() =>
                   {
                       this.progressBar1.Value = value;
                       this.Refresh();
                   }));
            }
        }

        public string Status
        {
            get
            {
                return this.label1.Text;
            }
            set
            {

                this.InvokeUpdate(new MethodInvoker(() =>
                    {
                        label1.Text = value;
                        this.Refresh();
                    }));
            }
        }

        void InvokeUpdate(Delegate d)
        {
            if (this.IsHandleCreated)
                this.Invoke(d);
            else
                d.Method.Invoke(d.Target, null);
        }

        private SplashForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(SplashForm_FormClosing);
        }

        bool _canClose = false;

        void SplashForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !_canClose;
        }


        public void HideForm()
        {
            if(Owner!=null)
                WinAPI.EnableWindow(Owner.Handle, true);

            _canClose = true;
            Close();
        }

        public static SplashForm CreateAndShowForm(bool useOwner, bool allowCancel)
        {
            SplashForm result = new SplashForm();
            if (useOwner)
            {
                int n = Application.OpenForms.Count - 1;
                Form owner = Application.OpenForms[n];
                owner.Invoke(new MethodInvoker(() =>
                {
                    result.Owner = owner;
                    WinAPI.EnableWindow(owner.Handle, false);
                    result.Show(owner);
                }));
            }
            else
                result.Show();

            result.buttonAdv1.Enabled = allowCancel;
            return result;
        }

        private void buttonAdv1_Click(object sender, EventArgs e)
        {
            if (MessageBoxAdv.Show(this, "Отменить процесс?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _canClose = true;

                if (OnCancel != null)
                    OnCancel(this, EventArgs.Empty);
            }
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            _canClose = true;
            OnCancel = null;
            base.Dispose();
        }

        #endregion
    }

    static class WinAPI
    {
        [DllImport("user32.dll")]
        internal static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
    }
}
