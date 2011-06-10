using UI.PresentationDesign.DesignUI.Helpers;
namespace UI.PresentationDesign.DesignUI.Controls.Equipment
{
    partial class PlayerEquipmentControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerEquipmentControl));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.gradientPanel1 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.groupView1 = new UI.PresentationDesign.DesignUI.Helpers.SwitchableGroupView();
            this.scrollersFrame1 = new Syncfusion.Windows.Forms.ScrollersFrame(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).BeginInit();
            this.gradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "empty.png");
            this.imageList1.Images.SetKeyName(1, "ok.png");
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.gradientPanel1.BorderColor = System.Drawing.Color.Black;
            this.gradientPanel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gradientPanel1.Controls.Add(this.groupView1);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientPanel1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gradientPanel1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(240, 201);
            this.gradientPanel1.TabIndex = 1;
            // 
            // groupView1
            // 
            this.groupView1.AllowDragDrop = true;
            this.groupView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.groupView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.groupView1.ButtonView = true;
            this.groupView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupView1.FlatLook = true;
            this.groupView1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.groupView1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.groupView1.GroupViewItems.AddRange(new Syncfusion.Windows.Forms.Tools.GroupViewItem[] {
            new Syncfusion.Windows.Forms.Tools.GroupViewItem("GroupViewItem0                                                                   " +
                    "                                             1              ", 0, true, null, "GroupViewItem0"),
            new Syncfusion.Windows.Forms.Tools.GroupViewItem("GroupViewItem1", 1, true, null, "GroupViewItem1"),
            new Syncfusion.Windows.Forms.Tools.GroupViewItem("GroupViewItem2", 0, true, null, "GroupViewItem2")});
            this.groupView1.ImageSpacing = 3;
            this.groupView1.ItemXSpacing = 3;
            this.groupView1.LargeImageList = null;
            this.groupView1.Location = new System.Drawing.Point(0, 0);
            this.groupView1.Name = "groupView1";
            this.groupView1.SelectedHighlightTextColor = System.Drawing.Color.CornflowerBlue;
            this.groupView1.SelectedTextColor = System.Drawing.Color.DarkViolet;
            this.groupView1.Size = new System.Drawing.Size(240, 201);
            this.groupView1.SmallImageList = this.imageList1;
            this.groupView1.SmallImageView = true;
            this.groupView1.TabIndex = 1;
            this.groupView1.Text = "groupView1";
            this.groupView1.ThemesEnabled = true;
            this.groupView1.GroupViewItemSelected += new System.EventHandler(this.groupView1_GroupViewItemSelected);
            // 
            // scrollersFrame1
            // 
            this.scrollersFrame1.AttachedTo = this.groupView1;
            this.scrollersFrame1.HorizontalSmallChange = 10;
            this.scrollersFrame1.RefreshOnValueChange = true;
            this.scrollersFrame1.SizeGripperVisibility = Syncfusion.Windows.Forms.SizeGripperVisibility.Auto;
            this.scrollersFrame1.VerticallSmallChange = 10;
            this.scrollersFrame1.VisualStyle = Syncfusion.Windows.Forms.ScrollBarCustomDrawStyles.Office2007;
            // 
            // PlayerEquipmentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gradientPanel1);
            this.Name = "PlayerEquipmentControl";
            this.Size = new System.Drawing.Size(240, 201);
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).EndInit();
            this.gradientPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private Syncfusion.Windows.Forms.ScrollersFrame scrollersFrame1;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel1;
        private SwitchableGroupView groupView1;
    }
}
