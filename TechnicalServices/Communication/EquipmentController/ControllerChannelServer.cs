using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Communication.EquipmentController
{
    public class TcpControllerChannelServer : IControllerChannel
    {
        private readonly Uri _connectionString;
        private readonly Encoding _encoding;
        private TcpListener _listener;

        public TcpControllerChannelServer(Uri connectionString)
        {
            _connectionString = connectionString;
            _encoding = Encoding.ASCII;
            _listener = new TcpListener(IPAddress.Any, _connectionString.Port);
            //_listener.Start();
        }

        
        #region IControllerChannel Members

        public void Start()
        {
            throw new NotImplementedException();
        }

        public string Send(CommandDescriptor cmd)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
