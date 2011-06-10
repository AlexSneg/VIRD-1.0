namespace DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator
{
    partial class DeviceHardPluginBaseControl
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
            if ((disposing) && (_controller != null))
            {
                _controller.Dispose();
                _controller = null;
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
            this.gpBottomCommon = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.alAvailable = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.cbaApplayAllPresentation = new Syncfusion.Windows.Forms.Tools.CheckBoxAdv();
            this.gpFillPanel = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            ((System.ComponentModel.ISupportInitialize)(this.gpBottomCommon)).BeginInit();
            this.gpBottomCommon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbaApplayAllPresentation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpFillPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // gpBottomCommon
            // 
            this.gpBottomCommon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.gpBottomCommon.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gpBottomCommon.BorderColor = System.Drawing.Color.Black;
            this.gpBottomCommon.BorderSides = System.Windows.Forms.Border3DSide.Bottom;
            this.gpBottomCommon.Controls.Add(this.alAvailable);
            this.gpBottomCommon.Controls.Add(this.cbaApplayAllPresentation);
            this.gpBottomCommon.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gpBottomCommon.Location = new System.Drawing.Point(0, 87);
            this.gpBottomCommon.Name = "gpBottomCommon";
            this.gpBottomCommon.Size = new System.Drawing.Size(210, 38);
            this.gpBottomCommon.TabIndex = 3;
            // 
            // alAvailable
            // 
            this.alAvailable.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.alAvailable.DX = 0;
            this.alAvailable.DY = 0;
            this.alAvailable.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alAvailable.ForeColor = System.Drawing.Color.Red;
            this.alAvailable.Location = new System.Drawing.Point(19, 19);
            this.alAvailable.Name = "alAvailable";
            this.alAvailable.Size = new System.Drawing.Size(168, 13);
            this.alAvailable.TabIndex = 1;
            this.alAvailable.Text = "нет связи с оборудованием";
            // 
            // cbaApplayAllPresentation
            // 
            this.cbaApplayAllPresentation.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.cbaApplayAllPresentation.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cbaApplayAllPresentation.ForeColor = System.Drawing.Color.MidnightBlue;
            this.cbaApplayAllPresentation.GradientEnd = System.Drawing.SystemColors.ControlDark;
            this.cbaApplayAllPresentation.GradientStart = System.Drawing.SystemColors.Control;
            this.cbaApplayAllPresentation.HotBorderColor = System.Drawing.SystemColors.WindowFrame;
            this.cbaApplayAllPresentation.ImageCheckBoxSize = new System.Drawing.Size(13, 13);
            this.cbaApplayAllPresentation.Location = new System.Drawing.Point(1, -1);
            this.cbaApplayAllPresentation.Name = "cbaApplayAllPresentation";
            this.cbaApplayAllPresentation.ShadowColor = System.Drawing.Color.Black;
            this.cbaApplayAllPresentation.ShadowOffset = new System.Drawing.Point(2, 2);
            this.cbaApplayAllPresentation.Size = new System.Drawing.Size(200, 21);
            this.cbaApplayAllPresentation.TabIndex = 0;
            this.cbaApplayAllPresentation.Text = "Применить для всего сценария";
            this.cbaApplayAllPresentation.ThemesEnabled = false;
            // 
            // gpFillPanel
            // 
            this.gpFillPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.gpFillPanel.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gpFillPanel.BorderColor = System.Drawing.Color.Black;
            this.gpFillPanel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gpFillPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpFillPanel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gpFillPanel.ForeColor = System.Drawing.Color.MidnightBlue;
            this.gpFillPanel.Location = new System.Drawing.Point(0, 0);
            this.gpFillPanel.Name = "gpFillPanel";
            this.gpFillPanel.Size = new System.Drawing.Size(210, 87);
            this.gpFillPanel.TabIndex = 4;
            // 
            // DeviceHardPluginBaseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gpFillPanel);
            this.Controls.Add(this.gpBottomCommon);
            this.Name = "DeviceHardPluginBaseControl";
            this.Size = new System.Drawing.Size(210, 125);
            ((System.ComponentModel.ISupportInitialize)(this.gpBottomCommon)).EndInit();
            this.gpBottomCommon.ResumeLayout(false);
            this.gpBottomCommon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbaApplayAllPresentation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpFillPanel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientPanel gpBottomCommon;
        private Syncfusion.Windows.Forms.Tools.CheckBoxAdv cbaApplayAllPresentation;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alAvailable;
        protected Syncfusion.Windows.Forms.Tools.GradientPanel gpFillPanel;
    }
}
