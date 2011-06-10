using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TechnicalServices.Interfaces
{
    public interface IDesignRenderSupport
    {
        void UpdateReference(IServiceProvider provider);
        void Render(Graphics gfx, RectangleF area); 
    }
}
