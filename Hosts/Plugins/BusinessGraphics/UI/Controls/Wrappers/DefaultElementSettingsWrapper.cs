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
    /// Обертка DefaultElement для сохранения свойств.
    /// </summary>
    [Serializable]
    public class DefaultElementSettingsWrapper
    {
        public DefaultElementSettingsWrapper() { }
        public DefaultElementSettingsWrapper(Element element)
        {
            ExplodeSlice = element.ExplodeSlice;
            HatchColor = new ColorSettingsWrapper(element.HatchColor);
            HatchStyle = element.HatchStyle;
            ShowValue = element.ShowValue;
            SmartLabel = new SmartLabelSettingsWrapper(element.SmartLabel);
            Transparency = element.Transparency;
        }
        public bool ExplodeSlice;
        public ColorSettingsWrapper HatchColor;
        public HatchStyle HatchStyle;
        public bool ShowValue;
        public SmartLabelSettingsWrapper SmartLabel;
        public int Transparency;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(Element element)
        {
            element.ExplodeSlice = ExplodeSlice;
            element.HatchColor = HatchColor.RestoreSettings();
            if (element.HatchStyle != HatchStyle) element.HatchStyle = HatchStyle;
            element.ShowValue = ShowValue;
            SmartLabel.RestoreSettings(element.SmartLabel);
            element.Transparency = Transparency;
        }

    }
}
