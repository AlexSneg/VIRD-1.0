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
    public delegate void PathChanged(FilePathTextBox sender);

    public partial class FilePathTextBox : UserControl
    {
        public event PathChanged OnPathChanged;

        string _selectedPath;
        public string SelectedPath
        {
            get { return _selectedPath; }
            set { _selectedPath = value; this.label1.Text = _selectedPath; }
        }

        public FilePathTextBox()
        {
            InitializeComponent();
        }

        private void selectPathButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = this.SelectedPath;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.SelectedPath = folderBrowserDialog.SelectedPath;
                if (OnPathChanged != null)
                    OnPathChanged(this);
            }
        }
    }
}
