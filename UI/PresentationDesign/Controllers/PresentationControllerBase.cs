using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Diagram;
using Syncfusion.Windows.Forms.Tools;
using UI.PresentationDesign.DesignUI.Classes.View;
using System.Drawing;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.PresentationDesign.DesignUI.Classes.Helpers;

namespace UI.PresentationDesign.DesignUI.Classes.Controller
{

    public class SlideEventArgs : EventArgs
    {
        public Slide Slide
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Simple abstract PresentationController with Hint functionality
    /// </summary>
    public abstract class PresentationDiagramControllerBase : DiagramController
    {
        SuperToolTip tip;
        Node tooltipNode = null;

        public event EventHandler<ToolEventArgs> ToolActivated;
        public event EventHandler<ToolEventArgs> ToolDeactivated;
        public event EventHandler<SlideEventArgs> OnSlideHover;

        public bool ShowHints
        {
            get;
            set;
        }

        protected override void OnToolActivated(ToolEventArgs evtArgs)
        {
            base.OnToolActivated(evtArgs);
            if (ToolActivated != null)
                ToolActivated(this, evtArgs);
        }

        protected override void OnToolDeactivated(ToolEventArgs evtArgs)
        {
            base.OnToolDeactivated(evtArgs);
            if (ToolDeactivated != null)
                ToolDeactivated(this, evtArgs);
        }

        public PresentationDiagramControllerBase()
        {
            tip = new SuperToolTip();
            tip.MaxWidth = 350;
            ShowHints = true;
        }

        public override void OnMouseMove(System.Windows.Forms.MouseEventArgs evtArgs)
        {
            base.OnMouseMove(evtArgs);

            if (evtArgs.Button == System.Windows.Forms.MouseButtons.None)
            {
                Node n = this.GetNodeAtPoint(ConvertToModelCoordinates(evtArgs.Location));
                if (n != null)
                {
                    if (n is SlideView)
                    {
                        if (OnSlideHover != null)
                            OnSlideHover(this, new SlideEventArgs { Slide = ((SlideView)n).Slide });

                        if (ShowHints)
                        {
                            if (tooltipNode != n)
                            {
                                tooltipNode = n;
                                Slide slide = ((SlideView)n).Slide;
                                ToolTipInfo info = new ToolTipInfo();
                                info.Header.Text = slide.Name;
                                info.Header.Font = new Font(info.Header.Font, System.Drawing.FontStyle.Bold);
                                info.Body.Text = slide.Comment;

                                LockingInfo li = PresentationController.Instance.GetSlideLockingInfo(slide);
                                if (li != null)
                                    info.Footer.Text = PresentationStatusInfo.GetSlideLockingInfoDescr(li);

                                PointF pf = ConvertFromModelToClientCoordinates((new PointF(n.BoundingRectangle.Right, n.BoundingRectangle.Bottom)));
                                Point screen = new Point((int)pf.X, (int)pf.Y);

                                tip.Show(info, this.ParentControl.PointToScreen(screen), 2500);
                            }
                        }

                        return;
                    }
                }
                else
                {
                    if (ShowHints)
                        tip.Hide();

                    tooltipNode = null;
                }

                if (OnSlideHover != null)
                    OnSlideHover(this, new SlideEventArgs { Slide = null });
            }
        }
    }
}
