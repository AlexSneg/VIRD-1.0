namespace Hosts.Plugins.BusinessGraphics.Player
{
    partial class ChartManageControl
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
            this.interactiveCheckBox = new System.Windows.Forms.CheckBox();
            this.makeDefaultButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // interactiveCheckBox
            // 
            this.interactiveCheckBox.AutoSize = true;
            this.interactiveCheckBox.Checked = true;
            this.interactiveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.interactiveCheckBox.Location = new System.Drawing.Point(16, 9);
            this.interactiveCheckBox.Name = "interactiveCheckBox";
            this.interactiveCheckBox.Size = new System.Drawing.Size(86, 17);
            this.interactiveCheckBox.TabIndex = 0;
            this.interactiveCheckBox.Text = "Интерактив";
            this.interactiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // makeDefaultButton
            // 
            this.makeDefaultButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.makeDefaultButton.Location = new System.Drawing.Point(48, 33);
            this.makeDefaultButton.Name = "makeDefaultButton";
            this.makeDefaultButton.Size = new System.Drawing.Size(106, 23);
            this.makeDefaultButton.TabIndex = 1;
            this.makeDefaultButton.Text = "По умолчанию";
            this.makeDefaultButton.UseVisualStyleBackColor = true;
            this.makeDefaultButton.Click += new System.EventHandler(this.makeDefaultButton_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ChartManageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.makeDefaultButton);
            this.Controls.Add(this.interactiveCheckBox);
            this.Name = "ChartManageControl";
            this.Size = new System.Drawing.Size(192, 64);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox interactiveCheckBox;
        private System.Windows.Forms.Button makeDefaultButton;
        private System.Windows.Forms.Timer timer1;
    }
}
