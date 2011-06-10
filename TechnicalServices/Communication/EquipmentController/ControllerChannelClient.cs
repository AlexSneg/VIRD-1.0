using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace TechnicalServices.Communication.EquipmentController
{
    public abstract class ControllerChannelClient : IControllerChannel
    {
        private const int MaxErrCount = 5;
        private const int MaxTimeOut = 1000;

        private const string CommandFormat = "{0} {1}({2})";
        private const string ParameterDelimiter = ",";
        private const string PacketEndMarker = "\r";
        private const string PacketFormat = "{0}" + PacketEndMarker;

        private static readonly NumberFormatInfo numberFormat = new NumberFormatInfo();
        private static readonly StringBuilder param = new StringBuilder(1024);

        private readonly Queue<CommandDescriptor> _queue = new Queue<CommandDescriptor>(182);
        private readonly Uri _connectionString;
        private readonly Encoding _encoding;
        private readonly byte[] feedback = new byte[16384];
        private Stream _stream;

        protected ControllerChannelClient(Uri connectionString, Encoding encoding)
        {
            _connectionString = connectionString;
            _encoding = encoding;
        }

        #region IControllerChannel Members

        public void Start()
        {
            try
            {
                _stream = OpenStream(_connectionString);
            }
            catch (Exception)
            {
                if (!Reconnect()) 
                    throw new ChannelException(ChannelError.ConnectionBroken);
            }
        }

        private void SendText(CommandDescriptor cmd)
        {
            string commandText = FormatToString(cmd);
            string message = String.Format(PacketFormat, commandText);
            byte[] body = _encoding.GetBytes(message);
            _stream.Write(body, 0, body.Length);
        }

        private string RecieveText()
        {
            int count = _stream.Read(feedback, 0, feedback.Length);
            CheckFeedbackLength(count);
            string message = _encoding.GetString(feedback, 0, count);
            if (!message.EndsWith(PacketEndMarker))
                throw new ChannelException(ChannelError.ErrorEndMarker);
            return message.Substring(0, message.Length - PacketEndMarker.Length);
        }

        public void ClearQueue()
        {
            _queue.Clear();
        }

        public string Send(CommandDescriptor cmd)
        {
            try
            {
                SendText(cmd);
            }
            catch (IOException)
            {
                if (!Reconnect()) 
                    throw new ChannelException(ChannelError.ConnectionBroken);
            }
            try
            {
                return RecieveText();
            }
            catch (IOException)
            {
                throw new ChannelException(ChannelError.ConnectionBroken);
            }
        }

        public void Stop()
        {
            if (_stream != null) CloseStream(_stream);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Stop();
        }

        #endregion

        protected virtual bool Reconnect()
        {
            int errCount = MaxErrCount;
            do
            {
                CloseStream(_stream);
                try
                {
                    _stream = OpenStream(_connectionString);
                    return true;
                }
                catch (Exception)
                {
                    errCount--;
                    Thread.Sleep(MaxTimeOut);
                }
            }
            while (errCount > 0);

            return false;
        }

        private static string FormatToString(CommandDescriptor cmd)
        {
            lock (typeof(CommandDescriptor))
            {
                param.Length = 0;
                foreach (IConvertible item in cmd.Parameters)
                {
                    param.Append(ParameterDelimiter);
                    switch (item.GetTypeCode())
                    {
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            param.Append(item.ToString(numberFormat));
                            break;
                        case TypeCode.String:
                            param.Append('"' + item.ToString().Replace("\"", "\"\"") + '"');
                            break;
                        default:
                            throw new ApplicationException("Не поддерживаемый тип в параметре команды");
                    }
                }
                if (param.Length > 0) param.Remove(0, ParameterDelimiter.Length);
                return String.Format(CommandFormat, cmd.EquipmentId, cmd.CommandName, param);
            }
        }

        [Conditional("DEBUG")]
        private void CheckFeedbackLength(int count)
        {
            if (count == feedback.Length) new ApplicationException("feedback overflow");
            if (count < PacketEndMarker.Length) new ApplicationException("feedback overflow");
        }

        protected abstract Stream OpenStream(Uri ConnectionString);
        protected abstract void CloseStream(Stream stream);
    }
}