namespace Hosts.Plugins.StandardSource.Player
{
    partial class StandartSourcePlayerControl
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
            this.autoLabel11 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.alStatus = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).BeginInit();
            this.gpDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpDetail
            // 
            this.gpDetail.Controls.Add(this.alStatus);
            this.gpDetail.Controls.Add(this.autoLabel11);
            this.gpDetail.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gpDetail.ForeColor = System.Drawing.Color.MidnightBlue;
            this.gpDetail.Size = new System.Drawing.Size(210, 44);
            // 
            // autoLabel11
            // 
            this.autoLabel11.DX = 0;
            this.autoLabel11.DY = 0;
            this.autoLabel11.Location = new System.Drawing.Point(3, 14);
            this.autoLabel11.Name = "autoLabel11";
            this.autoLabel11.Size = new System.Drawing.Size(47, 13);
            this.autoLabel11.TabIndex = 0;
            this.autoLabel11.Text = "Сигнал:";
            // 
            // alStatus
            // 
            this.alStatus.DX = 0;
            this.alStatus.DY = 0;
            this.alStatus.Location = new System.Drawing.Point(58, 14);
            this.alStatus.Name = "alStatus";
            this.alStatus.Size = new System.Drawing.Size(65, 13);
            this.alStatus.TabIndex = 1;
            this.alStatus.Text = "неизвестно";
            // 
            // StandartSourcePlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MinimumSize = new System.Drawing.Size(216, 203);
            this.Name = "StandartSourcePlayerControl";
            this.Size = new System.Drawing.Size(216, 203);
            ((System.ComponentModel.ISupportInitialize)(this.gpDetail)).EndInit();
            this.gpDetail.ResumeLayout(false);
            this.gpDetail.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel11;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alStatus;
    }
}
