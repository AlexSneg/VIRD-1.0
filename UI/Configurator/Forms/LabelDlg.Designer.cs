namespace UI.PresentationDesign.ConfiguratorUI.Forms
{
    partial class LabelDlg
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
            this.labelControl1 = new UI.PresentationDesign.ConfiguratorUI.Controls.LabelControl();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl1.Location = new System.Drawing.Point(0, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(593, 513);
            this.labelControl1.TabIndex = 0;
            // 
            // LabelDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 513);
            this.Controls.Add(this.labelControl1);
            this.MinimizeBox = false;
            this.Name = "LabelDlg";
            this.Text = "Системные метки";
            this.UseOffice2007SchemeBackColor = true;
            this.ResumeLayout(false);

        }

        #endregion

        private UI.PresentationDesign.ConfiguratorUI.Controls.LabelControl labelControl1;
    }
}