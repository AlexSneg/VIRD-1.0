namespace UI.ImportExport.ImportExportUI.Forms
{
    partial class PresentationExistsDialog
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
            this.buttonAdv1 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv2 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.radioButtonAdv1 = new Syncfusion.Windows.Forms.Tools.RadioButtonAdv();
            this.radioButtonAdv2 = new Syncfusion.Windows.Forms.Tools.RadioButtonAdv();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.radioButtonAdv1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioButtonAdv2)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonAdv1
            // 
            this.buttonAdv1.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonAdv1.Location = new System.Drawing.Point(85, 121);
            this.buttonAdv1.Name = "buttonAdv1";
            this.buttonAdv1.Size = new System.Drawing.Size(75, 23);
            this.buttonAdv1.TabIndex = 0;
            this.buttonAdv1.Text = "OK";
            this.buttonAdv1.UseVisualStyle = true;
            // 
            // buttonAdv2
            // 
            this.buttonAdv2.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.buttonAdv2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonAdv2.Location = new System.Drawing.Point(181, 121);
            this.buttonAdv2.Name = "buttonAdv2";
            this.buttonAdv2.Size = new System.Drawing.Size(75, 23);
            this.buttonAdv2.TabIndex = 0;
            this.buttonAdv2.Text = "Отмена";
            this.buttonAdv2.UseVisualStyle = true;
            // 
            // radioButtonAdv1
            // 
            this.radioButtonAdv1.BackColor = System.Drawing.Color.Transparent;
            this.radioButtonAdv1.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.radioButtonAdv1.GradientEnd = System.Drawing.SystemColors.ControlDark;
            this.radioButtonAdv1.GradientStart = System.Drawing.SystemColors.Control;
            this.radioButtonAdv1.HotBorderColor = System.Drawing.SystemColors.WindowFrame;
            this.radioButtonAdv1.ImageCheckBoxSize = new System.Drawing.Size(13, 13);
            this.radioButtonAdv1.Location = new System.Drawing.Point(13, 58);
            this.radioButtonAdv1.Name = "radioButtonAdv1";
            this.radioButtonAdv1.ShadowColor = System.Drawing.Color.Black;
            this.radioButtonAdv1.ShadowOffset = new System.Drawing.Point(2, 2);
            this.radioButtonAdv1.Size = new System.Drawing.Size(243, 21);
            this.radioButtonAdv1.Style = Syncfusion.Windows.Forms.Tools.RadioButtonAdvStyle.Office2007;
            this.radioButtonAdv1.TabIndex = 1;
            this.radioButtonAdv1.Text = "Заменить существующий сценарий";
            this.radioButtonAdv1.ThemesEnabled = false;
            // 
            // radioButtonAdv2
            // 
            this.radioButtonAdv2.BackColor = System.Drawing.Color.Transparent;
            this.radioButtonAdv2.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.radioButtonAdv2.Checked = true;
            this.radioButtonAdv2.GradientEnd = System.Drawing.SystemColors.ControlDark;
            this.radioButtonAdv2.GradientStart = System.Drawing.SystemColors.Control;
            this.radioButtonAdv2.HotBorderColor = System.Drawing.SystemColors.WindowFrame;
            this.radioButtonAdv2.ImageCheckBoxSize = new System.Drawing.Size(13, 13);
            this.radioButtonAdv2.Location = new System.Drawing.Point(12, 85);
            this.radioButtonAdv2.Name = "radioButtonAdv2";
            this.radioButtonAdv2.ShadowColor = System.Drawing.Color.Black;
            this.radioButtonAdv2.ShadowOffset = new System.Drawing.Point(2, 2);
            this.radioButtonAdv2.Size = new System.Drawing.Size(195, 21);
            this.radioButtonAdv2.Style = Syncfusion.Windows.Forms.Tools.RadioButtonAdvStyle.Office2007;
            this.radioButtonAdv2.TabIndex = 1;
            this.radioButtonAdv2.Text = "Сделать копию";
            this.radioButtonAdv2.ThemesEnabled = false;
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "RESOURCE_NAME";
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(9, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(315, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Сценарий уже существует в системе. Выберите действие:";
            // 
            // PresentationExistsDialog
            // 
            this.AcceptButton = this.buttonAdv1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonAdv2;
            this.ClientSize = new System.Drawing.Size(318, 151);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonAdv2);
            this.Controls.Add(this.radioButtonAdv1);
            this.Controls.Add(this.buttonAdv2);
            this.Controls.Add(this.buttonAdv1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PresentationExistsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подтверждение";
            this.UseOffice2007SchemeBackColor = true;
            ((System.ComponentModel.ISupportInitialize)(this.radioButtonAdv1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioButtonAdv2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv1;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv2;
        private Syncfusion.Windows.Forms.Tools.RadioButtonAdv radioButtonAdv1;
        private Syncfusion.Windows.Forms.Tools.RadioButtonAdv radioButtonAdv2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}