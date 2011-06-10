using System;
using System.Windows.Forms;

namespace TechnicalServices.Util
{
    public static class AsyncFormCallerResult
    {
        public static R AsyncInvokeResult<R>(this Control control, Func<R> handler)
        {
            return (R) control.Invoke(handler);
        }

        public static R AsyncInvokeResult<T, R>(this Control control, Func<T, R> handler, T arg)
        {
            return (R) control.Invoke(handler, arg);
        }

        public static R AsyncInvokeResult<T1, T2, R>(this Control control, Func<T1, T2, R> handler, T1 arg1, T2 arg2)
        {
            return (R) control.Invoke(handler, arg1, arg2);
        }
    }
}