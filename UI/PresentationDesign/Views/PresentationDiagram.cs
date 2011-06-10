using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Classes.View;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Diagram;
using UI.PresentationDesign.DesignUI.Views;
using Syncfusion.Windows.Forms.Diagram.Controls;
using System.Reflection;
using System.Drawing;
using System.Drawing.Drawing2D;
using Syncfusion.Windows.Forms.Tools.Win32API;
using UI.PresentationDesign.DesignUI.Helpers;
using UI.PresentationDesign.DesignUI.Controls.SourceTree;
using UI.PresentationDesign.DesignUI.Controllers;
using System.ComponentModel;

namespace UI.PresentationDesign.DesignUI.Controls
{
    public class PresentationDiagram : Syncfusion.Windows.Forms.Diagram.Controls.Diagram
    {
        FocusManager mgrFocus;

        /// <summary>
        /// True, если диаграмму можно редактировать
        /// </summary>
        public bool ReadOnly
        {
            get;
            set;
        }

        public bool AllowNodesDrop
        {
            get;
            set;
        }

        /// <summary>
        /// True, если разрешено масштабирование колесом мышки
        /// </summary>
        public bool EnableMouseWheelZoom
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AllowSelect
        {
            get;
            set;
        }

        public PresentationDiagram(System.ComponentModel.IContainer container)
            : base(container)
        {
            ReadOnly = false;
            AllowNodesDrop = true;
            EnableMouseWheelZoom = false;
            AllowSelect = true;
        }


        private void UpdateFocusManager()
        {
            if (mgrFocus == null)
            {
                //please, never do that!
                mgrFocus = new FocusManagerExt();
                mgrFocus.UpdateServiceReferences(this);

                BindingFlags bf = BindingFlags.Default;
                bf |= BindingFlags.NonPublic;
                bf |= BindingFlags.Instance;

                FieldInfo fi = typeof(Diagram).GetField("m_mgrFocus", bf);
                fi.SetValue(this, mgrFocus);
            }
        }

        protected override void OnDiagramInitialized(object sender, EventArgs evtArgs)
        {
            base.OnDiagramInitialized(sender, evtArgs);
            UpdateFocusManager();
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);

            e.Effect = DragDropEffects.None;
            if (ReadOnly)
                return;
            else
            {
                IDataObject data = e.Data;
                bool NodesDropping = false;
                if (data.GetDataPresent(typeof(NodeCollection)))
                {
                    NodesDropping = true;
                }
                else if (data.GetDataPresent(typeof(DragDropData)))
                {
                    DragDropData dropdata = (DragDropData)data.GetData(typeof(DragDropData));
                    if (data != null)
                    {
                        if ((dropdata.Nodes != null) && (dropdata.Nodes.Count > 0))
                        {
                            NodesDropping = true;
                        }
                    }
                }

                if (NodesDropping && AllowNodesDrop)
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                    e.Effect = DragDropEffects.None;
            }

        }


        protected override void OnDragDrop(DragEventArgs e)
        {
            if (!ReadOnly)
            {
                IDataObject obj2 = e.Data;
                Point pt = new Point(e.X, e.Y);
                pt = this.PointToClient(pt);

                PointF tf = new PointF((float)pt.X, (float)pt.Y);
                if (obj2.GetDataPresent(typeof(DragDropData)) && (this.Model != null))
                {
                    DragDropData data = (DragDropData)obj2.GetData(typeof(DragDropData));
                    IUnitIndependent independent = data.Nodes[0];
                    tf = this.Controller.ConvertToModelCoordinates(tf);
                    Matrix scaleTransformation = this.Model.DocumentScale.GetScaleTransformation(this.Model.MeasurementUnits);
                    scaleTransformation.Invert();
                    tf = Geometry.AppendMatrix(tf, scaleTransformation);
                    if (this.Controller as LayoutController != null)
                    {
                        CreateWindowStatus createStatus;
                        if (((LayoutController)this.Controller).CreateWindow(data.Nodes[0], tf, out createStatus))
                        {
                            this.Model.AppendChild(data.Nodes[0]);
                            //ControllerCheckSelection();
                        }
                        else
                        {
                            ISourceNode src = ((ISourceNode)data.Nodes[0]);
                            String s = String.Empty;

                            switch(createStatus)
                            {
                                case CreateWindowStatus.InvalidDisplay:
                                    s = String.Format("Источник типа \"{0}\" недопустим для дисплея \"{1}\"", src.SourceType.Name, PresentationController.Instance.CurrentSlideLayout.Display.Type.Name);
                                break;
                                case CreateWindowStatus.InvalidResource:
                                    s = String.Format("{0} - некорректный источник", src.SourceType.Name);
                                break;
                                case CreateWindowStatus.NotSupportMultiWindow:
                                    s = "Дисплей не поддерживает несколько окон";
                                break;
                            }

                            if(!String.IsNullOrEmpty(s))
                                MessageBoxExt.Show(s, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            base.Focus();
            //base.OnDragDrop(e);
        }

        public override Syncfusion.Windows.Forms.Diagram.View CreateView()
        {
            return new PresentationView(this);
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.KeyValue >= 0x10) && (e.KeyValue <= 0x12))
            {
                base.OnKeyDown(e);
            }
            else
            {
                if (!AllowSelect) return;

                SlideGraphController controller = this.Controller as SlideGraphController;
                if (controller != null)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Up:
                        case Keys.Down:
                        case Keys.Right:
                        case Keys.Left:
                            e.Handled = true;
                            break;

                        case Keys.Space:
                            {
                                controller.ActivateTool(ToolDescriptor.PanTool);
                                e.Handled = true;
                                break;
                            }
                        case Keys.Delete:
                            {
                                e.Handled = true;
                                break;
                            }
                    }
                }

                Controller.OnKeyDown(e);

                if (!e.Handled)
                    base.OnKeyDown(e);
            }
        }

        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            if (!AllowSelect) return;

            SlideGraphController controller = this.Controller as SlideGraphController;

            if (controller != null)
            {
                switch (e.KeyCode)
                {
                    case Keys.Space:
                        {
                            controller.ActivateTool(ToolDescriptor.SelectTool);
                            e.Handled = true;
                            break;
                        }
                    case Keys.Z:
                        {
                            if (e.Control)
                            {
                                if (!ReadOnly)
                                    controller.Undo();
                                e.Handled = true;
                            }
                            break;
                        }
                    case Keys.Y:
                        {
                            if (e.Control)
                            {
                                if (!ReadOnly)
                                    controller.Redo();
                                e.Handled = true;
                            }
                            break;
                        }
                    case Keys.Delete:
                        {
                            if (!ReadOnly)
                                controller.RemoveSelected();
                            e.Handled = true;
                            break;
                        }
                }
            }

            Controller.OnKeyUp(e);

            if (!e.Handled)
                base.OnKeyUp(e);
        }

        bool CtrlHold
        {
            get
            {
                return (WindowsAPI.GetAsyncKeyState(VirtualKeys.VK_CONTROL) & 0x8000) != 0;
            }
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (AllowSelect)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (!CtrlHold)
                    {
                        if (!ReadOnly | this.Controller.SelectionList.Count == 0 | this.Controller.ActiveTool.Name == ToolDescriptor.PanTool | this.Controller.ActiveTool.Name == ToolDescriptor.SelectTool)
                            base.OnMouseMove(e);
                    }
                    else
                    {
                        if(Controller.ActiveTool.Name.Contains("Move"))
                            Controller.ActiveTool.Abort();
                    }
                }
                else
                {
                    base.OnMouseMove(e);
                }
            }
            else
                this.Cursor = Cursors.No;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (AllowSelect)
            {
                base.OnMouseDown(e);
            }
            else
                Focus();
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            if (AllowSelect)
            {
                base.OnMouseUp(e);
                ControllerCheckSelection();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            //Focus();
        }

        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            if (EnableMouseWheelZoom && CtrlHold)
            {
                float scale = this.View.Magnification;
                float delta = 3f * Math.Sign(e.Delta);//e.Delta / 10f;
                if (scale + delta >= 0 && scale + delta <= 500)
                    this.View.Magnification = scale + delta;
            }
            else
                base.OnMouseWheel(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            if (this.Parent is SlideDiagramControl)
            {
                (this.Parent as SlideDiagramControl).SlideDoubleClick();
                return;
            }

            base.OnDoubleClick(e);
        }

        private void ControllerCheckSelection()
        {
            ISelectionController c = (this.Controller as ISelectionController);

            if (c != null)
            {
                if ((WindowsAPI.GetAsyncKeyState(VirtualKeys.VK_CONTROL) & 0x8000) != 0 || (WindowsAPI.GetAsyncKeyState(VirtualKeys.VK_SHIFT) & 0x8000) != 0) 
                {
                    c.CheckSelection();
                }
                else
                {
                    c.CheckHitSelection();
                }
            }

        }

    }
}
