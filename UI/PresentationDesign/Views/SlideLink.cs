using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Presentation = TechnicalServices.Persistence.SystemPersistence.Presentation;
using Syncfusion.Windows.Forms.Diagram;
using System.Drawing;
using System.Windows.Forms;

namespace UI.PresentationDesign.DesignUI.Classes.View
{

    public class SlideLink : LineConnector
    {
        Presentation.Link m_link;

        public Presentation.Link Link
        {
            get { return m_link; }
            set { m_link = value; Refresh(); }
        }

        public bool DefaultPath
        {
            get;
            set;
        }

        bool m_default = false;
        public bool IsDefault
        {
            get { return m_default; }
            set { m_default = value; Refresh(); }
        }

        public SlideView FromSlideView
        {
            get
            {
                return FromNode as SlideView;
            }
        }

        public SlideView ToSlideView
        {
            get
            {
                return ToNode as SlideView;
            }
        }

        public SlideLink(SlideLink source)
            : base(source)
        {
        }

        public SlideLink(PointF from, PointF to) :
            base(from, to)
        {
            DefaultPath = false;

            HeadDecorator.DecoratorShape = DecoratorShape.Filled45Arrow;
            HeadDecorator.Size = new SizeF(7, 5);
            HeadDecorator.FillStyle.Color = Color.White;
            HeadDecorator.LineStyle.LineColor = Color.Black;
            HeadDecorator.LineStyle.LineWidth = 1;

            LineStyle.LineColor = Color.LightBlue;
            LineStyle.LineWidth = 1;

            EditStyle.AllowMoveX = false;
            EditStyle.AllowMoveY = false;
            EditStyle.AllowSelect = true;
            EditStyle.AllowRotate = false;
            EditStyle.HidePinPoint = true;

        }

        public override bool CanEditSegment()
        {
            return false;
        }

        /// <summary>
        /// Обновляет информацию о ссылке, основываясь на мэппинге
        /// </summary>
        public void Refresh()
        {
            if (m_default)
            {
                LineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                if (!DefaultPath)
                    LineStyle.LineColor = Color.LightBlue;
                else
                    LineStyle.LineColor = Color.Green;

                LineStyle.LineWidth = 2;
            }
            else
            {
                LineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                LineStyle.LineColor = Color.Gray;
                LineStyle.DashOffset = 10f;
                LineStyle.LineWidth = 2;
            }
        }

        public override object Clone()
        {
            return new SlideLink(this) { IsDefault = m_default };
        }
    }
}
