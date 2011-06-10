using System;
using System.Windows.Forms;

namespace TechnicalServices.Util
{
    public static class AsyncFormCaller
    {
        public static void AsyncInvoke(this Control control, Action handler)
        {
            control.Invoke(handler);
        }

        public static void AsyncInvoke<T>(this Control control, Action<T> handler, T arg)
        {
            control.Invoke(handler, arg);
        }

        public static void AsyncInvoke<T1, T2>(this Control control, Action<T1, T2> handler, T1 arg1, T2 arg2)
        {
            control.Invoke(handler, arg1, arg2);
        }
    }
}