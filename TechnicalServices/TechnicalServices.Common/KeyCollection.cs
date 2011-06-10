using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace TechnicalServices.Common
{
    public abstract class KeyCollection<TKey> : IEnumerable<TKey>
        where TKey : class, IEquatable<TKey>
    {
        private readonly HashSet<TKey> _dict = new HashSet<TKey>();
        private readonly ReaderWriterLock _sync = new ReaderWriterLock();

        protected HashSet<TKey> Dictionary
        {
            get { return _dict; }
        }

        protected ReaderWriterLock SyncRoot
        {
            get { return _sync; }
        }

        public int Count
        {
            get { return Dictionary.Count; }
        }

        #region Implementation of IEnumerable

        public IEnumerator<TKey> GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public event EventHandler<KeyEventArgs> OnAddItem;
        public event EventHandler<KeyEventArgs> OnRemoveItem;

        protected virtual void NotifyRemove(TKey key)
        {
            if (OnRemoveItem != null)
            {
                OnRemoveItem(this, new KeyEventArgs(key));
            }
        }

        protected virtual void NotifyAdd(TKey key)
        {
            if (OnAddItem != null)
            {
                OnAddItem(this, new KeyEventArgs(key));
            }
        }

        public bool Add(TKey key)
        {
            SyncRoot.AcquireWriterLock(Timeout.Infinite);
            try
            {
                return AddItem(key);
            }
            finally
            {
                SyncRoot.ReleaseWriterLock();
            }
        }

        protected virtual bool AddItem(TKey key)
        {
            bool result = Dictionary.Add(key);
            if (result) NotifyAdd(key);
            return result;
        }

        public bool Remove(TKey key)
        {
            SyncRoot.AcquireWriterLock(Timeout.Infinite);
            try
            {
                return RemoveItem(key);
            }
            finally
            {
                SyncRoot.ReleaseWriterLock();
            }
        }

        protected virtual bool RemoveItem(TKey key)
        {
            bool result = Dictionary.Remove(key);
            if (result) NotifyRemove(key);
            return result;
        }

        public bool ContainsKey(TKey key)
        {
            SyncRoot.AcquireReaderLock(Timeout.Infinite);
            try
            {
                return Dictionary.Contains(key);
            }
            finally
            {
                SyncRoot.ReleaseReaderLock();
            }
        }

        #region Nested type: KeyEventArgs

        public class KeyEventArgs : EventArgs
        {
            private readonly TKey _key;

            public KeyEventArgs(TKey key)
            {
                _key = key;
            }

            public TKey Key
            {
                get { return _key; }
            }
        }

        #endregion
    }
}