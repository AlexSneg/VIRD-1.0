using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace UI.PresentationDesign.DesignUI.Controls.Config
{
    public delegate void FileNameChanged(FileNameControl sender);

    public partial class FileNameControl : UserControl
    {
        public event FileNameChanged OnFileNameChanged;

        string _selectedFileName;
        public string SelectedFileName
        {
            get { return _selectedFileName; }
            set { _selectedFileName = value; this.label1.Text = _selectedFileName; }
        }

        public FileNameControl()
        {
            InitializeComponent();
        }

        private void selectPathButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = this.SelectedFileName;
            openFileDialog1.AddExtension = true;
            openFileDialog1.DefaultExt = "jpg";
            openFileDialog1.Filter = "JPG files (*.jpg)|*.jpg";


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.SelectedFileName = openFileDialog1.SafeFileName;
                if (OnFileNameChanged != null)
                    OnFileNameChanged(this);
            }
        }
    }
}
