using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms;
using System.Windows.Forms;
using UI.PresentationDesign.DesignUI.Helpers;

namespace UI.PresentationDesign.DesignUI.Forms
{
    public class PropertyDialog : Office2007Form
    {
        public virtual bool AcceptChanges() { return true; }
        public virtual void CancelChanges()
        {
            

        }

        public virtual bool Changed() { return false; }

        public bool CanClose { get; set; }

        protected virtual string QuestionOnExit
        {
            get { return "Сохранить изменения?"; }
        }

        public PropertyDialog()
        {
            CanClose = true;
            this.FormClosing += new FormClosingEventHandler(PropertyDialog_FormClosing);
            this.UseOffice2007SchemeBackColor = true;
            this.ShowInTaskbar = false;
        }

        //Отмена дефекта 626
        //public bool CheckCancelForSave()
        //{
        //    bool canCloseForm = true;
        //    if (Changed())
        //    {
        //        canCloseForm = false;
        //        switch (MessageBoxAdv.Show("Сохранить изменения?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
        //        //switch (MessageBoxExt.Show("Сохранить изменения?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question, new string[] { "Да", "Нет" }))
        //        {
        //            case DialogResult.Yes: canCloseForm = AcceptChanges(); break;
        //            case DialogResult.No: CancelChanges(); break;
        //            //case DialogResult.Cancel: canCloseForm = false;
        //               // CanClose = false; break;
        //        }
        //    }

        //    return canCloseForm;
        //}

        public void CloseMe()
        {
            this.CanClose = true;
            this.FormClosing -= new FormClosingEventHandler(PropertyDialog_FormClosing);
            Close();
        }

        void PropertyDialog_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            //if (e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)

            if (!CanClose)
            {
                e.Cancel = true;
                CanClose = true;
                return;

            }

            if (Changed())
            {
                e.Cancel = false;
                switch (MessageBoxAdv.Show(QuestionOnExit, "Вопрос", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes: e.Cancel = !AcceptChanges(); break;
                    case DialogResult.No: CancelChanges(); break;
                    case DialogResult.Cancel: e.Cancel = true; break;
                }
            }
            else
                CancelChanges();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PropertyDialog
            // 
            this.ClientSize = new System.Drawing.Size(288, 268);
            this.Name = "PropertyDialog";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
    }
}

