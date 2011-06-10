using System.Windows.Forms;

using Syncfusion.Windows.Forms;

namespace UI.PresentationDesign.ConfiguratorUI.Forms
{
    public partial class LabelDlg : Office2007Form
    {
        public LabelDlg()
        {
            InitializeComponent();
        }

        public static bool Execute(Form owner)
        {
            using (LabelDlg dlg = new LabelDlg())
            {
                DialogResult result = dlg.ShowDialog(owner);
                return result == DialogResult.OK;
            }
        }
    }
}