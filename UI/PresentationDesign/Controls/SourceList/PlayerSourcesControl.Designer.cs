namespace UI.PresentationDesign.DesignUI.Controls.SourceList
{
    partial class PlayerSourcesControl
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
            this.contextMenuStripEx1 = new Syncfusion.Windows.Forms.Tools.ContextMenuStripEx();
            this.propsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBar1 = new Syncfusion.Windows.Forms.Tools.GroupBar();
            this.contextMenuStripEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStripEx1
            // 
            this.contextMenuStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propsToolStripMenuItem});
            this.contextMenuStripEx1.Name = "contextMenuStripEx1";
            this.contextMenuStripEx1.Size = new System.Drawing.Size(134, 26);
            // 
            // propsToolStripMenuItem
            // 
            this.propsToolStripMenuItem.Name = "propsToolStripMenuItem";
            this.propsToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.propsToolStripMenuItem.Text = "Свойства";
            this.propsToolStripMenuItem.Click += new System.EventHandler(this.propsToolStripMenuItem_Click);
            // 
            // groupBar1
            // 
            this.groupBar1.AllowDrop = true;
            this.groupBar1.BackColor = System.Drawing.Color.White;
            this.groupBar1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.groupBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBar1.FlatLook = true;
            this.groupBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(77)))), ((int)(((byte)(140)))));
            this.groupBar1.HeaderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(65)))), ((int)(((byte)(140)))));
            this.groupBar1.Location = new System.Drawing.Point(0, 0);
            this.groupBar1.Name = "groupBar1";
            this.groupBar1.PopupClientSize = new System.Drawing.Size(0, 0);
            this.groupBar1.Size = new System.Drawing.Size(317, 317);
            this.groupBar1.TabIndex = 0;
            this.groupBar1.Text = "groupBar1";
            this.groupBar1.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            // 
            // PlayerSourcesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBar1);
            this.Name = "PlayerSourcesControl";
            this.Size = new System.Drawing.Size(317, 317);
            this.contextMenuStripEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GroupBar groupBar1;
        private Syncfusion.Windows.Forms.Tools.ContextMenuStripEx contextMenuStripEx1;
        private System.Windows.Forms.ToolStripMenuItem propsToolStripMenuItem;

    }
}
