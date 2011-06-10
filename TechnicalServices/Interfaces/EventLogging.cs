using System;
using System.Diagnostics;

namespace TechnicalServices.Interfaces
{
    public abstract class EventLogging : IEventLogging, IDisposable
    {
        #region IEventLogging

        public abstract void WriteLine(EventLogEntryType type, string message);

        public void WriteInformation(string message)
        {
            WriteLine(EventLogEntryType.Information, message);
        }

        public void WriteWarning(string message)
        {
            WriteLine(EventLogEntryType.Warning, message);
        }

        public void WriteError(string message)
        {
            WriteLine(EventLogEntryType.Error, message);
        }
        #endregion

        #region IDisposable

        public abstract void Dispose();
        
        #endregion
    }
}