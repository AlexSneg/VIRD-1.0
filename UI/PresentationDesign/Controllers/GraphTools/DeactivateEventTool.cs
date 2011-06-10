using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Diagram;

namespace UI.PresentationDesign.DesignUI.Classes.Controller
{
    public delegate void ToolDeactivate();

    public abstract class DeactivateEventTool: Tool
    {
        public DeactivateEventTool(DiagramController controller, String name)
            : base(controller, name)
        {
        }

        public event ToolDeactivate OnToolDeactivate;

        public override void DeactivateTool()
        {
            base.DeactivateTool();

            if (OnToolDeactivate != null)
                OnToolDeactivate();
        }
    }
}
