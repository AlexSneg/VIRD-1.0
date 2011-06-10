using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using TechnicalServices.Entity;
using TechnicalServices.Persistence.CommonPersistence.Presentation;

namespace TechnicalServices.Common
{
    [Obsolete(
        "Работает неправильно, не используйте его!!! Вместо надо использовать потомков от CollectionEditorAdv<T> или ClonableObjectCollectionEditorAdv<T>"
        )]
    public class CorrectNameCollectionEditor : CollectionEditor
    {
        private CollectionForm collectionForm;
        private string collectionName = "&Члены:";
        private ListBox listbox;
        private Button okButton;
        private string propertiesName = "&Cвойства";
        private PropertyGrid propertyGrid;

        public CorrectNameCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override CollectionForm CreateCollectionForm()
        {
            collectionForm = base.CreateCollectionForm();
            string formText = Context.PropertyDescriptor.DisplayName;

            if (Context.PropertyDescriptor.Attributes[typeof (CollectionNameAttribute)] != null)
            {
                collectionName =
                    ((CollectionNameAttribute) Context.PropertyDescriptor.Attributes[typeof (CollectionNameAttribute)]).
                        Name;
                formText = collectionName;
            }
            if (Context.PropertyDescriptor.Attributes[typeof (CollectionFormNameAttribute)] != null)
            {
                formText =
                    ((CollectionFormNameAttribute)
                     Context.PropertyDescriptor.Attributes[typeof (CollectionFormNameAttribute)]).Name;
                ;
            }

            if (Context.PropertyDescriptor.Attributes[typeof (PropertiesNameAttribute)] != null)
                propertiesName =
                    ((PropertiesNameAttribute) Context.PropertyDescriptor.Attributes[typeof (PropertiesNameAttribute)]).
                        Name;

            collectionForm.Text = formText;

            listbox = (ListBox) collectionForm.Controls["overArchingTableLayoutPanel"].Controls["listbox"];
            listbox.DrawItem += new DrawItemEventHandler(listbox_DrawItem);

            if (collectionForm.Controls.Find("propertyBrowser", true).Length > 0)
            {
                propertyGrid = (PropertyGrid) collectionForm.Controls.Find("propertyBrowser", true)[0];
                propertyGrid.Validating += new CancelEventHandler(propertyGrid_Validating);
            }
            if (collectionForm.Controls.Find("addButton", true).Length > 0)
            {
                ((Button) collectionForm.Controls.Find("addButton", true)[0]).Click +=
                    new EventHandler(CorrectNameCollectionEditor_Click);
            }
            if (collectionForm.Controls.Find("okButton", true).Length > 0)
            {
                okButton = ((Button) collectionForm.Controls.Find("okButton", true)[0]);
                okButton.Visible = false;
                Button saveButton = new Button();
                saveButton.Size = okButton.Size;
                saveButton.Location = okButton.Location;
                saveButton.Click += new EventHandler(saveButton_Click);
                saveButton.Text = "Сохранить";
                okButton.Parent.Controls.Add(saveButton);
            }
            LocalizeEditorForm(collectionForm, collectionName, propertiesName);
            return collectionForm;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if ((Context.Instance) is ICollectionItemValidation)
            {
                string message;
                bool res = ((ICollectionItemValidation) Context.Instance).ValidateItem(out message);
                if (!res)
                {
                    MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            okButton.PerformClick();
            ((Form) collectionForm).DialogResult = DialogResult.OK;
        }

        private void CorrectNameCollectionEditor_Click(object sender, EventArgs e)
        {
            propertyGrid.Select();
        }


        private void propertyGrid_Validating(object sender, CancelEventArgs e)
        {
            object item = ((PropertyGrid) sender).SelectedObject;
            if (item is ICollectionItemValidation)
            {
                string message;
                bool correct = ((ICollectionItemValidation) item).ValidateItem(out message);
                if (!correct)
                {
                    MessageBox.Show(message);
                    e.Cancel = true;
                }
            }
            //var listInfo = this.Context.Instance.GetType().GetProperty("AbonentList");
            //if (listInfo != null)
            //{
            //    var list = listInfo.GetValue(this.Context.Instance, null);
            //    if (list != null)
            //    {
            //        foreach (object item in (IList)list)
            //        {
            //            if (item is ICollectionItemValidation)
            //            {
            //                string message;
            //                bool correct = ((ICollectionItemValidation)item).ValidateItem(out message);
            //                if (!correct) MessageBox.Show(message);
            //                e.Cancel = true;

            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Простая локализация формы редактирования свойств.
        /// </summary>
        /// <param name="collectionForm"></param>
        private void LocalizeEditorForm(CollectionForm collectionForm, string collectionName, string propertiesName)
        {
            #region Русификация и изменение надписей на форме

            Label BlaBlaProperties = new Label();
            // перебираем все control-ы на форме и заменяем
            // неправильные надписи
            foreach (Control ctrl in collectionForm.Controls)
            {
                foreach (Control ctrl1 in ctrl.Controls)
                {
                    if (ctrl1.GetType().ToString() == "System.Windows.Forms.Label"
                        && (ctrl1.Text == "&Members:"))
                    {
                        ctrl1.Text = collectionName;
                    }

                    if (ctrl1.GetType().ToString() == "System.Windows.Forms.Label"
                        && (ctrl1.Text.Contains("&Properties")))
                    {
                        BlaBlaProperties = (Label) ctrl1;
                        BlaBlaProperties.TextChanged += new EventHandler(BlaBlaProperties_TextChanged);
                        ctrl1.Text = propertiesName;
                    }

                    if (ctrl1.GetType().ToString() == "System.Windows.Forms.Button"
                        && (ctrl1.Text == "&Members:"))
                    {
                        ctrl1.Text = collectionName == null ? "&Члены:" : collectionName;
                    }

                    if (ctrl1.GetType().ToString() == "System.Windows.Forms.TableLayoutPanel")
                    {
                        foreach (Control ctrl2 in ctrl1.Controls)
                        {
                            if ((ctrl2.Text == "&Add"))
                            {
                                ctrl2.Text = "&Добавить";
                            }
                            if (ctrl2.GetType().ToString() == "System.Windows.Forms.Button"
                                && (ctrl2.Text == "&Remove"))
                            {
                                ctrl2.Text = "&Удалить";
                            }
                            if (ctrl2.GetType().ToString() == "System.Windows.Forms.Button"
                                && (ctrl2.Text == "Cancel"))
                            {
                                ctrl2.Text = "&Отменить";
                            }
                        }
                    }
                    if (ctrl1.GetType().ToString() == "System.Windows.Forms.Design.VsPropertyGrid")
                    {
                        foreach (Control ctrl2 in ctrl1.Controls)
                        {
                            if (ctrl2.GetType().ToString() == "System.Windows.Forms.ToolStrip")
                            {
                                foreach (ToolStripItem item in ((ToolStrip) ctrl2).Items)
                                {
                                    if (item.Text == "Categorized")
                                    {
                                        item.Text = "По категориям";
                                        item.ToolTipText = "По категориям";
                                    }
                                    if (item.Text == "Alphabetical")
                                    {
                                        item.Text = "По алфавиту";
                                        item.ToolTipText = "По алфавиту";
                                    }
                                }
                            }
                        }
                    }

                    #region Заголовок списка свойств

                    if (ctrl1.GetType().ToString()
                        == "System.ComponentModel.Design.CollectionEditor+FilterListBox")
                    {
                        ((ListBox) ctrl1).SelectedIndexChanged +=
                            delegate(object sndr, EventArgs ea) { BlaBlaProperties.Text = propertiesName; };
                    }

                    if (ctrl1.GetType().ToString()
                        == "System.Windows.Forms.Design.VsPropertyGrid")
                    {
                        // и на редактирование в PropertyGrid
                        ((PropertyGrid) ctrl1).SelectedGridItemChanged +=
                            delegate(Object sndr, SelectedGridItemChangedEventArgs segichd) { BlaBlaProperties.Text = propertiesName; };

                        // также сделать доступным окно с подсказками по параметрам 
                        // в нижней части
                        ((PropertyGrid) ctrl1).HelpVisible = true;
                        ((PropertyGrid) ctrl1).HelpBackColor =
                            SystemColors.Info;
                    }

                    #endregion
                }
            }

            #endregion
        }

        private void BlaBlaProperties_TextChanged(object sender, EventArgs e)
        {
            ((Label) sender).Text = propertiesName;
        }

        #region Отрисовка элемента коллекции

        private double LOG10 = Math.Log(10);

        private void listbox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1)
            {
                Graphics graphics = e.Graphics;
                ListBox listBox = ((ListBox) sender);
                int count = listBox.Items.Count;
                int num2 = (count > 1) ? (count - 1) : count;
                SizeF ef = graphics.MeasureString(num2.ToString(CultureInfo.CurrentCulture), listBox.Font);
                int num3 = ((int) (Math.Log((double) num2)/LOG10)) + 1;
                int num4 = 4 + (num3*(e.Font.Height/2));
                num4 = Math.Max(num4, (int) Math.Ceiling((double) ef.Width)) + (SystemInformation.BorderSize.Width*4);
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