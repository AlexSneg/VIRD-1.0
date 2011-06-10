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
    /// Обертка Hotspot для сохранения свойств.
    /// </summary>
    [Serializable]
    public class HotspotSettingsWrapper
    {
        public HotspotSettingsWrapper() { }
        public HotspotSettingsWrapper(Hotspot hotspot)
        {
            ToolTip = hotspot.ToolTip;
            URL = hotspot.URL;
        }
        public string ToolTip;
        public string URL;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(Hotspot hotspot)
        {
            hotspot.ToolTip = ToolTip;
            hotspot.URL = URL;
        }

    }
}
