using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Hosts.Plugins.VDCServer.SystemModule.Design.TypeEditors
{
    public partial class ExportFileForm : Form
    {
        List<VDCServerAbonentInfo> abonents;
        public ExportFileForm(List<VDCServerAbonentInfo> list)
        {
            abonents = list;
            InitializeComponent();
        }

        string fileName;

        private void ExportFileForm_Load(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
            }
        }
        private void ExportFileForm_Shown(object sender, EventArgs e)
        {
            if (fileName != null)
            {
                TextWriter writer = new StreamWriter(fileName);
                foreach (VDCServerAbonentInfo info in abonents)
                {
                    writer.WriteLine
                    (
                    string.Format("'{0}';'{1}';'{2}';'{3}';'{4}';",
                    info.Name, info.Number1, info.Number2, info.ConnectionType, info.ConnectionQuality
                    )
                     );
                }
                writer.Close();
            }
            this.Close();
        }

    }
}
