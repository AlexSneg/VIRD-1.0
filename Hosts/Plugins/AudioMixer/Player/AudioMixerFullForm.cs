using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using Hosts.Plugins.AudioMixer.UI;

namespace Hosts.Plugins.AudioMixer.Player
{
    public partial class AudioMixerFullForm : Office2007Form
    {
        public AudioMixerFullForm()
        {
            InitializeComponent();
        }
        public void ShowModal()
        {
            ShowDialog();
        }
        public IAudioMixerFullView AudioMixerFullView
        {
            get { return audioMixerFullView; }
        }
        
    }
}
