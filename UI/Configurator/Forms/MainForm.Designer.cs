namespace UI.PresentationDesign.ConfiguratorUI.Forms
{
    partial class MainForm
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
            Syncfusion.Windows.Forms.Tools.SplitContainerAdv splitContainerAdv1;
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Дисплей активный односегментный");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Дисплей активный многосегментный");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Дисплей пассивный односегментный");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Дисплеи", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Изображение");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Видеофайл");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Удаленный рабочий стол");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Бизнес графика");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Презентация PowerPoint");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("ArcGIS-карта");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Документ Word");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Internet Explorer");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Программные источники", new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            /*treeNode10,*/
            treeNode11,
            treeNode12});
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("DVD");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Видеокамера");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Терминал ВКС");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Стандартный источник изображения");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Аппаратные источники", new System.Windows.Forms.TreeNode[] {
            treeNode14,
            treeNode15,
            treeNode16,
            treeNode17});
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Видеостена");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Логический блок переключателей");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Освещение");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Сервер многоточечной ВКС");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Цифровой аудиомикшер");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Оборудование", new System.Windows.Forms.TreeNode[] {
            treeNode19,
            treeNode20,
            treeNode21,
            treeNode22,
            treeNode23});
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Конфигурация системы", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode13,
            treeNode18,
            treeNode24});
            Syncfusion.Windows.Forms.Tools.TabControlAdv tabControlAdv1;
            this.treeView = new System.Windows.Forms.TreeView();
            this.tabPageAdv1 = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.pgParams = new System.Windows.Forms.PropertyGrid();
            this.tabPageAdv2 = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.gdRelations = new System.Windows.Forms.DataGridView();
            this.msMainMenu = new System.Windows.Forms.MenuStrip();
            this.ConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.InstanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createInstToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteInstToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMainMenu = new System.Windows.Forms.ToolStrip();
            this.SaveStripButton = new System.Windows.Forms.ToolStripButton();
            this.AddStripButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveStripButton = new System.Windows.Forms.ToolStripButton();
            this.CancelStripButton = new System.Windows.Forms.ToolStripButton();
            splitContainerAdv1 = new Syncfusion.Windows.Forms.Tools.SplitContainerAdv();
            tabControlAdv1 = new Syncfusion.Windows.Forms.Tools.TabControlAdv();
            ((System.ComponentModel.ISupportInitialize)(splitContainerAdv1)).BeginInit();
            splitContainerAdv1.Panel1.SuspendLayout();
            splitContainerAdv1.Panel2.SuspendLayout();
            splitContainerAdv1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(tabControlAdv1)).BeginInit();
            tabControlAdv1.SuspendLayout();
            this.tabPageAdv1.SuspendLayout();
            this.tabPageAdv2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdRelations)).BeginInit();
            this.msMainMenu.SuspendLayout();
            this.tsMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerAdv1
            // 
            splitContainerAdv1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerAdv1.Location = new System.Drawing.Point(0, 63);
            splitContainerAdv1.Name = "splitContainerAdv1";
            // 
            // splitContainerAdv1.Panel1
            // 
            splitContainerAdv1.Panel1.AutoSize = true;
            splitContainerAdv1.Panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            splitContainerAdv1.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainerAdv1.Panel2
            // 
            splitContainerAdv1.Panel2.Controls.Add(tabControlAdv1);
            splitContainerAdv1.Size = new System.Drawing.Size(730, 381);
            splitContainerAdv1.SplitterDistance = 327;
            splitContainerAdv1.TabIndex = 3;
            splitContainerAdv1.Text = "splitContainerAdv1";
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            treeNode1.Name = "ComputerDisplayConfig";
            treeNode1.Text = "Дисплей активный односегментный";
            treeNode2.Name = "JupiterDisplayConfig";
            treeNode2.Text = "Дисплей активный многосегментный";
            treeNode3.Name = "MonitorDisplayConfig";
            treeNode3.Text = "Дисплей пассивный односегментный";
            treeNode4.Name = "Display";
            treeNode4.Text = "Дисплеи";
            treeNode5.Name = "ImageSourceConfig";
            treeNode5.Text = "Изображение";
            treeNode6.Name = "VideoSourceConfig";
            treeNode6.Text = "Видеофайл";
            treeNode7.Name = "VNCSourceConfig";
            treeNode7.Text = "Удаленный рабочий стол";
            treeNode8.Name = "BusinessGraphicsSourceConfig";
            treeNode8.Text = "Бизнес графика";
            treeNode9.Name = "PowerPointPresentationSourceConfig";
            treeNode9.Text = "Презентация PowerPoint";
            treeNode10.Name = "ArcGISMapSourceConfig";
            treeNode10.Text = "ArcGIS-карта";
            treeNode11.Name = "WordDocumentSourceConfig";
            treeNode11.Text = "Документ Word";
            treeNode12.Name = "IEDocumentSourceConfig";
            treeNode12.Text = "Internet Explorer";
            treeNode13.Name = "SoftwareSource";
            treeNode13.Text = "Программные источники";
            treeNode14.Name = "DVDPlayerSourceConfig";
            treeNode14.Text = "DVD";
            treeNode15.Name = "VideoCameraSourceConfig";
            treeNode15.Text = "Видеокамера";
            treeNode16.Name = "VDCTerminalSourceConfig";
            treeNode16.Text = "Терминал ВКС";
            treeNode17.Name = "StandardSourceSourceConfig";
            treeNode17.Text = "Стандартный источник изображения";
            treeNode18.Name = "HardwareSource";
            treeNode18.Text = "Аппаратные источники";
            treeNode19.Name = "JupiterDeviceConfig";
            treeNode19.Text = "Видеостена";
            treeNode20.Name = "GangSwitchDeviceConfig";
            treeNode20.Text = "Логический блок переключателей";
            treeNode21.Name = "LightDeviceConfig";
            treeNode21.Text = "Освещение";
            treeNode22.Name = "VDCServerDeviceConfig";
            treeNode22.Text = "Сервер многоточечной ВКС";
            treeNode23.Name = "AudioMixerDeviceConfig";
            treeNode23.Text = "Цифровой аудиомикшер";
            treeNode24.Name = "Equipment";
            treeNode24.Text = "Оборудование";
            treeNode25.Name = "Root";
            treeNode25.Text = "Конфигурация системы";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode25});
            this.treeView.Size = new System.Drawing.Size(324, 381);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // tabControlAdv1
            // 
            tabControlAdv1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            tabControlAdv1.Controls.Add(this.tabPageAdv1);
            tabControlAdv1.Controls.Add(this.tabPageAdv2);
            tabControlAdv1.Location = new System.Drawing.Point(0, 0);
            tabControlAdv1.Name = "tabControlAdv1";
            tabControlAdv1.Size = new System.Drawing.Size(396, 381);
            tabControlAdv1.SizeMode = Syncfusion.Windows.Forms.Tools.TabSizeMode.ShrinkToFit;
            tabControlAdv1.TabIndex = 0;
            // 
            // tabPageAdv1
            // 
            this.tabPageAdv1.Controls.Add(this.pgParams);
            this.tabPageAdv1.Location = new System.Drawing.Point(1, 25);
            this.tabPageAdv1.Name = "tabPageAdv1";
            this.tabPageAdv1.Size = new System.Drawing.Size(393, 354);
            this.tabPageAdv1.TabIndex = 1;
            this.tabPageAdv1.Text = "Параметры";
            this.tabPageAdv1.ThemesEnabled = false;
            // 
            // pgParams
            // 
            this.pgParams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pgParams.HelpVisible = false;
            this.pgParams.Location = new System.Drawing.Point(-1, 0);
            this.pgParams.Name = "pgParams";
            this.pgParams.Size = new System.Drawing.Size(396, 356);
            this.pgParams.TabIndex = 0;
            this.pgParams.ToolbarVisible = false;
            this.pgParams.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.pgParams_SelectedGridItemChanged);
            this.pgParams.Click += new System.EventHandler(this.pgParams_Click);
            this.pgParams.Validating += new System.ComponentModel.CancelEventHandler(this.pgParams_Validating);
            this.pgParams.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgParams_PropertyValueChanged);
            // 
            // tabPageAdv2
            // 
            this.tabPageAdv2.Controls.Add(this.gdRelations);
            this.tabPageAdv2.Location = new System.Drawing.Point(1, 25);
            this.tabPageAdv2.Name = "tabPageAdv2";
            this.tabPageAdv2.Size = new System.Drawing.Size(393, 354);
            this.tabPageAdv2.TabIndex = 2;
            this.tabPageAdv2.Text = "Связи";
            this.tabPageAdv2.ThemesEnabled = false;
            // 
            // gdRelations
            // 
            this.gdRelations.AllowUserToAddRows = false;
            this.gdRelations.AllowUserToDeleteRows = false;
            this.gdRelations.AllowUserToResizeRows = false;
            this.gdRelations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gdRelations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gdRelations.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gdRelations.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gdRelations.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gdRelations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gdRelations.Location = new System.Drawing.Point(0, 0);
            this.gdRelations.MultiSelect = false;
            this.gdRelations.Name = "gdRelations";
            this.gdRelations.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gdRelations.RowHeadersVisible = false;
            this.gdRelations.ShowEditingIcon = false;
            this.gdRelations.Size = new System.Drawing.Size(393, 356);
            this.gdRelations.TabIndex = 2;
            // 
            // msMainMenu
            // 
            this.msMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConfigurationToolStripMenuItem,
            this.InstanceToolStripMenuItem,
            this.menuItemQuit});
            this.msMainMenu.Location = new System.Drawing.Point(0, 0);
            this.msMainMenu.Name = "msMainMenu";
            this.msMainMenu.Size = new System.Drawing.Size(730, 24);
            this.msMainMenu.TabIndex = 1;
            this.msMainMenu.Text = "menuStrip1";
            // 
            // ConfigurationToolStripMenuItem
            // 
            this.ConfigurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSave,
            this.menuItemCancel});
            this.ConfigurationToolStripMenuItem.Name = "ConfigurationToolStripMenuItem";
            this.ConfigurationToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.ConfigurationToolStripMenuItem.Text = "Конфигурация";
            // 
            // menuItemSave
            // 
            this.menuItemSave.Enabled = false;
            this.menuItemSave.Name = "menuItemSave";
            this.menuItemSave.Size = new System.Drawing.Size(140, 22);
            this.menuItemSave.Text = "Сохранить";
            this.menuItemSave.ToolTipText = "Сохранить конфигурацию";
            this.menuItemSave.EnabledChanged += new System.EventHandler(this.SaveToolStripMenuItem_EnabledChanged);
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // menuItemCancel
            // 
            this.menuItemCancel.Enabled = false;
            this.menuItemCancel.Name = "menuItemCancel";
            this.menuItemCancel.Size = new System.Drawing.Size(140, 22);
            this.menuItemCancel.Text = "Отменить";
            this.menuItemCancel.ToolTipText = "Отменить изменения значений параметров";
            this.menuItemCancel.EnabledChanged += new System.EventHandler(this.CancelToolStripMenuItem_EnabledChanged);
            this.menuItemCancel.Click += new System.EventHandler(this.menuItemCancel_Click);
            // 
            // InstanceToolStripMenuItem
            // 
            this.InstanceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCreate,
            this.menuItemRemove});
            this.InstanceToolStripMenuItem.Name = "InstanceToolStripMenuItem";
            this.InstanceToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.InstanceToolStripMenuItem.Text = "Экземпляр";
            // 
            // menuItemCreate
            // 
            this.menuItemCreate.Enabled = false;
            this.menuItemCreate.Name = "menuItemCreate";
            this.menuItemCreate.Size = new System.Drawing.Size(129, 22);
            this.menuItemCreate.Text = "Создать";
            this.menuItemCreate.ToolTipText = "Создать экземпляр";
            this.menuItemCreate.EnabledChanged += new System.EventHandler(this.CreateToolStripMenuItem_EnabledChanged);
            this.menuItemCreate.Click += new System.EventHandler(this.menuItemCreate_Click);
            // 
            // menuItemRemove
            // 
            this.menuItemRemove.Enabled = false;
            this.menuItemRemove.Name = "menuItemRemove";
            this.menuItemRemove.Size = new System.Drawing.Size(129, 22);
            this.menuItemRemove.Text = "Удалить";
            this.menuItemRemove.ToolTipText = "Удалить экземпляр";
            this.menuItemRemove.EnabledChanged += new System.EventHandler(this.RemoveToolStripMenuItem_EnabledChanged);
            this.menuItemRemove.Click += new System.EventHandler(this.menuItemRemove_Click);
            // 
            // menuItemQuit
            // 
            this.menuItemQuit.Name = "menuItemQuit";
            this.menuItemQuit.Size = new System.Drawing.Size(52, 20);
            this.menuItemQuit.Text = "Выход";
            this.menuItemQuit.Click += new System.EventHandler(this.menuItemQuit_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveConfigToolStripMenuItem,
            this.cancelConfigToolStripMenuItem});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.configToolStripMenuItem.Text = "Конфигурация";
            // 
            // saveConfigToolStripMenuItem
            // 
            this.saveConfigToolStripMenuItem.Name = "saveConfigToolStripMenuItem";
            this.saveConfigToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.saveConfigToolStripMenuItem.Text = "Сохранить";
            // 
            // cancelConfigToolStripMenuItem
            // 
            this.cancelConfigToolStripMenuItem.Name = "cancelConfigToolStripMenuItem";
            this.cancelConfigToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.cancelConfigToolStripMenuItem.Text = "Отменить";
            // 
            // instToolStripMenuItem
            // 
            this.instToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createInstToolStripMenuItem,
            this.deleteInstToolStripMenuItem});
            this.instToolStripMenuItem.Name = "instToolStripMenuItem";
            this.instToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.instToolStripMenuItem.Text = "Экземпляр";
            // 
            // createInstToolStripMenuItem
            // 
            this.createInstToolStripMenuItem.Name = "createInstToolStripMenuItem";
            this.createInstToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.createInstToolStripMenuItem.Text = "Создать";
            // 
            // deleteInstToolStripMenuItem
            // 
            this.deleteInstToolStripMenuItem.Name = "deleteInstToolStripMenuItem";
            this.deleteInstToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.deleteInstToolStripMenuItem.Text = "Удалить";
            // 
            // tsMainMenu
            // 
            this.tsMainMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.tsMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveStripButton,
            this.AddStripButton,
            this.RemoveStripButton,
            this.CancelStripButton});
            this.tsMainMenu.Location = new System.Drawing.Point(0, 24);
            this.tsMainMenu.Name = "tsMainMenu";
            this.tsMainMenu.Size = new System.Drawing.Size(730, 39);
            this.tsMainMenu.TabIndex = 2;
            this.tsMainMenu.Text = "toolStrip1";
            // 
            // SaveStripButton
            // 
            this.SaveStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveStripButton.Image = global::UI.PresentationDesign.ConfiguratorUI.Properties.Resources.Save;
            this.SaveStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveStripButton.Name = "SaveStripButton";
            this.SaveStripButton.Size = new System.Drawing.Size(36, 36);
            this.SaveStripButton.Text = "Сохранить";
            this.SaveStripButton.ToolTipText = "Сохранить конфигурацию";
            this.SaveStripButton.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // AddStripButton
            // 
            this.AddStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddStripButton.Enabled = false;
            this.AddStripButton.Image = global::UI.PresentationDesign.ConfiguratorUI.Properties.Resources.Symbol_Add;
            this.AddStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddStripButton.Name = "AddStripButton";
            this.AddStripButton.Size = new System.Drawing.Size(36, 36);
            this.AddStripButton.Text = "Создать";
            this.AddStripButton.ToolTipText = "Создать экземпляр";
            this.AddStripButton.Click += new System.EventHandler(this.menuItemCreate_Click);
            // 
            // RemoveStripButton
            // 
            this.RemoveStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RemoveStripButton.Enabled = false;
            this.RemoveStripButton.Image = global::UI.PresentationDesign.ConfiguratorUI.Properties.Resources.Symbol_Delete;
            this.RemoveStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemoveStripButton.Name = "RemoveStripButton";
            this.RemoveStripButton.Size = new System.Drawing.Size(36, 36);
            this.RemoveStripButton.Text = "Удалить";
            this.RemoveStripButton.ToolTipText = "Удалить экземпляр";
            this.RemoveStripButton.Click += new System.EventHandler(this.menuItemRemove_Click);
            // 
            // CancelStripButton
            // 
            this.CancelStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CancelStripButton.Image = global::UI.PresentationDesign.ConfiguratorUI.Properties.Resources.Undo;
            this.CancelStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CancelStripButton.Name = "CancelStripButton";
            this.CancelStripButton.Size = new System.Drawing.Size(36, 36);
            this.CancelStripButton.Text = "Отменить";
            this.CancelStripButton.ToolTipText = "Отменить изменения значений параметров";
            this.CancelStripButton.Click += new System.EventHandler(this.menuItemCancel_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 444);
            this.Controls.Add(splitContainerAdv1);
            this.Controls.Add(this.tsMainMenu);
            this.Controls.Add(this.msMainMenu);
            this.Name = "MainForm";
            this.Text = "ВИРД - Конфигурирование";
            this.UseOffice2007SchemeBackColor = true;
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            splitContainerAdv1.Panel1.ResumeLayout(false);
            splitContainerAdv1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainerAdv1)).EndInit();
            splitContainerAdv1.ResumeLayout(false);
            splitContainerAdv1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(tabControlAdv1)).EndInit();
            tabControlAdv1.ResumeLayout(false);
            this.tabPageAdv1.ResumeLayout(false);
            this.tabPageAdv2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdRelations)).EndInit();
            this.msMainMenu.ResumeLayout(false);
            this.msMainMenu.PerformLayout();
            this.tsMainMenu.ResumeLayout(false);
            this.tsMainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMainMenu;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem instToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createInstToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteInstToolStripMenuItem;
        private System.Windows.Forms.ToolStrip tsMainMenu;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageAdv1;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageAdv2;
		  private System.Windows.Forms.PropertyGrid pgParams;
        private System.Windows.Forms.ToolStripButton SaveStripButton;
		  private System.Windows.Forms.ToolStripMenuItem ConfigurationToolStripMenuItem;
		  private System.Windows.Forms.ToolStripMenuItem menuItemSave;
		  private System.Windows.Forms.ToolStripMenuItem menuItemCancel;
		  private System.Windows.Forms.ToolStripMenuItem InstanceToolStripMenuItem;
		  private System.Windows.Forms.ToolStripMenuItem menuItemCreate;
		  private System.Windows.Forms.ToolStripMenuItem menuItemRemove;
		  private System.Windows.Forms.ToolStripMenuItem menuItemQuit;
          private System.Windows.Forms.TreeView treeView;
		  private System.Windows.Forms.DataGridView gdRelations;
			private System.Windows.Forms.ToolStripButton AddStripButton;
			private System.Windows.Forms.ToolStripButton RemoveStripButton;
			private System.Windows.Forms.ToolStripButton CancelStripButton;
    }
}