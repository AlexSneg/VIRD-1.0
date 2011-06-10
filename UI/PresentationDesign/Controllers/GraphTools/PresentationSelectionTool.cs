using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Diagram;
using System.Drawing;
using UI.PresentationDesign.DesignUI.Classes.View;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools.Win32API;
using UI.PresentationDesign.DesignUI.Controls.SourceTree;

namespace UI.PresentationDesign.DesignUI.Classes.Controller
{
    public delegate void ShowSelectionContextMenu(Point p); 
    public delegate void ToolStateChanged();

    public class PresentationSelectionTool : SelectTool
    {
        public bool AllowSelect
        {
            get;
            set;
        }

        public bool AllowMultiSelect
        {
            get;
            set;
        }

        public event ShowSelectionContextMenu OnShowSlideContextMenu;
        public event ShowSelectionContextMenu OnShowModelContextMenu;

        static Dictionary<DiagramController, PresentationSelectionTool> singletons = new Dictionary<DiagramController, PresentationSelectionTool>();

        public PresentationSelectionTool(DiagramController controller)
            : base(controller)
        {
            singletons.Add(controller, this);
            this.SelectMode = SelectMode.Containing;
            AllowMultiSelect = true;
        }

        protected override bool CanActivateEndPointMoveTools(PointF ptCurrent)
        {
            IHandle handleHit = null;
            Node p = HandlesHitTesting.GetEndPointAtPoint(base.Controller.SelectionList, ptCurrent, ref handleHit);
            if (p is SlideLink)
            {
                this.ToolToActivate = SingleActionTools.None;
                base.ToolCursor = Cursors.Default;
                return false;
            }

            return base.CanActivateEndPointMoveTools(ptCurrent);
        }

        protected override bool CanActivateMoveTool(PointF ptMouseLocation)
        {
            NodeCollection nodes = base.Controller.GetAllNodesAtPoint(base.Controller.Model, ptMouseLocation, true);
            if (nodes.Count > 0)
            {
                if (nodes.First is SlideLink)
                {
                    this.ToolToActivate = SingleActionTools.MoveTool;
                    base.CurrentCursor = Cursors.Hand;
                    return true;
                }
            }

            bool result = base.CanActivateMoveTool(ptMouseLocation);
            return result;
        }

        protected override Tool PerformMove(MouseEventArgs evtArgs)
        {
            NodeCollection nodes = base.Controller.GetAllNodesAtPoint(base.Controller.Model, base.Controller.MouseLocation, true);
            if (nodes.Count > 0)
                if (nodes.First is SlideLink)
                {
                    base.Controller.SelectionList.Clear();
                    base.Controller.SelectionList.Add(nodes.First);
                    return this;
                }

            return base.PerformMove(evtArgs);
        }

        public override Tool ProcessMouseUp(System.Windows.Forms.MouseEventArgs evtArgs)
        {
            Tool tool = base.ProcessMouseUp(evtArgs);
            if (evtArgs.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Node selected = (Node)Controller.GetNodeUnderMouse(evtArgs.Location);
                if (selected != null)
                {
                    if (!Controller.SelectionList.Contains(selected))
                    {
                        Controller.SelectionList.Clear();
                        Controller.SelectionList.Add(selected);
                    }

                    OnShowSlideContextMenu.Fire(evtArgs.Location);
                }
                else
                {
                    OnShowModelContextMenu.Fire(evtArgs.Location);
                }
            }
            else
            {
                if (evtArgs.Button == MouseButtons.Left)
                {
                    Type t = Controller is LayoutController ? typeof(SourceWindow) : typeof(SlideView);

                    List<Node> nodes = (from  n in Controller.SelectionList.OfType<Node>() where n.GetType().Equals(t) select n).ToList();

                    if (nodes.Count > 0 )
                    {
                        if (AllowMultiSelect == false)
                        {
                            Node lastNode = nodes.Last();
                            Controller.SelectionList.Clear();
                            Controller.SelectionList.Add(lastNode);
                        }
                    }
                }
            }

            return tool;
        }

        public override Tool ProcessMouseDown(MouseEventArgs evtArgs)
        {
            if (((WindowsAPI.GetAsyncKeyState(VirtualKeys.VK_SHIFT) & 0x8000) != 0 || (WindowsAPI.GetAsyncKeyState(VirtualKeys.VK_CONTROL) & 0x8000) != 0) && evtArgs.Button == MouseButtons.Left)
            {
                Node selected = (Node)Controller.GetNodeUnderMouse(evtArgs.Location);
                if (selected != null)
                {
                    if (!Controller.SelectionList.Contains(selected) && AllowMultiSelect)
                        Controller.SelectionList.Add(selected);
                    else
                        Controller.SelectionList.Remove(selected);

                    Controller.UpdateInfo.UpdateRefreshRect(this.WorkRect);

                    if (Controller is ISelectionController)
                        ((ISelectionController)Controller).CheckSelection();
                }

                return this;
            }

            return base.ProcessMouseDown(evtArgs);;
        }


        public override Tool ProcessKeyUp(System.Windows.Forms.KeyEventArgs evtArgs)
        {
            Tool tool = base.ProcessKeyUp(evtArgs);

            if (evtArgs.KeyCode == System.Windows.Forms.Keys.Apps)
            {
                if (Controller.SelectionList.Count > 0)
                {
                    Node node = Controller.SelectionList.First;
                    PointF pf = Controller.ConvertFromModelToClientCoordinates(node.ConvertToModelCoordinates(node.CentralPort.GetPosition()));
                    Point p = new Point((int)pf.X, (int)pf.Y);
                    OnShowSlideContextMenu.Fire(p);
                }
            }

            return tool;
        }

        internal static Tool GetInstance(DiagramController controller)
        {
            if (singletons.ContainsKey(controller))
                return singletons[controller];
            else
                return null;
        }
    }

    public static class EventExtension
    {
        public static void Fire(this ShowSelectionContextMenu e, Point p)
        {
            if (e != null)
            {
                if (e.GetInvocationList() != null)
                    e.Invoke(p);
            }
        }
    }
}
