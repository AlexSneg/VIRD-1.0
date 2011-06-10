namespace UI.PresentationDesign.DesignUI.Controls
{
    partial class PresentationOverviewControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PresentationOverviewControl));
            this.toolStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.zoomToolButton = new System.Windows.Forms.ToolStripButton();
            this.panToolButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.zoomCombo = new Syncfusion.Windows.Forms.Tools.ToolStripComboBoxEx();
            this.model = new Syncfusion.Windows.Forms.Diagram.Model(this.components);
            this.diagram = new UI.PresentationDesign.DesignUI.Controls.PresentationDiagram(this.components);
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.model)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagram)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Image = null;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToolButton,
            this.panToolButton,
            this.toolStripSeparator1,
            this.zoomCombo});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.ShowCaption = false;
            this.toolStrip.Size = new System.Drawing.Size(326, 44);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStripEx1";
            // 
            // zoomToolButton
            // 
            this.zoomToolButton.Image = ((System.Drawing.Image)(resources.GetObject("zoomToolButton.Image")));
            this.zoomToolButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.zoomToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomToolButton.Name = "zoomToolButton";
            this.zoomToolButton.Size = new System.Drawing.Size(98, 41);
            this.zoomToolButton.Text = "Масштабировать";
            this.zoomToolButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.zoomToolButton.Click += new System.EventHandler(this.zoomToolButton_Click);
            // 
            // panToolButton
            // 
            this.panToolButton.Image = ((System.Drawing.Image)(resources.GetObject("panToolButton.Image")));
            this.panToolButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.panToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.panToolButton.Name = "panToolButton";
            this.panToolButton.Size = new System.Drawing.Size(54, 41);
            this.panToolButton.Text = "Двигать";
            this.panToolButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.panToolButton.Click += new System.EventHandler(this.panToolButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 44);
            // 
            // zoomCombo
            // 
            this.zoomCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zoomCombo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.zoomCombo.Name = "zoomCombo";
            this.zoomCombo.Size = new System.Drawing.Size(60, 44);
            // 
            // model
            // 
            this.model.BackgroundStyle.Color = System.Drawing.Color.White;
            this.model.BackgroundStyle.PathBrushStyle = Syncfusion.Windows.Forms.Diagram.PathGradientBrushStyle.RectangleCenter;
            this.model.CanUngroup = false;
            this.model.DocumentScale = ((Syncfusion.Windows.Forms.Diagram.PageScale)(resources.GetObject("model.DocumentScale")));
            this.model.DocumentSize = ((Syncfusion.Windows.Forms.Diagram.PageSize)(resources.GetObject("model.DocumentSize")));
            this.model.LogicalSize = new System.Drawing.SizeF(827F, 1169F);
            this.model.OptimizeLineBridging = false;
            this.model.ShadowStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.model.ShadowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            // 
            // diagram
            // 
            this.diagram.Controller.PasteOffset = new System.Drawing.SizeF(10F, 10F);
            this.diagram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagram.EnableMouseWheelZoom = false;
            this.diagram.HorizontalScrollTips = true;
            this.diagram.HorizontalThumbTrack = true;
            this.diagram.HScroll = true;
            this.diagram.LayoutManager = null;
            this.diagram.Location = new System.Drawing.Point(0, 44);
            this.diagram.Model = null;
            this.diagram.Name = "diagram";
            this.diagram.Office2007ScrollBars = true;
            this.diagram.ReadOnly = false;
            this.diagram.ScrollTipFormat = "Позиция {0} ";
            this.diagram.ScrollVirtualBounds = ((System.Drawing.RectangleF)(resources.GetObject("diagram.ScrollVirtualBounds")));
            this.diagram.ShowRulers = false;
            this.diagram.Size = new System.Drawing.Size(326, 156);
            this.diagram.SmartSizeBox = false;
            this.diagram.TabIndex = 1;
            this.diagram.Text = "diagram";
            this.diagram.VerticalThumbTrack = true;
            // 
            // 
            // 
            this.diagram.View.Grid.HorizontalSpacing = 10F;
            this.diagram.View.Grid.VerticalSpacing = 10F;
            this.diagram.View.Grid.Visible = false;
            this.diagram.View.HandleColor = System.Drawing.Color.LightGreen;
            this.diagram.View.MouseTrackingEnabled = false;
            this.diagram.View.ScrollVirtualBounds = ((System.Drawing.RectangleF)(resources.GetObject("diagram.View.ScrollVirtualBounds")));
            this.diagram.VScroll = true;
            // 
            // PresentationOverviewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.diagram);
            this.Controls.Add(this.toolStrip);
            this.Name = "PresentationOverviewControl";
            this.Size = new System.Drawing.Size(326, 200);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.model)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagram)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PresentationDiagram diagram;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStrip;
        private System.Windows.Forms.ToolStripButton zoomToolButton;
        private System.Windows.Forms.ToolStripButton panToolButton;
        private Syncfusion.Windows.Forms.Diagram.Model model;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Syncfusion.Windows.Forms.Tools.ToolStripComboBoxEx zoomCombo;
    }
}
