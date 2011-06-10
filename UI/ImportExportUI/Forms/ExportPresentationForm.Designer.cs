namespace UI.ImportExport.ImportExportUI.Forms
{
    partial class ExportPresentationForm
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
            this.splitContainerAdv1 = new Syncfusion.Windows.Forms.Tools.SplitContainerAdv();
            this.lvFiles = new System.Windows.Forms.ListView();
            this.tbFolder = new System.Windows.Forms.TextBox();
            this.lbFolder = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lbFileOfType = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cbFileName = new System.Windows.Forms.ComboBox();
            this.lbFileName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAdv1)).BeginInit();
            this.splitContainerAdv1.Panel1.SuspendLayout();
            this.splitContainerAdv1.Panel2.SuspendLayout();
            this.splitContainerAdv1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerAdv1
            // 
            this.splitContainerAdv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerAdv1.FixedPanel = Syncfusion.Windows.Forms.Tools.Enums.FixedPanel.Panel2;
            this.splitContainerAdv1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerAdv1.Name = "splitContainerAdv1";
            this.splitContainerAdv1.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // splitContainerAdv1.Panel1
            // 
            this.splitContainerAdv1.Panel1.Controls.Add(this.lvFiles);
            // 
            // splitContainerAdv1.Panel2
            // 
            this.splitContainerAdv1.Panel2.Controls.Add(this.lbFileName);
            this.splitContainerAdv1.Panel2.Controls.Add(this.cbFileName);
            this.splitContainerAdv1.Panel2.Controls.Add(this.tbFolder);
            this.splitContainerAdv1.Panel2.Controls.Add(this.lbFolder);
            this.splitContainerAdv1.Panel2.Controls.Add(this.cbType);
            this.splitContainerAdv1.Panel2.Controls.Add(this.lbFileOfType);
            this.splitContainerAdv1.Panel2.Controls.Add(this.btnCancel);
            this.splitContainerAdv1.Panel2.Controls.Add(this.btnSave);
            this.splitContainerAdv1.Panel2.MinimumSize = new System.Drawing.Size(0, 50);
            this.splitContainerAdv1.Size = new System.Drawing.Size(535, 297);
            this.splitContainerAdv1.SplitterDistance = 196;
            this.splitContainerAdv1.SplitterWidth = 1;
            this.splitContainerAdv1.TabIndex = 0;
            this.splitContainerAdv1.Text = "splitContainerAdv1";
            // 
            // lvFiles
            // 
            this.lvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFiles.Location = new System.Drawing.Point(0, 0);
            this.lvFiles.MultiSelect = false;
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(535, 196);
            this.lvFiles.TabIndex = 0;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.List;
            // 
            // tbFolder
            // 
            this.tbFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFolder.Enabled = false;
            this.tbFolder.Location = new System.Drawing.Point(131, 5);
            this.tbFolder.Name = "tbFolder";
            this.tbFolder.Size = new System.Drawing.Size(246, 20);
            this.tbFolder.TabIndex = 7;
            // 
            // lbFolder
            // 
            this.lbFolder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbFolder.AutoSize = true;
            this.lbFolder.Location = new System.Drawing.Point(42, 12);
            this.lbFolder.Name = "lbFolder";
            this.lbFolder.Size = new System.Drawing.Size(42, 13);
            this.lbFolder.TabIndex = 6;
            this.lbFolder.Text = "Папка:";
            // 
            // cbType
            // 
            this.cbType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.Enabled = false;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "*.xml"});
            this.cbType.Location = new System.Drawing.Point(131, 68);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(246, 21);
            this.cbType.TabIndex = 5;
            // 
            // lbFileOfType
            // 
            this.lbFileOfType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbFileOfType.AutoSize = true;
            this.lbFileOfType.Location = new System.Drawing.Point(42, 71);
            this.lbFileOfType.Name = "lbFileOfType";
            this.lbFileOfType.Size = new System.Drawing.Size(61, 13);
            this.lbFileOfType.TabIndex = 3;
            this.lbFileOfType.Text = "Тип файла";
            this.lbFileOfType.UseMnemonic = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(404, 65);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 24);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSave.Location = new System.Drawing.Point(404, 34);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(81, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbFileName
            // 
            this.cbFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFileName.FormattingEnabled = true;
            this.cbFileName.Location = new System.Drawing.Point(131, 37);
            this.cbFileName.Name = "cbFileName";
            this.cbFileName.Size = new System.Drawing.Size(246, 21);
            this.cbFileName.TabIndex = 8;
            // 
            // lbFileName
            // 
            this.lbFileName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbFileName.AutoSize = true;
            this.lbFileName.Location = new System.Drawing.Point(42, 40);
            this.lbFileName.Name = "lbFileName";
            this.lbFileName.Size = new System.Drawing.Size(64, 13);
            this.lbFileName.TabIndex = 9;
            this.lbFileName.Text = "Имя файла";
            // 
            // ExportPresentationForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(535, 297);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainerAdv1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "ExportPresentationForm";
            this.ShowIcon = false;
            this.Text = "Сохранить как";
            this.Load += new System.EventHandler(this.ExportPresentationForm_Load);
            this.splitContainerAdv1.Panel1.ResumeLayout(false);
            this.splitContainerAdv1.Panel2.ResumeLayout(false);
            this.splitContainerAdv1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAdv1)).EndInit();
            this.splitContainerAdv1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.SplitContainerAdv splitContainerAdv1;
        private System.Windows.Forms.Label lbFileOfType;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.TextBox tbFolder;
        private System.Windows.Forms.Label lbFolder;
        private System.Windows.Forms.ComboBox cbFileName;
        private System.Windows.Forms.Label lbFileName;

    }
}