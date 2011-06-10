namespace Hosts.Plugins.VideoCamera.Player
{
    partial class VideoCameraPlayerControl
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
            this.gradientPanel2 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.cbaPreset = new Syncfusion.Windows.Forms.Tools.ComboBoxAdv();
            this.baHome = new Syncfusion.Windows.Forms.ButtonAdv();
            this.baSave = new Syncfusion.Windows.Forms.ButtonAdv();
            this.autoLabel11 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.gradientPanel4 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.itbPan = new Syncfusion.Windows.Forms.Tools.IntegerTextBox();
            this.autoLabel14 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel13 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.tbePan = new Syncfusion.Windows.Forms.Tools.TrackBarEx(0, 360);
            this.autoLabel12 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.gradientPanel5 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.itbTilt = new Syncfusion.Windows.Forms.Tools.IntegerTextBox();
            this.alTiltMax = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.alTiltMin = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.tbeTilt = new Syncfusion.Windows.Forms.Tools.TrackBarEx(-180, 180);
            this.autoLabel17 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.gradientPanel6 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.itbZoom = new Syncfusion.Windows.Forms.Tools.IntegerTextBox();
            this.alZoomMax = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.alZoomMin = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.tbeZoom = new Syncfusion.Windows.Forms.Tools.TrackBarEx(0, 30);
            this.autoLabel20 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).BeginInit();
            this.gpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).BeginInit();
            this.gradientPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbaPreset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel4)).BeginInit();
            this.gradientPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itbPan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel5)).BeginInit();
            this.gradientPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itbTilt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel6)).BeginInit();
            this.gradientPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itbZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // gpDetail
            // 
            this.gpDetail.Controls.Add(this.gradientPanel6);
            this.gpDetail.Controls.Add(this.gradientPanel5);
            this.gpDetail.Controls.Add(this.gradientPanel4);
            this.gpDetail.Controls.Add(this.gradientPanel2);
            this.gpDetail.Size = new System.Drawing.Size(210, 184);
            // 
            // gradientPanel2
            // 
            this.gradientPanel2.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gradientPanel2.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel2.BorderSides = System.Windows.Forms.Border3DSide.Bottom;
            this.gradientPanel2.Controls.Add(this.cbaPreset);
            this.gradientPanel2.Controls.Add(this.baHome);
            this.gradientPanel2.Controls.Add(this.baSave);
            this.gradientPanel2.Controls.Add(this.autoLabel11);
            this.gradientPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel2.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel2.Name = "gradientPanel2";
            this.gradientPanel2.Size = new System.Drawing.Size(206, 45);
            this.gradientPanel2.TabIndex = 4;
            // 
            // cbaPreset
            // 
            this.cbaPreset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.cbaPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbaPreset.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbaPreset.IgnoreThemeBackground = true;
            this.cbaPreset.Location = new System.Drawing.Point(3, 16);
            this.cbaPreset.Name = "cbaPreset";
            this.cbaPreset.Size = new System.Drawing.Size(67, 21);
            this.cbaPreset.Style = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.cbaPreset.TabIndex = 5;
            this.cbaPreset.Tag = "CamPresetLoad";
            this.cbaPreset.SelectedIndexChanged += new System.EventHandler(this.cbaPreset_ActionSend);
            // 
            // baHome
            // 
            this.baHome.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baHome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.baHome.ForeColor = System.Drawing.Color.MidnightBlue;
            this.baHome.Location = new System.Drawing.Point(150, 16);
            this.baHome.Name = "baHome";
            this.baHome.Size = new System.Drawing.Size(49, 21);
            this.baHome.TabIndex = 2;
            this.baHome.Tag = "CamHome";
            this.baHome.Text = "HOME";
            this.baHome.UseVisualStyle = true;
            this.baHome.Click += new System.EventHandler(this.controlSendHomeCommand_Click);
            // 
            // baSave
            // 
            this.baSave.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.baSave.ForeColor = System.Drawing.Color.MidnightBlue;
            this.baSave.Location = new System.Drawing.Point(76, 16);
            this.baSave.Name = "baSave";
            this.baSave.Size = new System.Drawing.Size(68, 21);
            this.baSave.TabIndex = 1;
            this.baSave.Tag = "CamPresetSave";
            this.baSave.Text = "Сохранить";
            this.baSave.UseVisualStyle = true;
            this.baSave.Click += new System.EventHandler(this.cbaPreset_ActionSend);
            // 
            // autoLabel11
            // 
            this.autoLabel11.DX = 0;
            this.autoLabel11.DY = 0;
            this.autoLabel11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel11.Location = new System.Drawing.Point(3, 0);
            this.autoLabel11.Name = "autoLabel11";
            this.autoLabel11.Size = new System.Drawing.Size(50, 13);
            this.autoLabel11.TabIndex = 4;
            this.autoLabel11.Text = "Пресет";
            // 
            // gradientPanel4
            // 
            this.gradientPanel4.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gradientPanel4.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel4.BorderSides = System.Windows.Forms.Border3DSide.Bottom;
            this.gradientPanel4.Controls.Add(this.itbPan);
            this.gradientPanel4.Controls.Add(this.autoLabel14);
            this.gradientPanel4.Controls.Add(this.autoLabel13);
            this.gradientPanel4.Controls.Add(this.tbePan);
            this.gradientPanel4.Controls.Add(this.autoLabel12);
            this.gradientPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel4.Location = new System.Drawing.Point(0, 45);
            this.gradientPanel4.Name = "gradientPanel4";
            this.gradientPanel4.Size = new System.Drawing.Size(206, 46);
            this.gradientPanel4.TabIndex = 5;
            // 
            // itbPan
            // 
            this.itbPan.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.itbPan.ForeColor = System.Drawing.Color.MidnightBlue;
            this.itbPan.IntegerValue = ((long)(0));
            this.itbPan.Location = new System.Drawing.Point(83, 1);
            this.itbPan.MaxValue = ((long)(360));
            this.itbPan.MinValue = ((long)(0));
            this.itbPan.Name = "itbPan";
            this.itbPan.NegativeColor = System.Drawing.Color.MidnightBlue;
            this.itbPan.NegativeInputPendingOnSelectAll = false;
            this.itbPan.NullString = "0";
            this.itbPan.OverflowIndicatorToolTipText = null;
            this.itbPan.PositiveColor = System.Drawing.Color.MidnightBlue;
            this.itbPan.Size = new System.Drawing.Size(30, 21);
            this.itbPan.TabIndex = 10;
            this.itbPan.Tag = "CamSetPos";
            this.itbPan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.itbPan.Leave += new System.EventHandler(this.itbControl_Leave);
            this.itbPan.IntegerValueChanged += new System.EventHandler(this.itbPan_IntegerValueChanged);
            this.itbPan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.itbControl_KeyPress);
            // 
            // autoLabel14
            // 
            this.autoLabel14.DX = 0;
            this.autoLabel14.DY = 0;
            this.autoLabel14.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel14.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel14.Location = new System.Drawing.Point(180, 24);
            this.autoLabel14.Name = "autoLabel14";
            this.autoLabel14.Size = new System.Drawing.Size(25, 13);
            this.autoLabel14.TabIndex = 9;
            this.autoLabel14.Text = "360";
            // 
            // autoLabel13
            // 
            this.autoLabel13.DX = 0;
            this.autoLabel13.DY = 0;
            this.autoLabel13.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel13.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel13.Location = new System.Drawing.Point(7, 24);
            this.autoLabel13.Name = "autoLabel13";
            this.autoLabel13.Size = new System.Drawing.Size(13, 13);
            this.autoLabel13.TabIndex = 8;
            this.autoLabel13.Text = "0";
            // 
            // tbePan
            // 
            this.tbePan.Location = new System.Drawing.Point(26, 21);
            this.tbePan.Name = "tbePan";
            this.tbePan.Size = new System.Drawing.Size(150, 20);
            this.tbePan.TabIndex = 4;
            this.tbePan.Tag = "CamSetPos";
            this.tbePan.Text = "trackBarEx1";
            this.tbePan.TimerInterval = 100;
            this.tbePan.Transparent = true;
            this.tbePan.Value = 0;
            this.tbePan.ValueChanged += new System.EventHandler(this.tbPan_ValueChanged);
            this.tbePan.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbeControl_MouseUp);
            // 
            // autoLabel12
            // 
            this.autoLabel12.DX = 0;
            this.autoLabel12.DY = 0;
            this.autoLabel12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel12.Location = new System.Drawing.Point(3, 1);
            this.autoLabel12.Name = "autoLabel12";
            this.autoLabel12.Size = new System.Drawing.Size(29, 13);
            this.autoLabel12.TabIndex = 5;
            this.autoLabel12.Text = "Pan";
            // 
            // gradientPanel5
            // 
            this.gradientPanel5.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gradientPanel5.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel5.BorderSides = System.Windows.Forms.Border3DSide.Bottom;
            this.gradientPanel5.Controls.Add(this.itbTilt);
            this.gradientPanel5.Controls.Add(this.alTiltMax);
            this.gradientPanel5.Controls.Add(this.alTiltMin);
            this.gradientPanel5.Controls.Add(this.tbeTilt);
            this.gradientPanel5.Controls.Add(this.autoLabel17);
            this.gradientPanel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel5.Location = new System.Drawing.Point(0, 91);
            this.gradientPanel5.Name = "gradientPanel5";
            this.gradientPanel5.Size = new System.Drawing.Size(206, 46);
            this.gradientPanel5.TabIndex = 6;
            // 
            // itbTilt
            // 
            this.itbTilt.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.itbTilt.ForeColor = System.Drawing.Color.MidnightBlue;
            this.itbTilt.IntegerValue = ((long)(0));
            this.itbTilt.Location = new System.Drawing.Point(83, 1);
            this.itbTilt.MaxValue = ((long)(180));
            this.itbTilt.MinValue = ((long)(-180));
            this.itbTilt.Name = "itbTilt";
            this.itbTilt.NegativeColor = System.Drawing.Color.MidnightBlue;
            this.itbTilt.NegativeInputPendingOnSelectAll = false;
            this.itbTilt.NullString = "0";
            this.itbTilt.OverflowIndicatorToolTipText = null;
            this.itbTilt.PositiveColor = System.Drawing.Color.MidnightBlue;
            this.itbTilt.Size = new System.Drawing.Size(30, 21);
            this.itbTilt.TabIndex = 11;
            this.itbTilt.Tag = "CamSetPos";
            this.itbTilt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.itbTilt.Leave += new System.EventHandler(this.itbControl_Leave);
            this.itbTilt.IntegerValueChanged += new System.EventHandler(this.itbTilt_IntegerValueChanged);
            this.itbTilt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.itbControl_KeyPress);
            // 
            // alTiltMax
            // 
            this.alTiltMax.DX = 0;
            this.alTiltMax.DY = 0;
            this.alTiltMax.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alTiltMax.ForeColor = System.Drawing.Color.MidnightBlue;
            this.alTiltMax.Location = new System.Drawing.Point(174, 24);
            this.alTiltMax.Name = "alTiltMax";
            this.alTiltMax.Size = new System.Drawing.Size(33, 13);
            this.alTiltMax.TabIndex = 9;
            this.alTiltMax.Text = "+180";
            // 
            // alTiltMin
            // 
            this.alTiltMin.DX = 0;
            this.alTiltMin.DY = 0;
            this.alTiltMin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alTiltMin.ForeColor = System.Drawing.Color.MidnightBlue;
            this.alTiltMin.Location = new System.Drawing.Point(-2, 24);
            this.alTiltMin.Name = "alTiltMin";
            this.alTiltMin.Size = new System.Drawing.Size(29, 13);
            this.alTiltMin.TabIndex = 8;
            this.alTiltMin.Text = "-180";
            // 
            // tbeTilt
            // 
            this.tbeTilt.Location = new System.Drawing.Point(25, 21);
            this.tbeTilt.Name = "tbeTilt";
            this.tbeTilt.Size = new System.Drawing.Size(150, 20);
            this.tbeTilt.TabIndex = 6;
            this.tbeTilt.Tag = "CamSetPos";
            this.tbeTilt.Text = "trackBarEx2";
            this.tbeTilt.TimerInterval = 100;
            this.tbeTilt.Transparent = true;
            this.tbeTilt.Value = 0;
            this.tbeTilt.ValueChanged += new System.EventHandler(this.tbTilt_ValueChanged);
            this.tbeTilt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbeControl_MouseUp);
            // 
            // autoLabel17
            // 
            this.autoLabel17.DX = 0;
            this.autoLabel17.DY = 0;
            this.autoLabel17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel17.Location = new System.Drawing.Point(0, 1);
            this.autoLabel17.Name = "autoLabel17";
            this.autoLabel17.Size = new System.Drawing.Size(25, 13);
            this.autoLabel17.TabIndex = 5;
            this.autoLabel17.Text = "Tilt";
            // 
            // gradientPanel6
            // 
            this.gradientPanel6.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gradientPanel6.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel6.BorderSides = System.Windows.Forms.Border3DSide.Bottom;
            this.gradientPanel6.Controls.Add(this.itbZoom);
            this.gradientPanel6.Controls.Add(this.alZoomMax);
            this.gradientPanel6.Controls.Add(this.alZoomMin);
            this.gradientPanel6.Controls.Add(this.tbeZoom);
            this.gradientPanel6.Controls.Add(this.autoLabel20);
            this.gradientPanel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel6.Location = new System.Drawing.Point(0, 137);
            this.gradientPanel6.Name = "gradientPanel6";
            this.gradientPanel6.Size = new System.Drawing.Size(206, 46);
            this.gradientPanel6.TabIndex = 7;
            // 
            // itbZoom
            // 
            this.itbZoom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.itbZoom.ForeColor = System.Drawing.Color.MidnightBlue;
            this.itbZoom.IntegerValue = ((long)(0));
            this.itbZoom.Location = new System.Drawing.Point(83, 1);
            this.itbZoom.MaxValue = ((long)(30));
            this.itbZoom.MinValue = ((long)(0));
            this.itbZoom.Name = "itbZoom";
            this.itbZoom.NegativeColor = System.Drawing.Color.MidnightBlue;
            this.itbZoom.NegativeInputPendingOnSelectAll = false;
            this.itbZoom.NullString = "0";
            this.itbZoom.OverflowIndicatorToolTipText = null;
            this.itbZoom.PositiveColor = System.Drawing.Color.MidnightBlue;
            this.itbZoom.Size = new System.Drawing.Size(30, 21);
            this.itbZoom.TabIndex = 11;
            this.itbZoom.Tag = "CamSetPos";
            this.itbZoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.itbZoom.Leave += new System.EventHandler(this.itbControl_Leave);
            this.itbZoom.IntegerValueChanged += new System.EventHandler(this.itbZoom_IntegerValueChanged);
            this.itbZoom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.itbControl_KeyPress);
            // 
            // alZoomMax
            // 
            this.alZoomMax.DX = 0;
            this.alZoomMax.DY = 0;
            this.alZoomMax.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alZoomMax.ForeColor = System.Drawing.Color.MidnightBlue;
            this.alZoomMax.Location = new System.Drawing.Point(185, 24);
            this.alZoomMax.Name = "alZoomMax";
            this.alZoomMax.Size = new System.Drawing.Size(19, 13);
            this.alZoomMax.TabIndex = 9;
            this.alZoomMax.Text = "30";
            // 
            // alZoomMin
            // 
            this.alZoomMin.DX = 0;
            this.alZoomMin.DY = 0;
            this.alZoomMin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alZoomMin.ForeColor = System.Drawing.Color.MidnightBlue;
            this.alZoomMin.Location = new System.Drawing.Point(7, 24);
            this.alZoomMin.Name = "alZoomMin";
            this.alZoomMin.Size = new System.Drawing.Size(13, 13);
            this.alZoomMin.TabIndex = 8;
            this.alZoomMin.Text = "0";
            // 
            // tbeZoom
            // 
            this.tbeZoom.Location = new System.Drawing.Point(26, 21);
            this.tbeZoom.Name = "tbeZoom";
            this.tbeZoom.Size = new System.Drawing.Size(150, 20);
            this.tbeZoom.TabIndex = 8;
            this.tbeZoom.Tag = "CamSetPos";
            this.tbeZoom.Text = "trackBarEx3";
            this.tbeZoom.TimerInterval = 100;
            this.tbeZoom.Transparent = true;
            this.tbeZoom.Value = 0;
            this.tbeZoom.ValueChanged += new System.EventHandler(this.tbZoom_ValueChanged);
            this.tbeZoom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbeControl_MouseUp);
            // 
            // autoLabel20
            // 
            this.autoLabel20.DX = 0;
            this.autoLabel20.DY = 0;
            this.autoLabel20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel20.Location = new System.Drawing.Point(3, 1);
            this.autoLabel20.Name = "autoLabel20";
            this.autoLabel20.Size = new System.Drawing.Size(38, 13);
            this.autoLabel20.TabIndex = 5;
            this.autoLabel20.Text = "Zoom";
            // 
            // VideoCameraPlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "VideoCameraPlayerControl";
            this.Size = new System.Drawing.Size(216, 343);
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).EndInit();
            this.gpDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).EndInit();
            this.gradientPanel2.ResumeLayout(false);
            this.gradientPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbaPreset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel4)).EndInit();
            this.gradientPanel4.ResumeLayout(false);
            this.gradientPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itbPan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel5)).EndInit();
            this.gradientPanel5.ResumeLayout(false);
            this.gradientPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itbTilt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel6)).EndInit();
            this.gradientPanel6.ResumeLayout(false);
            this.gradientPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itbZoom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel2;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel4;
        private Syncfusion.Windows.Forms.ButtonAdv baHome;
        private Syncfusion.Windows.Forms.ButtonAdv baSave;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel11;
        private Syncfusion.Windows.Forms.Tools.TrackBarEx tbePan;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel12;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel14;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel13;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel6;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alZoomMax;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alZoomMin;
        private Syncfusion.Windows.Forms.Tools.TrackBarEx tbeZoom;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel20;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel5;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alTiltMax;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alTiltMin;
        private Syncfusion.Windows.Forms.Tools.TrackBarEx tbeTilt;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel17;
        private Syncfusion.Windows.Forms.Tools.ComboBoxAdv cbaPreset;
        private Syncfusion.Windows.Forms.Tools.IntegerTextBox itbPan;
        private Syncfusion.Windows.Forms.Tools.IntegerTextBox itbZoom;
        private Syncfusion.Windows.Forms.Tools.IntegerTextBox itbTilt;
    }
}
