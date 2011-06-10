using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using UI.PresentationDesign.DesignUI.Forms;

namespace UI.ImportExport.ImportExportUI.Forms
{
    public partial class WrongSlideName : PropertyDialog
    {
        private readonly string _message;
        public WrongSlideName()
        {
            InitializeComponent();
        }

        public WrongSlideName(string message) : this()
        {
            _message = message;
        }

        public string SlideName
        {
            get { return tbName.Text.Trim(); }
        }

        private void WrongSlideName_Load(object sender, EventArgs e)
        {
            lbMessage.Text = _message;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SlideName))
            {
                MessageBoxAdv.Show("Имя сцены не может быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
