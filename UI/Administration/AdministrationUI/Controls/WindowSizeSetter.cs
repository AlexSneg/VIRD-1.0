using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UI.Administration.AdministrationUI.Forms;

namespace UI.Administration.AdministrationUI.Controls
{
    public delegate void SizeChanged(WindowSizeSetter sender);

    public partial class WindowSizeSetter : UserControl
    {
        public WindowSizeSetter(string windowSize)
        {
            InitializeComponent();
            this.WindowSize = windowSize;
        }

        public event SizeChanged OnSizeChanged;

        private string _windowSize;

        public string WindowSize
        {
            get
            {
                return _windowSize; 
            }
            set
            {
                _windowSize = value; this.label1.Text = _windowSize; 
            }

        }

        private void btnWindowSize_Click(object sender, EventArgs e)
        {
            WindowSize windowSizeEditor = new WindowSize(WindowSize);
            if (windowSizeEditor.ShowDialog() == DialogResult.OK)
            {
                this.WindowSize = windowSizeEditor.SizeParameter;
                if (OnSizeChanged != null)
                    OnSizeChanged(this);
            }
        }
        


    }
}
