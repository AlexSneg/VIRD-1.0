using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TechnicalServices.Interfaces;
using ThreadState = System.Threading.ThreadState;

namespace TechnicalServices.Communication.TcpExternalSystemController
{
    public class TcpExternalSystemServer : IExternalSystemCommand
    {
        private const int ReceiveTimeout = 1000;

        private readonly Uri _connectionString;
        private readonly Encoding _encoding;
        private ManualResetEvent _exit;
        private readonly Thread _thread;
        private readonly IEventLogging _log;
        private AutoResetEvent _tcpClientConnected = null;

        #region command

        private const string _gotoLabelCommandPrefix = "label";
        private const string _gotoSlideCommandPrefix = "id";
        private const string _gotoNextSlideCommandPrefix = "callnext";
        private const string _gotoPrevSlideCommandPrefix = "callprev";

        private const char _delimeter = ':';

        private const string _errorResponse = "ERROR";
        private const string _okResponse = "OK";

        #endregion

        public TcpExternalSystemServer(Uri connectionString, IEventLogging log)
        {
            Debug.Assert(connectionString.Scheme == "tcp");
            Debug.Assert(connectionString.Port != 0);

            _log = log;
            _encoding = Encoding.Default;
            _connectionString = connectionString;
            _exit = new ManualResetEvent(false);
            _thread = new Thread(ThreadProc);
            _thread.Start();

        }

        private void ThreadProc()
        {
            TcpListener listner = null;
            try
            {
                listner = new TcpListener(IPAddress.Any, _connectionString.Port);
                listner.Start(1);
                Thread clientThread = null;
                do
                {
                    try
                    {
                        //_tcpClientConnected.Reset();
                        IAsyncResult aResult = listner.BeginAcceptTcpClient(null, null);

                        int eventIndex = WaitHandle.WaitAny(new[] { _exit, aResult.AsyncWaitHandle });
                        if (eventIndex == 0) break;

                        //using (TcpClient client = listner.EndAcceptTcpClient(aResult))
                        //{
                        //    aResult.AsyncWaitHandle.Close();
                        //    ReadInLoop(client);
                        //}
                        TcpClient client = listner.EndAcceptTcpClient(aResult);
                        aResult.AsyncWaitHandle.Close();
                        if (_tcpClientConnected != null)
                            _tcpClientConnected.Set();
                        else
                            _tcpClientConnected = new AutoResetEvent(false);
                        if (clientThread != null && clientThread.ThreadState == ThreadState.Running)
                            clientThread.Join();
                        clientThread = new Thread(ReadInLoop) { IsBackground = true };
                        clientThread.Start(client);

                        //_client = listner.EndAcceptTcpClient(aResult);
                        //aResult.AsyncWaitHandle.Close();
                        //listner.Stop();

                        //_client.Client.ReceiveTimeout = ReceiveTimeout;
                        //ReadInLoop();
                    }
                    catch (Exception ex)
                    {
                        _log.WriteError(string.Format("TcpExternalSystemServer: {0}", ex));
                        Thread.Sleep(5000);
                    }
                } while (!_exit.WaitOne(0));
                listner.Stop();
            }
            catch (Exception ex)
            {
                _log.WriteError(string.Format("TcpExternalSystemServer.ThreadProc:\n{0}",ex));
            }
            finally
            {
                listner = null;
            }
        }


        //private void ReadInLoop(TcpClient client)
        //{
        //        byte[] buffer = new byte[client.ReceiveBufferSize];
        //        using (NetworkStream reader = client.GetStream())
        //        {
        //            //do
        //            //{
        //                StringBuilder result = new StringBuilder();
        //                while (reader.DataAvailable)
        //                {
        //                    int size = reader.Read(buffer, 0, buffer.Length);
        //                    result.AppendFormat("{0}", Encoding.ASCII.GetString(buffer, 0, size));
        //                }
        //                if (result.Length != 0)
        //                {
        //                    ProccessMessage(result.ToString());
        //                }
        //                Thread.Sleep(0);
        //            //} while (WaitHandle.WaitAny(new WaitHandle[] { _exit, _tcpClientConnected }, 0) == WaitHandle.WaitTimeout);        // !_exit.WaitOne(5000));
        //        }
        //}


        private void ReadInLoop(object obj)
        {
            try
            {
                using (TcpClient client = obj as TcpClient)
                {
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    using (NetworkStream reader = client.GetStream())
                    {
                        do
                        {
                            StringBuilder result = new StringBuilder();
                            while (reader.DataAvailable)
                            {
                                int size = reader.Read(buffer, 0, buffer.Length);
                                result.AppendFormat("{0}", _encoding.GetString(buffer, 0, size));
                            }
                            if (result.Length != 0)
                            {
                                bool response = ProccessMessage(result.ToString());
                                byte[] responseArr = _encoding.GetBytes(response ? _okResponse : _errorResponse);
                                reader.Write(responseArr, 0, responseArr.Length);
                            }
                            Thread.Sleep(0);
                        } while (WaitHandle.WaitAny(new WaitHandle[] { _exit, _tcpClientConnected }, 1000) == WaitHandle.WaitTimeout);        // !_exit.WaitOne(5000));
                    }
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(string.Format(
                    "TcpExternalSystemServer.ReadInLoop: {0}", ex));
            }
        }

        private bool ProccessMessage(string message)
        {
            lock (this)
            {
                try
                {
                    _log.WriteInformation(string.Format("TcpExternalSystemServer: Получена воманда от внешней системы: {0}",
                        message));
                    if (message.StartsWith(_gotoLabelCommandPrefix,StringComparison.InvariantCultureIgnoreCase)
                        && OnGoToLabel != null)
                    {
                        string[] parts = message.Split(_delimeter);
                        return OnGoToLabel.Invoke(parts[1].Trim());
                    }
                    else if (message.StartsWith(_gotoSlideCommandPrefix,StringComparison.InvariantCultureIgnoreCase)
                        && OnGoToSlideById != null)
                    {
                        string[] parts = message.Split(_delimeter);
                        return OnGoToSlideById.Invoke(parts[1].Trim());
                    }
                    else if (message.StartsWith(_gotoNextSlideCommandPrefix, StringComparison.InvariantCultureIgnoreCase)
                        && OnGoToNextSlide != null)
                    {
                        return OnGoToNextSlide.Invoke();
                    }
                    else if (message.StartsWith(_gotoPrevSlideCommandPrefix, StringComparison.InvariantCultureIgnoreCase)
                        && OnGoToPrevSlide != null)
                    {
                        return OnGoToPrevSlide.Invoke();
                    }
                    else
                    {
                        _log.WriteWarning(string.Format("Команда {0} не распознана", message));
                    }
                }
                catch (Exception ex)
                {
                    _log.WriteError(string.Format("TcpExternalSystemServer: {0}", ex));
                }
                return false;
            }
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _exit.Set();
            _thread.Join();
            _exit.Close();
            _exit = null;
            if (_tcpClientConnected != null)
            {
                _tcpClientConnected.Close();
                _tcpClientConnected = null;
            }
        }

        #endregion

        #region Implementation of IExternalSystemCommand


        #endregion

        #region Implementation of IExternalSystemCommand

        public event Func<string, bool> OnGoToLabel;
        public event Func<string, bool> OnGoToSlideById;
        public event Func<bool> OnGoToNextSlide;
        public event Func<bool> OnGoToPrevSlide;

        #endregion
    }
}