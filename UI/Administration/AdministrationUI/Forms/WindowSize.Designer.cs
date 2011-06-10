namespace UI.Administration.AdministrationUI.Forms
{
    partial class WindowSize
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
            this.numericUpDownExt1 = new Syncfusion.Windows.Forms.Tools.NumericUpDownExt();
            this.numericUpDownExt2 = new Syncfusion.Windows.Forms.Tools.NumericUpDownExt();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ExitButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.OkButton = new Syncfusion.Windows.Forms.ButtonAdv();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExt1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExt2)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownExt1
            // 
            this.numericUpDownExt1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.numericUpDownExt1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(197)))), ((int)(((byte)(218)))));
            this.numericUpDownExt1.Location = new System.Drawing.Point(64, 9);
            this.numericUpDownExt1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownExt1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownExt1.Name = "numericUpDownExt1";
            this.numericUpDownExt1.Size = new System.Drawing.Size(156, 20);
            this.numericUpDownExt1.TabIndex = 0;
            this.numericUpDownExt1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownExt1.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.numericUpDownExt1.Validated += new System.EventHandler(this.numericUpDownExt1_ValueChanged);
            // 
            // numericUpDownExt2
            // 
            this.numericUpDownExt2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.numericUpDownExt2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(197)))), ((int)(((byte)(218)))));
            this.numericUpDownExt2.Location = new System.Drawing.Point(64, 35);
            this.numericUpDownExt2.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownExt2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownExt2.Name = "numericUpDownExt2";
            this.numericUpDownExt2.Size = new System.Drawing.Size(156, 20);
            this.numericUpDownExt2.TabIndex = 2;
            this.numericUpDownExt2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownExt2.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.numericUpDownExt2.Validated += new System.EventHandler(this.numericUpDownExt2_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Высота";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ширина";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.Location = new System.Drawing.Point(145, 61);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 5;
            this.ExitButton.Text = "Отменить";
            this.ExitButton.UseVisualStyle = true;
            this.ExitButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(64, 61);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 6;
            this.OkButton.Text = "Сохранить";
            this.OkButton.UseVisualStyle = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // WindowSize
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ExitButton;
            this.ClientSize = new System.Drawing.Size(225, 84);
            this.ColorScheme = Syncfusion.Windows.Forms.Office2007Theme.Blue;
            this.ControlBox = false;
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownExt2);
            this.Controls.Add(this.numericUpDownExt1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WindowSize";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройка размеров изображения";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExt1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExt2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.NumericUpDownExt numericUpDownExt1;
        private Syncfusion.Windows.Forms.Tools.NumericUpDownExt numericUpDownExt2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Syncfusion.Windows.Forms.ButtonAdv ExitButton;
        private Syncfusion.Windows.Forms.ButtonAdv OkButton;
    }
}