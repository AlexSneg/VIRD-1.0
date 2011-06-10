using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Diagram;
using UI.PresentationDesign.DesignUI.Classes.View;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Controls.Utils;

namespace UI.PresentationDesign.DesignUI.Controls
{
    public partial class PresentationOverviewControl : UserControl, IControllerAssignable
    {
        public Syncfusion.Windows.Forms.Diagram.Model Model
        {
            get { return model; }
            set
            {
                InitModel(value);
            }
        }

        Tool zoomTool;
        Tool panTool;

        public PresentationOverviewControl()
        {
            InitializeComponent();
            this.diagram.ReadOnly = true;
            this.diagram.AllowNodesDrop = false;
            this.diagram.EnableMouseWheelZoom = true;
            (this.diagram.View as PresentationView).NeedDrawHandles = false;

            this.zoomCombo.Items.AddRange(ZoomComboHelper.CreateList());
            this.zoomCombo.SelectedIndex = ZoomComboHelper.GetIndexForPercentage(50);
            this.zoomCombo.SelectedIndexChanged += new EventHandler(zoomCombo_SelectedIndexChanged);
        }

        void zoomCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.diagram.View.Magnification = (zoomCombo.Items[zoomCombo.SelectedIndex] as ZoomComboHelper).Value;
        }

        public void InitModel(Syncfusion.Windows.Forms.Diagram.Model AModel)
        {
            this.diagram.Model = AModel;
            this.model = AModel;
        }

        public void InitController()
        {
            (this.diagram.Controller as PresentationOverviewController).InitController();
            zoomTool = this.diagram.Controller.GetTool(ToolDescriptor.ZoomTool);
            panTool = this.diagram.Controller.GetTool(ToolDescriptor.PanTool);
        }

        #region IControllerAssignable Members

        public void AssignController(PresentationDiagramControllerBase controller)
        {
            PresentationOverviewController c = controller as PresentationOverviewController;
            if(c == null)
                throw new ApplicationException("Controller is not a " + typeof(PresentationOverviewController).FullName);
            
            this.diagram.Controller = c;

            c.ToolActivated += new EventHandler<ToolEventArgs>(controller_ToolActivated);
            c.ToolDeactivated += new EventHandler<ToolEventArgs>(controller_ToolDeactivated);

            InitController();
        }

        #endregion

        void controller_ToolActivated(object sender, ToolEventArgs e)
        {
            this.zoomToolButton.Checked = (e.Tool is ZoomTool);
            this.panToolButton.Checked = (e.Tool is PanTool);
        }

        void controller_ToolDeactivated(object sender, ToolEventArgs e)
        {
            //nop
        }

        private void zoomToolButton_Click(object sender, EventArgs e)
        {
            SwitchTools(zoomTool, panTool);
        }

        private void panToolButton_Click(object sender, EventArgs e)
        {
            SwitchTools(panTool, zoomTool);
        }

        void SwitchTools(Tool tool1, Tool tool2)
        {
            if (this.diagram.Controller.ActiveTool == tool1)
                this.diagram.Controller.ActivateTool(tool2);
            else
                this.diagram.Controller.ActivateTool(tool1);
        }

        public void ConnectView(PresentationView view)
        {
            (this.diagram.View as PresentationView).ConnectToView(view);
        }
    }
}
