using System.Drawing;
using System.Windows.Forms;

using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;

using Hosts.Plugins.Video.Common;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.Video.UI
{
    public partial class VideoForm : Form, IDoCommand
    {
        public VideoForm()
        {
            InitializeComponent();
        }

        #region IDoCommand Members

        public string DoCommand(string command)
        {
            return PlayerCommand.Parse(command).DoCommand(wpfVideoHostControl1, command);
        }

        #endregion

        internal bool Init(DisplayType display, Window window)
        {
            this.InitBorderTitle(window, wpfVideoHostControl1);
            return true;
        }

        public void LoadVideo(string videoFile, int startPosition, bool startPlay)
        {
            //axWindowsMediaPlayer.settings.autoStart = startPlay;
            //axWindowsMediaPlayer.settings.setMode("loop", false);
            //axWindowsMediaPlayer.URL = videoFile;
            //axWindowsMediaPlayer.stretchToFit = !aspectLock;
            //axWindowsMediaPlayer.Ctlcontrols.currentPosition = startPosition;
            //wpfVideoHostControl1.OpenVideo(videoFile, startPosition, startPlay, false);
            LoadVideo(videoFile, startPosition, startPlay, false);
        }

        public void LoadVideo(string videoFile, int startPosition, bool startPlay, bool mute)
        {
            wpfVideoHostControl1.OpenVideo(videoFile, startPosition, startPlay, mute);
        }

    }
}