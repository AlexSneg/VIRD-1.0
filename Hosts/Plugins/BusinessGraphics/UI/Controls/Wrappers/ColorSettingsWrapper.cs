using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotnetCHARTING.WinForms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Hosts.Plugins.BusinessGraphics.UI.Controls.Wrappers
{
    /// <summary>
    /// Обертка Color для сохранения свойств.
    /// </summary>
    [Serializable]
    public class ColorSettingsWrapper
    {
        public ColorSettingsWrapper() { }
        public ColorSettingsWrapper(Color color)
        {
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;
        }
        public int A;
        public int R;
        public int G;
        public int B;

        public Color RestoreSettings()
        {
            return Color.FromArgb(A, R, G, B);
        }
    }
}
