namespace Hosts.Plugins.WordDocument.Player
{
    partial class PlayerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerControl));
            this.lbInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPageNumber = new System.Windows.Forms.TextBox();
            this.btnGoTo = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tbeZoom = new Syncfusion.Windows.Forms.Tools.TrackBarEx(0, 10);
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.itbZoom = new Syncfusion.Windows.Forms.Tools.IntegerTextBox();
            this.alZoomMin = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.alZoomMax = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.alZoom = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.itbZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Location = new System.Drawing.Point(100, 8);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(0, 13);
            this.lbInfo.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(93, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "Номер страницы";
            // 
            // tbPageNumber
            // 
            this.tbPageNumber.Location = new System.Drawing.Point(96, 53);
            this.tbPageNumber.Name = "tbPageNumber";
            this.tbPageNumber.Size = new System.Drawing.Size(34, 20);
            this.tbPageNumber.TabIndex = 4;
            // 
            // btnGoTo
            // 
            this.btnGoTo.Location = new System.Drawing.Point(133, 52);
            this.btnGoTo.Name = "btnGoTo";
            this.btnGoTo.Size = new System.Drawing.Size(67, 23);
            this.btnGoTo.TabIndex = 5;
            this.btnGoTo.Text = "Перейти";
            this.btnGoTo.UseVisualStyleBackColor = true;
            this.btnGoTo.Click += new System.EventHandler(this.btnGoTo_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tbeZoom
            // 
            this.tbeZoom.ButtonColor = System.Drawing.SystemColors.Control;
            this.tbeZoom.Location = new System.Drawing.Point(30, 148);
            this.tbeZoom.Name = "tbeZoom";
            this.tbeZoom.Size = new System.Drawing.Size(140, 20);
            this.tbeZoom.TabIndex = 13;
            this.tbeZoom.TimerInterval = 100;
            this.tbeZoom.TrackBarGradientEnd = System.Drawing.SystemColors.ControlDark;
            this.tbeZoom.TrackBarGradientStart = System.Drawing.SystemColors.Control;
            this.tbeZoom.Value = 5;
            this.tbeZoom.ValueChanged += new System.EventHandler(this.tbeZoom_ValueChanged);
            this.tbeZoom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbeZoom_MouseUp);
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.Image = ((System.Drawing.Image)(resources.GetObject("btnFirstPage.Image")));
            this.btnFirstPage.Location = new System.Drawing.Point(5, 93);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(25, 25);
            this.btnFirstPage.TabIndex = 9;
            this.btnFirstPage.UseVisualStyleBackColor = true;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // btnLastPage
            // 
            this.btnLastPage.Image = ((System.Drawing.Image)(resources.GetObject("btnLastPage.Image")));
            this.btnLastPage.Location = new System.Drawing.Point(80, 93);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(25, 25);
            this.btnLastPage.TabIndex = 8;
            this.btnLastPage.UseVisualStyleBackColor = true;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Image = ((System.Drawing.Image)(resources.GetObject("btnNextPage.Image")));
            this.btnNextPage.Location = new System.Drawing.Point(55, 93);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(25, 25);
            this.btnNextPage.TabIndex = 7;
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Image = ((System.Drawing.Image)(resources.GetObject("btnPrevPage.Image")));
            this.btnPrevPage.Location = new System.Drawing.Point(30, 93);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(25, 25);
            this.btnPrevPage.TabIndex = 6;
            this.btnPrevPage.UseVisualStyleBackColor = true;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // btnNext
            // 
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.Location = new System.Drawing.Point(30, 58);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(25, 25);
            this.btnNext.TabIndex = 2;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Image = ((System.Drawing.Image)(resources.GetObject("btnPrev.Image")));
            this.btnPrev.Location = new System.Drawing.Point(30, 8);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(25, 25);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // itbZoom
            // 
            this.itbZoom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.itbZoom.ForeColor = System.Drawing.Color.MidnightBlue;
            this.itbZoom.IntegerValue = ((long)(10));
            this.itbZoom.Location = new System.Drawing.Point(96, 125);
            this.itbZoom.MaxValue = ((long)(30));
            this.itbZoom.MinValue = ((long)(0));
            this.itbZoom.Name = "itbZoom";
            this.itbZoom.NegativeColor = System.Drawing.Color.MidnightBlue;
            this.itbZoom.NegativeInputPendingOnSelectAll = false;
            this.itbZoom.NullString = "0";
            this.itbZoom.OverflowIndicatorToolTipText = null;
            this.itbZoom.PositiveColor = System.Drawing.Color.MidnightBlue;
            this.itbZoom.Size = new System.Drawing.Size(34, 21);
            this.itbZoom.TabIndex = 14;
            this.itbZoom.Tag = "CamSetPos";
            this.itbZoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.itbZoom.Leave += new System.EventHandler(this.itbZoom_Leave);
            this.itbZoom.IntegerValueChanged += new System.EventHandler(this.itbZoom_IntegerValueChanged);
            this.itbZoom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.itbZoom_KeyPress);
            // 
            // alZoomMin
            // 
            this.alZoomMin.DX = 0;
            this.alZoomMin.DY = 0;
            this.alZoomMin.Location = new System.Drawing.Point(10, 152);
            this.alZoomMin.Name = "alZoomMin";
            this.alZoomMin.Size = new System.Drawing.Size(19, 13);
            this.alZoomMin.TabIndex = 15;
            this.alZoomMin.Text = "10";
            // 
            // alZoomMax
            // 
            this.alZoomMax.DX = 0;
            this.alZoomMax.DY = 0;
            this.alZoomMax.Location = new System.Drawing.Point(172, 152);
            this.alZoomMax.Name = "alZoomMax";
            this.alZoomMax.Size = new System.Drawing.Size(25, 13);
            this.alZoomMax.TabIndex = 16;
            this.alZoomMax.Text = "500";
            // 
            // alZoom
            // 
            this.alZoom.DX = 0;
            this.alZoom.DY = 0;
            this.alZoom.Location = new System.Drawing.Point(58, 130);
            this.alZoom.Name = "alZoom";
            this.alZoom.Size = new System.Drawing.Size(34, 13);
            this.alZoom.TabIndex = 17;
            this.alZoom.Text = "Zoom";
            // 
            // btnLeft
            // 
            this.btnLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnLeft.Image")));
            this.btnLeft.Location = new System.Drawing.Point(5, 33);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(25, 25);
            this.btnLeft.TabIndex = 18;
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnRight
            // 
            this.btnRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRight.Image")));
            this.btnRight.Location = new System.Drawing.Point(55, 33);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(25, 25);
            this.btnRight.TabIndex = 19;
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // PlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.alZoom);
            this.Controls.Add(this.alZoomMax);
            this.Controls.Add(this.alZoomMin);
            this.Controls.Add(this.itbZoom);
            this.Controls.Add(this.tbeZoom);
            this.Controls.Add(this.btnFirstPage);
            this.Controls.Add(this.btnLastPage);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnPrevPage);
            this.Controls.Add(this.btnGoTo);
            this.Controls.Add(this.tbPageNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lbInfo);
            this.Name = "PlayerControl";
            this.Size = new System.Drawing.Size(222, 190);
            ((System.ComponentModel.ISupportInitialize)(this.itbZoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPageNumber;
        private System.Windows.Forms.Button btnGoTo;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnFirstPage;
        private Syncfusion.Windows.Forms.Tools.TrackBarEx tbeZoom;
        private Syncfusion.Windows.Forms.Tools.IntegerTextBox itbZoom;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alZoomMin;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alZoomMax;
        private Syncfusion.Windows.Forms.Tools.AutoLabel alZoom;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;

    }
}
