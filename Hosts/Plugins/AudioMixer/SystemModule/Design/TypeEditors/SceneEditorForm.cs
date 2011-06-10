using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hosts.Plugins.AudioMixer.UI;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.AudioMixer.SystemModule.Design.TypeEditors
{
    public partial class SceneEditorForm : Form
    {
        public IAudioMixerFullView view
        {
            get
            {
                return audioMixerFullControl;
            }
        }
        
        public SceneEditorForm(int inputListCount, int fadersCount)
        {
            InitializeComponent();
            this.Width = view.GetWidthControl(inputListCount, fadersCount);
        }

        public SceneEditorForm()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
