using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TechnicalServices.Common.Notification
{
    /// <summary>
    /// поведение уведомителей - либо создается один инстанс для подписчиков
    /// либо по одному инстансу для каждого ключа
    /// </summary>
    public enum NotifierBehaviour
    {
        OneInstance,
        OneInstancePerKey
    }

    /// <summary>
    /// Manager службы уведомлений
    /// </summary>
    /// <typeparam name="TKey">тип объекта поведение которого мониторится</typeparam>
    public class NotificationManager<TKey> where TKey : class
    {
        private static readonly NotificationManager<TKey> _instance = new NotificationManager<TKey>();
        public static NotificationManager<TKey> Instance
        {
            get { return _instance; }
        }
        private NotificationManager(){}

        //private Dictionary<Type, object> _storeDic = new Dictionary<Type, object>();

        /// <summary>
        /// хранилище для уведомителей
        /// </summary>
        /// <typeparam name="TIdentity">идентификатор канала</typeparam>
        /// <typeparam name="TCallBack">интерфейс обратного вызова</typeparam>
        public class NotificationStore<TIdentity, TCallBack>
            where TCallBack : class
            where TIdentity : class, IEquatable<TIdentity>
        {
            private Timer _timer;
            private const int _minutes = 30;

            private void Cleaner(object state)
            {
                if (!Monitor.TryEnter(this)) return;
                try
                {
                    List<object> keys = new List<object>();
                    foreach (KeyValuePair<object, Notifier<TIdentity, TCallBack>> keyValuePair in _notifiers)
                    {
                        if (0 == keyValuePair.Value.GetSubscribersCount())
                        {
                            keys.Add(keyValuePair.Key);
                        }
                        else
                        {
                            keyValuePair.Value.Clean();
                        }
                    }
                    foreach (object o in keys)
                    {
                        _notifiers.Remove(o);
                    }
                }
                finally
                {
                    Monitor.Exit(this);
                }
            }

            private readonly NotifierBehaviour _behaviour;
            private readonly Dictionary<object, Notifier<TIdentity, TCallBack>> _notifiers = new Dictionary<object, Notifier<TIdentity, TCallBack>>();
            private Notifier<TIdentity, TCallBack> GetNotifier(TKey key)
            {
                lock (this)
                {
                    Notifier<TIdentity, TCallBack> notifier;
                    object o = GetKey(key);
                    if (!_notifiers.TryGetValue(o, out notifier))
                    {
                        notifier = new Notifier<TIdentity, TCallBack>();
                        _notifiers[o] = notifier;
                    }
                    return notifier;
                }
            }

            private object GetKey(TKey key)
            {
                switch (_behaviour)
                {
                    case NotifierBehaviour.OneInstance:
                        return key.GetType();
                    case NotifierBehaviour.OneInstancePerKey:
                        return key;
                    default:
                        throw new Exception(String.Format("Incorrect key {0}", key));
                }
            }

            internal NotificationStore(NotifierBehaviour behaviour)
            {
                _timer = new Timer(Cleaner, null, TimeSpan.FromMinutes(_minutes), TimeSpan.FromMinutes(_minutes));
                _behaviour = behaviour;
            }

            public void Subscribe(TKey key, TIdentity identity)
            {
                GetNotifier(key).Subscribe(identity);
            }

            public void Unsubscribe(TKey key, TIdentity identity)
            {
                GetNotifier(key).Unsubscribe(identity);
            }

            public void Notify(TIdentity sender, TKey key, string callBackMethod, params object[] parameters)
            {
                GetNotifier(key).Notify(sender, callBackMethod, parameters);
            }

            public void RefreshSubscribers(TIdentity identity)
            {
                foreach (KeyValuePair<object, Notifier<TIdentity, TCallBack>> pair in _notifiers)
                {
                    pair.Value.RefreshSubscribers(identity);
                }

            }
        }

        private static object _notificationStore = null;

        public NotificationStore<TIdentity,TCallBack> RegisterDuplexService<TIdentity, TCallBack>(NotifierBehaviour behaviour)
            where TIdentity : class, IEquatable<TIdentity>
            where TCallBack : class
        {
            lock (this)
            {
                if (_notificationStore == null)
                {
                    _notificationStore = new NotificationStore<TIdentity,TCallBack>(behaviour);
                    return
                        (NotificationStore<TIdentity,TCallBack>) _notificationStore;
                }
                else
                {
                    if (_notificationStore.GetType() != typeof(NotificationStore<TIdentity, TCallBack>))
                    {
                        throw new Exception(String.Format("NotificationManager.RegisterDuplexService: invalid type, should be type {0}", _notificationStore.GetType()));
                    }
                    return
                        (NotificationStore<TIdentity, TCallBack>)_notificationStore;
                }
            }
        }
    }
}