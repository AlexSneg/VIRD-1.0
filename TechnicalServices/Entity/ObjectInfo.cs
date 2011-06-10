using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TechnicalServices.Entity
{
    [Serializable]
    public class ObjectInfo
    {
        protected readonly ObjectKey _objectKey;
        protected readonly UserIdentity _userIdentity;

        public ObjectKey ObjectKey
        {
            [DebuggerStepThrough]
            get { return _objectKey; }
        }

        public UserIdentity UserIdentity
        {
            [DebuggerStepThrough]
            get { return _userIdentity; }
        }

        public ObjectInfo(UserIdentity userIdentity, ObjectKey objectKey)
        {
            _objectKey = objectKey;
            _userIdentity = userIdentity;
        }
    }
}
