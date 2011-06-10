using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media.Animation;
using UserControl=System.Windows.Forms.UserControl;

namespace Hosts.Plugins.Video.UI
{
    public partial class WPFVideoHostControl : UserControl
    {
        #region Nested

        public enum PlayState
        {
            Paused,
            Stoped,
            Played
        }

        #endregion

        private WPFVideoControl _wpfVideoControl = null;
        private int _duration = 0;
        private int _videoWidth = 0;
        private int _videoHeight = 0;
        private bool _startPlay;
        private PlayState _playState = PlayState.Stoped;
        private int _startPosition = 0;

        public WPFVideoHostControl()
        {
            InitializeComponent();
            HostWpf();
        }

        private void HostWpf()
        {
            ElementHost host = new ElementHost();
            host.Dock = DockStyle.Fill;

            _wpfVideoControl = new WPFVideoControl();
            _wpfVideoControl.VideoMediaElement.MediaOpened += new System.Windows.RoutedEventHandler(VideoMediaElement_MediaOpened);
            _wpfVideoControl.VideoMediaElement.MediaFailed += new EventHandler<System.Windows.ExceptionRoutedEventArgs>(VideoMediaElement_MediaFailed);
            _wpfVideoControl.VideoMediaElement.MediaEnded += new System.Windows.RoutedEventHandler(VideoMediaElement_MediaEnded);
            host.Child = _wpfVideoControl;
            this.Controls.Add(host);
        }

        private void Opened()
        {
            if (OnOpened != null)
            {
                OnOpened(this, EventArgs.Empty);
            }
        }

        private void Failed()
        {
            if (OnFailed != null)
            {
                OnFailed(this, EventArgs.Empty);
            }
        }


        void VideoMediaElement_MediaFailed(object sender, System.Windows.ExceptionRoutedEventArgs e)
        {
            Failed();
        }

        void VideoMediaElement_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            _duration = (int)_wpfVideoControl.VideoMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            _videoWidth = _wpfVideoControl.VideoMediaElement.NaturalVideoWidth;
            _videoHeight = _wpfVideoControl.VideoMediaElement.NaturalVideoHeight;
            if (_startPosition <= _duration)
                CurrentPosition = _startPosition;
            Opened();
        }

        void VideoMediaElement_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            _wpfVideoControl.VideoMediaElement.Stop();
            _playState = PlayState.Stoped;
        }

        private void WPFHostControl_Resize(object sender, EventArgs e)
        {
            if (_wpfVideoControl == null) return;

            _wpfVideoControl.Width = this.ClientSize.Width;
            _wpfVideoControl.Height = this.ClientSize.Height;
        }

        public void OpenVideo(string videoFile, int startPosition, bool startPlay, bool mute)
        {
            //_wpfVideoControl.VideoMediaElement.Position = TimeSpan.FromSeconds(startPosition);
            _wpfVideoControl.VideoMediaElement.IsMuted = mute;
            _wpfVideoControl.VideoMediaElement.Source = new Uri(videoFile);
            _startPosition = startPosition;
            _startPlay = startPlay;
        }

        public event EventHandler OnOpened;
        public event EventHandler OnFailed;

        public int Duration { get { return _duration; } }
        public int VideoWidth { get { return _videoWidth; } }
        public int VideoHeight { get { return _videoHeight; } }

        public int CurrentPosition
        {
            get { return (int)_wpfVideoControl.VideoMediaElement.Position.TotalSeconds; }
            set { _wpfVideoControl.VideoMediaElement.Position = TimeSpan.FromSeconds(value); }
        }


        public void Play()
        {
            _wpfVideoControl.VideoMediaElement.Play();
            _playState = PlayState.Played;
        }

        public void Pause()
        {
            _wpfVideoControl.VideoMediaElement.Pause();
            _playState = PlayState.Paused;
        }

        public PlayState State
        {
            get
            {
                return _playState;
            }
        }

        private void WPFVideoHostControl_Load(object sender, EventArgs e)
        {
            if (_startPlay)
            {
                _wpfVideoControl.VideoMediaElement.Play();
                _playState = PlayState.Played;
            }
            else
            {
                _wpfVideoControl.VideoMediaElement.Pause();
                _playState = PlayState.Paused;
            }
        }
    }
}
