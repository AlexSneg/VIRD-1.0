using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Diagram;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Reflection;
using System.Drawing;

namespace UI.PresentationDesign.DesignUI.Classes.View
{
    public class PresentationLabel : Label
    {
        PropertyInfo BindedProperty;
        object Target;

        public bool IsMultiline
        {
            get;
            set;
        }

        public bool IsVisible
        {
            get;
            set;
        }

        public Color ForeColor
        {
            get;
            set;
        }

        bool _useCentering = true;

        public PresentationLabel(Node container, object target, String propertyName, bool UseCentering)
            : this(container, target, propertyName)
        {
            _useCentering = UseCentering;
            IsVisible = true;
        }

        public PresentationLabel(Node container, object target, String propertyName)
            : base(container, String.Empty)
        {
            IsVisible = true;
            ForeColor = Color.Black;
            Target = target;
            BindedProperty = target.GetType().GetProperty(propertyName);
            if (BindedProperty == null)
                throw new ApplicationException("No such propery " + propertyName);
        }

        public void UpdateReference(object target)
        {
            Target = target;
        }

        protected override void Render(System.Drawing.Graphics gfx)
        {
            if (!IsVisible) return;

            object data = BindedProperty.GetValue(Target, null);

            Brush br = null;
            string text = String.Empty;

            if (data != null)
            {
                text = data.ToString();
                br = new SolidBrush(ForeColor);
            }
            else
            {
                text = "--";
                br = new SolidBrush(Color.DarkGray);
            }

            PointF position = base.GetPosition();
            Font font = new Font(FontStyle.Family, FontStyle.Size, FontStyle.Style);
            SizeF mySize = new SizeF(base.Container.BoundingRectangle.Size.Width - 5, base.Container.BoundingRectangle.Size.Height - position.Y - 5);
            StringFormat sf = new StringFormat();
            if (_useCentering)
            {
                sf.LineAlignment = StringAlignment.Near;
                sf.Alignment = StringAlignment.Center;
            }
            else
            {
                position = new PointF(0, 0);
                mySize = base.Container.BoundingRectangle.Size;
                sf.LineAlignment = StringAlignment.Near;
                sf.Alignment = StringAlignment.Near;
            }

            sf.Trimming = StringTrimming.EllipsisCharacter;

            if (!IsMultiline)
                sf.FormatFlags |= StringFormatFlags.NoWrap;

            gfx.DrawString(text, font, br, new RectangleF(position, mySize), sf);
        }
    }
}
