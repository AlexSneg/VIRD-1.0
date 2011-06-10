namespace UI.PresentationDesign.DesignUI.Forms
{
    partial class FindItemForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.autoLabel3 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.tbAuthor = new Syncfusion.Windows.Forms.Tools.TextBoxExt();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbHardSource = new System.Windows.Forms.RadioButton();
            this.rbGlobalSource = new System.Windows.Forms.RadioButton();
            this.rbLocalSource = new System.Windows.Forms.RadioButton();
            this.rbDevice = new System.Windows.Forms.RadioButton();
            this.rbDisplay = new System.Windows.Forms.RadioButton();
            this.rbSlide = new System.Windows.Forms.RadioButton();
            this.buttonAdv2 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv1 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.autoLabel2 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel1 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.textBoxExt2 = new Syncfusion.Windows.Forms.Tools.TextBoxExt();
            this.textBoxExt1 = new Syncfusion.Windows.Forms.Tools.TextBoxExt();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // autoLabel3
            // 
            this.autoLabel3.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel3.DX = 0;
            this.autoLabel3.DY = 0;
            this.autoLabel3.Location = new System.Drawing.Point(19, 272);
            this.autoLabel3.Name = "autoLabel3";
            this.autoLabel3.Size = new System.Drawing.Size(37, 13);
            this.autoLabel3.TabIndex = 9;
            this.autoLabel3.Text = "Автор";
            // 
            // tbAuthor
            // 
            this.tbAuthor.Location = new System.Drawing.Point(93, 268);
            this.tbAuthor.Name = "tbAuthor";
            this.tbAuthor.OverflowIndicatorToolTipText = null;
            this.tbAuthor.Size = new System.Drawing.Size(314, 20);
            this.tbAuthor.TabIndex = 9;
            this.tbAuthor.TextChanged += new System.EventHandler(this.textBoxExt1_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.rbHardSource);
            this.groupBox1.Controls.Add(this.rbGlobalSource);
            this.groupBox1.Controls.Add(this.rbLocalSource);
            this.groupBox1.Controls.Add(this.rbDevice);
            this.groupBox1.Controls.Add(this.rbDisplay);
            this.groupBox1.Controls.Add(this.rbSlide);
            this.groupBox1.Location = new System.Drawing.Point(10, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(397, 117);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Элемент сценария";
            // 
            // rbHardSource
            // 
            this.rbHardSource.AutoSize = true;
            this.rbHardSource.Location = new System.Drawing.Point(165, 75);
            this.rbHardSource.Name = "rbHardSource";
            this.rbHardSource.Size = new System.Drawing.Size(136, 17);
            this.rbHardSource.TabIndex = 6;
            this.rbHardSource.TabStop = true;
            this.rbHardSource.Text = "Аппаратный источник";
            this.rbHardSource.UseVisualStyleBackColor = true;
            this.rbHardSource.CheckedChanged += new System.EventHandler(this.rbSlide_CheckedChanged);
            // 
            // rbGlobalSource
            // 
            this.rbGlobalSource.AutoSize = true;
            this.rbGlobalSource.Location = new System.Drawing.Point(165, 52);
            this.rbGlobalSource.Name = "rbGlobalSource";
            this.rbGlobalSource.Size = new System.Drawing.Size(189, 17);
            this.rbGlobalSource.TabIndex = 5;
            this.rbGlobalSource.TabStop = true;
            this.rbGlobalSource.Text = "Программный источник (общий)";
            this.rbGlobalSource.UseVisualStyleBackColor = true;
            this.rbGlobalSource.CheckedChanged += new System.EventHandler(this.rbSlide_CheckedChanged);
            // 
            // rbLocalSource
            // 
            this.rbLocalSource.AutoSize = true;
            this.rbLocalSource.Location = new System.Drawing.Point(165, 29);
            this.rbLocalSource.Name = "rbLocalSource";
            this.rbLocalSource.Size = new System.Drawing.Size(204, 17);
            this.rbLocalSource.TabIndex = 4;
            this.rbLocalSource.TabStop = true;
            this.rbLocalSource.Text = "Программный источник (сценарий)";
            this.rbLocalSource.UseVisualStyleBackColor = true;
            this.rbLocalSource.CheckedChanged += new System.EventHandler(this.rbSlide_CheckedChanged);
            // 
            // rbDevice
            // 
            this.rbDevice.AutoSize = true;
            this.rbDevice.Location = new System.Drawing.Point(21, 75);
            this.rbDevice.Name = "rbDevice";
            this.rbDevice.Size = new System.Drawing.Size(98, 17);
            this.rbDevice.TabIndex = 3;
            this.rbDevice.TabStop = true;
            this.rbDevice.Text = "Оборудование";
            this.rbDevice.UseVisualStyleBackColor = true;
            this.rbDevice.CheckedChanged += new System.EventHandler(this.rbSlide_CheckedChanged);
            // 
            // rbDisplay
            // 
            this.rbDisplay.AutoSize = true;
            this.rbDisplay.Location = new System.Drawing.Point(21, 52);
            this.rbDisplay.Name = "rbDisplay";
            this.rbDisplay.Size = new System.Drawing.Size(70, 17);
            this.rbDisplay.TabIndex = 2;
            this.rbDisplay.TabStop = true;
            this.rbDisplay.Text = "Дисплей";
            this.rbDisplay.UseVisualStyleBackColor = true;
            this.rbDisplay.CheckedChanged += new System.EventHandler(this.rbSlide_CheckedChanged);
            // 
            // rbSlide
            // 
            this.rbSlide.AutoSize = true;
            this.rbSlide.Checked = true;
            this.rbSlide.Location = new System.Drawing.Point(21, 29);
            this.rbSlide.Name = "rbSlide";
            this.rbSlide.Size = new System.Drawing.Size(56, 17);
            this.rbSlide.TabIndex = 1;
            this.rbSlide.TabStop = true;
            this.rbSlide.Text = "Сцена";
            this.rbSlide.UseVisualStyleBackColor = true;
            this.rbSlide.CheckedChanged += new System.EventHandler(this.rbSlide_CheckedChanged);
            // 
            // buttonAdv2
            // 
            this.buttonAdv2.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv2.Location = new System.Drawing.Point(332, 309);
            this.buttonAdv2.Name = "buttonAdv2";
            this.buttonAdv2.Size = new System.Drawing.Size(75, 23);
            this.buttonAdv2.TabIndex = 11;
            this.buttonAdv2.Text = "Отмена";
            this.buttonAdv2.UseVisualStyle = true;
            this.buttonAdv2.Click += new System.EventHandler(this.buttonAdv2_Click);
            // 
            // buttonAdv1
            // 
            this.buttonAdv1.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv1.BackColor = System.Drawing.SystemColors.Control;
            this.buttonAdv1.Location = new System.Drawing.Point(247, 309);
            this.buttonAdv1.Name = "buttonAdv1";
            this.buttonAdv1.Size = new System.Drawing.Size(75, 23);
            this.buttonAdv1.TabIndex = 10;
            this.buttonAdv1.Text = "Найти";
            this.buttonAdv1.UseVisualStyle = true;
            this.buttonAdv1.Click += new System.EventHandler(this.buttonAdv1_Click);
            // 
            // autoLabel2
            // 
            this.autoLabel2.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel2.DX = 0;
            this.autoLabel2.DY = 0;
            this.autoLabel2.Location = new System.Drawing.Point(10, 165);
            this.autoLabel2.Name = "autoLabel2";
            this.autoLabel2.Size = new System.Drawing.Size(77, 13);
            this.autoLabel2.TabIndex = 4;
            this.autoLabel2.Text = "Комментарий";
            // 
            // autoLabel1
            // 
            this.autoLabel1.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel1.DX = 0;
            this.autoLabel1.DY = 0;
            this.autoLabel1.Location = new System.Drawing.Point(10, 130);
            this.autoLabel1.Name = "autoLabel1";
            this.autoLabel1.Size = new System.Drawing.Size(57, 13);
            this.autoLabel1.TabIndex = 4;
            this.autoLabel1.Text = "Название";
            // 
            // textBoxExt2
            // 
            this.textBoxExt2.Location = new System.Drawing.Point(93, 165);
            this.textBoxExt2.MaxLength = 500;
            this.textBoxExt2.Multiline = true;
            this.textBoxExt2.Name = "textBoxExt2";
            this.textBoxExt2.OverflowIndicatorToolTipText = null;
            this.textBoxExt2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxExt2.Size = new System.Drawing.Size(314, 87);
            this.textBoxExt2.TabIndex = 8;
            this.textBoxExt2.TextChanged += new System.EventHandler(this.textBoxExt1_TextChanged);
            // 
            // textBoxExt1
            // 
            this.textBoxExt1.Location = new System.Drawing.Point(93, 126);
            this.textBoxExt1.MaxLength = 100;
            this.textBoxExt1.Name = "textBoxExt1";
            this.textBoxExt1.OverflowIndicatorToolTipText = null;
            this.textBoxExt1.Size = new System.Drawing.Size(314, 20);
            this.textBoxExt1.TabIndex = 7;
            this.textBoxExt1.TextChanged += new System.EventHandler(this.textBoxExt1_TextChanged);
            // 
            // FindItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 346);
            this.Controls.Add(this.autoLabel3);
            this.Controls.Add(this.tbAuthor);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonAdv2);
            this.Controls.Add(this.buttonAdv1);
            this.Controls.Add(this.autoLabel2);
            this.Controls.Add(this.autoLabel1);
            this.Controls.Add(this.textBoxExt2);
            this.Controls.Add(this.textBoxExt1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindItemForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Поиск";
            this.UseOffice2007SchemeBackColor = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindItemForm_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv2;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv1;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel2;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel1;
        private Syncfusion.Windows.Forms.Tools.TextBoxExt textBoxExt2;
        private Syncfusion.Windows.Forms.Tools.TextBoxExt textBoxExt1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbHardSource;
        private System.Windows.Forms.RadioButton rbGlobalSource;
        private System.Windows.Forms.RadioButton rbLocalSource;
        private System.Windows.Forms.RadioButton rbDevice;
        private System.Windows.Forms.RadioButton rbDisplay;
        private System.Windows.Forms.RadioButton rbSlide;
        private Syncfusion.Windows.Forms.Tools.TextBoxExt tbAuthor;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel3;
    }
}