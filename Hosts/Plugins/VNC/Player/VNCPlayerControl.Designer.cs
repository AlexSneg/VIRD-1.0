namespace Hosts.Plugins.VNC.Player
{
    partial class VNCPlayerControl
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
            this.components = new System.ComponentModel.Container();
            this.statusLabel = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.controlButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.DX = 0;
            this.statusLabel.DY = 0;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.statusLabel.Location = new System.Drawing.Point(4, 9);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(68, 13);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "Connected";
            // 
            // controlButton
            // 
            this.controlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.WindowsXP;
            this.controlButton.Location = new System.Drawing.Point(92, 4);
            this.controlButton.Name = "controlButton";
            this.controlButton.Size = new System.Drawing.Size(75, 23);
            this.controlButton.TabIndex = 1;
            this.controlButton.Text = "Disconnect";
            this.controlButton.Click += new System.EventHandler(this.controlButton_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // VNCPlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.controlButton);
            this.Controls.Add(this.statusLabel);
            this.MaximumSize = new System.Drawing.Size(0, 31);
            this.MinimumSize = new System.Drawing.Size(172, 31);
            this.Name = "VNCPlayerControl";
            this.Size = new System.Drawing.Size(172, 31);
            this.Load += new System.EventHandler(this.VNCPlayerControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.AutoLabel statusLabel;
        private Syncfusion.Windows.Forms.ButtonAdv controlButton;
        private System.Windows.Forms.Timer timer1;
    }
}
