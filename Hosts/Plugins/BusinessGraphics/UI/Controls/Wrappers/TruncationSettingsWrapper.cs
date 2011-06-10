using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotnetCHARTING.WinForms;
using System.Drawing;
using System.Drawing.Drawing2D;
using dotnetCHARTING.WinForms;


namespace Hosts.Plugins.BusinessGraphics.UI.Controls.Wrappers
{
    /// <summary>
    /// Обертка Truncation для сохранения свойств.
    /// </summary>
    [Serializable]
    public class TruncationSettingsWrapper
    {
        public TruncationSettingsWrapper() { }
        public TruncationSettingsWrapper(Truncation truncation) 
        {
            Length = truncation.Length;
            Mode = truncation.Mode;
            Text = truncation.Text;
        }
        public int Length;
        public TruncationMode Mode;
        public string Text;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(Truncation truncation)
        {
            truncation.Length = Length;
            truncation.Mode = Mode;
            truncation.Text = Text;
        }

    }

}
