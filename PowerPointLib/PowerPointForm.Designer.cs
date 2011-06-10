namespace TechnicalServices.PowerPointLib
{
    partial class PowerPointForm
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
            this.powerPointControl1 = new TechnicalServices.PowerPointLib.PowerPointControl();
            this.SuspendLayout();
            // 
            // powerPointControl1
            // 
            this.powerPointControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.powerPointControl1.Location = new System.Drawing.Point(0, 0);
            this.powerPointControl1.Name = "powerPointControl1";
            this.powerPointControl1.Size = new System.Drawing.Size(800, 600);
            this.powerPointControl1.TabIndex = 0;
            // 
            // PowerPointForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.powerPointControl1);
            this.Name = "PowerPointForm";
            this.Text = "PowerPointForm";
            this.Load += new System.EventHandler(this.PowerPointForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PowerPointForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private PowerPointControl powerPointControl1;

        //private PowerPointControl powerPointControl;
    }
}