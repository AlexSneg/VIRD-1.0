namespace Hosts.Plugins.IEDocument.UI
{
    partial class IEForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            //TO DO: Строка добавлена из WordForm
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            //this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.panel1.Location = new System.Drawing.Point(0, 0);
            //this.panel1.Margin = new System.Windows.Forms.Padding(0);
            //this.panel1.Name = "panel1";
            //this.panel1.Size = new System.Drawing.Size(292, 266);
            //this.panel1.TabIndex = 0;
            //TO DO: Добавлено из WordForm
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(292, 266);
            this.panel1.TabIndex = 0;
            // 
            // IEForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "IEForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "IEForm";
            this.Load += new System.EventHandler(this.IEForm_Load);
            this.Shown += new System.EventHandler(this.IEForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IEForm_FormClosing);
            //TO DO: Добавлено
            this.panel1.ResumeLayout(false);

            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;

        //private AxDSOFramer.AxFramerControl axFramerControl1;
    }
}