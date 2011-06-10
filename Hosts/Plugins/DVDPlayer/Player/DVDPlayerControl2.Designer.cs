namespace Hosts.Plugins.DVDPlayer.Player
{
    partial class DVDPlayerControl2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DVDPlayerControl2));
            this.baPower = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv3 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv4 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv5 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv6 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv7 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv8 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv9 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.timeLabel = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.statusLabel = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.prevButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.rewButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.playButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.pauseButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.stopButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.ffButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.nextButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv10 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv11 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.cbaChapter = new Syncfusion.Windows.Forms.Tools.ComboBoxAdv();
            this.autoLabel11 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.cbaTrack = new Syncfusion.Windows.Forms.Tools.ComboBoxAdv();
            this.autoLabel12 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.baSetTrack = new Syncfusion.Windows.Forms.ButtonAdv();
            this.itbChapter = new Syncfusion.Windows.Forms.Tools.IntegerTextBox();
            this.itbTrack = new Syncfusion.Windows.Forms.Tools.IntegerTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).BeginInit();
            this.gpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbaChapter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbaTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itbChapter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itbTrack)).BeginInit();
            this.SuspendLayout();
            // 
            // gpDetail
            // 
            this.gpDetail.Controls.Add(this.itbTrack);
            this.gpDetail.Controls.Add(this.itbChapter);
            this.gpDetail.Controls.Add(this.baSetTrack);
            this.gpDetail.Controls.Add(this.autoLabel12);
            this.gpDetail.Controls.Add(this.cbaTrack);
            this.gpDetail.Controls.Add(this.autoLabel11);
            this.gpDetail.Controls.Add(this.cbaChapter);
            this.gpDetail.Controls.Add(this.buttonAdv11);
            this.gpDetail.Controls.Add(this.buttonAdv10);
            this.gpDetail.Controls.Add(this.prevButton);
            this.gpDetail.Controls.Add(this.rewButton);
            this.gpDetail.Controls.Add(this.playButton);
            this.gpDetail.Controls.Add(this.pauseButton);
            this.gpDetail.Controls.Add(this.stopButton);
            this.gpDetail.Controls.Add(this.ffButton);
            this.gpDetail.Controls.Add(this.nextButton);
            this.gpDetail.Controls.Add(this.timeLabel);
            this.gpDetail.Controls.Add(this.statusLabel);
            this.gpDetail.Controls.Add(this.buttonAdv9);
            this.gpDetail.Controls.Add(this.buttonAdv8);
            this.gpDetail.Controls.Add(this.buttonAdv7);
            this.gpDetail.Controls.Add(this.buttonAdv6);
            this.gpDetail.Controls.Add(this.buttonAdv5);
            this.gpDetail.Controls.Add(this.buttonAdv4);
            this.gpDetail.Controls.Add(this.buttonAdv3);
            this.gpDetail.Controls.Add(this.baPower);
            this.gpDetail.Size = new System.Drawing.Size(204, 193);
            // 
            // baPower
            // 
            this.baPower.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baPower.Image = ((System.Drawing.Image)(resources.GetObject("baPower.Image")));
            this.baPower.Location = new System.Drawing.Point(3, 12);
            this.baPower.Name = "baPower";
            this.baPower.PushButton = true;
            this.baPower.Size = new System.Drawing.Size(40, 40);
            this.baPower.TabIndex = 0;
            this.baPower.Tag = "Power";
            this.baPower.UseVisualStyle = true;
            this.baPower.Click += new System.EventHandler(this.baPower_Click);
            // 
            // buttonAdv3
            // 
            this.buttonAdv3.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAdv3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.buttonAdv3.Location = new System.Drawing.Point(45, 29);
            this.buttonAdv3.Name = "buttonAdv3";
            this.buttonAdv3.Size = new System.Drawing.Size(42, 23);
            this.buttonAdv3.TabIndex = 1;
            this.buttonAdv3.Tag = "Menu";
            this.buttonAdv3.Text = "Меню";
            this.buttonAdv3.UseVisualStyle = true;
            this.buttonAdv3.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // buttonAdv4
            // 
            this.buttonAdv4.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAdv4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.buttonAdv4.Location = new System.Drawing.Point(3, 53);
            this.buttonAdv4.Name = "buttonAdv4";
            this.buttonAdv4.Size = new System.Drawing.Size(84, 23);
            this.buttonAdv4.TabIndex = 2;
            this.buttonAdv4.Tag = "Setup";
            this.buttonAdv4.Text = "Настройки";
            this.buttonAdv4.UseVisualStyle = true;
            this.buttonAdv4.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // buttonAdv5
            // 
            this.buttonAdv5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdv5.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonAdv5.BackgroundImage")));
            this.buttonAdv5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonAdv5.Location = new System.Drawing.Point(149, 3);
            this.buttonAdv5.Name = "buttonAdv5";
            this.buttonAdv5.Size = new System.Drawing.Size(25, 25);
            this.buttonAdv5.TabIndex = 3;
            this.buttonAdv5.Tag = "CursorUp";
            this.buttonAdv5.UseVisualStyle = true;
            this.buttonAdv5.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // buttonAdv6
            // 
            this.buttonAdv6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdv6.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonAdv6.BackgroundImage")));
            this.buttonAdv6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonAdv6.Location = new System.Drawing.Point(124, 27);
            this.buttonAdv6.Name = "buttonAdv6";
            this.buttonAdv6.Size = new System.Drawing.Size(25, 25);
            this.buttonAdv6.TabIndex = 4;
            this.buttonAdv6.Tag = "CursorLeft";
            this.buttonAdv6.UseVisualStyle = true;
            this.buttonAdv6.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // buttonAdv7
            // 
            this.buttonAdv7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdv7.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonAdv7.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAdv7.ForeColor = System.Drawing.Color.MidnightBlue;
            this.buttonAdv7.Location = new System.Drawing.Point(149, 27);
            this.buttonAdv7.Name = "buttonAdv7";
            this.buttonAdv7.Size = new System.Drawing.Size(25, 25);
            this.buttonAdv7.TabIndex = 5;
            this.buttonAdv7.Tag = "CursorOk";
            this.buttonAdv7.Text = "OK";
            this.buttonAdv7.UseVisualStyle = true;
            this.buttonAdv7.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // buttonAdv8
            // 
            this.buttonAdv8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdv8.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv8.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonAdv8.BackgroundImage")));
            this.buttonAdv8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonAdv8.Location = new System.Drawing.Point(149, 51);
            this.buttonAdv8.Name = "buttonAdv8";
            this.buttonAdv8.Size = new System.Drawing.Size(25, 25);
            this.buttonAdv8.TabIndex = 6;
            this.buttonAdv8.Tag = "CursorDown";
            this.buttonAdv8.UseVisualStyle = true;
            this.buttonAdv8.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // buttonAdv9
            // 
            this.buttonAdv9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdv9.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv9.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonAdv9.BackgroundImage")));
            this.buttonAdv9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonAdv9.Location = new System.Drawing.Point(174, 27);
            this.buttonAdv9.Name = "buttonAdv9";
            this.buttonAdv9.Size = new System.Drawing.Size(25, 25);
            this.buttonAdv9.TabIndex = 7;
            this.buttonAdv9.Tag = "CursorRight";
            this.buttonAdv9.UseVisualStyle = true;
            this.buttonAdv9.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // timeLabel
            // 
            this.timeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timeLabel.DX = 0;
            this.timeLabel.DY = 0;
            this.timeLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.timeLabel.ForeColor = System.Drawing.Color.MidnightBlue;
            this.timeLabel.Location = new System.Drawing.Point(93, 79);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(105, 13);
            this.timeLabel.TabIndex = 9;
            this.timeLabel.Text = "00:15:07 / 01:05:06";
            this.timeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusLabel
            // 
            this.statusLabel.DX = 0;
            this.statusLabel.DY = 0;
            this.statusLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusLabel.ForeColor = System.Drawing.Color.MidnightBlue;
            this.statusLabel.Location = new System.Drawing.Point(3, 79);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(57, 13);
            this.statusLabel.TabIndex = 8;
            this.statusLabel.Text = "PLAYBACK";
            // 
            // prevButton
            // 
            this.prevButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.prevButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.prevButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("prevButton.BackgroundImage")));
            this.prevButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.prevButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.prevButton.Location = new System.Drawing.Point(3, 97);
            this.prevButton.Margin = new System.Windows.Forms.Padding(1);
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size(29, 22);
            this.prevButton.TabIndex = 14;
            this.prevButton.Tag = "Prev";
            this.prevButton.UseVisualStyle = true;
            this.prevButton.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // rewButton
            // 
            this.rewButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rewButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.rewButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rewButton.BackgroundImage")));
            this.rewButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rewButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rewButton.Location = new System.Drawing.Point(31, 97);
            this.rewButton.Margin = new System.Windows.Forms.Padding(1);
            this.rewButton.Name = "rewButton";
            this.rewButton.Size = new System.Drawing.Size(29, 22);
            this.rewButton.TabIndex = 15;
            this.rewButton.Tag = "Rew";
            this.rewButton.UseVisualStyle = true;
            this.rewButton.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // playButton
            // 
            this.playButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.playButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.playButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("playButton.BackgroundImage")));
            this.playButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.playButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.playButton.Location = new System.Drawing.Point(59, 97);
            this.playButton.Margin = new System.Windows.Forms.Padding(1);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(29, 22);
            this.playButton.TabIndex = 16;
            this.playButton.Tag = "Play";
            this.playButton.UseVisualStyle = true;
            this.playButton.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pauseButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.pauseButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pauseButton.BackgroundImage")));
            this.pauseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pauseButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pauseButton.Location = new System.Drawing.Point(87, 97);
            this.pauseButton.Margin = new System.Windows.Forms.Padding(1);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(29, 22);
            this.pauseButton.TabIndex = 13;
            this.pauseButton.Tag = "Pause";
            this.pauseButton.UseVisualStyle = true;
            this.pauseButton.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.stopButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.stopButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("stopButton.BackgroundImage")));
            this.stopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.stopButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.stopButton.Location = new System.Drawing.Point(115, 97);
            this.stopButton.Margin = new System.Windows.Forms.Padding(1);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(29, 22);
            this.stopButton.TabIndex = 10;
            this.stopButton.Tag = "Stop";
            this.stopButton.UseVisualStyle = true;
            this.stopButton.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // ffButton
            // 
            this.ffButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ffButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.ffButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ffButton.BackgroundImage")));
            this.ffButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ffButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ffButton.Location = new System.Drawing.Point(143, 97);
            this.ffButton.Margin = new System.Windows.Forms.Padding(1);
            this.ffButton.Name = "ffButton";
            this.ffButton.Size = new System.Drawing.Size(29, 22);
            this.ffButton.TabIndex = 11;
            this.ffButton.Tag = "Ff";
            this.ffButton.UseVisualStyle = true;
            this.ffButton.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.nextButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.nextButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("nextButton.BackgroundImage")));
            this.nextButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.nextButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nextButton.Location = new System.Drawing.Point(171, 97);
            this.nextButton.Margin = new System.Windows.Forms.Padding(1);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(29, 22);
            this.nextButton.TabIndex = 12;
            this.nextButton.Tag = "Next";
            this.nextButton.UseVisualStyle = true;
            this.nextButton.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // buttonAdv10
            // 
            this.buttonAdv10.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAdv10.ForeColor = System.Drawing.Color.MidnightBlue;
            this.buttonAdv10.Location = new System.Drawing.Point(2, 123);
            this.buttonAdv10.Name = "buttonAdv10";
            this.buttonAdv10.Size = new System.Drawing.Size(85, 23);
            this.buttonAdv10.TabIndex = 17;
            this.buttonAdv10.Tag = "Repeat";
            this.buttonAdv10.Text = "Повтор";
            this.buttonAdv10.UseVisualStyle = true;
            this.buttonAdv10.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // buttonAdv11
            // 
            this.buttonAdv11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdv11.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv11.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAdv11.ForeColor = System.Drawing.Color.MidnightBlue;
            this.buttonAdv11.Location = new System.Drawing.Point(114, 123);
            this.buttonAdv11.Name = "buttonAdv11";
            this.buttonAdv11.Size = new System.Drawing.Size(85, 23);
            this.buttonAdv11.TabIndex = 18;
            this.buttonAdv11.Tag = "Return";
            this.buttonAdv11.Text = "Возврат";
            this.buttonAdv11.UseVisualStyle = true;
            this.buttonAdv11.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // cbaChapter
            // 
            this.cbaChapter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.cbaChapter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbaChapter.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbaChapter.IgnoreThemeBackground = true;
            this.cbaChapter.Location = new System.Drawing.Point(2, 165);
            this.cbaChapter.Name = "cbaChapter";
            this.cbaChapter.Size = new System.Drawing.Size(46, 19);
            this.cbaChapter.Style = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.cbaChapter.TabIndex = 19;
            this.cbaChapter.Tag = "GetTrack";
            this.cbaChapter.SelectedIndexChanged += new System.EventHandler(this.cbaChapter_SelectedIndexChanged);
            // 
            // autoLabel11
            // 
            this.autoLabel11.DX = 0;
            this.autoLabel11.DY = 0;
            this.autoLabel11.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel11.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel11.Location = new System.Drawing.Point(2, 149);
            this.autoLabel11.Name = "autoLabel11";
            this.autoLabel11.Size = new System.Drawing.Size(41, 13);
            this.autoLabel11.TabIndex = 20;
            this.autoLabel11.Text = "Глава:";
            // 
            // cbaTrack
            // 
            this.cbaTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.cbaTrack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbaTrack.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbaTrack.IgnoreThemeBackground = true;
            this.cbaTrack.Location = new System.Drawing.Point(54, 165);
            this.cbaTrack.Name = "cbaTrack";
            this.cbaTrack.Size = new System.Drawing.Size(46, 19);
            this.cbaTrack.Style = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.cbaTrack.TabIndex = 21;
            // 
            // autoLabel12
            // 
            this.autoLabel12.DX = 0;
            this.autoLabel12.DY = 0;
            this.autoLabel12.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel12.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel12.Location = new System.Drawing.Point(54, 149);
            this.autoLabel12.Name = "autoLabel12";
            this.autoLabel12.Size = new System.Drawing.Size(35, 13);
            this.autoLabel12.TabIndex = 22;
            this.autoLabel12.Text = "Трек:";
            // 
            // baSetTrack
            // 
            this.baSetTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.baSetTrack.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baSetTrack.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.baSetTrack.ForeColor = System.Drawing.Color.MidnightBlue;
            this.baSetTrack.Location = new System.Drawing.Point(113, 161);
            this.baSetTrack.Name = "baSetTrack";
            this.baSetTrack.Size = new System.Drawing.Size(85, 23);
            this.baSetTrack.TabIndex = 23;
            this.baSetTrack.Tag = "Track,Chapter";
            this.baSetTrack.Text = "Установить";
            this.baSetTrack.UseVisualStyle = true;
            this.baSetTrack.Click += new System.EventHandler(this.baSetTrack_Click);
            // 
            // itbChapter
            // 
            this.itbChapter.Font = new System.Drawing.Font("Tahoma", 6.75F);
            this.itbChapter.IntegerValue = ((long)(1));
            this.itbChapter.Location = new System.Drawing.Point(3, 165);
            this.itbChapter.MaxValue = ((long)(50));
            this.itbChapter.MinValue = ((long)(1));
            this.itbChapter.Name = "itbChapter";
            this.itbChapter.NegativeInputPendingOnSelectAll = false;
            this.itbChapter.NullString = "0";
            this.itbChapter.OverflowIndicatorToolTipText = null;
            this.itbChapter.Size = new System.Drawing.Size(46, 18);
            this.itbChapter.TabIndex = 24;
            this.itbChapter.Visible = false;
            // 
            // itbTrack
            // 
            this.itbTrack.Font = new System.Drawing.Font("Tahoma", 6.75F);
            this.itbTrack.IntegerValue = ((long)(1));
            this.itbTrack.Location = new System.Drawing.Point(55, 165);
            this.itbTrack.MaxValue = ((long)(50));
            this.itbTrack.MinValue = ((long)(1));
            this.itbTrack.Name = "itbTrack";
            this.itbTrack.NegativeInputPendingOnSelectAll = false;
            this.itbTrack.NullString = "0";
            this.itbTrack.OverflowIndicatorToolTipText = null;
            this.itbTrack.Size = new System.Drawing.Size(46, 18);
            this.itbTrack.TabIndex = 25;
            this.itbTrack.Visible = false;
            // 
            // DVDPlayerControl2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MinimumSize = new System.Drawing.Size(210, 230);
            this.Name = "DVDPlayerControl2";
            this.Size = new System.Drawing.Size(210, 352);
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).EndInit();
            this.gpDetail.ResumeLayout(false);
            this.gpDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbaChapter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbaTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.itbChapter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.itbTrack)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv baPower;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv4;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv3;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv5;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv6;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv8;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv7;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv9;
        private Syncfusion.Windows.Forms.Tools.AutoLabel timeLabel;
        private Syncfusion.Windows.Forms.Tools.AutoLabel statusLabel;
        private Syncfusion.Windows.Forms.ButtonAdv prevButton;
        private Syncfusion.Windows.Forms.ButtonAdv rewButton;
        private Syncfusion.Windows.Forms.ButtonAdv playButton;
        private Syncfusion.Windows.Forms.ButtonAdv pauseButton;
        private Syncfusion.Windows.Forms.ButtonAdv stopButton;
        private Syncfusion.Windows.Forms.ButtonAdv ffButton;
        private Syncfusion.Windows.Forms.ButtonAdv nextButton;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv11;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv10;
        private Syncfusion.Windows.Forms.Tools.ComboBoxAdv cbaChapter;
        private Syncfusion.Windows.Forms.ButtonAdv baSetTrack;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel12;
        private Syncfusion.Windows.Forms.Tools.ComboBoxAdv cbaTrack;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel11;
        private Syncfusion.Windows.Forms.Tools.IntegerTextBox itbTrack;
        private Syncfusion.Windows.Forms.Tools.IntegerTextBox itbChapter;
    }
}
