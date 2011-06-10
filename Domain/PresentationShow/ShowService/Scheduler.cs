using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Timer = System.Timers.Timer;

namespace Domain.PresentationShow.ShowService
{
    internal class Scheduler : IDisposable
    {
        private Timer _timer = null;
        private readonly ReaderWriterLockSlim _sync = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private readonly IConfiguration _config;
        private int _working = 0;
        //private DateTime _currentTime = DateTime.MinValue;
        private readonly List<Item> _schedulerItems = new List<Item>();
        private int _nextIndex;
        public Scheduler(IConfiguration config)
        {
            _config = config;
            _timer = new Timer { AutoReset = false };
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
        }

        #region Nested

        private class Item : IComparable<Item>
        {
            public Item(int slideId, TimeSpan time)
            {
                SlideId = slideId;
                Time = time;
            }
            public int SlideId { get; set; }
            public TimeSpan Time { get; set; }

            #region Implementation of IComparable<Item>

            public int CompareTo(Item other)
            {
                return Time.CompareTo(other.Time);
            }

            #endregion
        }

        #endregion

        #region public

        public void Start(PresentationInfo presentationInfo)
        {
            Interlocked.Exchange(ref _working, 1);
            _sync.EnterWriteLock();
            try
            {
                _schedulerItems.Clear();
                // берем все слайды с ненулевым временем и запихиваем по возрастанию времени в словарь
                IOrderedEnumerable<SlideInfo> orderedSlides =
                    presentationInfo.SlideInfoList.Where(si => si.Time > TimeSpan.Zero).OrderBy(si => si.Time);
                foreach (SlideInfo info in orderedSlides)
                {
                    _schedulerItems.Add(new Item(info.Id, info.Time));
                }
                if (_schedulerItems.Count == 0) return;
                // стартуем таймеры и тюд
                DateTime currentTime = DateTime.Now;

                _timer.Interval = CalculateTimerInterval(currentTime);
                _timer.Start();
            }
            catch (Exception ex)
            {
                Interlocked.Exchange(ref _working, 0);
                _config.EventLog.WriteError(string.Format("Scheduler.Start: {0}", ex));
            }
            finally
            {
                _sync.ExitWriteLock();
            }
        }


        public void Stop()
        {
            Interlocked.Exchange(ref _working, 0);
            _timer.Stop();
            _schedulerItems.Clear();
        }

        public void UpdateSchedule(PresentationInfo presentationInfo)
        {
            if (_working == 0) return;
            if (_timer != null) _timer.Stop();
            Start(presentationInfo);
        }

        /// <summary>
        /// тикнуло - аргумент - slideId на который надо прыгнуть
        /// </summary>
        public event Action<int> OnTick;

        #endregion

        #region private

        private double CalculateTimerInterval(DateTime currentTime)
        {
            int index = _schedulerItems.BinarySearch(new Item(0, currentTime - currentTime.Date));
            int findingIndex;
            if (index >= 0)
            {
                findingIndex = index;
            }
            else
            {
                int ind = ~index;
                if (ind < _schedulerItems.Count)
                {
                    findingIndex = ind;
                }
                else
                {
                    findingIndex = 0;
                }
            }

            Item start = _schedulerItems[findingIndex];
            Interlocked.Exchange(ref _nextIndex, findingIndex);
            if (start.Time.CompareTo(currentTime - currentTime.Date) >= 0)
            {
                return (start.Time - (currentTime - currentTime.Date)).TotalMilliseconds;
            }
            else
            {
                return (start.Time + (currentTime.Date.AddDays(1) - currentTime)).TotalMilliseconds;
            }
        }


        private void Tick(int slideId)
        {
            if (OnTick != null)
            {
                OnTick.Invoke(slideId);
            }
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!_sync.TryEnterReadLock(0)) return;
            try
            {
                Tick(_schedulerItems[_nextIndex].SlideId);
                _timer.Interval = CalculateTimerInterval(e.SignalTime);
                _timer.Start();
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("Scheduler._timer_Elapsed: {0}", ex));
            }
            finally
            {
                _sync.ExitReadLock();
            }
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        #endregion
    }
}
