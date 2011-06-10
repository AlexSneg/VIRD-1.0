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
    /// Обертка TimeIntervalAdvanced для сохранения свойств.
    /// </summary>
    [Serializable]
    public class TimeIntervalAdvancedSettingsWrapper
    {
        public TimeIntervalAdvancedSettingsWrapper() { }
        public TimeIntervalAdvancedSettingsWrapper(TimeIntervalAdvanced interval)
        {
            Multiplier = interval.Multiplier;
            Start = interval.Start;
            StartDayOfWeek = interval.StartDayOfWeek;
            StartMonth = interval.StartMonth;
            TimeSpan = new TimeSpanSettingsWrapper(interval.TimeSpan);
        }
        public int Multiplier;
        public DateTime Start;
        public int StartDayOfWeek;
        public int StartMonth;
        public TimeSpanSettingsWrapper TimeSpan;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(TimeIntervalAdvanced interval)
        {
            interval.Multiplier = Multiplier;
            interval.Start = Start;
            interval.StartDayOfWeek = StartDayOfWeek;
            interval.StartMonth = StartMonth;
            interval.TimeSpan = new TimeSpan(TimeSpan.Ticks);
        }

    }
}
