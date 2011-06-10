using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Hosts.Plugins.VDCServer.SystemModule.Config;

namespace Hosts.Plugins.VDCServer.SystemModule.Design.TypeEditors
{
    /// <summary>
    /// Отрисовка названия раскладки и картинки.
    /// </summary>
    class ActiveLayoutUITypeEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value == null) return;
            Bitmap image = ((ScreenLayout)e.Value).LayoutPicture;
            if (image == null) return;

            Rectangle bounds = e.Bounds;

            Rectangle targetRectangle = CalculateTargetBounds(image, bounds);

            image.MakeTransparent();
            e.Graphics.DrawImage(image, /*destRect*/targetRectangle);
        }

        /// <summary>
        /// Вычислить границы области для отрисовки.
        /// </summary>
        private static Rectangle CalculateTargetBounds(Bitmap image, Rectangle bounds)
        {
            double kh = image.Height / (float)bounds.Height;
            double kw = image.Width / (float)bounds.Width;
            double k = kh > kw ? kh : kw;
            Rectangle targetRectangle = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            targetRectangle.Height = (int)(image.Height / k);
            targetRectangle.Width = (int)(image.Width / k);
            targetRectangle.X = bounds.X + (int)Math.Abs(bounds.Width - targetRectangle.Width) / 2;
            targetRectangle.Y = bounds.Y + (int)Math.Abs(bounds.Height - targetRectangle.Height) / 2;
            return targetRectangle;
        }

    }
}
