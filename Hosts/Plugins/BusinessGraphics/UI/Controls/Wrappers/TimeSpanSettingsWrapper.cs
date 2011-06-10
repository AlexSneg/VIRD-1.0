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
    /// Обертка TimeSpan для сохранения свойств.
    /// </summary>
    [Serializable]
    public class TimeSpanSettingsWrapper
    {
        public TimeSpanSettingsWrapper() { }
        public TimeSpanSettingsWrapper(TimeSpan span) 
        {
            Ticks = span.Ticks;
        }
        public long Ticks;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(TimeSpan span)
        {
            throw new ArgumentException("Невозможно восстановить сохраненные настройки этого класса. Используйте явное пересоздание", "span");
        }

    }
}
