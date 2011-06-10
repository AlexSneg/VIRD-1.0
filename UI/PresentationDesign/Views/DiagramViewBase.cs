using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Syncfusion.Windows.Forms.Diagram;
using System.Drawing.Drawing2D;

namespace UI.PresentationDesign.DesignUI.Classes.View
{
    public delegate void MagnificationChanged(ViewMagnificationEventArgs e);

    public class DiagramViewBase : Syncfusion.Windows.Forms.Diagram.View
    {
        public event MagnificationChanged OnViewMagnifincationChanged;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool NeedDrawHandles
        {
            get;
            set;
        }

        protected override void OnMagnificationChanged(ViewMagnificationEventArgs evtArgs)
        {
            base.OnMagnificationChanged(evtArgs);
            if (OnViewMagnifincationChanged != null)
                OnViewMagnifincationChanged(evtArgs);
        }

        private Matrix GetParentsTransform(Node node)
        {
            if (node is PseudoGroup)
            {
                return new Matrix();
            }
            if ((node == null) || (node.Parent == null))
            {
                throw new ArgumentNullException("node && parent are null!");
            }
            Matrix matrix = new Matrix();
            for (ICompositeNode node2 = node.Parent; !(node2 is Syncfusion.Windows.Forms.Diagram.Model); node2 = ((Node)node2).Parent)
            {
                Matrix transformations = ((Node)node2).GetTransformations();
                ((Node)node2).AppendFlipTransforms(transformations);
                matrix.Multiply(transformations, MatrixOrder.Append);
            }
            return matrix;
        }

        protected override void DrawHandles(System.Drawing.Graphics grfx, Syncfusion.Windows.Forms.Diagram.NodeCollection nodesSelected)
        {
            if (NeedDrawHandles)
            {
                if ((nodesSelected != null) && (nodesSelected.Count > 0))
                {
                    foreach (Node node in nodesSelected)
                    {
                        PathNode node2;
                        if (!node.Visible)
                        {
                            continue;
                        }
                        HandleEditMode defaultHandleEditMode = node.EditStyle.DefaultHandleEditMode;
                        Matrix parentsTransform = this.GetParentsTransform(node);
                        GraphicsState gstate = grfx.Save();
                        grfx.MultiplyTransform(parentsTransform);
                        if (this.CustomHandleRenderer != null)
                        {
                            this.CustomHandleRenderer.Render(grfx, defaultHandleEditMode, node);
                        }
                        else
                        {
                            switch (defaultHandleEditMode)
                            {
                                case HandleEditMode.Resize:
                                    if (!node.ShowResizeHandles())
                                    {
                                        HandleRenderer.OutlineBoundingRectangle(grfx, node);
                                        if (node is IEndPointContainer)
                                        {
                                            HandleRenderer.DrawEndPoints(grfx, node);
                                        }
                                    }
                                    else
                                    {
                                        HandleRenderer.OutlineBoundingRectangle(grfx, node);
                                        //HandleRenderer.DrawRotationHandles(grfx, node);
                                        HandleRenderer.DrawSelectionHandles(grfx, node);
                                    }
                                    break;

                                case HandleEditMode.Vertex:
                                    node2 = node as PathNode;
                                    if (!(node is IEndPointContainer))
                                    {
                                        if ((node2 != null) && (node2.CanChangePath || node2.IsVertexEditable))
                                        {
                                            HandleRenderer.OutlineBoundingRectangle(grfx, node2);
                                            if (node2.CanDrawControlPoints())
                                            {
                                                node2.DrawControlPoints(grfx);
                                            }
                                            else
                                            {
                                                HandleRenderer.DrawVertexHandles(grfx, node2);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        HandleRenderer.DrawEndPoints(grfx, node);
                                        HandleRenderer.OutlineBoundingRectangle(grfx, node);
                                        if (node2.CanDrawControlPoints())
                                        {
                                            node2.DrawControlPoints(grfx);
                                        }
                                    }
                                    break;
                            }
                        }

                        grfx.Restore(gstate);
                    }
                }
            }
        }
    }
}
