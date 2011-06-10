using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using System.Threading;

namespace UI.ImportExport.ImportExportUI.Forms
{
    public partial class PresentationExistsDialog : Office2007Form
    {
        public PresentationExistsDialog()
        {
            InitializeComponent();
        }


        public static DialogResult Show(string name)
        {

            PresentationExistsDialog d = new PresentationExistsDialog();
            d.label1.Text = name;
            if (d.ShowDialog() == DialogResult.OK)
            {
                if (d.radioButtonAdv2.Checked)
                    return DialogResult.Yes;
                else
                    return DialogResult.No;
            }
            return DialogResult.Cancel;
        }
    }
}
