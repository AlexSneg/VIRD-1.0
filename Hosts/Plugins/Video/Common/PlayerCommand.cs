using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using AxWMPLib;
using Hosts.Plugins.Video.UI;
//using WMPLib;

namespace Hosts.Plugins.Video.Common
{
    internal abstract class PlayerCommand
    {
        protected const char _commandDelimeter = '.';
        public abstract string GetCommand(params string[] parameters);
        protected abstract PlayerCommand Next
        { get;}

        public abstract string DoCommand(WPFVideoHostControl player, params string[] parameters);

        protected virtual PlayerCommand ParseCommand(string command)
        {
            return Next == null ? null : Next.ParseCommand(command);
        }
        
        public static PlayerCommand Play()
        {
            return PlayCommand.Instance;
        }

        public static PlayerCommand Pause()
        {
            return PauseCommand.Instance;
        }

        public static PlayerCommand Seek()
        {
            return SeekCommand.Instance;
        }

        public static PlayerCommand State()
        {
            return StateCommand.Instance;
        }

        public static PlayerCommand Parse(string command)
        {
            return PlayCommand.Instance.ParseCommand(command);
        }
    }

    internal class PlayCommand : PlayerCommand
    {
        private const string _command = "play";
        private static readonly PlayCommand _instance = new PlayCommand();
        public static PlayCommand Instance
        {
            get { return _instance; }
        }

        private PlayCommand()
        {}

        #region Overrides of PlayerCommand

        //public override string GetCommand
        //{
        //    get { return _command; }
        //}

        public override string DoCommand(WPFVideoHostControl player, params string[] parameters)
        {
            player.Play();
            return StateCommand.Instance.GetState(player);
        }

        public override string GetCommand(params string[] parameters)
        {
            return _command;
        }

        protected override PlayerCommand Next
        {
            get
            {
                return PauseCommand.Instance;
            }
        }

        protected override PlayerCommand ParseCommand(string command)
        {
            if (command.Equals(_command)) return this;
            return base.ParseCommand(command);
        }

        #endregion
    }

    internal class PauseCommand : PlayerCommand
    {
        private const string _command = "pause";
        private static readonly PauseCommand _instance = new PauseCommand();
        public static PauseCommand Instance
        {
            get { return _instance; }
        }

        private PauseCommand()
        {}

        #region Overrides of PlayerCommand

        //public override string GetCommand
        //{
        //    get { return _command; }
        //}

        public override string GetCommand(params string[] parameters)
        {
            return _command;
        }

        protected override PlayerCommand Next
        {
            get { return SeekCommand.Instance; }
        }

        public override string DoCommand(WPFVideoHostControl player, params string[] parameters)
        {
            player.Pause();
            return StateCommand.Instance.GetState(player);
        }

        protected override PlayerCommand ParseCommand(string command)
        {
            if (command.Equals(_command)) return this;
            return base.ParseCommand(command);
        }

        #endregion
    }

    internal class SeekCommand : PlayerCommand
    {
        private const string _command = "seek";
        private static readonly SeekCommand _instance = new SeekCommand();
        public static SeekCommand Instance
        {
            get { return _instance; }
        }

        private SeekCommand()
        {}

        //private int _seconds;
        //public SeekCommand SetSeconds(int seconds)
        //{
        //    _seconds = seconds;
        //    return this;
        //}

        #region Overrides of PlayerCommand

        //public override string GetCommand
        //{
        //    get { return string.Format("{0}{1}{2}",
        //        _command, _commandDelimeter, _seconds); }
        //}

        public override string GetCommand(params string[] parameters)
        {
            // в качестве параметра должны быть секунды
            return string.Format("{0}{1}{2}",
                _command, _commandDelimeter, parameters[0]);
        }

        protected override PlayerCommand Next
        {
            get { return StateCommand.Instance; }
        }

        public override string DoCommand(WPFVideoHostControl player, params string[] parameters)
        {
            string[] commands = parameters[0].Split(_commandDelimeter);
            player.CurrentPosition = Int32.Parse(commands[1]);     // _seconds;
            return StateCommand.Instance.GetState(player);
        }

        protected override PlayerCommand ParseCommand(string command)
        {
            string[] parts = command.Split(_commandDelimeter);
            if (parts[0].Equals(_command)) return this;     //.SetSeconds(Int32.Parse(parts[1]));
            return base.ParseCommand(command);
        }

        #endregion
    }

    internal class PlayerStatusClass
    {
        public PlayerStatus PlayerStatus { get; set; }
        public int CurrentPosition { get; set; }
    }
    internal enum PlayerStatus
    {
        Played,
        Paused,
        Unknown
    }
    internal class StateCommand : PlayerCommand
    {
        private const string _command = "state";
        private const char _internalDelimeter = ' ';
        //private PlayerStatus _playerStatus;
        //public PlayerStatus PlayerStatus
        //{
        //    get { return _playerStatus; }
        //}

        //private int _currentPosition;
        //public int CurrentPosition
        //{
        //    get { return _currentPosition; }
        //}

        private static readonly StateCommand _instance = new StateCommand();
        public static StateCommand Instance
        {
            get { return _instance; }
        }

        private StateCommand()
        {}

        public static PlayerStatusClass ParseState(string state)
        {
            string[] states = state.Split(_internalDelimeter);
            PlayerStatus playerStatus = (PlayerStatus)Enum.Parse(typeof(PlayerStatus), states[0]);
            int currentPosition = Int32.Parse(states[1]);
            return new PlayerStatusClass {CurrentPosition = currentPosition, PlayerStatus = playerStatus };
        }

        public string GetState(WPFVideoHostControl player)
        {
            return DoCommand(player);
        }


        #region Overrides of PlayerCommand

        public override string GetCommand(params string[] parameters)
        {
            return _command;
        }
        //public override string GetCommand
        //{
        //    get { return _command; }
        //}

        protected override PlayerCommand Next
        {
            get { return null; }
        }

        public override string DoCommand(WPFVideoHostControl player, params string[] parameters)
        {
            PlayerStatus playerStatus;
            int currentPosition;
            switch (player.State)
            {
                case WPFVideoHostControl.PlayState.Paused:
                case WPFVideoHostControl.PlayState.Stoped:
                    playerStatus = PlayerStatus.Paused;
                    break;
                case WPFVideoHostControl.PlayState.Played:
                    playerStatus = PlayerStatus.Played;
                    break;
                default:
                    playerStatus = PlayerStatus.Unknown;
                    break;
            }
            if (playerStatus != PlayerStatus.Unknown)
            {
                currentPosition = player.CurrentPosition;
            }
            else
            {
                currentPosition = 0;
            }
            return string.Format("{0}{1}{2}",
                playerStatus, _internalDelimeter, currentPosition);
        }

        protected override PlayerCommand ParseCommand(string command)
        {
            string[] parts = command.Split(_commandDelimeter);
            if (parts[0].Equals(_command)) return this;
            return base.ParseCommand(command);
        }

        #endregion
    }

}
