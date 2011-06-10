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
    /// Обертка TimeScaleLabelInfo для сохранения свойств.
    /// </summary>
    [Serializable]
    public class TimeScaleLabelInfoSettingsWrapper
    {
        public TimeScaleLabelInfoSettingsWrapper() { }
        public TimeScaleLabelInfoSettingsWrapper(Axis.TimeScaleLabelInfo info) 
        {
            DayFormatString = info.DayFormatString;
            HourFormatString = info.HourFormatString;
            MaximumRangeRows = info.MaximumRangeRows;
            MillisecondFormatString = info.MillisecondFormatString;
            MinuteFormatString = info.MinuteFormatString;
            Mode = info.Mode;
            MonthFormatString = info.MonthFormatString;
            PercentEdgeOverlapToRemoveTicks = info.PercentEdgeOverlapToRemoveTicks;
            QuarterFormatString = info.QuarterFormatString;
            RangeIntervals = new int[info.RangeIntervals.Count];
            for (int i = 0; i < RangeIntervals.Length; i++)
            {
                RangeIntervals[i] = (int)info.RangeIntervals[i];
            }

            RangeMode = info.RangeMode;
            SecondFormatString = info.SecondFormatString;
            WeekFormatString = info.WeekFormatString;
            YearFormatString = info.YearFormatString;
        }

        public string DayFormatString;
        public string HourFormatString;
        public int MaximumRangeRows;
        public string MillisecondFormatString;
        public string MinuteFormatString;
        public TimeScaleLabelMode Mode;
        public string MonthFormatString;
        public int PercentEdgeOverlapToRemoveTicks;
        public string QuarterFormatString;
        public int[] RangeIntervals;
        public TimeScaleLabelRangeMode RangeMode;
        public string SecondFormatString;
        public string WeekFormatString;
        public string YearFormatString;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(Axis.TimeScaleLabelInfo info)
        {
            info.DayFormatString = DayFormatString;
            info.HourFormatString = HourFormatString;
            info.MaximumRangeRows = MaximumRangeRows;
            info.MillisecondFormatString = MillisecondFormatString;
            info.MinuteFormatString = MinuteFormatString;
            info.Mode = Mode;
            info.MonthFormatString = MonthFormatString;
            info.PercentEdgeOverlapToRemoveTicks = PercentEdgeOverlapToRemoveTicks;
            info.QuarterFormatString = QuarterFormatString;

            info.RangeIntervals.Clear();
            for (int i = 0; i < RangeIntervals.Length; i++)
            {
                info.RangeIntervals.Add((TimeInterval)RangeIntervals[i]);
            }

            info.RangeMode = RangeMode;
            info.SecondFormatString = SecondFormatString;
            info.WeekFormatString = WeekFormatString;
            info.YearFormatString = YearFormatString;
        }

    }
}
