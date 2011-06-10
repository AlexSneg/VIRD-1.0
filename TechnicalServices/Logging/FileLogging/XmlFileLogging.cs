using System;
using System.Diagnostics;
using System.IO;

using TechnicalServices.Interfaces;

namespace TechnicalServices.Logging.FileLogging
{
    public class XmlFileLogging : EventLogging
    {
        private const string TraceName = "trace.xml";

        private readonly XmlWriterTraceListener _trace;

        public XmlFileLogging()
        {
            // Удаляем старый лог файл, а то он быстро растет
            // и при большом размере замедляет работу
            if (File.Exists(TraceName))
            {
                try
                {
                    File.Delete(TraceName);
                }
                catch (Exception)
                {
                }
            }
            _trace = new XmlWriterTraceListener(TraceName);
            Trace.Listeners.Add(_trace);
        }

        #region IEventLogging

        public override void WriteLine(EventLogEntryType type, string message)
        {
            lock (_trace)
            {
                _trace.WriteLine(message, type.ToString());
            }
        }

        #endregion

        #region IDisposable

        public override void Dispose()
        {
            Trace.Listeners.Remove(_trace);
            _trace.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}