using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WMPLib;

namespace Hosts.Plugins.Video
{
    public class WMPlayer : IDisposable
    {
        private WindowsMediaPlayerClass _player = null;
        private AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        private const int wmposMediaOpen = 13;

        private double _duration;
        private int _width;
        private int _height;

        private readonly int _waitingTime = 30000; // 30 сек


        public static WMPlayer Create(string fileName)
        {
            return new WMPlayer(fileName);
        }

        private WMPlayer(string fileName)
        {
            _player = new WindowsMediaPlayerClass();
            _player.mute = true;
            _player.OpenStateChange += new _WMPOCXEvents_OpenStateChangeEventHandler(_player_OpenStateChange);
            //_player.windowlessVideo = true;
            _player.URL = fileName;
            if (!_autoResetEvent.WaitOne(_waitingTime))
                throw new Exception(String.Format("WMPlayer не смог обработать файл {0}",
                    fileName));
            //Thread.Sleep(_waitingTime);
            _duration = _player.currentMedia.duration;
            _width = _player.currentMedia.imageSourceWidth;
            _height = _player.currentMedia.imageSourceHeight;
        }

        void _player_OpenStateChange(int NewState)
        {
            if (NewState == wmposMediaOpen)
                _autoResetEvent.Set();
        }

        public int Duration
        {
            get { return (int)Math.Round(_duration); }
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        ~WMPlayer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_autoResetEvent != null)
                {
                    _autoResetEvent.Close();
                    _autoResetEvent = null;
                }
            }
            if (_player != null)
            {
                _player.close();
                _player = null;
            }
        }

    }
}
