using System;
using System.ServiceModel;

namespace TechnicalServices.Communication.Communication.Client
{
    public class SimpleClientUri<TChannel> : ClientBase<TChannel>
        where TChannel : class
    {
        private readonly EndpointAddress _address;
        private readonly string _endpointName;
        protected ChannelFactory<TChannel> _channelFactory = null;

        public SimpleClientUri(string endpointName, Uri address)
        {
            _endpointName = endpointName;
            _address = new EndpointAddress(address);
            CreateChannel();
        }

        public void Open()
        {
            OpenChannel();
        }

        public void Close()
        {
            ICommunicationObject obj = _channel as ICommunicationObject;
            obj.Close();
        }


        protected override ChannelFactory<TChannel> ChannelFactory
        {
            get
            {
                if (_channelFactory == null)
                {
                    _channelFactory = new ChannelFactory<TChannel>(_endpointName, _address);
                }
                return _channelFactory;
            }
        }
    }
}