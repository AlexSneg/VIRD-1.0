using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Hosts.Plugins.Video.UI
{
    [SuppressUnmanagedCodeSecurity]
    internal class UnsafeNativeMethods
    {
        public const int LAYOUT_BITMAPORIENTATIONPRESERVED = 8;
        public const int LAYOUT_RTL = 1;
        public const int MB_PRECOMPOSED = 1;
        public const int SMTO_ABORTIFHUNG = 2;
        private static readonly Version VistaOSVersion;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetActiveWindow();
    }
}
