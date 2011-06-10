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
    /// Обертка LegendBox для сохранения свойств.
    /// </summary>
    [Serializable]
    public class LegendBoxSettingsWrapper
    {
        public LegendBoxSettingsWrapper() { }
        public LegendBoxSettingsWrapper(LegendBox legendBox) 
        {
            Background = new BackgroundSettingsWrapper(legendBox.Background);
            CornerBottomLeft = legendBox.CornerBottomLeft;
            CornerBottomRight = legendBox.CornerBottomRight;
            CornerSize = legendBox.CornerSize;
            CornerTopLeft = legendBox.CornerTopLeft;
            CornerTopRight = legendBox.CornerTopRight;
            DefaultCorner = legendBox.DefaultCorner;
            DefaultEntry = new LegendEntrySettingsWrapper(legendBox.DefaultEntry);
            
            // Скопируем ExtraEntries
            ExtraEntries = new LegendEntrySettingsWrapper[legendBox.ExtraEntries.Count];
            for (int i = 0; i < legendBox.ExtraEntries.Count; i++)
            {
                ExtraEntries[i] = new LegendEntrySettingsWrapper(legendBox.ExtraEntries[i]);
            }

            HeaderBackground = new BackgroundSettingsWrapper(legendBox.HeaderBackground);
            HeaderEntry = new LegendEntrySettingsWrapper(legendBox.HeaderEntry);
            HeaderLabel = new LabelSettingsWrapper(legendBox.HeaderLabel);
            IconPath = legendBox.IconPath;
            InteriorLine = new LineSettingsWrapper(legendBox.InteriorLine);
            LabelStyle = new LabelSettingsWrapper(legendBox.LabelStyle);
            Line = new LineSettingsWrapper(legendBox.Line);
            ListTopToBottom = legendBox.ListTopToBottom;
            Orientation = legendBox.Orientation;
            Padding = legendBox.Padding;
            Position = (LegendBoxPosition) legendBox.Position;
            Shadow = new ShadowSettingsWrapper(legendBox.Shadow);
            Template = legendBox.Template;
            Visible = legendBox.Visible;
        }

        public BackgroundSettingsWrapper Background;
        public BoxCorner CornerBottomLeft;
        public BoxCorner CornerBottomRight;
        public int CornerSize;
        public BoxCorner CornerTopLeft;
        public BoxCorner CornerTopRight;
        public BoxCorner DefaultCorner;
        public LegendEntrySettingsWrapper DefaultEntry;
        public LegendEntrySettingsWrapper[] ExtraEntries;
        public BackgroundSettingsWrapper HeaderBackground;
        public LegendEntrySettingsWrapper HeaderEntry;
        public LabelSettingsWrapper HeaderLabel;
        public string IconPath;
        public LineSettingsWrapper InteriorLine;
        public LabelSettingsWrapper LabelStyle;
        public LineSettingsWrapper Line;
        public bool ListTopToBottom;
        public Orientation Orientation;
        public int Padding;
        public LegendBoxPosition Position;
        public ShadowSettingsWrapper Shadow;
        public string Template;
        public bool Visible;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(LegendBox legendBox)
        {
            Background.RestoreSettings(legendBox.Background);
            legendBox.CornerBottomLeft = CornerBottomLeft;
            legendBox.CornerBottomRight = CornerBottomRight;
            legendBox.CornerSize = CornerSize;
            legendBox.CornerTopLeft = CornerTopLeft;
            legendBox.CornerTopRight = CornerTopRight;
            legendBox.DefaultCorner = DefaultCorner;
            DefaultEntry.RestoreSettings(legendBox.DefaultEntry);

            legendBox.ExtraEntries.Clear();
            for (int i = 0; i < ExtraEntries.Length; i++)
            {
                LegendEntry entry = new LegendEntry();
                ExtraEntries[i].RestoreSettings(entry);
                legendBox.ExtraEntries.Add(entry);
            }

            HeaderBackground.RestoreSettings(legendBox.HeaderBackground);
            HeaderEntry.RestoreSettings(legendBox.HeaderEntry);
            HeaderLabel.RestoreSettings(legendBox.HeaderLabel);
            legendBox.IconPath = IconPath;
            InteriorLine.RestoreSettings(legendBox.InteriorLine);
            LabelStyle.RestoreSettings(legendBox.LabelStyle);
            Line.RestoreSettings(legendBox.Line);
            legendBox.ListTopToBottom = ListTopToBottom;
            legendBox.Orientation = Orientation;
            legendBox.Padding = Padding;
            legendBox.Position = Position;
            Shadow.RestoreSettings(legendBox.Shadow);
            legendBox.Template = Template;
            legendBox.Visible = Visible;
        }

    }
}
