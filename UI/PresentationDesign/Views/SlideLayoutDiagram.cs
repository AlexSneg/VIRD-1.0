using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI.PresentationDesign.DesignUI.Controls;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using Syncfusion.Windows.Forms.Diagram;
using System.ComponentModel;

namespace UI.PresentationDesign.DesignUI.Classes.View
{
    public class SlideLayoutDiagram : PresentationDiagram
    {
        public SlideLayoutDiagram(System.ComponentModel.IContainer container)
            : base(container)
        {
            ReadOnly = false;
            AllowNodesDrop = true;
            EnableMouseWheelZoom = false;

        }

        public override Syncfusion.Windows.Forms.Diagram.View CreateView()
        {
            return new SlideLayoutVeiw();
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (!ReadOnly)
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Delete)
                {
                    e.Handled = true;
                    LayoutController c = this.Controller as LayoutController;
                    if (c != null)
                        c.RemoveWindow();
                }
            }

            if(!e.Handled)
                base.OnKeyDown(e);
        }


        public override Syncfusion.Windows.Forms.Diagram.DiagramController CreateController()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                LayoutController result = new LayoutController();
                Tool tool = result.GetTool(ToolDescriptor.SelectTool);
                result.UnRegisterTool(tool);
                result.RegisterTool(new PresentationSelectionTool(result) { AllowMultiSelect = false });
                return result;
            }
            return base.CreateController();
        }

    }
}
