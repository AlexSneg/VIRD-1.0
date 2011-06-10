namespace Hosts.Plugins.AudioMixer.Player
{
    partial class AudioMixerFullForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioMixerFullForm));
            this.audioMixerFullView = new Hosts.Plugins.AudioMixer.UI.AudioMixerFullControl();
            this.SuspendLayout();
            // 
            // audioMixerFullView
            // 
            this.audioMixerFullView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioMixerFullView.Location = new System.Drawing.Point(0, 0);
            this.audioMixerFullView.MinimumSize = new System.Drawing.Size(130, 335);
            this.audioMixerFullView.Name = "audioMixerFullView";
            this.audioMixerFullView.Size = new System.Drawing.Size(218, 439);
            this.audioMixerFullView.TabIndex = 0;
            // 
            // AudioMixerFullForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 439);
            this.Controls.Add(this.audioMixerFullView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AudioMixerFullForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.UseOffice2007SchemeBackColor = true;
            this.ResumeLayout(false);

        }

        #endregion

        private Hosts.Plugins.AudioMixer.UI.AudioMixerFullControl audioMixerFullView;
    }
}