using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

using TechnicalServices.Common;
using TechnicalServices.Entity;

namespace TechnicalServices.Locking.Locking
{
    public class LockingStorage : AbstractStorage<ObjectKey, LockingInfoWithCommunicationObject>
    {
        public bool AcquireLock(ICommunicationObject communicationObject, UserIdentity user, ObjectKey objectKey, RequireLock requireLock)
        {
            SyncRoot.AcquireWriterLock(Timeout.Infinite);
            try
            {
                LockingInfoWithCommunicationObject item;
                if (Dictionary.TryGetValue(objectKey, out item) && item != null)
                {
                    if (user.User.Priority > item.LockingInfo.UserIdentity.User.Priority
                        //||
                        //(user.User.Priority >= item.UserIdentity.User.Priority && requireLock == RequireLock.ForShow)
                        )
                    {
                        RemoveItem(user, objectKey);
                        AddItem(user, objectKey, new LockingInfoWithCommunicationObject(new LockingInfo(user, requireLock, objectKey), communicationObject));
                        return true;
                    }
                    return false;
                }
                AddItem(user, objectKey, new LockingInfoWithCommunicationObject(new LockingInfo(user, requireLock, objectKey), communicationObject));
                return true;
            }
            finally
            {
                SyncRoot.ReleaseWriterLock();
            }
        }

        //public LockStatus GetLockStatus(ObjectKey objectKey, out UserIdentity owner)
        //{
        //    _sync.AcquireReaderLock(Timeout.Infinite);
        //    try
        //    {
        //        owner = null;
        //        LockingInfo item;
        //        if (_dic.TryGetValue(objectKey, out item) && item != null)
        //        {
        //            owner = item.UserIdentity;
        //            switch (item.RequireLock)
        //            {
        //                case RequireLock.ForEdit:
        //                    return LockStatus.LockedForEdit;
        //                case RequireLock.ForShow:
        //                    return LockStatus.LockedForShow;
        //                default:
        //                    throw new ArgumentOutOfRangeException("item.RequireLock",
        //                                                          String.Format(
        //                                                              "GetLockStatus: Неизвестный тип блокировки {0}",
        //                                                              item.RequireLock));
        //            }
        //        }
        //        return LockStatus.Deleted;
        //    }
        //    finally
        //    {
        //        _sync.ReleaseReaderLock();
        //    }
        //}

        public bool ReleaseLock(UserIdentity user, ObjectKey objectKey)
        {
            SyncRoot.AcquireWriterLock(Timeout.Infinite);
            try
            {
                LockingInfoWithCommunicationObject item;
                if (Dictionary.TryGetValue(objectKey, out item) && item != null)
                {
                    if (user.Equals(item.LockingInfo.UserIdentity)
                        || user.User.Priority > item.LockingInfo.UserIdentity.User.Priority)
                        return RemoveItem(user, objectKey);
                }
                return false;
            }
            finally
            {
                SyncRoot.ReleaseWriterLock();
            }
        }

        public LockingInfo GetLockInfo(ObjectKey objectKey)
        {
            LockingInfoWithCommunicationObject locking = this[objectKey];
            return locking == null ? null : locking.LockingInfo;
        }

        public void Clean(UserIdentity sender)
        {
            SyncRoot.AcquireWriterLock(Timeout.Infinite);
            try
            {
                List<ObjectKey> listToRemove = new List<ObjectKey>(Count);
                foreach (KeyValuePair<ObjectKey, LockingInfoWithCommunicationObject> pair in this)
                {
                    ICommunicationObject communicationObject = pair.Value.CommunicationObject;
                    if (communicationObject != null && communicationObject.State != CommunicationState.Opened)
                    {
                        listToRemove.Add(pair.Key);
                    }
                }
                foreach (ObjectKey objectKey in listToRemove)
                {
                    this.RemoveItem(sender, objectKey);
                }
            }
            finally
            {
                SyncRoot.ReleaseWriterLock();
            }

        }
    }
}