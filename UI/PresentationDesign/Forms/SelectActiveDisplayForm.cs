using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace UI.PresentationDesign.DesignUI.Forms
{
    public partial class SelectActiveDisplayForm : Office2007Form
    {
        private List<DisplayType> _activeDisplays;

        public SelectActiveDisplayForm()
        {
            InitializeComponent();
        }

        public SelectActiveDisplayForm(IEnumerable<DisplayType> activeDisplays) : this()
        {
            _activeDisplays = activeDisplays.ToList();
            cbDisplay.DataSource = _activeDisplays;
            cbDisplay.DisplayMember = "Name";
            cbDisplay.ValueMember = "Name";
            cbDisplay.SelectedValue = _activeDisplays[0].Name;
            SelectedDisplay = _activeDisplays[0];
        }

        public DisplayType SelectedDisplay { get; private set; }

        private void cbDisplay_SelectedValueChanged(object sender, EventArgs e)
        {
            SelectedDisplay = (DisplayType)cbDisplay.SelectedItem;
        }
    }
}
