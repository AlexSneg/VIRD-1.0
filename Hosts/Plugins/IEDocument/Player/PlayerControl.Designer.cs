namespace Hosts.Plugins.IEDocument.Player
{
    partial class PlayerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerControl));
            this.lbInfo = new System.Windows.Forms.Label();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tbeZoom = new Syncfusion.Windows.Forms.Tools.TrackBarEx(0, 10);
            this.itbZoom = new Syncfusion.Windows.Forms.Tools.IntegerTextBox();
            this.alZoom = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.alZoomMin = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.alZoomMax = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.itbZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Location = new System.Drawing.Point(8, 19);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(0, 13);
            this.lbInfo.TabIndex = 0;
            // 
            // btnUp
            // 
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.Location = new System.Drawing.Point(91, 67);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(25, 25);
            this.btnUp.TabIndex = 1;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.Location = new System.Drawing.Point(91, 117);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(25, 25);
            this.btnDown.TabIndex = 2;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tbeZoom
            // 
            this.tbeZoom.ButtonColor = System.Drawing.SystemColors.Control;
            this.tbeZoom.Location = new System.Drawing.Point(28, 38);
            this.tbeZoom.Name = "tbeZoom";
            this.tbeZoom.Size = new System.Drawing.Size(152, 20);
            this.tbeZoom.TabIndex = 14;
            this.tbeZoom.TimerInterval = 100;
            this.tbeZoom.TrackBarGradientEnd = System.Drawing.SystemColors.ControlDark;
            this.tbeZoom.TrackBarGradientStart = System.Drawing.SystemColors.Control;
            this.tbeZoom.Value = 5;
            this.tbeZoom.ValueChanged += new System.EventHandler(this.tbeZoom_ValueChanged);
            this.tbeZoom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbeZoom_MouseUp);
            // 
            // itbZoom
            // 
            this.itbZoom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.itbZoom.ForeColor = System.Drawing.Color.MidnightBlue;
            this.itbZoom.IntegerValue = ((long)(10));
            this.itbZoom.Location = new System.Drawing.Point(88, 10);
            this.itbZoom.MaxValue = ((long)(30));
            this.itbZoom.MinValue = ((long)(0));
            this.itbZoom.Name = "itbZoom";
            this.itbZoom.NegativeColor = System.Drawing.Color.MidnightBlue;
            this.itbZoom.NegativeInputPendingOnSelectAll = false;
            this.itbZoom.NullString = "0";
            this.itbZoom.OverflowIndicatorToolTipText = null;
            this.itbZoom.PositiveColor = System.Drawing.Color.MidnightBlue;
            this.itbZoom.Size = new System.Drawing.Size(34, 21);
            this.itbZoom.TabIndex = 15;
            this.itbZoom.Tag = "CamSetPos";
            this.itbZoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.itbZoom.Leave += new System.EventHandler(this.itbZoom_Leave);
            this.itbZoom.IntegerValueChanged += new System.EventHandler(this.itbZoom_IntegerValueChanged);
            this.itbZoom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.itbZoom_KeyPress);
            // 
            // alZoom
            // 
            this.alZoom.DX = 0;
            this.alZoom.DY = 0;
            this.alZoom.Location = new System.Drawing.Point(48, 18);
            this.alZoom.Name = "alZoom";
            this.alZoom.Size = new System.Drawing.Size(34, 13);
            this.alZoom.TabIndex = 18;
            this.alZoom.Text = "Zoom";
            // 
            // alZoomMin
            // 
            this.alZoomMin.DX = 0;
            this.alZoomMin.DY = 0;
            this.alZoomMin.Location = new System.Drawing.Point(6, 41);
            this.alZoomMin.Name = "alZoomMin";
            this.alZoomMin.Size = new System.Drawing.Size(19, 13);
            this.alZoomMin.TabIndex = 19;
            this.alZoomMin.Text = "10";
            // 
            // alZoomMax
            // 
            this.alZoomMax.DX = 0;
            this.alZoomMax.DY = 0;
            this.alZoomMax.Location = new System.Drawing.Point(182, 41);
            this.alZoomMax.Name = "alZoomMax";
            this.alZoomMax.Size = new System.Drawing.Size(25, 13);
            this.alZoomMax.TabIndex = 20;
            this.alZoomMax.Text = "500";
            // 
            // btnLeft
            // 
            this.btnLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnLeft.Image")));
            this.btnLeft.Location = new System.Drawing.Point(66, 92);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(25, 25);
            this.btnLeft.TabIndex = 21;
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnRight
            // 
            this.btnRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRight.Image")));
            this.btnRight.Location = new System.Drawing.Point(116, 92);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(25, 25);
            this.btnRight.TabIndex = 22;
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // PlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.alZoomMax);
            this.Controls.Add(this.alZoomMin);
            this.Controls.Add(this.alZoom);
            this.Controls.Add(this.itbZoom);
            this.Controls.Add(this.tbeZoom);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.lbInfo);
            this.Name = "PlayerControl";
            this.Size = new System.Drawing.Size(222, 148);
            ((System.ComponentModel.ISupportInitialize)(this.itbZoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Timer timer1;
        private Syncfusion.Windows.Forms.Tools.TrackBarEx tbeZoom;
        private Syncfusion.Windows.Forms.Tools.IntegerTextBox itbZoom;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alZoom;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alZoomMin;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alZoomMax;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;

    }
}
