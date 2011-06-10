namespace Hosts.Plugins.VDCTerminal.Player
{
    partial class VDCTerminalControl
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
            this.gradientPanel1 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.baCallState = new Syncfusion.Windows.Forms.ButtonAdv();
            this.alStatusDesc = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel11 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.gradientPanel4 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.cbaAbonents = new Syncfusion.Windows.Forms.Tools.ComboBoxAdv();
            this.autoLabel14 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.baDial = new Syncfusion.Windows.Forms.ButtonAdv();
            this.tbeAbobentNumber = new Syncfusion.Windows.Forms.Tools.TextBoxExt();
            this.gradientPanel2 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.baPIP = new Syncfusion.Windows.Forms.ButtonAdv();
            this.baAuto = new Syncfusion.Windows.Forms.ButtonAdv();
            this.baContent = new Syncfusion.Windows.Forms.ButtonAdv();
            this.baDND = new Syncfusion.Windows.Forms.ButtonAdv();
            this.baPrivacy = new Syncfusion.Windows.Forms.ButtonAdv();
            this.gradientPanel3 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.baIncomingAnswer = new Syncfusion.Windows.Forms.ButtonAdv();
            this.alIncomingDesc = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.baIncomingCancel = new Syncfusion.Windows.Forms.ButtonAdv();
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).BeginInit();
            this.gpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).BeginInit();
            this.gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel4)).BeginInit();
            this.gradientPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbaAbonents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).BeginInit();
            this.gradientPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel3)).BeginInit();
            this.gradientPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpDetail
            // 
            this.gpDetail.Controls.Add(this.gradientPanel3);
            this.gpDetail.Controls.Add(this.gradientPanel2);
            this.gpDetail.Controls.Add(this.gradientPanel4);
            this.gpDetail.Controls.Add(this.gradientPanel1);
            this.gpDetail.Size = new System.Drawing.Size(210, 191);
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gradientPanel1.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel1.BorderSingle = System.Windows.Forms.ButtonBorderStyle.None;
            this.gradientPanel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gradientPanel1.Controls.Add(this.baCallState);
            this.gradientPanel1.Controls.Add(this.alStatusDesc);
            this.gradientPanel1.Controls.Add(this.autoLabel11);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(206, 59);
            this.gradientPanel1.TabIndex = 5;
            // 
            // baCallState
            // 
            this.baCallState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.baCallState.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baCallState.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.baCallState.Image = global::Hosts.Plugins.VDCTerminal.Properties.Resources.Phone_off;
            this.baCallState.Location = new System.Drawing.Point(168, 23);
            this.baCallState.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.baCallState.Name = "baCallState";
            this.baCallState.Size = new System.Drawing.Size(30, 30);
            this.baCallState.TabIndex = 9;
            this.baCallState.Tag = "Disconnect";
            this.baCallState.UseVisualStyle = true;
            this.baCallState.Click += new System.EventHandler(this.baCallState_Click);
            // 
            // alStatusDesc
            // 
            this.alStatusDesc.AutoSize = false;
            this.alStatusDesc.DX = 0;
            this.alStatusDesc.DY = 0;
            this.alStatusDesc.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alStatusDesc.ForeColor = System.Drawing.Color.MidnightBlue;
            this.alStatusDesc.Location = new System.Drawing.Point(3, 14);
            this.alStatusDesc.Name = "alStatusDesc";
            this.alStatusDesc.Size = new System.Drawing.Size(162, 42);
            this.alStatusDesc.TabIndex = 6;
            this.alStatusDesc.Text = "Текущее состояние соединения не определено";
            // 
            // autoLabel11
            // 
            this.autoLabel11.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.autoLabel11.DX = 0;
            this.autoLabel11.DY = 0;
            this.autoLabel11.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel11.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel11.Location = new System.Drawing.Point(69, 1);
            this.autoLabel11.Name = "autoLabel11";
            this.autoLabel11.Size = new System.Drawing.Size(68, 13);
            this.autoLabel11.TabIndex = 5;
            this.autoLabel11.Text = "Состояние";
            // 
            // gradientPanel4
            // 
            this.gradientPanel4.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gradientPanel4.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel4.BorderSides = System.Windows.Forms.Border3DSide.Top;
            this.gradientPanel4.Controls.Add(this.cbaAbonents);
            this.gradientPanel4.Controls.Add(this.autoLabel14);
            this.gradientPanel4.Controls.Add(this.baDial);
            this.gradientPanel4.Controls.Add(this.tbeAbobentNumber);
            this.gradientPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel4.Location = new System.Drawing.Point(0, 59);
            this.gradientPanel4.Name = "gradientPanel4";
            this.gradientPanel4.Size = new System.Drawing.Size(206, 61);
            this.gradientPanel4.TabIndex = 7;
            // 
            // cbaAbonents
            // 
            this.cbaAbonents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.cbaAbonents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbaAbonents.IgnoreThemeBackground = true;
            this.cbaAbonents.Location = new System.Drawing.Point(50, 35);
            this.cbaAbonents.Name = "cbaAbonents";
            this.cbaAbonents.Size = new System.Drawing.Size(148, 21);
            this.cbaAbonents.Style = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.cbaAbonents.TabIndex = 12;
            this.cbaAbonents.SelectedValueChanged += new System.EventHandler(this.cbaAbonents_SelectedValueChanged);
            // 
            // autoLabel14
            // 
            this.autoLabel14.DX = 0;
            this.autoLabel14.DY = 0;
            this.autoLabel14.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel14.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel14.Location = new System.Drawing.Point(1, 38);
            this.autoLabel14.Name = "autoLabel14";
            this.autoLabel14.Size = new System.Drawing.Size(50, 13);
            this.autoLabel14.TabIndex = 11;
            this.autoLabel14.Text = "Абонент";
            // 
            // baDial
            // 
            this.baDial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.baDial.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baDial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.baDial.Image = global::Hosts.Plugins.VDCTerminal.Properties.Resources.Phone;
            this.baDial.Location = new System.Drawing.Point(166, 3);
            this.baDial.Name = "baDial";
            this.baDial.Size = new System.Drawing.Size(30, 30);
            this.baDial.TabIndex = 10;
            this.baDial.Tag = "Dial";
            this.baDial.UseVisualStyle = true;
            this.baDial.Click += new System.EventHandler(this.baDial_Click);
            // 
            // tbeAbobentNumber
            // 
            this.tbeAbobentNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbeAbobentNumber.Location = new System.Drawing.Point(1, 8);
            this.tbeAbobentNumber.Name = "tbeAbobentNumber";
            this.tbeAbobentNumber.OverflowIndicatorToolTipText = null;
            this.tbeAbobentNumber.Size = new System.Drawing.Size(161, 21);
            this.tbeAbobentNumber.TabIndex = 0;
            this.tbeAbobentNumber.TextChanged += new System.EventHandler(this.tbeAbobentNumber_TextChanged);
            // 
            // gradientPanel2
            // 
            this.gradientPanel2.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gradientPanel2.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel2.BorderSides = System.Windows.Forms.Border3DSide.Top;
            this.gradientPanel2.Controls.Add(this.baPIP);
            this.gradientPanel2.Controls.Add(this.baAuto);
            this.gradientPanel2.Controls.Add(this.baContent);
            this.gradientPanel2.Controls.Add(this.baDND);
            this.gradientPanel2.Controls.Add(this.baPrivacy);
            this.gradientPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel2.Location = new System.Drawing.Point(0, 120);
            this.gradientPanel2.Name = "gradientPanel2";
            this.gradientPanel2.Size = new System.Drawing.Size(206, 33);
            this.gradientPanel2.TabIndex = 8;
            // 
            // baPIP
            // 
            this.baPIP.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.baPIP.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baPIP.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.baPIP.ForeColor = System.Drawing.Color.MidnightBlue;
            this.baPIP.Location = new System.Drawing.Point(156, 3);
            this.baPIP.Name = "baPIP";
            this.baPIP.PushButton = true;
            this.baPIP.Size = new System.Drawing.Size(40, 23);
            this.baPIP.TabIndex = 4;
            this.baPIP.Tag = "SetPIP";
            this.baPIP.Text = "PiP";
            this.baPIP.UseVisualStyle = true;
            this.baPIP.Click += new System.EventHandler(this.baSendCommand_Click);
            // 
            // baAuto
            // 
            this.baAuto.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.baAuto.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baAuto.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.baAuto.ForeColor = System.Drawing.Color.MidnightBlue;
            this.baAuto.Location = new System.Drawing.Point(117, 3);
            this.baAuto.Name = "baAuto";
            this.baAuto.PushButton = true;
            this.baAuto.Size = new System.Drawing.Size(40, 23);
            this.baAuto.TabIndex = 3;
            this.baAuto.Tag = "SetAutoAnswer";
            this.baAuto.Text = "Auto";
            this.baAuto.UseVisualStyle = true;
            this.baAuto.Click += new System.EventHandler(this.baSendCommand_Click);
            // 
            // baContent
            // 
            this.baContent.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.baContent.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baContent.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.baContent.ForeColor = System.Drawing.Color.MidnightBlue;
            this.baContent.Location = new System.Drawing.Point(78, 3);
            this.baContent.Name = "baContent";
            this.baContent.PushButton = true;
            this.baContent.Size = new System.Drawing.Size(40, 23);
            this.baContent.TabIndex = 2;
            this.baContent.Tag = "SetContent";
            this.baContent.Text = "P+C";
            this.baContent.UseVisualStyle = true;
            this.baContent.Click += new System.EventHandler(this.baSendCommand_Click);
            // 
            // baDND
            // 
            this.baDND.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.baDND.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baDND.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.baDND.ForeColor = System.Drawing.Color.MidnightBlue;
            this.baDND.Location = new System.Drawing.Point(39, 3);
            this.baDND.Name = "baDND";
            this.baDND.PushButton = true;
            this.baDND.Size = new System.Drawing.Size(40, 23);
            this.baDND.TabIndex = 1;
            this.baDND.Tag = "SetDND";
            this.baDND.Text = "DnD";
            this.baDND.UseVisualStyle = true;
            this.baDND.Click += new System.EventHandler(this.baSendCommand_Click);
            // 
            // baPrivacy
            // 
            this.baPrivacy.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.baPrivacy.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baPrivacy.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.baPrivacy.ForeColor = System.Drawing.Color.MidnightBlue;
            this.baPrivacy.Location = new System.Drawing.Point(0, 3);
            this.baPrivacy.Name = "baPrivacy";
            this.baPrivacy.PushButton = true;
            this.baPrivacy.Size = new System.Drawing.Size(40, 23);
            this.baPrivacy.TabIndex = 0;
            this.baPrivacy.Tag = "SetPrivacy";
            this.baPrivacy.Text = "PRV";
            this.baPrivacy.UseVisualStyle = true;
            this.baPrivacy.Click += new System.EventHandler(this.baSendCommand_Click);
            // 
            // gradientPanel3
            // 
            this.gradientPanel3.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gradientPanel3.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel3.BorderSides = System.Windows.Forms.Border3DSide.Top;
            this.gradientPanel3.Controls.Add(this.baIncomingAnswer);
            this.gradientPanel3.Controls.Add(this.alIncomingDesc);
            this.gradientPanel3.Controls.Add(this.baIncomingCancel);
            this.gradientPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel3.Location = new System.Drawing.Point(0, 153);
            this.gradientPanel3.Name = "gradientPanel3";
            this.gradientPanel3.Size = new System.Drawing.Size(206, 33);
            this.gradientPanel3.TabIndex = 9;
            // 
            // baIncomingAnswer
            // 
            this.baIncomingAnswer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.baIncomingAnswer.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baIncomingAnswer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.baIncomingAnswer.Image = global::Hosts.Plugins.VDCTerminal.Properties.Resources.Phone;
            this.baIncomingAnswer.Location = new System.Drawing.Point(137, 1);
            this.baIncomingAnswer.Name = "baIncomingAnswer";
            this.baIncomingAnswer.Size = new System.Drawing.Size(30, 30);
            this.baIncomingAnswer.TabIndex = 13;
            this.baIncomingAnswer.Tag = "Reply";
            this.baIncomingAnswer.UseVisualStyle = true;
            this.baIncomingAnswer.Click += new System.EventHandler(this.baIncomingAnswer_Click);
            // 
            // alIncomingDesc
            // 
            this.alIncomingDesc.AutoSize = false;
            this.alIncomingDesc.DX = 0;
            this.alIncomingDesc.DY = 0;
            this.alIncomingDesc.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alIncomingDesc.ForeColor = System.Drawing.Color.MidnightBlue;
            this.alIncomingDesc.Location = new System.Drawing.Point(1, 1);
            this.alIncomingDesc.Name = "alIncomingDesc";
            this.alIncomingDesc.Size = new System.Drawing.Size(134, 30);
            this.alIncomingDesc.TabIndex = 12;
            this.alIncomingDesc.Text = "Информация о входящем звонке не доступна";
            // 
            // baIncomingCancel
            // 
            this.baIncomingCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.baIncomingCancel.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baIncomingCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.baIncomingCancel.Image = global::Hosts.Plugins.VDCTerminal.Properties.Resources.Phone_off;
            this.baIncomingCancel.Location = new System.Drawing.Point(166, 1);
            this.baIncomingCancel.Name = "baIncomingCancel";
            this.baIncomingCancel.Size = new System.Drawing.Size(30, 30);
            this.baIncomingCancel.TabIndex = 11;
            this.baIncomingCancel.Tag = "Reply";
            this.baIncomingCancel.UseVisualStyle = true;
            this.baIncomingCancel.Click += new System.EventHandler(this.baIncomingCancel_Click);
            // 
            // VDCTerminalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MinimumSize = new System.Drawing.Size(216, 220);
            this.Name = "VDCTerminalControl";
            this.Size = new System.Drawing.Size(216, 350);
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).EndInit();
            this.gpDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).EndInit();
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel4)).EndInit();
            this.gradientPanel4.ResumeLayout(false);
            this.gradientPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbaAbonents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).EndInit();
            this.gradientPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel3)).EndInit();
            this.gradientPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel1;
        private Syncfusion.Windows.Forms.ButtonAdv baCallState;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alStatusDesc;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel11;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel3;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel2;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel4;
        private Syncfusion.Windows.Forms.Tools.TextBoxExt tbeAbobentNumber;
        private Syncfusion.Windows.Forms.Tools.ComboBoxAdv cbaAbonents;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel14;
        private Syncfusion.Windows.Forms.ButtonAdv baDial;
        private Syncfusion.Windows.Forms.ButtonAdv baPIP;
        private Syncfusion.Windows.Forms.ButtonAdv baAuto;
        private Syncfusion.Windows.Forms.ButtonAdv baContent;
        private Syncfusion.Windows.Forms.ButtonAdv baDND;
        private Syncfusion.Windows.Forms.ButtonAdv baPrivacy;
        private Syncfusion.Windows.Forms.ButtonAdv baIncomingCancel;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alIncomingDesc;
        private Syncfusion.Windows.Forms.ButtonAdv baIncomingAnswer;

    }
}
