using System.Diagnostics;

using TechnicalServices.Interfaces;

namespace TechnicalServices.Logging.StubLogging
{
    public class StubLogging : EventLogging
    {
        #region IEventLogging

        public override void WriteLine(EventLogEntryType type, string message)
        {
        }

        #endregion

        #region IDisposable

        public override void Dispose()
        {
        }

        #endregion
    }
}