using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TechnicalServices.Exceptions;
//using WMPLib;

namespace Hosts.Plugins.Video.UI
{
    public partial class WMPlayerForm : Form
    {
        //private WindowsMediaPlayerClass player;
        private WPFVideoHostControl _control = null;
        public WMPlayerForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            _control = new WPFVideoHostControl();
            this.Controls.Add(_control);
            _control.OnFailed += new EventHandler(_control_OnFailed);
            _control.OnOpened += new EventHandler(_control_OnOpened);
            StartPlay(FileName);

            //player = new WindowsMediaPlayerClass();
            //player.enableErrorDialogs = false;
            //player.mute = true;
            //player.settings.enableErrorDialogs = false;
            //StartPlay(FileName);
            //base.OnLoad(e);
        }

        void _control_OnOpened(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        void _control_OnFailed(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        public string FileName { get; set; }

        private void StartPlay(string videoFile)
        {
            _control.OpenVideo(videoFile, 0, true, true);
            //axWindowsMediaPlayer1.URL = videoFile;
            //player.OpenStateChange += new _WMPOCXEvents_OpenStateChangeEventHandler(player_OpenStateChange);
            //player._WMPOCXEvents_Event_Error += new _WMPOCXEvents_ErrorEventHandler(player__WMPOCXEvents_Event_Error);
            //player.URL = videoFile;
        }

        //void player__WMPOCXEvents_Event_Error()
        //{
        //    DialogResult = DialogResult.Abort;
        //}

        //void player_OpenStateChange(int NewState)
        //{
        //    if (NewState != (int)WMPOpenState.wmposMediaOpen) return;
        //    player.OpenStateChange -= player_OpenStateChange;
        //    _duration = (int)player.currentMedia.duration;
        //    _videoWidth = player.currentMedia.imageSourceWidth;
        //    _videoHeight = player.currentMedia.imageSourceHeight;
        //    DialogResult = System.Windows.Forms.DialogResult.OK;
        //}

        //private int _duration;
        public int Duration
        {
            //get { return _duration; }
            get { return _control.Duration; }
        }

        //private int _videoWidth;
        public int VideoWidth
        {
            //get { return _videoWidth; }
            get { return _control.VideoWidth; }
        }

        //private int _videoHeight;
        public int VideoHeight
        {
            //get { return _videoHeight; }
            get { return _control.VideoHeight; }
        }

        //private void axWindowsMediaPlayer1_OpenStateChange(object sender, AxWMPLib._WMPOCXEvents_OpenStateChangeEvent e)
        //{
        //    if (e.newState != (int)WMPOpenState.wmposMediaOpen) return;
        //    axWindowsMediaPlayer1.OpenStateChange -= axWindowsMediaPlayer1_OpenStateChange;
        //    _duration = (int)axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
        //    _videoWidth = axWindowsMediaPlayer1.Ctlcontrols.currentItem.imageSourceWidth;
        //    _videoHeight = axWindowsMediaPlayer1.Ctlcontrols.currentItem.imageSourceHeight;
        //    DialogResult = System.Windows.Forms.DialogResult.OK;
        //}

        protected override void OnClosing(CancelEventArgs e)
        {
            //if (player != null)
            //{
            //    player.close();
            //    player = null;
            //}
            //if (axWindowsMediaPlayer1 != null)
            //{
            //    axWindowsMediaPlayer1.Ctlcontrols.stop();
            //    axWindowsMediaPlayer1.close();
            //    axWindowsMediaPlayer1 = null;
            //}
            base.OnClosing(e);
        }

       
    }
}
