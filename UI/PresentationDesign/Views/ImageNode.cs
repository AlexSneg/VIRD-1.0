using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Diagram;
using System.Drawing;

namespace UI.PresentationDesign.DesignUI.Views
{
    public class ImageNode : Syncfusion.Windows.Forms.Diagram.Rectangle
    {
        Image _image;

        public ImageNode(ImageNode dest)
            : base(dest)
        {
            this.EnableShading = true;
        }


        public ImageNode(Image img)
            : base(0, 0, img.Width, img.Height, MeasureUnits.Pixel)
        {
            _image = img;
            this.EditStyle.HidePinPoint = true;
            this.EditStyle.HideRotationHandle = true;
            this.EditStyle.Enabled = false;
        }

        public override object Clone()
        {
            ImageNode node = new ImageNode(this);
            node._image = this._image;
            return node;
        }

        protected override void Render(System.Drawing.Graphics gfx)
        {
            base.Render(gfx);
            gfx.DrawImage(_image, this.BoundingRect);
        }
    }
}
