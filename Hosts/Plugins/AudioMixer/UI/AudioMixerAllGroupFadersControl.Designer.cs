namespace Hosts.Plugins.AudioMixer.UI
{
    partial class AudioMixerAllGroupFadersControl
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
            this.autoLabel1 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.gpDetail = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.scrollersFrame1 = new Syncfusion.Windows.Forms.ScrollersFrame(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // autoLabel1
            // 
            this.autoLabel1.AutoSize = false;
            this.autoLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.autoLabel1.DX = 0;
            this.autoLabel1.DY = 0;
            this.autoLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel1.Location = new System.Drawing.Point(0, 0);
            this.autoLabel1.Name = "autoLabel1";
            this.autoLabel1.Size = new System.Drawing.Size(210, 18);
            this.autoLabel1.TabIndex = 1;
            this.autoLabel1.Text = "Фейдеры";
            this.autoLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // gpDetail
            // 
            this.gpDetail.AutoScroll = true;
            this.gpDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.gpDetail.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gpDetail.BorderColor = System.Drawing.Color.Black;
            this.gpDetail.BorderSingle = System.Windows.Forms.ButtonBorderStyle.None;
            this.gpDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gpDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpDetail.IgnoreThemeBackground = true;
            this.gpDetail.Location = new System.Drawing.Point(0, 18);
            this.gpDetail.Name = "gpDetail";
            this.gpDetail.Size = new System.Drawing.Size(210, 167);
            this.gpDetail.TabIndex = 2;
            this.gpDetail.ThemesEnabled = true;
            // 
            // scrollersFrame1
            // 
            this.scrollersFrame1.AttachedTo = this.gpDetail;
            this.scrollersFrame1.HorizontalSmallChange = 10;
            this.scrollersFrame1.SizeGripperVisibility = Syncfusion.Windows.Forms.SizeGripperVisibility.Auto;
            this.scrollersFrame1.VerticallSmallChange = 10;
            this.scrollersFrame1.VisualStyle = Syncfusion.Windows.Forms.ScrollBarCustomDrawStyles.Office2007;
            // 
            // AudioMixerAllGroupFadersControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.Controls.Add(this.gpDetail);
            this.Controls.Add(this.autoLabel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.ForeColor = System.Drawing.Color.MidnightBlue;
            this.MinimumSize = new System.Drawing.Size(210, 185);
            this.Name = "AudioMixerAllGroupFadersControl";
            this.Size = new System.Drawing.Size(210, 185);
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel1;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gpDetail;
        private Syncfusion.Windows.Forms.ScrollersFrame scrollersFrame1;
    }
}
