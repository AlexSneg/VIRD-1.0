using UI.PresentationDesign.DesignUI.Helpers;
using UI.PresentationDesign.DesignUI.Views;
namespace UI.PresentationDesign.DesignUI.Forms
{
    partial class LayoutPreviewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutPreviewForm));
            this.diagram1 = new UI.PresentationDesign.DesignUI.Views.PreviewDiagram(this.components);
            this.model1 = new Syncfusion.Windows.Forms.Diagram.Model(this.components);
            this.toolStripEx1 = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.moveButton = new System.Windows.Forms.ToolStripButton();
            this.zoomCombo = new UI.PresentationDesign.DesignUI.Helpers.ZoomComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.diagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.model1)).BeginInit();
            this.toolStripEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // diagram1
            // 
            this.diagram1.AllowDrop = true;
            this.diagram1.Controller.PasteOffset = new System.Drawing.SizeF(10F, 10F);
            this.diagram1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagram1.HorizontalScrollTips = true;
            this.diagram1.HorizontalThumbTrack = true;
            this.diagram1.HScroll = true;
            this.diagram1.LayoutManager = null;
            this.diagram1.Location = new System.Drawing.Point(0, 44);
            this.diagram1.Model = this.model1;
            this.diagram1.Name = "diagram1";
            this.diagram1.Office2007ScrollBars = true;
            this.diagram1.ScrollVirtualBounds = ((System.Drawing.RectangleF)(resources.GetObject("diagram1.ScrollVirtualBounds")));
            this.diagram1.ShowRulers = true;
            this.diagram1.Size = new System.Drawing.Size(723, 511);
            this.diagram1.SmartSizeBox = false;
            this.diagram1.TabIndex = 0;
            this.diagram1.Text = "diagram1";
            this.diagram1.VerticalScrollTips = true;
            this.diagram1.VerticalThumbTrack = true;
            // 
            // 
            // 
            this.diagram1.View.Grid.HorizontalSpacing = 10F;
            this.diagram1.View.Grid.VerticalSpacing = 10F;
            this.diagram1.View.Grid.Visible = false;
            this.diagram1.View.MouseTrackingEnabled = false;
            this.diagram1.View.ScrollVirtualBounds = ((System.Drawing.RectangleF)(resources.GetObject("diagram1.View.ScrollVirtualBounds")));
            this.diagram1.VScroll = true;
            // 
            // model1
            // 
            this.model1.BackgroundStyle.Color = System.Drawing.Color.White;
            this.model1.BackgroundStyle.PathBrushStyle = Syncfusion.Windows.Forms.Diagram.PathGradientBrushStyle.RectangleCenter;
            this.model1.CanUngroup = false;
            this.model1.DocumentScale = ((Syncfusion.Windows.Forms.Diagram.PageScale)(resources.GetObject("model1.DocumentScale")));
            this.model1.DocumentSize = ((Syncfusion.Windows.Forms.Diagram.PageSize)(resources.GetObject("model1.DocumentSize")));
            this.model1.LogicalSize = new System.Drawing.SizeF(827F, 1169F);
            this.model1.OptimizeLineBridging = false;
            this.model1.ShadowStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.model1.ShadowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            // 
            // toolStripEx1
            // 
            this.toolStripEx1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripEx1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripEx1.Image = null;
            this.toolStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveButton,
            this.zoomCombo});
            this.toolStripEx1.Location = new System.Drawing.Point(0, 0);
            this.toolStripEx1.Name = "toolStripEx1";
            this.toolStripEx1.ShowCaption = false;
            this.toolStripEx1.ShowLauncher = false;
            this.toolStripEx1.Size = new System.Drawing.Size(723, 44);
            this.toolStripEx1.TabIndex = 2;
            this.toolStripEx1.Text = "1";
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
            this.moveButton.Click += new System.EventHandler(this.moveButton_Click);
            // 
            // zoomCombo
            // 
            this.zoomCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zoomCombo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.zoomCombo.Name = "zoomCombo";
            this.zoomCombo.Size = new System.Drawing.Size(60, 44);
            // 
            // LayoutPreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 555);
            this.Controls.Add(this.diagram1);
            this.Controls.Add(this.toolStripEx1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LayoutPreviewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Предварительный просмотр раскладки";
            ((System.ComponentModel.ISupportInitialize)(this.diagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.model1)).EndInit();
            this.toolStripEx1.ResumeLayout(false);
            this.toolStripEx1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PreviewDiagram diagram1;
        private Syncfusion.Windows.Forms.Diagram.Model model1;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripEx1;
        private System.Windows.Forms.ToolStripButton moveButton;
        private ZoomComboBox zoomCombo;
    }
}