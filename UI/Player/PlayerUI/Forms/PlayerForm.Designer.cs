namespace UI.Player.PlayerUI.Forms
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
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbsourcesControl = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbslideDiagramControl = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbdisplayListControl = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbequipmentControl = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbsourcesCommandControl = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbequipmentCommandControl = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            this.dockingManager = new Syncfusion.Windows.Forms.Tools.DockingManager(this.components);
            this.sourcesControl = new UI.PresentationDesign.DesignUI.Controls.SourcesControl();
            this.slideDiagramControl = new UI.PresentationDesign.DesignUI.SlideDiagramControl();
            this.displayListControl = new UI.PresentationDesign.DesignUI.Controls.DisplayList.DisplayListControl();
            this.equipmentControl = new UI.PresentationDesign.DesignUI.Controls.EquipmentControl();
            this.sourcesCommandControl = new UI.Player.PlayerUI.Controls.CommandListControl();
            this.equipmentCommandControl = new UI.Player.PlayerUI.Controls.CommandListControl();
            this.ribbonControlAdv = new Syncfusion.Windows.Forms.Tools.RibbonControlAdv();
            this.viewMenuButton = new Syncfusion.Windows.Forms.Tools.OfficeDropDownButton();
            this.displaysMenuButton = new Syncfusion.Windows.Forms.Tools.ToolStripCheckBox();
            this.sourcesMenuButton = new Syncfusion.Windows.Forms.Tools.ToolStripCheckBox();
            this.sourcesControlMenuButton = new Syncfusion.Windows.Forms.Tools.ToolStripCheckBox();
            this.equipmentMenuButton = new Syncfusion.Windows.Forms.Tools.ToolStripCheckBox();
            this.equipmentControlMenuButton = new Syncfusion.Windows.Forms.Tools.ToolStripCheckBox();
            this.windowsMenuButton = new Syncfusion.Windows.Forms.Tools.OfficeDropDownButton();
            this.cascadeMenuButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.verticalMenuButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.horizontalMenuButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.closeAllMenuButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.closeMenuButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.dockingClientPanel1 = new Syncfusion.Windows.Forms.Tools.DockingClientPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dockingManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControlAdv)).BeginInit();
            this.SuspendLayout();
            // 
            // dockingManager
            // 
            this.dockingManager.AutoHideActiveControl = true;
            this.dockingManager.AutoHideTabFont = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockingManager.CloseEnabled = false;
            this.dockingManager.DockLayoutStream = ((System.IO.MemoryStream)(resources.GetObject("dockingManager.DockLayoutStream")));
            this.dockingManager.DockTabFont = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockingManager.DragProviderStyle = Syncfusion.Windows.Forms.Tools.DragProviderStyle.VS2008;
            this.dockingManager.HostControl = this;
            this.dockingManager.MenuButtonEnabled = false;
            this.dockingManager.Office2007MdiChildForm = true;
            this.dockingManager.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Close, "CloseButton"));
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Pin, "PinButton"));
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Menu, "MenuButton"));
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Maximize, "MaximizeButton"));
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Restore, "RestoreButton"));
            this.dockingManager.SetDockLabel(this.sourcesControl, "Источники");
            ccbsourcesControl.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.sourcesControl, ccbsourcesControl);
            this.dockingManager.SetDockLabel(this.slideDiagramControl, "Граф сцен");
            ccbslideDiagramControl.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.slideDiagramControl, ccbslideDiagramControl);
            this.dockingManager.SetDockLabel(this.displayListControl, "Дисплеи");
            ccbdisplayListControl.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.displayListControl, ccbdisplayListControl);
            this.dockingManager.SetDockLabel(this.equipmentControl, "Оборудование");
            ccbequipmentControl.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.equipmentControl, ccbequipmentControl);
            this.dockingManager.SetDockLabel(this.sourcesCommandControl, "Управление источниками");
            ccbsourcesCommandControl.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.sourcesCommandControl, ccbsourcesCommandControl);
            this.dockingManager.SetDockLabel(this.equipmentCommandControl, "Управление оборудованием");
            ccbequipmentCommandControl.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.equipmentCommandControl, ccbequipmentCommandControl);
            // 
            // sourcesControl
            // 
            this.dockingManager.SetEnableDocking(this.sourcesControl, true);

            this.sourcesControl.Location = new System.Drawing.Point(3, 23);
            this.sourcesControl.Name = "sourcesControl";
            
            this.sourcesControl.Size = new System.Drawing.Size(167, 132);
            this.sourcesControl.TabIndex = 1;
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
            this.displayListControl.Size = new System.Drawing.Size(197, 86);
            this.displayListControl.TabIndex = 4;
            // 
            // equipmentControl
            // 
            this.dockingManager.SetEnableDocking(this.equipmentControl, true);
            this.equipmentControl.Location = new System.Drawing.Point(3, 23);
            this.equipmentControl.Name = "equipmentControl";
            this.equipmentControl.Size = new System.Drawing.Size(197, 73);
            this.equipmentControl.TabIndex = 10;
            // 
            // sourcesCommandControl
            // 
            this.dockingManager.SetEnableDocking(this.sourcesCommandControl, true);
            this.sourcesCommandControl.Location = new System.Drawing.Point(3, 23);
            this.sourcesCommandControl.Name = "sourcesCommandControl";
            this.sourcesCommandControl.Size = new System.Drawing.Size(167, 135);
            this.sourcesCommandControl.TabIndex = 0;
            // 
            // equipmentCommandControl
            // 
            this.dockingManager.SetEnableDocking(this.equipmentCommandControl, true);
            this.equipmentCommandControl.Location = new System.Drawing.Point(3, 23);
            this.equipmentCommandControl.Name = "equipmentCommandControl";
            this.equipmentCommandControl.Size = new System.Drawing.Size(197, 78);
            this.equipmentCommandControl.TabIndex = 0;
            // 
            // ribbonControlAdv
            // 
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
            this.displaysMenuButton.Size = new System.Drawing.Size(169, 17);
            this.displaysMenuButton.Text = "Дисплеи";
            this.displaysMenuButton.CheckedChanged += new System.EventHandler(this.displaysMenuButton_CheckedChanged);
            // 
            // sourcesMenuButton
            // 
            this.sourcesMenuButton.Checked = true;
            this.sourcesMenuButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sourcesMenuButton.Name = "sourcesMenuButton";
            this.sourcesMenuButton.Size = new System.Drawing.Size(169, 17);
            this.sourcesMenuButton.Text = "Источники";
            this.sourcesMenuButton.CheckedChanged += new System.EventHandler(this.displaysMenuButton_CheckedChanged);
            // 
            // sourcesControlMenuButton
            // 
            this.sourcesControlMenuButton.Checked = true;
            this.sourcesControlMenuButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sourcesControlMenuButton.Name = "sourcesControlMenuButton";
            this.sourcesControlMenuButton.Size = new System.Drawing.Size(169, 17);
            this.sourcesControlMenuButton.Text = "Управление источниками";
            this.sourcesControlMenuButton.CheckedChanged += new System.EventHandler(this.displaysMenuButton_CheckedChanged);
            // 
            // equipmentMenuButton
            // 
            this.equipmentMenuButton.Checked = true;
            this.equipmentMenuButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.equipmentMenuButton.Name = "equipmentMenuButton";
            this.equipmentMenuButton.Size = new System.Drawing.Size(169, 17);
            this.equipmentMenuButton.Text = "Оборудование";
            this.equipmentMenuButton.CheckedChanged += new System.EventHandler(this.displaysMenuButton_CheckedChanged);
            // 
            // equipmentControlMenuButton
            // 
            this.equipmentControlMenuButton.Checked = true;
            this.equipmentControlMenuButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.equipmentControlMenuButton.Name = "equipmentControlMenuButton";
            this.equipmentControlMenuButton.Size = new System.Drawing.Size(169, 17);
            this.equipmentControlMenuButton.Text = "Управление оборудованием";
            this.equipmentControlMenuButton.CheckedChanged += new System.EventHandler(this.displaysMenuButton_CheckedChanged);
            // 
            // windowsMenuButton
            // 
            this.windowsMenuButton.DropDownFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.windowsMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cascadeMenuButton,
            this.verticalMenuButton,
            this.horizontalMenuButton,
            this.toolStripLabel1,
            this.closeAllMenuButton});
            this.windowsMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("windowsMenuButton.Image")));
            this.windowsMenuButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.windowsMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.windowsMenuButton.Name = "windowsMenuButton";
            this.windowsMenuButton.Size = new System.Drawing.Size(87, 36);
            this.windowsMenuButton.Text = "Окна";
            // 
            // cascadeMenuButton
            // 
            this.cascadeMenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.cascadeMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("cascadeMenuButton.Image")));
            this.cascadeMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cascadeMenuButton.Name = "cascadeMenuButton";
            this.cascadeMenuButton.Size = new System.Drawing.Size(184, 23);
            this.cascadeMenuButton.Text = "Каскад";
            // 
            // arrange2x2MenuButton
            // 
            this.verticalMenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.verticalMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("verticalMenuButton.Image")));
            this.verticalMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.verticalMenuButton.Name = "verticalMenuButton";
            this.verticalMenuButton.Size = new System.Drawing.Size(184, 23);
            this.verticalMenuButton.Text = "Выстроить по вертикали";
            // 
            // arrange3x3MenuButton
            // 
            this.horizontalMenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.horizontalMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("horizontalMenuButton.Image")));
            this.horizontalMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.horizontalMenuButton.Name = "horizontalMenuButton";
            this.horizontalMenuButton.Size = new System.Drawing.Size(184, 23);
            this.horizontalMenuButton.Text = "Выстроить по горизонтали";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Enabled = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(184, 13);
            this.toolStripLabel1.Text = "_____________________________";
            // 
            // closeAllMenuButton
            // 
            this.closeAllMenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.closeAllMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("closeAllMenuButton.Image")));
            this.closeAllMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.closeAllMenuButton.Name = "closeAllMenuButton";
            this.closeAllMenuButton.Size = new System.Drawing.Size(184, 23);
            this.closeAllMenuButton.Text = "Закрыть все";
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
            this.dockingClientPanel1.Location = new System.Drawing.Point(183, 45);
            this.dockingClientPanel1.Name = "dockingClientPanel1";
            this.dockingClientPanel1.Size = new System.Drawing.Size(438, 323);
            this.dockingClientPanel1.SizeToFit = true;
            this.dockingClientPanel1.TabIndex = 13;
            // 
            // PlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 585);
            this.Controls.Add(this.dockingClientPanel1);
            this.Controls.Add(this.ribbonControlAdv);
            this.Name = "PlayerForm";
            this.Text = "Проигрыватель";
            ((System.ComponentModel.ISupportInitialize)(this.dockingManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControlAdv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.DockingManager dockingManager;
        private Syncfusion.Windows.Forms.Tools.RibbonControlAdv ribbonControlAdv;
        private UI.PresentationDesign.DesignUI.Controls.SourcesControl sourcesControl;
        private UI.PresentationDesign.DesignUI.SlideDiagramControl slideDiagramControl;
        private UI.PresentationDesign.DesignUI.Controls.DisplayList.DisplayListControl displayListControl;
        private UI.PresentationDesign.DesignUI.Controls.EquipmentControl equipmentControl;
        private Syncfusion.Windows.Forms.Tools.DockingClientPanel dockingClientPanel1;
        private Syncfusion.Windows.Forms.Tools.OfficeDropDownButton viewMenuButton;
        private Syncfusion.Windows.Forms.Tools.OfficeDropDownButton windowsMenuButton;
        private Syncfusion.Windows.Forms.Tools.OfficeButton cascadeMenuButton;
        private Syncfusion.Windows.Forms.Tools.OfficeButton verticalMenuButton;
        private Syncfusion.Windows.Forms.Tools.OfficeButton horizontalMenuButton;
        private Syncfusion.Windows.Forms.Tools.OfficeButton closeAllMenuButton;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private Syncfusion.Windows.Forms.Tools.OfficeButton closeMenuButton;
        private Syncfusion.Windows.Forms.Tools.ToolStripCheckBox displaysMenuButton;
        private Syncfusion.Windows.Forms.Tools.ToolStripCheckBox sourcesMenuButton;
        private Syncfusion.Windows.Forms.Tools.ToolStripCheckBox sourcesControlMenuButton;
        private Syncfusion.Windows.Forms.Tools.ToolStripCheckBox equipmentMenuButton;
        private Syncfusion.Windows.Forms.Tools.ToolStripCheckBox equipmentControlMenuButton;
        private UI.Player.PlayerUI.Controls.CommandListControl sourcesCommandControl;
        private UI.Player.PlayerUI.Controls.CommandListControl equipmentCommandControl;
    }
}