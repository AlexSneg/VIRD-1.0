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
    /// Обертка Line для сохранения свойств.
    /// </summary>
    [Serializable]
    public class LineSettingsWrapper
    {
        public LineSettingsWrapper() { }
        public LineSettingsWrapper(Line line) 
        {
            if(line ==null) return;
            AnchorCapScale = line.AnchorCapScale;
            if(line.Color!=null) Color = new ColorSettingsWrapper(line.Color);
            DashStyle = line.DashStyle;
            EndCap = line.EndCap;
            Length = line.Length;
            StartCap = line.StartCap;
            Width = line.Width;
        }
        public int AnchorCapScale;
        public ColorSettingsWrapper Color;
        public DashStyle DashStyle;
        public LineCap EndCap;
        public int Length;
        public LineCap StartCap;
        public int Width;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(Line line)
        {
            if (line == null) return;
            AnchorCapScale = line.AnchorCapScale;
            if(Color!=null)line.Color = Color.RestoreSettings();
            line.DashStyle = DashStyle;
            line.EndCap = EndCap;
            line.Length = Length;
            line.StartCap = StartCap;
            line.Width = Width;
        }

    }
}
