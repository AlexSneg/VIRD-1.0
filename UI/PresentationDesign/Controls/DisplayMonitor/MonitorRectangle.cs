using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Syncfusion.Windows.Forms.Diagram;
using UI.PresentationDesign.DesignUI.Controllers.Interfaces;
using System.Drawing.Drawing2D;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools.Controls.StatusBar;
using Syncfusion.Windows.Forms.Diagram.Controls;
using System.Windows.Forms;

namespace UI.PresentationDesign.DesignUI.Controls.DisplayMonitor
{
    /// <summary>
    /// Окошко с отбражением скриншота одного дисплея в плеере
    /// </summary>
    public class MonitorRectangle : Syncfusion.Windows.Forms.Diagram.Rectangle
    {
        private IDisplayViewer m_Viewer;
        public const int HEADER_HEIGHT = 20;
        private const int MARGINS = 5;
        private Pen rectForSourcePen;

        public Diagram Diagram { get; set; }

        public MonitorRectangle(IDisplayViewer viewer)
            : base(viewer.Pos)
        {
            init(viewer);
        }

        public MonitorRectangle(IDisplayViewer viewer, MonitorRectangle src)
            : base(src)
        {
            init(viewer);
        }

        public MonitorRectangle(IDisplayViewer viewer, float x, float y, float width, float height)
            : base(x, y, width, height)
        {
            init(viewer);
        }

        public void MouseClick(float x, float y)
        {
            x -= MARGINS;
            y -= MARGINS + HEADER_HEIGHT;
            m_Viewer.NotifyUserClicked(x / (this.BoundingRectangle.Width - 2 * MARGINS), y / (this.BoundingRectangle.Height - 2 * MARGINS - HEADER_HEIGHT));
            m_Viewer_OnImageLoaded();
        }

        private void init(IDisplayViewer viewer)
        {
            rectForSourcePen = (Pen)Pens.Yellow.Clone();
            rectForSourcePen.Width = 2;

            EditStyle.AllowRotate = false;
            EditStyle.HideRotationHandle = true;
            EditStyle.HidePinPoint = true;
            this.EnableShading = true;
            this.EditStyle.AspectRatio = true;

            m_Viewer = viewer;

            m_Viewer.OnImageLoaded += new ImageChanged(m_Viewer_OnImageLoaded);
            m_Viewer.OnSourceSelected += new ImageChanged(m_Viewer_OnImageLoaded);
        }

        void m_Viewer_OnImageLoaded()
        {
            if (this.Diagram != null)
            {
                this.Diagram.Invalidate(new System.Drawing.Rectangle((int)BoundingRect.X, (int)BoundingRect.Y, (int)BoundingRect.Width, (int)BoundingRect.Height));
                if(this.Diagram.IsHandleCreated)
                    this.Diagram.Invoke(new MethodInvoker(this.Diagram.Refresh));
            }
        }

        public override object Clone()
        {
            return new MonitorRectangle(m_Viewer, this);
        }

        public String Title
        {
            get { return m_Viewer.Name; }
        }

        public IDisplayViewer DisplayViewer
        {
            get { return m_Viewer; }
        }

        public void SetPosition(System.Drawing.Rectangle rect)
        {
            this.Size = new SizeF(rect.Width, rect.Height);
            this.PinPointOffset = new SizeF(1, 1);
            this.PinPoint = new PointF(rect.Location.X + 1, rect.Location.Y + 1);
        }

        public void SavePosition()
        {
            m_Viewer.Pos = new System.Drawing.Rectangle((int)BoundingRectangle.X, (int)BoundingRectangle.Y, (int)BoundingRectangle.Width, (int)BoundingRectangle.Height);
        }

        protected override void Render(System.Drawing.Graphics gfx)
        {
            Office2007Colors colorTable = Office2007Colors.GetColorTable(Office2007Theme.Blue);
            Color color = colorTable.ActiveTitleGradientBegin;
            Color color2 = colorTable.ActiveTitleGradientEnd;
            int width = (int)this.BoundingRectangle.Width;
            int titleHeight = HEADER_HEIGHT;
            if (titleHeight > 0)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(new System.Drawing.Rectangle(0, 0, 1, titleHeight), color, color2, 90f))
                {
                    gfx.FillRectangle(brush, new System.Drawing.Rectangle(0, 0, (int)this.BoundingRectangle.Width, titleHeight));
                }
            }
            using (Brush brush2 = new SolidBrush(colorTable.ActiveFormBorderColor))
            {
                gfx.FillRectangle(brush2, new System.Drawing.Rectangle(0, titleHeight, (int)this.BoundingRectangle.Width, (int)(this.BoundingRectangle.Height - titleHeight)));
            }
            using (Font font = new Font(FontFamily.GenericSansSerif, (float)(titleHeight * 0.5)))
            {
                gfx.DrawString(this.DisplayViewer.Name, font, new SolidBrush(colorTable.TabItemTextColor), new RectangleF(0, 0, this.BoundingRectangle.Width, titleHeight));
            }
            Image screen = null; 
            if (!m_Viewer.HasLayout)
            {
                using (Font font = new Font(FontFamily.GenericSansSerif, 7))
                {
                    if (this.BoundingRectangle.Height > HEADER_HEIGHT + 2 * MARGINS && this.BoundingRectangle.Width > 2 * MARGINS)
                        gfx.DrawString("Для дисплея не задана раскладка текущей сцены!", font, Brushes.Red,
                            new RectangleF(MARGINS, HEADER_HEIGHT + MARGINS, this.BoundingRectangle.Width - 2 * MARGINS, this.BoundingRectangle.Height - 2 * MARGINS - HEADER_HEIGHT));
                }
            }
            else if ((screen = m_Viewer.getSceenshot()) != null)
            {
                if (this.BoundingRectangle.Height > HEADER_HEIGHT + 2 * MARGINS && this.BoundingRectangle.Width > 2 * MARGINS)
                    gfx.DrawImage(m_Viewer.getSceenshot(), MARGINS, HEADER_HEIGHT + MARGINS, this.BoundingRectangle.Width - 2 * MARGINS, this.BoundingRectangle.Height - 2 * MARGINS - HEADER_HEIGHT);
            }
            else if (m_Viewer.HasImage)
            {
                using (Font font = new Font(FontFamily.GenericSansSerif, 7))
                {
                    if (this.BoundingRectangle.Height > HEADER_HEIGHT + 2 * MARGINS && this.BoundingRectangle.Width > 2 * MARGINS)
                        gfx.DrawString("Изображение загружается...", font, Brushes.Black,
                            new RectangleF(MARGINS, HEADER_HEIGHT + MARGINS, this.BoundingRectangle.Width - 2 * MARGINS, this.BoundingRectangle.Height - 2 * MARGINS - HEADER_HEIGHT));
                }
            }
            else
            {
                using (Font font = new Font(FontFamily.GenericSansSerif, 7))
                {
                    if (this.BoundingRectangle.Height > HEADER_HEIGHT + 2 * MARGINS && this.BoundingRectangle.Width > 2 * MARGINS)
                        gfx.DrawString("Нет связи с агентом!", font, Brushes.Red,
                            new RectangleF(MARGINS, HEADER_HEIGHT + MARGINS, this.BoundingRectangle.Width - 2 * MARGINS, this.BoundingRectangle.Height - 2 * MARGINS - HEADER_HEIGHT));
                }
            }

            if (m_Viewer.IsPresentationShow && m_Viewer.SelectedSource.HasValue)
            {
                RectangleF source = m_Viewer.SelectedSource.Value;
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                    (int)(MARGINS + source.Left * (this.BoundingRectangle.Width - 2 * MARGINS)),
                    (int)(HEADER_HEIGHT + MARGINS + source.Top * (this.BoundingRectangle.Height - 2 * MARGINS - HEADER_HEIGHT)),
                    (int)(source.Width * (this.BoundingRectangle.Width - 2 * MARGINS)),
                    (int)(source.Height * (this.BoundingRectangle.Height - 2 * MARGINS - HEADER_HEIGHT)));
                gfx.DrawRectangle(rectForSourcePen, rect);
            }

            gfx.DrawRectangle(Pens.Black, 0, 0, (int)this.BoundingRect.Width, (int)this.BoundingRect.Height);
        }
    }
}
