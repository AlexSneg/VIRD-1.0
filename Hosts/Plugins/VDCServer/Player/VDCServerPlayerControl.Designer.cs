namespace Hosts.Plugins.VDCServer.Player
{
    partial class VDCServerPlayerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDCServerPlayerControl));
            this.gpTop = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.baStatus = new Syncfusion.Windows.Forms.ButtonAdv();
            this.autoLabel8 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel7 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.alVoiceSwitched = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel5 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.alStatus = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel3 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.alComment = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.alName = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.gpDetail = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.autoLabel9 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.cbaSettings = new Syncfusion.Windows.Forms.Tools.ComboBoxAdv();
            this.baApply = new Syncfusion.Windows.Forms.ButtonAdv();
            this.baFocus = new Syncfusion.Windows.Forms.ButtonAdv();
            this.gpMain = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.lbAbonents = new System.Windows.Forms.ListBox();
            this.lbMembers = new System.Windows.Forms.ListBox();
            this.baRemoveMember = new Syncfusion.Windows.Forms.ButtonAdv();
            this.baAddMember = new Syncfusion.Windows.Forms.ButtonAdv();
            ((System.ComponentModel.ISupportInitialize)(this.gpFillPanel)).BeginInit();
            this.gpFillPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gpTop)).BeginInit();
            this.gpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).BeginInit();
            this.gpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbaSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpMain)).BeginInit();
            this.gpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpFillPanel
            // 
            this.gpFillPanel.Controls.Add(this.gpMain);
            this.gpFillPanel.Controls.Add(this.gpDetail);
            this.gpFillPanel.Controls.Add(this.gpTop);
            this.gpFillPanel.Size = new System.Drawing.Size(210, 235);
            // 
            // gpTop
            // 
            this.gpTop.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gpTop.BorderColor = System.Drawing.Color.Black;
            this.gpTop.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gpTop.Controls.Add(this.baStatus);
            this.gpTop.Controls.Add(this.autoLabel8);
            this.gpTop.Controls.Add(this.autoLabel7);
            this.gpTop.Controls.Add(this.alVoiceSwitched);
            this.gpTop.Controls.Add(this.autoLabel5);
            this.gpTop.Controls.Add(this.alStatus);
            this.gpTop.Controls.Add(this.autoLabel3);
            this.gpTop.Controls.Add(this.alComment);
            this.gpTop.Controls.Add(this.alName);
            this.gpTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpTop.Location = new System.Drawing.Point(0, 0);
            this.gpTop.Name = "gpTop";
            this.gpTop.Size = new System.Drawing.Size(210, 107);
            this.gpTop.TabIndex = 3;
            // 
            // baStatus
            // 
            this.baStatus.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baStatus.BackgroundImage = global::Hosts.Plugins.VDCServer.Properties.Resources.Phone;
            this.baStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.baStatus.Location = new System.Drawing.Point(160, 57);
            this.baStatus.Name = "baStatus";
            this.baStatus.PushButton = true;
            this.baStatus.Size = new System.Drawing.Size(30, 30);
            this.baStatus.TabIndex = 14;
            this.baStatus.Tag = "CreateConference,DestroyConference";
            this.baStatus.UseVisualStyle = true;
            this.baStatus.Click += new System.EventHandler(this.baStatus_Click);
            // 
            // autoLabel8
            // 
            this.autoLabel8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoLabel8.DX = 0;
            this.autoLabel8.DY = 0;
            this.autoLabel8.Location = new System.Drawing.Point(85, 90);
            this.autoLabel8.Name = "autoLabel8";
            this.autoLabel8.Size = new System.Drawing.Size(125, 13);
            this.autoLabel8.TabIndex = 9;
            this.autoLabel8.Text = "Справочник абонентов";
            // 
            // autoLabel7
            // 
            this.autoLabel7.DX = 0;
            this.autoLabel7.DY = 0;
            this.autoLabel7.Location = new System.Drawing.Point(0, 90);
            this.autoLabel7.Name = "autoLabel7";
            this.autoLabel7.Size = new System.Drawing.Size(61, 13);
            this.autoLabel7.TabIndex = 8;
            this.autoLabel7.Text = "Участники";
            // 
            // alVoiceSwitched
            // 
            this.alVoiceSwitched.DX = 0;
            this.alVoiceSwitched.DY = 0;
            this.alVoiceSwitched.Location = new System.Drawing.Point(88, 74);
            this.alVoiceSwitched.Name = "alVoiceSwitched";
            this.alVoiceSwitched.Size = new System.Drawing.Size(20, 13);
            this.alVoiceSwitched.TabIndex = 7;
            this.alVoiceSwitched.Text = "да";
            // 
            // autoLabel5
            // 
            this.autoLabel5.DX = 0;
            this.autoLabel5.DY = 0;
            this.autoLabel5.Location = new System.Drawing.Point(0, 74);
            this.autoLabel5.Name = "autoLabel5";
            this.autoLabel5.Size = new System.Drawing.Size(82, 13);
            this.autoLabel5.TabIndex = 6;
            this.autoLabel5.Text = "Voice-switched:";
            // 
            // alStatus
            // 
            this.alStatus.DX = 0;
            this.alStatus.DY = 0;
            this.alStatus.Location = new System.Drawing.Point(88, 57);
            this.alStatus.Name = "alStatus";
            this.alStatus.Size = new System.Drawing.Size(49, 13);
            this.alStatus.TabIndex = 5;
            this.alStatus.Text = "активна";
            // 
            // autoLabel3
            // 
            this.autoLabel3.DX = 0;
            this.autoLabel3.DY = 0;
            this.autoLabel3.Location = new System.Drawing.Point(0, 57);
            this.autoLabel3.Name = "autoLabel3";
            this.autoLabel3.Size = new System.Drawing.Size(65, 13);
            this.autoLabel3.TabIndex = 4;
            this.autoLabel3.Text = "Состояние:";
            // 
            // alComment
            // 
            this.alComment.AutoSize = false;
            this.alComment.Dock = System.Windows.Forms.DockStyle.Top;
            this.alComment.DX = 0;
            this.alComment.DY = 0;
            this.alComment.Location = new System.Drawing.Point(0, 26);
            this.alComment.Name = "alComment";
            this.alComment.Size = new System.Drawing.Size(210, 29);
            this.alComment.TabIndex = 3;
            this.alComment.Text = "Комментарий к конференции, может быть какой то длинной строкой,  которая не влази" +
                "ет на две строки";
            // 
            // alName
            // 
            this.alName.AutoSize = false;
            this.alName.Dock = System.Windows.Forms.DockStyle.Top;
            this.alName.DX = 0;
            this.alName.DY = 0;
            this.alName.Location = new System.Drawing.Point(0, 0);
            this.alName.Name = "alName";
            this.alName.Size = new System.Drawing.Size(210, 26);
            this.alName.TabIndex = 2;
            this.alName.Text = "Название конференции: <например: конференция по вопросу производства>";
            // 
            // gpDetail
            // 
            this.gpDetail.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gpDetail.BorderColor = System.Drawing.Color.Black;
            this.gpDetail.BorderSides = System.Windows.Forms.Border3DSide.Bottom;
            this.gpDetail.Controls.Add(this.pictureBox1);
            this.gpDetail.Controls.Add(this.autoLabel9);
            this.gpDetail.Controls.Add(this.cbaSettings);
            this.gpDetail.Controls.Add(this.baApply);
            this.gpDetail.Controls.Add(this.baFocus);
            this.gpDetail.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gpDetail.Location = new System.Drawing.Point(0, 167);
            this.gpDetail.Name = "gpDetail";
            this.gpDetail.Size = new System.Drawing.Size(210, 68);
            this.gpDetail.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(142, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // autoLabel9
            // 
            this.autoLabel9.DX = 0;
            this.autoLabel9.DY = 0;
            this.autoLabel9.Location = new System.Drawing.Point(1, 24);
            this.autoLabel9.Name = "autoLabel9";
            this.autoLabel9.Size = new System.Drawing.Size(61, 13);
            this.autoLabel9.TabIndex = 9;
            this.autoLabel9.Text = "Раскладка";
            // 
            // cbaSettings
            // 
            this.cbaSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.cbaSettings.DisplayMember = "LayoutName";
            this.cbaSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbaSettings.IgnoreThemeBackground = true;
            this.cbaSettings.Location = new System.Drawing.Point(1, 40);
            this.cbaSettings.Name = "cbaSettings";
            this.cbaSettings.Size = new System.Drawing.Size(115, 21);
            this.cbaSettings.Style = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.cbaSettings.TabIndex = 2;
            this.cbaSettings.ValueMember = "LayoutNumber";
            this.cbaSettings.SelectedIndexChanged += new System.EventHandler(this.cbaSettings_SelectedIndexChanged);
            // 
            // baApply
            // 
            this.baApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.baApply.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baApply.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("baApply.BackgroundImage")));
            this.baApply.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.baApply.Location = new System.Drawing.Point(118, 40);
            this.baApply.Name = "baApply";
            this.baApply.Size = new System.Drawing.Size(22, 22);
            this.baApply.TabIndex = 1;
            this.baApply.Tag = "UpdateConference";
            this.baApply.UseVisualStyle = true;
            this.baApply.Click += new System.EventHandler(this.baApply_Click);
            // 
            // baFocus
            // 
            this.baFocus.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baFocus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baFocus.Location = new System.Drawing.Point(1, -2);
            this.baFocus.Name = "baFocus";
            this.baFocus.Size = new System.Drawing.Size(58, 23);
            this.baFocus.TabIndex = 0;
            this.baFocus.Tag = "FocusParticipant";
            this.baFocus.Text = "Фокус";
            this.baFocus.UseVisualStyle = true;
            this.baFocus.Click += new System.EventHandler(this.baFocus_Click);
            // 
            // gpMain
            // 
            this.gpMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.gpMain.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gpMain.BorderColor = System.Drawing.Color.Black;
            this.gpMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gpMain.Controls.Add(this.lbAbonents);
            this.gpMain.Controls.Add(this.lbMembers);
            this.gpMain.Controls.Add(this.baRemoveMember);
            this.gpMain.Controls.Add(this.baAddMember);
            this.gpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpMain.Location = new System.Drawing.Point(0, 107);
            this.gpMain.Name = "gpMain";
            this.gpMain.Size = new System.Drawing.Size(210, 60);
            this.gpMain.TabIndex = 5;
            // 
            // lbAbonents
            // 
            this.lbAbonents.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbAbonents.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbAbonents.FormattingEnabled = true;
            this.lbAbonents.ItemHeight = 11;
            this.lbAbonents.Location = new System.Drawing.Point(118, 0);
            this.lbAbonents.Name = "lbAbonents";
            this.lbAbonents.Size = new System.Drawing.Size(92, 59);
            this.lbAbonents.TabIndex = 16;
            // 
            // lbMembers
            // 
            this.lbMembers.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbMembers.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbMembers.FormattingEnabled = true;
            this.lbMembers.ItemHeight = 11;
            this.lbMembers.Location = new System.Drawing.Point(0, 0);
            this.lbMembers.Name = "lbMembers";
            this.lbMembers.Size = new System.Drawing.Size(92, 59);
            this.lbMembers.TabIndex = 15;
            // 
            // baRemoveMember
            // 
            this.baRemoveMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.baRemoveMember.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baRemoveMember.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("baRemoveMember.BackgroundImage")));
            this.baRemoveMember.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.baRemoveMember.Location = new System.Drawing.Point(94, 37);
            this.baRemoveMember.Name = "baRemoveMember";
            this.baRemoveMember.Size = new System.Drawing.Size(22, 22);
            this.baRemoveMember.TabIndex = 14;
            this.baRemoveMember.Tag = "RemoveParticipant";
            this.baRemoveMember.UseVisualStyle = true;
            this.baRemoveMember.Click += new System.EventHandler(this.baRemoveMember_Click);
            // 
            // baAddMember
            // 
            this.baAddMember.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baAddMember.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("baAddMember.BackgroundImage")));
            this.baAddMember.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.baAddMember.Location = new System.Drawing.Point(94, 0);
            this.baAddMember.Name = "baAddMember";
            this.baAddMember.Size = new System.Drawing.Size(22, 22);
            this.baAddMember.TabIndex = 13;
            this.baAddMember.Tag = "AddParticipant";
            this.baAddMember.UseVisualStyle = true;
            this.baAddMember.Click += new System.EventHandler(this.baAddMember_Click);
            // 
            // VDCServerPlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MinimumSize = new System.Drawing.Size(210, 273);
            this.Name = "VDCServerPlayerControl";
            this.Size = new System.Drawing.Size(210, 273);
            ((System.ComponentModel.ISupportInitialize)(this.gpFillPanel)).EndInit();
            this.gpFillPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gpTop)).EndInit();
            this.gpTop.ResumeLayout(false);
            this.gpTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).EndInit();
            this.gpDetail.ResumeLayout(false);
            this.gpDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbaSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpMain)).EndInit();
            this.gpMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientPanel gpTop;
        private Syncfusion.Windows.Forms.ButtonAdv baStatus;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel8;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel7;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alVoiceSwitched;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel5;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alStatus;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel3;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alComment;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alName;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gpDetail;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel9;
        private Syncfusion.Windows.Forms.Tools.ComboBoxAdv cbaSettings;
        private Syncfusion.Windows.Forms.ButtonAdv baApply;
        private Syncfusion.Windows.Forms.ButtonAdv baFocus;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gpMain;
        private System.Windows.Forms.ListBox lbAbonents;
        private System.Windows.Forms.ListBox lbMembers;
        private Syncfusion.Windows.Forms.ButtonAdv baRemoveMember;
        private Syncfusion.Windows.Forms.ButtonAdv baAddMember;
    }
}
