using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace TechnicalServices.Common.Utils
{
    public class WatchDog
    {
        private const string HandleName = "WatchDogLincense";

        [Conditional("Security_Release")]
        public static void WatchDogHandler(ServiceBase service, Action<string> errorHandler, WaitHandle exit)
        {
            EventWaitHandle _watchDog = new EventWaitHandle(false, EventResetMode.AutoReset, HandleName);
            int index = WaitHandle.WaitAny(new[] {_watchDog, exit}, Timeout.Infinite);
            if (index == 0)
            {
                errorHandler("Нарушение лицензии");
                service.Stop();
            }
        }

        [Conditional("Security_Release")]
        public static void WatchDogAction(Action<string> errorHandler, Action handler)
        {
            try
            {
                handler();
            }
            catch (Exception ex)
            {
                errorHandler(ex.Message);
                EventWaitHandle.OpenExisting(HandleName).Set();
            }
        }

        [Conditional("Security_Release")]
        public static void WatchDogAction<T>(Action<string> errorHandler, Action<T> handler, T param)
        {
            try
            {
                handler(param);
            }
            catch (Exception ex)
            {
                errorHandler(ex.Message);
                EventWaitHandle.OpenExisting(HandleName).Set();
            }
        }

    }
}