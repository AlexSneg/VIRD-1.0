using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Diagram;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;
using Syncfusion.Windows.Forms.Tools;
using UI.PresentationDesign.DesignUI.Controls;

namespace UI.PresentationDesign.DesignUI.Classes.View
{
    public class PresentationView : DiagramViewBase
    {
        PresentationView m_ConnectedView;
        PresentationDiagram m_Diagram;

        public PresentationView(PresentationDiagram ADiagram)
        {
            NeedDrawHandles = true;
            HandleColor = Color.LightGreen;
            HandleRenderer.HandleDisabledColor = Color.LightGreen;
            m_Diagram = ADiagram;
        }

        public void ConnectToView(PresentationView ConnectedView)
        {
            if (m_ConnectedView == null)
                (m_ConnectedView = ConnectedView).ConnectToView(this);
        }

        protected override void OnOriginChanged(ViewOriginEventArgs evtArgs)
        {
            if (evtArgs.NewOrigin.X < -m_Diagram.Width/2)
            {
                this.Origin = new PointF(-m_Diagram.Width/2, evtArgs.NewOrigin.Y);
                return;
            }

            if (evtArgs.NewOrigin.Y < -m_Diagram.Height/2)
            {
                this.Origin = new PointF(evtArgs.NewOrigin.X, -m_Diagram.Height/2);
                return;
            }

            if (evtArgs.NewOrigin.X + m_Diagram.Width > m_document.LogicalSize.Width)
            {
                this.Origin = new PointF(m_document.LogicalSize.Width - m_Diagram.Width, evtArgs.NewOrigin.Y);
                return;
            }

            if (evtArgs.NewOrigin.Y + m_Diagram.Height > m_document.LogicalSize.Height)
            {
                this.Origin = new PointF(evtArgs.NewOrigin.X, m_document.LogicalSize.Height - m_Diagram.Height);
                return;
            }

            base.OnOriginChanged(evtArgs);

            if (m_ConnectedView != null)
            {
                if (m_ConnectedView.Origin != evtArgs.NewOrigin)
                    m_ConnectedView.Origin = evtArgs.NewOrigin;

                m_ConnectedView.Repaint();
            }

        }

        private void Repaint()
        {
            m_Diagram.Refresh();
        }

    }
}
