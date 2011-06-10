namespace UI.PresentationDesign.DesignUI.Controls.DisplayList
{
    partial class DisplayListControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayListControl));
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo1 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo2 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo3 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo4 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo5 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo6 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.TreeNodeAdvStyleInfo treeNodeAdvStyleInfo1 = new Syncfusion.Windows.Forms.Tools.TreeNodeAdvStyleInfo();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStripEx1 = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.createDisplayGroup = new System.Windows.Forms.ToolStripButton();
            this.removeDisplayGroup = new System.Windows.Forms.ToolStripButton();
            this.excludeFromGroupButton = new System.Windows.Forms.ToolStripButton();
            this.propertiesButton = new System.Windows.Forms.ToolStripButton();
            this.findDisplayButton = new System.Windows.Forms.ToolStripButton();
            this.refreshDisplayButton = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStripEx1 = new Syncfusion.Windows.Forms.Tools.ContextMenuStripEx();
            this.propsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.superToolTip1 = new Syncfusion.Windows.Forms.Tools.SuperToolTip(null);
            this.displayTree = new UI.PresentationDesign.DesignUI.Controls.DisplayList.DisplayTreeView();
            this.toolStripEx1.SuspendLayout();
            this.contextMenuStripEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.displayTree)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "display");
            this.imageList1.Images.SetKeyName(1, "group");
            // 
            // toolStripEx1
            // 
            this.toolStripEx1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripEx1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripEx1.Image = null;
            this.toolStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createDisplayGroup,
            this.removeDisplayGroup,
            this.excludeFromGroupButton,
            this.propertiesButton,
            this.findDisplayButton,
            this.refreshDisplayButton});
            this.toolStripEx1.Location = new System.Drawing.Point(0, 0);
            this.toolStripEx1.Name = "toolStripEx1";
            this.toolStripEx1.ShowCaption = false;
            this.toolStripEx1.ShowLauncher = false;
            this.toolStripEx1.Size = new System.Drawing.Size(246, 25);
            this.toolStripEx1.TabIndex = 1;
            this.toolStripEx1.Text = "toolStripEx1";
            // 
            // createDisplayGroup
            // 
            this.createDisplayGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.createDisplayGroup.Image = ((System.Drawing.Image)(resources.GetObject("createDisplayGroup.Image")));
            this.createDisplayGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createDisplayGroup.Name = "createDisplayGroup";
            this.createDisplayGroup.Size = new System.Drawing.Size(23, 22);
            this.createDisplayGroup.Text = "Создать группу";
            toolTipInfo1.Body.Text = "Создать группу дисплеев";
            toolTipInfo1.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo1.Header.Text = "Создать группу";
            this.superToolTip1.SetToolTip(this.createDisplayGroup, toolTipInfo1);
            this.createDisplayGroup.Click += new System.EventHandler(this.createDisplayGroup_Click);
            // 
            // removeDisplayGroup
            // 
            this.removeDisplayGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeDisplayGroup.Image = ((System.Drawing.Image)(resources.GetObject("removeDisplayGroup.Image")));
            this.removeDisplayGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeDisplayGroup.Name = "removeDisplayGroup";
            this.removeDisplayGroup.Size = new System.Drawing.Size(23, 22);
            this.removeDisplayGroup.Text = "Удалить группу";
            toolTipInfo2.Body.Text = "Разгруппировать дисплеи \r\nи удалить группу\r\n";
            toolTipInfo2.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo2.Header.Text = "Удалить группу";
            this.superToolTip1.SetToolTip(this.removeDisplayGroup, toolTipInfo2);
            this.removeDisplayGroup.Click += new System.EventHandler(this.removeDisplayGroup_Click);
            // 
            // excludeFromGroupButton
            // 
            this.excludeFromGroupButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.excludeFromGroupButton.Image = ((System.Drawing.Image)(resources.GetObject("excludeFromGroupButton.Image")));
            this.excludeFromGroupButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.excludeFromGroupButton.Name = "excludeFromGroupButton";
            this.excludeFromGroupButton.Size = new System.Drawing.Size(23, 22);
            this.excludeFromGroupButton.Text = "Исключить из группы";
            toolTipInfo3.Body.Text = "Исключить дисплей из группы";
            toolTipInfo3.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo3.Header.Text = "Исключить из группы";
            this.superToolTip1.SetToolTip(this.excludeFromGroupButton, toolTipInfo3);
            this.excludeFromGroupButton.Click += new System.EventHandler(this.excludeFromGroupButton_Click);
            // 
            // propertiesButton
            // 
            this.propertiesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.propertiesButton.Image = ((System.Drawing.Image)(resources.GetObject("propertiesButton.Image")));
            this.propertiesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.propertiesButton.Name = "propertiesButton";
            this.propertiesButton.Size = new System.Drawing.Size(23, 22);
            this.propertiesButton.Text = "toolStripButton1";
            toolTipInfo4.Body.Text = "Свойства дисплея или \r\nгруппы дисплеев";
            toolTipInfo4.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo4.Header.Text = "Свойства";
            this.superToolTip1.SetToolTip(this.propertiesButton, toolTipInfo4);
            this.propertiesButton.Click += new System.EventHandler(this.propertiesButton_Click);
            // 
            // findDisplayButton
            // 
            this.findDisplayButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.findDisplayButton.Image = ((System.Drawing.Image)(resources.GetObject("findDisplayButton.Image")));
            this.findDisplayButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findDisplayButton.Name = "findDisplayButton";
            this.findDisplayButton.Size = new System.Drawing.Size(23, 22);
            this.findDisplayButton.Text = "toolStripButton1";
            toolTipInfo5.Body.Text = "Поиск дисплеев";
            toolTipInfo5.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo5.Header.Text = "Поиск";
            this.superToolTip1.SetToolTip(this.findDisplayButton, toolTipInfo5);
            this.findDisplayButton.Click += new System.EventHandler(this.findDisplayButton_Click);
            // 
            // refreshDisplayButton
            // 
            this.refreshDisplayButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshDisplayButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshDisplayButton.Image")));
            this.refreshDisplayButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshDisplayButton.Name = "refreshDisplayButton";
            this.refreshDisplayButton.Size = new System.Drawing.Size(23, 22);
            this.refreshDisplayButton.Text = "toolStripButton2";
            toolTipInfo6.Body.Text = "Обновить список дисплеев";
            toolTipInfo6.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo6.Header.Text = "Обновить";
            this.superToolTip1.SetToolTip(this.refreshDisplayButton, toolTipInfo6);
            this.refreshDisplayButton.Visible = false;
            this.refreshDisplayButton.Click += new System.EventHandler(this.refreshDisplayButton_Click);
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
            // superToolTip1
            // 
            this.superToolTip1.UseFading = Syncfusion.Windows.Forms.Tools.SuperToolTip.FadingType.System;
            // 
            // displayTree
            // 
            this.displayTree.AllowDrop = true;
            this.displayTree.AllowMouseBasedSelection = true;
            treeNodeAdvStyleInfo1.EnsureDefaultOptionedChild = true;
            treeNodeAdvStyleInfo1.InteractiveCheckBox = true;
            this.displayTree.BaseStylePairs.AddRange(new Syncfusion.Windows.Forms.Tools.StyleNamePair[] {
            new Syncfusion.Windows.Forms.Tools.StyleNamePair("Standard", treeNodeAdvStyleInfo1)});
            this.displayTree.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.displayTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.displayTree.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            this.displayTree.HelpTextControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.displayTree.HelpTextControl.Location = new System.Drawing.Point(0, 0);
            this.displayTree.HelpTextControl.Name = "helpText";
            this.displayTree.HelpTextControl.Size = new System.Drawing.Size(49, 15);
            this.displayTree.HelpTextControl.TabIndex = 0;
            this.displayTree.HelpTextControl.Text = "help text";
            this.displayTree.HideSelection = false;
            this.displayTree.InteractiveCheckBoxes = true;
            this.displayTree.KeepDottedSelection = false;
            this.displayTree.LabelEdit = true;
            this.displayTree.LineStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.displayTree.Location = new System.Drawing.Point(0, 25);
            this.displayTree.Name = "displayTree";
            this.displayTree.Office2007ScrollBars = true;
            this.displayTree.SelectedNodeBackground = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.Gray);
            this.displayTree.Size = new System.Drawing.Size(246, 225);
            this.displayTree.SortWithChildNodes = true;
            this.displayTree.StateImageList = this.imageList1;
            this.displayTree.TabIndex = 0;
            this.displayTree.Text = "treeViewAdv1";
            // 
            // 
            // 
            this.displayTree.ToolTipControl.BackColor = System.Drawing.SystemColors.Info;
            this.displayTree.ToolTipControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.displayTree.ToolTipControl.Location = new System.Drawing.Point(0, 0);
            this.displayTree.ToolTipControl.Name = "toolTip";
            this.displayTree.ToolTipControl.Size = new System.Drawing.Size(41, 15);
            this.displayTree.ToolTipControl.TabIndex = 1;
            this.displayTree.ToolTipControl.Text = "toolTip";
            this.displayTree.AfterSelect += new System.EventHandler(this.displayTree_AfterSelect_1);
            this.displayTree.EditCancelled += new Syncfusion.Windows.Forms.Tools.TreeNodeAdvEditEventHandler(this.displayTree_EditCancelled);
            this.displayTree.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DisplayListControl_MouseClick);
            this.displayTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.displayTree_MouseDown);
            this.displayTree.BeforeEdit += new Syncfusion.Windows.Forms.Tools.TreeViewAdvBeforeEditEventHandler(this.displayTree_BeforeEdit);
            this.displayTree.NodeEditorValidated += new Syncfusion.Windows.Forms.Tools.TreeNodeAdvEditEventHandler(this.displayTree_NodeEditorValidated);
            this.displayTree.AfterCheck += new Syncfusion.Windows.Forms.Tools.TreeNodeAdvEventHandler(this.displayTree_AfterCheck);
            // 
            // DisplayListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.displayTree);
            this.Controls.Add(this.toolStripEx1);
            this.Name = "DisplayListControl";
            this.Size = new System.Drawing.Size(246, 250);
            this.toolStripEx1.ResumeLayout(false);
            this.toolStripEx1.PerformLayout();
            this.contextMenuStripEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.displayTree)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DisplayTreeView displayTree;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripEx1;
        private System.Windows.Forms.ToolStripButton createDisplayGroup;
        private System.Windows.Forms.ToolStripButton removeDisplayGroup;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripButton propertiesButton;
        private Syncfusion.Windows.Forms.Tools.ContextMenuStripEx contextMenuStripEx1;
        private System.Windows.Forms.ToolStripMenuItem propsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton excludeFromGroupButton;
        private Syncfusion.Windows.Forms.Tools.SuperToolTip superToolTip1;
        private System.Windows.Forms.ToolStripButton findDisplayButton;
        private System.Windows.Forms.ToolStripButton refreshDisplayButton;
    }
}
