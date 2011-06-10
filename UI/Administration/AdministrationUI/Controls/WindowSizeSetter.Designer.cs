namespace UI.Administration.AdministrationUI.Controls
{
    partial class WindowSizeSetter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowSizeSetter));
            this.btnWindowSize = new Syncfusion.Windows.Forms.ButtonAdv();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnWindowSize
            // 
            this.btnWindowSize.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.btnWindowSize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnWindowSize.Image = ((System.Drawing.Image)(resources.GetObject("btnWindowSize.Image")));
            this.btnWindowSize.Location = new System.Drawing.Point(310, 0);
            this.btnWindowSize.Name = "btnWindowSize";
            this.btnWindowSize.Size = new System.Drawing.Size(34, 20);
            this.btnWindowSize.TabIndex = 0;
            this.btnWindowSize.UseVisualStyle = true;
            this.btnWindowSize.Click += new System.EventHandler(this.btnWindowSize_Click);
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(254)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(310, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // WindowSizeSetter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnWindowSize);
            this.Name = "WindowSizeSetter";
            this.Size = new System.Drawing.Size(344, 20);
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv btnWindowSize;
        private System.Windows.Forms.Label label1;
    }
}
