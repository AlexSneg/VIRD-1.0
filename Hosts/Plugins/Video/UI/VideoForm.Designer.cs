namespace Hosts.Plugins.Video.UI
{
    partial class VideoForm
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
            this.wpfVideoHostControl1 = new Hosts.Plugins.Video.UI.WPFVideoHostControl();
            this.SuspendLayout();
            // 
            // wpfVideoHostControl1
            // 
            this.wpfVideoHostControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wpfVideoHostControl1.Location = new System.Drawing.Point(0, 0);
            this.wpfVideoHostControl1.Name = "wpfVideoHostControl1";
            this.wpfVideoHostControl1.Size = new System.Drawing.Size(292, 273);
            this.wpfVideoHostControl1.TabIndex = 0;
            // 
            // VideoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.ControlBox = false;
            this.Controls.Add(this.wpfVideoHostControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VideoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "VideoForm";
            this.ResumeLayout(false);

        }

        #endregion

        private WPFVideoHostControl wpfVideoHostControl1;

    }
}