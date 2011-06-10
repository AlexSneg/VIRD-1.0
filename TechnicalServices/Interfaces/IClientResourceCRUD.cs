using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Interfaces
{
    public class OperationStatusEventArgs<T> : EventArgs
    {
        private readonly FileSaveStatus _status;
        private readonly T _resource;
        private readonly string _otherResourceId;

        public OperationStatusEventArgs(FileSaveStatus status, string otherResourceId, T resource)
        {
            _status = status;
            _resource = resource;
            _otherResourceId = otherResourceId;
        }
        public FileSaveStatus Status
        {
            get { return _status; }
        }
        public T Resource
        {
            get { return _resource; }
        }

        public string OtherResourceId
        {
            get { return _otherResourceId; }
        }
    }

    public class PartSendEventArgs : EventArgs
    {
        private readonly int _part;
        private readonly int _numberOfParts;
        private readonly string _displayName;

        public PartSendEventArgs(int part, int numberOfParts, string displayName)
        {
            _part = part;
            _numberOfParts = numberOfParts;
            _displayName = displayName;
        }

        /// <summary>
        /// номер части
        /// </summary>
        public int Part { get { return _part; } }

        /// <summary>
        /// всего частей
        /// </summary>
        public int NumberOfParts { get { return _numberOfParts; } }

        /// <summary>
        /// Имя дисплея
        /// </summary>
        public string DisplayName { get { return _displayName; } }

    }

    public interface IClientResourceCRUD<T>
    {
        FileSaveStatus CreateSource(T resource, out string otherResourceId);
        FileSaveStatus SaveSource(T resource, out string otherResourceId);
        bool GetSource(T resource, bool autoCommit);
        event EventHandler<PartSendEventArgs> OnPartTransmit;
        event EventHandler OnTerminate;
        event EventHandler<OperationStatusEventArgs<T>> OnComplete;
        event Action<double, string> OnUploadSpeed;
        void Commit();
        void RollBack();
        void Terminate();
    }

}
