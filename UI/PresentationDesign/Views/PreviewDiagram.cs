using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI.PresentationDesign.DesignUI.Classes.View;
using System.ComponentModel;
using Syncfusion.Windows.Forms.Tools.Win32API;

namespace UI.PresentationDesign.DesignUI.Views
{
    public class PreviewDiagram : Syncfusion.Windows.Forms.Diagram.Controls.Diagram
    {
        public PreviewDiagram(IContainer container)
            : base(container)
        {
        }

        public override Syncfusion.Windows.Forms.Diagram.View CreateView()
        {
            return new DiagramViewBase { NeedDrawHandles = true };
        }
        bool CtrlHold
        {
            get
            {
                return (WindowsAPI.GetAsyncKeyState(VirtualKeys.VK_CONTROL) & 0x8000) != 0;
            }
        }
        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            if (CtrlHold)
            {
                float scale = this.View.Magnification;
                float delta = 3f * Math.Sign(e.Delta);//e.Delta / 10f;
                if (scale + delta >= 0 && scale + delta <= 500)
                    this.View.Magnification = scale + delta;
            }
            else
                base.OnMouseWheel(e);
        }
    }
}
