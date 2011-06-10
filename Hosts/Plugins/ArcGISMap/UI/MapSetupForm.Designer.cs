namespace Hosts.Plugins.ArcGISMap.UI
{
    partial class MapSetupForm
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.map = new Hosts.Plugins.ArcGISMap.UI.Controls.MapControl();
            this.downButton = new System.Windows.Forms.Button();
            this.rigthButton = new System.Windows.Forms.Button();
            this.leftButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.scaleCombo = new System.Windows.Forms.ComboBox();
            this.treeView = new System.Windows.Forms.TreeView();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.map);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.treeView);
            this.splitContainer.Panel2.Controls.Add(this.downButton);
            this.splitContainer.Panel2.Controls.Add(this.rigthButton);
            this.splitContainer.Panel2.Controls.Add(this.leftButton);
            this.splitContainer.Panel2.Controls.Add(this.upButton);
            this.splitContainer.Panel2.Controls.Add(this.scaleCombo);
            this.splitContainer.Size = new System.Drawing.Size(576, 431);
            this.splitContainer.SplitterDistance = 350;
            this.splitContainer.TabIndex = 0;
            // 
            // map
            // 
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Name = "map";
            this.map.Scale = 1;
            this.map.Size = new System.Drawing.Size(350, 431);
            this.map.TabIndex = 0;
            this.map.XOffset = 0;
            this.map.YOffset = 0;
            // 
            // downButton
            // 
            this.downButton.Location = new System.Drawing.Point(93, 132);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(36, 36);
            this.downButton.TabIndex = 4;
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.MapMoveClick);
            // 
            // rigthButton
            // 
            this.rigthButton.Location = new System.Drawing.Point(129, 96);
            this.rigthButton.Name = "rigthButton";
            this.rigthButton.Size = new System.Drawing.Size(36, 36);
            this.rigthButton.TabIndex = 3;
            this.rigthButton.UseVisualStyleBackColor = true;
            this.rigthButton.Click += new System.EventHandler(this.MapMoveClick);
            // 
            // leftButton
            // 
            this.leftButton.Location = new System.Drawing.Point(57, 96);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(36, 36);
            this.leftButton.TabIndex = 2;
            this.leftButton.UseVisualStyleBackColor = true;
            this.leftButton.Click += new System.EventHandler(this.MapMoveClick);
            // 
            // upButton
            // 
            this.upButton.Location = new System.Drawing.Point(93, 60);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(36, 36);
            this.upButton.TabIndex = 1;
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
            this.scaleCombo.Location = new System.Drawing.Point(56, 30);
            this.scaleCombo.Name = "scaleCombo";
            this.scaleCombo.Size = new System.Drawing.Size(121, 21);
            this.scaleCombo.TabIndex = 0;
            this.scaleCombo.Text = "100%";
            this.scaleCombo.TextChanged += new System.EventHandler(this.scaleCombo_TextChanged);
            // 
            // treeView
            // 
            this.treeView.CheckBoxes = true;
            this.treeView.Location = new System.Drawing.Point(56, 174);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(121, 97);
            this.treeView.TabIndex = 5;
            this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCheck);
            // 
            // MapSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 431);
            this.Controls.Add(this.splitContainer);
            this.Name = "MapSetupForm";
            this.Text = "Настройка карты";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private Hosts.Plugins.ArcGISMap.UI.Controls.MapControl map;
        private System.Windows.Forms.ComboBox scaleCombo;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button rigthButton;
        private System.Windows.Forms.Button leftButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.TreeView treeView;
    }
}