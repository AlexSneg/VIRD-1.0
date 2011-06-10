using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Tools;
using System.Windows.Forms;
using System.Diagnostics;
using Syncfusion.Windows.Forms.Tools.Win32API;
using System.Reflection;

namespace UI.PresentationDesign.DesignUI.Helpers
{
    public class RibbonControlExt: RibbonControlAdv
    {
        private CallWndProcHook my_callWndProcHook;
        MethodInfo OnActivateMethod;
        MethodInfo OnWmMdiActivateMethod;
        MethodInfo OnWmSysCommandMethod;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            //dispose old hook, cauze we should hack std wndProc
            BindingFlags bf = BindingFlags.Default;
            bf |= BindingFlags.NonPublic;
            bf |= BindingFlags.Instance;

            FieldInfo fi = typeof(RibbonControlAdv).GetField("m_callWndProcHook", bf);
            IDisposable d = fi.GetValue(this) as IDisposable;
            d.Dispose();

            my_callWndProcHook = new CallWndProcHook(new WindowsAPI.WindowProc(CallWndProc));

            OnActivateMethod = typeof(RibbonControlAdv).GetMethod("OnActivate", bf);
            OnWmMdiActivateMethod = typeof(RibbonControlAdv).GetMethod("OnWmMdiActivate", bf);
            OnWmSysCommandMethod = typeof(RibbonControlAdv).GetMethod("OnWmSysCommand", bf);

        }

        IntPtr CallWndProc(IntPtr hWnd, int nMsg, IntPtr wParam, IntPtr lParam)
        {
            switch (((Msg)nMsg))
            {
                case Msg.WM_SYSCOMMAND:
                    this.OnWmSysCommand(wParam);
                    break;

                case Msg.WM_MDIACTIVATE:
                    this.OnWmMdiActivate(hWnd, wParam, lParam);
                    break;

                case Msg.WM_ACTIVATE:
                    this.OnActivate(hWnd, wParam, lParam);
                    break;

                case Msg.WM_CONTEXTMENU:
                    {
                        //do nothing. 
                        //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-156
                        //this.OnWmContextMenu(hWnd, lParam);
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void OnActivate(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            OnActivateMethod.Invoke(this, new object [] {hWnd, wParam, lParam});
        }

        private void OnWmMdiActivate(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            OnWmMdiActivateMethod.Invoke(this, new object[] { hWnd, wParam, lParam });
        }

        private void OnWmSysCommand(IntPtr wParam)
        {
            OnWmSysCommandMethod.Invoke(this, new object[] { wParam });
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            my_callWndProcHook.Dispose();
        }

    }
}
