using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI.PresentationDesign.DesignUI.Controls
{
    public partial class SlideFilterControl : UserControl
    {
        public event SwitchToNext OnSwitchToNext;
        public event SwitchToNext OnSwitchToPrev;

        #region Properties
        public string SlideName
        {
            get
            {
                return nameTextBox.Text;
            }
        }

        public string Author
        {
            get
            {
                return authorTextBox.Text;
            }
        }

        public string Comment
        {
            get
            {
                return commentTextBox.Text;
            }
        }

        public DateTime FromDate
        {
            get
            {
                return dateTimePicker1.Value.Date;
            }
        }

        public DateTime ToDate
        {
            get
            {
                return dateTimePicker2.Value.Date.AddDays(1).AddSeconds(-1);
            }
        }
        #endregion

        public SlideFilterControl()
        {
            InitializeComponent();

            this.dateTimePicker1.Value = DateTime.Now.AddDays(-7);
            this.dateTimePicker2.Value = DateTime.Now.AddHours(1);

            this.SetStyle(ControlStyles.ResizeRedraw, false);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        internal void Clear()
        {
            this.nameTextBox.Clear();
            this.commentTextBox.Clear();
            this.authorTextBox.Clear();

            this.dateTimePicker1.Value = DateTime.Now.AddDays(-7);
            this.dateTimePicker2.Value = DateTime.Now.AddHours(1);


        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker2.Value < dateTimePicker1.Value)
                dateTimePicker2.Value = dateTimePicker1.Value;

            dateTimePicker2.MinDate = dateTimePicker1.Value;
        }

        private void commentTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //nop
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Tab & commentTextBox.Focused)
            {
                if (OnSwitchToNext != null)
                    OnSwitchToNext();

                return true;
            }
            else
            {
                //if ((keyData & Keys.Shift) == Keys.Shift & (keyData & Keys.Tab) == Keys.Tab)
                if((int)keyData == 65545)
                {
                    if (nameTextBox.Focused)
                    {
                        if (OnSwitchToPrev != null)
                            OnSwitchToPrev();

                        return true;
                    }
                }

            }

            return base.ProcessDialogKey(keyData);
        }

        internal void FocusTextBox()
        {
            nameTextBox.Focus();
        }

        internal void FocusLastControl()
        {
            commentTextBox.Focus();
        }
    }
}
