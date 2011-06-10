using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TechnicalServices.Common.Locking;
using TechnicalServices.Entity;

namespace TechnicalServices.Locking.Locking
{
    [Serializable]
    public class LockingInfo
    {
        private readonly ObjectKey _objectKey;
        private readonly UserIdentity _user;
        private readonly RequireLock _requireLock;

        protected internal LockingInfo(UserIdentity user, RequireLock requireLock, ObjectKey objectKey)
        {
            _user = user;
            _requireLock = requireLock;
            _objectKey = objectKey;
        }

        public UserIdentity UserIdentity
        {
            [DebuggerStepThrough]
            get { return _user; }
        }

        public RequireLock RequireLock
        {
            [DebuggerStepThrough]
            get { return _requireLock; }
        }

        public ObjectKey ObjectKey
        {
            [DebuggerStepThrough]
            get { return _objectKey; }
        }
    }
}
