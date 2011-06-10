using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.VNC.SystemModule.Design;
using VncLib;

namespace Hosts.Plugins.VNC.Common
{
    internal abstract class VNCCommand
    {
        protected static char _commandDelimeter = '.';
        protected abstract VNCCommand Next
        { get; }

        public abstract string DoCommand(IVNCAction vncAction);

        protected virtual VNCCommand ParseCommand(string command)
        {
            return Next == null ? null : Next.ParseCommand(command);
        }

        public abstract string Command
        { get; }

        internal static VNCCommand Disconnect()
        {
            return VNCDisconnect.Instance;
        }

        public static VNCCommand Connect()
        {
            return VNCConnect.Instance;
        }

        public static VNCCommand State()
        {
            return VNCStateCommand.Instance;
        }

        public static VNCCommand Parse(string command)
        {
            return VNCConnect.Instance.ParseCommand(command);
        }
    }

    internal class VNCConnect : VNCCommand
    {
        private readonly static VNCConnect _instance = new VNCConnect();
        public static VNCConnect Instance
        {
            get { return _instance; }
        }

        private const string _command = "connect";
        #region Overrides of VNCCommand

        protected override VNCCommand Next
        {
            get { return VNCDisconnect.Instance; }
        }

        public override string DoCommand(IVNCAction vncAction)
        {
            vncAction.Connect();
            return VNCStateCommand.Instance.GetState(vncAction);
        }

        public override string Command
        {
            get { return _command; }
        }

        protected override VNCCommand ParseCommand(string command)
        {
            if (command.Equals(_command)) return this;
            return base.ParseCommand(command);
        }

        #endregion
    }

    internal class VNCDisconnect : VNCCommand
    {
        private readonly static VNCDisconnect _instance = new VNCDisconnect();
        public static VNCDisconnect Instance
        {
            get { return _instance; }
        }

        private const string _command = "disconnect";
        #region Overrides of VNCCommand

        protected override VNCCommand Next
        {
            get { return VNCStateCommand.Instance; }
        }

        public override string DoCommand(IVNCAction vncAction)
        {
            vncAction.Disconnect();
            return VNCStateCommand.Instance.GetState(vncAction);
        }

        public override string Command
        {
            get { return _command; }
        }

        protected override VNCCommand ParseCommand(string command)
        {
            if (command.Equals(_command)) return this;
            return base.ParseCommand(command);
        }

        #endregion
    }

    internal class VNCStateCommand : VNCCommand
    {
        private static readonly VNCStateCommand _instance = new VNCStateCommand();
        public static VNCStateCommand Instance
        {
            get { return _instance; }
        }

        private const string _command = "state";
        private ConnectionStatus _status;

        public VNCStateCommand ParseState(string state)
        {
            _status = (ConnectionStatus)Enum.Parse(typeof(ConnectionStatus), state, true);
            return this;
        }

        public ConnectionStatus Status
        {
            get { return _status; }
        }

        #region Overrides of VNCCommand

        protected override VNCCommand Next
        {
            get { return null;}
        }

        public override string DoCommand(IVNCAction vncAction)
        {
            if (vncAction.IsConnected())
                return ConnectionStatus.Connected.ToString();
            else
                return ConnectionStatus.Disconnected.ToString();
        }

        public override string Command
        {
            get { return _command; }
        }

        protected override VNCCommand ParseCommand(string command)
        {
            if (command.Equals(_command)) return this;
            return base.ParseCommand(command);
        }

        #endregion

        public string GetState(IVNCAction vncAction)
        {
            return DoCommand(vncAction);
        }
    }
}
