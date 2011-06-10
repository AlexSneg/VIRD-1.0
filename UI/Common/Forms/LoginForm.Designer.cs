namespace UI.Common.CommonUI
{
	partial class LoginForm
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
            this.components = new System.ComponentModel.Container();
            Syncfusion.Windows.Forms.Tools.AutoLabel autoLabelName;
            Syncfusion.Windows.Forms.Tools.AutoLabel autoLabelPassword;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.textBoxName = new Syncfusion.Windows.Forms.Tools.TextBoxExt();
            this.textBoxPassword = new Syncfusion.Windows.Forms.Tools.TextBoxExt();
            this.btnOk = new Syncfusion.Windows.Forms.ButtonAdv();
            this.btnCancel = new Syncfusion.Windows.Forms.ButtonAdv();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            autoLabelName = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            autoLabelPassword = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // autoLabelName
            // 
            autoLabelName.BackColor = System.Drawing.Color.Transparent;
            autoLabelName.DX = -42;
            autoLabelName.DY = 3;
            autoLabelName.LabeledControl = this.textBoxName;
            autoLabelName.Location = new System.Drawing.Point(19, 17);
            autoLabelName.Name = "autoLabelName";
            autoLabelName.Size = new System.Drawing.Size(38, 13);
            autoLabelName.TabIndex = 0;
            autoLabelName.Text = "Логин";
            // 
            // textBoxName
            // 
            this.textBoxName.CausesValidation = false;
            this.textBoxName.Location = new System.Drawing.Point(61, 14);
            this.textBoxName.MaxLength = 50;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.OverflowIndicatorToolTipText = null;
            this.textBoxName.Size = new System.Drawing.Size(124, 20);
            this.textBoxName.TabIndex = 1;
            // 
            // autoLabelPassword
            // 
            autoLabelPassword.BackColor = System.Drawing.Color.Transparent;
            autoLabelPassword.DX = -49;
            autoLabelPassword.DY = 3;
            autoLabelPassword.LabeledControl = this.textBoxPassword;
            autoLabelPassword.Location = new System.Drawing.Point(12, 43);
            autoLabelPassword.Name = "autoLabelPassword";
            autoLabelPassword.Size = new System.Drawing.Size(45, 13);
            autoLabelPassword.TabIndex = 2;
            autoLabelPassword.Text = "Пароль";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(61, 40);
            this.textBoxPassword.MaxLength = 50;
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.OverflowIndicatorToolTipText = null;
            this.textBoxPassword.PasswordChar = '●';
            this.textBoxPassword.Size = new System.Drawing.Size(124, 20);
            this.textBoxPassword.TabIndex = 3;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOk.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.btnOk.Location = new System.Drawing.Point(12, 74);
            this.btnOk.Name = "btnOk";
            this.btnOk.Office2007ColorScheme = Syncfusion.Windows.Forms.Office2007Theme.Managed;
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Вход";
            this.btnOk.UseVisualStyle = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(110, 74);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Office2007ColorScheme = Syncfusion.Windows.Forms.Office2007Theme.Managed;
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyle = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(204, 109);
            this.Controls.Add(autoLabelPassword);
            this.Controls.Add(autoLabelName);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Вход в систему";
            this.UseOffice2007SchemeBackColor = true;
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private Syncfusion.Windows.Forms.ButtonAdv btnOk;
		private Syncfusion.Windows.Forms.ButtonAdv btnCancel;
		private Syncfusion.Windows.Forms.Tools.TextBoxExt textBoxName;
		private Syncfusion.Windows.Forms.Tools.TextBoxExt textBoxPassword;
		private System.Windows.Forms.ErrorProvider errorProvider;
	}
}