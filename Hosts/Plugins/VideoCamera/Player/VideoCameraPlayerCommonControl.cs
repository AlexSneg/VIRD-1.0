using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using Hosts.Plugins.VideoCamera.SystemModule.Design;
using Syncfusion.Windows.Forms;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.VideoCamera.Player
{
    public partial class VideoCameraPlayerCommonControl : SourceHardPluginBaseControl, IVideoCameraPlayerCommonView
    {
        public VideoCameraPlayerCommonControl()
        {
            InitializeComponent();
        }

        public VideoCameraPlayerCommonControl(Source source, IPlayerCommand playerCommand, IEventLogging logging, IPresentationClient client)
            : this()
        {
            InitializeController(new VideoCameraPlayerCommonController(client, source, this, playerCommand, logging));
            SetControlPlayerTimerEnable(true, 1000);
        }

        public void InitializeData(VideoCameraDeviceDesign device)
        {
            InitializeData();
            for (int i = 1; i <= device.PresetAmount; i++)
            {
                cbaPreset.Items.Add(i.ToString());
            }
            cbaPreset.SelectedItem = device.Preset.ToString();
            baSave.Enabled = cbaPreset.SelectedItem == null ? false : true;
            tsmiDetails.Enabled = device.HasPreciseControl;
            CollapsedRGBOption = true;
        }

        public event Action UpCommandButtonEvent;
        public event Action DetailExecuteEvent;

        private void tsmiDetails_Click(object sender, EventArgs e)
        {
            if (DetailExecuteEvent != null)
                DetailExecuteEvent();
        }
        
        private void sendUpCommandButtonEvent()
        {
            if (UpCommandButtonEvent != null)
                UpCommandButtonEvent();
        }

        private void baCommandButton_MouseDown(object sender, MouseEventArgs e)
        {
            string cmd = (sender as ButtonAdv).Tag as string;
            if (!string.IsNullOrEmpty(cmd))
            {
                sendPushCommandButtonEvent(cmd, null);
            }
        }
        private void baCommandButton_MouseUp(object sender, MouseEventArgs e)
        {
            sendUpCommandButtonEvent();
        }

        private void cbaPreset_ActionSend(object sender, EventArgs e)
        {
            baSave.Enabled = cbaPreset.SelectedItem == null ? false : true;
            string cmd = (sender as Control).Tag as string;
            if (!string.IsNullOrEmpty(cmd))
            {
                int numPreset = Convert.ToInt32(cbaPreset.SelectedItem);
                if (numPreset > 0)
                    sendPushCommandButtonEvent(cmd, numPreset);
            }
        }
        
    }
}
