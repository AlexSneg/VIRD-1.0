using UI.PresentationDesign.DesignUI.Helpers;
namespace UI.PresentationDesign.DesignUI.Controls
{
    partial class SlideLayoutControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlideLayoutControl));
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo1 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo2 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            Syncfusion.Windows.Forms.Tools.ToolTipInfo toolTipInfo3 = new Syncfusion.Windows.Forms.Tools.ToolTipInfo();
            this.model = new Syncfusion.Windows.Forms.Diagram.Model(this.components);
            this.windowContextMenu = new Syncfusion.Windows.Forms.Tools.ContextMenuStripEx();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bringToFrontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveForwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveBackwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.fullsizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripEx1 = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.moveButton = new System.Windows.Forms.ToolStripButton();
            this.zoomCombo = new UI.PresentationDesign.DesignUI.Helpers.ZoomComboBox();
            this.backgroundPropsButton = new System.Windows.Forms.ToolStripButton();
            this.superToolTip1 = new Syncfusion.Windows.Forms.Tools.SuperToolTip(null);
            this.slideLayoutDiagram = new UI.PresentationDesign.DesignUI.Classes.View.SlideLayoutDiagram(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.model)).BeginInit();
            this.windowContextMenu.SuspendLayout();
            this.toolStripEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slideLayoutDiagram)).BeginInit();
            this.SuspendLayout();
            // 
            // model
            // 
            this.model.BackgroundStyle.Color = System.Drawing.Color.White;
            this.model.BackgroundStyle.ForeColor = System.Drawing.Color.White;
            this.model.BackgroundStyle.PathBrushStyle = Syncfusion.Windows.Forms.Diagram.PathGradientBrushStyle.CircleRightTop;
            this.model.CanUngroup = false;
            this.model.DocumentScale = ((Syncfusion.Windows.Forms.Diagram.PageScale)(resources.GetObject("model.DocumentScale")));
            this.model.DocumentSize = ((Syncfusion.Windows.Forms.Diagram.PageSize)(resources.GetObject("model.DocumentSize")));
            this.model.LogicalSize = new System.Drawing.SizeF(800F, 600F);
            this.model.OptimizeLineBridging = false;
            this.model.ShadowStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.model.ShadowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.model.Size = new System.Drawing.SizeF(800F, 600F);
            // 
            // windowContextMenu
            // 
            this.windowContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem,
            this.toolStripSeparator1,
            this.bringToFrontToolStripMenuItem,
            this.sendToBackToolStripMenuItem,
            this.moveForwardToolStripMenuItem,
            this.moveBackwardToolStripMenuItem,
            this.toolStripMenuItem1,
            this.fullsizeMenuItem,
            this.x1ToolStripMenuItem,
            this.x2ToolStripMenuItem,
            this.x3ToolStripMenuItem});
            this.windowContextMenu.Name = "contextMenuStripEx1";
            this.windowContextMenu.Size = new System.Drawing.Size(170, 214);
            this.windowContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripEx1_Opening);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeToolStripMenuItem.Image")));
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.removeToolStripMenuItem.Text = "Удалить";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
            // 
            // bringToFrontToolStripMenuItem
            // 
            this.bringToFrontToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("bringToFrontToolStripMenuItem.Image")));
            this.bringToFrontToolStripMenuItem.Name = "bringToFrontToolStripMenuItem";
            this.bringToFrontToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.bringToFrontToolStripMenuItem.Text = "На передний план";
            this.bringToFrontToolStripMenuItem.Click += new System.EventHandler(this.bringToFrontToolStripMenuItem_Click);
            // 
            // sendToBackToolStripMenuItem
            // 
            this.sendToBackToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("sendToBackToolStripMenuItem.Image")));
            this.sendToBackToolStripMenuItem.Name = "sendToBackToolStripMenuItem";
            this.sendToBackToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.sendToBackToolStripMenuItem.Text = "На задний план";
            this.sendToBackToolStripMenuItem.Click += new System.EventHandler(this.sendToBackToolStripMenuItem_Click);
            // 
            // moveForwardToolStripMenuItem
            // 
            this.moveForwardToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("moveForwardToolStripMenuItem.Image")));
            this.moveForwardToolStripMenuItem.Name = "moveForwardToolStripMenuItem";
            this.moveForwardToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.moveForwardToolStripMenuItem.Text = "Вперед";
            this.moveForwardToolStripMenuItem.Click += new System.EventHandler(this.moveForwardToolStripMenuItem_Click);
            // 
            // moveBackwardToolStripMenuItem
            // 
            this.moveBackwardToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("moveBackwardToolStripMenuItem.Image")));
            this.moveBackwardToolStripMenuItem.Name = "moveBackwardToolStripMenuItem";
            this.moveBackwardToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.moveBackwardToolStripMenuItem.Text = "Назад";
            this.moveBackwardToolStripMenuItem.Click += new System.EventHandler(this.moveBackwardToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(166, 6);
            // 
            // fullsizeMenuItem
            // 
            this.fullsizeMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fullsizeMenuItem.Image")));
            this.fullsizeMenuItem.Name = "fullsizeMenuItem";
            this.fullsizeMenuItem.Size = new System.Drawing.Size(169, 22);
            this.fullsizeMenuItem.Text = "Во весь экран";
            this.fullsizeMenuItem.Click += new System.EventHandler(this.fullsizeMenuItem_Click);
            // 
            // x1ToolStripMenuItem
            // 
            this.x1ToolStripMenuItem.Name = "x1ToolStripMenuItem";
            this.x1ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.x1ToolStripMenuItem.Text = "1x1";
            this.x1ToolStripMenuItem.Click += new System.EventHandler(this.x1ToolStripMenuItem_Click);
            // 
            // x2ToolStripMenuItem
            // 
            this.x2ToolStripMenuItem.Name = "x2ToolStripMenuItem";
            this.x2ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.x2ToolStripMenuItem.Text = "2x2";
            this.x2ToolStripMenuItem.Click += new System.EventHandler(this.x2ToolStripMenuItem_Click);
            // 
            // x3ToolStripMenuItem
            // 
            this.x3ToolStripMenuItem.Name = "x3ToolStripMenuItem";
            this.x3ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.x3ToolStripMenuItem.Text = "3x3";
            this.x3ToolStripMenuItem.Click += new System.EventHandler(this.x3ToolStripMenuItem_Click);
            // 
            // toolStripEx1
            // 
            this.toolStripEx1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripEx1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripEx1.Image = null;
            this.toolStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator2,
            this.moveButton,
            this.zoomCombo,
            this.backgroundPropsButton});
            this.toolStripEx1.Location = new System.Drawing.Point(0, 0);
            this.toolStripEx1.Name = "toolStripEx1";
            this.toolStripEx1.ShowCaption = false;
            this.toolStripEx1.ShowLauncher = false;
            this.toolStripEx1.Size = new System.Drawing.Size(471, 44);
            this.toolStripEx1.TabIndex = 1;
            this.toolStripEx1.Text = "1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(87, 41);
            this.toolStripLabel1.Text = "Сцена {0} из {1}";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 44);
            // 
            // moveButton
            // 
            this.moveButton.Image = ((System.Drawing.Image)(resources.GetObject("moveButton.Image")));
            this.moveButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.moveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(54, 41);
            this.moveButton.Text = "Двигать";
            this.moveButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            toolTipInfo1.Body.Text = "Выполнить перемещение раскладки";
            toolTipInfo1.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo1.Header.Text = "Двигать";
            this.superToolTip1.SetToolTip(this.moveButton, toolTipInfo1);
            this.moveButton.Click += new System.EventHandler(this.moveButton_Click);
            // 
            // zoomCombo
            // 
            this.zoomCombo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.zoomCombo.Name = "zoomCombo";
            this.zoomCombo.Size = new System.Drawing.Size(60, 44);
            toolTipInfo2.Body.Text = "Изменить масштаб раскладки";
            toolTipInfo2.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo2.Header.Text = "Масштаб";
            this.superToolTip1.SetToolTip(this.zoomCombo, toolTipInfo2);
            // 
            // backgroundPropsButton
            // 
            this.backgroundPropsButton.Image = ((System.Drawing.Image)(resources.GetObject("backgroundPropsButton.Image")));
            this.backgroundPropsButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.backgroundPropsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backgroundPropsButton.Name = "backgroundPropsButton";
            this.backgroundPropsButton.Size = new System.Drawing.Size(34, 41);
            this.backgroundPropsButton.Text = "Фон";
            this.backgroundPropsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            toolTipInfo3.Body.Text = "Редактировать фон";
            toolTipInfo3.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolTipInfo3.Header.Text = "Фон.";
            this.superToolTip1.SetToolTip(this.backgroundPropsButton, toolTipInfo3);
            this.backgroundPropsButton.Click += new System.EventHandler(this.backgroundPropsButton_Click);
            // 
            // superToolTip1
            // 
            this.superToolTip1.UseFading = Syncfusion.Windows.Forms.Tools.SuperToolTip.FadingType.System;
            // 
            // slideLayoutDiagram
            // 
            this.slideLayoutDiagram.AllowDrop = true;
            this.slideLayoutDiagram.AllowNodesDrop = true;
            this.slideLayoutDiagram.Controller.PasteOffset = new System.Drawing.SizeF(10F, 10F);
            this.slideLayoutDiagram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.slideLayoutDiagram.EnableMouseWheelZoom = true;
            this.slideLayoutDiagram.HScroll = true;
            this.slideLayoutDiagram.LayoutManager = null;
            this.slideLayoutDiagram.Location = new System.Drawing.Point(0, 44);
            this.slideLayoutDiagram.Model = this.model;
            this.slideLayoutDiagram.Name = "slideLayoutDiagram";
            this.slideLayoutDiagram.Office2007ScrollBars = true;
            this.slideLayoutDiagram.ReadOnly = false;
            this.slideLayoutDiagram.ScrollVirtualBounds = ((System.Drawing.RectangleF)(resources.GetObject("slideLayoutDiagram.ScrollVirtualBounds")));
            this.slideLayoutDiagram.ShowRulers = true;
            this.slideLayoutDiagram.Size = new System.Drawing.Size(471, 304);
            this.slideLayoutDiagram.SmartSizeBox = false;
            this.slideLayoutDiagram.TabIndex = 0;
            // 
            // 
            // 
            this.slideLayoutDiagram.View.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.slideLayoutDiagram.View.Grid.Color = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.slideLayoutDiagram.View.Grid.DashOffset = 5F;
            this.slideLayoutDiagram.View.Grid.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.slideLayoutDiagram.View.Grid.GridStyle = Syncfusion.Windows.Forms.Diagram.GridStyle.Line;
            this.slideLayoutDiagram.View.Grid.HorizontalSpacing = 10F;
            this.slideLayoutDiagram.View.Grid.VerticalSpacing = 10F;
            this.slideLayoutDiagram.View.Grid.Visible = false;
            this.slideLayoutDiagram.View.MouseTrackingEnabled = false;
            this.slideLayoutDiagram.View.ScrollVirtualBounds = ((System.Drawing.RectangleF)(resources.GetObject("slideLayoutDiagram.View.ScrollVirtualBounds")));
            this.slideLayoutDiagram.VScroll = true;
            this.slideLayoutDiagram.VerticalScroll += new System.Windows.Forms.ScrollEventHandler(this.Scrolling);
            this.slideLayoutDiagram.HorizontalScroll += new System.Windows.Forms.ScrollEventHandler(this.Scrolling);
            this.slideLayoutDiagram.Resize += new System.EventHandler(this.slideLayoutDiagram_Resize);
            // 
            // SlideLayoutControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.slideLayoutDiagram);
            this.Controls.Add(this.toolStripEx1);
            this.Name = "SlideLayoutControl";
            this.Size = new System.Drawing.Size(471, 348);
            ((System.ComponentModel.ISupportInitialize)(this.model)).EndInit();
            this.windowContextMenu.ResumeLayout(false);
            this.toolStripEx1.ResumeLayout(false);
            this.toolStripEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slideLayoutDiagram)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.PresentationDesign.DesignUI.Classes.View.SlideLayoutDiagram slideLayoutDiagram;
        private Syncfusion.Windows.Forms.Diagram.Model model;
        private Syncfusion.Windows.Forms.Tools.ContextMenuStripEx windowContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem bringToFrontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToBackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveForwardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveBackwardToolStripMenuItem;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripEx1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private ZoomComboBox zoomCombo;
        private System.Windows.Forms.ToolStripButton moveButton;
        private Syncfusion.Windows.Forms.Tools.SuperToolTip superToolTip1;
        private System.Windows.Forms.ToolStripButton backgroundPropsButton;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fullsizeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x3ToolStripMenuItem;
    }
}
