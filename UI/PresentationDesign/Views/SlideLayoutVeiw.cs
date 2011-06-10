using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using TechnicalServices.Interfaces;
using System.ComponentModel;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.CommonPersistence.Presentation;

namespace UI.PresentationDesign.DesignUI.Classes.View
{
    public class SlideLayoutVeiw : DiagramViewBase, IBackgroundSupport
    {
        LayoutController m_controller;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image BackgroundImage
        {
            get;
            set;
        }

        public SlideLayoutVeiw()
        {
            this.NeedDrawHandles = true;
            Magnification = 100f;
            Origin = new PointF(0, 0);
        }

        protected override void DrawDocumentBackground(Graphics gfx, RectangleF rectDocument)
        {
            base.DrawDocumentBackground(gfx, rectDocument);

            if (BackgroundImage != null)
            {
                gfx.DrawImage(BackgroundImage, rectDocument);
            }

            this.DrawSegments(gfx, rectDocument);
        }


        void DrawSegments(Graphics grfx, RectangleF rectClip)
        {
            int penWidth = 3;

            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                //рисование границ кубов
                if (PresentationController.Instance != null && PresentationController.Instance.CurrentSlideLayout != null)
                {
                    ISegmentationSupport disp = PresentationController.Instance.CurrentSlideLayout.Display as ISegmentationSupport;
                    if (disp != null)
                    {
                        float x = 0;
                        float y = 0;
                        List<RectangleF> rects = new List<RectangleF>();
                        for (int i = 0; i < disp.SegmentColumns; ++i)
                        {
                            for (int j = 0; j < disp.SegmentRows; j++)
                            {
                                RectangleF rect = new RectangleF(x, y, disp.SegmentWidth, disp.SegmentHeight);

                                if (rect.IntersectsWith(rectClip))
                                    rects.Add(rect);

                                y += disp.SegmentHeight + penWidth + 1;
                            }

                            x += disp.SegmentWidth + penWidth + 1;
                            y = 0;
                        }

                        if (rects.Count > 0)
                        {
                            Pen p = new Pen(new SolidBrush(Color.FromArgb(180, Color.Indigo)), penWidth);
                            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                            grfx.DrawRectangles(p, rects.ToArray());
                        }
                    }
                }
            }
        }
    }
}
