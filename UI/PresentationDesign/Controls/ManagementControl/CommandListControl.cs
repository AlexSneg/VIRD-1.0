using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using UI.PresentationDesign.DesignUI.Controllers;

namespace UI.PresentationDesign.DesignUI.Controls
{
    public partial class CommandListControl : UserControl
    {
        CommandListController m_Controller = null;

        public CommandListControl()
        {
            InitializeComponent();
            this.toolStripEx1.Height = this.Height;
            this.toolStripEx1.Visible = false;
        }

        public void AssignController(CommandListController ctrl)
        {
            m_Controller = ctrl;
            m_Controller.OnListChanged += new CommandListChanged(m_Controller_OnListChanged);
            m_Controller_OnListChanged();
        }

        void m_Controller_OnListChanged()
        {
            this.toolStripEx1.Items.Clear();
            foreach (var cmd in m_Controller.Commands)
                this.toolStripEx1.Items.Add(new ToolStripButton(cmd.Key, null, OnCommandClick) { Tag = cmd.Value, TextAlign = ContentAlignment.MiddleLeft });
        }

        private void OnCommandClick(object sender, EventArgs e)
        {
            m_Controller.ExecuteCommand((sender as ToolStripButton).Tag);
        }
    }
}
