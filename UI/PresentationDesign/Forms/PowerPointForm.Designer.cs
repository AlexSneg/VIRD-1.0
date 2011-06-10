namespace UI.PresentationDesign.DesignUI.Forms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PowerPointForm));
            this.framerControl = new AxDSOFramer.AxFramerControl();
            this.statusStripEx1 = new Syncfusion.Windows.Forms.Tools.StatusStripEx();
            ((System.ComponentModel.ISupportInitialize)(this.framerControl)).BeginInit();
            this.SuspendLayout();
            // 
            // framerControl
            // 
            this.framerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.framerControl.Enabled = true;
            this.framerControl.Location = new System.Drawing.Point(0, 0);
            this.framerControl.Name = "framerControl";
            this.framerControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("framerControl.OcxState")));
            this.framerControl.Size = new System.Drawing.Size(624, 379);
            this.framerControl.TabIndex = 0;
            this.framerControl.OnDocumentOpened += new AxDSOFramer._DFramerCtlEvents_OnDocumentOpenedEventHandler(this.framerControl_OnDocumentOpened);
            this.framerControl.OnSaveCompleted += new AxDSOFramer._DFramerCtlEvents_OnSaveCompletedEventHandler(this.framerControl_OnSaveCompleted);
            this.framerControl.OnDocumentClosed += new System.EventHandler(this.framerControl_OnDocumentClosed);
            this.framerControl.OnFileCommand += new AxDSOFramer._DFramerCtlEvents_OnFileCommandEventHandler(this.framerControl_OnFileCommand);
            // 
            // statusStripEx1
            // 
            this.statusStripEx1.Dock = Syncfusion.Windows.Forms.Tools.DockStyleEx.Bottom;
            this.statusStripEx1.Location = new System.Drawing.Point(0, 379);
            this.statusStripEx1.Name = "statusStripEx1";
            this.statusStripEx1.Size = new System.Drawing.Size(624, 22);
            this.statusStripEx1.TabIndex = 1;
            this.statusStripEx1.Text = "statusStripEx1";
            // 
            // PowerPointForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 401);
            this.Controls.Add(this.framerControl);
            this.Controls.Add(this.statusStripEx1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PowerPointForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование подложки";
            this.UseOffice2007SchemeBackColor = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PowerPointForm_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PowerPointForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.framerControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxDSOFramer.AxFramerControl framerControl;
        private Syncfusion.Windows.Forms.Tools.StatusStripEx statusStripEx1;
    }
}