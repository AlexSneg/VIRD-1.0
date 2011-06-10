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
    /// Обертка Box для сохранения свойств.
    /// </summary>
    [Serializable]
    public class BoxSettingsWrapper
    {
        public BoxSettingsWrapper() { }
        public BoxSettingsWrapper(Box box) 
        {
            Background = new BackgroundSettingsWrapper(box.Background);
            CornerBottomLeft = box.CornerBottomLeft;
            CornerBottomRight = box.CornerBottomRight;
            CornerSize = box.CornerSize;
            CornerTopLeft = box.CornerTopLeft;
            CornerTopRight = box.CornerTopRight;
            DefaultCorner = box.DefaultCorner;
            HeaderBackground = new BackgroundSettingsWrapper(box.HeaderBackground);
            HeaderLabel = new LabelSettingsWrapper(box.HeaderLabel);
            IconPath = box.IconPath;
            InteriorLine = new LineSettingsWrapper(box.InteriorLine);
            Label = new LabelSettingsWrapper(box.Label);
            Line = new LineSettingsWrapper(box.Line);
            Orientation = box.Orientation;
            Padding = box.Padding;
            Position = (LegendBoxPosition) box.Position;
            Shadow = new ShadowSettingsWrapper(box.Shadow);
            Visible = box.Visible;
        }

        public BackgroundSettingsWrapper Background;
        public BoxCorner CornerBottomLeft;
        public BoxCorner CornerBottomRight;
        public int CornerSize;
        public BoxCorner CornerTopLeft;
        public BoxCorner CornerTopRight;
        public BoxCorner DefaultCorner;
        public BackgroundSettingsWrapper HeaderBackground;
        public LabelSettingsWrapper HeaderLabel;
        public string IconPath;
        public LineSettingsWrapper InteriorLine;
        public LabelSettingsWrapper Label;
        public LineSettingsWrapper Line;
        public Orientation Orientation;
        public int Padding;
        public LegendBoxPosition Position;
        public ShadowSettingsWrapper Shadow;
        public string Template;
        public bool Visible;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(Box box)
        {
            Background.RestoreSettings(box.Background);
            box.CornerBottomLeft = CornerBottomLeft;
            box.CornerBottomRight = CornerBottomRight;
            box.CornerSize = CornerSize;
            box.CornerTopLeft = CornerTopLeft;
            box.CornerTopRight = CornerTopRight;
            box.DefaultCorner = DefaultCorner;
            HeaderBackground.RestoreSettings(box.HeaderBackground);
            HeaderLabel.RestoreSettings(box.HeaderLabel);
            box.IconPath = IconPath;
            InteriorLine.RestoreSettings(box.InteriorLine);
            Label.RestoreSettings(box.Label);
            Line.RestoreSettings(box.Line);
            box.Orientation = Orientation;
            box.Padding = Padding;
            box.Position = Position;
            Shadow.RestoreSettings(box.Shadow);
            Visible = box.Visible;
        }
    }
}
