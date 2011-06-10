using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UI.PresentationDesign.DesignUI.Controllers;
using UI.PresentationDesign.DesignUI.Controllers.Interfaces;
using Node = Syncfusion.Windows.Forms.Diagram.Node;

namespace UI.PresentationDesign.DesignUI.Controls.DisplayMonitor
{
    /// <summary>
    /// Контрол для мониторинга дисплеев
    /// </summary>
    public partial class DisplayMonitorControl : UserControl
    {
        private MonitoringController m_Controller;
        private const int CASCADE_SPACING = 20;

        public DisplayMonitorControl()
        {            
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.diagram1.AttachModel(this.model1);
                this.diagram1.Model.EventSink.NodeCollectionChanged += new Syncfusion.Windows.Forms.Diagram.CollectionExEventHandler(EventSink_NodeCollectionChanged);
                this.diagram1.MouseDown += new MouseEventHandler(diagram1_MouseDown);
                this.diagram1.MouseClick += new MouseEventHandler(diagram1_MouseClick);
            }
        }

        public void SavePositions()
        {
            for (int i = 0; i < this.model1.ChildCount; i++)
                (this.model1.GetChild(i) as MonitorRectangle).SavePosition();
        }

        void diagram1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.diagram1.Controller.SelectionList.Count > 0)
            {
                MonitorRectangle rect = (this.diagram1.Controller.SelectionList[0] as MonitorRectangle);
                rect.MouseClick(e.X - rect.BoundingRectangle.Left, e.Y - rect.BoundingRectangle.Top);
                m_Controller.SelectDisplayView(rect.DisplayViewer);
            }
        }

        void diagram1_MouseDown(object sender, MouseEventArgs e)
        {
            //for (int i = 0; i < this.diagram1.Controller.SelectionList.Count; i++)
            this.diagram1.Controller.BringToFront();
        }

        void EventSink_NodeCollectionChanged(Syncfusion.Windows.Forms.Diagram.CollectionExEventArgs evtArgs)
        {
            if (evtArgs.ChangeType == Syncfusion.Windows.Forms.Diagram.CollectionExChangeType.Remove)
            {
                m_Controller.removeViewer((evtArgs.Element as MonitorRectangle).DisplayViewer);
            }
            else if (evtArgs.ChangeType == Syncfusion.Windows.Forms.Diagram.CollectionExChangeType.Clear)
            {
                foreach (var elem in evtArgs.Elements)
                    m_Controller.removeViewer((elem as MonitorRectangle).DisplayViewer);
            }
        }

        public void AssingController(MonitoringController AController)
        {
            m_Controller = AController;

            m_Controller.OnViewerAdded += new ViewerChanged(m_Controller_OnViewerAdded);
            m_Controller.OnViewerRemoved += new ViewerChanged(m_Controller_OnViewerRemoved);
            m_Controller.OnCollectionChanged += new ViewerCollectionChanged(m_Controller_OnCollectionChanged);
            m_Controller.OnViewerSelected += m_Controller_OnSelectedDisplayChanged;
        }

        void m_Controller_OnCollectionChanged()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(m_Controller_OnCollectionChanged));
            else
                this.Refresh();
        }

        void m_Controller_OnViewerRemoved(IDisplayViewer viewer)
        {
            Node child = (from Node node in this.model1.Nodes where (node as MonitorRectangle).DisplayViewer.Name == viewer.Name select node).FirstOrDefault();
            if (child != null)
                this.model1.RemoveChild(child);
            this.Refresh();
        }

        private void m_Controller_OnSelectedDisplayChanged(IDisplayViewer viewer)
        {
            this.diagram1.Controller.SelectionList.Clear();
            Node child = (from Node node in this.model1.Nodes where (node as MonitorRectangle).DisplayViewer.Name == viewer.Name select node).FirstOrDefault();
            this.diagram1.Controller.SelectionList.Add(child);
        }

        private float getZoom()
        {
            return 0.9f * this.Width / m_Controller.MaxWidth;
        }

        private float getZoom(int width)
        {
            return width / m_Controller.MaxWidth;
        }

        void transformViewer(IDisplayViewer viewer)
        {
            transformViewer(viewer, getZoom());
        }

        void transformViewer(IDisplayViewer viewer, float zoom)
        {
            if (!viewer.IsTransformed)
            {
                viewer.Pos = new Rectangle(viewer.Pos.Location, new Size((int)(viewer.Pos.Width * zoom), (int)(viewer.Pos.Height * zoom)));
            }
            else
                viewer.IsTransformed = false;
        }

        void m_Controller_OnViewerAdded(IDisplayViewer viewer)
        {
            transformViewer(viewer);
            this.model1.AppendChild(new MonitorRectangle(viewer) { Diagram = this.diagram1 });
            this.Refresh();
            m_Controller.SelectDisplayView(viewer);
        }

        const int VIEW_MARGINS = 2;

        public void ArrangeCascade()
        {
            if (this.diagram1.Model.Nodes.Count == 0)
                return;
            Rectangle rect = new Rectangle(
                VIEW_MARGINS,
                VIEW_MARGINS,
                this.diagram1.Bounds.Width - CASCADE_SPACING * (this.diagram1.Model.Nodes.Count - 1) - 2 * VIEW_MARGINS,
                this.diagram1.Bounds.Height - CASCADE_SPACING * (this.diagram1.Model.Nodes.Count - 1) - 2 * VIEW_MARGINS);

            float ratio = this.getZoom(rect.Width);

            this.diagram1.BeginUpdate();
            for (int i = 0; i < this.diagram1.Model.Nodes.Count; i++)
            {
                IDisplayViewer viewer = (this.diagram1.Model.Nodes[i] as MonitorRectangle).DisplayViewer;
                Size s = m_Controller.getDefaultSize(viewer);
                viewer.Pos = new Rectangle(VIEW_MARGINS + CASCADE_SPACING * i, VIEW_MARGINS + CASCADE_SPACING * i, s.Width, s.Height);
                transformViewer(viewer, ratio);
                (this.diagram1.Model.Nodes[i] as MonitorRectangle).SetPosition(viewer.Pos);
            }
            this.diagram1.EndUpdate();
        }       

        public void ArrangeDefault()
        {
            this.diagram1.BeginUpdate();
            for (int i = 0; i < this.diagram1.Model.Nodes.Count; i++)
            {
                RectangleF rect = (this.diagram1.Model.Nodes[i] as MonitorRectangle).BoundingRectangle;
                IDisplayViewer viewer = (this.diagram1.Model.Nodes[i] as MonitorRectangle).DisplayViewer;
                viewer.Pos = new Rectangle(new Point((int)rect.Location.X, (int)rect.Location.Y), m_Controller.getDefaultSize(viewer));
                transformViewer(viewer);
                (this.diagram1.Model.Nodes[i] as MonitorRectangle).SetPosition(viewer.Pos);
            }
            this.diagram1.EndUpdate();
        }

        private void ArrangeBlocks(int num)
        {
            List<int> counts = new List<int>();
            for (int i = 0; i < num*num; i++)
            {
                int cnt = diagram1.Model.Nodes.Count / (num*num);
                if (diagram1.Model.Nodes.Count % (num*num) > i)
                    cnt++;
                counts.Add(cnt);
            }
            this.diagram1.BeginUpdate();
            for (int i = 0; i < num*num; i++)
            {
                Rectangle partRect = new Rectangle(
                    (this.diagram1.Bounds.Width / num) * (i % num) + VIEW_MARGINS,
                    (this.diagram1.Bounds.Height / num) * (i / num) + VIEW_MARGINS,
                    this.diagram1.Bounds.Width / num - num * VIEW_MARGINS,
                    this.diagram1.Bounds.Height / num - num * VIEW_MARGINS);

                for (int j = 0; j < counts[i]; j++)
                {
                    MonitorRectangle r = model1.Nodes[j * num*num + i] as MonitorRectangle;
                    r.DisplayViewer.Pos = new Rectangle(
                        new Point(),
                        m_Controller.getDefaultSize(r.DisplayViewer));
                    float ratio = (r.DisplayViewer.Pos.Width / (float)r.DisplayViewer.Pos.Height > partRect.Width / (float)partRect.Height) ?
                        (partRect.Width - counts[i] * CASCADE_SPACING / num) / (float)r.DisplayViewer.Pos.Width :
                        (partRect.Height - counts[i] * CASCADE_SPACING / num) / (float)r.DisplayViewer.Pos.Height;
                    transformViewer(r.DisplayViewer, ratio);
                    r.DisplayViewer.Pos = new Rectangle(
                        new Point(partRect.Left + j * CASCADE_SPACING / num, partRect.Top + j * CASCADE_SPACING / num),
                        r.DisplayViewer.Pos.Size);
                    r.SetPosition(r.DisplayViewer.Pos);
                }
            }
            this.diagram1.EndUpdate();
        }

        public void Arrange2x2()
        {
            ArrangeBlocks(2);
        }

        public void Arrange3x3()
        {
            ArrangeBlocks(3);
        }

        public void Arrange4x4()
        {
            ArrangeBlocks(4);
        }

        public void CloseAllWindows()
        {
            this.diagram1.Model.RemoveAllChildren();
        }

        private void diagram1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DisplayList.DisplayNode)))
            {
                DisplayList.DisplayNode node = (e.Data.GetData(typeof(DisplayList.DisplayNode)) as DisplayList.DisplayNode);
                if (m_Controller.canAddViewer(node))
                {
                    e.Effect = DragDropEffects.Move;
                    return;
                }
            }
            else if (e.Data.GetDataPresent(typeof(DisplayList.DisplayGroupNode)))
            {
                DisplayList.DisplayGroupNode node = e.Data.GetData(typeof(DisplayList.DisplayGroupNode)) as DisplayList.DisplayGroupNode;
                if (m_Controller.canAddGroup(node))
                {
                    e.Effect = DragDropEffects.Move;
                    return;
                }
            }
            e.Effect = DragDropEffects.None;
        }

        private void diagram1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DisplayList.DisplayNode)))
            {
                DisplayList.DisplayNode node = (e.Data.GetData(typeof(DisplayList.DisplayNode)) as DisplayList.DisplayNode);
                IDisplayViewer viewer = m_Controller.addViewer(node);
                if (viewer != null)
                {
                    transformViewer(viewer);
                    this.model1.AppendChild(new MonitorRectangle(viewer) { Diagram = this.diagram1 });
                }
            }
            else if (e.Data.GetDataPresent(typeof(DisplayList.DisplayGroupNode)))
            {
                DisplayList.DisplayGroupNode node = e.Data.GetData(typeof(DisplayList.DisplayGroupNode)) as DisplayList.DisplayGroupNode;
                List<IDisplayViewer> viewers = m_Controller.addGroup(node);
                foreach (IDisplayViewer viewer in viewers)
                {
                    transformViewer(viewer);
                    this.model1.AppendChild(new MonitorRectangle(viewer) { Diagram = this.diagram1 });
                }
            }
        }
    }
}
