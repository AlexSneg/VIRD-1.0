using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Threading;

namespace TechnicalServices.Common.Notification
{
    /// <summary>
    /// уведомитель
    /// </summary>
    /// <typeparam name="TIdentity">идентификатор канала</typeparam>
    /// <typeparam name="TCallBack">интерфейс обратного вызова</typeparam>
    public class Notifier<TIdentity, TCallBack>
        where TCallBack : class
        where TIdentity : class, IEquatable<TIdentity>
    {
        private readonly SubscriberStore<TIdentity, TCallBack> store = new SubscriberStore<TIdentity, TCallBack>();

        public void Subscribe(TIdentity identity)
        {
            if (OperationContext.Current == null) return;
            TCallBack callback = OperationContext.Current.GetCallbackChannel<TCallBack>();
            if (callback != null && identity != null)
                store.Add(identity, callback);
        }

        public void Unsubscribe(TIdentity identity)
        {
            if (identity != null)
                store.Remove(identity);
        }

        public int GetSubscribersCount()
        {
            return store.GetSubscriberCount();
        }

        /// <summary>
        /// уведомить подписчиков о событии
        /// </summary>
        /// <param name="sender">Инициатор события</param>
        /// <param name="method">имя метода, который будет использоваться для уведомления</param>
        /// <param name="parameters">параметры, которые должны передаться этому методу</param>
        public void Notify(TIdentity sender, string method, params object[] parameters)
        {
            //store.Notify(sender, method, parameters);
            store.BeginNotify(sender, method, parameters);
        }

        public void RefreshSubscribers(TIdentity identity)
        {
            if (OperationContext.Current == null) return;
            TCallBack callback = OperationContext.Current.GetCallbackChannel<TCallBack>();
            if (callback != null)
            {
                if (identity != null) store.RefreshSubscribers(identity, callback);
            }
        }

        internal void Clean()
        {
            store.Clean();
        }

        #region Nested type: SubscriberStore

        private class SubscriberStore<TSubscriberKey, TValue> : IEnumerable<KeyValuePair<TSubscriberKey, TValue>>
            where TSubscriberKey : class, TIdentity
            where TValue : TCallBack
        {
            private readonly Action<TSubscriberKey, string, object[]> _asyncNotify;

            private readonly Dictionary<TSubscriberKey, TValue> _subscriberDic =
                new Dictionary<TSubscriberKey, TValue>();

            //private readonly Dictionary<TSubscriberKey, int> _subscriberCounter = new Dictionary<TSubscriberKey, int>();
            private readonly ReaderWriterLock _sync = new ReaderWriterLock();

            public SubscriberStore()
            {
                _asyncNotify = Notify;
            }

            public void Add(TSubscriberKey subscriber, TValue callback)
            {
                _sync.AcquireWriterLock(Timeout.Infinite);
                try
                {
                    _subscriberDic[subscriber] = callback;
                    //int count = 0;
                    //_subscriberCounter.TryGetValue(subscriber, out count);
                    //_subscriberCounter[subscriber] = ++count;
                }
                finally
                {
                    _sync.ReleaseWriterLock();
                }
            }

            internal void Clean()
            {
                _sync.AcquireWriterLock(Timeout.Infinite);
                try
                {
                    List<TSubscriberKey> subscriberKeys = new List<TSubscriberKey>();
                    foreach (KeyValuePair<TSubscriberKey, TValue> pair in _subscriberDic)
                    {
                        ICommunicationObject communicationObject = pair.Value as ICommunicationObject;
                        if (communicationObject != null &&
                            (communicationObject.State == CommunicationState.Closed ||
                             communicationObject.State == CommunicationState.Faulted))
                        {
                            subscriberKeys.Add(pair.Key);
                        }
                    }
                    foreach (TSubscriberKey subscriberKey in subscriberKeys)
                    {
                        Remove(subscriberKey);
                    }
                }
                finally
                {
                    _sync.ReleaseWriterLock();
                }
            }

            public void Remove(TSubscriberKey subscriber)
            {
                _sync.AcquireWriterLock(Timeout.Infinite);
                try
                {
                    _subscriberDic.Remove(subscriber);
                    //int count = 0;
                    //_subscriberCounter.TryGetValue(subscriber, out count);
                    //if (count <= 1)
                    //{
                    //    _subscriberDic.Remove(subscriber);
                    //    _subscriberCounter.Remove(subscriber);
                    //}
                    //else
                    //{
                    //    _subscriberCounter[subscriber] = --count;
                    //}
                }
                finally
                {
                    _sync.ReleaseWriterLock();
                }
            }

            public int GetSubscriberCount()
            {
                _sync.AcquireReaderLock(Timeout.Infinite);
                try
                {
                    return _subscriberDic.Count;
                }
                finally
                {
                    _sync.ReleaseReaderLock();
                }
            }

            public void Notify(TSubscriberKey sender, string method, params object[] parameters)
            {
                //Thread.Sleep(5000);
                _sync.AcquireWriterLock(Timeout.Infinite);
                try
                {
                    List<TSubscriberKey> subscribersToRemove = new List<TSubscriberKey>();
                    foreach (KeyValuePair<TSubscriberKey, TValue> keyValuePair in _subscriberDic)
                    {
                        if (sender != null && sender.Equals(keyValuePair.Key)) continue;
                        try
                        {
                            MethodInfo info;
                            if (parameters == null || parameters.Length == 0)
                                info = keyValuePair.Value.GetType().GetMethod(method);
                            else
                            {
                                List<Type> types = new List<Type>(parameters.Length);
                                foreach (object o in parameters)
                                {
                                    types.Add(o.GetType());
                                }
                                info = keyValuePair.Value.GetType().GetMethod(method, types.ToArray());
                            }
                            info.Invoke(keyValuePair.Value, parameters);
                        }
                        catch (Exception /*ex*/)
                        {
                            //SystemEventLogging.Instance.WriteError(
                            //    String.Format("Notifier.Notify: Subscriber {0}, Error {1}",
                            //                  keyValuePair.Key, ex));
                            subscribersToRemove.Add(keyValuePair.Key);
                        }
                    }
                    foreach (TSubscriberKey subscriberKey in subscribersToRemove)
                    {
                        _subscriberDic.Remove(subscriberKey);
                        //_subscriberCounter.Remove(subscriberKey);
                    }
                }
                finally
                {
                    _sync.ReleaseWriterLock();
                }
            }

            public IAsyncResult BeginNotify(TSubscriberKey sender, string method, params object[] parameters)
            {
                return _asyncNotify.BeginInvoke(sender, method, parameters, null, null);
            }

            public void EndNotify(IAsyncResult result)
            {
                _asyncNotify.EndInvoke(result);
            }

            public void RefreshSubscribers(TSubscriberKey subscriber, TValue callback)
            {
                _sync.AcquireWriterLock(Timeout.Infinite);
                try
                {
                    if (_subscriberDic.ContainsKey(subscriber))
                        _subscriberDic[subscriber] = callback;
                }
                finally
                {
                    _sync.ReleaseWriterLock();
                }
            }

            #region Implementation of IEnumerable

            public IEnumerator<KeyValuePair<TSubscriberKey, TValue>> GetEnumerator()
            {
                return _subscriberDic.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion
    }
}