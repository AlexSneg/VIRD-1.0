using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Reflection;
using UI.PresentationDesign.DesignUI.Controllers;
using Syncfusion.Windows.Forms.Tools;
using UI.PresentationDesign.DesignUI.Helpers;
using UI.PresentationDesign.DesignUI.Properties;
using UI.PresentationDesign.DesignUI.Classes.Controller;

namespace UI.PresentationDesign.DesignUI.Forms
{
    public partial class DisplayGroupProperties : PropertyDialog
    {
        DisplayGroup editGroup;
        DisplayGroup destGroup;

        bool IsReadOnly = false;

        public DisplayGroupProperties(DisplayGroup group, bool isReadOnly)
            :this(group)
        {
            IsReadOnly = isReadOnly;
            okButton.Visible = !isReadOnly;
            
            if (isReadOnly)
                cancelButton.Text = "OK";

            nameText.ReadOnly = isReadOnly;
            commentText.ReadOnly = isReadOnly;
        }

        public DisplayGroupProperties(DisplayGroup group)
        {
            InitializeComponent();

            destGroup = group;

            this.Text = group.Name + " - Свойства";

            CloneGroup(group, editGroup = new DisplayGroup());
            nameText.DataBindings.Add("Text", editGroup, "Name", false, DataSourceUpdateMode.OnPropertyChanged);
            commentText.DataBindings.Add("Text", editGroup, "Comment", false, DataSourceUpdateMode.OnPropertyChanged);
            typeLabel.DataBindings.Add("Text", editGroup, "Type");

            if (editGroup.Width > 0 && editGroup.Height > 0)
                resolutionLabel.Text = String.Format("{0}*{1}", editGroup.Width, editGroup.Height);
            else
                resolutionLabel.Visible = false;
        }

        void CloneGroup(DisplayGroup from, DisplayGroup dest)
        {
            foreach (PropertyInfo info in from.GetType().GetProperties())
            {
                object value = info.GetValue(from, null);
                if (value is ICloneable)
                    value = ((ICloneable)value).Clone();

                if (info.CanWrite)
                    info.SetValue(dest, value, null);
            }
        }


        private void okButton_Click(object sender, EventArgs e)
        {
            if (!IsReadOnly)
            {
                CanClose = AcceptChanges();
                if (CanClose)
                    CloseMe();
             }
            else
                CloseMe();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (!IsReadOnly)
                CancelChanges();
            else
                CloseMe();
        }

        public override bool Changed()
        {
            return nameText.Modified || commentText.Modified;
        }

        public override bool AcceptChanges()
        {
            nameText.Text = nameText.Text.Trim();
            
            ToolTipInfo t_info = new ToolTipInfo();
            t_info.Body.Image = Resources.error;
            t_info.Header.Text = "Ошибка";

            Point p = nameText.Location;
            p.Offset(nameText.Width, 0);


            editGroup.Name = editGroup.Name.Trim();
            if (String.IsNullOrEmpty(nameText.Text.Trim()))
            {
                nameText.Focus();
                //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-347
                MessageBoxExt.Show("Заполните обязательное поле <Название>", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //t_info.Body.Text = "Заполните обязательное поле <Название>";
                //superToolTip1.Show(t_info, PointToScreen(p));
                return false;
            }

            if (PresentationController.Instance.Presentation.DisplayGroupList.Except(new[] { editGroup, destGroup }).Any(d => d.Name == editGroup.Name))
            {
                nameText.Focus();
                t_info.Body.Text = String.Format("Группа с названием {0} уже существует.\r\nВведите другое название", editGroup.Name);
                superToolTip1.Show(t_info, PointToScreen(p));
                return false;
            }


            CloneGroup(editGroup, destGroup);
            DialogResult = DialogResult.OK;
            //CloseMe();

            return true;
        }

        public override void CancelChanges()
        {
            DialogResult = DialogResult.Cancel;
            CloseMe();
        }

        private void nameText_Leave(object sender, EventArgs e)
        {
        }

        private void commentText_Leave(object sender, EventArgs e)
        {

        }
    }
}
