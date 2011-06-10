namespace UI.PresentationDesign.DesignUI.Forms
{
    partial class PreparePresentationForm
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
            this.cancelButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.detailsButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.okButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.detailsText = new Syncfusion.Windows.Forms.Tools.TextBoxExt();
            this.agentsProgressPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.cancelButton.Location = new System.Drawing.Point(315, 285);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Отмена";
            this.toolTip.SetToolTip(this.cancelButton, "Отменить весь процесс подготовки");
            this.cancelButton.UseVisualStyle = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // detailsButton
            // 
            this.detailsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.detailsButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.detailsButton.Image = global::UI.PresentationDesign.DesignUI.Properties.Resources.down;
            this.detailsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.detailsButton.Location = new System.Drawing.Point(396, 285);
            this.detailsButton.Name = "detailsButton";
            this.detailsButton.Size = new System.Drawing.Size(75, 23);
            this.detailsButton.TabIndex = 2;
            this.detailsButton.Text = "Детали";
            this.detailsButton.UseVisualStyle = true;
            this.detailsButton.Click += new System.EventHandler(this.buttonAdv1_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.okButton.Location = new System.Drawing.Point(477, 285);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(52, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "ОК";
            this.toolTip.SetToolTip(this.okButton, "Закрыть окно");
            this.okButton.UseVisualStyle = true;
            this.okButton.Visible = false;
            this.okButton.VisibleChanged += new System.EventHandler(this.okButton_VisibleChanged);
            this.okButton.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // detailsText
            // 
            this.detailsText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.detailsText.BackColor = System.Drawing.Color.White;
            this.detailsText.Location = new System.Drawing.Point(9, 331);
            this.detailsText.Margin = new System.Windows.Forms.Padding(0);
            this.detailsText.Multiline = true;
            this.detailsText.Name = "detailsText";
            this.detailsText.OverflowIndicatorToolTipText = null;
            this.detailsText.ReadOnly = true;
            this.detailsText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.detailsText.Size = new System.Drawing.Size(520, 133);
            this.detailsText.TabIndex = 3;
            // 
            // agentsProgressPanel
            // 
            this.agentsProgressPanel.AutoScroll = true;
            this.agentsProgressPanel.BackColor = System.Drawing.Color.Transparent;
            this.agentsProgressPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.agentsProgressPanel.Location = new System.Drawing.Point(3, 16);
            this.agentsProgressPanel.Name = "agentsProgressPanel";
            this.agentsProgressPanel.Size = new System.Drawing.Size(514, 236);
            this.agentsProgressPanel.TabIndex = 4;
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox.BackColor = System.Drawing.Color.Transparent;
            this.groupBox.Controls.Add(this.agentsProgressPanel);
            this.groupBox.Location = new System.Drawing.Point(9, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(520, 255);
            this.groupBox.TabIndex = 5;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Выполняется проверка дисплеев";
            // 
            // PreparePresentationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 473);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.detailsText);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.detailsButton);
            this.Controls.Add(this.cancelButton);
            this.MinimumSize = new System.Drawing.Size(550, 230);
            this.Name = "PreparePresentationForm";
            this.Text = "Подготовка сценария";
            this.UseOffice2007SchemeBackColor = true;
            this.Load += new System.EventHandler(this.PreparePresentationForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PreparePresentationForm_FormClosing);
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv cancelButton;
        private Syncfusion.Windows.Forms.ButtonAdv detailsButton;
        private Syncfusion.Windows.Forms.ButtonAdv okButton;
        private Syncfusion.Windows.Forms.Tools.TextBoxExt detailsText;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.FlowLayoutPanel agentsProgressPanel;
        private System.Windows.Forms.GroupBox groupBox;
    }
}