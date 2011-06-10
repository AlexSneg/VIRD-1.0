namespace UI.PresentationDesign.DesignUI
{
    partial class PlayerForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerForm));
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbslideDiagramControl = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbdisplayListControl = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbplayerSourcesControl1 = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbSourceManagementControl = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbequipmentControl = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbequipmentCommandControl = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            this.dockingManager = new Syncfusion.Windows.Forms.Tools.DockingManager(this.components);
            this.slideDiagramControl = new UI.PresentationDesign.DesignUI.SlideDiagramControl();
            this.displayListControl = new UI.PresentationDesign.DesignUI.Controls.DisplayList.DisplayListControl();
            this.playerSourcesControl1 = new UI.PresentationDesign.DesignUI.Controls.SourceList.PlayerSourcesControl();
            this.SourceManagementControl = new UI.PresentationDesign.DesignUI.Controls.ManagementControl.ManagementControl();
            this.equipmentControl = new UI.PresentationDesign.DesignUI.Controls.Equipment.PlayerEquipmentControl();
            this.equipmentCommandControl = new UI.PresentationDesign.DesignUI.Controls.ManagementControl.ManagementControl();
            this.ribbonControlAdv = new Syncfusion.Windows.Forms.Tools.RibbonControlAdv();
            this.viewMenuButton = new Syncfusion.Windows.Forms.Tools.OfficeDropDownButton();
            this.displaysMenuButton = new Syncfusion.Windows.Forms.Tools.ToolStripCheckBox();
            this.sourcesMenuButton = new Syncfusion.Windows.Forms.Tools.ToolStripCheckBox();
            this.sourcesControlMenuButton = new Syncfusion.Windows.Forms.Tools.ToolStripCheckBox();
            this.equipmentMenuButton = new Syncfusion.Windows.Forms.Tools.ToolStripCheckBox();
            this.equipmentControlMenuButton = new Syncfusion.Windows.Forms.Tools.ToolStripCheckBox();
            this.windowsMenuButton = new Syncfusion.Windows.Forms.Tools.OfficeDropDownButton();
            this.cascadeMenuButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.arrange2x2MenuButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.arrange3x3MenuButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.arrange4x4MenuButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.defaultSizeMenuButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.closeAllMenuButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.closeMenuButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.dockingClientPanel1 = new Syncfusion.Windows.Forms.Tools.DockingClientPanel();
            this.displayMonitorControl1 = new UI.PresentationDesign.DesignUI.Controls.DisplayMonitor.DisplayMonitorControl();
            this.statusStripEx1 = new Syncfusion.Windows.Forms.Tools.StatusStripEx();
            this.SlideLockingStatus = new Syncfusion.Windows.Forms.Tools.StatusStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dockingManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControlAdv)).BeginInit();
            this.dockingClientPanel1.SuspendLayout();
            this.statusStripEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockingManager
            // 
            this.dockingManager.ActiveCaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockingManager.AutoHideActiveControl = true;
            this.dockingManager.AutoHideTabFont = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockingManager.CloseEnabled = false;
            this.dockingManager.DockLayoutStream = ((System.IO.MemoryStream)(resources.GetObject("dockingManager.DockLayoutStream")));
            this.dockingManager.DockTabFont = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockingManager.DragProviderStyle = Syncfusion.Windows.Forms.Tools.DragProviderStyle.VS2008;
            this.dockingManager.HostControl = this;
            this.dockingManager.InActiveCaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockingManager.MenuButtonEnabled = false;
            this.dockingManager.Office2007MdiChildForm = true;
            this.dockingManager.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Close, "CloseButton"));
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Pin, "PinButton"));
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Menu, "MenuButton"));
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Maximize, "MaximizeButton"));
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Restore, "RestoreButton"));
            this.dockingManager.SetDockLabel(this.slideDiagramControl, "Граф сцен");
            ccbslideDiagramControl.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.slideDiagramControl, ccbslideDiagramControl);
            this.dockingManager.SetDockLabel(this.displayListControl, "Дисплеи");
            ccbdisplayListControl.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.displayListControl, ccbdisplayListControl);
            this.dockingManager.SetDockLabel(this.playerSourcesControl1, "Источники");
            ccbplayerSourcesControl1.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.playerSourcesControl1, ccbplayerSourcesControl1);
            this.dockingManager.SetDockLabel(this.SourceManagementControl, "Управление источниками");
            ccbSourceManagementControl.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.SourceManagementControl, ccbSourceManagementControl);
            this.dockingManager.SetDockLabel(this.equipmentControl, "Оборудование");
            ccbequipmentControl.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.equipmentControl, ccbequipmentControl);
            this.dockingManager.SetDockLabel(this.equipmentCommandControl, "Управление оборудованием");
            ccbequipmentCommandControl.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.equipmentCommandControl, ccbequipmentCommandControl);
            // 
            // slideDiagramControl
            // 
            this.slideDiagramControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dockingManager.SetEnableDocking(this.slideDiagramControl, true);
            this.slideDiagramControl.Location = new System.Drawing.Point(3, 23);
            this.slideDiagramControl.Name = "slideDiagramControl";
            this.slideDiagramControl.Size = new System.Drawing.Size(816, 181);
            this.slideDiagramControl.TabIndex = 2;
            // 
            // displayListControl
            // 
            this.dockingManager.SetEnableDocking(this.displayListControl, true);
            this.displayListControl.Location = new System.Drawing.Point(3, 23);
            this.displayListControl.Name = "displayListControl";
            this.displayListControl.Size = new System.Drawing.Size(197, 67);
            this.displayListControl.TabIndex = 4;
            // 
            // playerSourcesControl1
            // 
            this.dockingManager.SetEnableDocking(this.playerSourcesControl1, true);
            this.playerSourcesControl1.Location = new System.Drawing.Point(3, 23);
            this.playerSourcesControl1.Name = "playerSourcesControl1";
            this.playerSourcesControl1.Size = new System.Drawing.Size(187, 117);
            this.playerSourcesControl1.TabIndex = 32;
            // 
            // SourceManagementControl
            // 
            this.SourceManagementControl.AutoScroll = true;
            this.dockingManager.SetEnableDocking(this.SourceManagementControl, true);
            this.SourceManagementControl.Location = new System.Drawing.Point(3, 23);
            this.SourceManagementControl.Name = "SourceManagementControl";
            this.SourceManagementControl.Size = new System.Drawing.Size(187, 128);
            this.SourceManagementControl.TabIndex = 32;
            // 
            // equipmentControl
            // 
            this.dockingManager.SetEnableDocking(this.equipmentControl, true);
            this.equipmentControl.Location = new System.Drawing.Point(3, 23);
            this.equipmentControl.Name = "equipmentControl";
            this.equipmentControl.Size = new System.Drawing.Size(197, 64);
            this.equipmentControl.TabIndex = 29;
            // 
            // equipmentCommandControl
            // 
            this.equipmentCommandControl.AutoScroll = true;
            this.dockingManager.SetEnableDocking(this.equipmentCommandControl, true);
            this.equipmentCommandControl.Location = new System.Drawing.Point(3, 23);
            this.equipmentCommandControl.Name = "equipmentCommandControl";
            this.equipmentCommandControl.Size = new System.Drawing.Size(197, 84);
            this.equipmentCommandControl.TabIndex = 32;
            // 
            // ribbonControlAdv
            // 
            this.ribbonControlAdv.AllowCollapse = false;
            this.ribbonControlAdv.Location = new System.Drawing.Point(1, 0);
            this.ribbonControlAdv.MenuButtonImage = ((System.Drawing.Image)(resources.GetObject("ribbonControlAdv.MenuButtonImage")));
            this.ribbonControlAdv.Name = "ribbonControlAdv";
            // 
            // ribbonControlAdv.OfficeMenu
            // 
            this.ribbonControlAdv.OfficeMenu.MainPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewMenuButton,
            this.windowsMenuButton});
            this.ribbonControlAdv.OfficeMenu.Name = "OfficeMenu";
            this.ribbonControlAdv.OfficeMenu.Size = new System.Drawing.Size(99, 130);
            this.ribbonControlAdv.OfficeMenu.SystemPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeMenuButton});
            this.ribbonControlAdv.QuickPanelVisible = false;
            this.ribbonControlAdv.Size = new System.Drawing.Size(832, 44);
            this.ribbonControlAdv.SystemText.QuickAccessDialogDropDownName = "Start menu";
            this.ribbonControlAdv.TabIndex = 0;
            this.ribbonControlAdv.Text = "Проигрыватель";
            // 
            // viewMenuButton
            // 
            this.viewMenuButton.DropDownFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.viewMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displaysMenuButton,
            this.sourcesMenuButton,
            this.sourcesControlMenuButton,
            this.equipmentMenuButton,
            this.equipmentControlMenuButton});
            this.viewMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("viewMenuButton.Image")));
            this.viewMenuButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.viewMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.viewMenuButton.Name = "viewMenuButton";
            this.viewMenuButton.Size = new System.Drawing.Size(87, 36);
            this.viewMenuButton.Text = "Вид";
            // 
            // displaysMenuButton
            // 
            this.displaysMenuButton.Checked = true;
            this.displaysMenuButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.displaysMenuButton.Name = "displaysMenuButton";
            this.displaysMenuButton.Size = new System.Drawing.Size(170, 17);
            this.displaysMenuButton.Text = "Дисплеи";
            this.displaysMenuButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.displaysMenuButton.CheckedChanged += new System.EventHandler(this.displaysMenuButton_CheckedChanged);
            // 
            // sourcesMenuButton
            // 
            this.sourcesMenuButton.Checked = true;
            this.sourcesMenuButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sourcesMenuButton.Name = "sourcesMenuButton";
            this.sourcesMenuButton.Size = new System.Drawing.Size(170, 17);
            this.sourcesMenuButton.Text = "Источники";
            this.sourcesMenuButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.sourcesMenuButton.CheckedChanged += new System.EventHandler(this.displaysMenuButton_CheckedChanged);
            // 
            // sourcesControlMenuButton
            // 
            this.sourcesControlMenuButton.Checked = true;
            this.sourcesControlMenuButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sourcesControlMenuButton.Name = "sourcesControlMenuButton";
            this.sourcesControlMenuButton.Size = new System.Drawing.Size(170, 17);
            this.sourcesControlMenuButton.Text = "Управление источниками";
            this.sourcesControlMenuButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.sourcesControlMenuButton.CheckedChanged += new System.EventHandler(this.displaysMenuButton_CheckedChanged);
            // 
            // equipmentMenuButton
            // 
            this.equipmentMenuButton.Checked = true;
            this.equipmentMenuButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.equipmentMenuButton.Name = "equipmentMenuButton";
            this.equipmentMenuButton.Size = new System.Drawing.Size(170, 17);
            this.equipmentMenuButton.Text = "Оборудование";
            this.equipmentMenuButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.equipmentMenuButton.CheckedChanged += new System.EventHandler(this.displaysMenuButton_CheckedChanged);
            // 
            // equipmentControlMenuButton
            // 
            this.equipmentControlMenuButton.Checked = true;
            this.equipmentControlMenuButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.equipmentControlMenuButton.Name = "equipmentControlMenuButton";
            this.equipmentControlMenuButton.Size = new System.Drawing.Size(170, 17);
            this.equipmentControlMenuButton.Text = "Управление оборудованием";
            this.equipmentControlMenuButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.equipmentControlMenuButton.CheckedChanged += new System.EventHandler(this.displaysMenuButton_CheckedChanged);
            // 
            // windowsMenuButton
            // 
            this.windowsMenuButton.DropDownFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.windowsMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cascadeMenuButton,
            this.arrange2x2MenuButton,
            this.arrange3x3MenuButton,
            this.arrange4x4MenuButton,
            this.defaultSizeMenuButton,
            this.toolStripLabel1,
            this.closeAllMenuButton});
            this.windowsMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("windowsMenuButton.Image")));
            this.windowsMenuButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.windowsMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.windowsMenuButton.Name = "windowsMenuButton";
            this.windowsMenuButton.Size = new System.Drawing.Size(87, 36);
            this.windowsMenuButton.Text = "Окна";
            this.windowsMenuButton.Click += new System.EventHandler(this.windowsMenuButton_Click);
            // 
            // cascadeMenuButton
            // 
            this.cascadeMenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.cascadeMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("cascadeMenuButton.Image")));
            this.cascadeMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cascadeMenuButton.Name = "cascadeMenuButton";
            this.cascadeMenuButton.Size = new System.Drawing.Size(181, 23);
            this.cascadeMenuButton.Text = "Каскад";
            this.cascadeMenuButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cascadeMenuButton.Click += new System.EventHandler(this.cascadeMenuButton_Click);
            // 
            // arrange2x2MenuButton
            // 
            this.arrange2x2MenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.arrange2x2MenuButton.Image = ((System.Drawing.Image)(resources.GetObject("arrange2x2MenuButton.Image")));
            this.arrange2x2MenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.arrange2x2MenuButton.Name = "arrange2x2MenuButton";
            this.arrange2x2MenuButton.Size = new System.Drawing.Size(181, 23);
            this.arrange2x2MenuButton.Text = "2x2";
            this.arrange2x2MenuButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.arrange2x2MenuButton.Click += new System.EventHandler(this.arrange2x2MenuButton_Click);
            // 
            // arrange3x3MenuButton
            // 
            this.arrange3x3MenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.arrange3x3MenuButton.Image = ((System.Drawing.Image)(resources.GetObject("arrange3x3MenuButton.Image")));
            this.arrange3x3MenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.arrange3x3MenuButton.Name = "arrange3x3MenuButton";
            this.arrange3x3MenuButton.Size = new System.Drawing.Size(181, 23);
            this.arrange3x3MenuButton.Text = "3x3";
            this.arrange3x3MenuButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.arrange3x3MenuButton.Click += new System.EventHandler(this.arrange3x3MenuButton_Click);
            // 
            // arrange4x4MenuButton
            // 
            this.arrange4x4MenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.arrange4x4MenuButton.Image = ((System.Drawing.Image)(resources.GetObject("arrange4x4MenuButton.Image")));
            this.arrange4x4MenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.arrange4x4MenuButton.Name = "arrange4x4MenuButton";
            this.arrange4x4MenuButton.Size = new System.Drawing.Size(181, 23);
            this.arrange4x4MenuButton.Text = "4x4";
            this.arrange4x4MenuButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.arrange4x4MenuButton.Click += new System.EventHandler(this.arrange4x4MenuButton_Click);
            // 
            // defaultSizeMenuButton
            // 
            this.defaultSizeMenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.defaultSizeMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("defaultSizeMenuButton.Image")));
            this.defaultSizeMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.defaultSizeMenuButton.Name = "defaultSizeMenuButton";
            this.defaultSizeMenuButton.Size = new System.Drawing.Size(181, 23);
            this.defaultSizeMenuButton.Text = "Размер по умолчанию";
            this.defaultSizeMenuButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.defaultSizeMenuButton.Click += new System.EventHandler(this.defaultSizeMenuButton_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Enabled = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(181, 13);
            this.toolStripLabel1.Text = "_____________________________";
            this.toolStripLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // closeAllMenuButton
            // 
            this.closeAllMenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.closeAllMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("closeAllMenuButton.Image")));
            this.closeAllMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.closeAllMenuButton.Name = "closeAllMenuButton";
            this.closeAllMenuButton.Size = new System.Drawing.Size(181, 23);
            this.closeAllMenuButton.Text = "Закрыть все";
            this.closeAllMenuButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.closeAllMenuButton.Click += new System.EventHandler(this.closeAllMenuButton_Click);
            // 
            // closeMenuButton
            // 
            this.closeMenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.closeMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("closeMenuButton.Image")));
            this.closeMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.closeMenuButton.Name = "closeMenuButton";
            this.closeMenuButton.Size = new System.Drawing.Size(63, 23);
            this.closeMenuButton.Text = "Закрыть";
            this.closeMenuButton.Click += new System.EventHandler(this.closeMenuButton_Click);
            // 
            // dockingClientPanel1
            // 
            this.dockingClientPanel1.Controls.Add(this.displayMonitorControl1);
            this.dockingClientPanel1.Location = new System.Drawing.Point(203, 45);
            this.dockingClientPanel1.Name = "dockingClientPanel1";
            this.dockingClientPanel1.Size = new System.Drawing.Size(418, 301);
            this.dockingClientPanel1.SizeToFit = true;
            this.dockingClientPanel1.TabIndex = 13;
            // 
            // displayMonitorControl1
            // 
            this.displayMonitorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayMonitorControl1.Location = new System.Drawing.Point(0, 0);
            this.displayMonitorControl1.Name = "displayMonitorControl1";
            this.displayMonitorControl1.Size = new System.Drawing.Size(418, 301);
            this.displayMonitorControl1.TabIndex = 0;
            // 
            // statusStripEx1
            // 
            this.statusStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SlideLockingStatus});
            this.statusStripEx1.Location = new System.Drawing.Point(1, 561);
            this.statusStripEx1.Name = "statusStripEx1";
            this.statusStripEx1.Size = new System.Drawing.Size(832, 22);
            this.statusStripEx1.TabIndex = 51;
            this.statusStripEx1.Text = "statusStripEx1";
            // 
            // SlideLockingStatus
            // 
            this.SlideLockingStatus.Margin = new System.Windows.Forms.Padding(0, 4, 0, 2);
            this.SlideLockingStatus.Name = "SlideLockingStatus";
            this.SlideLockingStatus.Size = new System.Drawing.Size(0, 0);
            // 
            // PlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 585);
            this.Controls.Add(this.statusStripEx1);
            this.Controls.Add(this.dockingClientPanel1);
            this.Controls.Add(this.ribbonControlAdv);
            this.Name = "PlayerForm";
            this.Text = "Проигрыватель";
            this.Load += new System.EventHandler(this.PlayerForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PlayerForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dockingManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControlAdv)).EndInit();
            this.dockingClientPanel1.ResumeLayout(false);
            this.statusStripEx1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.DockingManager dockingManager;
        private Syncfusion.Windows.Forms.Tools.RibbonControlAdv ribbonControlAdv;
        private UI.PresentationDesign.DesignUI.SlideDiagramControl slideDiagramControl;
        private UI.PresentationDesign.DesignUI.Controls.DisplayList.DisplayListControl displayListControl;
        private Syncfusion.Windows.Forms.Tools.DockingClientPanel dockingClientPanel1;
        private Syncfusion.Windows.Forms.Tools.OfficeDropDownButton viewMenuButton;
        private Syncfusion.Windows.Forms.Tools.OfficeDropDownButton windowsMenuButton;
        private Syncfusion.Windows.Forms.Tools.OfficeButton cascadeMenuButton;
        private Syncfusion.Windows.Forms.Tools.OfficeButton arrange2x2MenuButton;
        private Syncfusion.Windows.Forms.Tools.OfficeButton arrange3x3MenuButton;
        private Syncfusion.Windows.Forms.Tools.OfficeButton arrange4x4MenuButton;
        private Syncfusion.Windows.Forms.Tools.OfficeButton defaultSizeMenuButton;
        private Syncfusion.Windows.Forms.Tools.OfficeButton closeAllMenuButton;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private Syncfusion.Windows.Forms.Tools.OfficeButton closeMenuButton;
        private Syncfusion.Windows.Forms.Tools.ToolStripCheckBox displaysMenuButton;
        private Syncfusion.Windows.Forms.Tools.ToolStripCheckBox sourcesMenuButton;
        private Syncfusion.Windows.Forms.Tools.ToolStripCheckBox sourcesControlMenuButton;
        private Syncfusion.Windows.Forms.Tools.ToolStripCheckBox equipmentMenuButton;
        private Syncfusion.Windows.Forms.Tools.ToolStripCheckBox equipmentControlMenuButton;
        private UI.PresentationDesign.DesignUI.Controls.DisplayMonitor.DisplayMonitorControl displayMonitorControl1;
        private UI.PresentationDesign.DesignUI.Controls.SourceList.PlayerSourcesControl playerSourcesControl1;
        private UI.PresentationDesign.DesignUI.Controls.ManagementControl.ManagementControl SourceManagementControl;
        private UI.PresentationDesign.DesignUI.Controls.Equipment.PlayerEquipmentControl equipmentControl;
        private UI.PresentationDesign.DesignUI.Controls.ManagementControl.ManagementControl equipmentCommandControl;
        private Syncfusion.Windows.Forms.Tools.StatusStripEx statusStripEx1;
        private Syncfusion.Windows.Forms.Tools.StatusStripLabel SlideLockingStatus;
    }
}