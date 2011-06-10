using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using UI.Administration.AdministrationUI.Controls;
using UI.PresentationDesign.DesignUI.Forms;
using Syncfusion.Windows.Forms;


namespace UI.Administration.AdministrationUI.Forms
{
    public partial class WindowSize : PropertyDialog
    {
        private string Hight;
        private string Width;

        public string SizeParameter
        {
            get
            {
                return Width + "*" + Hight;
            }
        }

        public WindowSize(string parameter)
        {
            InitializeComponent();
            string[] parmeters = parameter.Split('*');
            if (parmeters.Count()==2)
            {
                Width = parmeters[0];
                Hight = parmeters[1];
                
            }
            else
            {
                Width = "800";
                Hight = "600";
            }


            numericUpDownExt1.Text = Width;
            numericUpDownExt2.Text = Hight;
        }

        

        private void numericUpDownExt1_ValueChanged(object sender, EventArgs e)
        {
            Width = ((NumericUpDownExt)sender).Text;
        }
        
        private void numericUpDownExt2_ValueChanged(object sender, EventArgs e)
        {
            Hight = ((NumericUpDownExt)sender).Text;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            AcceptChanges();
        }
        public override bool AcceptChanges()
        {


            if (numericUpDownExt1.Text.Trim() == "" || numericUpDownExt2.Text.Trim() == "")
            {
                string errorMessage = "Поля обязательны для заполнения";
                MessageBoxAdv.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CanClose = false;
                return false;
            }

            DialogResult = DialogResult.OK;
            CanClose = true;
            CloseMe();
            return true;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            CancelChanges();
        }

        public override void CancelChanges()
        {
            DialogResult = DialogResult.Cancel;
            CloseMe();
        }

        
        
    }
}
