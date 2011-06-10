using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace TechnicalServices.Communication.Communication.Internal
{
    internal abstract class AbstractNotifier<TKey, TValue, TCallBack>
        where TKey : class, IEquatable<TKey>
        where TCallBack : class
    {
        protected readonly Dictionary<TKey, TCallBack> _subscriberDic = new Dictionary<TKey, TCallBack>();

        internal virtual void Subscribe(TKey key, TValue value)
        {
            lock(this)
            {
                Notify(key, value, NotifyStatus.Subscribe);
                _subscriberDic[key] = OperationContext.Current.GetCallbackChannel<TCallBack>();
            }
        }

        internal virtual void UnSubscribe(TKey key, TValue value)
        {
            lock(this)
            {
                _subscriberDic.Remove(key);
                Notify(key, value, NotifyStatus.Unsubscribe);
            }
        }

        protected abstract void Notify(TKey key, TValue value, NotifyStatus notifyStatus);
    }
}