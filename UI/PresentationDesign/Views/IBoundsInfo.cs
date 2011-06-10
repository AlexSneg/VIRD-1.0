using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace UI.PresentationDesign.DesignUI.Views
{
    public interface IBoundsInfo
    {
        void SetBoundsInfo(RectangleF rect);
        RectangleF GetBoundsInfo();
        SizeF GetWindowSize();
    }
}
