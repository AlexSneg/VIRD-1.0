using UI.PresentationDesign.DesignUI.Helpers;
using UI.PresentationDesign.DesignUI.Controls.Equipment;
namespace UI.PresentationDesign.DesignUI.Controls
{
    partial class EquipmentControl
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
            Syncfusion.Windows.Forms.Tools.TreeNodeAdvStyleInfo treeNodeAdvStyleInfo1 = new Syncfusion.Windows.Forms.Tools.TreeNodeAdvStyleInfo();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EquipmentControl));
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo1 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo2 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            this.propertyGrid = new UI.PresentationDesign.DesignUI.Helpers.PMediaPropertyGrid();
            this.equipmentList = new EquipmentTreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.toolStripEx1 = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.findStripButton = new System.Windows.Forms.ToolStripButton();
            this.refreshStripButton = new System.Windows.Forms.ToolStripButton();
            this.superToolTip2 = new Syncfusion.Windows.Forms.Tools.SuperToolTip(null);
            ((System.ComponentModel.ISupportInitialize)(this.equipmentList)).BeginInit();
            this.toolStripEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.AssignedObject = null;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.IsReadOnly = true;
            this.propertyGrid.Location = new System.Drawing.Point(0, 137);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(264, 138);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // equipmentList
            // 
            treeNodeAdvStyleInfo1.EnsureDefaultOptionedChild = true;
            treeNodeAdvStyleInfo1.ShowCheckBox = true;
            treeNodeAdvStyleInfo1.ThemesEnabled = false;
            this.equipmentList.BaseStylePairs.AddRange(new Syncfusion.Windows.Forms.Tools.StyleNamePair[] {
            new Syncfusion.Windows.Forms.Tools.StyleNamePair("Standard", treeNodeAdvStyleInfo1)});
            this.equipmentList.Dock = System.Windows.Forms.DockStyle.Top;
            // 
            // 
            // 
            this.equipmentList.HelpTextControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.equipmentList.HelpTextControl.Location = new System.Drawing.Point(0, 0);
            this.equipmentList.HelpTextControl.Name = "helpText";
            this.equipmentList.HelpTextControl.Size = new System.Drawing.Size(49, 15);
            this.equipmentList.HelpTextControl.TabIndex = 0;
            this.equipmentList.HelpTextControl.Text = "help text";
            this.equipmentList.Location = new System.Drawing.Point(0, 23);
            this.equipmentList.MinimumSize = new System.Drawing.Size(50, 25);
            this.equipmentList.Name = "equipmentList";
            this.equipmentList.Office2007ScrollBars = true;
            this.equipmentList.ShowCheckBoxes = true;
            this.equipmentList.Size = new System.Drawing.Size(264, 114);
            this.equipmentList.TabIndex = 1;
            this.equipmentList.Text = "treeViewAdv1";
            this.equipmentList.ThemesEnabled = false;
            // 
            // 
            // 
            this.equipmentList.ToolTipControl.BackColor = System.Drawing.SystemColors.Info;
            this.equipmentList.ToolTipControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.equipmentList.ToolTipControl.Location = new System.Drawing.Point(0, 0);
            this.equipmentList.ToolTipControl.Name = "toolTip";
            this.equipmentList.ToolTipControl.Size = new System.Drawing.Size(41, 15);
            this.equipmentList.ToolTipControl.TabIndex = 1;
            this.equipmentList.ToolTipControl.Text = "toolTip";
            this.equipmentList.BeforeCheck += new Syncfusion.Windows.Forms.Tools.TreeViewAdvBeforeCheckEventHandler(this.equipmentList_BeforeCheck);
            this.equipmentList.AfterCheck += new Syncfusion.Windows.Forms.Tools.TreeNodeAdvEventHandler(this.equipmentList_AfterCheck);
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(230)))), ((int)(((byte)(250)))));
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 137);
            this.splitter1.MinSize = 30;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(264, 2);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // toolStripEx1
            // 
            this.toolStripEx1.CaptionTextStyle = Syncfusion.Windows.Forms.Tools.CaptionTextStyle.Plain;
            this.toolStripEx1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripEx1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripEx1.Image = null;
            this.toolStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findStripButton,
            this.refreshStripButton});
            this.toolStripEx1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripEx1.Location = new System.Drawing.Point(0, 0);
            this.toolStripEx1.Name = "toolStripEx1";
            this.toolStripEx1.ShowCaption = false;
            this.toolStripEx1.ShowLauncher = false;
            this.toolStripEx1.Size = new System.Drawing.Size(264, 23);
            this.toolStripEx1.TabIndex = 3;
            this.toolStripEx1.Text = "Оборудование";
            // 
            // findStripButton
            // 
            this.findStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.findStripButton.Image = ((System.Drawing.Image)(resources.GetObject("findStripButton.Image")));
            this.findStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findStripButton.Name = "findStripButton";
            this.findStripButton.Size = new System.Drawing.Size(23, 20);
            this.findStripButton.Text = "Поиск";
            toolTipInfo1.Body.Text = "Поиск оборудования";
            toolTipInfo1.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo1.Header.Text = "Поиск";
            this.superToolTip2.SetToolTip(this.findStripButton, toolTipInfo1);
            this.findStripButton.Click += new System.EventHandler(this.findStripButton_Click);
            // 
            // refreshStripButton
            // 
            this.refreshStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshStripButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshStripButton.Image")));
            this.refreshStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshStripButton.Name = "refreshStripButton";
            this.refreshStripButton.Size = new System.Drawing.Size(23, 20);
            this.refreshStripButton.Text = "toolStripButton1";
            toolTipInfo2.Body.Text = "Обновить список оборудования";
            toolTipInfo2.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo2.Header.Text = "Обновить";
            this.superToolTip2.SetToolTip(this.refreshStripButton, toolTipInfo2);
            this.refreshStripButton.Visible = false;
            this.refreshStripButton.Click += new System.EventHandler(this.refreshStripButton_Click);
            // 
            // superToolTip2
            // 
            this.superToolTip2.UseFading = Syncfusion.Windows.Forms.Tools.SuperToolTip.FadingType.System;
            // 
            // EquipmentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.equipmentList);
            this.Controls.Add(this.toolStripEx1);
            this.Name = "EquipmentControl";
            this.Size = new System.Drawing.Size(264, 275);
            ((System.ComponentModel.ISupportInitialize)(this.equipmentList)).EndInit();
            this.toolStripEx1.ResumeLayout(false);
            this.toolStripEx1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PMediaPropertyGrid propertyGrid;
        private EquipmentTreeView equipmentList;
        private System.Windows.Forms.Splitter splitter1;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripEx1;
        private System.Windows.Forms.ToolStripButton findStripButton;
        private System.Windows.Forms.ToolStripButton refreshStripButton;
        private Syncfusion.Windows.Forms.Tools.SuperToolTip superToolTip2;

    }
}
