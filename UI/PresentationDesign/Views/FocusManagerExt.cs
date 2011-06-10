using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Diagram;

namespace UI.PresentationDesign.DesignUI.Views
{
    internal class FocusManagerExt: FocusManager
    {
        public override void Draw(System.Drawing.Graphics gfx)
        {
            //don't draw node focus
        }
    }
}
