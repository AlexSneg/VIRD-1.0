using Syncfusion.Windows.Forms.Tools;
namespace UI.PresentationDesign.DesignUI.Controls
{
    partial class SourcesControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SourcesControl));
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo1 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo2 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo3 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo4 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo5 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo6 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo7 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo8 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo9 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo10 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo11 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo12 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            this.mainTabControl = new Syncfusion.Windows.Forms.Tools.TabControlAdv();
            this.presentationTabPage = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.toolStripExPresentation = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.presentationAddSourceToolButton = new System.Windows.Forms.ToolStripButton();
            this.copyToGlobalStripButton = new System.Windows.Forms.ToolStripButton();
            this.removePresentationSourceStripButton = new System.Windows.Forms.ToolStripButton();
            this.previewPresentationSourceStripButton = new System.Windows.Forms.ToolStripButton();
            this.presentationSourceProperties = new System.Windows.Forms.ToolStripButton();
            this.searchButton = new System.Windows.Forms.ToolStripButton();
            this.globalSourcesTab = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.toolStripExGlobal = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.addGlobalSourceStripButton = new System.Windows.Forms.ToolStripButton();
            this.copyToPresentationStripButton = new System.Windows.Forms.ToolStripButton();
            this.globalSourceRemoveStripButton = new System.Windows.Forms.ToolStripButton();
            this.globalPreviewStripButton = new System.Windows.Forms.ToolStripButton();
            this.globalSourcePropertyButton = new System.Windows.Forms.ToolStripButton();
            this.globalSearchButton = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip1 = new Syncfusion.Windows.Forms.Tools.ContextMenuStripEx();
            this.superToolTip1 = new Syncfusion.Windows.Forms.Tools.SuperToolTip(null);
            this.presentationSourceGroupBar = new UI.PresentationDesign.DesignUI.Controls.SourceTree.SourceGroupBar();
            this.globalSourceGroupBar = new UI.PresentationDesign.DesignUI.Controls.SourceTree.SourceGroupBar();
            ((System.ComponentModel.ISupportInitialize)(this.mainTabControl)).BeginInit();
            this.mainTabControl.SuspendLayout();
            this.presentationTabPage.SuspendLayout();
            this.toolStripExPresentation.SuspendLayout();
            this.globalSourcesTab.SuspendLayout();
            this.toolStripExGlobal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.presentationSourceGroupBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.globalSourceGroupBar)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTabControl
            // 
            this.mainTabControl.BackColor = System.Drawing.SystemColors.Control;
            this.mainTabControl.Controls.Add(this.presentationTabPage);
            this.mainTabControl.Controls.Add(this.globalSourcesTab);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.Size = new System.Drawing.Size(229, 303);
            this.mainTabControl.TabIndex = 0;
            this.mainTabControl.TabPanelBackColor = System.Drawing.Color.White;
            this.mainTabControl.TabStyle = typeof(Syncfusion.Windows.Forms.Tools.TabRendererVS2008);
            this.mainTabControl.VSLikeScrollButton = true;
            this.mainTabControl.SelectedIndexChanged += new System.EventHandler(this.mainTabControl_SelectedIndexChanged);
            // 
            // presentationTabPage
            // 
            this.presentationTabPage.Controls.Add(this.presentationSourceGroupBar);
            this.presentationTabPage.Controls.Add(this.toolStripExPresentation);
            this.presentationTabPage.Location = new System.Drawing.Point(1, 31);
            this.presentationTabPage.Name = "presentationTabPage";
            this.presentationTabPage.Size = new System.Drawing.Size(226, 270);
            this.presentationTabPage.TabIndex = 1;
            this.presentationTabPage.Text = "Сценарий";
            this.presentationTabPage.ThemesEnabled = false;
            // 
            // toolStripExPresentation
            // 
            this.toolStripExPresentation.CaptionTextStyle = Syncfusion.Windows.Forms.Tools.CaptionTextStyle.Plain;
            this.toolStripExPresentation.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripExPresentation.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripExPresentation.Image = null;
            this.toolStripExPresentation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.presentationAddSourceToolButton,
            this.copyToGlobalStripButton,
            this.removePresentationSourceStripButton,
            this.previewPresentationSourceStripButton,
            this.presentationSourceProperties,
            this.searchButton});
            this.toolStripExPresentation.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripExPresentation.Location = new System.Drawing.Point(0, 0);
            this.toolStripExPresentation.Name = "toolStripExPresentation";
            this.toolStripExPresentation.ShowCaption = true;
            this.toolStripExPresentation.ShowLauncher = false;
            this.toolStripExPresentation.Size = new System.Drawing.Size(226, 36);
            this.toolStripExPresentation.TabIndex = 0;
            this.toolStripExPresentation.Text = "Источники сценария";
            // 
            // presentationAddSourceToolButton
            // 
            this.presentationAddSourceToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.presentationAddSourceToolButton.Image = ((System.Drawing.Image)(resources.GetObject("presentationAddSourceToolButton.Image")));
            this.presentationAddSourceToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.presentationAddSourceToolButton.Name = "presentationAddSourceToolButton";
            this.presentationAddSourceToolButton.Size = new System.Drawing.Size(23, 20);
            this.presentationAddSourceToolButton.Text = "Добавление источника";
            toolTipInfo1.Body.Text = "Добавление источника в сценарий";
            toolTipInfo1.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo1.Header.Text = "Добавить";
            this.superToolTip1.SetToolTip(this.presentationAddSourceToolButton, toolTipInfo1);
            this.presentationAddSourceToolButton.Click += new System.EventHandler(this.presentationAddSourceToolButton_Click);
            // 
            // copyToGlobalStripButton
            // 
            this.copyToGlobalStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToGlobalStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToGlobalStripButton.Image")));
            this.copyToGlobalStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToGlobalStripButton.Name = "copyToGlobalStripButton";
            this.copyToGlobalStripButton.Size = new System.Drawing.Size(23, 20);
            this.copyToGlobalStripButton.Text = "Копировать в Общие";
            toolTipInfo2.Body.Text = "Копировать источник в Общие источники";
            toolTipInfo2.Footer.Text = "";
            toolTipInfo2.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo2.Header.Text = "Копировать";
            this.superToolTip1.SetToolTip(this.copyToGlobalStripButton, toolTipInfo2);
            this.copyToGlobalStripButton.Click += new System.EventHandler(this.copyToGlobalStripButton_Click);
            // 
            // removePresentationSourceStripButton
            // 
            this.removePresentationSourceStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removePresentationSourceStripButton.Image = ((System.Drawing.Image)(resources.GetObject("removePresentationSourceStripButton.Image")));
            this.removePresentationSourceStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removePresentationSourceStripButton.Name = "removePresentationSourceStripButton";
            this.removePresentationSourceStripButton.Size = new System.Drawing.Size(23, 20);
            this.removePresentationSourceStripButton.Text = "Удалить";
            toolTipInfo3.Body.Text = "Удалить источник сценария";
            toolTipInfo3.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo3.Header.Text = "Удалить";
            this.superToolTip1.SetToolTip(this.removePresentationSourceStripButton, toolTipInfo3);
            this.removePresentationSourceStripButton.Click += new System.EventHandler(this.removePresentationSourceStripButton_Click);
            // 
            // previewPresentationSourceStripButton
            // 
            this.previewPresentationSourceStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.previewPresentationSourceStripButton.Image = ((System.Drawing.Image)(resources.GetObject("previewPresentationSourceStripButton.Image")));
            this.previewPresentationSourceStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previewPresentationSourceStripButton.Name = "previewPresentationSourceStripButton";
            this.previewPresentationSourceStripButton.Size = new System.Drawing.Size(23, 20);
            this.previewPresentationSourceStripButton.Text = "Предварительный просмотр";
            toolTipInfo4.Body.Text = "Предварительный просмотр исчтоника сценария";
            toolTipInfo4.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo4.Header.Text = "Предпросмотр";
            this.superToolTip1.SetToolTip(this.previewPresentationSourceStripButton, toolTipInfo4);
            this.previewPresentationSourceStripButton.Click += new System.EventHandler(this.previewPresentationSourceStripButton_Click);
            // 
            // presentationSourceProperties
            // 
            this.presentationSourceProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.presentationSourceProperties.Image = ((System.Drawing.Image)(resources.GetObject("presentationSourceProperties.Image")));
            this.presentationSourceProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.presentationSourceProperties.Name = "presentationSourceProperties";
            this.presentationSourceProperties.Size = new System.Drawing.Size(23, 20);
            this.presentationSourceProperties.Text = "Свойства источника";
            toolTipInfo5.Body.Text = "Свойства источника сценария";
            toolTipInfo5.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo5.Header.Text = "Свойства";
            this.superToolTip1.SetToolTip(this.presentationSourceProperties, toolTipInfo5);
            this.presentationSourceProperties.Click += new System.EventHandler(this.presentationSourceProperties_Click);
            // 
            // searchButton
            // 
            this.searchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.searchButton.Image = ((System.Drawing.Image)(resources.GetObject("searchButton.Image")));
            this.searchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(23, 20);
            this.searchButton.Text = "Поиск";
            toolTipInfo6.Body.Text = "Поиск источника";
            toolTipInfo6.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo6.Header.Text = "Поиск";
            this.superToolTip1.SetToolTip(this.searchButton, toolTipInfo6);
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // globalSourcesTab
            // 
            this.globalSourcesTab.Controls.Add(this.globalSourceGroupBar);
            this.globalSourcesTab.Controls.Add(this.toolStripExGlobal);
            this.globalSourcesTab.Location = new System.Drawing.Point(1, 0);
            this.globalSourcesTab.Name = "globalSourcesTab";
            this.globalSourcesTab.Size = new System.Drawing.Size(226, 301);
            this.globalSourcesTab.TabIndex = 2;
            this.globalSourcesTab.Text = "Общие";
            this.globalSourcesTab.ThemesEnabled = false;
            // 
            // toolStripExGlobal
            // 
            this.toolStripExGlobal.CaptionTextStyle = Syncfusion.Windows.Forms.Tools.CaptionTextStyle.Plain;
            this.toolStripExGlobal.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripExGlobal.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripExGlobal.Image = null;
            this.toolStripExGlobal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addGlobalSourceStripButton,
            this.copyToPresentationStripButton,
            this.globalSourceRemoveStripButton,
            this.globalPreviewStripButton,
            this.globalSourcePropertyButton,
            this.globalSearchButton});
            this.toolStripExGlobal.Location = new System.Drawing.Point(0, 0);
            this.toolStripExGlobal.Name = "toolStripExGlobal";
            this.toolStripExGlobal.ShowCaption = true;
            this.toolStripExGlobal.ShowLauncher = false;
            this.toolStripExGlobal.Size = new System.Drawing.Size(226, 38);
            this.toolStripExGlobal.TabIndex = 1;
            this.toolStripExGlobal.Text = "Общие источники";
            this.toolStripExGlobal.Visible = true;
            // 
            // addGlobalSourceStripButton
            // 
            this.addGlobalSourceStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addGlobalSourceStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addGlobalSourceStripButton.Image")));
            this.addGlobalSourceStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addGlobalSourceStripButton.Name = "addGlobalSourceStripButton";
            this.addGlobalSourceStripButton.Size = new System.Drawing.Size(23, 22);
            this.addGlobalSourceStripButton.Text = "Добавить источник";
            toolTipInfo7.Body.Text = "Добавление общего источника";
            toolTipInfo7.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo7.Header.Text = "Добавить";
            this.superToolTip1.SetToolTip(this.addGlobalSourceStripButton, toolTipInfo7);
            this.addGlobalSourceStripButton.Click += new System.EventHandler(this.addGlobalSourceStripButton_Click);
            // 
            // copyToPresentationStripButton
            // 
            this.copyToPresentationStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToPresentationStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToPresentationStripButton.Image")));
            this.copyToPresentationStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToPresentationStripButton.Name = "copyToPresentationStripButton";
            this.copyToPresentationStripButton.Size = new System.Drawing.Size(23, 22);
            this.copyToPresentationStripButton.Text = "Копировать в сценарий";
            toolTipInfo8.Body.Text = "Копировать общий источник в источники сценария";
            toolTipInfo8.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo8.Header.Text = "Копировать";
            this.superToolTip1.SetToolTip(this.copyToPresentationStripButton, toolTipInfo8);
            this.copyToPresentationStripButton.Click += new System.EventHandler(this.copyToPresentationStripButton_Click);
            // 
            // globalSourceRemoveStripButton
            // 
            this.globalSourceRemoveStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.globalSourceRemoveStripButton.Image = ((System.Drawing.Image)(resources.GetObject("globalSourceRemoveStripButton.Image")));
            this.globalSourceRemoveStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.globalSourceRemoveStripButton.Name = "globalSourceRemoveStripButton";
            this.globalSourceRemoveStripButton.Size = new System.Drawing.Size(23, 22);
            this.globalSourceRemoveStripButton.Text = "Удалить общий источник";
            toolTipInfo9.Body.Text = "Удалить общий источник";
            toolTipInfo9.Footer.Text = "";
            toolTipInfo9.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo9.Header.Text = "Удалить";
            this.superToolTip1.SetToolTip(this.globalSourceRemoveStripButton, toolTipInfo9);
            this.globalSourceRemoveStripButton.Click += new System.EventHandler(this.globalSourceRemoveStripButton_Click);
            // 
            // globalPreviewStripButton
            // 
            this.globalPreviewStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.globalPreviewStripButton.Enabled = false;
            this.globalPreviewStripButton.Image = ((System.Drawing.Image)(resources.GetObject("globalPreviewStripButton.Image")));
            this.globalPreviewStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.globalPreviewStripButton.Name = "globalPreviewStripButton";
            this.globalPreviewStripButton.Size = new System.Drawing.Size(23, 22);
            this.globalPreviewStripButton.Text = "Предварительный просмотр источника";
            toolTipInfo10.Body.Text = "Предварительный просмотр источника";
            toolTipInfo10.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo10.Header.Text = "Предпросмотр";
            this.superToolTip1.SetToolTip(this.globalPreviewStripButton, toolTipInfo10);
            this.globalPreviewStripButton.Click += new System.EventHandler(this.globalPreviewStripButton_Click);
            // 
            // globalSourcePropertyButton
            // 
            this.globalSourcePropertyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.globalSourcePropertyButton.Image = ((System.Drawing.Image)(resources.GetObject("globalSourcePropertyButton.Image")));
            this.globalSourcePropertyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.globalSourcePropertyButton.Name = "globalSourcePropertyButton";
            this.globalSourcePropertyButton.Size = new System.Drawing.Size(23, 22);
            toolTipInfo11.Body.Text = "Свойства общего источника";
            toolTipInfo11.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo11.Header.Text = "Свойства";
            this.superToolTip1.SetToolTip(this.globalSourcePropertyButton, toolTipInfo11);
            this.globalSourcePropertyButton.Click += new System.EventHandler(this.commonSourcePropertyButton_Click);
            // 
            // globalSearchButton
            // 
            this.globalSearchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.globalSearchButton.Image = ((System.Drawing.Image)(resources.GetObject("globalSearchButton.Image")));
            this.globalSearchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.globalSearchButton.Name = "globalSearchButton";
            this.globalSearchButton.Size = new System.Drawing.Size(23, 22);
            this.globalSearchButton.Text = "Поиск";
            toolTipInfo12.Body.Text = "Поиск источника";
            toolTipInfo12.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo12.Header.Text = "Поиск";
            this.superToolTip1.SetToolTip(this.globalSearchButton, toolTipInfo12);
            this.globalSearchButton.Click += new System.EventHandler(this.globalSearchButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // superToolTip1
            // 
            this.superToolTip1.UseFading = Syncfusion.Windows.Forms.Tools.SuperToolTip.FadingType.System;
            // 
            // presentationSourceGroupBar
            // 
            this.presentationSourceGroupBar.AllowDrop = true;
            this.presentationSourceGroupBar.BackColor = System.Drawing.Color.Ivory;
            this.presentationSourceGroupBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.presentationSourceGroupBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.presentationSourceGroupBar.FlatLook = true;
            this.presentationSourceGroupBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.presentationSourceGroupBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(77)))), ((int)(((byte)(140)))));
            this.presentationSourceGroupBar.HeaderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(65)))), ((int)(((byte)(140)))));
            this.presentationSourceGroupBar.Location = new System.Drawing.Point(0, 36);
            this.presentationSourceGroupBar.Name = "presentationSourceGroupBar";
            this.presentationSourceGroupBar.PopupClientSize = new System.Drawing.Size(0, 0);
            this.presentationSourceGroupBar.ShowChevron = false;
            this.presentationSourceGroupBar.ShowItemImageInHeader = true;
            this.presentationSourceGroupBar.Size = new System.Drawing.Size(226, 234);
            this.presentationSourceGroupBar.TabIndex = 0;
            this.presentationSourceGroupBar.Text = "sourceGroupBar1";
            this.presentationSourceGroupBar.TextAlign = Syncfusion.Windows.Forms.Tools.TextAlignment.Left;
            this.presentationSourceGroupBar.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.presentationSourceGroupBar.GroupBarItemSelected += new System.EventHandler(this.presentationSourceGroupBar_GroupBarItemSelected);
            // 
            // globalSourceGroupBar
            // 
            this.globalSourceGroupBar.AllowDrop = true;
            this.globalSourceGroupBar.BackColor = System.Drawing.Color.Ivory;
            this.globalSourceGroupBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.globalSourceGroupBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.globalSourceGroupBar.FlatLook = true;
            this.globalSourceGroupBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.globalSourceGroupBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(77)))), ((int)(((byte)(140)))));
            this.globalSourceGroupBar.HeaderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(65)))), ((int)(((byte)(140)))));
            this.globalSourceGroupBar.Location = new System.Drawing.Point(0, 38);
            this.globalSourceGroupBar.Name = "globalSourceGroupBar";
            this.globalSourceGroupBar.PopupClientSize = new System.Drawing.Size(0, 0);
            this.globalSourceGroupBar.ShowChevron = false;
            this.globalSourceGroupBar.ShowItemImageInHeader = true;
            this.globalSourceGroupBar.Size = new System.Drawing.Size(226, 263);
            this.globalSourceGroupBar.TabIndex = 2;
            this.globalSourceGroupBar.Text = "sourceGroupBar1";
            this.globalSourceGroupBar.TextAlign = Syncfusion.Windows.Forms.Tools.TextAlignment.Left;
            this.globalSourceGroupBar.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.globalSourceGroupBar.GroupBarItemSelected += new System.EventHandler(this.globalSourceGroupBar_GroupBarItemSelected);
            // 
            // SourcesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTabControl);
            this.Name = "SourcesControl";
            this.Size = new System.Drawing.Size(229, 303);
            ((System.ComponentModel.ISupportInitialize)(this.mainTabControl)).EndInit();
            this.mainTabControl.ResumeLayout(false);
            this.presentationTabPage.ResumeLayout(false);
            this.presentationTabPage.PerformLayout();
            this.toolStripExPresentation.ResumeLayout(false);
            this.toolStripExPresentation.PerformLayout();
            this.globalSourcesTab.ResumeLayout(false);
            this.globalSourcesTab.PerformLayout();
            this.toolStripExGlobal.ResumeLayout(false);
            this.toolStripExGlobal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.presentationSourceGroupBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.globalSourceGroupBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.TabControlAdv mainTabControl;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv presentationTabPage;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv globalSourcesTab;
        private UI.PresentationDesign.DesignUI.Controls.SourceTree.SourceGroupBar presentationSourceGroupBar;
        private ContextMenuStripEx contextMenuStrip1;
        private ToolStripEx toolStripExPresentation;
        private System.Windows.Forms.ToolStripButton presentationAddSourceToolButton;
        private System.Windows.Forms.ToolStripButton copyToGlobalStripButton;
        private System.Windows.Forms.ToolStripButton removePresentationSourceStripButton;
        private SuperToolTip superToolTip1;
        private System.Windows.Forms.ToolStripButton previewPresentationSourceStripButton;
        private ToolStripEx toolStripExGlobal;
        private System.Windows.Forms.ToolStripButton addGlobalSourceStripButton;
        private System.Windows.Forms.ToolStripButton copyToPresentationStripButton;
        private System.Windows.Forms.ToolStripButton globalSourceRemoveStripButton;
        private System.Windows.Forms.ToolStripButton globalPreviewStripButton;
        private UI.PresentationDesign.DesignUI.Controls.SourceTree.SourceGroupBar globalSourceGroupBar;
        private System.Windows.Forms.ToolStripButton presentationSourceProperties;
        private System.Windows.Forms.ToolStripButton globalSourcePropertyButton;
        private System.Windows.Forms.ToolStripButton searchButton;
        private System.Windows.Forms.ToolStripButton globalSearchButton;


    }
}
