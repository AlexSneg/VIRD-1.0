using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using Syncfusion.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using Hosts.Plugins.DVDPlayer.SystemModule.Design;
using Hosts.Plugins.DVDPlayer.SystemModule.Config;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.DVDPlayer.Player
{
    public partial class DVDPlayerControl2 : SourceHardPluginBaseControl, IDVDPlayerView
    {
        private InterfaceTypeEnum _DVDType;
        private int _prevTotalTrackNumber = -1;
        private int _prevTotalChapterNumber = -1;
        private int _prevCurrTrackNumber = -1;
        private int _prevCurrChapterNumber = -1;

        public DVDPlayerControl2()
        {
            InitializeComponent();
        }
        public DVDPlayerControl2(Source source, IPlayerCommand playerCommand, IEventLogging logging, IPresentationClient client)
            : this()
        {
            InitializeController(new PlayerController(client, source, this, playerCommand, logging));
            switch (_DVDType)
            {
                case InterfaceTypeEnum.IR:
                    SetControlPlayerTimerEnable(false, 1);
                    break;
                case InterfaceTypeEnum.RS232:
                    SetControlPlayerTimerEnable(true, 1000);
                    break;
            }
        }

        public void InitializeData(DVDPlayerDeviceDesign device)
        {
            base.InitializeData();
            CollapsedRGBOption = true;
            /////инициализация параметров по девайсу
            _DVDType = device.InterfaceType;
            UpdateView(true, device, device.IsPlayerOn, "00:00:00", -1, -1, DVDState.NO_CD);
            switch (device.InterfaceType)
            {
                case InterfaceTypeEnum.IR:
                    InitIR(device);
                    //SetControlPlayerTimerEnable(false, 1);
                    break;
                case InterfaceTypeEnum.RS232:
                    InitRS232(device);
                    //SetControlPlayerTimerEnable(true, 1000);
                    break;
            }
            cbaChapter.Enabled = itbChapter.Enabled = (device.MediumType == MediumTypeEnum.DVD ? true :  false);
            ///////////////////////////////////
            
        }

        private delegate void UpdateViewDelegate(bool available, DVDPlayerDeviceDesign device, 
            bool IsPlayerOn, string elapsedTime,
            int trackCurrNumber, int chapterCurrNumber, DVDState state);

        public void UpdateView(bool available, DVDPlayerDeviceDesign device, 
            bool IsPlayerOn, string elapsedTime,
            int trackCurrNumber, int chapterCurrNumber, DVDState state)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateViewDelegate(UpdateView),
                    available, device, IsPlayerOn, elapsedTime, trackCurrNumber, chapterCurrNumber, state);
                return;
            }
            SetAvailableStatus(available);
            if (available)
            {
                if (IsPlayerOn)
                {
                    baPower.State = ButtonAdvState.Pressed;
                    if ((_prevTotalTrackNumber != device.TrackAmount) ||
                        (_prevTotalChapterNumber != device.DVDChapterAmount))
                        InitRS232(device);
                    if (state != DVDState.NO_CD)
                    {
                        if (_prevCurrChapterNumber != chapterCurrNumber)
                        {
                            _prevCurrChapterNumber = chapterCurrNumber;
                            if (chapterCurrNumber >= 0)
                            {
                                cbaChapter.SelectedItem = chapterCurrNumber.ToString();
                                itbChapter.Text = chapterCurrNumber.ToString();
                            }
                        }
                        if (_prevCurrTrackNumber != trackCurrNumber)
                        {
                            _prevCurrTrackNumber = trackCurrNumber;
                            if (trackCurrNumber >= 0)
                            {
                                cbaTrack.SelectedItem = trackCurrNumber.ToString();
                                itbTrack.Text = trackCurrNumber.ToString();
                            }
                        }
                        cbaChapter.Enabled = (device.MediumType == MediumTypeEnum.DVD ? true : false);
                        if (device.MediumType == MediumTypeEnum.DVD)
                            cbaTrack.Enabled = string.IsNullOrEmpty((string)cbaChapter.SelectedItem) ? false : true;

                        timeLabel.Text = elapsedTime + "/" + device.TotalPlaybackTime;
                        rewButton.State = playButton.State = stopButton.State =
                            pauseButton.State = ffButton.State = ButtonAdvState.Default;
                        switch (state)
                        {
                            case DVDState.Playback:
                                playButton.State = ButtonAdvState.Pressed;
                                break;
                            case DVDState.Pause:
                                pauseButton.State = ButtonAdvState.Pressed;
                                break;
                            case DVDState.Stopped:
                                stopButton.State = ButtonAdvState.Pressed;
                                break;
                            case DVDState.FFWD:
                                ffButton.State = ButtonAdvState.Pressed;
                                break;
                            case DVDState.REW:
                                rewButton.State = ButtonAdvState.Pressed;
                                break;
                        }
                    }
                    else
                    {
                        //плеер включен но диска нет
                        setDVDDefaultState();
                    }
                }
                else
                {
                    //плеер выключен
                    baPower.State = ButtonAdvState.Default;
                    setDVDDefaultState();
                }
            }
            else
            {
                //состояние железа не доступно
            }
            this.Refresh();
        }

        private void setDVDDefaultState()
        {
            timeLabel.Text = "0:00:00 / 0:00:00";
            cbaChapter.SelectedItem = cbaTrack.SelectedItem = string.Empty;
            cbaChapter.Enabled = cbaTrack.Enabled = false;
            rewButton.State = playButton.State = stopButton.State =
                pauseButton.State = ffButton.State = ButtonAdvState.Default;
        }

        private void InitIR(DVDPlayerDeviceDesign device)
        {
            cbaChapter.Visible = cbaTrack.Visible = false;
            itbChapter.Visible = itbTrack.Visible = true;
            rewButton.PushButton = playButton.PushButton = stopButton.PushButton =
                pauseButton.PushButton = ffButton.PushButton = false;
        }
        private void InitRS232(DVDPlayerDeviceDesign device)
        {
            _prevTotalChapterNumber = device.DVDChapterAmount;
            _prevTotalTrackNumber = device.TrackAmount;
            cbaChapter.Visible = cbaTrack.Visible = true;
            itbChapter.Visible = itbTrack.Visible = false;
            object chapterSelected = cbaChapter.SelectedItem;
            object trackSelected = cbaTrack.SelectedItem;
            cbaChapter.Items.Clear();
            cbaTrack.Items.Clear();
            cbaChapter.Items.Add(string.Empty);
            for (int i = 1; i <= device.DVDChapterAmount; i++)
            {
                cbaChapter.Items.Add(i.ToString());
            }
            cbaTrack.Items.Add(string.Empty);
            for (int i = 1; i <= device.TrackAmount; i++)
            {
                cbaTrack.Items.Add(i.ToString());
            }
            cbaChapter.SelectedItem = chapterSelected;
            cbaTrack.SelectedItem = trackSelected;
            rewButton.PushButton = playButton.PushButton = stopButton.PushButton =
                pauseButton.PushButton = ffButton.PushButton = true;
            rewButton.State = playButton.State = stopButton.State =
                pauseButton.State = ffButton.State = ButtonAdvState.Default;
        }

        private void commandButton_Click(object sender, EventArgs e)
        {
            string cmd = (sender as ButtonAdv).Tag as string;
            if (!string.IsNullOrEmpty(cmd))
            {
                sendPushCommandButtonEvent(cmd, null);
            }
        }

        private void baPower_Click(object sender, EventArgs e)
        {
            ButtonAdv btn = sender as ButtonAdv;
            if ((btn != null) && (!string.IsNullOrEmpty((string)btn.Tag)))
            {
                //если нажата значит включено
                int param = (btn.State == ButtonAdvState.Pressed ? 1 : 0);
                sendPushCommandButtonEvent((string)btn.Tag, param);
            }
        }

        private void baSetTrack_Click(object sender, EventArgs e)
        {
            string cmd = (sender as ButtonAdv).Tag as string;
            if (!string.IsNullOrEmpty(cmd))
            {
                foreach (string item in cmd.Split(','))
                {
                    string tmp = textForSetTrackAndChapter(item);
                    if (!string.IsNullOrEmpty(tmp))
                    {
                        int numTC = Convert.ToInt32(tmp);
                        sendPushCommandButtonEvent(item, numTC);
                    }
                }
            }
        }
        private string textForSetTrackAndChapter(string key)
        {
            if (_DVDType == InterfaceTypeEnum.RS232)
                key = "cba" + key;
            else if (_DVDType == InterfaceTypeEnum.IR)
                key = "itb" + key;
            Control cntrl = this.Controls.Find(key, true)[0];
            return cntrl.Text;
        }

        private void cbaChapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)cbaChapter.SelectedItem))
                cbaTrack.Enabled = false;
            else
            {
                int chapter = Convert.ToInt32((string)cbaChapter.SelectedItem);
                cbaTrack.Enabled = true;
                sendPushCommandButtonEvent((string)cbaChapter.Tag, chapter);
            }
        }
    }
}
