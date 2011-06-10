using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TechnicalServices.PowerPointLib
{
   
    public partial class PowerPointForm : Form
    {
        public string pathToPpt = "";
        public PowerPointForm()
        {
            InitializeComponent();
       
        }

        private void PowerPointForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(pathToPpt)) return;
            powerPointControl1.CreateApplication();
            powerPointControl1.LoadFile(pathToPpt);
        }

        private void PowerPointForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            powerPointControl1.AdjustMenu(true);
            powerPointControl1.CloseApplication();
            
        }
    }
}
