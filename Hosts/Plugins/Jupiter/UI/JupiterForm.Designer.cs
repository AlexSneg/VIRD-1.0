﻿namespace Hosts.Plugins.Jupiter.UI
{
    partial class JupiterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JupiterForm));
            this.axGalileoCtrl = new AxGALILEOOCXLib.AxGalileoCtrl();
            ((System.ComponentModel.ISupportInitialize)(this.axGalileoCtrl)).BeginInit();
            this.SuspendLayout();
            // 
            // axGalileoCtrl
            // 
            this.axGalileoCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.axGalileoCtrl.Enabled = true;
            this.axGalileoCtrl.Location = new System.Drawing.Point(0, 0);
            this.axGalileoCtrl.Name = "axGalileoCtrl";
            this.axGalileoCtrl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axGalileoCtrl.OcxState")));
            this.axGalileoCtrl.Size = new System.Drawing.Size(512, 403);
            this.axGalileoCtrl.TabIndex = 0;
            // 
            // JupiterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 403);
            this.ControlBox = false;
            this.Controls.Add(this.axGalileoCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "JupiterForm";
            this.Text = "JupiterForm";
            ((System.ComponentModel.ISupportInitialize)(this.axGalileoCtrl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxGALILEOOCXLib.AxGalileoCtrl axGalileoCtrl;
    }
}