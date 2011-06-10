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
    /// Обертка Font для сохранения свойств.
    /// </summary>
    [Serializable]
    public class FontSettingsWrapper
    {
        public FontSettingsWrapper() { }
        public FontSettingsWrapper(Font font)
        {
            FamilyName = font.FontFamily.Name;
            GdiCharSet = font.GdiCharSet;
            GdiVerticalFont = font.GdiVerticalFont;
            Size = font.Size;
            Style = font.Style;
            Unit = font.Unit;
        }
        public string FamilyName;
        public byte GdiCharSet;
        public bool GdiVerticalFont;
        public float Size;
        public FontStyle Style;
        public GraphicsUnit Unit;

        /// <summary>
        /// Считать шрифт из сохраненных настроек.
        /// </summary>
        public Font RestoreSettings()
        {
            return new Font(FamilyName, Size, Style, Unit, GdiCharSet, GdiVerticalFont);
        }
    }
}
