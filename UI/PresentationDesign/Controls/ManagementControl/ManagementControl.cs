using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using UI.PresentationDesign.DesignUI.Controllers;

namespace UI.PresentationDesign.DesignUI.Controls.ManagementControl
{
    public partial class ManagementControl : UserControl
    {
        private CommandListController m_Controller = null;
        private Control m_ManagementControl = null;

        public ManagementControl()
        {
            InitializeComponent();
        }

        public void AssignController(CommandListController sourceCommandListController)
        {
            m_Controller = sourceCommandListController;
            m_Controller.OnListChanged += new CommandListChanged(m_Controller_OnListChanged);
            this.CommandList.AssignController(sourceCommandListController);
            PlayerController.Instance.OnPresentationStarted += new Action(Instance_OnPresentationStarted);
        }

        void Instance_OnPresentationStarted()
        {
            if (m_ManagementControl != null)
                m_ManagementControl.Enabled = true;
        }

        void m_Controller_OnListChanged()
        {
            if (m_ManagementControl != null)
            {
                this.Controls.Remove(m_ManagementControl);
                m_ManagementControl.Dispose();
            }
            m_ManagementControl = m_Controller.CreateManagementControl(this);
            this.SuspendLayout();
            CommandList.Visible = false;
            //CommandList.Dock = DockStyle.None;
            if (m_ManagementControl != null)
            {
                this.MinimumSize = new Size(m_ManagementControl.MinimumSize.Width, m_ManagementControl.MinimumSize.Height + CommandList.MinimumSize.Height);
                m_ManagementControl.Dock = DockStyle.Fill;
                m_ManagementControl.Enabled = PlayerController.Instance.CanPlay;
            }
            //CommandList.Dock = DockStyle.Fill;
            this.ResumeLayout();
        }
    }
}
