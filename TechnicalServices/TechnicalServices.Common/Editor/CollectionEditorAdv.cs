using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using TechnicalServices.Entity;
using TechnicalServices.Persistence.CommonPersistence.Presentation;

namespace TechnicalServices.Common.Editor
{
    public abstract class CollectionEditorAdv<T> : CollectionEditor
    {
        private bool _isCancel;
        private PropertyGrid propertyGrid;

        public EventHandler RemoveItem;

        protected CollectionEditorAdv(Type type)
            : base(type)
        {
        }

        protected abstract List<T> CreateCopy(ITypeDescriptorContext context, List<T> list);
        //protected abstract void RestoreCopy(ITypeDescriptorContext context, List<T> list);

        protected override void CancelChanges()
        {
            base.CancelChanges();
            _isCancel = true;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _isCancel = false;

            List<T> storage = CreateCopy(context, (List<T>) value);
            object result = base.EditValue(context, provider, value);
            if (_isCancel)
            {
                ((List<T>) result).Clear();
                ((List<T>) result).AddRange(storage);
                //RestoreCopy(context, storage);
            }
            else
                result = CreateCopy(context, (List<T>) result);

            storage.Clear();
            _isCancel = false;
            return result;
        }

        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm form = base.CreateCollectionForm();
            form.HelpButton = false;
            //form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            form.ControlBox = false;
            
            Control[] cList = form.Controls.Find("listbox", true);
            ((ListBox) cList[0]).DrawItem += listbox_DrawItem;

            if (Context.PropertyDescriptor.Attributes[typeof (CollectionFormNameAttribute)] != null)
            {
                form.Text =
                    ((CollectionFormNameAttribute)
                     Context.PropertyDescriptor.Attributes[typeof (CollectionFormNameAttribute)]).Name;
            }

            if (Context.PropertyDescriptor.Attributes[typeof (CollectionNameAttribute)] != null)
            {
                cList = form.Controls.Find("membersLabel", true);
                cList[0].Text =
                    ((CollectionNameAttribute) Context.PropertyDescriptor.Attributes[typeof (CollectionNameAttribute)]).
                        Name;
            }

            if (form.Controls.Find("propertyBrowser", true).Length > 0)
            {
                propertyGrid = (PropertyGrid) form.Controls.Find("propertyBrowser", true)[0];
                propertyGrid.Validating += new CancelEventHandler(propertyGrid_Validating);
            }
            if (form.Controls.Find("addButton", true).Length > 0)
            {
                ((Button) form.Controls.Find("addButton", true)[0]).Click += new EventHandler(addButton_Click);
            }

            if (form.Controls.Find("removeButton", true).Length > 0)
            {
                ((Button)form.Controls.Find("removeButton", true)[0]).Click += new EventHandler(removeButton_Click);
            }

            form.KeyPreview = true;
            form.KeyDown += new KeyEventHandler(form_KeyDown);
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            return form;
        }

        private bool Tag = false;
        void form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4) Tag = true;
        }
        
        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Tag)
            {
                Tag = false;
                e.Cancel = true;
            }
        }

        //void form_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    if (e.CloseReason == CloseReason.UserClosing && propertyGrid != null)
        //    {
        //        propertyGrid_Validating(propertyGrid, e);
        //    }
        //}

        private void addButton_Click(object sender, EventArgs e)
        {
            propertyGrid.Select();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            T data = (T) propertyGrid.SelectedObject;
            if (data != null) OnRemoveItem((T)propertyGrid.SelectedObject);
        }

        protected virtual void OnRemoveItem(T sender)
        {

        }

        private void propertyGrid_Validating(object sender, CancelEventArgs e)
        {
            object item = ((PropertyGrid) sender).SelectedObject;
            string message;
            if (item is ICollectionItemValidation)
            {
                bool correct = ((ICollectionItemValidation) item).ValidateItem(out message);
                if (!correct)
                {
                    MessageBox.Show(message);
                    e.Cancel = true;
                }
            }

            ICollectionItemValidation instance = Context.Instance as ICollectionItemValidation;
            if (instance != null && !instance.ValidateItem(out message))
            {
                MessageBox.Show(message);
                e.Cancel = true;
            }
        }

        #region Отрисовка элемента коллекции

        private readonly double LOG10 = Math.Log(10);

        private void listbox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1)
            {
                Graphics graphics = e.Graphics;
                ListBox listBox = ((ListBox) sender);
                int count = listBox.Items.Count;
                int num2 = (count > 1) ? (count - 1) : count;
                SizeF ef = graphics.MeasureString(num2.ToString(CultureInfo.CurrentCulture), listBox.Font);
                int num3 = ((int) (Math.Log(num2)/LOG10)) + 1;
                int num4 = 4 + (num3*(e.Font.Height/2));
                num4 = Math.Max(num4, (int) Math.Ceiling(ef.Width)) + (SystemInformation.BorderSize.Width*4);
                Rectangle rectangle = new Rectangle(e.Bounds.X, e.Bounds.Y, num4, e.Bounds.Height);
                ControlPaint.DrawButton(graphics, rectangle, ButtonState.Normal);
                rectangle.Inflate(-SystemInformation.BorderSize.Width*2, -SystemInformation.BorderSize.Height*2);
                int num5 = num4;
                Color window = SystemColors.Window;
                Color windowText = SystemColors.WindowText;
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    window = SystemColors.Highlight;
                    windowText = SystemColors.HighlightText;
                }
                Rectangle rect = new Rectangle(e.Bounds.X + num5, e.Bounds.Y, e.Bounds.Width - num5, e.Bounds.Height);
                graphics.FillRectangle(new SolidBrush(window), rect);
                if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                {
                    ControlPaint.DrawFocusRectangle(graphics, rect);
                }
                num5 += 2;
                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    graphics.DrawString((e.Index + 1).ToString(CultureInfo.CurrentCulture), e.Font,
                                        SystemBrushes.ControlText,
                                        new Rectangle(e.Bounds.X, e.Bounds.Y, num4, e.Bounds.Height), format);
                }
                Brush brush = new SolidBrush(windowText);
                string displayText = listBox.Items[e.Index].ToString();
                try
                {
                    graphics.DrawString(displayText, e.Font, brush,
                                        new Rectangle(e.Bounds.X + num5, e.Bounds.Y, e.Bounds.Width - num5,
                                                      e.Bounds.Height));
                }
                finally
                {
                    if (brush != null)
                    {
                        brush.Dispose();
                    }
                }
                int num6 = num5 + ((int) graphics.MeasureString(displayText, e.Font).Width);
                if ((num6 > e.Bounds.Width) && (listBox.HorizontalExtent < num6))
                {
                    listBox.HorizontalExtent = num6;
                }
            }
        }

        #endregion
    }
}