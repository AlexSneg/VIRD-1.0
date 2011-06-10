using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace TechnicalServices.Communication.Communication.Client
{
    public class SimpleClient<TChannel> : ClientBase<TChannel>
        where TChannel : class
    {

        protected ChannelFactory<TChannel> _channelFactory = null;

        public SimpleClient() : base()
        {
            //CreateChannel();
            //OpenChannel();
        }

        public SimpleClient(string endpointName, Uri address) : 
            base(endpointName, address)
        {}

        protected override ChannelFactory<TChannel> ChannelFactory
        {
            get
            {
                if (_channelFactory == null)
                {
                    if (_address == null)
                        _channelFactory = new ChannelFactory<TChannel>(_endpointName);
                    else
                        _channelFactory = new ChannelFactory<TChannel>(_endpointName, _address);
                }
                return _channelFactory;
            }
        }
    }
}
