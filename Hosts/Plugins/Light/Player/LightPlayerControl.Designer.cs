namespace Hosts.Plugins.Light.Player
{
    partial class LightPlayerControl
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
                SetControlPlayerTimerEnable(false, 1);
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
            this.gradientPanel2 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.autoLabel1 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.gpDetail = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.scrollersFrame1 = new Syncfusion.Windows.Forms.ScrollersFrame(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gpFillPanel)).BeginInit();
            this.gpFillPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).BeginInit();
            this.gradientPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // gpFillPanel
            // 
            this.gpFillPanel.Controls.Add(this.gpDetail);
            this.gpFillPanel.Controls.Add(this.gradientPanel2);
            this.gpFillPanel.Size = new System.Drawing.Size(210, 138);
            // 
            // gradientPanel2
            // 
            this.gradientPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.gradientPanel2.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gradientPanel2.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel2.BorderSides = System.Windows.Forms.Border3DSide.Bottom;
            this.gradientPanel2.Controls.Add(this.autoLabel1);
            this.gradientPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel2.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel2.Name = "gradientPanel2";
            this.gradientPanel2.Size = new System.Drawing.Size(210, 20);
            this.gradientPanel2.TabIndex = 6;
            // 
            // autoLabel1
            // 
            this.autoLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.autoLabel1.DX = 0;
            this.autoLabel1.DY = 0;
            this.autoLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.autoLabel1.Location = new System.Drawing.Point(49, 0);
            this.autoLabel1.Name = "autoLabel1";
            this.autoLabel1.Size = new System.Drawing.Size(118, 13);
            this.autoLabel1.TabIndex = 0;
            this.autoLabel1.Text = "Группы освещения";
            // 
            // gpDetail
            // 
            this.gpDetail.AutoScroll = true;
            this.gpDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.gpDetail.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gpDetail.BorderColor = System.Drawing.Color.Black;
            this.gpDetail.BorderSides = System.Windows.Forms.Border3DSide.Bottom;
            this.gpDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpDetail.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gpDetail.ForeColor = System.Drawing.Color.MidnightBlue;
            this.gpDetail.Location = new System.Drawing.Point(0, 20);
            this.gpDetail.Name = "gpDetail";
            this.gpDetail.Size = new System.Drawing.Size(210, 118);
            this.gpDetail.TabIndex = 7;
            // 
            // scrollersFrame1
            // 
            this.scrollersFrame1.AttachedTo = this.gpDetail;
            this.scrollersFrame1.HorizontalSmallChange = 10;
            this.scrollersFrame1.SizeGripperVisibility = Syncfusion.Windows.Forms.SizeGripperVisibility.Auto;
            this.scrollersFrame1.VerticallSmallChange = 10;
            this.scrollersFrame1.VisualStyle = Syncfusion.Windows.Forms.ScrollBarCustomDrawStyles.Office2007;
            // 
            // LightPlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MinimumSize = new System.Drawing.Size(210, 176);
            this.Name = "LightPlayerControl";
            this.Size = new System.Drawing.Size(210, 176);
            ((System.ComponentModel.ISupportInitialize)(this.gpFillPanel)).EndInit();
            this.gpFillPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).EndInit();
            this.gradientPanel2.ResumeLayout(false);
            this.gradientPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel2;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel1;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gpDetail;
        private Syncfusion.Windows.Forms.ScrollersFrame scrollersFrame1;
    }
}
