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
    /// Обертка SmartLabel для сохранения свойств.
    /// </summary>
    [Serializable]
    public class SmartLabelSettingsWrapper
    {
        public SmartLabelSettingsWrapper() { }
        public SmartLabelSettingsWrapper(SmartLabel label)
        {
            Alignment = label.Alignment;
            AllowMarkerOverlap = label.AllowMarkerOverlap;
            AutoWrap = label.AutoWrap;
            Color = new ColorSettingsWrapper(label.Color);
            DistanceMaximum = label.DistanceMaximum;
            DynamicDisplay = label.DynamicDisplay;
            DynamicPosition = label.DynamicPosition;
            Font = new FontSettingsWrapper(label.Font);
            ForceVertical = label.ForceVertical;
            GlowColor = label.GlowColor;
            Hotspot = new HotspotSettingsWrapper(label.Hotspot);
            Line = new LineSettingsWrapper(label.Line);
            LineAlignment = label.LineAlignment;
            OutlineColor = label.OutlineColor;
            Padding = label.Padding;
            PieLabelMode = label.PieLabelMode;
            RadarLabelMode = label.RadarLabelMode;
            Shadow = new ShadowSettingsWrapper(label.Shadow);
            Text = label.Text;
            Truncation = new TruncationSettingsWrapper(label.Truncation);
            Type = label.Type;
        }

        public LabelAlignment Alignment;
        public bool AllowMarkerOverlap;
        public bool AutoWrap;
        public ColorSettingsWrapper Color;
        public int DistanceMaximum;
        public bool DynamicDisplay;
        public bool DynamicPosition;
        public FontSettingsWrapper Font;
        public bool ForceVertical;
        public Color GlowColor;
        public HotspotSettingsWrapper Hotspot;
        public LineSettingsWrapper Line;
        public StringAlignment LineAlignment;
        public Color OutlineColor;
        public int Padding;
        public PieLabelMode PieLabelMode;
        public RadarLabelMode RadarLabelMode;
        public ShadowSettingsWrapper Shadow;
        public string Text;
        public TruncationSettingsWrapper Truncation;
        public LabelType Type;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(SmartLabel label)
        {
            label.Alignment = Alignment;
            label.AllowMarkerOverlap = AllowMarkerOverlap;
            label.AutoWrap = AutoWrap;
            label.Color = Color.RestoreSettings();
            label.DistanceMaximum = DistanceMaximum;
            label.DynamicDisplay = DynamicDisplay;
            label.DynamicPosition = DynamicPosition;
            label.Font =  Font.RestoreSettings();
            label.ForceVertical = ForceVertical;
            label.GlowColor = GlowColor;
            Hotspot.RestoreSettings(label.Hotspot);
            Line.RestoreSettings(label.Line);
            label.LineAlignment = LineAlignment;
            label.OutlineColor = OutlineColor;
            label.Padding = Padding;
            label.PieLabelMode = PieLabelMode;
            label.RadarLabelMode = RadarLabelMode;
            Shadow.RestoreSettings(label.Shadow);
            label.Text = Text;
            Truncation.RestoreSettings(label.Truncation);
            label.Type = Type;
        }

    }
}
