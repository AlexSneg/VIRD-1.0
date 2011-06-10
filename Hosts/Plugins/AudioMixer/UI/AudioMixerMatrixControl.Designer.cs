namespace Hosts.Plugins.AudioMixer.UI
{
    partial class AudioMixerMatrixControl
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
            this.autoLabel1 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.scrollersFrame1 = new Syncfusion.Windows.Forms.ScrollersFrame(this.components);
            this.gpDetail = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.gpOutput = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.gpInput = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpInput)).BeginInit();
            this.SuspendLayout();
            // 
            // autoLabel1
            // 
            this.autoLabel1.AutoSize = false;
            this.autoLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.autoLabel1.DX = 0;
            this.autoLabel1.DY = 0;
            this.autoLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoLabel1.Location = new System.Drawing.Point(0, 0);
            this.autoLabel1.Name = "autoLabel1";
            this.autoLabel1.Size = new System.Drawing.Size(210, 18);
            this.autoLabel1.TabIndex = 0;
            this.autoLabel1.Text = "Матричный микшер";
            this.autoLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scrollersFrame1
            // 
            this.scrollersFrame1.AttachedTo = this.gpDetail;
            this.scrollersFrame1.HorizontalSmallChange = 10;
            this.scrollersFrame1.SizeGripperVisibility = Syncfusion.Windows.Forms.SizeGripperVisibility.Auto;
            this.scrollersFrame1.VerticallSmallChange = 10;
            this.scrollersFrame1.VisualStyle = Syncfusion.Windows.Forms.ScrollBarCustomDrawStyles.Office2007;
            // 
            // gpDetail
            // 
            this.gpDetail.AutoScroll = true;
            this.gpDetail.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gpDetail.BorderColor = System.Drawing.Color.Black;
            this.gpDetail.BorderSides = System.Windows.Forms.Border3DSide.Bottom;
            this.gpDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpDetail.Location = new System.Drawing.Point(55, 40);
            this.gpDetail.Margin = new System.Windows.Forms.Padding(0);
            this.gpDetail.Name = "gpDetail";
            this.gpDetail.Size = new System.Drawing.Size(155, 110);
            this.gpDetail.TabIndex = 4;
            this.gpDetail.Scroll += new System.Windows.Forms.ScrollEventHandler(this.gpDetail_Scroll);
            // 
            // gpOutput
            // 
            this.gpOutput.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gpOutput.BorderColor = System.Drawing.Color.Black;
            this.gpOutput.BorderSides = System.Windows.Forms.Border3DSide.Bottom;
            this.gpOutput.Dock = System.Windows.Forms.DockStyle.Left;
            this.gpOutput.Location = new System.Drawing.Point(0, 18);
            this.gpOutput.Margin = new System.Windows.Forms.Padding(0);
            this.gpOutput.Name = "gpOutput";
            this.gpOutput.Size = new System.Drawing.Size(55, 132);
            this.gpOutput.TabIndex = 2;
            // 
            // gpInput
            // 
            this.gpInput.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.gpInput.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.gpInput.BorderSides = System.Windows.Forms.Border3DSide.Middle;
            this.gpInput.BorderSingle = System.Windows.Forms.ButtonBorderStyle.None;
            this.gpInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpInput.Location = new System.Drawing.Point(55, 18);
            this.gpInput.Name = "gpInput";
            this.gpInput.Size = new System.Drawing.Size(155, 22);
            this.gpInput.TabIndex = 3;
            // 
            // AudioMixerMatrixControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.Controls.Add(this.gpDetail);
            this.Controls.Add(this.gpInput);
            this.Controls.Add(this.gpOutput);
            this.Controls.Add(this.autoLabel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.ForeColor = System.Drawing.Color.MidnightBlue;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(440, 260);
            this.MinimumSize = new System.Drawing.Size(210, 50);
            this.Name = "AudioMixerMatrixControl";
            this.Size = new System.Drawing.Size(210, 150);
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel1;
        private Syncfusion.Windows.Forms.ScrollersFrame scrollersFrame1;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gpOutput;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gpInput;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gpDetail;
    }
}
