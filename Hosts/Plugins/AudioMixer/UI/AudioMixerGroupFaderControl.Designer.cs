namespace Hosts.Plugins.AudioMixer.UI
{
    partial class AudioMixerGroupFaderControl
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
            this.alName = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.SuspendLayout();
            // 
            // alName
            // 
            this.alName.AutoSize = false;
            this.alName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.alName.DX = 0;
            this.alName.DY = 0;
            this.alName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alName.Location = new System.Drawing.Point(0, 132);
            this.alName.Name = "alName";
            this.alName.Size = new System.Drawing.Size(102, 18);
            this.alName.TabIndex = 2;
            this.alName.Text = "Имя группы";
            this.alName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AudioMixerGroupFaderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.Controls.Add(this.alName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.ForeColor = System.Drawing.Color.MidnightBlue;
            this.Name = "AudioMixerGroupFaderControl";
            this.Size = new System.Drawing.Size(102, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.AutoLabel alName;
    }
}
