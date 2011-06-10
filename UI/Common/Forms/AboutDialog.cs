using System;
using System.Windows.Forms;

using Syncfusion.Windows.Forms;

namespace UI.Common.CommonUI.Forms
{
    public partial class AboutDialog : Office2007Form
    {
        private AboutDialog()
        {
            InitializeComponent();
        }

        public static void Execute(IWin32Window owner)
        {
            using (AboutDialog dlg = new AboutDialog())
            {
                string buildVersion = dlg.GetType().Assembly.FullName;
                string[] items = buildVersion.Split(new[] {'=', ','}, StringSplitOptions.RemoveEmptyEntries);
                dlg.labelBuild.Text = "(" + items[2] + ")";
                dlg.ShowDialog(owner);
            }
        }
    }
}