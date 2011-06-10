using System;

namespace TechnicalServices.Util
{
    public static class AsyncCallerResult
    {
        public static IAsyncResult BeginCall<R>(Func<R> handler)
        {
            return handler.BeginInvoke(null, handler);
        }

        public static IAsyncResult BeginCall<T, R>(Func<T, R> handler, T arg)
        {
            return handler.BeginInvoke(arg, null, handler);
        }

        public static IAsyncResult BeginCall<T1, T2, R>(Func<T1, T2, R> handler, T1 arg1, T2 arg2)
        {
            return handler.BeginInvoke(arg1, arg2, null, handler);
        }

        public static IAsyncResult BeginCall<T1, T2, T3, R>(Func<T1, T2, T3, R> handler, T1 arg1, T2 arg2, T3 arg3)
        {
            return handler.BeginInvoke(arg1, arg2, arg3, null, handler);
        }

        public static IAsyncResult BeginCall<T1, T2, T3, T4, R>(Func<T1, T2, T3, T4, R> handler, T1 arg1, T2 arg2,
                                                                T3 arg3,
                                                                T4 arg4)
        {
            return handler.BeginInvoke(arg1, arg2, arg3, arg4, null, handler);
        }

        public static R EndCall<R>(IAsyncResult result)
        {
            Func<R> handler = (Func<R>) result.AsyncState;
            result.AsyncWaitHandle.WaitOne();
            R ret = handler.EndInvoke(result);
            result.AsyncWaitHandle.Close();
            return ret;
        }

        public static R EndCall<T, R>(IAsyncResult result)
        {
            Func<T, R> handler = (Func<T, R>) result.AsyncState;
            result.AsyncWaitHandle.WaitOne();
            R ret = handler.EndInvoke(result);
            result.AsyncWaitHandle.Close();
            return ret;
        }

        public static R EndCall<T1, T2, R>(IAsyncResult result)
        {
            Func<T1, T2, R> handler = (Func<T1, T2, R>) result.AsyncState;
            result.AsyncWaitHandle.WaitOne();
            R ret = handler.EndInvoke(result);
            result.AsyncWaitHandle.Close();
            return ret;
        }

        public static R EndCall<T1, T2, T3, R>(IAsyncResult result)
        {
            Func<T1, T2, T3, R> handler = (Func<T1, T2, T3, R>) result.AsyncState;
            result.AsyncWaitHandle.WaitOne();
            R ret = handler.EndInvoke(result);
            result.AsyncWaitHandle.Close();
            return ret;
        }

        public static R EndCall<T1, T2, T3, T4, R>(IAsyncResult result)
        {
            Func<T1, T2, T3, T4, R> handler = (Func<T1, T2, T3, T4, R>) result.AsyncState;
            result.AsyncWaitHandle.WaitOne();
            R ret = handler.EndInvoke(result);
            result.AsyncWaitHandle.Close();
            return ret;
        }
    }
}