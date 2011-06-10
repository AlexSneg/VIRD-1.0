using UI.PresentationDesign.DesignUI.Helpers;
namespace UI.PresentationDesign.DesignUI.Controls
{
    partial class SourcePropertiesControl
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
            this.propertyGrid1 = new UI.PresentationDesign.DesignUI.Helpers.PMediaPropertyGrid();
            this.superToolTip1 = new Syncfusion.Windows.Forms.Tools.SuperToolTip(null);
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.AssignedObject = null;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.IsReadOnly = true;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(316, 263);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // superToolTip1
            // 
            this.superToolTip1.UseFading = Syncfusion.Windows.Forms.Tools.SuperToolTip.FadingType.System;
            // 
            // SourcePropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.propertyGrid1);
            this.Name = "SourcePropertiesControl";
            this.Size = new System.Drawing.Size(316, 263);
            this.ResumeLayout(false);

        }

        #endregion

        private PMediaPropertyGrid propertyGrid1;
        private Syncfusion.Windows.Forms.Tools.SuperToolTip superToolTip1;
    }
}
