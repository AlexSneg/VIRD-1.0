namespace Hosts.Plugins.Light.Player
{
    partial class LightGroupControl
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
            this.alName = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.comboBoxAdv1 = new Syncfusion.Windows.Forms.Tools.ComboBoxAdv();
            this.trackBarEx1 = new Syncfusion.Windows.Forms.Tools.TrackBarEx(0, 100);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAdv1)).BeginInit();
            this.SuspendLayout();
            // 
            // alName
            // 
            this.alName.AutoSize = false;
            this.alName.DX = 0;
            this.alName.DY = 0;
            this.alName.Location = new System.Drawing.Point(2, 5);
            this.alName.Name = "alName";
            this.alName.Size = new System.Drawing.Size(87, 13);
            this.alName.TabIndex = 0;
            this.alName.Text = "autoLabel1";
            // 
            // comboBoxAdv1
            // 
            this.comboBoxAdv1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(251)))));
            this.comboBoxAdv1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAdv1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.comboBoxAdv1.IgnoreThemeBackground = true;
            this.comboBoxAdv1.Items.AddRange(new object[] {
            "Выключен",
            "Включен"});
            this.comboBoxAdv1.ItemsImageIndexes.Add(new Syncfusion.Windows.Forms.Tools.ComboBoxAdv.ImageIndexItem(this.comboBoxAdv1, "Выключен"));
            this.comboBoxAdv1.ItemsImageIndexes.Add(new Syncfusion.Windows.Forms.Tools.ComboBoxAdv.ImageIndexItem(this.comboBoxAdv1, "Включен"));
            this.comboBoxAdv1.Location = new System.Drawing.Point(90, 1);
            this.comboBoxAdv1.Name = "comboBoxAdv1";
            this.comboBoxAdv1.Size = new System.Drawing.Size(115, 19);
            this.comboBoxAdv1.Style = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.comboBoxAdv1.TabIndex = 1;
            this.comboBoxAdv1.Tag = "LightSetGroupState,";
            this.comboBoxAdv1.Text = "Выключен";
            this.comboBoxAdv1.SelectedIndexChanged += new System.EventHandler(this.comboBoxAdv1_SelectedIndexChanged);
            // 
            // trackBarEx1
            // 
            this.trackBarEx1.DecreaseButtonSize = new System.Drawing.Size(0, 0);
            this.trackBarEx1.IncreaseButtonSize = new System.Drawing.Size(0, 0);
            this.trackBarEx1.Location = new System.Drawing.Point(90, 1);
            this.trackBarEx1.Name = "trackBarEx1";
            this.trackBarEx1.ShowButtons = false;
            this.trackBarEx1.Size = new System.Drawing.Size(115, 20);
            this.trackBarEx1.TabIndex = 2;
            this.trackBarEx1.Tag = "LightSetGroupLevel,";
            this.trackBarEx1.Text = "trackBarEx1";
            this.trackBarEx1.TimerInterval = 100;
            this.trackBarEx1.Transparent = true;
            this.trackBarEx1.Value = 0;
            this.trackBarEx1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LightGroupControl_MouseUp);
            // 
            // LightGroupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.Controls.Add(this.trackBarEx1);
            this.Controls.Add(this.comboBoxAdv1);
            this.Controls.Add(this.alName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.ForeColor = System.Drawing.Color.MidnightBlue;
            this.Name = "LightGroupControl";
            this.Size = new System.Drawing.Size(210, 22);
            this.Tag = "LightSetGroupLevel,";
            this.Resize += new System.EventHandler(this.LightGroupControl_Resize);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LightGroupControl_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAdv1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.AutoLabel alName;
        private Syncfusion.Windows.Forms.Tools.ComboBoxAdv comboBoxAdv1;
        private Syncfusion.Windows.Forms.Tools.TrackBarEx trackBarEx1;
    }
}
