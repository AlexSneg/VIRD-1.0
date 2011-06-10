using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Entity;

namespace TechnicalServices.Common.Caching
{
    public class Cache<T> where T : class
    {
        private readonly Dictionary<ObjectKey, T> _cacheDic = new Dictionary<ObjectKey, T>();
        private readonly Dictionary<ObjectKey, Timer> _timerDic = new Dictionary<ObjectKey, Timer>();
        private readonly ReaderWriterLock _sync = new ReaderWriterLock();

        private readonly TimeSpan _cachingTime;

        public Cache(TimeSpan cachingTime)
        {
            _cachingTime = cachingTime;
        }

        public void Add(ObjectKey objectKey, T t)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                _cacheDic[objectKey] = t;
                ChangeTimer(objectKey);
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public void Delete(ObjectKey objectKey)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                _cacheDic.Remove(objectKey);
                Timer timer;
                if (_timerDic.TryGetValue(objectKey, out timer))
                {
                    _timerDic.Remove(objectKey);
                    timer.Dispose();
                }
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public T Read(ObjectKey objectKey)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {

                T t;
                if (_cacheDic.TryGetValue(objectKey, out t))
                {
                    ChangeTimer(objectKey);
                    return t;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        private void ChangeTimer(ObjectKey objectKey)
        {
            Timer timer;
            if (_timerDic.TryGetValue(objectKey, out timer))
            {
                timer.Change(_cachingTime, TimeSpan.FromMilliseconds(-1));
            }
            else
            {
                _timerDic[objectKey] = new Timer(new TimerCallback(CallBack), objectKey, _cachingTime, TimeSpan.FromMilliseconds(-1));
            }
        }

        private void CallBack(object state)
        {
            Delete((ObjectKey) state);
        }
    }
}
