namespace UI.PresentationDesign.DesignUI.Forms
{
    partial class PresentationPropertiesForm
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
            this.commentText = new System.Windows.Forms.TextBox();
            this.authorText = new System.Windows.Forms.TextBox();
            this.nameText = new System.Windows.Forms.TextBox();
            this.createdLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cancelButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.okButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.label8 = new System.Windows.Forms.Label();
            this.modifiedLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.slideCountLabel = new System.Windows.Forms.Label();
            this.superToolTip1 = new Syncfusion.Windows.Forms.Tools.SuperToolTip(this);
            this.SuspendLayout();
            // 
            // commentText
            // 
            this.commentText.Location = new System.Drawing.Point(118, 32);
            this.commentText.MaxLength = 500;
            this.commentText.Multiline = true;
            this.commentText.Name = "commentText";
            this.commentText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.commentText.Size = new System.Drawing.Size(338, 72);
            this.commentText.TabIndex = 1;
            // 
            // authorText
            // 
            this.authorText.Location = new System.Drawing.Point(118, 110);
            this.authorText.MaxLength = 50;
            this.authorText.Name = "authorText";
            this.authorText.Size = new System.Drawing.Size(338, 20);
            this.authorText.TabIndex = 2;
            // 
            // nameText
            // 
            this.nameText.Location = new System.Drawing.Point(118, 6);
            this.nameText.MaxLength = 100;
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(338, 20);
            this.nameText.TabIndex = 0;
            this.nameText.Validating += new System.ComponentModel.CancelEventHandler(this.nameText_Validating);
            // 
            // createdLabel
            // 
            this.createdLabel.AutoSize = true;
            this.createdLabel.BackColor = System.Drawing.Color.Transparent;
            this.createdLabel.Location = new System.Drawing.Point(115, 146);
            this.createdLabel.Name = "createdLabel";
            this.createdLabel.Size = new System.Drawing.Size(94, 13);
            this.createdLabel.TabIndex = 9;
            this.createdLabel.Text = "00.00.00 00:00:00";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(12, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Создан";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(12, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Комментарий";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(12, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Автор";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Название *";
            // 
            // cancelButton
            // 
            this.cancelButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(381, 226);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyle = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(300, 226);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyle = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(12, 169);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Изменен";
            // 
            // modifiedLabel
            // 
            this.modifiedLabel.AutoSize = true;
            this.modifiedLabel.BackColor = System.Drawing.Color.Transparent;
            this.modifiedLabel.Location = new System.Drawing.Point(115, 169);
            this.modifiedLabel.Name = "modifiedLabel";
            this.modifiedLabel.Size = new System.Drawing.Size(94, 13);
            this.modifiedLabel.TabIndex = 9;
            this.modifiedLabel.Text = "00.00.00 00:00:00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(12, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Число сцен";
            // 
            // slideCountLabel
            // 
            this.slideCountLabel.AutoSize = true;
            this.slideCountLabel.BackColor = System.Drawing.Color.Transparent;
            this.slideCountLabel.Location = new System.Drawing.Point(115, 191);
            this.slideCountLabel.Name = "slideCountLabel";
            this.slideCountLabel.Size = new System.Drawing.Size(13, 13);
            this.slideCountLabel.TabIndex = 9;
            this.slideCountLabel.Text = "0";
            // 
            // superToolTip1
            // 
            this.superToolTip1.UseFading = Syncfusion.Windows.Forms.Tools.SuperToolTip.FadingType.System;
            // 
            // PresentationPropertiesForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(471, 250);
            this.Controls.Add(this.commentText);
            this.Controls.Add(this.authorText);
            this.Controls.Add(this.nameText);
            this.Controls.Add(this.slideCountLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.modifiedLabel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.createdLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PresentationPropertiesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "props";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox commentText;
        private System.Windows.Forms.TextBox authorText;
        private System.Windows.Forms.TextBox nameText;
        private System.Windows.Forms.Label createdLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Syncfusion.Windows.Forms.ButtonAdv cancelButton;
        private Syncfusion.Windows.Forms.ButtonAdv okButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label modifiedLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label slideCountLabel;
        private Syncfusion.Windows.Forms.Tools.SuperToolTip superToolTip1;
    }
}