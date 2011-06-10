using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Syncfusion.Windows.Forms;

namespace UI.PresentationDesign.DesignUI.Forms
{
    public partial class DisplayPropsForm : Office2007Form
    {
        public DisplayPropsForm(Display display)
        {
            InitializeComponent();

            if(display.Type.Name!=null)
                this.Text = display.Type.Name + " - Свойства";

            this.propertyGrid.AssignedObject = display;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
