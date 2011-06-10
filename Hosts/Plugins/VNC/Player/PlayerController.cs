using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.VNC.Common;
using Hosts.Plugins.VNC.SystemModule.Design;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.VNC.Player
{
    internal class PlayerController
    {
        private ConnectionStatus _connectionStatus = ConnectionStatus.Disconnected;
        private readonly IPlayerCommand _playerProvider;
        private readonly Source _source = null;

        internal PlayerController(Source source, IPlayerCommand playerProvider)
        {
            _source = source;
            _playerProvider = playerProvider;
            GetStatus();
        }

        internal ConnectionStatus Status
        {
            get { return _connectionStatus; }
        }

        internal void Connect()
        {
            string state = _playerProvider.DoSourceCommand(_source,
                                                            _connectionStatus == ConnectionStatus.Connected
                                                                ?
                                                                    VNCCommand.Disconnect().Command
                                                                :
                                                                    VNCCommand.Connect().Command);
            ParseState(state);
        }

        internal void GetStatus()
        {
            string state = _playerProvider.DoSourceCommand(_source, VNCCommand.State().Command);
            ParseState(state);
        }

        private void ParseState(string state)
        {
            if (string.IsNullOrEmpty(state)) return;
            VNCStateCommand vncStateCommand = VNCStateCommand.Instance.ParseState(state);
            _connectionStatus = vncStateCommand.Status;
        }

    }
}
