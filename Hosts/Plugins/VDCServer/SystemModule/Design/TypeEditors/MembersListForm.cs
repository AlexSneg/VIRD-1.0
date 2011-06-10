using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hosts.Plugins.VDCServer.SystemModule.Design.TypeEditors
{
    public partial class MembersListForm : Form
    {
        private List<VDCServerAbonentInfo> abonents;
        private List<VDCServerAbonentInfo> selected;

        public MembersListForm(List<VDCServerAbonentInfo> list, List<VDCServerAbonentInfo> selectedList, bool readOnly)
        {
            abonents = list;
            selected = selectedList;
            InitializeComponent();

            if (readOnly)
            {
                abonentsListBox.SelectionMode = SelectionMode.None;
                saveButton.Enabled = false;
                AcceptButton = null;
            }
        }

        public List<VDCServerAbonentInfo> values
        {
            get
            {
                List<VDCServerAbonentInfo> values = new List<VDCServerAbonentInfo>();
                foreach (object item in abonentsListBox.CheckedItems)
                {
                    values.Add((VDCServerAbonentInfo) item);
                }
                return values;
            }
        }

        private void MembersListForm_Load(object sender, EventArgs e)
        {
            foreach (VDCServerAbonentInfo abonent in abonents)
            {
                bool isChecked = selected.Contains(abonent);
                abonentsListBox.Items.Add(abonent, isChecked);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}