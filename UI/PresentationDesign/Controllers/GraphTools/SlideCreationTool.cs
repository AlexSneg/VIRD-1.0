using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Diagram;
using System.Drawing;
using Syncfusion.Windows.Forms.Diagram.Controls;
using System.Windows.Forms;
using UI.PresentationDesign.DesignUI.Controllers;

namespace UI.PresentationDesign.DesignUI.Classes.Controller
{
    public class SlideCreationTool : DeactivateEventTool
    {
        SlideGraphController m_controller;
        Diagram m_diagram;
        public SlideCreationTool(SlideGraphController AController, Diagram ADiagram)
            : base(AController, ToolDescriptor.SlideCreationTool)
        {
            this.SingleActionTool = false;

            m_controller = AController;
            m_diagram = ADiagram;
        }

        public override Tool ProcessMouseUp(System.Windows.Forms.MouseEventArgs evtArgs)
        {
            Tool tool = base.ProcessMouseUp(evtArgs);
            if (evtArgs.Button == MouseButtons.Left)
            {
                Point p = m_controller.ConvertToModelCoordinates(evtArgs.Location);
                m_controller.CreateSlide(p);
            }
            else
            {
                PresentationSelectionTool.GetInstance(Controller).ProcessMouseUp(evtArgs);
            }
            return tool;
        }

    }
}
