namespace Hosts.Plugins.VideoCamera.Player
{
    partial class VideoCameraPlayerCommonControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoCameraPlayerCommonControl));
            this.gradientPanel2 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.cbaPreset = new Syncfusion.Windows.Forms.Tools.ComboBoxAdv();
            this.baSave = new Syncfusion.Windows.Forms.ButtonAdv();
            this.autoLabel11 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.gradientPanel4 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.baPanRight = new Syncfusion.Windows.Forms.ButtonAdv();
            this.baTiltUp = new Syncfusion.Windows.Forms.ButtonAdv();
            this.baHome = new Syncfusion.Windows.Forms.ButtonAdv();
            this.baTiltDown = new Syncfusion.Windows.Forms.ButtonAdv();
            this.baPanLeft = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv8 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv6 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.cmsDetails = new Syncfusion.Windows.Forms.Tools.ContextMenuStripEx();
            this.tsmiDetails = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).BeginInit();
            this.gpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).BeginInit();
            this.gradientPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbaPreset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel4)).BeginInit();
            this.gradientPanel4.SuspendLayout();
            this.cmsDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpDetail
            // 
            this.gpDetail.ContextMenuStrip = this.cmsDetails;
            this.gpDetail.Controls.Add(this.gradientPanel4);
            this.gpDetail.Controls.Add(this.buttonAdv8);
            this.gpDetail.Controls.Add(this.buttonAdv6);
            this.gpDetail.Controls.Add(this.gradientPanel2);
            this.gpDetail.Size = new System.Drawing.Size(210, 242);
            // 
            // gradientPanel2
            // 
            this.gradientPanel2.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gradientPanel2.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel2.BorderSides = System.Windows.Forms.Border3DSide.Bottom;
            this.gradientPanel2.Controls.Add(this.cbaPreset);
            this.gradientPanel2.Controls.Add(this.baSave);
            this.gradientPanel2.Controls.Add(this.autoLabel11);
            this.gradientPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel2.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel2.Name = "gradientPanel2";
            this.gradientPanel2.Size = new System.Drawing.Size(206, 45);
            this.gradientPanel2.TabIndex = 5;
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
            this.gradientPanel4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gradientPanel4.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gradientPanel4.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gradientPanel4.Controls.Add(this.baPanRight);
            this.gradientPanel4.Controls.Add(this.baTiltUp);
            this.gradientPanel4.Controls.Add(this.baHome);
            this.gradientPanel4.Controls.Add(this.baTiltDown);
            this.gradientPanel4.Controls.Add(this.baPanLeft);
            this.gradientPanel4.Location = new System.Drawing.Point(38, 51);
            this.gradientPanel4.Name = "gradientPanel4";
            this.gradientPanel4.Size = new System.Drawing.Size(130, 130);
            this.gradientPanel4.TabIndex = 14;
            // 
            // baPanRight
            // 
            this.baPanRight.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baPanRight.BackColor = System.Drawing.Color.Transparent;
            this.baPanRight.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.None;
            this.baPanRight.Cursor = System.Windows.Forms.Cursors.Default;
            this.baPanRight.FlatAppearance.BorderSize = 0;
            this.baPanRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.baPanRight.Image = ((System.Drawing.Image)(resources.GetObject("baPanRight.Image")));
            this.baPanRight.Location = new System.Drawing.Point(86, 43);
            this.baPanRight.Name = "baPanRight";
            this.baPanRight.Size = new System.Drawing.Size(44, 44);
            this.baPanRight.TabIndex = 20;
            this.baPanRight.Tag = "CamPanRight";
            this.baPanRight.UseVisualStyle = true;
            this.baPanRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.baCommandButton_MouseDown);
            this.baPanRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.baCommandButton_MouseUp);
            // 
            // baTiltUp
            // 
            this.baTiltUp.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baTiltUp.BackColor = System.Drawing.SystemColors.Control;
            this.baTiltUp.Cursor = System.Windows.Forms.Cursors.Default;
            this.baTiltUp.FlatAppearance.BorderSize = 0;
            this.baTiltUp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.baTiltUp.Image = ((System.Drawing.Image)(resources.GetObject("baTiltUp.Image")));
            this.baTiltUp.Location = new System.Drawing.Point(43, 0);
            this.baTiltUp.Name = "baTiltUp";
            this.baTiltUp.Size = new System.Drawing.Size(44, 44);
            this.baTiltUp.TabIndex = 18;
            this.baTiltUp.Tag = "CamTiltUp";
            this.baTiltUp.UseVisualStyle = true;
            this.baTiltUp.UseVisualStyleBackColor = true;
            this.baTiltUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.baCommandButton_MouseDown);
            this.baTiltUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.baCommandButton_MouseUp);
            // 
            // baHome
            // 
            this.baHome.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baHome.BackColor = System.Drawing.SystemColors.Control;
            this.baHome.Cursor = System.Windows.Forms.Cursors.Default;
            this.baHome.FlatAppearance.BorderSize = 0;
            this.baHome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.baHome.ForeColor = System.Drawing.Color.MidnightBlue;
            this.baHome.Location = new System.Drawing.Point(43, 43);
            this.baHome.Name = "baHome";
            this.baHome.Size = new System.Drawing.Size(44, 44);
            this.baHome.TabIndex = 16;
            this.baHome.Tag = "CamHome";
            this.baHome.Text = "HOME";
            this.baHome.UseVisualStyle = true;
            this.baHome.UseVisualStyleBackColor = true;
            this.baHome.MouseDown += new System.Windows.Forms.MouseEventHandler(this.baCommandButton_MouseDown);
            // 
            // baTiltDown
            // 
            this.baTiltDown.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baTiltDown.BackColor = System.Drawing.Color.Transparent;
            this.baTiltDown.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.None;
            this.baTiltDown.Cursor = System.Windows.Forms.Cursors.Default;
            this.baTiltDown.FlatAppearance.BorderSize = 0;
            this.baTiltDown.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.baTiltDown.Image = ((System.Drawing.Image)(resources.GetObject("baTiltDown.Image")));
            this.baTiltDown.Location = new System.Drawing.Point(43, 86);
            this.baTiltDown.Name = "baTiltDown";
            this.baTiltDown.Size = new System.Drawing.Size(44, 44);
            this.baTiltDown.TabIndex = 15;
            this.baTiltDown.Tag = "CamTiltDown";
            this.baTiltDown.UseVisualStyle = true;
            this.baTiltDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.baCommandButton_MouseDown);
            this.baTiltDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.baCommandButton_MouseUp);
            // 
            // baPanLeft
            // 
            this.baPanLeft.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baPanLeft.BackColor = System.Drawing.Color.Transparent;
            this.baPanLeft.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.None;
            this.baPanLeft.Cursor = System.Windows.Forms.Cursors.Default;
            this.baPanLeft.FlatAppearance.BorderSize = 0;
            this.baPanLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.baPanLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.baPanLeft.Image = ((System.Drawing.Image)(resources.GetObject("baPanLeft.Image")));
            this.baPanLeft.Location = new System.Drawing.Point(0, 43);
            this.baPanLeft.Name = "baPanLeft";
            this.baPanLeft.Size = new System.Drawing.Size(44, 44);
            this.baPanLeft.TabIndex = 14;
            this.baPanLeft.Tag = "CamPanLeft";
            this.baPanLeft.UseVisualStyle = true;
            this.baPanLeft.UseVisualStyleBackColor = false;
            this.baPanLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.baCommandButton_MouseDown);
            this.baPanLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.baCommandButton_MouseUp);
            // 
            // buttonAdv8
            // 
            this.buttonAdv8.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv8.BackColor = System.Drawing.Color.Transparent;
            this.buttonAdv8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonAdv8.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.None;
            this.buttonAdv8.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonAdv8.FlatAppearance.BorderSize = 0;
            this.buttonAdv8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonAdv8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAdv8.ForeColor = System.Drawing.Color.MidnightBlue;
            this.buttonAdv8.Image = ((System.Drawing.Image)(resources.GetObject("buttonAdv8.Image")));
            this.buttonAdv8.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAdv8.Location = new System.Drawing.Point(103, 187);
            this.buttonAdv8.Name = "buttonAdv8";
            this.buttonAdv8.Size = new System.Drawing.Size(85, 44);
            this.buttonAdv8.TabIndex = 19;
            this.buttonAdv8.Tag = "CamZoomNear";
            this.buttonAdv8.Text = "Zoom";
            this.buttonAdv8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAdv8.UseVisualStyle = true;
            this.buttonAdv8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.baCommandButton_MouseDown);
            this.buttonAdv8.MouseUp += new System.Windows.Forms.MouseEventHandler(this.baCommandButton_MouseUp);
            // 
            // buttonAdv6
            // 
            this.buttonAdv6.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv6.BackColor = System.Drawing.Color.Transparent;
            this.buttonAdv6.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.None;
            this.buttonAdv6.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonAdv6.FlatAppearance.BorderSize = 0;
            this.buttonAdv6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonAdv6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.buttonAdv6.ForeColor = System.Drawing.Color.MidnightBlue;
            this.buttonAdv6.Image = ((System.Drawing.Image)(resources.GetObject("buttonAdv6.Image")));
            this.buttonAdv6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAdv6.Location = new System.Drawing.Point(19, 187);
            this.buttonAdv6.Name = "buttonAdv6";
            this.buttonAdv6.Size = new System.Drawing.Size(85, 44);
            this.buttonAdv6.TabIndex = 17;
            this.buttonAdv6.Tag = "CamZoomFar";
            this.buttonAdv6.Text = "Zoom";
            this.buttonAdv6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAdv6.UseVisualStyle = true;
            this.buttonAdv6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.baCommandButton_MouseDown);
            this.buttonAdv6.MouseUp += new System.Windows.Forms.MouseEventHandler(this.baCommandButton_MouseUp);
            // 
            // cmsDetails
            // 
            this.cmsDetails.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDetails});
            this.cmsDetails.Name = "cmsDetails";
            this.cmsDetails.Size = new System.Drawing.Size(166, 48);
            // 
            // tsmiDetails
            // 
            this.tsmiDetails.Name = "tsmiDetails";
            this.tsmiDetails.Size = new System.Drawing.Size(165, 22);
            this.tsmiDetails.Text = "Дополнительно";
            this.tsmiDetails.Click += new System.EventHandler(this.tsmiDetails_Click);
            // 
            // VideoCameraPlayerCommonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MinimumSize = new System.Drawing.Size(216, 275);
            this.Name = "VideoCameraPlayerCommonControl";
            this.Size = new System.Drawing.Size(216, 401);
            this.Controls.SetChildIndex(this.gpDetail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).EndInit();
            this.gpDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).EndInit();
            this.gradientPanel2.ResumeLayout(false);
            this.gradientPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbaPreset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel4)).EndInit();
            this.gradientPanel4.ResumeLayout(false);
            this.cmsDetails.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel2;
        private Syncfusion.Windows.Forms.Tools.ComboBoxAdv cbaPreset;
        private Syncfusion.Windows.Forms.ButtonAdv baSave;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel11;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel4;
        private Syncfusion.Windows.Forms.ButtonAdv baPanRight;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv8;
        private Syncfusion.Windows.Forms.ButtonAdv baTiltUp;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv6;
        private Syncfusion.Windows.Forms.ButtonAdv baHome;
        private Syncfusion.Windows.Forms.ButtonAdv baTiltDown;
        private Syncfusion.Windows.Forms.ButtonAdv baPanLeft;
        private Syncfusion.Windows.Forms.Tools.ContextMenuStripEx cmsDetails;
        private System.Windows.Forms.ToolStripMenuItem tsmiDetails;
    }
}
