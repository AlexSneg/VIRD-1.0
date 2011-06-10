namespace UI.PresentationDesign.DesignUI.Forms
{
    partial class SlidePropertiesForm
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
            this.okButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.cancelButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.modifiedLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.isStartupCheckBox = new System.Windows.Forms.CheckBox();
            this.nameText = new System.Windows.Forms.TextBox();
            this.authorText = new System.Windows.Forms.TextBox();
            this.commentText = new System.Windows.Forms.TextBox();
            this.nextSlideList = new System.Windows.Forms.ComboBox();
            this.labelsList = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.secondSpanEdit = new Syncfusion.Windows.Forms.Tools.NumericUpDownExt();
            this.minuteSpanEdit = new Syncfusion.Windows.Forms.Tools.NumericUpDownExt();
            this.hourSpanEdit = new Syncfusion.Windows.Forms.Tools.NumericUpDownExt();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.secondSpanEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minuteSpanEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hourSpanEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.okButton.Location = new System.Drawing.Point(344, 201);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyle = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(425, 201);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyle = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Название *";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(340, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Автор";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(12, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Комментарий";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(12, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Метка";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(12, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Изменен";
            // 
            // modifiedLabel
            // 
            this.modifiedLabel.AutoSize = true;
            this.modifiedLabel.BackColor = System.Drawing.Color.Transparent;
            this.modifiedLabel.Location = new System.Drawing.Point(115, 187);
            this.modifiedLabel.Name = "modifiedLabel";
            this.modifiedLabel.Size = new System.Drawing.Size(94, 13);
            this.modifiedLabel.TabIndex = 1;
            this.modifiedLabel.Text = "00.00.00 00:00:00";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(12, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 36);
            this.label4.TabIndex = 1;
            this.label4.Text = "Следующая сцена по умолчанию";
            // 
            // isStartupCheckBox
            // 
            this.isStartupCheckBox.AutoSize = true;
            this.isStartupCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.isStartupCheckBox.Location = new System.Drawing.Point(380, 123);
            this.isStartupCheckBox.Name = "isStartupCheckBox";
            this.isStartupCheckBox.Size = new System.Drawing.Size(114, 17);
            this.isStartupCheckBox.TabIndex = 4;
            this.isStartupCheckBox.Text = "Начальная сцена";
            this.isStartupCheckBox.UseVisualStyleBackColor = false;
            this.isStartupCheckBox.CheckedChanged += new System.EventHandler(this.ValueChanged);
            // 
            // nameText
            // 
            this.nameText.Location = new System.Drawing.Point(118, 6);
            this.nameText.MaxLength = 100;
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(203, 20);
            this.nameText.TabIndex = 0;
            // 
            // authorText
            // 
            this.authorText.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.authorText.Location = new System.Drawing.Point(383, 6);
            this.authorText.Name = "authorText";
            this.authorText.ReadOnly = true;
            this.authorText.Size = new System.Drawing.Size(113, 20);
            this.authorText.TabIndex = 1;
            // 
            // commentText
            // 
            this.commentText.Location = new System.Drawing.Point(118, 38);
            this.commentText.MaxLength = 500;
            this.commentText.Multiline = true;
            this.commentText.Name = "commentText";
            this.commentText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.commentText.Size = new System.Drawing.Size(378, 72);
            this.commentText.TabIndex = 2;
            // 
            // nextSlideList
            // 
            this.nextSlideList.DisplayMember = "SlideName";
            this.nextSlideList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nextSlideList.FormattingEnabled = true;
            this.nextSlideList.Location = new System.Drawing.Point(118, 119);
            this.nextSlideList.Name = "nextSlideList";
            this.nextSlideList.Size = new System.Drawing.Size(186, 21);
            this.nextSlideList.TabIndex = 3;
            this.nextSlideList.SelectedValueChanged += new System.EventHandler(this.nextSlideList_SelectedValueChanged);
            // 
            // labelsList
            // 
            this.labelsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.labelsList.Enabled = false;
            this.labelsList.FormattingEnabled = true;
            this.labelsList.Location = new System.Drawing.Point(118, 150);
            this.labelsList.Name = "labelsList";
            this.labelsList.Size = new System.Drawing.Size(186, 21);
            this.labelsList.TabIndex = 5;
            this.labelsList.SelectedValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.secondSpanEdit);
            this.groupBox1.Controls.Add(this.minuteSpanEdit);
            this.groupBox1.Controls.Add(this.hourSpanEdit);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(310, 146);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(190, 45);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Время начала сцены";
            // 
            // secondSpanEdit
            // 
            this.secondSpanEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.secondSpanEdit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(197)))), ((int)(((byte)(218)))));
            this.secondSpanEdit.Location = new System.Drawing.Point(132, 20);
            this.secondSpanEdit.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.secondSpanEdit.Name = "secondSpanEdit";
            this.secondSpanEdit.Size = new System.Drawing.Size(35, 20);
            this.secondSpanEdit.TabIndex = 2;
            this.secondSpanEdit.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.secondSpanEdit.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // minuteSpanEdit
            // 
            this.minuteSpanEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.minuteSpanEdit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(197)))), ((int)(((byte)(218)))));
            this.minuteSpanEdit.Location = new System.Drawing.Point(68, 20);
            this.minuteSpanEdit.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.minuteSpanEdit.Name = "minuteSpanEdit";
            this.minuteSpanEdit.Size = new System.Drawing.Size(38, 20);
            this.minuteSpanEdit.TabIndex = 1;
            this.minuteSpanEdit.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.minuteSpanEdit.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // hourSpanEdit
            // 
            this.hourSpanEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.hourSpanEdit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(197)))), ((int)(((byte)(218)))));
            this.hourSpanEdit.Location = new System.Drawing.Point(7, 20);
            this.hourSpanEdit.Name = "hourSpanEdit";
            this.hourSpanEdit.Size = new System.Drawing.Size(41, 20);
            this.hourSpanEdit.TabIndex = 0;
            this.hourSpanEdit.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.hourSpanEdit.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(164, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "сек";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(107, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "мин";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(50, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "ч";
            // 
            // SlidePropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(508, 236);
            this.Controls.Add(this.labelsList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.nextSlideList);
            this.Controls.Add(this.commentText);
            this.Controls.Add(this.authorText);
            this.Controls.Add(this.nameText);
            this.Controls.Add(this.isStartupCheckBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.modifiedLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SlidePropertiesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Свойства";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SlidePropertiesForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.secondSpanEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minuteSpanEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hourSpanEdit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv okButton;
        private Syncfusion.Windows.Forms.ButtonAdv cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label modifiedLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox isStartupCheckBox;
        private System.Windows.Forms.TextBox nameText;
        private System.Windows.Forms.TextBox authorText;
        private System.Windows.Forms.TextBox commentText;
        private System.Windows.Forms.ComboBox nextSlideList;
        private System.Windows.Forms.ComboBox labelsList;
        private System.Windows.Forms.GroupBox groupBox1;
        private Syncfusion.Windows.Forms.Tools.NumericUpDownExt secondSpanEdit;
        private Syncfusion.Windows.Forms.Tools.NumericUpDownExt minuteSpanEdit;
        private Syncfusion.Windows.Forms.Tools.NumericUpDownExt hourSpanEdit;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;

    }
}