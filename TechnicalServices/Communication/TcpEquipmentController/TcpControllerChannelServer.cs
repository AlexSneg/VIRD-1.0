#define SOCK

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using TechnicalServices.Common.Utils;
using TechnicalServices.Communication.EquipmentController;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Licensing;

namespace TechnicalServices.Communication.TcpEquipmentController
{
    public class TcpControllerChannelServer : IControllerChannel
    {
        private const int ListenerTimeout = 5 * 60 * 1000;
        private readonly IEventLogging _logging;
        private readonly CommandDescriptor _checkCommand;
        private readonly int _checkTimeout;
        private readonly Uri _connectionString;
        private readonly Encoding _encoding;
        private readonly ManualResetEvent _exit;
        private readonly byte[] _feedback = new byte[16384];
        private readonly int _receiveTimeout;
        private readonly CommandDescriptor _securityCommand;
        private readonly object _sync = new object();
        private readonly ReaderWriterLockSlim _stateSync = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private readonly Thread _thread;
        private readonly StreamWriter _writer = new StreamWriter("crestron.log");
#if SOCK
        private Socket _client;
#else
        private TcpClient _client;
#endif

        /// <summary>
        /// статус устройств при опросе - битовая маска
        /// </summary>
        private byte[] _currentState;

        /// <summary>
        /// если недоступен то 0, если доступен то 1
        /// </summary>
        private int _isControllerOnLine;

        public TcpControllerChannelServer(IEventLogging logging, Uri connectionString, int receiveTimeout, int checkTimeout)
        {
            Debug.Assert(connectionString.Scheme == "tcp");
            Debug.Assert(connectionString.Port != 0);

            _logging = logging;
            _currentState = new byte[] { 1 };
            _connectionString = connectionString;
            _receiveTimeout = receiveTimeout;
            _checkTimeout = checkTimeout;
            _encoding = Encoding.Default;
            _checkCommand = new CommandDescriptor(Constants.ControllerUID, "Check");
            _securityCommand = new CommandDescriptor(Constants.ControllerUID, "SecurityControllerCheck", String.Empty);
            _exit = new ManualResetEvent(false);
            _thread = new Thread(ThreadProc);
            _thread.Start();
        }

        #region IControllerChannel Members

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public bool IsControllerOnLine
        {
            get { return _isControllerOnLine == 1; }
            private set { Interlocked.Exchange(ref _isControllerOnLine, value ? 1 : 0); }
        }

        public bool IsOnLine(int uid)
        {
            if (uid < 0 || !IsControllerOnLine) return false;
            if (uid == Constants.ControllerUID) return IsControllerOnLine;

            _stateSync.EnterReadLock();
            try
            {
                if (_currentState.Length <= uid) return false;
                return Convert.ToBoolean(_currentState[uid]);
            }
            finally
            {
                _stateSync.ExitReadLock();
            }
        }

        public event EventHandler<DeviceCheckResultEventArgs> OnCheck;
        public event EventHandler<DeviceStatusChangeEventArgs> OnStatusChange;

        public string Send(CommandDescriptor cmd)
        {
            lock (_sync)
            {
                if (_client == null || !IsControllerOnLine || !IsOnLine(cmd.EquipmentId)) return null;
                try
                {
                    string commandText = cmd.FormatToString();
                    string message = String.Format(CommandDescriptorExtenstion.PacketFormat, commandText);
                    byte[] data = _encoding.GetBytes(message);
                    WriteToLog("Send:" + message);
#if SOCK
                    _client.Send(data);
                    int count = _client.Receive(_feedback);
#else
                    _client.Client.Send(data);
                    int count = _client.Client.Receive(_feedback);
#endif
                    CheckFeedbackLength(count);
                    message = _encoding.GetString(_feedback, 0, count);
                    WriteToLog("Recieve:" + message);
                    if (String.IsNullOrEmpty((message))) throw new SocketException();
                    if (!message.EndsWith(CommandDescriptorExtenstion.PacketEndMarker))
                        throw new ChannelException(ChannelError.ErrorEndMarker);
                    message = message.Substring(0, message.Length - CommandDescriptorExtenstion.PacketEndMarker.Length);

                    string uid = cmd.EquipmentId.ToString();
                    if (!message.StartsWith(uid)) throw new ChannelException(ChannelError.WrongUID);
                    return message.Remove(0, uid.Length).Trim();
                }
                catch (SocketException ex)
                {
                    WriteToLog(string.Format("Exception: {0}, ErrorCode: {1}",ex, ex.ErrorCode));
                    //SetEquipmentOffLine(cmd.EquipmentId);
                    _client.Close();
                    _client = null;
                    IsControllerOnLine = false;
                    //SetEquipmentOffLine(cmd.EquipmentId);
                    return null;
                }
            }
        }

        public void Dispose()
        {
            _exit.Set();
            _thread.Join();
            _writer.Dispose();
        }

        #endregion

        private void SetEquipmentOffLine(int uid)
        {
            _stateSync.EnterWriteLock();
            try
            {
                if (_currentState.Length > uid)
                {
                    _currentState[uid] = Convert.ToByte(false);
                }
                StateChange(uid, false);
            }
            finally
            {
                _stateSync.ExitWriteLock();
            }
        }

        private void Check(string result)
        {
            string[] items = result.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length != 2) return;
            if (String.Compare(items[0], "OK", true, CultureInfo.InvariantCulture) != 0) return;

            // Добавляем бит для самого контроллера, 
            // если мы находимся тут,
            // то полюбому контроллер работает
            result = "1," + items[1];
            items = result.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] _checkList = Array.ConvertAll(items, value => Byte.Parse(value));
            _stateSync.EnterUpgradeableReadLock();
            try
            {
                if (!_currentState.SequenceEqual(_checkList))
                {
                    for (int i = 0; i < _checkList.Length && i < _currentState.Length; i++)
                    {
                        if (_checkList[i] != _currentState[i])
                            StateChange(i, Convert.ToBoolean(_checkList[i]));
                    }
                    _stateSync.EnterWriteLock();
                    try
                    {
                        _currentState = _checkList;
                    }
                    finally
                    {
                        _stateSync.ExitWriteLock();
                    }
                    if (OnCheck != null) OnCheck(this, new DeviceCheckResultEventArgs(_currentState));
                }
            }
            finally
            {
                _stateSync.ExitUpgradeableReadLock();
            }
        }

        private void SecurityCheck()
        {
            byte[] data = new byte[1000];
            Random rnd = new Random((int)DateTime.Now.Ticks);
            LicenseChecker checker = new LicenseChecker();
            StringBuilder seedStorage = new StringBuilder(40);
            seedStorage.Length = rnd.Next(seedStorage.Capacity - 1) + 1;
            for (int i = 0; i < seedStorage.Length; i++)
                seedStorage[i] = (char)((byte)'A' + (byte)rnd.Next(25));
            string seed = seedStorage.ToString();
            string command = String.Format("0 SecurityControllerCheck({0})\r", seed);
            try
            {
                lock (_sync)
                {
                    if (_client != null)
                    {
                        for (int i = 0; i < command.Length; i++)
                            data[i] = (byte)command[i];
                        _client.Send(data, 0, command.Length, SocketFlags.None);
                        command = String.Empty;
                        int count = _client.Receive(data);
                        for (int i = 0; i < count; i++)
                            command += (char)data[i];

                        // в какой то момент после не ответа крестрона ломается порядок прихолдящих команд - соответсвенно на запрос секьюрити может придти ответ от совсем другой команды - здесь обязательно надо проверять!!!!
                        if (!command.ToLower().Contains("securitycontrollerresponse")) return;
                        command = command.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries)[1];
                        command = DecodeHashedValue(command);
                        checker.CheckCrestronID(command, seed, DecodeHashedValue);
                    }
                    //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1855
                    //В ситуации с защищенной версиией, При запуске Плеера с любым сценарием, через минуты 2 ядро отваливается с сообщением о том что ID Crestron в ключе не соответствует прошивке Crestron
                    // это лишний вызов из-за него получалась такая ситуация
                    //else
                    //    checker.CheckUnavailableCrestron();
                }
            }
            catch (HaspException ex)
            {
                _exit.Set();
                throw ex;
            }
            catch (LicenseInvalidException ex)
            {
                _exit.Set();
                throw ex;
            }
            catch (Exception ex)
            {
            }
        }

        private static string DecodeHashedValue(string value)
        {
            value = value.Replace((char)0xA1, (char)0x00);
            value = value.Replace((char)0xA2, (char)0x0a);
            value = value.Replace((char)0xA4, (char)0x0d);
            value = value.Replace((char)0xAC, (char)0x22);
            value = value.Replace((char)0xAF, (char)0x28);
            value = value.Replace((char)0xB4, (char)0x29);
            value = value.Replace((char)0X9C, (char)0x2C);
            return value;
        }

        private void SecurityCheckUnavailableCrestron()
        {
            try
            {
                LicenseChecker checker = new LicenseChecker();
                checker.CheckUnavailableCrestron();
            }
            catch (HaspException ex)
            {
                _exit.Set();
                throw ex;
            }
            catch (LicenseInvalidException ex)
            {
                _exit.Set();
                throw ex;
            }
            catch (Exception ex)
            {
            }
        }

        private void StateChange(int uid, bool isOnLine)
        {
            try
            {
                if (OnStatusChange != null)
                {
                    OnStatusChange(this, new DeviceStatusChangeEventArgs(uid, isOnLine));
                }
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format("StateChange:\n {0}", ex));
            }
        }

        private void ClearControllerState()
        {
            IsControllerOnLine = false;
            StateChange(Constants.ControllerUID, false);
            _stateSync.EnterWriteLock();
            //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1855
            // надо не сбрасывать а обнулять!!!!
            //_currentState = new byte[0];
            _currentState = new byte[_currentState.Length];
            _stateSync.ExitWriteLock();
        }

        private void ThreadProc()
        {
            do
            {
                ClearControllerState();
                try
                {
#if SOCK
                    Socket listner = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress hostIP = (Dns.Resolve(IPAddress.Any.ToString())).AddressList[0];
                    IPEndPoint ep = new IPEndPoint(hostIP, _connectionString.Port);
                    listner.Bind(ep);
                    listner.Listen(1);
                    IAsyncResult aResult = listner.BeginAccept(null, null);
#else                    
                    TcpListener listner = new TcpListener(IPAddress.Any, _connectionString.Port);
                    listner.Start(1);
                    IAsyncResult aResult = listner.BeginAcceptTcpClient(null, null);
#endif
                    int eventIndex;
                    do
                    {
                        eventIndex = WaitHandle.WaitAny(new[] { _exit, aResult.AsyncWaitHandle }, ListenerTimeout);
                        if (eventIndex == WaitHandle.WaitTimeout)
                            WatchDog.WatchDogAction(_logging.WriteError, SecurityCheckUnavailableCrestron);
                    } while (eventIndex == WaitHandle.WaitTimeout);

                    if (eventIndex == 0) break;
#if SOCK
                    lock (_sync) _client = listner.EndAccept(aResult);
                    aResult.AsyncWaitHandle.Close();
                    listner.Close();
#else                    
                    lock (_sync) _client = listner.EndAcceptTcpClient(aResult);
                    aResult.AsyncWaitHandle.Close();
                    listner.Stop();
#endif


                    IsControllerOnLine = true;
                    // Из-за этого параметра (KeepAlive) валилось с ошибкой 
                    // "A connection attempt failed because the connected party 
                    // did not properly respond after a period of time, or established 
                    // connection failed because connected host has failed to respond"
                    //_client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);

#if SOCK
                    _client.ReceiveTimeout = _receiveTimeout;
#else
                    _client.Client.ReceiveTimeout = _receiveTimeout;
#endif
                    do
                    {
                        WatchDog.WatchDogAction(_logging.WriteError, SecurityCheck);
                        string result = Send(_checkCommand);
                        if (result == null) break;
                        Check(result);
                    } while (!_exit.WaitOne(_checkTimeout));
                }
                catch (Exception ex)
                {
                    WriteToLog("Exception:" + ex);
                }
                try
                {
                    lock (_sync)
                    {
                        if (_client != null)
                        {
                            _client.Close();
                            _client = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteToLog("Exception:" + ex);
                }
            } while (!_exit.WaitOne(0));

            //lock (_sync)
            //{
            //    if (_client != null) _client.Close();
            //}
        }

        [Conditional("DEBUG")]
        private void CheckFeedbackLength(int count)
        {
            if (count == _feedback.Length) new ApplicationException("feedback overflow");
            if (count < CommandDescriptorExtenstion.PacketEndMarker.Length)
                new ApplicationException("feedback overflow");
        }

        private void WriteToLog(string message)
        {
            message = String.Format("{0}:{1}", DateTime.Now.TimeOfDay, message);
            Debug.WriteLine(message);
            _writer.WriteLine(message);
            _writer.Flush();
        }
    }
}