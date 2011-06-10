using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Communication.Communication.Client
{
    /// <summary>
    /// клиент с пингованием - то бишь автоматом поддерживает соединение
    /// </summary>
    /// <typeparam name="TChannel"></typeparam>
    public class PingClient<TChannel> : SimpleClient<TChannel>
        where TChannel : class
    {
        protected readonly object _pingSync = new object();
        protected readonly int _pingInterval;
        protected Timer _timer = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pingInterval">интервал пингования в секундах</param>
        /// <param name="endpointName"></param>
        /// <param name="address"></param>
        public PingClient(int pingInterval, string endpointName, Uri address)
            : base(endpointName, address)
        {
            _pingInterval = pingInterval;
            //if (_channel is IPing)
            //    _timer = new Timer(new TimerCallback(CallBack), null, pingInterval * 1000, pingInterval * 1000);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pingInterval">интервал пингования в секундах</param>
        public PingClient(int pingInterval)
            : this(pingInterval, "*", null)
        { }

        protected override void CreateChannel()
        {
            base.CreateChannel();
            if (_channel is IPing && _timer == null)
                _timer = new Timer(new TimerCallback(CallBack), null, _pingInterval * 1000, _pingInterval * 1000);
        }

        protected void CallBack(object state)
        {
            if (!Monitor.TryEnter(_pingSync)) return;
            try
            {
                Ping();
            }
            catch (Exception)
            {
                lock (this)
                {
                    Abort();
                    //CreateChannel();
                    try
                    {
                        Open();
                        //OpenChannel();
                    }
                    catch (Exception)
                    {
                        Abort();
                    }
                }
            }
            finally
            {
                Monitor.Exit(_pingSync);
            }
        }

        protected void Ping()
        {
            IPing ping = _channel as IPing;
            if (ping != null)
            {
                UserIdentity identity = Thread.CurrentPrincipal as UserIdentity;
                ping.Ping(identity);
            }
        }

        public override void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
            base.Dispose();
        }
    }
}
