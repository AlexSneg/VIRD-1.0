using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Threading;
using TechnicalServices.Entity;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Properties;
using UI.PresentationDesign.DesignUI.Helpers;
using Domain.PresentationDesign.Client;

namespace UI.PresentationDesign.DesignUI.Forms
{
    public partial class PresentationPropertiesForm : PropertyDialog
    {
        PresentationInfo DestInfo;
        bool _isCreateNew = false;
        PresentationInfo info;
        bool canClose = true;

        public PresentationPropertiesForm(PresentationInfo AInfo, bool creatingNew)
        {
            InitializeComponent();
            _isCreateNew = creatingNew;
            DestInfo = new PresentationInfo(AInfo);
            info = AInfo;

            nameText.Text = DestInfo.Name;
            authorText.Text = DestInfo.Author;
            commentText.Text = DestInfo.Comment;
            //nameText.DataBindings.Add("Text", DestInfo, "Name");
            //authorText.DataBindings.Add("Text", DestInfo, "Author");
            createdLabel.DataBindings.Add("Text", DestInfo, "CreationDate");
            modifiedLabel.DataBindings.Add("Text", DestInfo, "LastChangeDate");
            //commentText.DataBindings.Add("Text", DestInfo, "Comment");
            slideCountLabel.DataBindings.Add("Text", DestInfo, "SlideCount");

            if (creatingNew)
            {
                this.Text = "Создание нового сценария";
                this.modifiedLabel.Visible = false;
                this.label8.Visible = false;
                this.slideCountLabel.Visible = false;
                this.label6.Visible = false;
                this.createdLabel.Visible = false;
                this.label4.Visible = false;
                authorText.ReadOnly = false;
            }
            else
            {
                //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1302
                //authorText.ReadOnly = true;
                this.Text = String.Concat(DestInfo.Name, " - Свойства");
            }

        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            bool isReadOnly = false;
            if (PresentationController.Instance != null)
                isReadOnly = !(PresentationController.Instance.PresentationLocked || _isCreateNew);
            else
                isReadOnly = false;

            if (DesignerClient.Instance.IsStandAlone)
                isReadOnly = false;

            okButton.Visible = !isReadOnly;

            if (isReadOnly)
            {
                authorText.ReadOnly = isReadOnly;
                nameText.ReadOnly = isReadOnly;
                commentText.ReadOnly = isReadOnly;
                this.AcceptButton = cancelButton;
                cancelButton.Text = "OK";
            }

        }

        public PresentationPropertiesForm(PresentationInfo info)
            : this(info, false)
        { }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.CanClose = AcceptChanges();
            if (CanClose)
                CloseMe();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            CancelChanges();
        }

        public override bool Changed()
        {
            return nameText.Modified || commentText.Modified || authorText.Modified;
        }

        public override bool AcceptChanges()
        {
            char[] restrict_symbols = { '/', '|', '\\', '<', '>', '"', '?', '.', ':' };

            ToolTipInfo t_info = new ToolTipInfo();
            t_info.Body.Image = Resources.error;
            t_info.Header.Text = "Ошибка";

            Point p = nameText.Location;
            p.Offset(0, nameText.Height);

            if (nameText.Text.ToCharArray().Any(restrict_symbols.Contains))
            {
                nameText.Focus();
                t_info.Body.Text = "Некорректно введенные данные";
                superToolTip1.Show(t_info, PointToScreen(p));
                return false;
            }

            info.Name = info.Name.Trim();
            if (String.IsNullOrEmpty(nameText.Text.Trim()))
            {
                nameText.Focus();
                //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-347
                MessageBoxExt.Show("Заполните обязательное поле <Название>", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //t_info.Body.Text = "Заполните обязательное поле <Название>";
                //superToolTip1.Show(t_info, PointToScreen(p));
                return false;
            }

            nameText.Text = nameText.Text.Trim();

            if (_isCreateNew)
            {
                Mutex presentationMutex = new Mutex(false, "Created::" + nameText.Text);
                if (!presentationMutex.WaitOne(5, true))
                {
                    //already creating
                    MessageBoxExt.Show(String.Format("Сценарий с названием \"{0}{1}\" уже создается на этом компьютере!", nameText.Text.Length > 30 ? nameText.Text.Substring(0, 30) : nameText.Text, nameText.Text.Length > 30 ? "..." : String.Empty), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                presentationMutex.ReleaseMutex();
            }
            if (PresentationListController.Instance.IsUniqueName(nameText.Text, _isCreateNew ? String.Empty : DestInfo.Name))
            {
                //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1305
                // тупо сохраняем, другого метода не нашел
                SaveChanges();
                PresentationListController.Instance.AcceptChangeProperties(info, DestInfo);
                this.DialogResult = DialogResult.OK;
                return true;
            }
            else
            {
                nameText.Focus();
                t_info.Body.Text = String.Format("Сценарий с названием \"{0}{1}\" уже существует! Введите другое название", nameText.Text.Length > 30 ? nameText.Text.Substring(0, 30) : nameText.Text, nameText.Text.Length > 30 ? "..." : String.Empty);
                superToolTip1.Show(t_info, PointToScreen(p));
            }

            nameText.Focus();
            nameText.SelectAll();
            return false;
        }

        private void SaveChanges()
        {
            DestInfo.Name = nameText.Text;
            DestInfo.Comment = commentText.Text;
            DestInfo.Author = authorText.Text;
        }

        public override void CancelChanges()
        {
            this.DialogResult = DialogResult.Cancel;
            CloseMe();
        }

        private void nameText_Validating(object sender, CancelEventArgs e)
        {
            //nop
        }

    }
}
