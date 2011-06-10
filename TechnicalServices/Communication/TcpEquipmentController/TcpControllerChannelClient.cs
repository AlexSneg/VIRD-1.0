using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

using TechnicalServices.Communication.EquipmentController;

namespace TechnicalServices.Communication.TcpEquipmentController
{
    public class TcpControllerChannelClient : ControllerChannelClient
    {
        private TcpClient _client;

        public TcpControllerChannelClient(Uri connectionString)
            : base(connectionString, Encoding.ASCII)
        {

        }

        protected override Stream OpenStream(Uri ConnectionString)
        {
            Debug.Assert(ConnectionString.Scheme == "tcp");
            Debug.Assert(ConnectionString.Port != 0);
            Debug.Assert(!String.IsNullOrEmpty(ConnectionString.Host));

            _client = new TcpClient(ConnectionString.Host, ConnectionString.Port);
            return _client.GetStream();
        }

        protected override void CloseStream(Stream stream)
        {
            Debug.Assert(stream == null);
            stream.Dispose();
            _client.Close();
        }
    }
}