namespace Hosts.Plugins.Jupiter.Player
{
    partial class JupiterPlayerControl
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
                SetControlPlayerTimerEnable(false, 1);
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
            this.baPictureMuteButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.gradientPanel1 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.autoLabel1 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.baPowerButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.alPowerState = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.gradientPanel2 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.alPictureMuteState = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.cbaBrightness = new Syncfusion.Windows.Forms.Tools.ComboBoxAdv();
            this.autoLabel4 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel5 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            ((System.ComponentModel.ISupportInitialize)(this.gpFillPanel)).BeginInit();
            this.gpFillPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).BeginInit();
            this.gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).BeginInit();
            this.gradientPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbaBrightness)).BeginInit();
            this.SuspendLayout();
            // 
            // gpFillPanel
            // 
            this.gpFillPanel.Controls.Add(this.gradientPanel2);
            this.gpFillPanel.Controls.Add(this.gradientPanel1);
            this.gpFillPanel.Size = new System.Drawing.Size(210, 92);
            // 
            // baPictureMuteButton
            // 
            this.baPictureMuteButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baPictureMuteButton.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.baPictureMuteButton.ForeColor = System.Drawing.Color.MidnightBlue;
            this.baPictureMuteButton.Location = new System.Drawing.Point(128, 3);
            this.baPictureMuteButton.Name = "baPictureMuteButton";
            this.baPictureMuteButton.PushButton = true;
            this.baPictureMuteButton.Size = new System.Drawing.Size(73, 23);
            this.baPictureMuteButton.TabIndex = 3;
            this.baPictureMuteButton.Tag = "SetPicMute";
            this.baPictureMuteButton.Text = "Выключить";
            this.baPictureMuteButton.UseVisualStyle = true;
            this.baPictureMuteButton.Click += new System.EventHandler(this.baPowerButton_Click);
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.gradientPanel1.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gradientPanel1.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel1.BorderSides = System.Windows.Forms.Border3DSide.Bottom;
            this.gradientPanel1.Controls.Add(this.autoLabel1);
            this.gradientPanel1.Controls.Add(this.baPowerButton);
            this.gradientPanel1.Controls.Add(this.alPowerState);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(210, 35);
            this.gradientPanel1.TabIndex = 5;
            // 
            // autoLabel1
            // 
            this.autoLabel1.DX = 0;
            this.autoLabel1.DY = 0;
            this.autoLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.autoLabel1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel1.Location = new System.Drawing.Point(1, 8);
            this.autoLabel1.Name = "autoLabel1";
            this.autoLabel1.Size = new System.Drawing.Size(67, 13);
            this.autoLabel1.TabIndex = 2;
            this.autoLabel1.Text = "Видеостена";
            // 
            // baPowerButton
            // 
            this.baPowerButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baPowerButton.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.baPowerButton.ForeColor = System.Drawing.Color.MidnightBlue;
            this.baPowerButton.Location = new System.Drawing.Point(128, 3);
            this.baPowerButton.Name = "baPowerButton";
            this.baPowerButton.PushButton = true;
            this.baPowerButton.Size = new System.Drawing.Size(73, 23);
            this.baPowerButton.TabIndex = 1;
            this.baPowerButton.Tag = "SetPower";
            this.baPowerButton.Text = "Выключить";
            this.baPowerButton.UseVisualStyle = true;
            this.baPowerButton.Click += new System.EventHandler(this.baPowerButton_Click);
            // 
            // alPowerState
            // 
            this.alPowerState.DX = 0;
            this.alPowerState.DY = 0;
            this.alPowerState.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.alPowerState.ForeColor = System.Drawing.Color.MidnightBlue;
            this.alPowerState.Location = new System.Drawing.Point(65, 8);
            this.alPowerState.Name = "alPowerState";
            this.alPowerState.Size = new System.Drawing.Size(66, 13);
            this.alPowerState.TabIndex = 0;
            this.alPowerState.Text = "выключена";
            // 
            // gradientPanel2
            // 
            this.gradientPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.gradientPanel2.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gradientPanel2.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel2.BorderSides = System.Windows.Forms.Border3DSide.Bottom;
            this.gradientPanel2.Controls.Add(this.alPictureMuteState);
            this.gradientPanel2.Controls.Add(this.cbaBrightness);
            this.gradientPanel2.Controls.Add(this.baPictureMuteButton);
            this.gradientPanel2.Controls.Add(this.autoLabel4);
            this.gradientPanel2.Controls.Add(this.autoLabel5);
            this.gradientPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientPanel2.Location = new System.Drawing.Point(0, 35);
            this.gradientPanel2.Name = "gradientPanel2";
            this.gradientPanel2.Size = new System.Drawing.Size(210, 57);
            this.gradientPanel2.TabIndex = 6;
            // 
            // alPictureMuteState
            // 
            this.alPictureMuteState.DX = 0;
            this.alPictureMuteState.DY = 0;
            this.alPictureMuteState.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.alPictureMuteState.ForeColor = System.Drawing.Color.MidnightBlue;
            this.alPictureMuteState.Location = new System.Drawing.Point(67, 8);
            this.alPictureMuteState.Name = "alPictureMuteState";
            this.alPictureMuteState.Size = new System.Drawing.Size(60, 13);
            this.alPictureMuteState.TabIndex = 5;
            this.alPictureMuteState.Text = "выключен";
            // 
            // cbaBrightness
            // 
            this.cbaBrightness.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.cbaBrightness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbaBrightness.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cbaBrightness.ForeColor = System.Drawing.Color.MidnightBlue;
            this.cbaBrightness.IgnoreThemeBackground = true;
            this.cbaBrightness.Items.AddRange(new object[] {
            "0",
            "20",
            "40",
            "60",
            "80",
            "100"});
            this.cbaBrightness.ItemsImageIndexes.Add(new Syncfusion.Windows.Forms.Tools.ComboBoxAdv.ImageIndexItem(this.cbaBrightness, "0"));
            this.cbaBrightness.ItemsImageIndexes.Add(new Syncfusion.Windows.Forms.Tools.ComboBoxAdv.ImageIndexItem(this.cbaBrightness, "20"));
            this.cbaBrightness.ItemsImageIndexes.Add(new Syncfusion.Windows.Forms.Tools.ComboBoxAdv.ImageIndexItem(this.cbaBrightness, "40"));
            this.cbaBrightness.ItemsImageIndexes.Add(new Syncfusion.Windows.Forms.Tools.ComboBoxAdv.ImageIndexItem(this.cbaBrightness, "60"));
            this.cbaBrightness.ItemsImageIndexes.Add(new Syncfusion.Windows.Forms.Tools.ComboBoxAdv.ImageIndexItem(this.cbaBrightness, "80"));
            this.cbaBrightness.ItemsImageIndexes.Add(new Syncfusion.Windows.Forms.Tools.ComboBoxAdv.ImageIndexItem(this.cbaBrightness, "100"));
            this.cbaBrightness.Location = new System.Drawing.Point(128, 30);
            this.cbaBrightness.Name = "cbaBrightness";
            this.cbaBrightness.Size = new System.Drawing.Size(73, 21);
            this.cbaBrightness.Style = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.cbaBrightness.TabIndex = 4;
            this.cbaBrightness.Tag = "SetBrightness";
            this.cbaBrightness.Text = "0";
            this.cbaBrightness.SelectedIndexChanged += new System.EventHandler(this.cbaBrightness_SelectedIndexChanged);
            // 
            // autoLabel4
            // 
            this.autoLabel4.DX = 0;
            this.autoLabel4.DY = 0;
            this.autoLabel4.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.autoLabel4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel4.Location = new System.Drawing.Point(1, 34);
            this.autoLabel4.Name = "autoLabel4";
            this.autoLabel4.Size = new System.Drawing.Size(49, 13);
            this.autoLabel4.TabIndex = 2;
            this.autoLabel4.Text = "Яркость";
            // 
            // autoLabel5
            // 
            this.autoLabel5.DX = 0;
            this.autoLabel5.DY = 0;
            this.autoLabel5.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.autoLabel5.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel5.Location = new System.Drawing.Point(1, 8);
            this.autoLabel5.Name = "autoLabel5";
            this.autoLabel5.Size = new System.Drawing.Size(67, 13);
            this.autoLabel5.TabIndex = 1;
            this.autoLabel5.Text = "Picture mute";
            // 
            // JupiterPlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MinimumSize = new System.Drawing.Size(210, 130);
            this.Name = "JupiterPlayerControl";
            this.Size = new System.Drawing.Size(210, 130);
            ((System.ComponentModel.ISupportInitialize)(this.gpFillPanel)).EndInit();
            this.gpFillPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).EndInit();
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).EndInit();
            this.gradientPanel2.ResumeLayout(false);
            this.gradientPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbaBrightness)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv baPictureMuteButton;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel1;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel1;
        private Syncfusion.Windows.Forms.ButtonAdv baPowerButton;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alPowerState;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel2;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alPictureMuteState;
        private Syncfusion.Windows.Forms.Tools.ComboBoxAdv cbaBrightness;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel4;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel5;
    }
}
