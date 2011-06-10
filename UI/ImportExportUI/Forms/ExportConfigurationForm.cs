using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using UI.PresentationDesign.DesignUI.Forms;

namespace UI.ImportExport.ImportExportUI.Forms
{
    public partial class ExportConfigurationForm : PropertyDialog
    {
        private readonly string _directory;
        private readonly string _filter;
        private readonly string[] _excludeFiles;
        private string _selectedFile;

        public ExportConfigurationForm()
        {
            InitializeComponent();
        }

        public ExportConfigurationForm(string directory, string filter, params string[] excludeFiles) : this()
        {
            _directory = directory;
            _filter = filter;
            _excludeFiles = excludeFiles;
        }

        public string SelectedFile
        {
            get
            {
                return Path.ChangeExtension(_selectedFile, Path.GetExtension(_filter));
                //return _selectedFile;
            }
        }

        private void ExportConfigurationForm_Load(object sender, EventArgs e)
        {
            tbFolder.Text = Path.GetFileName(_directory);
            // найдем все xml и загрузим их во вьюлист
            string[] configs = Directory.GetFiles(_directory, _filter, SearchOption.TopDirectoryOnly);
            List<string> fileList = new List<string>(configs);
            foreach (string file in configs)
            {
                string fileName = Path.GetFileName(file);
                if (_excludeFiles != null &&
                    _excludeFiles.Any(str => str.Equals(fileName, StringComparison.InvariantCultureIgnoreCase)))
                    continue;
                lvFiles.Items.Add(fileName);
                cbFileName.Items.Add(fileName);
            }
            cbType.SelectedItem = cbType.Items[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _selectedFile = cbFileName.Text;
            if (_selectedFile.Any(ch=>Path.GetInvalidFileNameChars().Contains(ch)))
            {
                MessageBoxAdv.Show("Имя файла содержит недопустимые символы", "Ошибка", MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count == 0) return;
            cbFileName.Text = lvFiles.SelectedItems[0].Text;
        }
    }
}
