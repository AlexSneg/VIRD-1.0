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
    /// Обертка Background для сохранения свойств.
    /// </summary>
    [Serializable]
    public class BackgroundSettingsWrapper
    {
        public BackgroundSettingsWrapper() { }
        public BackgroundSettingsWrapper(Background background) 
        {
            Bevel = background.Bevel;
            Color = new ColorSettingsWrapper(background.Color);
            GlassEffect = background.GlassEffect;
            GradientAngle = background.GradientAngle;
            HatchColor = background.HatchColor;
            HatchStyle = background.HatchStyle;
            ImagePath = background.ImagePath;
            Mode = background.Mode;
            SecondaryColor = background.SecondaryColor;
            ShadingEffectMode = background.ShadingEffectMode;
            Transparency = background.Transparency;
            Visible = background.Visible;
        }
        public bool Bevel;
        public ColorSettingsWrapper Color;
        public bool GlassEffect;
        public float GradientAngle;
        public Color HatchColor;
        public HatchStyle HatchStyle;
        public string ImagePath;
        public BackgroundMode Mode;
        public Color SecondaryColor;
        public ShadingEffectMode ShadingEffectMode;
        public int Transparency;
        public bool Visible;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(Background background)
        {
            background.Bevel = Bevel;
            background.Color = Color.RestoreSettings();
            background.GlassEffect = GlassEffect;
            background.GradientAngle = GradientAngle;
            background.HatchColor = HatchColor;
            background.HatchStyle = HatchStyle;
            background.ImagePath = ImagePath;
            background.Mode = Mode;
            background.SecondaryColor = SecondaryColor;
            background.ShadingEffectMode = ShadingEffectMode;
            background.Transparency = Transparency;
            background.Visible = Visible;
        }
    }
}
