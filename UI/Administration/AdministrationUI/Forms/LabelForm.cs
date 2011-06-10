using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using TechnicalServices.Entity;
using UI.Administration.AdministrationUI.Controllers;
using UI.PresentationDesign.DesignUI.Forms;
using Label = TechnicalServices.Persistence.SystemPersistence.Configuration.Label;

namespace UI.Administration.AdministrationUI.Forms
{
    public partial class LabelForm : PropertyDialog
    {

       private bool NewLabel;

        private Label _labelEditor;
        public LabelForm()
        {
            InitializeComponent();
            NewLabel = true;
            Text = "Новая метка";
            this.lblType.Text = "Пользовательская";
        }
        public LabelForm(Label labelEditor)
        {
            _labelEditor = labelEditor;
            InitializeComponent();
            InitializeControls();
            NewLabel = false;
            Text = "Метка " + _labelEditor.Name;
        }

        private void InitializeControls()
        {
            this.lblType.Text = _labelEditor.IsSystem ? "Конфигурация" : "Пользовательская";
            txtLabelName.Text = _labelEditor.Name;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            AcceptChanges();
        }

        private Label GetLabelInfo()
        {
            if (!NewLabel)
            {
                return _labelEditor;    
            }
            Label labelInfo = new Label();

            labelInfo.IsSystem = false;
            return labelInfo;
        }

        private void FillLabelInfo(ref Label labelInfo)
        {
            labelInfo.Name = txtLabelName.Text.Trim();
        }

        public override bool AcceptChanges()
        {
            LabelError resultError;
            Label labelInfo;

            labelInfo = GetLabelInfo();

            FillLabelInfo(ref labelInfo);

            if(NewLabel)
            {
                resultError = LabelListController.Instance.CRUD(LabelListController.Instance.AddLabel, labelInfo);
            }
            else
            {
                resultError = LabelListController.Instance.CRUD(LabelListController.Instance.UpdateLabel, labelInfo);
            }

            if ((resultError & LabelError.NoError) != resultError)//TODO вынести получение строки в более общий класс
            {
                string errorMessage = LabelListController.Instance.GetErrorMessage(resultError, labelInfo);
                MessageBoxAdv.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CanClose = false;
                return false;
            }

            LabelListController.Instance.AddedLabelCode = labelInfo.Name;

            DialogResult = DialogResult.OK;
            CanClose = true;
            CloseMe();
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Отмена дефекта 626
            //if (CheckCancelForSave())
                CancelChanges();
        }
        public override void CancelChanges()
        {
            
            DialogResult = DialogResult.Cancel;
            CloseMe();
        }

        public override bool Changed()
        {
            return txtLabelName.Modified;
        }

        private void txtLabelName_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = txtLabelName.Modified;
        }
    }
}
