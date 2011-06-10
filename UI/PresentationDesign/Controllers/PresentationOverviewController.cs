using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Diagram;

namespace UI.PresentationDesign.DesignUI.Classes.Controller
{
    /// <summary>
    /// Simple controller with Zoom and Pan tool
    /// </summary>
    public class PresentationOverviewController: PresentationDiagramControllerBase
    {
        public PresentationOverviewController()
        {
        }

        public void InitController()
        {
            Tool panTool = this.GetTool(ToolDescriptor.PanTool);
            Tool zoomTool = this.GetTool(ToolDescriptor.ZoomTool);
            new List<Tool>(this.GetAllTools().Except(new Tool[] { panTool, zoomTool })).ForEach(tool => this.UnRegisterTool(tool));
            this.ActivateTool(panTool);
            this.View.Magnification = 50F;
        }

        public static PresentationOverviewController CreatePresentationController()
        {
            return new PresentationOverviewController();
        }
    }
}
