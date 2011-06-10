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
    /// Обертка AxisTick для сохранения свойств.
    /// </summary>
    [Serializable]
    public class AxisTickSettingsWrapper
    {
        public AxisTickSettingsWrapper() { }
        public AxisTickSettingsWrapper(AxisTick tick) 
        {
            GridLine = new LineSettingsWrapper(tick.GridLine);
            IncludeInAxisScale = tick.IncludeInAxisScale;
            Label = new LabelSettingsWrapper(tick.Label);
            Line = new LineSettingsWrapper(tick.Line);
            OverrideTicks = tick.OverrideTicks;
        }
        public LineSettingsWrapper GridLine;
        public bool IncludeInAxisScale;
        public LabelSettingsWrapper Label;
        public LineSettingsWrapper Line;
        public bool OverrideTicks;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(AxisTick tick)
        {
            GridLine.RestoreSettings(tick.GridLine);
            tick.IncludeInAxisScale = IncludeInAxisScale;
            Label.RestoreSettings(tick.Label);
            Line.RestoreSettings(tick.Line);
            tick.OverrideTicks = OverrideTicks;
        }
    }
}
