using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using TechnicalServices.Entity;

namespace TechnicalServices.Common
{
    public abstract class AbstractStorage<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : class, IEquatable<TKey>
        where TValue : class
    {
        private readonly Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>(10);
        private readonly ReaderWriterLock _sync = new ReaderWriterLock();

        protected Dictionary<TKey, TValue> Dictionary
        {
            get { return _dict; }
        }

        protected ReaderWriterLock SyncRoot
        {
            get { return _sync; }
        }

        public TValue this[TKey key]
        {
            get
            {
                SyncRoot.AcquireReaderLock(Timeout.Infinite);
                try
                {
                    return GetItem(key);
                }
                finally
                {
                    SyncRoot.ReleaseReaderLock();
                }
            }
        }

        public ICollection<TKey> KeyCollection
        {
            get { return Dictionary.Keys; }
        }

        public ICollection<TValue> ValueCollection
        {
            get { return Dictionary.Values; }
        }

        public int Count
        {
            get { return Dictionary.Count; }
        }

        #region Implementation of IEnumerable

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public event Action<UserIdentity, TKey, TValue> OnAddItem;
        public event Action<UserIdentity, TKey, TValue> OnRemoveItem;

        protected virtual void NotifyRemove(UserIdentity sender, TKey key, TValue value)
        {
            if (OnRemoveItem != null)
            {
                OnRemoveItem(sender, key, value);
            }
        }

        protected virtual void NotifyAdd(UserIdentity sender, TKey key, TValue value)
        {
            if (OnAddItem != null)
            {
                OnAddItem(sender, key, value);
            }
        }

        public KeyValuePair<TKey, TValue> Add(UserIdentity sender, TKey key, TValue value)
        {
            SyncRoot.AcquireWriterLock(Timeout.Infinite);
            try
            {
                return AddItem(sender, key, value);
            }
            finally
            {
                SyncRoot.ReleaseWriterLock();
            }
        }

        protected virtual KeyValuePair<TKey, TValue> AddItem(UserIdentity sender, TKey key, TValue value)
        {
            KeyValuePair<TKey, TValue> pair = new KeyValuePair<TKey, TValue>(key, value);
            Dictionary[key] = value;
            NotifyAdd(sender, key, value);
            return pair;
        }

        public bool Remove(UserIdentity sender, TKey key)
        {
            SyncRoot.AcquireWriterLock(Timeout.Infinite);
            try
            {
                return RemoveItem(sender, key);
            }
            finally
            {
                SyncRoot.ReleaseWriterLock();
            }
        }

        protected virtual bool RemoveItem(UserIdentity sender, TKey key)
        {
            TValue value;
            bool isSuccess = false;
            if (Dictionary.TryGetValue(key, out value))
            {
                isSuccess = Dictionary.Remove(key);
                if (isSuccess)
                    NotifyRemove(sender, key, value);
            }
            return isSuccess;
        }

        protected TValue GetItem(TKey key)
        {
            TValue value;
            if (Dictionary.TryGetValue(key, out value))
                return value;
            return null;
        }


        public bool ContainsKey(TKey key)
        {
            SyncRoot.AcquireReaderLock(Timeout.Infinite);
            try
            {
                return Dictionary.ContainsKey(key);
            }
            finally
            {
                SyncRoot.ReleaseReaderLock();
            }
        }

        //public bool TryGetValue(TKey key, out TValue value)
        //{
        //    lock (_sync)
        //    {
        //        return _dic.TryGetValue(key, out value);
        //    }
        //}

        //protected void MonitorEnter()
        //{
        //    Monitor.Enter(_sync);
        //}

        //protected void MonitorExit()
        //{
        //    Monitor.Exit(_sync);
        //}
    }
}