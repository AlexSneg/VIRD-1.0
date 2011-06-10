namespace UI.PresentationDesign.DesignUI.Controls.DisplayMonitor
{
    partial class DisplayMonitorControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayMonitorControl));
            this.diagram1 = new Syncfusion.Windows.Forms.Diagram.Controls.Diagram(this.components);
            this.model1 = new Syncfusion.Windows.Forms.Diagram.Model(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.diagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.model1)).BeginInit();
            this.SuspendLayout();
            // 
            // diagram1
            // 
            this.diagram1.AllowDrop = true;
            this.diagram1.Controller.PasteOffset = new System.Drawing.SizeF(10F, 10F);
            this.diagram1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagram1.LayoutManager = null;
            this.diagram1.Location = new System.Drawing.Point(0, 0);
            this.diagram1.Model = this.model1;
            this.diagram1.Name = "diagram1";
            this.diagram1.ScrollVirtualBounds = ((System.Drawing.RectangleF)(resources.GetObject("diagram1.ScrollVirtualBounds")));
            this.diagram1.ShowRulers = false;
            this.diagram1.Size = new System.Drawing.Size(454, 406);
            this.diagram1.SmartSizeBox = false;
            this.diagram1.TabIndex = 0;
            this.diagram1.Text = "diagram1";
            // 
            // 
            // 
            this.diagram1.View.BackgroundColor = System.Drawing.Color.Gray;
            this.diagram1.View.Grid.Color = System.Drawing.Color.Transparent;
            this.diagram1.View.Grid.HorizontalSpacing = 10F;
            this.diagram1.View.Grid.VerticalSpacing = 10F;
            this.diagram1.View.MouseTrackingEnabled = false;
            this.diagram1.View.ScrollVirtualBounds = ((System.Drawing.RectangleF)(resources.GetObject("diagram1.View.ScrollVirtualBounds")));
            this.diagram1.DragDrop += new System.Windows.Forms.DragEventHandler(this.diagram1_DragDrop);
            this.diagram1.DragEnter += new System.Windows.Forms.DragEventHandler(this.diagram1_DragEnter);
            // 
            // model1
            // 
            this.model1.BackgroundStyle.Color = System.Drawing.Color.Gray;
            this.model1.BackgroundStyle.PathBrushStyle = Syncfusion.Windows.Forms.Diagram.PathGradientBrushStyle.RectangleCenter;
            this.model1.BoundaryConstraintsEnabled = false;
            this.model1.CanUngroup = false;
            this.model1.DocumentScale = ((Syncfusion.Windows.Forms.Diagram.PageScale)(resources.GetObject("model1.DocumentScale")));
            this.model1.DocumentSize = ((Syncfusion.Windows.Forms.Diagram.PageSize)(resources.GetObject("model1.DocumentSize")));
            this.model1.LogicalSize = new System.Drawing.SizeF(1000F, 1000F);
            this.model1.OptimizeLineBridging = false;
            this.model1.ShadowStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.model1.ShadowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.model1.Size = new System.Drawing.SizeF(1000F, 1000F);
            // 
            // DisplayMonitorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.diagram1);
            this.Name = "DisplayMonitorControl";
            this.Size = new System.Drawing.Size(454, 406);
            ((System.ComponentModel.ISupportInitialize)(this.diagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.model1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Diagram.Controls.Diagram diagram1;
        private Syncfusion.Windows.Forms.Diagram.Model model1;
    }
}
