namespace UI.PresentationDesign.DesignUI.Controls.Config
{
    partial class FilePathTextBox
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
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(304, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "filepath";
            // 
            // selectPathButton
            // 
            this.selectPathButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.selectPathButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.selectPathButton.Location = new System.Drawing.Point(304, 0);
            this.selectPathButton.Name = "selectPathButton";
            this.selectPathButton.Size = new System.Drawing.Size(30, 19);
            this.selectPathButton.TabIndex = 1;
            this.selectPathButton.Text = "...";
            this.selectPathButton.UseVisualStyle = true;
            this.selectPathButton.Click += new System.EventHandler(this.selectPathButton_Click);
            // 
            // FilePathTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.selectPathButton);
            this.Name = "FilePathTextBox";
            this.Size = new System.Drawing.Size(334, 19);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Syncfusion.Windows.Forms.ButtonAdv selectPathButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;


    }
}
