using System;
using System.Diagnostics;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Logging.SystemEventLogging
{
    public class SystemEventLogging : EventLogging
    {
        protected internal readonly EventLog _eventLog;

        public SystemEventLogging()
        {
            _eventLog = new EventLog();
            _eventLog.BeginInit();
            _eventLog.Log = "ВИРД";
            _eventLog.Source = "ВИРД";
            _eventLog.EndInit();
        }

        #region IEventLogging

        public override void WriteLine(EventLogEntryType type, string message)
        {
            lock (_eventLog)
            {
                Debug.Assert(EventLog.SourceExists(_eventLog.Source),
                             @"EventLog не зарегестрирован, запустите: \trunk\TechnicalServices\Logging\SystemEventLogging\install.cmd");
                EventLog.WriteEntry(_eventLog.Source, message, type);
            }
        }

        #endregion

        #region IDisposable

        public override void Dispose()
        {
            _eventLog.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}