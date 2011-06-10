namespace UI.PresentationDesign.DesignUI.Controls.Config
{
    partial class FileNameControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.selectPathButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(254)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "filepath";
            // 
            // selectPathButton
            // 
            this.selectPathButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.selectPathButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.selectPathButton.Location = new System.Drawing.Point(300, 0);
            this.selectPathButton.Name = "selectPathButton";
            this.selectPathButton.Size = new System.Drawing.Size(34, 19);
            this.selectPathButton.TabIndex = 1;
            this.selectPathButton.Text = "...";
            this.selectPathButton.UseVisualStyle = true;
            this.selectPathButton.Click += new System.EventHandler(this.selectPathButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FileNameControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Maroon;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.selectPathButton);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "FileNameControl";
            this.Size = new System.Drawing.Size(334, 19);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Syncfusion.Windows.Forms.ButtonAdv selectPathButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;


    }
}
