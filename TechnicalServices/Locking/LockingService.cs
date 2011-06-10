using System;
using System.ServiceModel;
using System.Threading;
using System.Timers;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using Timer = System.Timers.Timer;

namespace TechnicalServices.Locking.Locking
{
    public class LockingService : ILockService
    {
        private readonly LockingStorage _lockingStorage;
        private readonly UserIdentity _systemUser;
        private Timer _timer = null;
        private readonly IEventLogging _log;
        private const int _interval = 60000;
        //private readonly LockingNotifier _notifier = new LockingNotifier();

        public LockingService(UserIdentity systemUser, IEventLogging log)
        {
            _systemUser = systemUser;
            _log = log;
            _timer = new Timer(_interval);
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Start();
            _lockingStorage = new LockingStorage();
            _lockingStorage.OnAddItem += _lockingStorage_OnAddItem;
            _lockingStorage.OnRemoveItem += _lockingStorage_OnRemoveItem;
            //_lockingStorage.OnAddItem += new TechnicalServices.Common.StorageAction<ObjectKey, LockingInfo>(_notifier.SubscribeForMonitor);
            //_lockingStorage.OnRemoveItem +=new TechnicalServices.Common.StorageAction<ObjectKey,LockingInfo>(_notifier.UnSubscribeForMonitor);
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Monitor.TryEnter(this))
            {
                try
                {
                    _lockingStorage.Clean(_systemUser);
                }
                catch (Exception ex)
                {
                    _log.WriteError(string.Format("LockingService._timer_Elapsed\n{0}", ex));
                }
                finally
                {
                    Monitor.Exit(this);
                }
            }
        }

        //public IPresentationDAL PresentationDAL{get; set;}

        #region ILockService Members

        public bool AcquireLock(ICommunicationObject communicationObject, UserIdentity user, ObjectKey objectKey, RequireLock requireLock)
        {
            return _lockingStorage.AcquireLock(communicationObject, user, objectKey, requireLock);
        }

        //public LockStatus GetLockStatus(ObjectKey objectKey, out UserIdentity owner)
        //{
        //    return _lockingStorage.GetLockStatus(objectKey, out owner);
        //}

        public bool ReleaseLock(UserIdentity user, ObjectKey objectKey)
        {
            return _lockingStorage.ReleaseLock(user, objectKey);
        }

        public LockingInfo GetLockInfo(ObjectKey objectKey)
        {
            return _lockingStorage.GetLockInfo(objectKey);
        }

        public event Action<UserIdentity, ObjectKey, LockingInfo> AddItem;
        public event Action<UserIdentity, ObjectKey, LockingInfo> RemoveItem;

        #endregion

        private void _lockingStorage_OnRemoveItem(UserIdentity sender, ObjectKey key, LockingInfoWithCommunicationObject value)
        {
            if (RemoveItem != null)
            {
                RemoveItem(sender, key, value.LockingInfo);
            }
        }

        private void _lockingStorage_OnAddItem(UserIdentity sender, ObjectKey key, LockingInfoWithCommunicationObject value)
        {
            if (AddItem != null)
            {
                AddItem(sender, key, value.LockingInfo);
            }
        }
    }
}