using System;
using System.Diagnostics;
using System.ServiceModel;

namespace TechnicalServices.Communication.Communication.Client
{
    public class ClientState : EventArgs
    {
        public readonly CommunicationState State;

        public ClientState(CommunicationState state)
        {
            State = state;
        }
    }

    public abstract class ClientBase<TChannel> : IDisposable
        where TChannel : class
    {
        protected readonly EndpointAddress _address = null;
        protected readonly string _endpointName = "*";

        protected TChannel _channel;
        protected abstract ChannelFactory<TChannel> ChannelFactory { get; }

        protected ClientBase()
        {
        }

        protected ClientBase(string endpointName, Uri address)
        {
            if (!string.IsNullOrEmpty(endpointName))
                _endpointName = endpointName;
            if (address != null)
                _address = new EndpointAddress(address);
        }

        public TChannel Channel
        {
            get
            {
                lock (this)
                {
                    return _channel;
                }
            }
        }

        public void Open()
        {
            CreateChannel();
            OpenChannel();
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
            Abort();
            ChannelFactory.Close();
        }

        #endregion

        public event EventHandler<ClientState> OnChanged;

        protected void OpenChannel()
        {
            if (_channel == null) return;
            ICommunicationObject obj = (ICommunicationObject)_channel;
            obj.Open();
        }

        protected void Abort()
        {
            if (_channel == null) return;
            ICommunicationObject obj = (ICommunicationObject)_channel;
            obj.Abort();
        }

        protected virtual void CreateChannel()
        {
            _channel = ChannelFactory.CreateChannel();
            ICommunicationObject obj = (ICommunicationObject)_channel;
            obj.Closed += obj_Closed;
            obj.Closing += obj_Closing;
            obj.Faulted += obj_Faulted;
            obj.Opened += obj_Opened;
            obj.Opening += obj_Opening;
        }

        protected CommunicationState GetState()
        {
            ICommunicationObject obj = (ICommunicationObject)_channel;
            return obj.State;
        }

        private void OnChange()
        {
            if (OnChanged != null)
            {
                OnChanged(this, new ClientState(GetState()));
            }
        }

        private void obj_Opening(object sender, EventArgs e)
        {
            OnChange();
        }

        private void obj_Opened(object sender, EventArgs e)
        {
            OnChange();
        }

        private void obj_Faulted(object sender, EventArgs e)
        {
            //Debug.Assert(false, String.Format("ПОПА! канал накрылся, какая то фигня - надо разбираться. \n{0}",
            //                                  sender));
            OnChange();
        }

        private void obj_Closing(object sender, EventArgs e)
        {
            OnChange();
        }

        private void obj_Closed(object sender, EventArgs e)
        {
            OnChange();
        }
    }
}