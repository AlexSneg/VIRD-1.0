using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Tools;
using System.Reflection;
using System.Drawing;
using System.Collections;
using System.Drawing.Drawing2D;
using Syncfusion.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace UI.PresentationDesign.DesignUI.Helpers
{
    public class SwitchableGroupView : GroupView
    {
        //public HashSet<GroupViewItem> _markedItems = new HashSet<GroupViewItem>();
        Dictionary<GroupViewItem, bool?> _markedItems = new Dictionary<GroupViewItem, bool?>();

        public Dictionary<GroupViewItem, bool?> MarkedItems
        {
            get { return _markedItems; }
        }
        //public HashSet<GroupViewItem> MarkedItems
        //{
        //    get { return _markedItems; }
        //}

        public HashSet<GroupViewItem> _markedBoldItems = new HashSet<GroupViewItem>();

        public HashSet<GroupViewItem> MarkedBoldItems
        {
            get { return _markedBoldItems; }
        }

        static PropertyInfo _visibleItemsProp = typeof(GroupView).GetProperty("VisibleItems", BindingFlags.Instance | BindingFlags.NonPublic);

        private ArrayList VisibleItems
        {
            get { return (ArrayList)_visibleItemsProp.GetValue(this, null); }
        }

        protected override void DrawText(Graphics gph, int nindex, Rectangle rc, GroupView.ItemState state)
        {
            if (((rc.Width > 0) && (rc.Height > 0)) && ((nindex >= 0) && (nindex < this.VisibleItems.Count)))
            {
                GroupViewItem item = (GroupViewItem)this.VisibleItems[nindex];
                if (nindex != this.nRenameItem)
                {
                    if (this.bTextWrap && this.bSmallImageView)
                    {
                        rc.Y += this.nImageSpacing;
                        rc.Height -= this.nImageSpacing;
                    }
                    Matrix transform = gph.Transform;
                    if (item.Enabled && base.Enabled)
                    {
                        Brush brush;
                        FontStyle style = this._markedBoldItems.Contains(item)
                            ? FontStyle.Bold
                            : ((nindex == this.nHighlightedItem) && this.bTextUnderline) 
                                    ? FontStyle.Underline 
                                    : this.Font.Style;
                        Font font = FontUtil.CreateFont(this.Font, style);
                        if ((nindex == this.nHighlightedItem) && (nindex == this.nSelectedItem))
                        {
                            if (state == ItemState.Selecting)
                            {
                                brush = new SolidBrush(this.clrSelectingText);
                            }
                            else
                            {
                                brush = new SolidBrush(this.clrSelectedHighlightText);
                            }
                        }
                        else if (nindex == this.nSelectedItem)
                        {
                            brush = new SolidBrush(this.clrSelectedText);
                        }
                        else if (nindex == this.nHighlightedItem)
                        {
                            brush = new SolidBrush(this.clrHighlightText);
                        }
                        else
                        {
                            brush = new SolidBrush(this.ForeColor);
                        }
                        bool? isOnline;
                        if (_markedItems.TryGetValue(item, out isOnline))
                        {
                            brush = isOnline.HasValue ? (isOnline.Value ? new SolidBrush(Color.Green) : new SolidBrush(Color.Red)) : brush;
                        }
                        //if (this._markedItems.Contains(item))
                        //{
                        //    brush = new SolidBrush(Color.Red);
                        //}
                        StringFormat format = new StringFormat(StringFormatFlags.LineLimit);
                        if (this.ctrlToolTip.Visible && (nindex == this.nHighlightedItem))
                        {
                            format.Alignment = StringAlignment.Near;
                            format.FormatFlags |= StringFormatFlags.NoWrap;
                            format.Trimming = StringTrimming.None;
                            format.LineAlignment = StringAlignment.Center;
                        }
                        else
                        {
                            if (this.bSmallImageView)
                            {
                                format.Alignment = StringAlignment.Near;
                                gph.MeasureString(item.Text, font);
                            }
                            else
                            {
                                format.Alignment = StringAlignment.Center;
                            }
                            if (!this.bTextWrap)
                            {
                                format.Trimming = StringTrimming.EllipsisCharacter;
                                format.FormatFlags |= StringFormatFlags.NoWrap;
                                format.LineAlignment = StringAlignment.Center;
                            }
                            else
                            {
                                format.Trimming = StringTrimming.EllipsisWord;
                            }
                        }
                        if (this.GetIsMirrored())
                        {
                            format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                        }
                        gph.TextRenderingHint = TextRenderingHint.SystemDefault;
                        gph.DrawString(item.Text, font, brush, rc, format);
                        font.Dispose();
                        format.Dispose();
                    }
                    else
                    {
                        StringFormat format2 = new StringFormat(StringFormatFlags.LineLimit);
                        format2.Trimming = StringTrimming.EllipsisWord;
                        if (this.bSmallImageView)
                        {
                            format2.Alignment = StringAlignment.Near;
                        }
                        else
                        {
                            format2.Alignment = StringAlignment.Center;
                        }
                        if (!this.bTextWrap)
                        {
                            format2.FormatFlags |= StringFormatFlags.NoWrap;
                            format2.LineAlignment = StringAlignment.Center;
                        }
                        if (this.GetIsMirrored())
                        {
                            format2.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                        }
                        ControlPaint.DrawStringDisabled(gph, item.Text, this.Font, SystemColors.Control, rc, format2);
                        format2.Dispose();
                    }
                    gph.Transform = transform;
                }
            }

        }
    }

}
