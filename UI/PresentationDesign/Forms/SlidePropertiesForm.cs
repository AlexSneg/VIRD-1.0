using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.PresentationDesign.DesignUI.Classes.View;
using UI.PresentationDesign.DesignUI.Classes.Model;
using UI.PresentationDesign.DesignUI.Controllers;
using UI.PresentationDesign.DesignUI.Helpers;
using Domain.PresentationDesign.Client;
using UI.PresentationDesign.DesignUI.Classes.Controller;


namespace UI.PresentationDesign.DesignUI.Forms
{
    public partial class SlidePropertiesForm : PropertyDialog
    {
        SlideGraphController m_controller;
        SlideView CurrentSlideView;
        
        Slide Slide;
        SlideLink NewDefLink, OldDefLink;

        List<TechnicalServices.Persistence.SystemPersistence.Configuration.Label> Labels;
        bool _modified = false;

        public SlidePropertiesForm(SlideGraphController AController)
        {
            InitializeComponent();
            m_controller = AController;
            CurrentSlideView = m_controller.SelectedSlideView;

            m_controller.OnLabelListChanhed += m_controller_OnLabelListChanhed;

            bool slideLocked = (CurrentSlideView.IsLocked && PresentationController.Instance.CanUnlockSlide(CurrentSlideView.Slide)) || DesignerClient.Instance.IsStandAlone;
            bool presentationLocked = PresentationController.Instance.PresentationLocked || DesignerClient.Instance.IsStandAlone;

            this.Text = CurrentSlideView.SlideName + " - Свойства";

            Slide = CurrentSlideView.Slide.Copy();
            Slide.SaveSlideLevelChanges(CurrentSlideView.Slide);

            if (CurrentSlideView.GetOutgoingLinks().Count > 0)
            {
                OldDefLink = NewDefLink = CurrentSlideView.GetOutgoingLinks().Where(l => l.IsDefault).First();
            }

            #region Add bindings
            nameText.DataBindings.Add("Text", Slide, "Name");
            nameText.ReadOnly = !slideLocked;
            labelsList.Enabled = slideLocked;
            labelsList.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            RefreshLabels();

            hourSpanEdit.DataBindings.Add("Value", Slide.Time, "Hours");
            hourSpanEdit.Enabled = slideLocked;
            minuteSpanEdit.DataBindings.Add("Value", Slide.Time, "Minutes");
            minuteSpanEdit.Enabled = slideLocked;
            secondSpanEdit.DataBindings.Add("Value", Slide.Time, "Seconds");
            secondSpanEdit.Enabled = slideLocked;
            authorText.DataBindings.Add("Text", Slide, "Author");
            modifiedLabel.DataBindings.Add("Text", Slide, "Modified");
            commentText.DataBindings.Add("Text", Slide, "Comment");
            commentText.ReadOnly = !slideLocked;
            #endregion

            if (nextSlideList.Enabled = NewDefLink != null && presentationLocked)
            {
                List<SlideView> list = CurrentSlideView.GetOutgoingSlideViews();
                list.ForEach(s => nextSlideList.Items.Add(s));
                nextSlideList.SelectedIndex = list.IndexOf(NewDefLink.ToSlideView);
            }

            bool flag = m_controller.StartSlide == CurrentSlideView;
            isStartupCheckBox.Checked = flag;

            bool flag2 = !flag & CurrentSlideView.GetIncomingSlideLinks().Count == 0;

            isStartupCheckBox.Enabled = flag2 & presentationLocked;
            
            bool visible =  presentationLocked | slideLocked;

            if (presentationLocked && !slideLocked)
            {
                visible = isStartupCheckBox.Enabled || nextSlideList.Items.Count > 0;
            }

            okButton.Visible = visible;

            if (!visible)
            {
                this.AcceptButton = cancelButton;
                cancelButton.Text = "OK";
            }

        }

        private void m_controller_OnLabelListChanhed()
        {
            RefreshLabels();
        }

        private void RefreshLabels()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(RefreshLabels));

            int curLabId = Slide.LabelId;
            Labels = m_controller.GetAllLabels();
            labelsList.DataBindings.Clear();
            labelsList.DataSource = Labels;
            labelsList.DisplayMember = "Name";
            labelsList.ValueMember = "Id";
            if ((curLabId != -1) && (Labels.Count(lb => lb.Id.Equals(curLabId)) == 0))
            {
                curLabId = -1;
                MessageBoxExt.Show("Администратор удалил выбранную вами метку", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            labelsList.SelectedValue = curLabId;
            Slide.LabelId = curLabId;
            labelsList.DataBindings.Add("SelectedValue", Slide, "LabelId");
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            CanClose = AcceptChanges();
            if (CanClose)
                CloseMe();
        }

        public override bool Changed()
        {
            return _modified || nameText.Modified || commentText.Modified || authorText.Modified;
        }

        public override bool AcceptChanges()
        {
            //this.CanClose = true;

            if (okButton.Visible)
            {
                if (String.IsNullOrEmpty(nameText.Text.Trim()))
                {
                    MessageBoxExt.Show("Заполните обязательное поле <Название>", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                if (labelsList.SelectedValue != null && !m_controller.IsLabelUnique((int)labelsList.SelectedValue, CurrentSlideView))
                {
                    MessageBoxExt.Show(String.Format("Метка {0} уже присвоена другой сцене. Выберите другую метку!", Labels[labelsList.SelectedIndex].Name), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                nameText.Text = nameText.Text.Trim();
                Slide.Name = Slide.Name.Trim();
                if (m_controller.IsSlideUniqueName(nameText.Text, CurrentSlideView.SlideName))
                {
                    Slide.Time = new TimeSpan((int)hourSpanEdit.Value, (int)minuteSpanEdit.Value, (int)secondSpanEdit.Value);
                    m_controller.ChangeSlideData(CurrentSlideView, Slide, OldDefLink == NewDefLink ? null : NewDefLink, isStartupCheckBox.Checked);
                }
                else
                {
                    MessageBoxExt.Show(String.Format("{0} уже имеется в сценарии. Введите другое название!", nameText.Text), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //this.CanClose = false;
                    return false;
                }

            }

            this.DialogResult = DialogResult.OK;
            return true;
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            CancelChanges();
        }

        public override void CancelChanges()
        {
            this.DialogResult = DialogResult.Cancel;
            CanClose = true;
            CloseMe();
        }

        private void nextSlideList_SelectedValueChanged(object sender, EventArgs e)
        {
            SlideView nextSlide = ((SlideView)nextSlideList.SelectedItem);
            NewDefLink = CurrentSlideView.GetOutgoingLinks().Where(l => nextSlide == l.ToSlideView).First();
            _modified = true;
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            _modified = true;
        }

        private void SlidePropertiesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_controller.OnLabelListChanhed -= m_controller_OnLabelListChanhed;
        }

    }
}
