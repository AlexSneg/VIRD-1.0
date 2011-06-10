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
    /// Обертка LegendEntry для сохранения свойств.
    /// </summary>
    [Serializable]
    public class LegendEntrySettingsWrapper
    {
        public LegendEntrySettingsWrapper() { }
        public LegendEntrySettingsWrapper(LegendEntry legendEntry) 
        {
            Background = new BackgroundSettingsWrapper(legendEntry.Background);
            DashStyle = legendEntry.DashStyle;
            DividerLine = new LineSettingsWrapper(legendEntry.DividerLine);
            HeaderMode = legendEntry.HeaderMode;
            Hotspot = new HotspotSettingsWrapper(legendEntry.Hotspot);
            LabelStyle = new LabelSettingsWrapper(legendEntry.LabelStyle);
            Name = legendEntry.Name;
            PaddingTop = legendEntry.PaddingTop;
            SeriesType = legendEntry.SeriesType;
            ShapeType = legendEntry.ShapeType;
            SortOrder = legendEntry.SortOrder;
            ToolTip = legendEntry.ToolTip;
            URL = legendEntry.URL;
            Use3D = legendEntry.Use3D;
            Value = legendEntry.Value;
            Visible = legendEntry.Visible;
        }

        public BackgroundSettingsWrapper Background;
        public DashStyle DashStyle;
        public LineSettingsWrapper DividerLine;
        public LegendEntryHeaderMode HeaderMode;
        public HotspotSettingsWrapper Hotspot;
        public LabelSettingsWrapper LabelStyle;
        public string Name;
        public int PaddingTop;
        public SeriesType SeriesType;
        public ShapeType ShapeType;
        public int SortOrder;
        public string ToolTip;
        public string URL;
        public bool Use3D;
        public string Value;
        public bool Visible;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(LegendEntry legendEntry)
        {
            Background.RestoreSettings(legendEntry.Background);
            legendEntry.DashStyle = DashStyle;
            DividerLine.RestoreSettings(legendEntry.DividerLine);
            legendEntry.HeaderMode = HeaderMode;
            Hotspot.RestoreSettings(legendEntry.Hotspot);
            LabelStyle.RestoreSettings(legendEntry.LabelStyle);
            legendEntry.Name = Name;
            legendEntry.PaddingTop = PaddingTop;
            legendEntry.SeriesType = SeriesType;
            legendEntry.ShapeType = ShapeType;
            legendEntry.SortOrder = SortOrder;
            legendEntry.ToolTip = ToolTip;
            legendEntry.URL = URL;
            legendEntry.Use3D = Use3D;
            legendEntry.Value = Value;
            legendEntry.Visible = Visible;
        }

    }
}
