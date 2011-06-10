using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;

namespace Hosts.Plugins.BusinessGraphics.UI
{
    public partial class InputForm : Office2007Form
    {
        public string StyleName
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public InputForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            textBox1.Focus();
            textBox1.SelectAll();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.Text.Trim().Length == 0)
            {
                e.Cancel = true;
                MessageBoxAdv.Show("Имя стиля должно быть заполнено", "Предупреждение", MessageBoxButtons.OK);
            }
        }
    }
}
