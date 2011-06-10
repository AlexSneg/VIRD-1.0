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
    /// Обертка Axis для сохранения свойств.
    /// </summary>
    [Serializable]
    public class AxisSettingsWrapper
    {
        public AxisSettingsWrapper() { }
        public AxisSettingsWrapper(Axis axis)
        {
            AlternateGridBackground = new BackgroundSettingsWrapper(axis.AlternateGridBackground);
            CenterTickMarks = axis.CenterTickMarks;
            ClearValues = axis.ClearValues;
            ClusterColumns = axis.ClusterColumns;
            CombinedElementsCalculation = axis.CombinedElementsCalculation;
            CultureName = axis.CultureName;
            DefaultTick = new AxisTickSettingsWrapper(axis.DefaultTick);
            ExtraTicks = new AxisTickSettingsWrapper[axis.ExtraTicks.Count];
            for (int i = 0; i < ExtraTicks.Length; i++)
            {
                ExtraTicks[i] = new AxisTickSettingsWrapper(axis.ExtraTicks[i]);
            }

            FormatString = axis.FormatString;
            GaugeLabelMode = axis.GaugeLabelMode;
            GaugeNeedleType = axis.GaugeNeedleType;
            GenerateElementTicks = axis.GenerateElementTicks;
            Interval = axis.Interval;
            InvertScale = axis.InvertScale;
            Label = new LabelSettingsWrapper(axis.Label);
            LabelRotate = axis.LabelRotate;
            Line = new LineSettingsWrapper(axis.Line);
            LogarithmicBase = axis.LogarithmicBase;
            Maximum = axis.Maximum;//
            Minimum = axis.Minimum;
            MinimumInterval = axis.MinimumInterval;
            MinorInterval = axis.MinorInterval;
            MinorTicksPerInterval = axis.MinorTicksPerInterval;
            MinorTimeIntervalAdvanced = new TimeIntervalAdvancedSettingsWrapper(axis.MinorTimeIntervalAdvanced);
            Name = axis.Name;
            NumberPercision = axis.NumberPercision;
            NumberPrecision = axis.NumberPrecision;
            Orientation = axis.Orientation;
            OrientationAngle = axis.OrientationAngle;
            OverlappingCircularLabelSeparator = axis.OverlappingCircularLabelSeparator;
            Percent = axis.Percent;
            Position = axis.Position;
            RadarMode = axis.RadarMode;
            RangeAngle = axis.RangeAngle;
            ReverseSeries = axis.ReverseSeries;
            ReverseSeriesPositions = axis.ReverseSeriesPositions;
            ReverseStack = axis.ReverseStack;
            Scale = axis.Scale;
            ScaleBreaks = new ScaleRangeSettingsWrapper[axis.ScaleBreaks.Count];
            for (int i = 0; i < ScaleBreaks.Length; i++)
            {
                ScaleBreaks[i] = new ScaleRangeSettingsWrapper(axis.ScaleBreaks[i]);
            }
            ScaleBreakStyle = axis.ScaleBreakStyle;
            ScaleRange = new ScaleRangeSettingsWrapper(axis.ScaleRange);
            ShowGrid = axis.ShowGrid;
            SmartMinorTicks = axis.SmartMinorTicks;
            SmartScaleBreak = axis.SmartScaleBreak;
            SmartScaleBreakLimit = axis.SmartScaleBreakLimit;
            SpacingPercentage = axis.SpacingPercentage;
            StaticColumnWidth = axis.StaticColumnWidth;
            SweepAngle = axis.SweepAngle;
            TickLabelAngle = axis.TickLabelAngle;
            TickLabelMode = axis.TickLabelMode;
            TickLabelPadding = axis.TickLabelPadding;
            TickLabelSeparatorLine = new LineSettingsWrapper(axis.TickLabelSeparatorLine);
            TickLine = new LineSettingsWrapper(axis.TickLine);
            TickNumberMaximum = axis.TickNumberMaximum;
            TimeInterval = (int)axis.TimeInterval;
            TimeIntervalAdvanced = new TimeIntervalAdvancedSettingsWrapper(axis.TimeIntervalAdvanced);
            TimePadding = axis.TimePadding;
            TimeScaleLabels = new TimeScaleLabelInfoSettingsWrapper(axis.TimeScaleLabels);
            ZeroLine = new LineSettingsWrapper(axis.ZeroLine);
            ZeroTick = new AxisTickSettingsWrapper(axis.ZeroTick);
        }
        public BackgroundSettingsWrapper AlternateGridBackground;
        public bool CenterTickMarks;
        public bool ClearValues;
        public bool ClusterColumns;
        public Calculation CombinedElementsCalculation;
        public string CultureName;
        public AxisTickSettingsWrapper DefaultTick;
        public AxisTickSettingsWrapper[] ExtraTicks;
        public string FormatString;
        public GaugeLabelMode GaugeLabelMode;
        public GaugeNeedleType GaugeNeedleType;
        public bool GenerateElementTicks;
        public double Interval;
        public bool InvertScale;
        public LabelSettingsWrapper Label;
        public bool LabelRotate;
        public LineSettingsWrapper Line;
        public double LogarithmicBase;
        public object Maximum;//
        public object Minimum;
        public double MinimumInterval;
        public double MinorInterval;
        public int MinorTicksPerInterval;
        public TimeIntervalAdvancedSettingsWrapper MinorTimeIntervalAdvanced;
        public string Name;
        public int NumberPercision;
        public int NumberPrecision;
        public Orientation Orientation;
        public int OrientationAngle;
        public string OverlappingCircularLabelSeparator;
        public bool Percent;
        public int Position;
        public RadarMode RadarMode;
        public int RangeAngle;
        public bool ReverseSeries;
        public bool ReverseSeriesPositions;
        public bool ReverseStack;
        public Scale Scale;
        public ScaleRangeSettingsWrapper[] ScaleBreaks;
        public ScaleBreakStyle ScaleBreakStyle;
        public ScaleRangeSettingsWrapper ScaleRange;
        public bool ShowGrid;
        public bool SmartMinorTicks;
        public bool SmartScaleBreak;
        public int SmartScaleBreakLimit;
        public float SpacingPercentage;
        public float StaticColumnWidth;
        public int SweepAngle;
        public float TickLabelAngle;
        public TickLabelMode TickLabelMode;
        public int TickLabelPadding;
        public LineSettingsWrapper TickLabelSeparatorLine;
        public LineSettingsWrapper TickLine;
        public int TickNumberMaximum;
        public int TimeInterval;
        public TimeIntervalAdvancedSettingsWrapper TimeIntervalAdvanced;
        public TimeSpan TimePadding;
        public TimeScaleLabelInfoSettingsWrapper TimeScaleLabels;
        public LineSettingsWrapper ZeroLine;
        public AxisTickSettingsWrapper ZeroTick;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(Axis axis)
        {
            AlternateGridBackground.RestoreSettings(axis.AlternateGridBackground);
            axis.CenterTickMarks = CenterTickMarks;
            axis.ClearValues = ClearValues;
            axis.ClusterColumns = ClusterColumns;
            axis.CombinedElementsCalculation = CombinedElementsCalculation;
            axis.CultureName = CultureName;
            DefaultTick.RestoreSettings(axis.DefaultTick);
            
            axis.ExtraTicks.Clear();
            for (int i = 0; i < ExtraTicks.Length; i++)
            {
                AxisTick tick =new AxisTick();
                ExtraTicks[i].RestoreSettings(tick);
                axis.ExtraTicks.Add(tick);
            }
            axis.FormatString = FormatString;
            axis.GaugeLabelMode = GaugeLabelMode;
            axis.GaugeNeedleType = GaugeNeedleType;
            axis.GenerateElementTicks = GenerateElementTicks;
            axis.Interval = Interval;
            axis.InvertScale = InvertScale;
            Label.RestoreSettings(axis.Label);
            axis.LabelRotate = LabelRotate;
            Line.RestoreSettings(axis.Line);
            axis.LogarithmicBase = LogarithmicBase;
            Maximum = axis.Maximum;
            axis.Minimum = Minimum;
            axis.MinimumInterval = MinimumInterval;
            axis.MinorInterval = MinorInterval;
            axis.MinorTicksPerInterval = MinorTicksPerInterval;
            MinorTimeIntervalAdvanced.RestoreSettings(axis.MinorTimeIntervalAdvanced);
            axis.Name = Name;
            axis.NumberPercision = NumberPercision;
            axis.NumberPrecision = NumberPrecision;
            axis.Orientation = Orientation;
            axis.OrientationAngle = OrientationAngle;
            axis.OverlappingCircularLabelSeparator = OverlappingCircularLabelSeparator;
            axis.Percent = Percent;
            axis.Position = Position;
            axis.RadarMode = RadarMode;
            axis.RangeAngle = RangeAngle;
            axis.ReverseSeries = ReverseSeries;
            axis.ReverseSeriesPositions = ReverseSeriesPositions;
            axis.ReverseStack = ReverseStack;
            axis.Scale = Scale;

            axis.ScaleBreaks.Clear();
            for (int i = 0; i < ScaleBreaks.Length; i++)
            {
                ScaleRange range=new ScaleRange();
                ScaleBreaks[i].RestoreSettings(range);
                axis.ScaleBreaks.Add(range);
            }

            axis.ScaleBreakStyle = ScaleBreakStyle;
            ScaleRange.RestoreSettings(axis.ScaleRange);
            axis.ShowGrid = ShowGrid;
            axis.SmartMinorTicks = SmartMinorTicks;
            axis.SmartScaleBreak = SmartScaleBreak;
            axis.SmartScaleBreakLimit = SmartScaleBreakLimit;
            axis.SpacingPercentage = SpacingPercentage;
            axis.StaticColumnWidth = StaticColumnWidth;
            axis.SweepAngle = SweepAngle;
            axis.TickLabelAngle = TickLabelAngle;
            axis.TickLabelMode = TickLabelMode;
            axis.TickLabelPadding = TickLabelPadding;
            TickLabelSeparatorLine.RestoreSettings(axis.TickLabelSeparatorLine);
            TickLine.RestoreSettings(axis.TickLine);
            axis.TickNumberMaximum = TickNumberMaximum;
            axis.TimeInterval = (TimeInterval)TimeInterval;
            TimeIntervalAdvanced.RestoreSettings(axis.TimeIntervalAdvanced);
            axis.TimePadding = TimePadding;
            TimeScaleLabels.RestoreSettings(axis.TimeScaleLabels);
            ZeroLine.RestoreSettings(axis.ZeroLine);
            ZeroTick.RestoreSettings(axis.ZeroTick);   

        }
    }

}
