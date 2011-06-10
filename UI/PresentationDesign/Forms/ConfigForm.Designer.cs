namespace UI.PresentationDesign.DesignUI.Forms
{
    partial class ConfigForm
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
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle1 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle2 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle3 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle4 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle1 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle2 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle3 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle4 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle5 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle6 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle7 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle8 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle9 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle10 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle11 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle12 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle13 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle14 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle15 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle16 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle17 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle18 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.cancelButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.okButton = new Syncfusion.Windows.Forms.ButtonAdv();
            this.settingsGrid = new Syncfusion.Windows.Forms.Grid.GridControl();
            ((System.ComponentModel.ISupportInitialize)(this.settingsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(533, 193);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 23;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyle = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2007;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(452, 193);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 22;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyle = true;
            // 
            // settingsGrid
            // 
            gridBaseStyle1.Name = "Row Header";
            gridBaseStyle1.StyleInfo.BaseStyle = "Header";
            gridBaseStyle1.StyleInfo.HorizontalAlignment = Syncfusion.Windows.Forms.Grid.GridHorizontalAlignment.Left;
            gridBaseStyle1.StyleInfo.Interior = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Horizontal, System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(199)))), ((int)(((byte)(184))))), System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(234)))), ((int)(((byte)(216))))));
            gridBaseStyle2.Name = "Column Header";
            gridBaseStyle2.StyleInfo.BaseStyle = "Header";
            gridBaseStyle2.StyleInfo.FloatCell = false;
            gridBaseStyle2.StyleInfo.FloodCell = false;
            gridBaseStyle2.StyleInfo.HorizontalAlignment = Syncfusion.Windows.Forms.Grid.GridHorizontalAlignment.Center;
            gridBaseStyle3.Name = "Standard";
            gridBaseStyle3.StyleInfo.Font.Facename = "Tahoma";
            gridBaseStyle3.StyleInfo.Interior = new Syncfusion.Drawing.BrushInfo(System.Drawing.SystemColors.Window);
            gridBaseStyle4.Name = "Header";
            gridBaseStyle4.StyleInfo.Borders.Bottom = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle4.StyleInfo.Borders.Left = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle4.StyleInfo.Borders.Right = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle4.StyleInfo.Borders.Top = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle4.StyleInfo.CellType = "Header";
            gridBaseStyle4.StyleInfo.Font.Bold = true;
            gridBaseStyle4.StyleInfo.Interior = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Vertical, System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(199)))), ((int)(((byte)(184))))), System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(234)))), ((int)(((byte)(216))))));
            gridBaseStyle4.StyleInfo.VerticalAlignment = Syncfusion.Windows.Forms.Grid.GridVerticalAlignment.Middle;
            this.settingsGrid.BaseStylesMap.AddRange(new Syncfusion.Windows.Forms.Grid.GridBaseStyle[] {
            gridBaseStyle1,
            gridBaseStyle2,
            gridBaseStyle3,
            gridBaseStyle4});
            this.settingsGrid.ColCount = 3;
            this.settingsGrid.ColWidthEntries.AddRange(new Syncfusion.Windows.Forms.Grid.GridColWidth[] {
            new Syncfusion.Windows.Forms.Grid.GridColWidth(0, 35),
            new Syncfusion.Windows.Forms.Grid.GridColWidth(1, 163),
            new Syncfusion.Windows.Forms.Grid.GridColWidth(2, 180),
            new Syncfusion.Windows.Forms.Grid.GridColWidth(3, 262)});
            this.settingsGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.settingsGrid.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsGrid.GridVisualStyles = Syncfusion.Windows.Forms.GridVisualStyles.Office2007Blue;
            this.settingsGrid.HScrollBehavior = Syncfusion.Windows.Forms.Grid.GridScrollbarMode.Disabled;
            this.settingsGrid.Location = new System.Drawing.Point(0, 0);
            this.settingsGrid.Name = "settingsGrid";
            this.settingsGrid.Properties.RowHeaders = false;
            gridRangeStyle1.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Col(3);
            gridRangeStyle1.StyleInfo.ReadOnly = true;
            gridRangeStyle2.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(0, 1);
            gridRangeStyle2.StyleInfo.Enabled = false;
            gridRangeStyle2.StyleInfo.Text = "Параметр";
            gridRangeStyle3.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(0, 2);
            gridRangeStyle3.StyleInfo.StrictValueType = true;
            gridRangeStyle3.StyleInfo.Text = "Значение";
            gridRangeStyle4.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(0, 3);
            gridRangeStyle4.StyleInfo.Text = "Комментарий";
            gridRangeStyle5.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(1, 1);
            gridRangeStyle5.StyleInfo.AutoSize = false;
            gridRangeStyle5.StyleInfo.CellType = "Static";
            gridRangeStyle5.StyleInfo.ShowButtons = Syncfusion.Windows.Forms.Grid.GridShowButtons.Show;
            gridRangeStyle5.StyleInfo.Text = "COMMON_SOURCE_FOLDER";
            gridRangeStyle6.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cells(1, 2, 2, 2);
            gridRangeStyle6.StyleInfo.CellType = "Control";
            gridRangeStyle7.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(1, 3);
            gridRangeStyle7.StyleInfo.AutoSize = false;
            gridRangeStyle7.StyleInfo.CellType = "Static";
            gridRangeStyle7.StyleInfo.ReadOnly = true;
            gridRangeStyle7.StyleInfo.Text = "Папка для хранения общих источников в автономном режиме работы";
            gridRangeStyle7.StyleInfo.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            gridRangeStyle7.StyleInfo.VerticalScrollbar = false;
            gridRangeStyle8.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(2, 1);
            gridRangeStyle8.StyleInfo.AutoSize = false;
            gridRangeStyle8.StyleInfo.CellType = "Static";
            gridRangeStyle8.StyleInfo.Text = "CONFIGURATION_FOLDER";
            gridRangeStyle9.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(2, 3);
            gridRangeStyle9.StyleInfo.AutoSize = false;
            gridRangeStyle9.StyleInfo.CellType = "Static";
            gridRangeStyle9.StyleInfo.ReadOnly = true;
            gridRangeStyle9.StyleInfo.Text = "Папка для хранения конфигураций для автономного режима работы";
            gridRangeStyle9.StyleInfo.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            gridRangeStyle9.StyleInfo.VerticalScrollbar = false;
            gridRangeStyle10.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(3, 1);
            gridRangeStyle10.StyleInfo.AutoSize = false;
            gridRangeStyle10.StyleInfo.CellType = "Static";
            gridRangeStyle10.StyleInfo.Text = "CURRENT_CONFIGURATION";
            gridRangeStyle11.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(3, 2);
            gridRangeStyle11.StyleInfo.CellType = "ComboBox";
            gridRangeStyle11.StyleInfo.DropDownStyle = Syncfusion.Windows.Forms.Grid.GridDropDownStyle.Exclusive;
            gridRangeStyle12.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(3, 3);
            gridRangeStyle12.StyleInfo.AutoSize = false;
            gridRangeStyle12.StyleInfo.CellType = "Static";
            gridRangeStyle12.StyleInfo.ReadOnly = true;
            gridRangeStyle12.StyleInfo.Text = "Текущая конфигурация для автономного режима";
            gridRangeStyle12.StyleInfo.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            gridRangeStyle12.StyleInfo.VerticalScrollbar = false;
            gridRangeStyle13.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(4, 1);
            gridRangeStyle13.StyleInfo.AutoSize = false;
            gridRangeStyle13.StyleInfo.CellType = "Static";
            gridRangeStyle13.StyleInfo.Text = "PRESENTATION_FOLDER";
            gridRangeStyle14.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(4, 2);
            gridRangeStyle14.StyleInfo.CellType = "Control";
            gridRangeStyle15.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(4, 3);
            gridRangeStyle15.StyleInfo.AutoSize = false;
            gridRangeStyle15.StyleInfo.CellType = "Static";
            gridRangeStyle15.StyleInfo.ReadOnly = true;
            gridRangeStyle15.StyleInfo.Text = "Папка для хранения сценариев в автономном режиме работы";
            gridRangeStyle15.StyleInfo.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            gridRangeStyle15.StyleInfo.VerticalScrollbar = false;
            gridRangeStyle16.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(5, 1);
            gridRangeStyle16.StyleInfo.AutoSize = false;
            gridRangeStyle16.StyleInfo.CellType = "Static";
            gridRangeStyle16.StyleInfo.Text = "SOURCE_FOLDER";
            gridRangeStyle17.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(5, 2);
            gridRangeStyle17.StyleInfo.AutoSize = false;
            gridRangeStyle17.StyleInfo.CellType = "Control";
            gridRangeStyle17.StyleInfo.DropDownStyle = Syncfusion.Windows.Forms.Grid.GridDropDownStyle.Editable;
            gridRangeStyle18.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Cell(5, 3);
            gridRangeStyle18.StyleInfo.AutoSize = false;
            gridRangeStyle18.StyleInfo.CellType = "Static";
            gridRangeStyle18.StyleInfo.ReadOnly = true;
            gridRangeStyle18.StyleInfo.Text = "Папка для хранения источников сценариев в автономном режиме работы";
            gridRangeStyle18.StyleInfo.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            gridRangeStyle18.StyleInfo.VerticalScrollbar = false;
            this.settingsGrid.RangeStyles.AddRange(new Syncfusion.Windows.Forms.Grid.GridRangeStyle[] {
            gridRangeStyle1,
            gridRangeStyle2,
            gridRangeStyle3,
            gridRangeStyle4,
            gridRangeStyle5,
            gridRangeStyle6,
            gridRangeStyle7,
            gridRangeStyle8,
            gridRangeStyle9,
            gridRangeStyle10,
            gridRangeStyle11,
            gridRangeStyle12,
            gridRangeStyle13,
            gridRangeStyle14,
            gridRangeStyle15,
            gridRangeStyle16,
            gridRangeStyle17,
            gridRangeStyle18});
            this.settingsGrid.ResizeColsBehavior = Syncfusion.Windows.Forms.Grid.GridResizeCellsBehavior.None;
            this.settingsGrid.ResizeRowsBehavior = Syncfusion.Windows.Forms.Grid.GridResizeCellsBehavior.None;
            this.settingsGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.settingsGrid.RowCount = 5;
            this.settingsGrid.RowHeightEntries.AddRange(new Syncfusion.Windows.Forms.Grid.GridRowHeight[] {
            new Syncfusion.Windows.Forms.Grid.GridRowHeight(0, 23),
            new Syncfusion.Windows.Forms.Grid.GridRowHeight(1, 35),
            new Syncfusion.Windows.Forms.Grid.GridRowHeight(2, 33),
            new Syncfusion.Windows.Forms.Grid.GridRowHeight(3, 30),
            new Syncfusion.Windows.Forms.Grid.GridRowHeight(4, 31),
            new Syncfusion.Windows.Forms.Grid.GridRowHeight(5, 35)});
            this.settingsGrid.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.settingsGrid.Size = new System.Drawing.Size(606, 188);
            this.settingsGrid.SmartSizeBox = false;
            this.settingsGrid.SmoothMouseWheelScrolling = false;
            this.settingsGrid.TabIndex = 24;
            this.settingsGrid.UseRightToLeftCompatibleTextBox = true;
            this.settingsGrid.VScrollBehavior = Syncfusion.Windows.Forms.Grid.GridScrollbarMode.Disabled;
            this.settingsGrid.CellClick += new Syncfusion.Windows.Forms.Grid.GridCellClickEventHandler(this.settingsGrid_CellClick);
            // 
            // ConfigForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(606, 218);
            this.Controls.Add(this.settingsGrid);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Параметры пользователя";
            ((System.ComponentModel.ISupportInitialize)(this.settingsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv cancelButton;
        private Syncfusion.Windows.Forms.ButtonAdv okButton;
        private Syncfusion.Windows.Forms.Grid.GridControl settingsGrid;
    }
}