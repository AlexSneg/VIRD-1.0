using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using UI.Common.CommonUI.Forms;
using System.Diagnostics;

namespace UI.PresentationDesign.DesignUI.Helpers
{
    public static class MessageBoxExt
    {
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, string[] buttonText, bool useCancelButton)
        {
            //int n = Application.OpenForms.Count - 1;
            //Form owner = null;
            //if (n > -1)
            //    owner = Application.OpenForms[n];

            //if (owner != null)
            //    owner.BringToFront();

            IntPtr zero = IntPtr.Zero;
            if (owner == null)
            {
                zero = Process.GetCurrentProcess().MainWindowHandle; 
                //zero = NativeMethod.GetActiveWindow();
                owner = Form.FromHandle(zero);
            }
            else
            {
                zero = owner.Handle;
            }

            DialogResult result;
            using (MessageBoxForm frm = new MessageBoxForm())
            {
                if (!useCancelButton)
                {
                    frm.ControlBox = false;
                    frm.CancelButton = new Button(); // Иначе по Esc закрывается форма.
                }
                result = frm.ShowForm(owner, text, caption, buttons, icon, buttonText, useCancelButton);
            }

            //if (zero != IntPtr.Zero)
            //{
            //    NativeMethod.SendMessage(zero, 7, 0, 0);
            //}

            return result;
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, string[] buttonText)
        {
            return MessageBoxExt.Show(null, text, caption, buttons, icon, buttonText);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBoxExt.Show(text, caption, buttons, icon, new string[] { });
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, bool useCancelButton)
        {
            return MessageBoxExt.Show(null, text, caption, buttons, icon, new string[] { }, useCancelButton);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, string[] buttonText)
        {
            return MessageBoxExt.Show(owner, text, caption, buttons, icon, buttonText, true);
        }

    }
}
