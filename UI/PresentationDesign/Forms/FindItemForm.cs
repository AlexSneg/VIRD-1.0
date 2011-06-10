using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UI.PresentationDesign.DesignUI.Controllers;
using UI.PresentationDesign.DesignUI.Helpers;

namespace UI.PresentationDesign.DesignUI.Forms
{
    public partial class FindItemForm : Syncfusion.Windows.Forms.Office2007Form
    {
        FindItemController controller;

        public FindItemForm(FindItemController ctrl)
        {
            controller = ctrl;
            InitializeComponent();
            ChangeStateFindButton();
            SetItemToSearch();
        }

        private void SetItemToSearch()
        {
            rbDevice.Checked = controller.FindDevices;
            rbDisplay.Checked = controller.FindDisplays;
            rbSlide.Checked = controller.FindSlides;
            rbLocalSource.Checked = controller.FindLocalSources;
            rbGlobalSource.Checked = controller.FindGlobalSources;
            rbHardSource.Checked = controller.FindHardwareSources;
        }

        private void SetData()
        {
            controller.FindDevices = rbDevice.Checked;
            controller.FindDisplays = rbDisplay.Checked;
            controller.FindSlides = rbSlide.Checked;
            controller.FindLocalSources = rbLocalSource.Checked;
            controller.FindGlobalSources = rbGlobalSource.Checked;
            controller.FindHardwareSources = rbHardSource.Checked;
            controller.FindName = this.textBoxExt1.Text;
            controller.Comment = this.textBoxExt2.Text;
            controller.Author = this.tbAuthor.Text;
        }

        private void buttonAdv1_Click(object sender, EventArgs e)
        {
            SetData();
            if (!controller.Find())
                MessageBoxExt.Show("Элемент с заданными условиями не найден", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonAdv2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbSlide_CheckedChanged(object sender, EventArgs e)
        {
            this.tbAuthor.Enabled = rbSlide.Checked;
            if (!rbSlide.Checked)
                tbAuthor.Text = "";
            controller.ClearState();
        }

        private void FindItemForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    buttonAdv1_Click(null, e);
                    break;
                case Keys.Escape:
                    buttonAdv2_Click(null, e);
                    break;
            }
        }
        private void ChangeStateFindButton()
        {
            if (!string.IsNullOrEmpty(this.textBoxExt1.Text) ||
                !string.IsNullOrEmpty(this.textBoxExt2.Text) ||
                !string.IsNullOrEmpty(this.tbAuthor.Text))
                buttonAdv1.Enabled = true;
            else
                buttonAdv1.Enabled = false;
        }

        private void textBoxExt1_TextChanged(object sender, EventArgs e)
        {
            ChangeStateFindButton();
        }
    }
}
