using System;

namespace TechnicalServices.Util
{
    public static class AsyncCaller
    {
        public static IAsyncResult BeginCall<T>(Action<T> handler, T arg)
        {
            return handler.BeginInvoke(arg, null, handler);
        }

        public static IAsyncResult BeginCall<T1, T2>(Action<T1, T2> handler, T1 arg1, T2 arg2)
        {
            return handler.BeginInvoke(arg1, arg2, null, handler);
        }

        public static IAsyncResult BeginCall<T1, T2, T3>(Action<T1, T2, T3> handler, T1 arg1, T2 arg2, T3 arg3)
        {
            return handler.BeginInvoke(arg1, arg2, arg3, null, handler);
        }

        public static IAsyncResult BeginCall<T1, T2, T3, T4>(Action<T1, T2, T3, T4> handler, T1 arg1, T2 arg2, T3 arg3,
                                                             T4 arg4)
        {
            return handler.BeginInvoke(arg1, arg2, arg3, arg4, null, handler);
        }

        public static void EndCall<T>(IAsyncResult result)
        {
            Action<T> handler = (Action<T>) result.AsyncState;
            result.AsyncWaitHandle.WaitOne();
            handler.EndInvoke(result);
            result.AsyncWaitHandle.Close();
        }

        public static void EndCall<T1, T2>(IAsyncResult result)
        {
            Action<T1, T2> handler = (Action<T1, T2>)result.AsyncState;
            result.AsyncWaitHandle.WaitOne();
            handler.EndInvoke(result);
            result.AsyncWaitHandle.Close();
        }

        public static void EndCall<T1, T2, T3>(IAsyncResult result)
        {
            Action<T1, T2, T3> handler = (Action<T1, T2, T3>)result.AsyncState;
            result.AsyncWaitHandle.WaitOne();
            handler.EndInvoke(result);
            result.AsyncWaitHandle.Close();
        }

        public static void EndCall<T1, T2, T3, T4>(IAsyncResult result)
        {
            Action<T1, T2, T3, T4> handler = (Action<T1, T2, T3, T4>)result.AsyncState;
            result.AsyncWaitHandle.WaitOne();
            handler.EndInvoke(result);
            result.AsyncWaitHandle.Close();
        }
    }
}