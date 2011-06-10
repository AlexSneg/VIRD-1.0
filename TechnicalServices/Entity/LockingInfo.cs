using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TechnicalServices.Entity;

namespace TechnicalServices.Entity
{
    [Serializable]
    public class LockingInfo : ObjectInfo
    {
        protected readonly RequireLock _requireLock;

        public LockingInfo(UserIdentity userIdentity, RequireLock requireLock, ObjectKey objectKey)
            : base(userIdentity, objectKey)
        {
            _requireLock = requireLock;
        }

        public RequireLock RequireLock
        {
            [DebuggerStepThrough]
            get { return _requireLock; }
        }
    }

    public class LockingInfoWithCommunicationObject
    {
        private readonly LockingInfo _lockingInfo;
        private readonly ICommunicationObject _communicationObject;

        public LockingInfoWithCommunicationObject(LockingInfo lockingInfo, ICommunicationObject communicationObject)
        {
            _lockingInfo = lockingInfo;
            _communicationObject = communicationObject;
        }

        public LockingInfo LockingInfo
        {
            get { return _lockingInfo; }
        }

        public ICommunicationObject CommunicationObject
        {
            get { return _communicationObject; }
        }
    }
}