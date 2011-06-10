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
    /// Обертка Shadow для сохранения свойств.
    /// </summary>
    [Serializable]
    public class ShadowSettingsWrapper
    {
        public ShadowSettingsWrapper() { }
        public ShadowSettingsWrapper(Shadow shadow)
        {
            Color = new ColorSettingsWrapper(shadow.Color);
            Depth = shadow.Depth;
            Soft = shadow.Soft;
            Visible = shadow.Visible;
        }

        public ColorSettingsWrapper Color;
        public int Depth;
        public bool Soft;
        public bool Visible;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(Shadow shadow)
        {
            shadow.Color = Color.RestoreSettings();
            shadow.Depth = Depth;
            shadow.Soft = Soft;
            shadow.Visible = Visible;
        }

    }
}
