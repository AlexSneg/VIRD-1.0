using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Diagram;
using System.Diagnostics;
using UI.PresentationDesign.DesignUI.Classes.View;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.PresentationDesign.DesignUI.Classes.Model;
using UI.PresentationDesign.DesignUI.Classes.History;
using System.Drawing.Drawing2D;
using UI.PresentationDesign.DesignUI.Controllers;

namespace UI.PresentationDesign.DesignUI.Classes.Controller
{
    /// <summary>
    /// Инструмент связывания сцен
    /// </summary>
    public class SlideLinkTool : SlideLinkerToolBase
    {
        public event ToolDeactivate OnToolDeactivate;

        SlideLink _createdLink;

        public SlideLinkTool(DiagramController controller)
            : base(controller, ToolDescriptor.SlideLinkTool)
        {
            base.ToolCursor = base.ActionCursor = Cursors.Cross;
            this.SingleActionTool = false;
        }

        protected override Node CreateNode(PointF ptStart, PointF ptEnd)
        {
            SlideLink link = new SlideLink(ptStart, ptEnd);
            _createdLink = link;
            return link;
        }

        public override void ActivateTool()
        {
            base.ActivateTool();
        }

        protected override bool CanConnect()
        {
            if (this.HeadPossibleConnection != null && this.TailPossibleConnection != null)
            {
                SlideView from = this.HeadPossibleConnection.Container as SlideView;
                SlideView to = this.TailPossibleConnection.Container as SlideView;

                if (from != null && to != null && from != to)
                {
                    return from.CanLinkTo(to);
                }
            }

            return false;
        }

        protected override void CompleteLinking()
        {
            if (_createdLink != null)
            {
                SlideView node1 = _createdLink.FromSlideView;
                SlideView node2 = _createdLink.ToSlideView;

                //check node links and free space
                if ((node1 != null && node2 != null))
                {
                    _createdLink.IsDefault = node1.GetOutgoingLinks().Count == 1;
                    ((SlideGraphController)Controller).RefreshDefaultSlidePath(false);
                }

                _createdLink = null;
            }

        }

        /// <summary>
        /// Событие деактивации инструмента, отменяет действие связи, если связь не соответствует заданным условиям
        /// </summary>
        public override void DeactivateTool()
        {
            base.DeactivateTool();

            if (OnToolDeactivate != null)
                OnToolDeactivate();
        }

    }

    public abstract class SlideLinkerToolBase : UITool
    {
        // Fields
        private HeadDecorator m_decorHead;
        private TailDecorator m_decorTail;
        private ConnectionPoint m_portHeadPossConn;
        private ConnectionPoint m_portTailPossConn;
        private System.Drawing.Rectangle m_rectHeadPC;
        private System.Drawing.Rectangle m_rectTailPC;

        // Methods
        public SlideLinkerToolBase(DiagramController controller, string name)
            : base(controller, name)
        {
        }

        private void CompleteAction(PointF ptStart, PointF ptEnd)
        {
            RectangleF ef = MeasureUnitsConverter.ToPixels(base.Controller.Model.Bounds, base.Controller.Model.MeasurementUnits);
            if (!base.Controller.Model.BoundaryConstraintsEnabled || (ef.Contains(ptStart) && ef.Contains(ptEnd)))
            {
                if (CanConnect())
                {
                    Node child = this.CreateNode(ptStart, ptEnd);

                    HistoryManager historyManager = base.Controller.Model.HistoryManager;
                    if (historyManager != null)
                    {
                        historyManager.StartAtomicAction(CommandDescr.CreateLinkDescr);
                    }

                    base.Controller.Model.AppendChild(child);
                    IEndPointContainer container = child as IEndPointContainer;
                    if (container != null)
                    {
                        if (this.HeadPossibleConnection != null && this.TailPossibleConnection != null)
                        {
                            this.HeadPossibleConnection.TryConnect(container.HeadEndPoint);
                            this.TailPossibleConnection.TryConnect(container.TailEndPoint);

                            CompleteLinking();
                        }
                    }


                    if (historyManager != null)
                    {
                        historyManager.EndAtomicAction();
                    }
                }

                this.Controller.UpdateInfo.UpdateRefreshRect(this.WorkRectPrev);

                this.TailPossibleConnection = null;
                this.HeadPossibleConnection = null;
            }
        }

        protected abstract Node CreateNode(PointF ptStart, PointF ptEnd);
        protected abstract void CompleteLinking();

        public override void Draw(Graphics gfx)
        {
            Node renderringHelper = this.RenderringHelper;
            if (base.InAction && (renderringHelper != null))
            {
                renderringHelper.Draw(gfx);
            }
            base.OutlineConnectionPoint(gfx, this.HeadPossibleConnection);
            base.OutlineConnectionPoint(gfx, this.TailPossibleConnection);
        }

        public override Tool ProcessMouseDown(MouseEventArgs evtArgs)
        {
            Tool tool = base.ProcessMouseDown(evtArgs);
            this.TailPossibleConnection = base.CheckConnectionPossibility((PointF)base.StartPoint);

            if (this.TailPossibleConnection != null)
            {
                this.CurrentCursor = Cursors.Cross;

                if (!(this.TailPossibleConnection.Container is SlideView))
                {
                    this.CurrentCursor = Cursors.No;
                }
            }
            else
                this.CurrentCursor = Cursors.No;

            base.UpdatePortRefreshRect(this.TailPossibleConnection, ref this.m_rectTailPC);
            return tool;
        }

        public override Tool ProcessMouseMove(MouseEventArgs evtArgs)
        {
            base.CurrentPoint = new Point(evtArgs.X, evtArgs.Y);
            base.CanRender = false;
            if (base.InAction)
            {
                PointF endPointLocation = base.GetEndPointLocation(this.TailPossibleConnection, (PointF)base.GetStartPoint(false));
                endPointLocation = base.Controller.ConvertFromModelToClientCoordinates(endPointLocation);
                PointF ptModelLocation = base.GetEndPointLocation(this.HeadPossibleConnection, (PointF)base.CurrentPoint);
                ptModelLocation = base.Controller.ConvertFromModelToClientCoordinates(ptModelLocation);
                System.Drawing.Rectangle rectangle = Geometry.ConvertRectangle(Geometry.CreateRect(endPointLocation, ptModelLocation));
                this.HeadPossibleConnection = base.CheckConnectionPossibility((PointF)base.CurrentPoint);

                if (this.HeadPossibleConnection != null)
                {
                    if (!(this.HeadPossibleConnection.Container is SlideView) || (!CanConnect() & this.HeadPossibleConnection != this.TailPossibleConnection))
                    {
                        this.HeadPossibleConnection = null;
                        this.CurrentCursor = Cursors.No;
                    }
                    else
                        this.CurrentCursor = Cursors.Cross;
                }
                else
                    this.CurrentCursor = Cursors.No;

                if (rectangle != base.WorkRectPrev)
                {
                    base.CanRender = true;

                    base.WorkRectPrev = base.WorkRect;
                    base.WorkRect = rectangle;

                    base.UpdatePortRefreshRect(this.HeadPossibleConnection, ref this.m_rectHeadPC);
                    base.UpdatePortRefreshRect(this.TailPossibleConnection, ref this.m_rectTailPC);
                }

                this.Controller.UpdateInfo.UpdateRefreshRect(WorkRectPrev);
            }
            return this;
        }

        protected abstract bool CanConnect();

        public override Tool ProcessMouseUp(MouseEventArgs evtArgs)
        {
            if (evtArgs.Button == MouseButtons.Right)
            {
                PresentationSelectionTool.GetInstance(Controller).ProcessMouseUp(evtArgs);
            }

            base.CanRender = false;
            base.CurrentPoint = new Point(evtArgs.X, evtArgs.Y);
            if (base.InAction)
            {
                base.InAction = false;
                PointF endPointLocation = base.GetEndPointLocation(this.TailPossibleConnection, (PointF)base.GetStartPoint(false));
                PointF ptEnd = base.GetEndPointLocation(this.HeadPossibleConnection, (PointF)base.CurrentPoint);
                if (endPointLocation != ptEnd)
                {
                    this.CompleteAction(endPointLocation, ptEnd);
                    if (!this.m_rectHeadPC.IsEmpty)
                    {
                        base.Controller.UpdateInfo.UpdateRefreshRect(this.m_rectHeadPC);
                        this.m_rectHeadPC = System.Drawing.Rectangle.Empty;
                    }
                    if (!this.m_rectTailPC.IsEmpty)
                    {
                        base.Controller.UpdateInfo.UpdateRefreshRect(this.m_rectTailPC);
                        this.m_rectTailPC = System.Drawing.Rectangle.Empty;
                    }

                    ((Control)this.Controller.Viewer).Invalidate();
                }
            }
            return base.ProcessMouseUp(evtArgs);
        }

        protected void SetDecorator(LineBase line)
        {
            Decorator headDecorator = this.HeadDecorator;
            Decorator decorNode = line.HeadDecorator;
            base.ApplyDecorator(headDecorator, decorNode);
            if (headDecorator != null)
            {
                decorNode.Container = line;
            }
            headDecorator = this.TailDecorator;
            decorNode = line.TailDecorator;
            base.ApplyDecorator(headDecorator, decorNode);
            if (headDecorator != null)
            {
                decorNode.Container = line;
            }
        }

        // Properties
        public HeadDecorator HeadDecorator
        {
            get
            {
                if (this.m_decorHead == null)
                {
                    this.m_decorHead = new HeadDecorator();
                }
                return this.m_decorHead;
            }
            set
            {
                this.m_decorHead = value;
            }
        }

        protected ConnectionPoint HeadPossibleConnection
        {
            get
            {
                return this.m_portHeadPossConn;
            }
            set
            {
                this.m_portHeadPossConn = value;
            }
        }

        protected Node RenderringHelper
        {
            get
            {
                Node node = null;
                PointF endPointLocation = base.GetEndPointLocation(this.TailPossibleConnection, (PointF)base.GetStartPoint(false));
                PointF ptEnd = base.GetEndPointLocation(this.HeadPossibleConnection, (PointF)base.CurrentPoint);
                if (endPointLocation != ptEnd)
                {
                    node = this.CreateNode(endPointLocation, ptEnd);
                    base.AlterStyle(node);
                }
                return node;
            }
        }

        public TailDecorator TailDecorator
        {
            get
            {
                if (this.m_decorTail == null)
                {
                    this.m_decorTail = new TailDecorator();
                }
                return this.m_decorTail;
            }
            set
            {
                this.m_decorTail = value;
            }
        }

        protected ConnectionPoint TailPossibleConnection
        {
            get
            {
                return this.m_portTailPossConn;
            }
            set
            {
                this.m_portTailPossConn = value;
            }
        }
    }

}
