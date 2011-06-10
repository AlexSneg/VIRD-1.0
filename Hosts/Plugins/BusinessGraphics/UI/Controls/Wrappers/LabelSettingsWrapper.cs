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
    /// Обертка Label для сохранения свойств.
    /// </summary>
    [Serializable]
    public class LabelSettingsWrapper
    {
        public LabelSettingsWrapper() { }
        public LabelSettingsWrapper(Label label) 
        {
            Alignment = label.Alignment;
            AutoWrap = label.AutoWrap;
            Color = new ColorSettingsWrapper(label.Color);
            Font = new FontSettingsWrapper(label.Font);
            GlowColor = label.GlowColor;
            Hotspot = new HotspotSettingsWrapper(label.Hotspot);
            LineAlignment = label.LineAlignment;
            OutlineColor = label.OutlineColor;
            Shadow = new ShadowSettingsWrapper(label.Shadow);
            Text = label.Text;
            Truncation = new TruncationSettingsWrapper(label.Truncation);
            Type = label.Type;
        }

        public StringAlignment Alignment;
        public bool AutoWrap;
        public ColorSettingsWrapper Color;
        public FontSettingsWrapper Font;
        public Color GlowColor;
        public HotspotSettingsWrapper Hotspot;
        public StringAlignment LineAlignment;
        public Color OutlineColor;
        public ShadowSettingsWrapper Shadow;
        public string Text;
        public TruncationSettingsWrapper Truncation;
        public LabelType Type;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(Label label)
        {
            label.Alignment = Alignment;
            label.AutoWrap = AutoWrap;
            label.Color = Color.RestoreSettings();
            label.Font = Font.RestoreSettings();
            label.GlowColor = GlowColor;
            Hotspot.RestoreSettings(label.Hotspot);
            label.LineAlignment = LineAlignment;
            label.OutlineColor = OutlineColor;
            Shadow.RestoreSettings(label.Shadow);
            label.Text = Text;
            Truncation.RestoreSettings(label.Truncation);
            label.Type = Type;
        }

    }
}
