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
    public partial class ExportPresentationForm : PropertyDialog
    {
        private readonly string _directory;
        private readonly string _filter;
        private readonly string[] _presentationFiles;
        private string _selectedFile;

        public ExportPresentationForm()
        {
            InitializeComponent();
        }

        public ExportPresentationForm(string directory, string filter, params string[] presentationFiles)
            : this()
        {
            _directory = directory;
            _filter = filter;
            _presentationFiles = presentationFiles;
            cbFileName.Enabled = presentationFiles.Length == 1;
        }

        public string SelectedFile
        {
            get
            {
                return Path.ChangeExtension(_selectedFile, Path.GetExtension(_filter));
                //return _selectedFile;
            }
        }

        private void ExportPresentationForm_Load(object sender, EventArgs e)
        {
            tbFolder.Text = Path.GetFileName(_directory);
            // найдем все xml и загрузим их во вьюлист
            foreach (string file in _presentationFiles)
            {
                string fileName = Path.GetFileName(file);
                lvFiles.Items.Add(fileName);
                cbFileName.Items.Add(fileName);
            }
            cbFileName.Text = _presentationFiles.Aggregate((prev, next) => prev + "; " + next);
            cbType.SelectedItem = _filter;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _selectedFile = cbFileName.Text.Trim();
            if (_selectedFile.Any(ch => Path.GetInvalidFileNameChars().Contains(ch)))
            {
                MessageBoxAdv.Show("Имя файла содержит недопустимые символы", "Ошибка", MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
