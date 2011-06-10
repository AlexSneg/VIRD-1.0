using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UI.PresentationDesign.DesignUI.Forms;

namespace UI.ImportExport.ImportExportUI.Forms
{
    public partial class HowImportForm : PropertyDialog
    {
        private readonly string _message;
        public HowImportForm()
        {
            InitializeComponent();
        }

        public HowImportForm(String message) :this()
        {
            _message = message;
        }

        public bool CreateNew
        {
            get
            {
                return rbNew.Checked;
            }
        }

        private void HowImportForm_Load(object sender, EventArgs e)
        {
            this.lbMessage.Text = _message;
        }
    }
}
