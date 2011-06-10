namespace Hosts.Plugins.ArcGISMap.Player
{
    partial class MapManageControl
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
            this.downButton = new System.Windows.Forms.Button();
            this.rigthButton = new System.Windows.Forms.Button();
            this.leftButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.scaleCombo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // downButton
            // 
            this.downButton.Location = new System.Drawing.Point(42, 80);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(36, 36);
            this.downButton.TabIndex = 8;
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.MapMoveClick);
            // 
            // rigthButton
            // 
            this.rigthButton.Location = new System.Drawing.Point(78, 44);
            this.rigthButton.Name = "rigthButton";
            this.rigthButton.Size = new System.Drawing.Size(36, 36);
            this.rigthButton.TabIndex = 7;
            this.rigthButton.UseVisualStyleBackColor = true;
            this.rigthButton.Click += new System.EventHandler(this.MapMoveClick);
            // 
            // leftButton
            // 
            this.leftButton.Location = new System.Drawing.Point(6, 44);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(36, 36);
            this.leftButton.TabIndex = 6;
            this.leftButton.UseVisualStyleBackColor = true;
            this.leftButton.Click += new System.EventHandler(this.MapMoveClick);
            // 
            // upButton
            // 
            this.upButton.Location = new System.Drawing.Point(42, 8);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(36, 36);
            this.upButton.TabIndex = 5;
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.MapMoveClick);
            // 
            // scaleCombo
            // 
            this.scaleCombo.FormattingEnabled = true;
            this.scaleCombo.Items.AddRange(new object[] {
            "10%",
            "25%",
            "50%",
            "75%",
            "100%",
            "200%",
            "300%"});
            this.scaleCombo.Location = new System.Drawing.Point(6, 144);
            this.scaleCombo.Name = "scaleCombo";
            this.scaleCombo.Size = new System.Drawing.Size(118, 21);
            this.scaleCombo.TabIndex = 9;
            this.scaleCombo.Text = "100%";
            this.scaleCombo.TextChanged += new System.EventHandler(this.scaleCombo_TextChanged);
            // 
            // MapManageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scaleCombo);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.rigthButton);
            this.Controls.Add(this.leftButton);
            this.Controls.Add(this.upButton);
            this.Name = "MapManageControl";
            this.Size = new System.Drawing.Size(127, 179);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button rigthButton;
        private System.Windows.Forms.Button leftButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.ComboBox scaleCombo;
    }
}
