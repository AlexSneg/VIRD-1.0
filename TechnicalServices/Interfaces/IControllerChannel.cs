using System;
using System.Collections.Generic;

namespace TechnicalServices.Interfaces
{
    public class DeviceCheckResultEventArgs : EventArgs
    {
        private readonly byte[] _result;
        public DeviceCheckResultEventArgs(byte[] result)
        {
            _result = result;
        }

        public byte[] Result
        {
            get { return _result; }
        }
    }

    public class DeviceStatusChangeEventArgs : EventArgs
    {
        public DeviceStatusChangeEventArgs(int uid, bool isOnLine)
        {
            UID = uid;
            IsOnLine = isOnLine;
        }

        public int UID { get; private set; }
        public bool IsOnLine { get; private set; }
    }

    public interface IControllerChannel : IDisposable
    {
        void Start();
        string Send(CommandDescriptor cmd);
        void Stop();
        bool IsControllerOnLine { get; }
        bool IsOnLine(int uid);
        event EventHandler<DeviceCheckResultEventArgs> OnCheck;
        event EventHandler<DeviceStatusChangeEventArgs> OnStatusChange;
    }
}