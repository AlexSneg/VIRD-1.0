using System;
using System.Diagnostics;

namespace TechnicalServices.Interfaces
{
    public interface IEventLogging
    {
        void WriteLine(EventLogEntryType type, string message);
        void WriteInformation(string message);
        void WriteWarning(string message);
        void WriteError(string message);
    }
}