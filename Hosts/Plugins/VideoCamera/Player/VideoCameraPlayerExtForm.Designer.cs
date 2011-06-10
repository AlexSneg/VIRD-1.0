namespace Hosts.Plugins.VideoCamera.Player
{
    partial class VideoCameraPlayerExtForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoCameraPlayerExtForm));
            this.videoCameraPlayerControl1 = new Hosts.Plugins.VideoCamera.Player.VideoCameraPlayerControl();
            this.SuspendLayout();
            // 
            // videoCameraPlayerControl1
            // 
            this.videoCameraPlayerControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.videoCameraPlayerControl1.CollapsedRGBOption = false;
            this.videoCameraPlayerControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.videoCameraPlayerControl1.Location = new System.Drawing.Point(0, 0);
            this.videoCameraPlayerControl1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.videoCameraPlayerControl1.Name = "videoCameraPlayerControl1";
            this.videoCameraPlayerControl1.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.videoCameraPlayerControl1.Size = new System.Drawing.Size(217, 345);
            this.videoCameraPlayerControl1.TabIndex = 0;
            // 
            // VideoCameraPlayerExtForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CaptionFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ClientSize = new System.Drawing.Size(217, 343);
            this.ColorScheme = Syncfusion.Windows.Forms.Office2007Theme.Blue;
            this.Controls.Add(this.videoCameraPlayerControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VideoCameraPlayerExtForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Устройство - Управление";
            this.UseOffice2007SchemeBackColor = true;
            this.ResumeLayout(false);

        }

        #endregion

        internal VideoCameraPlayerControl videoCameraPlayerControl1;
    }
}