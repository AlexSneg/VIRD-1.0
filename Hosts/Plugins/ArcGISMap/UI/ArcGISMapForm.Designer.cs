namespace Hosts.Plugins.ArcGISMap.UI
{
    partial class ArcGISMapForm
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
            this.map = new Hosts.Plugins.ArcGISMap.UI.Controls.MapControl();
            this.SuspendLayout();
            // 
            // map
            // 
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Name = "map";
            this.map.Scale = 1;
            this.map.Size = new System.Drawing.Size(324, 276);
            this.map.TabIndex = 0;
            this.map.XOffset = 0;
            this.map.YOffset = 0;
            // 
            // ArcGISMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 276);
            this.ControlBox = false;
            this.Controls.Add(this.map);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ArcGISMapForm";
            this.Text = "ArcGISMapForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Hosts.Plugins.ArcGISMap.UI.Controls.MapControl map;
    }
}