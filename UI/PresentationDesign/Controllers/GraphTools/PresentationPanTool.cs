using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Diagram;

namespace UI.PresentationDesign.DesignUI.Classes.Controller
{
    public class PresentationPanTool : PanTool
    {
        public event ToolDeactivate OnToolDeactivate;

        public PresentationPanTool(DiagramController controller)
            : base(controller)
        {
        }

        public override Tool ProcessMouseDown(System.Windows.Forms.MouseEventArgs evtArgs)
        {
            Tool tool = base.ProcessMouseDown(evtArgs);
            if (evtArgs.Button == System.Windows.Forms.MouseButtons.Right)
            {
                PresentationSelectionTool.GetInstance(Controller).ProcessMouseUp(evtArgs);
            }
            return tool;
        }

        public override void DeactivateTool()
        {
            base.DeactivateTool();

            if (OnToolDeactivate != null)
                OnToolDeactivate();
        }
    }
}
