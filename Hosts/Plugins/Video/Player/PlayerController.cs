using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.Video.Common;
using Hosts.Plugins.Video.SystemModule.Design;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.Video.Player
{
    internal class PlayerController
    {
        //private static String _playCommand = "play";
        //private static String _pauseComamnd = "pause";

        private String _status;
        private String _action;
        private bool _isPlaying = false;
        private readonly Source _source = null;
        private readonly IPlayerCommand _playerProvider;
        private readonly int _duration = 0;
        private int _currentPosition = 0;

        internal PlayerController(Source source, IPlayerCommand playerProvider)
        {
            _source = source;
            _playerProvider = playerProvider;
            VideoSourceDesign videoSourceDesign = source as VideoSourceDesign;
            if (videoSourceDesign != null)
            {
                _duration = videoSourceDesign.Duration;
            }
            GetStatus();
        }

        internal String Status
        {
            get { return _isPlaying ? "Воспроизведение" : "Остановлено"; ; }
        }

        internal String Action
        {
            get { return _isPlaying ? "Pause" : "Play"; }
        }

        internal int Duration
        {
            get { return _duration; }
        }

        internal int CurrentPosition
        {
            get { return _currentPosition; }
        }

        internal void Play()
        {
            string state = _playerProvider.DoSourceCommand(_source, _isPlaying ? PlayerCommand.Pause().GetCommand() : PlayerCommand.Play().GetCommand());
            ParseState(state);
        }

        internal void Seek(int seconds)
        {
            string state = _playerProvider.DoSourceCommand(_source, PlayerCommand.Seek().GetCommand(seconds.ToString()));
            ParseState(state);
        }

        internal void GetStatus()
        {
            string state = _playerProvider.DoSourceCommand(_source, PlayerCommand.State().GetCommand());
            ParseState(state);
        }

        private void ParseState(string state)
        {
            if (string.IsNullOrEmpty(state)) return;
            PlayerStatusClass psc = StateCommand.ParseState(state);
            _isPlaying = psc.PlayerStatus == PlayerStatus.Played;
            _currentPosition = psc.CurrentPosition;
        }

    }
}
