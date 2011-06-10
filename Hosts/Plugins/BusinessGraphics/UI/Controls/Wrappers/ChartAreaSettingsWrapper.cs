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
    /// Обертка ChartArea для сохранения свойств.
    /// </summary>
    [Serializable]
    public class ChartAreaSettingsWrapper
    {
        public ChartAreaSettingsWrapper() { }
        public ChartAreaSettingsWrapper(ChartArea area)
        {
            Background = new BackgroundSettingsWrapper(area.Background);
            CornerBottomLeft = area.CornerBottomLeft;
            CornerBottomRight = area.CornerBottomRight;
            CornerSize = area.CornerSize;
            CornerTopLeft = area.CornerTopLeft;
            CornerTopRight = area.CornerTopRight;
            DefaultCorner = area.DefaultCorner;
            Label = new LabelSettingsWrapper(area.Label);
            LegendBox = new LegendBoxSettingsWrapper(area.LegendBox);
            Shadow = new ShadowSettingsWrapper(area.Shadow);
            Title = area.Title;
            TitleBox = new BoxSettingsWrapper(area.TitleBox);
            XAxis = new AxisSettingsWrapper(area.XAxis);
            YAxis = new AxisSettingsWrapper(area.YAxis);
        }
        public BackgroundSettingsWrapper Background;
        public BoxCorner CornerBottomLeft;
        public BoxCorner CornerBottomRight;
        public int CornerSize;
        public BoxCorner CornerTopLeft;
        public BoxCorner CornerTopRight;
        public BoxCorner DefaultCorner;
        public LabelSettingsWrapper Label;
        public LegendBoxSettingsWrapper LegendBox;
        public ShadowSettingsWrapper Shadow;
        public string Title;
        public BoxSettingsWrapper TitleBox;
        public AxisSettingsWrapper XAxis;
        public AxisSettingsWrapper YAxis;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(ChartArea area)
        {
            Background.RestoreSettings(area.Background);
            area.CornerBottomLeft = CornerBottomLeft;
            area.CornerBottomRight = CornerBottomRight;
            area.CornerSize = CornerSize;
            area.CornerTopLeft = CornerTopLeft;
            area.CornerTopRight = CornerTopRight;
            area.DefaultCorner = DefaultCorner;
            Label.RestoreSettings(area.Label);
            LegendBox.RestoreSettings(area.LegendBox);
            Shadow.RestoreSettings(area.Shadow);
            area.Title = Title;
            TitleBox.RestoreSettings(area.TitleBox);
            XAxis.RestoreSettings(area.XAxis);
            YAxis.RestoreSettings(area.YAxis);
        }

    }
}
