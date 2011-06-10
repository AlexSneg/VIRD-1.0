namespace DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator
{
    partial class SourceHardPluginBaseControl
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
            if ((disposing) && (_controller != null))
            {
                _controller.Dispose();
                _controller = null;
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
            this.gpTop = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.alStatus = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.baRGBOption = new Syncfusion.Windows.Forms.ButtonAdv();
            this.gpRGBOption = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.autoLabel10 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.nudOffsetH = new Syncfusion.Windows.Forms.Tools.NumericUpDownExt();
            this.autoLabel9 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.nudActiveH = new Syncfusion.Windows.Forms.Tools.NumericUpDownExt();
            this.autoLabel8 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.nudTotalH = new Syncfusion.Windows.Forms.Tools.NumericUpDownExt();
            this.buttonAdv1 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.autoLabel7 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel6 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel5 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel4 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel3 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.nudVFreq = new Syncfusion.Windows.Forms.Tools.NumericUpDownExt();
            this.nudOffsetW = new Syncfusion.Windows.Forms.Tools.NumericUpDownExt();
            this.nudActiveW = new Syncfusion.Windows.Forms.Tools.NumericUpDownExt();
            this.autoLabel2 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel1 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.nudTotalW = new Syncfusion.Windows.Forms.Tools.NumericUpDownExt();
            this.gpDetail = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            ((System.ComponentModel.ISupportInitialize)(this.gpTop)).BeginInit();
            this.gpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gpRGBOption)).BeginInit();
            this.gpRGBOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffsetH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudActiveH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotalH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVFreq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffsetW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudActiveW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotalW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // gpTop
            // 
            this.gpTop.BorderColor = System.Drawing.Color.Black;
            this.gpTop.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gpTop.Controls.Add(this.alStatus);
            this.gpTop.Controls.Add(this.baRGBOption);
            this.gpTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpTop.Location = new System.Drawing.Point(3, 3);
            this.gpTop.Name = "gpTop";
            this.gpTop.Size = new System.Drawing.Size(210, 29);
            this.gpTop.TabIndex = 1;
            // 
            // alStatus
            // 
            this.alStatus.AutoSize = false;
            this.alStatus.DX = 0;
            this.alStatus.DY = 0;
            this.alStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alStatus.ForeColor = System.Drawing.Color.Red;
            this.alStatus.Location = new System.Drawing.Point(119, 8);
            this.alStatus.Name = "alStatus";
            this.alStatus.Size = new System.Drawing.Size(80, 13);
            this.alStatus.TabIndex = 2;
            this.alStatus.Text = "нет связи";
            this.alStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baRGBOption
            // 
            this.baRGBOption.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.baRGBOption.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.baRGBOption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baRGBOption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.baRGBOption.ForeColor = System.Drawing.Color.MidnightBlue;
            this.baRGBOption.Location = new System.Drawing.Point(3, 3);
            this.baRGBOption.Name = "baRGBOption";
            this.baRGBOption.PushButton = true;
            this.baRGBOption.Size = new System.Drawing.Size(110, 23);
            this.baRGBOption.TabIndex = 1;
            this.baRGBOption.Text = "Настройки RGB <<";
            this.baRGBOption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.baRGBOption.UseVisualStyle = true;
            this.baRGBOption.Click += new System.EventHandler(this.baRGBOption_Click);
            // 
            // gpRGBOption
            // 
            this.gpRGBOption.Border3DStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.gpRGBOption.BorderColor = System.Drawing.Color.Black;
            this.gpRGBOption.Controls.Add(this.autoLabel10);
            this.gpRGBOption.Controls.Add(this.nudOffsetH);
            this.gpRGBOption.Controls.Add(this.autoLabel9);
            this.gpRGBOption.Controls.Add(this.nudActiveH);
            this.gpRGBOption.Controls.Add(this.autoLabel8);
            this.gpRGBOption.Controls.Add(this.nudTotalH);
            this.gpRGBOption.Controls.Add(this.buttonAdv1);
            this.gpRGBOption.Controls.Add(this.autoLabel7);
            this.gpRGBOption.Controls.Add(this.autoLabel6);
            this.gpRGBOption.Controls.Add(this.autoLabel5);
            this.gpRGBOption.Controls.Add(this.autoLabel4);
            this.gpRGBOption.Controls.Add(this.autoLabel3);
            this.gpRGBOption.Controls.Add(this.nudVFreq);
            this.gpRGBOption.Controls.Add(this.nudOffsetW);
            this.gpRGBOption.Controls.Add(this.nudActiveW);
            this.gpRGBOption.Controls.Add(this.autoLabel2);
            this.gpRGBOption.Controls.Add(this.autoLabel1);
            this.gpRGBOption.Controls.Add(this.nudTotalW);
            this.gpRGBOption.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpRGBOption.Location = new System.Drawing.Point(3, 32);
            this.gpRGBOption.Name = "gpRGBOption";
            this.gpRGBOption.Size = new System.Drawing.Size(210, 124);
            this.gpRGBOption.TabIndex = 2;
            // 
            // autoLabel10
            // 
            this.autoLabel10.DX = 0;
            this.autoLabel10.DY = 0;
            this.autoLabel10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel10.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel10.Location = new System.Drawing.Point(130, 52);
            this.autoLabel10.Name = "autoLabel10";
            this.autoLabel10.Size = new System.Drawing.Size(14, 13);
            this.autoLabel10.TabIndex = 17;
            this.autoLabel10.Text = "H";
            // 
            // nudOffsetH
            // 
            this.nudOffsetH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.nudOffsetH.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.nudOffsetH.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(197)))), ((int)(((byte)(218)))));
            this.nudOffsetH.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudOffsetH.ForeColor = System.Drawing.Color.MidnightBlue;
            this.nudOffsetH.Location = new System.Drawing.Point(145, 49);
            this.nudOffsetH.Maximum = new decimal(new int[] {
            3200,
            0,
            0,
            0});
            this.nudOffsetH.Name = "nudOffsetH";
            this.nudOffsetH.Size = new System.Drawing.Size(54, 21);
            this.nudOffsetH.TabIndex = 5;
            this.nudOffsetH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudOffsetH.Value = new decimal(new int[] {
            768,
            0,
            0,
            0});
            this.nudOffsetH.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            // 
            // autoLabel9
            // 
            this.autoLabel9.DX = 0;
            this.autoLabel9.DY = 0;
            this.autoLabel9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel9.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel9.Location = new System.Drawing.Point(130, 29);
            this.autoLabel9.Name = "autoLabel9";
            this.autoLabel9.Size = new System.Drawing.Size(14, 13);
            this.autoLabel9.TabIndex = 15;
            this.autoLabel9.Text = "H";
            // 
            // nudActiveH
            // 
            this.nudActiveH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.nudActiveH.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.nudActiveH.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(197)))), ((int)(((byte)(218)))));
            this.nudActiveH.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudActiveH.ForeColor = System.Drawing.Color.MidnightBlue;
            this.nudActiveH.Location = new System.Drawing.Point(145, 26);
            this.nudActiveH.Maximum = new decimal(new int[] {
            3200,
            0,
            0,
            0});
            this.nudActiveH.Name = "nudActiveH";
            this.nudActiveH.Size = new System.Drawing.Size(54, 21);
            this.nudActiveH.TabIndex = 3;
            this.nudActiveH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudActiveH.Value = new decimal(new int[] {
            768,
            0,
            0,
            0});
            this.nudActiveH.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            // 
            // autoLabel8
            // 
            this.autoLabel8.DX = 0;
            this.autoLabel8.DY = 0;
            this.autoLabel8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel8.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel8.Location = new System.Drawing.Point(130, 7);
            this.autoLabel8.Name = "autoLabel8";
            this.autoLabel8.Size = new System.Drawing.Size(14, 13);
            this.autoLabel8.TabIndex = 13;
            this.autoLabel8.Text = "H";
            // 
            // nudTotalH
            // 
            this.nudTotalH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.nudTotalH.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.nudTotalH.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(197)))), ((int)(((byte)(218)))));
            this.nudTotalH.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudTotalH.ForeColor = System.Drawing.Color.MidnightBlue;
            this.nudTotalH.Location = new System.Drawing.Point(145, 3);
            this.nudTotalH.Maximum = new decimal(new int[] {
            3200,
            0,
            0,
            0});
            this.nudTotalH.Name = "nudTotalH";
            this.nudTotalH.Size = new System.Drawing.Size(54, 21);
            this.nudTotalH.TabIndex = 1;
            this.nudTotalH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudTotalH.Value = new decimal(new int[] {
            768,
            0,
            0,
            0});
            this.nudTotalH.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            // 
            // buttonAdv1
            // 
            this.buttonAdv1.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAdv1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.buttonAdv1.Location = new System.Drawing.Point(56, 96);
            this.buttonAdv1.Name = "buttonAdv1";
            this.buttonAdv1.Size = new System.Drawing.Size(89, 21);
            this.buttonAdv1.TabIndex = 7;
            this.buttonAdv1.Text = "Сохранить";
            this.buttonAdv1.UseVisualStyle = true;
            this.buttonAdv1.Click += new System.EventHandler(this.buttonAdv1_Click);
            // 
            // autoLabel7
            // 
            this.autoLabel7.DX = 0;
            this.autoLabel7.DY = 0;
            this.autoLabel7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel7.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel7.Location = new System.Drawing.Point(52, 52);
            this.autoLabel7.Name = "autoLabel7";
            this.autoLabel7.Size = new System.Drawing.Size(17, 13);
            this.autoLabel7.TabIndex = 10;
            this.autoLabel7.Text = "W";
            // 
            // autoLabel6
            // 
            this.autoLabel6.DX = 0;
            this.autoLabel6.DY = 0;
            this.autoLabel6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel6.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel6.Location = new System.Drawing.Point(52, 29);
            this.autoLabel6.Name = "autoLabel6";
            this.autoLabel6.Size = new System.Drawing.Size(17, 13);
            this.autoLabel6.TabIndex = 9;
            this.autoLabel6.Text = "W";
            // 
            // autoLabel5
            // 
            this.autoLabel5.DX = 0;
            this.autoLabel5.DY = 0;
            this.autoLabel5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel5.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel5.Location = new System.Drawing.Point(3, 75);
            this.autoLabel5.Name = "autoLabel5";
            this.autoLabel5.Size = new System.Drawing.Size(43, 13);
            this.autoLabel5.TabIndex = 8;
            this.autoLabel5.Text = "V FREQ";
            // 
            // autoLabel4
            // 
            this.autoLabel4.DX = 0;
            this.autoLabel4.DY = 0;
            this.autoLabel4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel4.Location = new System.Drawing.Point(3, 52);
            this.autoLabel4.Name = "autoLabel4";
            this.autoLabel4.Size = new System.Drawing.Size(45, 13);
            this.autoLabel4.TabIndex = 7;
            this.autoLabel4.Text = "OFFSET";
            // 
            // autoLabel3
            // 
            this.autoLabel3.DX = 0;
            this.autoLabel3.DY = 0;
            this.autoLabel3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel3.Location = new System.Drawing.Point(3, 29);
            this.autoLabel3.Name = "autoLabel3";
            this.autoLabel3.Size = new System.Drawing.Size(43, 13);
            this.autoLabel3.TabIndex = 6;
            this.autoLabel3.Text = "ACTIVE";
            // 
            // nudVFreq
            // 
            this.nudVFreq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.nudVFreq.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.nudVFreq.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(197)))), ((int)(((byte)(218)))));
            this.nudVFreq.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudVFreq.ForeColor = System.Drawing.Color.MidnightBlue;
            this.nudVFreq.Location = new System.Drawing.Point(70, 72);
            this.nudVFreq.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.nudVFreq.Name = "nudVFreq";
            this.nudVFreq.Size = new System.Drawing.Size(54, 21);
            this.nudVFreq.TabIndex = 6;
            this.nudVFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudVFreq.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudVFreq.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            // 
            // nudOffsetW
            // 
            this.nudOffsetW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.nudOffsetW.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.nudOffsetW.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(197)))), ((int)(((byte)(218)))));
            this.nudOffsetW.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudOffsetW.ForeColor = System.Drawing.Color.MidnightBlue;
            this.nudOffsetW.Location = new System.Drawing.Point(70, 49);
            this.nudOffsetW.Maximum = new decimal(new int[] {
            5120,
            0,
            0,
            0});
            this.nudOffsetW.Name = "nudOffsetW";
            this.nudOffsetW.Size = new System.Drawing.Size(54, 21);
            this.nudOffsetW.TabIndex = 4;
            this.nudOffsetW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudOffsetW.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nudOffsetW.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            // 
            // nudActiveW
            // 
            this.nudActiveW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.nudActiveW.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.nudActiveW.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(197)))), ((int)(((byte)(218)))));
            this.nudActiveW.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudActiveW.ForeColor = System.Drawing.Color.MidnightBlue;
            this.nudActiveW.Location = new System.Drawing.Point(70, 26);
            this.nudActiveW.Maximum = new decimal(new int[] {
            5120,
            0,
            0,
            0});
            this.nudActiveW.Name = "nudActiveW";
            this.nudActiveW.Size = new System.Drawing.Size(54, 21);
            this.nudActiveW.TabIndex = 2;
            this.nudActiveW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudActiveW.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nudActiveW.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            // 
            // autoLabel2
            // 
            this.autoLabel2.DX = 0;
            this.autoLabel2.DY = 0;
            this.autoLabel2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel2.Location = new System.Drawing.Point(52, 7);
            this.autoLabel2.Name = "autoLabel2";
            this.autoLabel2.Size = new System.Drawing.Size(17, 13);
            this.autoLabel2.TabIndex = 2;
            this.autoLabel2.Text = "W";
            // 
            // autoLabel1
            // 
            this.autoLabel1.DX = 0;
            this.autoLabel1.DY = 0;
            this.autoLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel1.Location = new System.Drawing.Point(3, 7);
            this.autoLabel1.Name = "autoLabel1";
            this.autoLabel1.Size = new System.Drawing.Size(39, 13);
            this.autoLabel1.TabIndex = 1;
            this.autoLabel1.Text = "TOTAL";
            // 
            // nudTotalW
            // 
            this.nudTotalW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.nudTotalW.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.nudTotalW.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(197)))), ((int)(((byte)(218)))));
            this.nudTotalW.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudTotalW.ForeColor = System.Drawing.Color.MidnightBlue;
            this.nudTotalW.Location = new System.Drawing.Point(70, 3);
            this.nudTotalW.Maximum = new decimal(new int[] {
            5120,
            0,
            0,
            0});
            this.nudTotalW.Name = "nudTotalW";
            this.nudTotalW.Size = new System.Drawing.Size(54, 21);
            this.nudTotalW.TabIndex = 0;
            this.nudTotalW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudTotalW.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nudTotalW.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            // 
            // gpDetail
            // 
            this.gpDetail.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gpDetail.BorderColor = System.Drawing.Color.Black;
            this.gpDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpDetail.Location = new System.Drawing.Point(3, 156);
            this.gpDetail.Name = "gpDetail";
            this.gpDetail.Size = new System.Drawing.Size(210, 168);
            this.gpDetail.TabIndex = 3;
            // 
            // SourceHardPluginBaseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.Controls.Add(this.gpDetail);
            this.Controls.Add(this.gpRGBOption);
            this.Controls.Add(this.gpTop);
            this.Name = "SourceHardPluginBaseControl";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(216, 327);
            ((System.ComponentModel.ISupportInitialize)(this.gpTop)).EndInit();
            this.gpTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gpRGBOption)).EndInit();
            this.gpRGBOption.ResumeLayout(false);
            this.gpRGBOption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffsetH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudActiveH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotalH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVFreq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffsetW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudActiveW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotalW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientPanel gpTop;
        private Syncfusion.Windows.Forms.ButtonAdv baRGBOption;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gpRGBOption;
        private Syncfusion.Windows.Forms.Tools.NumericUpDownExt nudTotalW;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel1;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel3;
        private Syncfusion.Windows.Forms.Tools.NumericUpDownExt nudVFreq;
        private Syncfusion.Windows.Forms.Tools.NumericUpDownExt nudOffsetW;
        private Syncfusion.Windows.Forms.Tools.NumericUpDownExt nudActiveW;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel2;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel4;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel7;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel6;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel5;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv1;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel10;
        private Syncfusion.Windows.Forms.Tools.NumericUpDownExt nudOffsetH;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel9;
        private Syncfusion.Windows.Forms.Tools.NumericUpDownExt nudActiveH;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel8;
        private Syncfusion.Windows.Forms.Tools.NumericUpDownExt nudTotalH;
        protected Syncfusion.Windows.Forms.Tools.GradientPanel gpDetail;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alStatus;

    }
}
