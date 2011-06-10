namespace UI.PresentationDesign.DesignUI.Controls.Preparation
{
    partial class AgentPrepareProgressControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgentPrepareProgressControl));
            this.agentLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.pauseButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.playButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.progressLabel = new System.Windows.Forms.Label();
            this.clearButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.stopButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.speedLabel = new System.Windows.Forms.Label();
            this.buttonsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // agentLabel
            // 
            this.agentLabel.AutoEllipsis = true;
            this.agentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.agentLabel.Location = new System.Drawing.Point(4, 17);
            this.agentLabel.Name = "agentLabel";
            this.agentLabel.Size = new System.Drawing.Size(107, 20);
            this.agentLabel.TabIndex = 0;
            this.agentLabel.Text = "Название агента";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(118, 16);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(186, 15);
            this.progressBar.TabIndex = 1;
            // 
            // pauseButton
            // 
            this.pauseButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.pauseButton.Image = ((System.Drawing.Image)(resources.GetObject("pauseButton.Image")));
            this.pauseButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.pauseButton.Location = new System.Drawing.Point(403, 6);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(32, 32);
            this.pauseButton.TabIndex = 3;
            this.pauseButton.TabStop = false;
            this.buttonsToolTip.SetToolTip(this.pauseButton, "Приостановить копирование источников");
            this.pauseButton.UseVisualStyle = true;
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // playButton
            // 
            this.playButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.playButton.Image = ((System.Drawing.Image)(resources.GetObject("playButton.Image")));
            this.playButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.playButton.Location = new System.Drawing.Point(432, 6);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(32, 32);
            this.playButton.TabIndex = 4;
            this.playButton.TabStop = false;
            this.buttonsToolTip.SetToolTip(this.playButton, "Продолжить копирование источников");
            this.playButton.UseVisualStyle = true;
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(4, -2);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(0, 13);
            this.progressLabel.TabIndex = 5;
            this.progressLabel.Tag = "Информация";
            // 
            // clearButton
            // 
            this.clearButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.clearButton.Image = ((System.Drawing.Image)(resources.GetObject("clearButton.Image")));
            this.clearButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.clearButton.Location = new System.Drawing.Point(490, 6);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(32, 32);
            this.clearButton.TabIndex = 7;
            this.clearButton.TabStop = false;
            this.buttonsToolTip.SetToolTip(this.clearButton, "Очистить дисковое пространство");
            this.clearButton.UseVisualStyle = true;
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.stopButton.Image = ((System.Drawing.Image)(resources.GetObject("stopButton.Image")));
            this.stopButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.stopButton.Location = new System.Drawing.Point(461, 6);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(32, 32);
            this.stopButton.TabIndex = 6;
            this.stopButton.TabStop = false;
            this.buttonsToolTip.SetToolTip(this.stopButton, "Прервать копирование источников");
            this.stopButton.UseVisualStyle = true;
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.commandButton_Click);
            // 
            // speedLabel
            // 
            this.speedLabel.Location = new System.Drawing.Point(310, 17);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(87, 13);
            this.speedLabel.TabIndex = 8;
            this.speedLabel.Text = "Скорость";
            this.speedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(6, 40);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1024, 2);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // AgentPrepareProgressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.speedLabel);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.agentLabel);
            this.Name = "AgentPrepareProgressControl";
            this.Size = new System.Drawing.Size(499, 44);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label agentLabel;
        private System.Windows.Forms.ProgressBar progressBar;
        private Syncfusion.Windows.Forms.ButtonAdv pauseButton;
        private Syncfusion.Windows.Forms.ButtonAdv playButton;
        private System.Windows.Forms.Label progressLabel;
        private Syncfusion.Windows.Forms.ButtonAdv clearButton;
        private Syncfusion.Windows.Forms.ButtonAdv stopButton;
        private System.Windows.Forms.Label speedLabel;
        private System.Windows.Forms.ToolTip buttonsToolTip;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
