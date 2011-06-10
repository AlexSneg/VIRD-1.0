using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hosts.Plugins.Video.UI
{
    internal class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        private IntPtr _hwnd;
    }
}
