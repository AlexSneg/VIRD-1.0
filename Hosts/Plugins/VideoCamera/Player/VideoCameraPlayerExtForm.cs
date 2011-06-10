using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;

namespace Hosts.Plugins.VideoCamera.Player
{
    public partial class VideoCameraPlayerExtForm : Office2007Form
    {
        public VideoCameraPlayerExtForm()
        {
            InitializeComponent();
        }
        public VideoCameraPlayerExtForm(string sourceName) : this()
        {
            this.Text = sourceName;
        }
    }
}
