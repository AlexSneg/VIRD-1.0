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
    public delegate void SwitchToNext();

    public partial class PresentationFilterControl : UserControl
    {
        public event SwitchToNext OnSwitchToNext;
        public event SwitchToNext OnSwitchToPrev;

        #region
        public string PresentationName
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

        public int FromSlideCount
        {
            get
            {
                return (int)numericUpDownExt1.Value;
            }
        }

        public int ToSlideCount
        {
            get
            {
                return (int)numericUpDownExt2.Value;
            }
        }
        #endregion

        public PresentationFilterControl()
        {
            InitializeComponent();

            this.dateTimePicker1.Value = DateTime.Now.AddDays(-7);
            this.dateTimePicker2.Value = DateTime.Now.AddHours(1);

            this.numericUpDownExt2.Value = 1000;

            this.SetStyle(ControlStyles.ResizeRedraw, false);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        internal void Clear()
        {
            this.nameTextBox.Clear();
            this.commentTextBox.Clear();
            this.authorTextBox.Clear();
            this.numericUpDownExt1.Value = this.numericUpDownExt1.Minimum;
            this.dateTimePicker1.Value = DateTime.Now.AddDays(-7);
            this.dateTimePicker2.Value = DateTime.Now.AddHours(1);

            this.numericUpDownExt2.Value = 1000;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker2.Value < dateTimePicker1.Value)
                dateTimePicker2.Value = dateTimePicker1.Value;

            dateTimePicker2.MinDate = dateTimePicker1.Value;
        }

        private void numericUpDownExt1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownExt2.Value < numericUpDownExt1.Value)
                numericUpDownExt2.Value = numericUpDownExt1.Value;

            numericUpDownExt2.Minimum = numericUpDownExt1.Value;
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


        internal void FocusLastControl()
        {
            commentTextBox.Focus();
        }

        internal void FocusFirstControl()
        {
            nameTextBox.Focus();
        }
    }
}
