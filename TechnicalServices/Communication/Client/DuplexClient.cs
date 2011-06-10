using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Communication.Communication.Client
{
    /// <summary>
    /// Класс который должен использоваться для создания клиентов
    /// Если клиентский интерфейс наследует интерфейс IPing, то клиент будет автоматически поддерживать канал
    /// </summary>
    /// <typeparam name="TChannel"> клиентский интерфейс</typeparam>
    public class DuplexClient<TChannel> : PingClient<TChannel>
        where TChannel : class
    {
        protected InstanceContext _callbackInstance;
        //private Timer _timer = null;
        //private readonly object _pingSync = new object();
        //private readonly DuplexChannelFactory<TChannel> _channelFactory = null;

        public DuplexClient(InstanceContext callbackInstance, int pingInterval,
            string endpointName, Uri address)
            : base(pingInterval, endpointName, address)
        {
            _callbackInstance = callbackInstance;
            //_channelFactory = new DuplexChannelFactory<TChannel>(callbackInstance, "*");
            ////CreateChannel();
            ////OpenChannel();
            //if (_channel is IPing)
            //    _timer = new Timer(new TimerCallback(CallBack), null, pingInterval * 1000, pingInterval * 1000);
        }

        /// <summary>
        /// constructer
        /// </summary>
        /// <param name="callbackInstance">callback</param>
        /// <param name="pingInterval">интервал пингования в секундах</param>
        public DuplexClient(InstanceContext callbackInstance, int pingInterval)
            : this(callbackInstance, pingInterval, "*", null)
        {
            //_callbackInstance = callbackInstance;
            //_channelFactory = new DuplexChannelFactory<TChannel>(callbackInstance, "*");
            //CreateChannel();
            //OpenChannel();
            //if (_channel is IPing)
            //    _timer = new Timer(new TimerCallback(CallBack), null, pingInterval * 1000, pingInterval * 1000);
        }

        //private void CallBack(object state)
        //{
        //    IPing ping = _channel as IPing;
        //    if (ping != null)
        //    {
        //        try
        //        {
        //            if (!Monitor.TryEnter(_pingSync)) return;
        //            try
        //            {
        //                UserIdentity identity = Thread.CurrentPrincipal as UserIdentity;
        //                ping.Ping(identity);
        //            }
        //            finally
        //            {
        //                Monitor.Exit(_pingSync);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            lock (this)
        //            {
        //                Abort();
        //                CreateChannel();
        //                try
        //                {
        //                    OpenChannel();
        //                }
        //                catch (Exception)
        //                {
        //                    Abort();
        //                }
        //            }
        //        }
        //    }
        //}

        protected override ChannelFactory<TChannel> ChannelFactory
        {
            get
            {
                if (_channelFactory == null)
                {
                    if (_address == null)
                        _channelFactory = new DuplexChannelFactory<TChannel>(_callbackInstance, _endpointName);
                    else
                        _channelFactory = new DuplexChannelFactory<TChannel>(_callbackInstance, _endpointName, _address);
                }
                return _channelFactory;
            }
        }

        //public override void Dispose()
        //{
        //    if (_timer != null)
        //    {
        //        _timer.Dispose();
        //        _timer = null;
        //    }
        //    base.Dispose();
        //}
    }
}
