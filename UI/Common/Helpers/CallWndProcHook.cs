using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Tools.Win32API;
using System.Runtime.InteropServices;

namespace UI.PresentationDesign.DesignUI.Helpers
{
    public class CallWndProcHook : IDisposable
    {
        protected IntPtr m_hHook;
        protected WindowsAPI.HookProc m_hookProc;
        protected WindowsAPI.WindowProc m_wndProc;

        public CallWndProcHook(WindowsAPI.WindowProc wndProc)
        {
            this.m_wndProc = wndProc;
            this.m_hookProc = new WindowsAPI.HookProc(this.CallWndProc);

            if (m_wndProc != null)
            {
                m_hHook = WindowsAPI.SetWindowsHookEx(4, m_hookProc, IntPtr.Zero, WindowsAPI.GetCurrentThreadId());
            }
        }

        private IntPtr CallWndProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            CallWndProcInternal(nCode, wParam, lParam);
            return WindowsAPI.CallNextHookEx(this.m_hHook, nCode, wParam, lParam);
        }

        protected void CallWndProcInternal(int nCode, IntPtr wParam, IntPtr lParam)
        {
            CWPSTRUCT cwpstruct = (CWPSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPSTRUCT));
            m_wndProc(cwpstruct.hwnd, cwpstruct.message, cwpstruct.wparam, cwpstruct.lparam);
        }

        public void Dispose()
        {
            if (this.m_hHook != IntPtr.Zero)
            {
                WindowsAPI.UnhookWindowsHookEx(this.m_hHook);
            }
        }
    }
}
