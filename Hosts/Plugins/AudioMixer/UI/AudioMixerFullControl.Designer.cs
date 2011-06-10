namespace Hosts.Plugins.AudioMixer.UI
{
    partial class AudioMixerFullControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.audioMixerAllGroupFadersControl1 = new Hosts.Plugins.AudioMixer.UI.AudioMixerAllGroupFadersControl();
            this.audioMixerMatrixControl1 = new Hosts.Plugins.AudioMixer.UI.AudioMixerMatrixControl();
            this.SuspendLayout();
            // 
            // audioMixerAllGroupFadersControl1
            // 
            this.audioMixerAllGroupFadersControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.audioMixerAllGroupFadersControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioMixerAllGroupFadersControl1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.audioMixerAllGroupFadersControl1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.audioMixerAllGroupFadersControl1.Location = new System.Drawing.Point(0, 150);
            this.audioMixerAllGroupFadersControl1.MinimumSize = new System.Drawing.Size(130, 185);
            this.audioMixerAllGroupFadersControl1.Name = "audioMixerAllGroupFadersControl1";
            this.audioMixerAllGroupFadersControl1.Size = new System.Drawing.Size(130, 185);
            this.audioMixerAllGroupFadersControl1.TabIndex = 1;
            // 
            // audioMixerMatrixControl1
            // 
            this.audioMixerMatrixControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.audioMixerMatrixControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.audioMixerMatrixControl1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.audioMixerMatrixControl1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.audioMixerMatrixControl1.Location = new System.Drawing.Point(0, 0);
            this.audioMixerMatrixControl1.Margin = new System.Windows.Forms.Padding(0);
            this.audioMixerMatrixControl1.MaximumSize = new System.Drawing.Size(440, 260);
            this.audioMixerMatrixControl1.MinimumSize = new System.Drawing.Size(130, 50);
            this.audioMixerMatrixControl1.Name = "audioMixerMatrixControl1";
            this.audioMixerMatrixControl1.Size = new System.Drawing.Size(130, 150);
            this.audioMixerMatrixControl1.TabIndex = 0;
            // 
            // AudioMixerFullControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.audioMixerAllGroupFadersControl1);
            this.Controls.Add(this.audioMixerMatrixControl1);
            this.MinimumSize = new System.Drawing.Size(130, 335);
            this.Name = "AudioMixerFullControl";
            this.Size = new System.Drawing.Size(130, 335);
            this.ResumeLayout(false);

        }

        #endregion

        private AudioMixerMatrixControl audioMixerMatrixControl1;
        private AudioMixerAllGroupFadersControl audioMixerAllGroupFadersControl1;
    }
}
