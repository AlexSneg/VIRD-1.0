namespace Hosts.Plugins.AudioMixer.UI
{
    partial class AudioMixerFaderControl
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
            this.cbMute = new Syncfusion.Windows.Forms.Tools.CheckBoxAdv();
            this.alMaxValue = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.alMinValue = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.tbeTrack = new Syncfusion.Windows.Forms.Tools.TrackBarEx(0, 10);
            this.itbCurrentValue = new Syncfusion.Windows.Forms.Tools.IntegerTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.cbMute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itbCurrentValue)).BeginInit();
            this.SuspendLayout();
            // 
            // alName
            // 
            this.alName.AutoSize = false;
            this.alName.DX = 0;
            this.alName.DY = 0;
            this.alName.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alName.Location = new System.Drawing.Point(0, 0);
            this.alName.Name = "alName";
            this.alName.Size = new System.Drawing.Size(32, 13);
            this.alName.TabIndex = 0;
            this.alName.Text = "Тембр";
            this.alName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbMute
            // 
            this.cbMute.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.cbMute.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.cbMute.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbMute.GradientEnd = System.Drawing.SystemColors.ControlDark;
            this.cbMute.GradientStart = System.Drawing.SystemColors.Control;
            this.cbMute.HotBorderColor = System.Drawing.SystemColors.WindowFrame;
            this.cbMute.ImageCheckBoxSize = new System.Drawing.Size(13, 13);
            this.cbMute.Location = new System.Drawing.Point(7, 12);
            this.cbMute.Margin = new System.Windows.Forms.Padding(0);
            this.cbMute.Name = "cbMute";
            this.cbMute.ShadowColor = System.Drawing.Color.Black;
            this.cbMute.ShadowOffset = new System.Drawing.Point(0, 0);
            this.cbMute.Size = new System.Drawing.Size(15, 13);
            this.cbMute.Style = Syncfusion.Windows.Forms.Tools.CheckBoxAdvStyle.Office2007;
            this.cbMute.TabIndex = 2;
            this.cbMute.Tag = "MixerSetFader";
            this.cbMute.ThemesEnabled = true;
            this.cbMute.CheckedChanged += new Syncfusion.Windows.Forms.Tools.CheckedChangedEventHandler(this.cbMute_CheckedChanged);
            // 
            // alMaxValue
            // 
            this.alMaxValue.AutoSize = false;
            this.alMaxValue.DX = 0;
            this.alMaxValue.DY = 0;
            this.alMaxValue.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alMaxValue.Location = new System.Drawing.Point(0, 25);
            this.alMaxValue.Name = "alMaxValue";
            this.alMaxValue.Size = new System.Drawing.Size(32, 11);
            this.alMaxValue.TabIndex = 3;
            this.alMaxValue.Text = "15";
            this.alMaxValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // alMinValue
            // 
            this.alMinValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.alMinValue.AutoSize = false;
            this.alMinValue.DX = 0;
            this.alMinValue.DY = 0;
            this.alMinValue.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alMinValue.Location = new System.Drawing.Point(0, 125);
            this.alMinValue.Name = "alMinValue";
            this.alMinValue.Size = new System.Drawing.Size(32, 11);
            this.alMinValue.TabIndex = 4;
            this.alMinValue.Text = "-60";
            this.alMinValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbeTrack
            // 
            this.tbeTrack.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(141)))), ((int)(((byte)(189)))));
            this.tbeTrack.ChannelHeight = 5;
            this.tbeTrack.DecreaseButtonSize = new System.Drawing.Size(0, 0);
            this.tbeTrack.IncreaseButtonSize = new System.Drawing.Size(0, 0);
            this.tbeTrack.Location = new System.Drawing.Point(6, 35);
            this.tbeTrack.Margin = new System.Windows.Forms.Padding(0);
            this.tbeTrack.Name = "tbeTrack";
            this.tbeTrack.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbeTrack.ShowButtons = false;
            this.tbeTrack.Size = new System.Drawing.Size(20, 90);
            this.tbeTrack.TabIndex = 5;
            this.tbeTrack.Tag = "";
            this.tbeTrack.Text = "trackBarEx1";
            this.tbeTrack.TimerInterval = 100;
            this.tbeTrack.Transparent = true;
            this.tbeTrack.Value = 0;
            this.tbeTrack.ValueChanged += new System.EventHandler(this.tbeTrack_ValueChanged);
            this.tbeTrack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbeTrack_MouseUp);
            this.tbeTrack.EnabledChanged += new System.EventHandler(this.tbeTrack_EnabledChanged);
            // 
            // itbCurrentValue
            // 
            this.itbCurrentValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.itbCurrentValue.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.itbCurrentValue.IntegerValue = ((long)(188));
            this.itbCurrentValue.Location = new System.Drawing.Point(3, 136);
            this.itbCurrentValue.Name = "itbCurrentValue";
            this.itbCurrentValue.NegativeColor = System.Drawing.SystemColors.ControlText;
            this.itbCurrentValue.NegativeInputPendingOnSelectAll = false;
            this.itbCurrentValue.NullString = "0";
            this.itbCurrentValue.OverflowIndicatorToolTipText = null;
            this.itbCurrentValue.Size = new System.Drawing.Size(25, 18);
            this.itbCurrentValue.TabIndex = 6;
            this.itbCurrentValue.Tag = "MixerSetFader";
            this.itbCurrentValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.itbCurrentValue.Leave += new System.EventHandler(this.itbCurrentValue_Leave);
            this.itbCurrentValue.IntegerValueChanged += new System.EventHandler(this.itbCurrentValue_IntegerValueChanged);
            this.itbCurrentValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.itbCurrentValue_KeyPress);
            // 
            // AudioMixerFaderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.Controls.Add(this.itbCurrentValue);
            this.Controls.Add(this.tbeTrack);
            this.Controls.Add(this.alMinValue);
            this.Controls.Add(this.alMaxValue);
            this.Controls.Add(this.cbMute);
            this.Controls.Add(this.alName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.ForeColor = System.Drawing.Color.MidnightBlue;
            this.MinimumSize = new System.Drawing.Size(32, 155);
            this.Name = "AudioMixerFaderControl";
            this.Size = new System.Drawing.Size(32, 155);
            this.Resize += new System.EventHandler(this.AudioMixerFaderControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.cbMute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.itbCurrentValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.AutoLabel alName;
        private Syncfusion.Windows.Forms.Tools.CheckBoxAdv cbMute;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alMaxValue;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alMinValue;
        private Syncfusion.Windows.Forms.Tools.TrackBarEx tbeTrack;
        private Syncfusion.Windows.Forms.Tools.IntegerTextBox itbCurrentValue;
    }
}
