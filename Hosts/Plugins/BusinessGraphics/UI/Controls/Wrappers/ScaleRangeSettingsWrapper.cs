using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotnetCHARTING.WinForms;

namespace Hosts.Plugins.BusinessGraphics.UI.Controls.Wrappers
{
    /// <summary>
    /// Обертка ScaleRange для сохранения свойств.
    /// </summary>
    [Serializable]
    public class ScaleRangeSettingsWrapper
    {
        public ScaleRangeSettingsWrapper() { }
        public ScaleRangeSettingsWrapper(ScaleRange range) 
        {
            IncludeInAxisScale = range.IncludeInAxisScale;
        }
        public bool IncludeInAxisScale;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(ScaleRange range)
        {
            range.IncludeInAxisScale = IncludeInAxisScale;
        }

    }
}
